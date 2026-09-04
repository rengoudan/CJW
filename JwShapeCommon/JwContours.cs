using System;
using System.Drawing;
using System.Drawing.Drawing2D;

public enum BeamEndPosition
{
    上左,
    上右,
    下左,
    下右,
    左上,
    右上,
    左下,
    右下
}

public class BeamEndContour
{
    // 输入
    public PointF BaseCenter { get; set; }
    public BeamEndPosition Position { get; set; }

    // 几何参数（绝对值）
    private const float DX = 68f;
    private const float DY = 56f;

    private const float RightOffset = 25f;
    private const float TopOffset = 35f;
    private const float BottomOffset = 40f;

    private const float TopLineLength = 47f;
    private const float BottomLineLength = 25f + 68f;

    private const float ArcRadius = 40f;
    public float HoleRadius { get; set; } = 20f;

    // 屏幕转换参数
    public float Scale { get; set; } = 1f;
    public PointF Offset { get; set; } = new PointF(0, 0);

    // 圆心
    public PointF C1 { get; private set; }
    public PointF C2 { get; private set; }
    public PointF C3 { get; private set; }

    // 类型属性
    public bool IsVertical { get; private set; }
    public bool IsHorizontal => !IsVertical;

    public bool IsTopType { get; private set; }
    public bool IsBottomType => !IsTopType;

    public bool IsLeftType { get; private set; }
    public bool IsRightType => !IsLeftType;

    // 轮廓关键点
    public PointF VerticalLineTop { get; private set; }
    public PointF VerticalLineBottom { get; private set; }

    public PointF TopLineStart { get; private set; }
    public PointF TopLineEnd { get; private set; }

    public PointF BottomLineStart { get; private set; }
    public PointF BottomLineEnd { get; private set; }

    public PointF SlopeLineStart { get; private set; }
    public PointF SlopeLineEnd { get; private set; }

    public PointF ArcStart { get; private set; }
    public PointF ArcEnd { get; private set; }

    // 边界
    public float MinX { get; private set; }
    public float MaxX { get; private set; }
    public float MinY { get; private set; }
    public float MaxY { get; private set; }

    // 主入口：构建工程坐标系路径
    public GraphicsPath BuildPath()
    {
        ClassifyType();
        ComputeCenters();
        ComputeContourCore();

        var path = new GraphicsPath();
        path.StartFigure();

        // 竖线
        path.AddLine(VerticalLineTop, VerticalLineBottom);

        // 上横线
        path.AddLine(TopLineStart, TopLineEnd);

        // 斜线
        path.AddLine(SlopeLineStart, SlopeLineEnd);

        // 弧线角度
        float startAngle = AngleFromPoints(C1, ArcStart);
        float endAngle = AngleFromPoints(C1, ArcEnd);
        float sweepAngle = endAngle - startAngle;

        // 自动修正弧线方向（必须朝外侧包裹）
        if (IsVertical)
        {
            if ((VerticalLineTop.X > C1.X && sweepAngle < 0) ||
                (VerticalLineTop.X < C1.X && sweepAngle > 0))
            {
                sweepAngle = -sweepAngle;
            }
        }
        else
        {
            if ((VerticalLineTop.Y > C1.Y && sweepAngle < 0) ||
                (VerticalLineTop.Y < C1.Y && sweepAngle > 0))
            {
                sweepAngle = -sweepAngle;
            }
        }

        path.AddArc(
            C1.X - ArcRadius, C1.Y - ArcRadius,
            ArcRadius * 2, ArcRadius * 2,
            startAngle,
            sweepAngle
        );

        // 下横线
        path.AddLine(BottomLineStart, BottomLineEnd);

        // 闭合
        path.AddLine(BottomLineEnd, VerticalLineBottom);
        path.CloseFigure();

        UpdateBounds();
        return path;
    }

    // 屏幕坐标转换
    private PointF ToScreen(PointF p)
    {
        return new PointF(
            p.X * Scale + Offset.X,
            -p.Y * Scale + Offset.Y
        );
    }

    // 绘制
    public void Draw(Graphics g, Pen pen)
    {
        using var geoPath = BuildPath();
        using var screenPath = (GraphicsPath)geoPath.Clone();

        using var m = new Matrix();
        m.Scale(Scale, -Scale);
        m.Translate(Offset.X, Offset.Y, MatrixOrder.Append);

        screenPath.Transform(m);

        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.DrawPath(pen, screenPath);
    }

