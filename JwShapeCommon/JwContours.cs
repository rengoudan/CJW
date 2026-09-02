using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

public enum BeamEndPosition
{
    LeftTop,
    LeftBottom,
    RightTop,
    RightBottom
}

public class BeamEndContour
{
    // ⭐ 输入参数：一个圆心 + 方位
    public PointF BaseCenter { get; set; }
    public BeamEndPosition Position { get; set; } = BeamEndPosition.RightTop;

    // ⭐ 固定几何参数（来自你的图纸）
    private const float DX = 68;   // 左右圆心间距
    private const float DY = 56;   // 上下圆心间距

    private const float TopOffset = 35;
    private const float BottomOffset = 35;
    private const float LeftOffset = 26;
    private const float ArcRadius = 35;

    // 绘制参数
    public float Scale = 1f;
    public PointF Offset = new PointF(0, 0);

    public GraphicsPath BuildPath()
    {
        var path = new GraphicsPath();

        // 1️⃣ 自动计算三个圆心（右下圆心为基准）
        PointF C3 = BaseCenter;
        PointF C2 = new PointF(C3.X - DX, C3.Y - DY);
        PointF C1 = new PointF(C3.X - DX * 2, C3.Y - DY * 2);

        // 2️⃣ 根据方位自动镜像（左右）
        bool mirrorX = Position == BeamEndPosition.LeftTop || Position == BeamEndPosition.LeftBottom;
        if (mirrorX)
        {
            float midX = C3.X;
            C1 = MirrorX(C1, midX);
            C2 = MirrorX(C2, midX);
        }

        // 3️⃣ 根据方位自动翻转（上下）
        bool mirrorY = Position == BeamEndPosition.LeftBottom || Position == BeamEndPosition.RightBottom;
        if (mirrorY)
        {
            float midY = C3.Y;
            C1 = MirrorY(C1, midY);
            C2 = MirrorY(C2, midY);
        }

        // 4️⃣ 计算轮廓直线边界
        float yTop = Math.Min(C1.Y, Math.Min(C2.Y, C3.Y)) - TopOffset;
        float yBottom = Math.Max(C1.Y, Math.Max(C2.Y, C3.Y)) + BottomOffset;

        float xLeft = Math.Min(C1.X, Math.Min(C2.X, C3.X)) - LeftOffset;
        float xRight = C3.X + ArcRadius;

        // 5️⃣ 构造轮廓点（矩形部分）
        var pts = new List<PointF>
        {
            new PointF(xLeft, yTop),
            new PointF(xRight, yTop),
            new PointF(xRight, yBottom),
            new PointF(xLeft, yBottom)
        };

        // 6️⃣ 缩放 + 平移
        for (int i = 0; i < pts.Count; i++)
        {
            pts[i] = new PointF(
                pts[i].X * Scale + Offset.X,
                pts[i].Y * Scale + Offset.Y
            );
        }

        path.AddPolygon(pts.ToArray());

        // 7️⃣ 添加圆弧（左右侧自动处理）
        AddArc(path, C3);

        path.CloseFigure();
        return path;
    }

    public void Draw(Graphics g, Pen pen)
    {
        using var path = BuildPath();
        g.DrawPath(pen, path);
    }

    private void AddArc(GraphicsPath path, PointF center)
    {
        var rect = new RectangleF(
            (center.X - ArcRadius) * Scale + Offset.X,
            (center.Y - ArcRadius) * Scale + Offset.Y,
            ArcRadius * 2 * Scale,
            ArcRadius * 2 * Scale
        );

        float startAngle = -90;
        float sweepAngle = 180;

        path.AddArc(rect, startAngle, sweepAngle);
    }

    private PointF MirrorX(PointF p, float midX)
    {
        float dx = p.X - midX;
        return new PointF(midX - dx, p.Y);
    }

    private PointF MirrorY(PointF p, float midY)
    {
        float dy = p.Y - midY;
        return new PointF(p.X, midY - dy);
    }
}

