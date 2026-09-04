using System;
using System.Drawing;
using System.Drawing.Drawing2D;

public enum BeamEndPosition
{
    上左,
    上右
}

public class BeamEndContour
{
    public PointF BaseCenter { get; set; }
    public BeamEndPosition Position { get; set; }

    // 几何参数（工程坐标系）
    private const float DX = 68f;
    private const float DY = 56f;

    private const float RightOffset = 25f;
    private const float TopOffset = 35f;
    private const float BottomOffset = 40f;

    private const float TopLineLength = 47f;
    private const float BottomLineLength = 25f + 68f;

    private const float ArcRadius = 40f;   // ⭐ 圆弧半径最终为 40

    public float HoleRadius { get; set; } = 20f;

    // 屏幕转换参数
    public float Scale { get; set; } = 1f;
    public PointF Offset { get; set; } = new PointF(0, 0);

    // 三个圆心（工程坐标系）
    public PointF C1 { get; private set; }
    public PointF C2 { get; private set; }
    public PointF C3 { get; private set; }

    // ⭐ 工程坐标系路径（不做屏幕转换）
    public GraphicsPath BuildPath()
    {
        var path = new GraphicsPath();

        ComputeCenters();

        // 右侧竖线 X 坐标
        float rightX = C2.X + (Position == BeamEndPosition.上左 ? RightOffset : -RightOffset);

        // 右侧竖线两个端点
        PointF R_top = new PointF(rightX, C3.Y + TopOffset);
        PointF R_bottom = new PointF(rightX, C2.Y - BottomOffset);

        // ⭐ 上横线方向：上左向左，上右向右
        PointF T_end = (Position == BeamEndPosition.上左)
            ? new PointF(R_top.X - TopLineLength, R_top.Y)
            : new PointF(R_top.X + TopLineLength, R_top.Y);

        // ⭐ 下横线方向：上左向左，上右向右
        PointF B_end = (Position == BeamEndPosition.上左)
            ? new PointF(R_bottom.X - BottomLineLength, R_bottom.Y)
            : new PointF(R_bottom.X + BottomLineLength, R_bottom.Y);

        // ⭐ 右上线段方向（平行 C1→C3）
        PointF slopeDir = Normalize(new PointF(C3.X - C1.X, C3.Y - C1.Y));

        // ⭐ 右上线段起点（固定）
        PointF slopeStart = T_end;

        // ⭐ 右上线段终点 = 圆与直线的远交点（t2）
        PointF arcTop = FindArcIntersection(C1, ArcRadius, slopeStart, slopeDir);

        // ⭐ 圆弧与下横线交点（弧线终点）
        float dy2 = B_end.Y - C1.Y;
        float inside2 = ArcRadius * ArcRadius - dy2 * dy2;
        float dx2 = (float)Math.Sqrt(inside2);

        float arcBottomX = (Position == BeamEndPosition.上左)
            ? C1.X + dx2
            : C1.X - dx2;

        PointF arcBottom = new PointF(arcBottomX, B_end.Y);

        // ⭐ 构建工程坐标系路径
        path.StartFigure();
        path.AddLine(R_top, T_end);        // 上横线
       // path.AddLine(T_end, arcTop);       // 右上线段（最终正确）

        // ⭐ 弧线角度
        float startAngle = AngleFromPoints(C1, arcTop);
        float endAngle = AngleFromPoints(C1, arcBottom);
        float sweepAngle = endAngle - startAngle;

        //// ⭐ 只绘制外侧弧线
        //path.AddArc(
        //    C1.X - ArcRadius, C1.Y - ArcRadius,
        //    ArcRadius * 2, ArcRadius * 2,
        //    startAngle,
        //    sweepAngle
        //);

        //path.AddLine(arcBottom, B_end);    // 弧线到下横线
        path.AddLine(B_end, R_bottom);     // 下横线
        //path.AddLine(R_bottom, R_top);     // 右侧竖线
        path.CloseFigure();

        return path;
    }

    // ⭐ 屏幕坐标转换（工程 → 屏幕）
    private PointF ToScreen(PointF p)
    {
        return new PointF(
            p.X * Scale + Offset.X,
            -p.Y * Scale + Offset.Y   // Y 反转
        );
    }

    // ⭐ 绘制轮廓（自动转换为屏幕坐标系）
    public void Draw(Graphics g, Pen pen)
    {
        using var geoPath = BuildPath();
        using var screenPath = (GraphicsPath)geoPath.Clone();

        using var m = new Matrix();
        m.Scale(Scale, -Scale); // Y 反转
        m.Translate(Offset.X, Offset.Y, MatrixOrder.Append);

        screenPath.Transform(m);

        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.DrawPath(pen, screenPath);
    }

    // ⭐ 绘制轮廓 + 三个圆
    public void DrawWithCircles(Graphics g, Pen contourPen, Pen circlePen)
    {
        Draw(g, contourPen);

        DrawCircle(g, circlePen, C1);
        DrawCircle(g, circlePen, C2);
        DrawCircle(g, circlePen, C3);
    }

    // ⭐ 绘制圆（屏幕坐标系）
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

    // ⭐ 计算三个圆心（工程坐标系）
    private void ComputeCenters()
    {
        C1 = BaseCenter;

        if (Position == BeamEndPosition.上左)
        {
            C2 = new PointF(C1.X + DX, C1.Y);
            C3 = new PointF(C2.X, C2.Y + DY);
        }
        else // 上右
        {
            C2 = new PointF(C1.X - DX, C1.Y);
            C3 = new PointF(C2.X, C2.Y + DY);
        }
    }

    private PointF Normalize(PointF v)
    {
        float len = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
        return new PointF(v.X / len, v.Y / len);
    }

    // ⭐ 圆与直线交点（取最长的那个 t2）
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
        float sqrtDisc = (float)Math.Sqrt(disc);

        // 两个交点
        float t1 = (-B - sqrtDisc) / (2 * A);   // 近点
        float t2 = (-B + sqrtDisc) / (2 * A);   // ⭐ 远点（你要的）

        float t = t2;

        return new PointF(start.X + dx * t, start.Y + dy * t);
    }

    private float AngleFromPoints(PointF center, PointF p)
    {
        return (float)(Math.Atan2(p.Y - center.Y, p.X - center.X) * 180.0 / Math.PI);
    }
}