    // 绘制轮廓 + 圆
    public void DrawWithCircles(Graphics g, Pen contourPen, Pen circlePen)
    {
        Draw(g, contourPen);
        DrawCircle(g, circlePen, C1);
        DrawCircle(g, circlePen, C2);
        DrawCircle(g, circlePen, C3);
    }

    private void DrawCircle(Graphics g, Pen pen, PointF center)
    {
        var c = ToScreen(center);

        var rect = new RectangleF(
            c.X - HoleRadius * Scale,
            c.Y - HoleRadius * Scale,
            HoleRadius * 2 * Scale,
            HoleRadius * 2 * Scale
        );

        g.DrawEllipse(pen, rect);
    }

    // 类型分类
    private void ClassifyType()
    {
        IsVertical = Position == BeamEndPosition.上左 ||
                     Position == BeamEndPosition.上右 ||
                     Position == BeamEndPosition.下左 ||
                     Position == BeamEndPosition.下右;

        IsTopType = Position == BeamEndPosition.上左 ||
                    Position == BeamEndPosition.上右 ||
                    Position == BeamEndPosition.左上 ||
                    Position == BeamEndPosition.右上;

        IsLeftType = Position == BeamEndPosition.上左 ||
                     Position == BeamEndPosition.下左 ||
                     Position == BeamEndPosition.左上 ||
                     Position == BeamEndPosition.左下;
    }

    // 计算三个圆心
    private void ComputeCenters()
    {
        C1 = BaseCenter;

        if (IsVertical)
        {
            float dx = IsLeftType ? DX : -DX;
            float dy = IsTopType ? DY : -DY;

            C2 = new PointF(C1.X + dx, C1.Y);
            C3 = new PointF(C2.X, C2.Y + dy);
        }
        else
        {
            float dyC2 = IsTopType ? -DX : DX;
            float dxC3 = IsRightType ? DY : -DY;

            C2 = new PointF(C1.X, C1.Y + dyC2);
            C3 = new PointF(C2.X + dxC3, C2.Y);
        }
    }

    // 核心轮廓构造
    private void ComputeContourCore()
    {
        float minX = Math.Min(C1.X, Math.Min(C2.X, C3.X));
        float maxX = Math.Max(C1.X, Math.Max(C2.X, C3.X));
        float minY = Math.Min(C1.Y, Math.Min(C2.Y, C3.Y));
        float maxY = Math.Max(C1.Y, Math.Max(C2.Y, C3.Y));

        if (IsVertical)
        {
            float verticalX = IsLeftType ? maxX + RightOffset : minX - RightOffset;

            VerticalLineTop = new PointF(verticalX, maxY + TopOffset);
            VerticalLineBottom = new PointF(verticalX, minY - BottomOffset);

            TopLineStart = VerticalLineTop;
            TopLineEnd = new PointF(
                verticalX + (IsLeftType ? -TopLineLength : TopLineLength),
                VerticalLineTop.Y
            );

            BottomLineEnd = VerticalLineBottom;
            BottomLineStart = new PointF(
                verticalX + (IsLeftType ? -BottomLineLength : BottomLineLength),
                VerticalLineBottom.Y
            );

            PointF dir = ComputeSlopeDirection();

            SlopeLineStart = TopLineEnd;
            SlopeLineEnd = FindArcIntersection(C1, ArcRadius, SlopeLineStart, dir);

            ArcStart = SlopeLineEnd;

            float dy2 = BottomLineStart.Y - C1.Y;
            float inside2 = ArcRadius * ArcRadius - dy2 * dy2;
            if (inside2 < 0) inside2 = 0;
            float dx2 = (float)Math.Sqrt(inside2);

            float arcBottomX = (verticalX > C1.X)
                ? C1.X + dx2
                : C1.X - dx2;

            ArcEnd = new PointF(arcBottomX, BottomLineStart.Y);
        }
        else
        {
            float verticalY = IsTopType ? minY - TopOffset : maxY + TopOffset;

            float verticalLeftX = minX - RightOffset;
            float verticalRightX = maxX + RightOffset;

            VerticalLineTop = new PointF(verticalRightX, verticalY);
            VerticalLineBottom = new PointF(verticalLeftX, verticalY);

            TopLineStart = VerticalLineTop;
            TopLineEnd = new PointF(
                TopLineStart.X + (IsRightType ? -TopLineLength : TopLineLength),
                TopLineStart.Y
            );

            BottomLineEnd = VerticalLineBottom;
            BottomLineStart = new PointF(
                BottomLineEnd.X + (IsRightType ? -BottomLineLength : BottomLineLength),
                BottomLineEnd.Y
            );

            PointF dir = ComputeSlopeDirection();

            SlopeLineStart = TopLineEnd;
            SlopeLineEnd = FindArcIntersection(C1, ArcRadius, SlopeLineStart, dir);

            ArcStart = SlopeLineEnd;

            float dy2 = BottomLineStart.Y - C1.Y;
            float inside2 = ArcRadius * ArcRadius - dy2 * dy2;
            if (inside2 < 0) inside2 = 0;
            float dx2 = (float)Math.Sqrt(inside2);

            float arcBottomX = (VerticalLineTop.X > C1.X)
                ? C1.X + dx2
                : C1.X - dx2;

            ArcEnd = new PointF(arcBottomX, BottomLineStart.Y);
        }
    }

