using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using JwCore;

namespace JwShapeCommon
{
    public class JwLinkDraw:ICanZoom, IChangeAxis
    {
        private JwLinkPart _part;

        private double width;
        private double height;
        private double tuchuwidth;
        private double tuchuheight;

        private double directOffset = 0;

        public LinkDrawModel linkDrawModel =new LinkDrawModel();

        private JWPoint CenterPoint= new JWPoint();

        public JwLinkDraw(JwLinkPart linkPart) 
        { 
            _part = new JwLinkPart
            {
                Id = Guid.NewGuid().ToString(),
                BujianName =linkPart.BujianName,
                BjCenterPoint=new JWPoint(linkPart.BjCenterPoint.X, linkPart.BjCenterPoint.Y),
                BeamId=linkPart.BeamId,
                ParentBeam=linkPart.ParentBeam,
                Directed = linkPart.Directed,
                GouJianType = linkPart.GouJianType,
                BBeam=linkPart.BBeam,
                IsDownPillar=linkPart.IsDownPillar
            };
            CenterPoint = new JWPoint(linkPart.BjCenterPoint.X, linkPart.BjCenterPoint.Y);
            width = JwFileConsts.GoujianWidth / JwFileConsts.JwScale;
            height = JwFileConsts.GoujianHeight / JwFileConsts.JwScale;
            directOffset = 10 / JwFileConsts.JwScale;
            if (linkPart.GouJianType == GouJianType.BG)
            {
                tuchuwidth=20/JwFileConsts.JwScale;
                tuchuheight = 90 / JwFileConsts.JwScale;
            }

        }
        public LinkDrawModel Change(double zoom, double axisx, double axisy)
        {
            Zoom(zoom);

            ChangeAxis(axisx, axisy);
            return linkDrawModel;
        }

        public void Zoom(double zoom)
        {
            CenterPoint.Zoom(zoom);
            width = width * zoom;
            height=height*zoom;
            tuchuheight = tuchuheight * zoom;
            tuchuwidth=tuchuwidth * zoom;
            directOffset=directOffset * zoom;
        }

