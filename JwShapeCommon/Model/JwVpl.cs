using JwCore;
using JwwHelper;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwVpl:IDrawToJww
    {

        public JwVpl(JWPoint p,BeamEndPosition bep) 
        {
            Location = p;
            Position = bep;
            //createOther();
            createOther();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 链接点 即三个圆孔中的基准孔中心
        /// </summary>
        public JWPoint Location { get; set; }

        /// <summary>
        /// 相对于Location的第二个孔位置
        /// </summary>
        public JWPoint SecondLoaction { get; set; }

        /// <summary>
        /// 第三个孔位置一般为败方G 打孔位置
        /// </summary>
        public JWPoint ThirdLocation { get; set; }

        public BeamEndPosition Position { get; set; }

        /// <summary>
        /// 部件侧边
        /// </summary>
        public JwXian SideLine { get; set; }

        /// <summary>
        /// 部件顶边
        /// </summary>
        public JwXian TopLine { get; set; }

        /// <summary>
        /// 部件底边
        /// </summary>
        public JwXian BottomLine { get; set; }

        /// <summary>
        /// 部件倾斜轮廓线
        /// </summary>
        public JwXian Slash { get; set; }

        public JwArc Arc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private const double bottomSpacing= 40;

        private const double secondCenterSpacing = 68;

        private const double sideLength = 131;

        private const double longestsideLength = 95;

        private const double shortsideLength = 50;

        private const double topSpacing = 35;
        private const double jpSpacing = 45;

        private void createOther()
        {
            switch (Position)
            {
                case BeamEndPosition.上右:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy;
                        var slx=cx-(secondCenterSpacing/JwFileConsts.JwScale); 
                        SecondLoaction=new JWPoint(slx,sly);
                        var by = cy - (bottomSpacing / JwFileConsts.JwScale);
                        var bx = cx - (longestsideLength / JwFileConsts.JwScale);
                        var sidebottom =new JWPoint(bx, by);
                        var ul = new JWPoint(cx, by);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsy = cy + ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(bx, tpsy);
                        SideLine = new JwXian(sidebottom, topside);
                        var trx=cx-(jpSpacing/JwFileConsts.JwScale);
                        var topother = new JWPoint(trx, tpsy);
                        TopLine = new JwXian(topother, topside);
                        var tlx=cy+JwFileConsts.EllipseSpacing/JwFileConsts.JwScale;
                        ThirdLocation=new JWPoint(slx, tlx);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, 1.5707963267948966, -2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;  
                        Slash=new JwXian(topother, l);
                        break;
                    }
                case BeamEndPosition.上左:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy;
                        var slx = cx + (secondCenterSpacing / JwFileConsts.JwScale);
                        SecondLoaction = new JWPoint(slx, sly);
                        var by = cy - (bottomSpacing / JwFileConsts.JwScale);
                        var bx = cx + (longestsideLength / JwFileConsts.JwScale);
                        var sidebottom = new JWPoint(bx, by);
                        var ul = new JWPoint(cx, by);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsy = cy + ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(bx, tpsy);
                        SideLine = new JwXian(sidebottom, topside);
                        var trx = cx + (jpSpacing / JwFileConsts.JwScale);
                        var topother = new JWPoint(trx, tpsy);
                        TopLine = new JwXian(topother, topside);
                        var tlx = cy + JwFileConsts.EllipseSpacing / JwFileConsts.JwScale;
                        ThirdLocation = new JWPoint(slx, tlx);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, 1.5707963267948966, 2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;
                        Slash = new JwXian(topother, l);
                        break;
                    }
                case BeamEndPosition.下右:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy;
                        var slx = cx - (secondCenterSpacing / JwFileConsts.JwScale);
                        SecondLoaction = new JWPoint(slx, sly);
                        var by = cy + (bottomSpacing / JwFileConsts.JwScale);
                        var bx = cx - (longestsideLength / JwFileConsts.JwScale);
                        var sidebottom = new JWPoint(bx, by);
                        var ul = new JWPoint(cx, by);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsy = cy - ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(bx, tpsy);
                        SideLine = new JwXian(sidebottom, topside);
                        var trx = cx - (jpSpacing / JwFileConsts.JwScale);
                        var topother = new JWPoint(trx, tpsy);
                        TopLine = new JwXian(topother, topside);
                        var tlx = cy - JwFileConsts.EllipseSpacing / JwFileConsts.JwScale;
                        ThirdLocation = new JWPoint(slx, tlx);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, -1.5707963267948966, 2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;
                        Slash = new JwXian(topother, l);
                        break;
                    }

                case BeamEndPosition.下左:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy;
                        var slx = cx + (secondCenterSpacing / JwFileConsts.JwScale);
                        SecondLoaction = new JWPoint(slx, sly);
                        var by = cy + (bottomSpacing / JwFileConsts.JwScale);
                        var bx = cx + (longestsideLength / JwFileConsts.JwScale);
                        var sidebottom = new JWPoint(bx, by);
                        var ul = new JWPoint(cx, by);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsy = cy - ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(bx, tpsy);
                        SideLine = new JwXian(sidebottom, topside);
                        var trx = cx + (jpSpacing / JwFileConsts.JwScale);
                        var topother = new JWPoint(trx, tpsy);
                        TopLine = new JwXian(topother, topside);
                        var tlx = cy - JwFileConsts.EllipseSpacing / JwFileConsts.JwScale;
                        ThirdLocation = new JWPoint(slx, tlx);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, -1.5707963267948966, -2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;
                        Slash = new JwXian(topother, l);
                        break;
                    }
                case BeamEndPosition.左上:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy - (secondCenterSpacing / JwFileConsts.JwScale);
                        var slx = cx;
                        SecondLoaction = new JWPoint(slx, sly);
                        var by = cy - (longestsideLength / JwFileConsts.JwScale);
                        var bx = cx+ (bottomSpacing / JwFileConsts.JwScale);  
                        var sidebottom = new JWPoint(bx, by);
                        var ul = new JWPoint(bx, cy);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsx = cx - ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(tpsx, by);
                        SideLine = new JwXian(sidebottom, topside);
                        var trxy = cy - (jpSpacing / JwFileConsts.JwScale);
                        var topother = new JWPoint(tpsx, trxy);
                        TopLine = new JwXian(topother, topside);
                        var tlx = cx - JwFileConsts.EllipseSpacing / JwFileConsts.JwScale;
                        ThirdLocation = new JWPoint(tlx,sly);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, 0, -2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;
                        Slash = new JwXian(topother, l);
                        break;
                    }
                case BeamEndPosition.左下:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy + (secondCenterSpacing / JwFileConsts.JwScale);
                        var slx = cx;
                        SecondLoaction = new JWPoint(slx, sly);
                        var by = cy + (longestsideLength / JwFileConsts.JwScale);
                        var bx = cx + (bottomSpacing / JwFileConsts.JwScale);
                        var sidebottom = new JWPoint(bx, by);
                        var ul = new JWPoint(bx, cy);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsx = cx - ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(tpsx, by);
                        SideLine = new JwXian(sidebottom, topside);
                        var trxy = cy + (jpSpacing / JwFileConsts.JwScale);
                        var topother = new JWPoint(tpsx, trxy);
                        TopLine = new JwXian(topother, topside);
                        var tlx = cx - JwFileConsts.EllipseSpacing / JwFileConsts.JwScale;
                        ThirdLocation = new JWPoint(tlx, sly);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, 0, 2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;
                        Slash = new JwXian(topother, l);
                        break;
                    }

                case BeamEndPosition.右上:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy - (secondCenterSpacing / JwFileConsts.JwScale);
                        var slx = cx;
                        SecondLoaction = new JWPoint(slx, sly);
                        var by = cy - (longestsideLength / JwFileConsts.JwScale);
                        var bx = cx - (bottomSpacing / JwFileConsts.JwScale);
                        var sidebottom = new JWPoint(bx, by);
                        var ul = new JWPoint(bx, cy);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsx = cx + ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(tpsx, by);
                        SideLine = new JwXian(sidebottom, topside);
                        var trxy = cy - (jpSpacing / JwFileConsts.JwScale);
                        var topother = new JWPoint(tpsx, trxy);
                        TopLine = new JwXian(topother, topside);
                        var tlx = cx + JwFileConsts.EllipseSpacing / JwFileConsts.JwScale;
                        ThirdLocation = new JWPoint(tlx, sly);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, 3.1415926535897931, 2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;
                        Slash = new JwXian(topother, l);
                        break;
                    }
                case BeamEndPosition.右下:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy + (secondCenterSpacing / JwFileConsts.JwScale);
                        var slx = cx;
                        SecondLoaction = new JWPoint(slx, sly);
                        var by = cy + (longestsideLength / JwFileConsts.JwScale);
                        var bx = cx - (bottomSpacing / JwFileConsts.JwScale);
                        var sidebottom = new JWPoint(bx, by);
                        var ul = new JWPoint(bx, cy);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsx = cx + ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(tpsx, by);
                        SideLine = new JwXian(sidebottom, topside);
                        var trxy = cy + (jpSpacing / JwFileConsts.JwScale);
                        var topother = new JWPoint(tpsx, trxy);
                        TopLine = new JwXian(topother, topside);
                        var tlx = cx + JwFileConsts.EllipseSpacing / JwFileConsts.JwScale;
                        ThirdLocation = new JWPoint(tlx, sly);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, 3.1415926535897931, -2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;
                        Slash = new JwXian(topother, l);
                        break;
                    }

            }
        }

        //enum TemplateType { Horizontal, Vertical }

        //struct DirConfig
        //{
        //    public TemplateType Type;
        //    public bool MirrorX;
        //    public bool MirrorY;
        //    public double ArcStart;
        //    public double ArcSweep;
        //}

        //private static readonly Dictionary<BeamEndPosition, DirConfig> DirMap =
        //    new Dictionary<BeamEndPosition, DirConfig>
        //    {
        //        // 水平模板（上/下）
        //        [BeamEndPosition.上右] = new DirConfig { Type = TemplateType.Horizontal, MirrorX = false, MirrorY = false, ArcStart = Math.PI / 2, ArcSweep = -2.4035303736600242 },
        //        [BeamEndPosition.上左] = new DirConfig { Type = TemplateType.Horizontal, MirrorX = true, MirrorY = false, ArcStart = Math.PI / 2, ArcSweep = 2.4035303736600242 },

        //        [BeamEndPosition.下右] = new DirConfig { Type = TemplateType.Horizontal, MirrorX = false, MirrorY = true, ArcStart = -Math.PI / 2, ArcSweep = 2.4035303736600242 },
        //        [BeamEndPosition.下左] = new DirConfig { Type = TemplateType.Horizontal, MirrorX = true, MirrorY = true, ArcStart = -Math.PI / 2, ArcSweep = -2.4035303736600242 },

        //        // 垂直模板（左/右）
        //        [BeamEndPosition.左上] = new DirConfig { Type = TemplateType.Vertical, MirrorX = false, MirrorY = false, ArcStart = 0, ArcSweep = -2.4035303736600242 },
        //        [BeamEndPosition.左下] = new DirConfig { Type = TemplateType.Vertical, MirrorX = false, MirrorY = true, ArcStart = 0, ArcSweep = 2.4035303736600242 },

        //        [BeamEndPosition.右上] = new DirConfig { Type = TemplateType.Vertical, MirrorX = true, MirrorY = false, ArcStart = Math.PI, ArcSweep = 2.4035303736600242 },
        //        [BeamEndPosition.右下] = new DirConfig { Type = TemplateType.Vertical, MirrorX = true, MirrorY = true, ArcStart = Math.PI, ArcSweep = -2.4035303736600242 },
        //    };
        //private JWPoint ApplyOffset(double dx, double dy, DirConfig cfg)
        //{
        //    if (cfg.MirrorX) dx = -dx;
        //    if (cfg.MirrorY) dy = -dy;

        //    return new JWPoint(
        //        Location.X + dx / JwFileConsts.JwScale,
        //        Location.Y + dy / JwFileConsts.JwScale
        //    );
        //}
        //private void createOther1()
        //{
        //    var cfg = DirMap[Position];

        //    // -------------------------
        //    // 水平模板（上/下）
        //    // -------------------------
        //    if (cfg.Type == TemplateType.Horizontal)
        //    {
        //        // 第二孔
        //        SecondLoaction = ApplyOffset(-secondCenterSpacing, 0, cfg);

        //        // 底边
        //        var sidebottom = ApplyOffset(-longestsideLength, -bottomSpacing, cfg);
        //        var ul = ApplyOffset(0, -bottomSpacing, cfg);
        //        BottomLine = new JwXian(sidebottom, ul);

        //        // 侧边
        //        var topside = ApplyOffset(-longestsideLength, sideLength - bottomSpacing, cfg);
        //        SideLine = new JwXian(sidebottom, topside);

        //        // 顶边
        //        var topother = ApplyOffset(-jpSpacing, sideLength - bottomSpacing, cfg);
        //        TopLine = new JwXian(topother, topside);

        //        // 第三孔
        //        ThirdLocation = ApplyOffset(0, JwFileConsts.EllipseSpacing, cfg);

        //        // 圆弧
        //        Arc = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, cfg.ArcStart, cfg.ArcSweep);
        //        Slash = new JwXian(topother, Arc.ArcFinish);
        //        return;
        //    }

        //    // -------------------------
        //    // 垂直模板（左/右）
        //    // -------------------------
        //    {
        //        // 第二孔
        //        SecondLoaction = ApplyOffset(0, -secondCenterSpacing, cfg);

        //        // 底边
        //        var sidebottom = ApplyOffset(bottomSpacing, -longestsideLength, cfg);
        //        var ul = ApplyOffset(bottomSpacing, 0, cfg);
        //        BottomLine = new JwXian(sidebottom, ul);

        //        // 侧边
        //        var topside = ApplyOffset(-(sideLength - bottomSpacing), -longestsideLength, cfg);
        //        SideLine = new JwXian(sidebottom, topside);

        //        // 顶边
        //        var topother = ApplyOffset(-(sideLength - bottomSpacing), -jpSpacing, cfg);
        //        TopLine = new JwXian(topother, topside);

        //        // 第三孔
        //        ThirdLocation = ApplyOffset(-JwFileConsts.EllipseSpacing, -secondCenterSpacing, cfg);

        //        // 圆弧
        //        Arc = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, cfg.ArcStart, cfg.ArcSweep);
        //        Slash = new JwXian(topother, Arc.ArcFinish);
        //    }
        //}


        public List<JwwData> DrawToJww()
        {
            List<JwwData> jwws = new List<JwwData>();
            jwws.Add(SideLine.ToJwwData(DrawShapeType.Pillar));
            jwws.Add(TopLine.ToJwwData(DrawShapeType.Pillar));
            jwws.Add(BottomLine.ToJwwData(DrawShapeType.Pillar));
            jwws.Add(Slash.ToJwwData(DrawShapeType.Pillar));
            jwws.Add(Arc.ToJwwData(DrawShapeType.Pillar));
            return jwws;
        }

        /// <summary>
        /// 用来实现绘制到屏幕 需要增加缩放和便宜旋转等操作
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        public void Draw(Graphics g, Pen pen, double zoom, double axisx, double axisy)
        {
            using var geoPath = BuildPath(zoom,axisx,axisy);
            //g.DrawLine(pen,SideLine.Pone.ToChangeCoordinate(zoom, axisx, axisy), SideLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            //g.DrawLine(pen, TopLine.Pone.ToChangeCoordinate(zoom, axisx, axisy), TopLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            //g.DrawLine(pen, BottomLine.Pone.ToChangeCoordinate(zoom, axisx, axisy), BottomLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            //g.DrawLine(pen, Slash.Pone.ToChangeCoordinate(zoom, axisx, axisy), Slash.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            //var zoomradius = Arc.Radius * zoom;
            //var arcnewcenterx = Arc.Center.X * zoom + axisx;
            //var arcnewcentery = axisy - Arc.Center.Y * zoom;
            //var qsh= JwExtend.RadToDeg((float)Arc.StartAngle);
            //var xz= JwExtend.RadToDeg((float)Arc.SweepAngle);
            //g.DrawArc(pen,
            //(float)(arcnewcenterx - zoomradius), (float)(arcnewcentery - zoomradius),
            //(float)zoomradius * 2, (float)
            //zoomradius * 2,
            // JwExtend.RadToDeg((float)Arc.StartAngle),
            //JwExtend.RadToDeg((float)Arc.SweepAngle));
            //using var screenPath = (GraphicsPath)geoPath.Clone();

            //using var m = new Matrix();
            //m.Scale(Scale, -Scale);
            //m.Translate(Offset.X, Offset.Y, MatrixOrder.Append);

            //screenPath.Transform(m);

            //g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawPath(pen, geoPath);
            //绘制圆圈
            DrawCircles(g, pen, zoom, axisx, axisy);
        }

        /// <summary>
        /// 根据缩放直接存放到path中 便于绘制到屏幕上
        /// </summary>
        /// <param name="zoom">缩放比例</param>
        /// <param name="axisx"></param>
        /// <param name="axisy"></param>
        /// <returns></returns>
        private GraphicsPath BuildPath(double zoom, double axisx, double axisy)
        {
            var path = new GraphicsPath();
            //绘制三根线
            path.AddLine(TopLine.Pone.ToChangeCoordinate(zoom, axisx, axisy), TopLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            path.AddLine(SideLine.Pone.ToChangeCoordinate(zoom,axisx, axisy), SideLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            path.AddLine(BottomLine.Pone.ToChangeCoordinate(zoom, axisx, axisy), BottomLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            //path.AddLine(Slash.Pone.ToChangeCoordinate(zoom, axisx, axisy), Slash.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            var zoomradius= Arc.Radius * zoom;
            var arcnewcenterx= Arc.Center.X * zoom + axisx;
            var arcnewcentery = axisy - Arc.Center.Y * zoom;
            path.AddArc(
            (float)(arcnewcenterx - zoomradius), (float)(arcnewcentery - zoomradius),
            (float)zoomradius * 2, (float)
            zoomradius * 2,
             JwExtend.RadToDeg((float)Arc.StartAngle),
            JwExtend.RadToDeg((float)Arc.SweepAngle));
            path.CloseFigure();
            return path;
        }


        private void DrawCircles(Graphics g, Pen pen, double zoom,double ax,double ay)
        {
            var pc = SecondLoaction.ToChangeCoordinate(zoom, ax, ay);

            var holeradius=JwFileConsts.EllipseDiameter/JwFileConsts.JwScale;
            var newradius = holeradius * zoom;
            var rect = new RectangleF(
                pc.X - (float)newradius,
                pc.Y - (float)newradius,
                (float)newradius * 2,
                (float)newradius * 2
            );
            g.DrawEllipse(pen, rect);

            var pc2 = ThirdLocation.ToChangeCoordinate(zoom, ax, ay);
            var rect2 = new RectangleF(
                pc2.X - (float)newradius,
                pc2.Y - (float)newradius,
                (float)newradius * 2,
                (float)newradius * 2
            );
            g.DrawEllipse(pen, rect2);

            var pc3 = Location.ToChangeCoordinate(zoom, ax, ay);
            var rect3 = new RectangleF(
                pc3.X - (float)newradius,
                pc3.Y - (float)newradius,
                (float)newradius * 2,
                (float)newradius * 2
            );
            g.DrawEllipse(pen, rect3);
        }
    }


    /// <summary>
    /// jw圆弧
    /// </summary>
    public class JwArc
    {
        public string Id { get; set; }
        public JWPoint Center { get; }
        public double Radius { get; }


        /// <summary>
        /// radkatamukikaku 起始度数 0 与x轴正方向相同
        /// -1.5707963267948966 
        /// </summary>
        public double StartAngle { get; }

        /// <summary>
        /// radenkokaku 旋转角度 2.4035303736600242 固定正为逆时针
        /// </summary>
        public double SweepAngle { get; }

        public JWPoint ArcFinish { get; set; }

        public JwArc(JWPoint center, double radius, double startAngle, double endAngle)
        {
            Id=Guid.NewGuid().ToString();
            Center = center;
            Radius = radius;
            StartAngle = startAngle;
            SweepAngle = endAngle;
            ArcFinish=JwExtend.GetArcEndPoint(center,radius, -startAngle, -endAngle);
        }


        public JwwData ToJwwData(DrawShapeType shapeType)
        {
            var sen = new JwwEnko();
            //sen.m_nPenWidth=1/
            sen.m_nPenColor = (short)shapeType;

            sen.m_start_x = Center.X;
            sen.m_start_y = Center.Y;
            sen.m_dHankei=Radius;
            sen.m_nLayer = (short)((int)shapeType + 1);
            sen.m_nPenColor = (short)((int)shapeType);
            sen.m_nPenStyle = 1;
            sen.m_nPenWidth = 0;
            sen.m_radEnkoKaku = -SweepAngle;
            sen.m_radKatamukiKaku= -StartAngle;
            sen.m_bZenEnFlg = 0;
            sen.m_dHenpeiRitsu = 1;
            return sen;
        }
    }

}