    // 斜线方向 = 平行 C1→C3，但必须朝外侧
    private PointF ComputeSlopeDirection()
    {
        PointF baseDir = Normalize(new PointF(C3.X - C1.X, C3.Y - C1.Y));

        float dirX = baseDir.X;
        float dirY = baseDir.Y;

        bool outerIsRight = VerticalLineTop.X > C1.X;
        bool outerIsLeft = VerticalLineTop.X < C1.X;

        bool outerIsUp = VerticalLineTop.Y > C1.Y;
        bool outerIsDown = VerticalLineTop.Y < C1.Y;

        if (outerIsRight && dirX < 0) dirX = -dirX;
        if (outerIsLeft && dirX > 0) dirX = -dirX;

        if (outerIsUp && dirY < 0) dirY = -dirY;
        if (outerIsDown && dirY > 0) dirY = -dirY;

        return Normalize(new PointF(dirX, dirY));
    }

    private void UpdateBounds()
    {
        float[] xs = { C1.X, C2.X, C3.X,
                       TopLineStart.X, TopLineEnd.X,
                       BottomLineStart.X, BottomLineEnd.X,
                       VerticalLineTop.X, VerticalLineBottom.X,
                       SlopeLineStart.X, SlopeLineEnd.X,
                       ArcStart.X, ArcEnd.X };

        float[] ys = { C1.Y, C2.Y, C3.Y,
                       TopLineStart.Y, TopLineEnd.Y,
                       BottomLineStart.Y, BottomLineEnd.Y,
                       VerticalLineTop.Y, VerticalLineBottom.Y,
                       SlopeLineStart.Y, SlopeLineEnd.Y,
                       ArcStart.Y, ArcEnd.Y };

        MinX = float.MaxValue;
        MaxX = float.MinValue;
        MinY = float.MaxValue;
        MaxY = float.MinValue;

        foreach (var x in xs)
        {
            if (x < MinX) MinX = x;
            if (x > MaxX) MaxX = x;
        }

        foreach (var y in ys)
        {
            if (y < MinY) MinY = y;
            if (y > MaxY) MaxY = y;
        }
    }

    private PointF Normalize(PointF v)
    {
        float len = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
        return new PointF(v.X / len, v.Y / len);
    }

    // 圆与直线交点（取远点 t2）
    private PointF FindArcIntersection(PointF center, float radius, PointF start, PointF dir)
    {
        float dx = dir.X;
        float dy = dir.Y;

        float sx = start.X - center.X;
        float sy = start.Y - center.Y;

        float A = dx * dx + dy * dy;
        float B = 2 * (sx * dx + sy * dy);
        float C = sx * sx + sy * sy - radius * radius;

        float disc = B * B - 4 * A * C;
        if (disc < 0) disc = 0;

        float sqrtDisc = (float)Math.Sqrt(disc);

        float t2 = (-B + sqrtDisc) / (2 * A);   // 远点

        return new PointF(start.X + dx * t2, start.Y + dy * t2);
    }

    private float AngleFromPoints(PointF center, PointF p)
    {
        return (float)(Math.Atan2(p.Y - center.Y, p.X - center.X) * 180.0 / Math.PI);
    }
}