        public void ChangeAxis(double x, double y)
        {
            LinkDrawModel linkDraw = new LinkDrawModel();
            linkDraw.Polygon = new List<PointF>();
            linkDraw.Bounds = new List<RectangleF>();
            linkDraw.LinkPart = _part;
            CenterPoint.ChangeAxis(x, y);
            if (_part.GouJianType == GouJianType.B)
            {
                if (_part.Directed == TaggDirect.Up)
                {
                    PointF p1 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y - (float)directOffset);
                    PointF p2 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y - (float)directOffset - (float)height);
                    PointF p3 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y - (float)directOffset);
                    PointF p4 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y - (float)directOffset - (float)height);
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4);
                    linkDraw.Bounds.Add(new RectangleF(p2,new SizeF((float)width, (float)height)));
                }
                if (_part.Directed == TaggDirect.Down)
                {
                    PointF p1 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y + (float)directOffset);
                    PointF p2 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y + (float)directOffset + (float)height);
                    PointF p3 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y + (float)directOffset);
                    PointF p4 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y + (float)directOffset + (float)height);
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4);
                    linkDraw.Bounds.Add(new RectangleF(p1, new SizeF((float)width, (float)height)));
                }
                if (_part.Directed == TaggDirect.Left)
                {
                    PointF p1 = new PointF((float)CenterPoint.X-(float)directOffset, (float)CenterPoint.Y - (float)(width / 2));
                    PointF p2 = new PointF((float)CenterPoint.X - (float)directOffset-(float)height , (float)CenterPoint.Y - (float)(width / 2));
                    PointF p3 = new PointF((float)CenterPoint.X - (float)directOffset, (float)CenterPoint.Y + (float)(width / 2));
                    PointF p4 = new PointF((float)CenterPoint.X - (float)directOffset - (float)height , (float)CenterPoint.Y + (float)(width / 2));
                    PointF p5 = new PointF((float)CenterPoint.X - (float)directOffset, (float)CenterPoint.Y - (float)(width / 2));
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4);
                    linkDraw.Polygon.Add(p5);
                    linkDraw.Bounds.Add(new RectangleF(p2, new SizeF((float)height, (float)width)));
                }
                if (_part.Directed == TaggDirect.Right)
                {
                    PointF p1 = new PointF((float)CenterPoint.X + (float)directOffset, (float)CenterPoint.Y - (float)(width / 2));
                    PointF p2 = new PointF((float)CenterPoint.X + (float)directOffset + (float)(height / 2), (float)CenterPoint.Y - (float)(width / 2));
                    PointF p3 = new PointF((float)CenterPoint.X + (float)directOffset, (float)CenterPoint.Y + (float)(width / 2));
                    PointF p4 = new PointF((float)CenterPoint.X + (float)directOffset + (float)(height / 2), (float)CenterPoint.Y + (float)(width / 2));
                    PointF p5 = new PointF((float)CenterPoint.X + (float)directOffset, (float)CenterPoint.Y - (float)(width / 2));
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4);
                    linkDraw.Polygon.Add(p5);
                    linkDraw.Bounds.Add(new RectangleF(p1, new SizeF((float)height, (float)width)));
                }
            }
            if (_part.GouJianType == GouJianType.BG)
            {
                if (_part.Directed == TaggDirect.Up)
                {
                    PointF p1 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y - (float)directOffset);
                    PointF p2 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y - (float)directOffset - (float)height);
                    PointF p3 = new PointF((float)CenterPoint.X - (float)(tuchuwidth / 2), (float)CenterPoint.Y - (float)directOffset - (float)height);
                    PointF p4 = new PointF((float)CenterPoint.X - (float)(tuchuwidth / 2), (float)CenterPoint.Y - (float)directOffset - (float)height-(float)tuchuheight);
                    PointF p5 = new PointF((float)CenterPoint.X + (float)(tuchuwidth / 2), (float)CenterPoint.Y - (float)directOffset - (float)height - (float)tuchuheight);
                    PointF p6 = new PointF((float)CenterPoint.X + (float)(tuchuwidth / 2), (float)CenterPoint.Y - (float)directOffset - (float)height);
                    PointF p7 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y - (float)directOffset);
                    PointF p8 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y - (float)directOffset - (float)height);
                    PointF p9 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y - (float)directOffset);
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4);linkDraw.Polygon.Add(p5);linkDraw.Polygon.Add(p6);
                    linkDraw.Polygon.Add(p7);
                    linkDraw.Polygon.Add(p8);
                    linkDraw.Polygon.Add(p9);
                    linkDraw.Bounds.Add(new RectangleF(p2, new SizeF((float)width, (float)height)));
                    linkDraw.Bounds.Add(new RectangleF(p4, new SizeF((float)tuchuwidth, (float)tuchuheight)));
                }
                if (_part.Directed == TaggDirect.Down)
                {
                    PointF p1 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y + (float)directOffset);
                    PointF p2 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y + (float)directOffset + (float)height);
                    PointF p3 = new PointF((float)CenterPoint.X - (float)(tuchuwidth / 2), (float)CenterPoint.Y + (float)directOffset + (float)height);
                    PointF p4 = new PointF((float)CenterPoint.X - (float)(tuchuwidth / 2), (float)CenterPoint.Y + (float)directOffset + (float)height + (float)tuchuheight);
                    PointF p5 = new PointF((float)CenterPoint.X + (float)(tuchuwidth / 2), (float)CenterPoint.Y + (float)directOffset +(float)height +(float)tuchuheight);
                    PointF p6 = new PointF((float)CenterPoint.X + (float)(tuchuwidth / 2), (float)CenterPoint.Y + (float)directOffset + (float)height);
                    PointF p7 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y + (float)directOffset);
                    PointF p8 = new PointF((float)CenterPoint.X + (float)(width / 2), (float)CenterPoint.Y + (float)directOffset + (float)height);
                    PointF p9 = new PointF((float)CenterPoint.X - (float)(width / 2), (float)CenterPoint.Y + (float)directOffset);
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4); linkDraw.Polygon.Add(p5); linkDraw.Polygon.Add(p6);
                    linkDraw.Polygon.Add(p7);
                    linkDraw.Polygon.Add(p8);
                    linkDraw.Polygon.Add(p9);
                    linkDraw.Bounds.Add(new RectangleF(p1, new SizeF((float)width, (float)height)));
                    linkDraw.Bounds.Add(new RectangleF(p3, new SizeF((float)tuchuwidth, (float)tuchuheight)));
                }
                if (_part.Directed == TaggDirect.Left)
                {
                    PointF p1 = new PointF((float)CenterPoint.X - (float)directOffset, (float)CenterPoint.Y - (float)(width / 2));
                    PointF p2 = new PointF((float)CenterPoint.X - (float)directOffset - (float)height , (float)CenterPoint.Y - (float)(width / 2));
                    PointF p3 = new PointF((float)CenterPoint.X - (float)directOffset - (float)(height / 2), (float)CenterPoint.Y - (float)(tuchuwidth / 2));
                    PointF p4 = new PointF((float)CenterPoint.X - (float)directOffset - (float)height - (float)tuchuheight, (float)CenterPoint.Y - (float)(tuchuwidth / 2));
                    PointF p5 = new PointF((float)CenterPoint.X - (float)directOffset - (float)(height / 2 - (float)tuchuheight), (float)CenterPoint.Y + (float)(tuchuwidth / 2));
                    PointF p6 = new PointF((float)CenterPoint.X - (float)directOffset - (float)(height / 2), (float)CenterPoint.Y + (float)(tuchuwidth / 2));
                    PointF p7 = new PointF((float)CenterPoint.X - (float)directOffset, (float)CenterPoint.Y + (float)(width / 2));
                    PointF p8 = new PointF((float)CenterPoint.X - (float)directOffset - (float)(height / 2), (float)CenterPoint.Y + (float)(width / 2));
                    PointF p9 = new PointF((float)CenterPoint.X - (float)directOffset, (float)CenterPoint.Y - (float)(width / 2));
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4); linkDraw.Polygon.Add(p5); linkDraw.Polygon.Add(p6);
                    linkDraw.Polygon.Add(p7);
                    linkDraw.Polygon.Add(p8);
                    linkDraw.Polygon.Add(p9);
                    linkDraw.Bounds.Add(new RectangleF(p2, new SizeF((float)height, (float)width)));
                    linkDraw.Bounds.Add(new RectangleF(p4, new SizeF((float)tuchuheight, (float)tuchuwidth)));
                }
                if (_part.Directed == TaggDirect.Right)
                {
                    PointF p1 = new PointF((float)CenterPoint.X + (float)directOffset, (float)CenterPoint.Y - (float)(width / 2));
                    PointF p2 = new PointF((float)CenterPoint.X + (float)directOffset + (float)(height / 2), (float)CenterPoint.Y - (float)(width / 2));
                    PointF p3 = new PointF((float)CenterPoint.X + (float)directOffset + (float)height , (float)CenterPoint.Y - (float)(tuchuwidth / 2));
                    PointF p4 = new PointF((float)CenterPoint.X + (float)directOffset + (float)(height / 2)+(float)tuchuheight, (float)CenterPoint.Y - (float)(tuchuwidth / 2));
                    PointF p5 = new PointF((float)CenterPoint.X + (float)directOffset + (float)(height / 2) + (float)tuchuheight, (float)CenterPoint.Y + (float)(tuchuwidth / 2));
                    PointF p6 = new PointF((float)CenterPoint.X + (float)directOffset + (float)(height / 2), (float)CenterPoint.Y + (float)(tuchuwidth / 2));
                    PointF p7 = new PointF((float)CenterPoint.X + (float)directOffset, (float)CenterPoint.Y + (float)(width / 2));
                    PointF p8 = new PointF((float)CenterPoint.X + (float)directOffset + (float)(height / 2), (float)CenterPoint.Y + (float)(width / 2));
                    linkDraw.Polygon.Add(p1);
                    linkDraw.Polygon.Add(p2);
                    linkDraw.Polygon.Add(p3);
                    linkDraw.Polygon.Add(p4); linkDraw.Polygon.Add(p5); linkDraw.Polygon.Add(p6);
                    linkDraw.Polygon.Add(p7);
                    linkDraw.Polygon.Add(p8);
                    linkDraw.Bounds.Add(new RectangleF(p1, new SizeF((float)height, (float)width)));
                    linkDraw.Bounds.Add(new RectangleF(p3, new SizeF((float)tuchuheight, (float)tuchuwidth)));
                }
            }
            linkDrawModel = linkDraw;
        }
    }

    public class LinkDrawModel
    {
        public JwLinkPart LinkPart { get; set; }    
        public List<PointF> Polygon { get; set;}

        public List<RectangleF> Bounds { get; set; }
    }
}
