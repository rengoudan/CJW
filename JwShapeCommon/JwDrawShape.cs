using JwCore;
using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwDrawShape:JwSquareBase,ICanZoom,IChangeAxis 
    {
        //private 

        public DrawShapeType ShapeType { get; set; }
        public JwSquareBase jwShape { get; set; }

        public List<ControlText> Texts =new List<ControlText>();

        public JwDrawShape(JwSquareBase jwSquare) 
        {
            jwShape=jwSquare;
            Init(jwSquare);
        }

        public JwDrawShape(JwSquareBase jwSquare,double height)
        {
            jwShape = jwSquare;
            Init(jwSquare, height);
        }

        public JwDrawShape(JwKongZu hole,DrawShapeType type)
        {
            ShapeType = type;
            //InitByHole(hole);
        }

        public JwDrawShape(JwHole hole, DrawShapeType type)
        {
            ShapeType = type;
            InitByHole(hole);
        }

        public List<float> FuzhuXs = new List<float>();

        public List<float> FuzhuYs = new List<float>();

        public void Init(JwSquareBase jwSquare,double hbeilv=1)
        {
            TopRight = new JWPoint(jwSquare.TopRight.X, jwSquare.TopRight.Y+( hbeilv==1?0:(jwSquare.Height/2)));
            TopLeft = new JWPoint(jwSquare.TopLeft.X, TopRight.Y);
            BottomRight = new JWPoint(jwSquare.BottomRight.X, jwSquare.BottomRight.Y - (hbeilv == 1 ? 0 : (jwSquare.Height / 2)));
            BottomLeft = new JWPoint(jwSquare.BottomLeft.X, BottomRight.Y);
            
            ShapeType = DrawShapeType.None;
            if (jwSquare.GetType().Name == "JwBeam")
            {
                if (hbeilv != 1.0)
                {
                    ShapeType = DrawShapeType.Beam;
                    JisuanWidthHeight();
                }
                else
                {
                    ShapeType = DrawShapeType.Beam;
                    base.Width = jwSquare.Width;
                    base.Height = jwSquare.Height;
                    base.DirectionType = jwShape.DirectionType;
                    base.CenterPoint = new JWPoint(jwShape.CenterPoint.X, jwShape.CenterPoint.Y);
                    base.Center = jwShape.Center;
                    base.Jiaodu = jwSquare.Jiaodu;
                }
                //ShapeType = DrawShapeType.Beam;
                //Width = jwSquare.Width;
                //Height= jwSquare.Height;
                //DirectionType=jwShape.DirectionType;
                //CenterPoint = jwShape.CenterPoint;
                //Center=jwShape.Center;
                //Jiaodu= jwSquare.Jiaodu;
            }
            if (jwSquare.GetType().Name == "JwBlock")
            {
                JisuanWidthHeight();
                ShapeType = DrawShapeType.Block;
            }
        }

        public void InitByHole(JwHole hole)
        {
            CenterPoint = new JWPoint( hole.Location.X,hole.Location.Y);
            double halfbj = JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale);
            if (hole.FirstCreateFrom == HoleCreateFrom.FengeJ)
            {
                TopLeft = new JWPoint(CenterPoint.X, CenterPoint.Y + halfbj);
                TopRight = new JWPoint(CenterPoint.X, CenterPoint.Y + halfbj);
                BottomLeft = new JWPoint(CenterPoint.X, CenterPoint.Y - halfbj);
                BottomRight = new JWPoint(CenterPoint.X, CenterPoint.Y - halfbj);
                ShapeType = DrawShapeType.Hole;
            }
            else
            {
                if (hole.KongNum == 4)
                {
                    //double halfbj = JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale);
                    TopLeft = new JWPoint(CenterPoint.X - halfbj, CenterPoint.Y + halfbj);
                    TopRight = new JWPoint(CenterPoint.X + halfbj, TopLeft.Y);
                    BottomLeft = new JWPoint(TopLeft.X, CenterPoint.Y - halfbj);
                    BottomRight = new JWPoint(TopRight.X, BottomLeft.Y);
                }
                else
                {
                    TopLeft = new JWPoint(CenterPoint.X, CenterPoint.Y + halfbj);
                    TopRight = new JWPoint(CenterPoint.X, CenterPoint.Y + halfbj);
                    BottomLeft = new JWPoint(CenterPoint.X, CenterPoint.Y - halfbj);
                    BottomRight = new JWPoint(CenterPoint.X, CenterPoint.Y - halfbj);
                    ShapeType = DrawShapeType.Hole;
                }
            }
            
        }

        /// <summary>
        /// 传入窗体大小 change完后返回 list 绘制内容
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offset">共同偏移量</param>
        public List<ControlDraw> Change(double zoom,double axisx,double axisy)
        {
            if (Width!=0&&Height!=0&&CenterPoint!=null)
            {
                
                Zoom(zoom);
                
                ChangeAxis(axisx, axisy);
            }
            if(ShapeType==DrawShapeType.Hole)
            {
                Zoom(zoom);

                ChangeAxis(axisx, axisy);
            }
         
            return Draw();
        }

        public List<RectangleF> RectangleFs=new List<RectangleF>();

        public List<ControlDraw> controlDraws=new List<ControlDraw>();



        public List<ControlDraw> Draw()
        {
            if(ShapeType==DrawShapeType.Beam)
            {
            
                PointF pf=new PointF((float)TopLeft.X, (float)TopLeft.Y);
                var z=new RectangleF(pf, new SizeF((float)Width, (float)Height));
                ControlDraw draw=new ControlDraw();
                draw.PenColor= Color.White;
                draw.DrawRectangleF = z;
                draw.ShapeType = ShapeType;
                draw.JwSquareBase = jwShape;
                controlDraws.Add(draw);
                if (jwShape.DirectionType == BeamDirectionType.Horizontal)
                {
                    FuzhuYs.Add((float)CenterPoint.Y);
                }
                else if(jwShape.DirectionType==BeamDirectionType.Vertical)
                {
                    FuzhuXs.Add((float)CenterPoint.X);
                }
                else
                {
                    draw.IsTeshuBeam = true;
                    draw.DrawPoints = new List<PointF>();
                    // 将角度转换为弧度
                    double radian = -Jiaodu * Math.PI / 180;

                    // 计算四个顶点的相对坐标
                    PointF[] points = new PointF[4];
                    draw.DrawPoints.Add(new PointF(
                       (float)CenterPoint.X + (float)(-Width / 2 * Math.Cos(radian) - Height / 2 * Math.Sin(radian)),
                        (float)CenterPoint.Y + (float)(-Width / 2 * Math.Sin(radian) + Height / 2 * Math.Cos(radian))));
                    draw.DrawPoints.Add(new PointF(
                        (float)CenterPoint.X + (float)(Width / 2 * Math.Cos(radian) - Height / 2 * Math.Sin(radian)),
                        (float)CenterPoint.Y + (float)(Width / 2 * Math.Sin(radian) + Height / 2 * Math.Cos(radian))));
                    draw.DrawPoints.Add(new PointF(
                        (float)CenterPoint.X + (float)(Width / 2 * Math.Cos(radian) + Height / 2 * Math.Sin(radian)),
                        (float)CenterPoint.Y + (float)(Width / 2 * Math.Sin(radian) - Height / 2 * Math.Cos(radian))));
                    draw.DrawPoints.Add(new PointF(
                        (float)CenterPoint.X + (float)(-Width / 2 * Math.Cos(radian) + Height / 2 * Math.Sin(radian)),
                        (float)CenterPoint.Y + (float)(-Width / 2 * Math.Sin(radian) - Height / 2 * Math.Cos(radian))));

                    // 绘制倾斜的矩形

                }
                float textx = DirectionType == BeamDirectionType.Horizontal ? (float)CenterPoint.X : (float)TopRight.X + 3;
                float texty = DirectionType == BeamDirectionType.Vertical ? (float)CenterPoint.Y : (float)BottomLeft.Y + 3;
                var bjm = jwShape as JwBeam;
                ControlText controlText = new ControlText
                {
                    Location = new PointF(textx, texty),
                    Text = bjm.BeamCode,
                    DirectionType = DirectionType
                };
                Texts.Add(controlText);
            }
            if (ShapeType == DrawShapeType.Block)
            {
                
                var sz = new SizeF((float)EllipseDiameter, (float)EllipseDiameter);
                var sp = new PointF((float)(CenterPoint.X - EllipseSpacing / 2 - EllipseDiameter / 2), (float)(CenterPoint.Y - EllipseSpacing / 2 - EllipseDiameter / 2));
                var zs=new RectangleF(sp, sz);
                    ControlDraw draw = new ControlDraw();
                    draw.PenColor = Color.Yellow;
                    draw.DrawRectangleF = zs;
                    draw.ShapeType = ShapeType;
                    controlDraws.Add(draw);

                var sp1 = new PointF((float)(CenterPoint.X + EllipseSpacing / 2 - EllipseDiameter / 2), (float)(CenterPoint.Y - EllipseSpacing / 2 - EllipseDiameter / 2));
                var ys=new RectangleF(sp1,sz);
                    ControlDraw draw1 = new ControlDraw();
                    draw1.PenColor = Color.Yellow;
                    draw1.DrawRectangleF = ys;
                    draw1.ShapeType = ShapeType;
                    controlDraws.Add(draw1);


                var sp2 = new PointF((float)(CenterPoint.X - EllipseSpacing / 2 - EllipseDiameter / 2), (float)(CenterPoint.Y + EllipseSpacing / 2 - EllipseDiameter / 2));
                var zx=new RectangleF(sp2, sz);
                    ControlDraw draw2 = new ControlDraw();
                    draw2.PenColor = Color.Yellow;
                    draw2.DrawRectangleF = zx;
                    draw2.ShapeType = ShapeType;
                    controlDraws.Add(draw2);
                var sp3 = new PointF((float)(CenterPoint.X + EllipseSpacing / 2 - EllipseDiameter / 2), (float)(CenterPoint.Y + EllipseSpacing / 2 - EllipseDiameter / 2));
                    var yx = new RectangleF(sp3, sz);
                    ControlDraw draw3 = new ControlDraw();
                    draw3.PenColor = Color.Yellow;
                    draw3.DrawRectangleF = yx;
                    draw3.ShapeType = ShapeType;
                    controlDraws.Add(draw3);
                }
            if (ShapeType == DrawShapeType.Pillar)
            {
                PointF pf = new PointF((float)TopLeft.X, (float)TopLeft.Y);
                var z = new RectangleF(pf, new SizeF((float)Width, (float)Height));
                ControlDraw draw = new ControlDraw();
                draw.PenColor = Color.White;
                draw.DrawRectangleF = z;
                draw.ShapeType = ShapeType;
                draw.JwSquareBase = jwShape;
                controlDraws.Add(draw);
            }
            if(ShapeType==DrawShapeType.Hole)
            {
                var sz = new SizeF((float)EllipseDiameter, (float)EllipseDiameter);
                var sp = new PointF((float)(TopLeft.X - EllipseDiameter / 2), (float)(TopLeft.Y  - EllipseDiameter / 2));
                var zs = new RectangleF(sp, sz);
                ControlDraw draw = new ControlDraw();
                draw.PenColor = Color.Yellow;
                draw.DrawRectangleF = zs;
                draw.ShapeType = ShapeType;
                controlDraws.Add(draw);

                if (TopRight != null)
                {
                    var spR = new PointF((float)(TopRight.X - EllipseDiameter / 2), (float)(TopRight.Y - EllipseDiameter / 2));
                    var zsR = new RectangleF(spR, sz);
                    ControlDraw drawR = new ControlDraw();
                    drawR.PenColor = Color.Yellow;
                    drawR.DrawRectangleF = zsR;
                    drawR.ShapeType = ShapeType;
                    controlDraws.Add(drawR);
                }
                

                var spBl = new PointF((float)(BottomLeft.X - EllipseDiameter / 2), (float)(BottomLeft.Y - EllipseDiameter / 2));
                var zsBl = new RectangleF(spBl, sz);
                ControlDraw drawBl = new ControlDraw();
                drawBl.PenColor = Color.Yellow;
                drawBl.DrawRectangleF = zsBl;
                drawBl.ShapeType = ShapeType;
                controlDraws.Add(drawBl);

                if(BottomRight!=null)
                {
                    var spBr = new PointF((float)(BottomRight.X - EllipseDiameter / 2), (float)(BottomRight.Y - EllipseDiameter / 2));
                    var zsBr = new RectangleF(spBr, sz);
                    ControlDraw drawBr = new ControlDraw();
                    drawBr.PenColor = Color.Yellow;
                    drawBr.DrawRectangleF = zsBr;
                    drawBr.ShapeType = ShapeType;
                    controlDraws.Add(drawBr);
                }
                
            }
            return controlDraws;
        }

        public double EllipseDiameter = 0;

        public double EllipseSpacing = 0;

        public void Zoom(double zoom)
        {
            TopLeft?.Zoom(zoom);
            TopRight?.Zoom(zoom);
            BottomLeft?.Zoom(zoom);
            BottomRight?.Zoom(zoom);
            CenterPoint?.Zoom(zoom);
            EllipseDiameter = JwFileConsts.EllipseDiameter / JwFileConsts.JwScale * zoom;

            EllipseSpacing = JwFileConsts.EllipseSpacing / JwFileConsts.JwScale * zoom;
            Width *= zoom;
            Height *= zoom;
        }

        public void ChangeAxis(double x, double y)
        {
            TopLeft?.ChangeAxis(x, y);
            TopRight?.ChangeAxis(x, y);
            BottomLeft?.ChangeAxis(x, y);
            BottomRight?.ChangeAxis(x, y);
            CenterPoint?.ChangeAxis(x, y);

        }
    }
}
