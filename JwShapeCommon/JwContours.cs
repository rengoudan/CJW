using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

public enum BeamEndPosition
{
    上左,
    上右,
    下左,
    下右,
    右上,
    右下,
    左上,
    左下
}

public class BeamEndContour
{
    public PointF BaseCenter { get; set; }
    public BeamEndPosition Position { get; set; }

    // 固定几何参数（来自图纸）
    private const float DX = 68f;
    private const float DY = 56f;

    private const float TopOffset = 35f;
    private const float BottomOffset = 35f;
    private const float LeftOffset = 26f;
    private const float ArcRadius = 35f;

    public float HoleRadius { get; set; } = 20f;

    public float Scale { get; set; } = 1f;
    public PointF Offset { get; set; } = new PointF(0, 0);

    // 三个圆心（自动计算）
    public PointF C1 { get; private set; }   // 最左或最上
    public PointF C2 { get; private set; }
    public PointF C3 { get; private set; }   // 最右或最下

    public GraphicsPath BuildPath()
    {
        var path = new GraphicsPath();

        ComputeCenters();

        // 上边折线：C1 → C2 → C3 上移 TopOffset
        var topPts = new List<PointF>
        {
            new PointF(C1.X, C1.Y - TopOffset),
            new PointF(C2.X, C2.Y - TopOffset),
            new PointF(C3.X, C3.Y - TopOffset)
        };

        // 下边折线：C3 → C2 → C1 下移 BottomOffset
        var bottomPts = new List<PointF>
        {
            new PointF(C3.X, C3.Y + BottomOffset),
            new PointF(C2.X, C2.Y + BottomOffset),
            new PointF(C1.X, C1.Y + BottomOffset)
        };

        // 左边偏移
        var leftTop = new PointF(C1.X - LeftOffset, C1.Y - TopOffset);
        var leftBottom = new PointF(C1.X - LeftOffset, C1.Y + BottomOffset);

        // 右侧圆弧
        bool arcOnRight =
            Position == BeamEndPosition.上右 ||
            Position == BeamEndPosition.下右 ||
            Position == BeamEndPosition.右上 ||
            Position == BeamEndPosition.右下;

        float sign = arcOnRight ? 1f : -1f;

        var arcRect = new RectangleF(
            C3.X - ArcRadius * sign,
            C3.Y - ArcRadius,
            ArcRadius * 2 * sign,
            ArcRadius * 2
        );

        float startAngle = arcOnRight ? -90f : 90f;
        float sweepAngle = 180f * sign;

        // 组合轮廓路径
        path.StartFigure();
        path.AddLine(leftTop, topPts[0]);
        path.AddLines(topPts.ToArray());
        path.AddArc(arcRect, startAngle, sweepAngle);
        path.AddLines(bottomPts.ToArray());
        path.AddLine(bottomPts[^1], leftBottom);
        path.CloseFigure();

        // 缩放 + 平移
        using var m = new Matrix();
        m.Scale(Scale, Scale);
        m.Translate(Offset.X, Offset.Y, MatrixOrder.Append);
        path.Transform(m);

        return path;
    }

    public void Draw(Graphics g, Pen contourPen, Pen circlePen)
    {
        using var path = BuildPath();
        g.DrawPath(contourPen, path);

        DrawCircle(g, circlePen, C1);
        DrawCircle(g, circlePen, C2);
        DrawCircle(g, circlePen, C3);
    }

    private void DrawCircle(Graphics g, Pen pen, PointF center)
    {
        var rect = new RectangleF(
            (center.X - HoleRadius) * Scale + Offset.X,
            (center.Y - HoleRadius) * Scale + Offset.Y,
            HoleRadius * 2 * Scale,
            HoleRadius * 2 * Scale
        );
        g.DrawEllipse(pen, rect);
    }

    private void ComputeCenters()
    {
        // BaseCenter 是该方位的外侧圆心
        switch (Position)
        {
            case BeamEndPosition.上左:
                C1 = BaseCenter;
                C2 = new PointF(C1.X + DX, C1.Y + DY);
                C3 = new PointF(C1.X + DX * 2, C1.Y + DY * 2);
                break;

            case BeamEndPosition.上右:
                C3 = BaseCenter;
                C2 = new PointF(C3.X - DX, C3.Y + DY);
                C1 = new PointF(C3.X - DX * 2, C3.Y + DY * 2);
                break;

            case BeamEndPosition.下左:
                C1 = BaseCenter;
                C2 = new PointF(C1.X + DX, C1.Y - DY);
                C3 = new PointF(C1.X + DX * 2, C1.Y - DY * 2);
                break;

            case BeamEndPosition.下右:
                C3 = BaseCenter;
                C2 = new PointF(C3.X - DX, C3.Y - DY);
                C1 = new PointF(C3.X - DX * 2, C3.Y - DY * 2);
                break;

            case BeamEndPosition.右上:
                C1 = BaseCenter;
                C2 = new PointF(C1.X + DX, C1.Y - DY);
                C3 = new PointF(C1.X + DX * 2, C1.Y - DY * 2);
                break;

            case BeamEndPosition.右下:
                C3 = BaseCenter;
                C2 = new PointF(C3.X - DX, C3.Y - DY);
                C1 = new PointF(C3.X - DX * 2, C3.Y - DY * 2);
                break;

            case BeamEndPosition.左上:
                C1 = BaseCenter;
                C2 = new PointF(C1.X - DX, C1.Y - DY);
                C3 = new PointF(C1.X - DX * 2, C1.Y - DY * 2);
                break;

            case BeamEndPosition.左下:
                C3 = BaseCenter;
                C2 = new PointF(C3.X + DX, C3.Y - DY);
                C1 = new PointF(C3.X + DX * 2, C3.Y - DY * 2);
                break;
        }
    }
}
