using JwCore;
using JwShapeCommon.Model;
using JwwHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class NewJwBeamJwDraw
    {
        private JwBeam _beam;

        public bool CanDraw = false;

        /// <summary>
        /// 统一为水平，X 从零开始（通过累加 相对距离） Y 分配固定 上面 12 中间0 下面-12 
        /// </summary>
        /// <param name="beam"></param>
        public NewJwBeamJwDraw(JwBeam beam)
        {
            if (beam.DirectionType == BeamDirectionType.Horizontal)
            {
                _beam = beam;
            }
            else
            {
                _beam = JwShapeHelper.VerticalToHorizontal(beam);
                //    JwDrawShape beamsp = new JwDrawShape(verticalbeam);
                //    controls.AddRange(beamsp.Change(_minbeilv, axisX, axisY));
            }
            _beam.AbsolutePD= Math.Round(beam.TopLeft.X, 6);//不用更改数据库从新生成间隔数据

            var xmark=_beam.jwBeamMarks.Find(t=>t.IsCenterStart);
            if (xmark != null)
            {
                CanDraw = true;
                offsetX = -xmark.Coordinate;//芯起点默认为0 重置各类坐标  
            }
            //默认芯起点为00 则可以直接使用predistance来生成孔组位置
            //这种情况下offset就为 负 芯起点 
        }

        public List<JwwData> Datas=new List<JwwData>();

        public List<JwwSen> Sens =new List<JwwSen>();

        public List<JwwTen> Tens =new List<JwwTen>();

        public List<JwwEnko> Enkos =new List<JwwEnko>();

        public List<JwwMoji> Mojis = new List<JwwMoji>();

        public List<JwwSunpou> Biaozhu=new List<JwwSunpou>();

        private double offsetX = 0;
        private double offsetY = 0;
        private double offsetMsg = 0;

        private double centerHeight = 0;

        private double jianju = 0;

        /// <summary>
        /// 头心的x坐标
        /// </summary>
        private double xstartx = 0;

        private double startx = 0;

        /// <summary>
        /// 尾心的x坐标
        /// </summary>
        private double xendx = 0;

        private double endx = 0;

        private double banjing = 0;

        public void Draw()
        {
            if(_beam!=null)
            {
                banjing = JwFileConsts.EllipseDiameter / (2*JwFileConsts.JwScale);
                offsetX = -_beam.CenterPoint.X;
                offsetY = -_beam.CenterPoint.Y;
                centerHeight = 2 * (Math.Min(_beam.Width, _beam.Height));

                xstartx = _beam.TopLeft.X + 50/JwFileConsts.JwScale+offsetX;//右移
                xendx = _beam.TopRight.X - 50 / JwFileConsts.JwScale + offsetX;
                startx = _beam.TopLeft.X + offsetX;
                endx = _beam.TopRight.X + offsetX;
                if (_beam.HasStartSide)
                {
                    if (_beam.StartTelosType == KongzuType.G)
                    {
                        xstartx = _beam.TopLeft.X + offsetX - 55 / JwFileConsts.JwScale;
                    }
                    if (_beam.StartTelosType == KongzuType.J)
                    {
                        xstartx = _beam.TopLeft.X + offsetX - 3 / JwFileConsts.JwScale;
                    }
                }
                if (_beam.HasEndSide)
                {
                    if (_beam.EndTelosType == KongzuType.G)
                    {
                        xendx = _beam.TopRight.X + offsetX + 55 / JwFileConsts.JwScale;
                    }
                    if (_beam.EndTelosType == KongzuType.J)
                    {
                        xendx = _beam.TopRight.X + offsetX + 3 / JwFileConsts.JwScale;
                    }
                }
                jianju = 6 * centerHeight;

                offsetMsg = 2 * centerHeight;

                var offsetTY = offsetY + jianju;
                var offsetBY = offsetY - jianju;
                CreateBeam(offsetX, offsetY,KongzuSuoshuMian.Center);//中间
                CreateBeam(offsetX, offsetTY,KongzuSuoshuMian.Top);//shangmian
                CreateBeam(offsetX, offsetBY,KongzuSuoshuMian.Bottom);//xiamian
                Datas.AddRange(this.Sens);
                Datas.AddRange(this.Biaozhu);
                Datas.AddRange(this.Tens);
                Datas.AddRange(this.Enkos);
                Datas.AddRange(this.Mojis);
            }
        }

        private void CreateBeam(double offsetx,double offsety,KongzuSuoshuMian iscenter)
        {
            var hodu = JwFileConsts.Lianghoudu / JwFileConsts.JwScale;
            double ctys = _beam.CenterPoint.Y + hodu / 2 + offsety;
            double ctyx = _beam.CenterPoint.Y - hodu / 2 + offsety;
            if (iscenter==KongzuSuoshuMian.Center)
            {
                JwwSen leftline = CreateSen(_beam.TopLeft.X + offsetx, 2 * (_beam.TopLeft.Y + offsety), _beam.BottomLeft.X + offsetx, 2*(_beam.BottomLeft.Y + offsety));
                Sens.Add(leftline);
                JwwSen rightline = CreateSen(_beam.TopRight.X + offsetx, 2 * (_beam.TopRight.Y + offsety), _beam.BottomRight.X + offsetx, 2 * (_beam.BottomRight.Y + offsety));
                Sens.Add(rightline);
                JwwSen topline = CreateSen(_beam.TopLeft.X + offsetx, 2 * (_beam.TopLeft.Y + offsety), _beam.TopRight.X + offsetx, 2 * (_beam.TopRight.Y + offsety));
                Sens.Add(topline);
                JwwSen bottomline = CreateSen(_beam.BottomRight.X + offsetx, 2 * (_beam.BottomRight.Y + offsety), _beam.BottomLeft.X + offsetx, 2 * (_beam.BottomLeft.Y + offsety));
                Sens.Add(bottomline);
                JwwSen topfz = CreateSen(topline.m_start_x, topline.m_start_y - hodu, topline.m_end_x, topline.m_end_y - hodu,true);
                Sens.Add(topfz);
                JwwSen bottomfz = CreateSen(bottomline.m_start_x, bottomline.m_start_y + hodu, bottomline.m_end_x, bottomline.m_end_y + hodu, true);
                Sens.Add(bottomfz);
                CreateMsgAndHole(offsetx, offsety,leftline.m_start_y+offsetMsg,iscenter);
                double shumsgx = leftline.m_start_x - 100 / JwFileConsts.JwScale;
                double shumsgxe= leftline.m_start_x - 50 / JwFileConsts.JwScale;
                createsinglemsg(shumsgx, shumsgxe, leftline.m_start_y, leftline.m_end_y, false);
                JwwMoji duan=new JwwMoji();
                duan.m_start_x = shumsgx - 200 / JwFileConsts.JwScale;
                duan.m_start_y = (leftline.m_start_y + leftline.m_end_y) * 3 / 4;
                duan.m_string = _beam.HasStartSide ? _beam.StartTelosType.ToString() : "B";
                duan.m_dSizeX = 200 / JwFileConsts.JwScale;
                duan.m_dSizeY = 200 / JwFileConsts.JwScale;
                Mojis.Add(duan);
                double endduanx = topline.m_end_x + 300 / JwFileConsts.JwScale;
                JwwMoji duanend = new JwwMoji();
                duanend.m_start_x = endduanx;
                duanend.m_start_y = (leftline.m_start_y + leftline.m_end_y) * 3 / 4;
                duanend.m_string = _beam.HasEndSide ? _beam.EndTelosType.ToString() : "B";
                duanend.m_dSizeX = 200 / JwFileConsts.JwScale;
                duanend.m_dSizeY = 200 / JwFileConsts.JwScale;
                Mojis.Add(duanend);

            }
            else
            {
                JwwSen leftline = CreateSen(_beam.TopLeft.X + offsetx, _beam.TopLeft.Y + offsety, _beam.BottomLeft.X + offsetx, _beam.BottomLeft.Y + offsety);
                Sens.Add(leftline);
                JwwSen rightline = CreateSen(_beam.TopRight.X + offsetx, _beam.TopRight.Y + offsety, _beam.BottomRight.X + offsetx, _beam.BottomRight.Y + offsety);
                Sens.Add(rightline);
                JwwSen topline = CreateSen(_beam.TopLeft.X + offsetx, _beam.TopLeft.Y + offsety, _beam.TopRight.X + offsetx, _beam.TopRight.Y + offsety);
                Sens.Add(topline);
                JwwSen bottomline = CreateSen(_beam.BottomRight.X + offsetx, _beam.BottomRight.Y + offsety, _beam.BottomLeft.X + offsetx, _beam.BottomLeft.Y + offsety);
                Sens.Add(bottomline);
                JwwSen fushang = CreateSen(_beam.TopLeft.X + offsetx, ctys, _beam.TopRight.X + offsetx, ctys,true);
                Sens.Add(fushang);
                JwwSen fuxia = CreateSen(_beam.TopLeft.X + offsetx, ctyx, _beam.TopRight.X + offsetx, ctyx,true);
                Sens.Add(fuxia);
                CreateMsgAndHole(offsetx, offsety, leftline.m_start_y + offsetMsg, iscenter);
            }
            if (iscenter == KongzuSuoshuMian.Bottom)
            {
                //梁长度 标识
                createsinglemsg(_beam.BottomLeft.X + offsetx, _beam.BottomRight.X + offsetx, _beam.BottomRight.Y - offsetMsg + offsety, _beam.CenterPoint.Y + offsety);
                //中心线长度
                createsinglemsg(xstartx, xendx , _beam.BottomRight.Y - offsetMsg*3/2 + offsety, _beam.CenterPoint.Y + offsety);
            }
            
        }

        private void CreateMsgAndHole(double offsetx,double offsety,double msgy, KongzuSuoshuMian mian)
        {
            List<JwHole> hole = new List<JwHole>();
            if (mian == KongzuSuoshuMian.Top)
            {
                hole=_beam.Holes.Where(t=>t.HasTop).OrderBy(t=>t.Location.X).ToList();
            }
            if(mian==KongzuSuoshuMian.Center)
            {
                hole = _beam.Holes.Where(t => t.HasCenter).OrderBy(t => t.Location.X).ToList();
            }
            if (mian == KongzuSuoshuMian.Bottom)
            {
                hole = _beam.Holes.Where(t => t.HasBottom).OrderBy(t => t.Location.X).ToList();
            }
            var xs = hole.Select(t => t.Location.X+offsetx).ToList();
            var thisbeamcentery = (_beam.TopLeft.Y + _beam.BottomLeft.Y)/2 + offsety;
            createMsgs(xs, thisbeamcentery,msgy,startx,endx,xstartx,xendx);
            createHoles(hole, offsetx, offsety);
        }

        private void createHoles(List<JwHole> holes,double offsetx,double offsety)
        {
            foreach(var  hole in holes) 
            {
                hole.createTBLF();
                createhole(hole.TopLeft.X + offsetx, hole.TopLeft.Y + offsety);
                createhole(hole.BottomLeft.X + offsetx, hole.BottomLeft.Y + offsety);
                if (hole.KongNum == 4)
                {
                    createhole(hole.TopRight.X + offsetx, hole.TopRight.Y + offsety);
                    createhole(hole.BottomRight.X + offsetx, hole.BottomRight.Y + offsety);
                }
            }
        }

        private void createhole(double x,double y)
        {
            JwwEnko enko=new JwwEnko();
            enko.m_dHankei = banjing;
            enko.m_radKaishiKaku = 0;
            enko.m_radEnkoKaku = 6.2831853;
            enko.m_radKatamukiKaku = 0;
            enko.m_dHenpeiRitsu = 1;
            enko.m_bZenEnFlg = 1;
            enko.m_start_x= x;
            enko.m_start_y= y;
            Enkos.Add(enko);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="sy">center y</param>
        /// <param name="msgy">标注高点</param>
        /// <param name="beamleftx"></param>
        /// <param name="beamrightx"></param>
        /// <param name="beamxleftx"></param>
        /// <param name="beamxrightx"></param>
        private void createMsgs(List<double> xs,double sy,double msgy,double beamleftx,double beamrightx,double beamxleftx,double beamxrightx)
        {
            if (xs.Count>0)
            {
                var blsx = beamleftx;
                
                var blsxx = beamxleftx;

                createsingleTen(blsxx, msgy);
                foreach (var x in xs)
                {
                    if(blsxx!=x) {
                        createsingleTen(x, msgy);//创建点
                        createsinglemsg(blsxx, x, msgy,sy);//创建寸法 标注
                        blsxx = x;
                    }
                    
                }
                if(blsxx!=beamxrightx)
                {
                    createsingleTen(beamxrightx, msgy);
                    createsinglemsg(blsxx,beamxrightx,msgy, sy);
                }
            }
        }


        private void createsinglemsg(double x1,double x2, double y,double sy,bool showx=true)
        {
            JwwSen sen1=new JwwSen();
            sen1.m_nPenStyle = 2;
            sen1.m_nPenColor = 4;
            sen1.m_start_x = x1;
            JwwSunpou sunpou = new JwwSunpou();
            //sunpou.m_nPenColor = 2;
            //sunpou.m_nPenStyle = 1;
            sunpou.m_Sen.m_nPenColor = 2;
            sunpou.m_Sen.m_nPenStyle = 2;
            JwwSen sen2 = new JwwSen();
            sen2.m_nPenStyle = 2;
            sen2.m_nPenColor = 4;
            if (showx)
            {
                sen1.m_end_x = x1;
                sen1.m_end_y = y;
                sen2.m_start_x = x2;
                sen2.m_start_y = sy;
                sunpou.m_Sen.m_end_x = x2;
                sunpou.m_Sen.m_start_y = y;
                sunpou.m_Moji.m_string = (Math.Round((x2 - x1) * JwFileConsts.JwScale, 2)).ToString();
            }
            else
            {
                sen1.m_end_x = x2;
                sen1.m_end_y = sy;
                sen2.m_start_x = x1;
                sen2.m_start_y = y;
                sunpou.m_Sen.m_end_x = x1;
                sunpou.m_Sen.m_start_y = sy;
                sunpou.m_Moji.m_string = (Math.Round((y - sy) * JwFileConsts.JwScale, 2)).ToString();
            }
            sen1.m_start_y = sy;
            
            this.Sens.Add(sen1);
            sen2.m_end_x = x2;
            sen2.m_end_y = y;
            this.Sens.Add(sen2);
           
            sunpou.m_Sen.m_end_y = y;
            
            sunpou.m_Sen.m_start_x = x1;
            sunpou.m_Moji.m_start_x = x1;
            sunpou.m_Moji.m_start_y = y;
            sunpou.m_Moji.m_dSizeX = 0.5;
            sunpou.m_Moji.m_dSizeY = 0.5;
            //sunpou.m_Moji.
            Biaozhu.Add(sunpou);
        }

        private void createsingleTen(double x, double y)
        {
            JwwTen ten=new JwwTen();
            ten.m_start_x=x;
            ten.m_start_y=y;
            Tens.Add(ten);
        }

        private JwwSen CreateSen(double x1,double y1,double x2,double y2,bool isfuzhu=false)
        {
            JwwSen sen = new JwwSen();
            if(isfuzhu)
            {
                sen.m_nPenStyle =  2;
            }
            else
            {
                sen.m_nPenStyle = 1;
            }
            sen.m_nPenColor = 3;
            sen.m_start_x = x1;
            sen.m_end_x= x2;
            sen.m_start_y = y1;
            sen.m_end_y = y2;
            return sen;
        }
    }
}
