using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 绘制下柱的类
    /// </summary>
    public class JwDownPillarDraw : ICanZoom, IChangeAxis
    {
        private JwDownPillarMark _downPillarMark;

        public JwDownPillarDraw(JwDownPillarMark downPillarMark)
        {
            var las=new JWPoint(downPillarMark.Line1.Pone.X, downPillarMark.Line1.Pone.Y);
            var lae = new JWPoint(downPillarMark.Line1.Ptwo.X, downPillarMark.Line1.Ptwo.Y);
            var lbs = new JWPoint(downPillarMark.Line2.Pone.X, downPillarMark.Line2.Pone.Y);
            var lbe = new JWPoint(downPillarMark.Line2.Ptwo.X, downPillarMark.Line2.Ptwo.Y);
            _downPillarMark = new JwDownPillarMark
            {
                Line1 = new JwXian(las, lae),
                Line2 = new JwXian(lbs, lbe),
                CenterPoint = new JWPoint(downPillarMark.CenterPoint.X, downPillarMark.CenterPoint.Y),
                HasBeam = downPillarMark.HasBeam,
                HasPillar = downPillarMark.HasPillar,
                IsInBeamCenter = downPillarMark.IsInBeamCenter,
                Id = downPillarMark.Id
            };
        }

        public JwDownPillarDrawModel Change(double zoom, double axisx, double axisy)
        {
            Zoom(zoom);

            ChangeAxis(axisx, axisy);
            JwDownPillarDrawModel re = new JwDownPillarDrawModel();
            re.DownPillarMark = _downPillarMark;
            re.LineAS=_downPillarMark.Line1.Pone.ToPointF();
            re.LineAE=_downPillarMark.Line1.Ptwo.ToPointF();
            re.LineBS=_downPillarMark.Line2.Pone.ToPointF();
            re.LineBE=_downPillarMark.Line2.Ptwo.ToPointF();
            return re;
        }

        public void ChangeAxis(double x, double y)
        {
            _downPillarMark.Line1.Pone.ChangeAxis(x, y);
            _downPillarMark.Line1.Ptwo.ChangeAxis(x, y);
            _downPillarMark.Line2.Pone.ChangeAxis(x, y);
            _downPillarMark.Line2.Ptwo.ChangeAxis(x, y);
        }

        public void Zoom(double zoom)
        {
            _downPillarMark.Line1.Pone.Zoom(zoom);
            _downPillarMark.Line1.Ptwo.Zoom(zoom);
            _downPillarMark.Line2.Pone.Zoom(zoom);
            _downPillarMark.Line2.Ptwo.Zoom(zoom);
            _downPillarMark.CenterPoint.Zoom(zoom);
        }
    }

    public class JwDownPillarDrawModel
    {
        public JwDownPillarMark DownPillarMark { get; set; }
        public PointF LineAS { get; set; }
        public PointF LineAE { get; set; }

        public PointF LineBS { get; set; }
        public PointF LineBE { get; set; }

        public bool IsSelected { get; set; }
    }
}
