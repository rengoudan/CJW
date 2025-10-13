using JwCore;
using JwShapeCommon.Model;
using JwwHelper;
using Microsoft.VisualBasic;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Strtree;
using NetTopologySuite.Triangulate;
using Newtonsoft.Json.Linq;
using Sunny.UI;
using Sunny.UI.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JwShapeCommon
{
    public class JwFileHandle
    {

        #region 私有共有变量
        /// <summary>
        /// 左上
        /// </summary>
        public JWPoint? TopLeft { get; set; }

        /// <summary>
        /// 右上
        /// </summary>
        public JWPoint? TopRight { get; set; }

        /// <summary>
        /// 左下
        /// </summary>
        public JWPoint? BottomLeft { get; set; }

        /// <summary>
        /// 右下
        /// </summary>
        public JWPoint? BottomRight { get; set; }

        public double? Width { get; set; }
        public double? Height { get; set; }

        /// <summary>
        /// 已识别的所有点 记录
        /// </summary>
        public List<JWPoint> JwAllPoints = new List<JWPoint>();

        private string _path;

        public List<JwwSen> SenLst = new List<JwwSen>();

        public List<JwwSen> ParseSenLst = new List<JwwSen>();

        /// <summary>
        /// br 组件的交叉线
        /// </summary>
        public List<JwwSen> BRSenLst = new List<JwwSen>();

        /// <summary>
        /// 连接线集合
        /// </summary>
        public List<JwwSen> LianjieLst = new List<JwwSen>();

        public List<JwwSolid> SolidLst = new List<JwwSolid>();

        public List<JwwBlock> JWWBlockLst = new List<JwwBlock>();

        /// <summary>
        /// 柱子识别方法
        /// </summary>
        public List<JwwSolid> ParseSolidLst = new List<JwwSolid>();

        /// <summary>
        /// 识别设计图内的block
        /// </summary>
        public List<JwwBlock> _tempBlocks = new List<JwwBlock>();

        /// <summary>
        /// 切割处理方法
        /// </summary>
        public List<JwwSolid> QieGeSolidLst = new List<JwwSolid>();

        public List<JwwMoji> MojiLst = new List<JwwMoji>();

        public List<JwwMoji> ParseMojiLst = new List<JwwMoji>();

        public List<JWMian> Mians { get; set; }
        private List<JWMian> _tempmians { get; set; }

        private List<JwXian> beamxians { get; set; }

        private List<JwXian> brXians { get; set; }

        private List<JWPoint> beampoints { get; set; }

        private List<string> beamxiantempids;
        private List<string> beamxianIds;

        /// <summary>
        /// 初次识辨出的块
        /// </summary>
        public List<JwBlock> _tempblocks { get; set; }

        public List<JwBlock> _yichublocks { get; set; }

        /// <summary>
        /// 合并后的矩形 block
        /// </summary>
        public List<JwBlock> RectangleBlocks { get; set; }

        /// <summary>
        /// 识别出的柱子
        /// </summary>
        public List<JwPillar> Pillars { get; set; }


        public bool IsSuccessRead { get; set; }



        private JwProjectMainData _projectMaindata;

        private string _floorName;

        public JwwHeader _jwwHeader { get; set; }

        private double _scale { get; set; }

        public int? MarkBeam { get; set; }

        private List<JwBlock> _pillaredothersanjiao = new List<JwBlock>();

        /// <summary>
        /// 判断 切割和柱是否同意颜色 样式
        /// </summary>
        private bool pillarsplitcolorxt = false;

        private bool pillarsplitstylext = false;

        public List<JwwSen> QuchongSenlst;

        public List<JwBeam> _tempBeams=new List<JwBeam>();

        public List<JwBeam> ParentQieGeBeam = new List<JwBeam>();

        public List<JwBeam> Beams=new List<JwBeam>();

        public int BeamsCount = 0;

        public int HorizontalBeamsCount = 0;

        public int VerticalBeamsCount = 0;

        public List<JwBeam> HorizontalBeams;

        public List<JwBeam> VerticalBeams;

        public List<double> RowsPointY { get; set; }

        public List<double> ColumnPointX { get; set; }

        public int BBCount { get; set; }

        public int BGCount { get; set; }

        /// <summary>
        /// 是否识别到beam
        /// </summary>
        public bool HasBeam = false;

        List<string> tempsquareid;
        /// <summary>
        /// 柱子总数
        /// </summary>
        public int PillarCount { get; set; }

        public int SinglePillarCount { get; set; }

        public int KPillarCount { get; set; }

        public bool HasPillar { get; set; }

        public List<JwTagg> _jwwmojitaggs = new List<JwTagg>();

        private List<JwDirected> _tempDirected = new List<JwDirected>();
        public List<JwDirected> Directeds;
        public bool HasCanvas { get; set; }

        public JwCanvas jwCanvas { get; set; }

        public JwProjectSubData? _subData { get; set; }

        public List<JwBeamData> _beamdatas = new List<JwBeamData>();

        public List<JwPillarData> _beampillarDatas = new List<JwPillarData>();

        public List<JwLinkPartData> _linkPartDatas = new List<JwLinkPartData>();

        public List<JwLianjieData> _lianjieDatas = new List<JwLianjieData>();

        public List<JwLinkPart> AllLinkPart = new List<JwLinkPart>();

        public List<JwLinkPart> IndependentLinkPart = new List<JwLinkPart>();

        public List<JwBeam> _baifanglianjie = new List<JwBeam>();

        public int GouJianZongshu { get; set; }
        #endregion

        #region 构造函数
        public JwFileHandle()
        {
            GlobalEvent.GetGlobalEvent().AddLinkPartEvent += AddLinkPartEvent;
        }

        public JwFileHandle(string path)
        {
            _path = path;
            GlobalEvent.GetGlobalEvent().AddLinkPartEvent += AddLinkPartEvent;
        }

        public JwFileHandle(JwProjectPathModel model)
        {
            _path = model.Path;
            _projectMaindata = model.MainData;
            _floorName = model.FloorName;
            GlobalEvent.GetGlobalEvent().AddLinkPartEvent += AddLinkPartEvent;
        }
        #endregion

        #region 读取

        public void ReadJwFile()
        {
            try
            {
                if (string.IsNullOrEmpty(_path))
                {
                    IsSuccessRead = false;
                    return;
                }
                SendMsg("jwファイルの読み込みを開始します");
                using (var reader = new JwwHelper.JwwReader())
                {
                    reader.Read(_path, Completed);
                }

            }
            catch (Exception ex)
            {
                IsSuccessRead = false;
            }


        }

        private void Completed(JwwReader A_0)
        {
            //AreadyRead = A_0;
            var header = A_0.Header;
            _scale = header.m_adScale[0];//暂时取第一个 
            SendMsg("scale : 1/" + _scale.ToString());
            JwFileConsts.JwScale = _scale;
            IsSuccessRead = true;
            ParseBySetting(A_0);//对文件读取内容处理 初步处理
        }

        #endregion

        private void SendMsg(string msg,bool st=false)
        {
            if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
            {
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs
                {
                    Msg = msg,
                    ShowTime = st,
                    UpdateTime = DateTime.Now
                });
            }
        }


        /// <summary>
        /// frist 先识别beam 然后识别柱子  柱子根据梁的范围进行确认
        /// 2025年4月6日 createBeamAbsolutePD();
        /// </summary>
        public void FollowTheStep()
        {
            
            ChangeJwXianFromJwwSen();//jwwSen到jwxian处理
            JudgmentJieChu();
           

            if (Beams == null || Beams.Count == 0)
            {
                var errbeamstr = string.Format("{0}番の色ではビームを識別できません", JwFileConsts.BeamParseColor.ColorNumber);
                SendMsg(errbeamstr);
            }

            ChangePillarFromJwwSolid();//处理文件solid
            ChangeQieGeSolis();//读取切割点内容
            CreateQieGeBeams();//生成具体的beam
            ParseSquareCreatePillar();//对处处理的pillar 精加工
            judgePillarBeam();
            //parsePillarBeam();
            panduanBlockGuishu();
            //Thread.Sleep(3000);
            ChangeJwwEnojiToText();//文件内文字处理 待完善
            SecondExtendTextPillar();
            ThirdExtendTextPillar();
            LastTextPillar();
            parseDownPillars();
            StatisticalBBGquantity();
            if (Pillars==null|| Pillars.Count == 0)
            {
                var errbeamstr = string.Format("色番号{0}は柱を識別できません", JwFileConsts.BeamPillarParseColor.ColorNumber);
                SendMsg(errbeamstr);
            }
            panduanBeamduankou();
            var qqqq = _jwwmojitaggs;
            var qqqqq = Pillars.Where(t => t.HasTag).ToList();

            if (HasBeam)
            {
                foreach (var item in Beams)
                {
                    SendMsg(item.ToString() + Environment.NewLine);
                }
            }

            //生成beam 的每个标记位置点

            //createBeamAbsolutePD();

            Revision();

            parsenLianjie();//寻找连接线
        }

        int nownumber = -1;

        /// <summary>
        /// 需要增加 多种块类型
        /// </summary>
        public void ParseBySetting(JwwReader jwwReader)
        {
            //
            foreach (var s in jwwReader.DataListList)
            {
                _dictionarytempblocklst.Add(s.m_nNumber, new List<JwwSolid>());
                nownumber = s.m_nNumber;
                s.EnumerateDataList(jwblockread);
            }

            var dataList = jwwReader.DataList;
            foreach(var data in dataList)
            {
                string typename = data.GetType().Name;
                if (typename == "JwwSen")//线
                {
                    var sen = data as JwwSen;
                    //if(sen?.m_nPenColor==JwFileConsts.BeamParseColor.ColorNumber)
                    //{
                    //    ParseSenLst.Add(sen);
                    //}
                    SenLst.Add(sen);
                }
                if (typename == "JwwSolid")//块
                {
                    var solid=data as JwwSolid;
                    SolidLst.Add(solid);
                }
                if (typename == "JwwMoji")//文字
                {
                    var moji=data as JwwMoji;
                    MojiLst.Add(moji);
                }
                if (typename == "JwwBlock")
                {
                    var block = data as JwwBlock;
                    JWWBlockLst.Add(block);
                }
            }
            
            if(SenLst.Count > 0)
            {
                ParseSenLst = SenLst.Where(t => t.m_nPenColor == JwFileConsts.BeamParseColor.ColorNumber).ToList();
                //BRSenLst = SenLst.Where(t => t.m_nPenColor == JwFileConsts.BRParseColore && t.m_nPenStyle == JwFileConsts.BRParseStyle).ToList();
                BRSenLst = SenLst.Where(t => t.m_nPenColor == JwFileConsts.BRParseColore).ToList();

                //2025年5月20日
                LianjieLst = SenLst.Where(t => t.m_nPenColor == JwFileConsts.LianjieParseColor.ColorNumber).ToList();


            }

            if(SolidLst.Count > 0)
            {
                ParseSolidLst = SolidLst;
                QieGeSolidLst = SolidLst;
                pillarsplitcolorxt = JwFileConsts.BeamPillarParseColor.ColorNumber == JwFileConsts.BeamSplitParseColor.ColorNumber;
                pillarsplitstylext = JwFileConsts.PillarPenStyle.StyleNumber == JwFileConsts.SplitPenStyle.StyleNumber;
                //if (JwFileConsts.PillarPenStyle!=null)
                //{
                //    ParseSolidLst = ParseSolidLst.Where(t => t.m_nPenStyle == Convert.ToByte(JwFileConsts.PillarPenStyle.StyleNumber)).ToList();
                //}
                if(JwFileConsts.BeamPillarParseColor!=null)
                {
                    ParseSolidLst = ParseSolidLst.Where(t => t.m_nPenColor == JwFileConsts.BeamPillarParseColor.ColorNumber).ToList();
                }
                //if (JwFileConsts.SplitPenStyle != null)
                //{
                //    QieGeSolidLst = QieGeSolidLst.Where(t => t.m_nPenStyle == Convert.ToByte(JwFileConsts.SplitPenStyle.StyleNumber)).ToList();
                //}
                if (JwFileConsts.BeamSplitParseColor != null)
                {
                    QieGeSolidLst = QieGeSolidLst.Where(t => t.m_nPenColor == JwFileConsts.BeamSplitParseColor.ColorNumber).ToList();
                }
                //ParseSolidLst = SolidLst.Where(t=>t.m_nPenColor==JwFileConsts.BeamPillarParseColor.ColorNumber).ToList();
                //QieGeSolidLst=SolidLst.Where(t=>t.m_nPenColor==JwFileConsts.BeamSplitParseColor.ColorNumber).ToList();  
            }
            if(MojiLst.Count > 0)
            {
                ParseMojiLst=MojiLst.Where(t=>t.m_nPenColor==JwFileConsts.BeamSymbolTextColor.ColorNumber).ToList();
            }

        }
        Dictionary<int, List<JwwSolid>> _dictionarytempblocklst = new Dictionary<int, List<JwwSolid>>();

        List<JwwData> _tempblocklist = new List<JwwData>();

        bool jwblockread(JwwData jd)
        {
            if (nownumber!=-1)
            {
                string typename = jd.GetType().Name;
                if (typename == "JwwBlock")
                {
                    var bl = jd as JwwBlock;
                    if (_dictionarytempblocklst.Keys.Contains(bl.m_nNumber))
                    {
                        _dictionarytempblocklst[nownumber] = _dictionarytempblocklst[bl.m_nNumber];
                    }
                }
                if (typename == "JwwSolid")
                {
                    if(jd.m_nPenColor == JwFileConsts.BeamPillarParseColor.ColorNumber)
                    {
                        var solid = jd as JwwSolid;
                        _dictionarytempblocklst[nownumber].Add(solid);
                    }
                }
                //_tempblocklist.Add(jd);
                return true;
            }
            else
            {
                return false;
            }
            
        }


        /// <summary>
        /// beam 线处理方法
        /// 2024年9月23日 需要处理重叠线，取合并
        /// </summary>
        private void ChangeJwXianFromJwwSen()
        {
            if(ParseSenLst.Count > 0)
            {
                List<JwXian> tempxians = new List<JwXian>();
                beamxians = new List<JwXian>();
                beampoints = new List<JWPoint>();
                beamxianIds = new List<string>();
                QuchongSenlst = ParseSenLst.Distinct(new JwSenComparint()).ToList();
                foreach (var sen in QuchongSenlst)
                {
                    JWPoint ps = new JWPoint(sen.m_start_x, sen.m_start_y);
                    JWPoint pe = new JWPoint(sen.m_end_x, sen.m_end_y);
                    JwXian j = new JwXian(ps, pe);
                    tempxians.Add(j);
                    //beamxianIds.Add(j.Id);
                    //beamxians.Add(j);
                    //beampoints.AddRange(j.GetXianPoints());
                    //JwAllPoints.Add(ps);
                }
                mergeoverlappingline(tempxians);//
                var l = beamxians;
                beamxians=beamxians.Distinct(new JwXianComparint()).ToList();

                //ConvertXiantoMians();
                // createRectangle();
                FindRectangles(beamxians);

                if (Mians.Count > 0)
                {
                    PareBeamByMian();
                }
            }
        }


        private void FindRectangles(List<JwXian> segments)
        {
            List<List<JwXian>> rectangles = new List<List<JwXian>>();
            Mians = new List<JWMian>();
            // 创建 R-tree 索引
            var rtree = new STRtree<JwXian>();
            foreach (var segment in segments)
            {
                var lineString = segment.ToLineString();
                rtree.Insert(lineString.EnvelopeInternal, segment);
            }
            // 使用 Parallel 并行处理外层循环
            Parallel.For(0, segments.Count, i =>
            {
                var seg1 = segments[i];
                var lineString1 = seg1.ToLineString();

                // 查询与 seg1 直接相交的线段
                var directIntersections = rtree.Query(lineString1.EnvelopeInternal)
                                              .Where(seg => seg.ToLineString().Intersects(lineString1))
                                              .ToList();

                // 遍历与 seg1 直接相交的线段
                foreach (var seg2 in directIntersections)
                {
                    if (seg2.Equals(seg1)) continue; // 跳过自身

                    // 查询与 seg2 相交的其他线段
                    var indirectIntersections = rtree.Query(seg2.ToLineString().EnvelopeInternal)
                                                    .Where(seg => seg.ToLineString().Intersects(seg2.ToLineString()))
                                                    .ToList();

                    // 遍历与 seg2 相交的线段
                    foreach (var seg3 in indirectIntersections)
                    {
                        if (seg3.Equals(seg1) || seg3.Equals(seg2)) continue; // 跳过已处理的线段

                        // 查询与 seg3 相交的其他线段
                        var finalIntersections = rtree.Query(seg3.ToLineString().EnvelopeInternal)
                                                     .Where(seg => seg.ToLineString().Intersects(seg3.ToLineString()))
                                                     .ToList();

                        // 遍历与 seg3 相交的线段
                        foreach (var seg4 in finalIntersections)
                        {
                            if (seg4.Equals(seg1) || seg4.Equals(seg2) || seg4.Equals(seg3)) continue; // 跳过已处理的线段
                            List<JWPoint> points = new List<JWPoint>();
                            //IsRectangle(seg1, seg2, seg3, seg4, out points);
                            // 检查四条线段是否形成矩形
                            if (IsRectangle(seg1, seg2, seg3, seg4, out points))
                            {
                                lock (rectangles)
                                {
                                    var s1 = new JwXian(points[0], points[1]);
                                    var s2=new JwXian(points[1], points[2]);
                                    var s3=new JwXian(points[2],points[3]);
                                    var s4 = new JwXian(points[3], points[0]);
                                    // 避免重复添加矩形
                                    var rectangle = new List<JwXian> { s1, s2, s3, s4 };
                                    bool rexist = rectangles.Any(r => AreRectanglesEqual(r, rectangle));
                                    if (!rexist)
                                    {
                                        JWMian m = new JWMian();
                                        var storlst = rectangle.OrderByDescending(t => t.Distance());
                                        var maxd= rectangle.OrderByDescending(t => t.Distance()).First();
                                        var mind = storlst.Last();
                                        m.Width = maxd.Distance();
                                        m.Height=mind.Distance();
                                        m.Jiaodu= maxd.Jiaodu();
                                        m.Xians = new List<JwXian>(rectangle);
                                        m.Points = points;
                                        Mians.Add(m);
                                        rectangles.Add(rectangle);
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private bool AreRectanglesEqual(List<JwXian> rect1, List<JwXian> rect2)
        {
            int i = rect1.Except(rect2,new JwXianComparint()).Count();
            return i == 0;
            //return rect1.All(seg => rect2.Contains(seg,new JwXianComparint())) && rect2.All(seg => rect1.Contains(seg, new JwXianComparint()));
        }

        private bool IsRectangle(JwXian seg1, JwXian seg2, JwXian seg3, JwXian seg4, out List<JWPoint> interpoints)
        {
            // 获取四条线段的所有交点
            var intersections = new HashSet<JWPoint>()
            {
                GetIntersection(seg1, seg2) ?? new JWPoint(false),
                GetIntersection(seg1, seg3) ?? new JWPoint(false),
                GetIntersection(seg1, seg4) ?? new JWPoint(false),
                GetIntersection(seg2, seg3) ?? new JWPoint(false),
                GetIntersection(seg2, seg4) ?? new JWPoint(false),
                GetIntersection(seg3, seg4) ?? new JWPoint(false)
            };

            // 移除无效的交点（不相交的情况）
            //intersections.RemoveWhere(p => double.IsNaN(p.X) || double.IsNaN(p.Y));
            intersections.RemoveWhere(p => !p.IsValid);
            var points = SortPointsClockwise(new List<JWPoint>(intersections));//按照极坐标排序
            interpoints = points;
            // 如果没有四个交点，则不能组成矩形
            if (intersections.Count != 4)
                return false;
            //return IsRectangleFromPoints(points);
            return true;
            //if(points.Count != 4) 
            //    return false; 
            // 检查是否形成矩形

        }


        /// <summary>
        /// 按照极坐标排序
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private List<JWPoint> SortPointsClockwise(List<JWPoint> points)
        {
            // 计算中心点
            double centerX = points.Average(p => p.X);
            double centerY = points.Average(p => p.Y);

            // 按极角排序
            return points.OrderBy(p => Math.Atan2(p.Y - centerY, p.X - centerX)).ToList();
        }

        private bool IsRectangleFromPoints(List<JWPoint> points)
        {
            points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            var orderedPoints = new List<JWPoint>
        {
            points[0], // 左下角
            points[1], // 右下角
            points[3], // 右上角
            points[2]  // 左上角
        };
            // 计算所有边的向量
            var vectors = new List<JWPoint>
        {
            new JWPoint(orderedPoints[1].X - orderedPoints[0].X, orderedPoints[1].Y - orderedPoints[0].Y),
            new JWPoint(orderedPoints[2].X - orderedPoints[1].X, orderedPoints[2].Y - orderedPoints[1].Y),
            new JWPoint(orderedPoints[3].X - orderedPoints[2].X, orderedPoints[3].Y - orderedPoints[2].Y),
            new JWPoint(orderedPoints[0].X - orderedPoints[3].X, orderedPoints[0].Y - orderedPoints[3].Y)
        };

            // 检查对边是否平行（叉积为零）
            for (int i = 0; i < 2; i++)
            {
                int j = (i + 2) % 4;
                double crossProduct = vectors[i].X * vectors[j].Y - vectors[i].Y * vectors[j].X;
                if (Math.Abs(crossProduct) > 0)
                    return false;
            }
            //// 检查相邻边是否垂直（点积为零）
            //for (int i = 0; i < 4; i++)
            //{
            //    int j = (i + 1) % 4;
            //    double dotProduct = vectors[i].X * vectors[j].X + vectors[i].Y * vectors[j].Y;
            //    if (Math.Abs(dotProduct) > 1e-10)
            //        return false;
            //}
            return true;
        }

        private JWPoint? GetIntersection(JwXian seg1, JwXian seg2)
        {
            // 计算两条线段的交点
            double x1 = seg1.Pone.X*1000000, y1 = seg1.Pone.Y * 1000000;
            double x2 = seg1.Ptwo.X * 1000000, y2 = seg1.Ptwo.Y * 1000000;
            double x3 = seg2.Pone.X * 1000000, y3 = seg2.Pone.Y * 1000000;
            double x4 = seg2.Ptwo.X * 1000000, y4 = seg2.Ptwo.Y * 1000000;

            double denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (Math.Abs(denom) < 1e-10) // 平行或重合
                return null;

            //double t = Math.Round(((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denom,0);
            //double u = Math.Round(-((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denom,0);

            //if (t >= 0 && t <= 1 && u >= 0 && u <= 1) // 交点在线段上
            //{
            //    double x = x1 + t * (x2 - x1);
            //    double y = y1 + t * (y2 - y1);
            //    return new JWPoint(x, y);
            //}
            //if (Math.Abs(denom) < 1e-10) // 平行或重合
            //    return null;

            double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denom;
            double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denom;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1) // 交点在线段上
            {
                double x = Math.Round( (x1 + t * (x2 - x1))/1000000,6);
                double y =Math.Round( (y1 + t * (y2 - y1))/1000000,6);
                return new JWPoint(x, y);
            }


            return null; // 交点不在线段上
        }

        /// <summary>
        /// 暂时合并一轮 测试情况
        /// </summary>
        /// <param name="lines"></param>
        //private void mergeoverlappingline(List<JwXian> lines)
        //{
        //    if (lines.Count > 2)
        //    {
        //        for (var i = 0; i < lines.Count; i++)
        //        {
        //            var isadd = true;
        //            for(var j = i+1; j < lines.Count; j++)
        //            {
        //               var q= lines[i].LineOverLapping(lines[j]);
        //                if (q.IsOverLapping)
        //                {
        //                    isadd = false;
        //                    beamxians.Add(q.MergeLine);
        //                    beamxianIds.Add(q.MergeLine.Id);
        //                    beampoints.AddRange(q.MergeLine.GetXianPoints());
        //                    JwAllPoints.AddRange(q.MergeLine.GetXianPoints());
        //                }
        //            }
        //            if (isadd && (!lines[i].IsMerge))
        //            {
        //                beamxians.Add(lines[i]);
        //                beamxianIds.Add(lines[i].Id);
        //                beampoints.AddRange(lines[i].GetXianPoints());
        //                JwAllPoints.AddRange(lines[i].GetXianPoints());
        //            }
        //        }
        //    }
        //    else
        //    {
        //        beamxians = lines;
        //    }
        //}

        private void mergeoverlappingline(List<JwXian> lines)
        {
            if (lines.Count > 2)
            {
                for (var i = 0; i < lines.Count; i++)
                {
                    var isadd = true;
                    for (var j = i + 1; j < lines.Count; j++)
                    {
                        //segment1.OverlapsWith(segment2)
                        var q = lines[i].OverlapsWith(lines[j]);
                        if (q)
                        {
                            isadd = false;
                            JwXian mergedSegment = lines[i].Merge(lines[j]);
                            beamxians.Add(mergedSegment);
                            beamxianIds.Add(mergedSegment.Id);
                            beampoints.AddRange(mergedSegment.GetXianPoints());
                            JwAllPoints.AddRange(mergedSegment.GetXianPoints());
                        }
                    }
                    if (isadd && (!lines[i].IsMerge))
                    {
                        beamxians.Add(lines[i]);
                        beamxianIds.Add(lines[i].Id);
                        beampoints.AddRange(lines[i].GetXianPoints());
                        JwAllPoints.AddRange(lines[i].GetXianPoints());
                    }
                }
            }
            else
            {
                beamxians = lines;
            }
        }
        /// <summary>
        /// BR组件 sen解析处理 2024年9月30日
        /// 构建br类 含 两条相交的 xian
        /// </summary>
        private void brParseBySetting()
        {
            brXians = new List<JwXian>();
            if (BRSenLst.Count > 0)
            {
                foreach (var sen in BRSenLst)
                {
                    JWPoint ps = new JWPoint(sen.m_start_x, sen.m_start_y);
                    JWPoint pe = new JWPoint(sen.m_end_x, sen.m_end_y);
                    JwXian j = new JwXian(ps, pe);
                    brXians.Add(j);
                }
            }
        }

        /// <summary>
        /// 临时beams生成
        /// </summary>
        private void PareBeamByMian()
        {
            Beams = new List<JwBeam>();
            _tempBeams = new List<JwBeam>();
            foreach (var item in Mians)
            {
                _tempBeams.Add(new JwBeam(item,true));
                //_tempBeams.Add(new JwBeam(item));
                //if (item.IsClosedLoop)
                //{
                //    _tempBeams.Add(new JwBeam(item));
                //}

            }
            //_tempBeams= _tempBeams.Distinct(new beam)
        }

        /// <summary>
        /// 极坐标排序后点 生成beam
        /// </summary>
        private void PareBeamByMian(bool flage)
        {
            Beams = new List<JwBeam>();
            _tempBeams = new List<JwBeam>();
            foreach (var item in Mians)
            {
                _tempBeams.Add(new JwBeam(item));
                //if (item.IsClosedLoop)
                //{
                //    _tempBeams.Add(new JwBeam(item));
                //}

            }
            //_tempBeams= _tempBeams.Distinct(new beam)
        }

        /// <summary>
        /// 第一阶段识别出矩形 solid
        /// 前提柱子为举行 或多个举行组成K型
        /// </summary>
        public void ChangePillarFromJwwSolid()
         {
            _tempblocks = new List<JwBlock>();
            _yichublocks = new List<JwBlock>();
            RectangleBlocks = new List<JwBlock>();
            double beammaxx = JwFileConsts.MaxBeamScope;// 200;
            double beamminx = -JwFileConsts.MaxBeamScope; 
            double beammaxy = JwFileConsts.MaxBeamScope;
            double beamminy = -JwFileConsts.MaxBeamScope;
            List<JWPoint> ptsbeams=new List<JWPoint>();
            if (Beams?.Count > 0)
            {
                Beams.ForEach(t =>
                {
                    ptsbeams.Add(t.TopLeft);
                    ptsbeams.Add(t.TopRight);
                    ptsbeams.Add(t.BottomLeft);
                    ptsbeams.Add(t.BottomRight);
                });
                beammaxx = ptsbeams.Select(t => t.X).Max();
                beamminx = ptsbeams.Select(t => t.X).Min();

                beammaxy = ptsbeams.Select(t => t.Y).Max();
                beamminy = ptsbeams.Select(t => t.Y).Min();
                //RectangleBlocks = RectangleBlocks.Where(t => t.CenterPoint.X>(beamminx-1/_scale)&&t.CenterPoint.X<(beammaxx+1/_scale)&&t.CenterPoint.Y>(beamminy-1/_scale)&&t.CenterPoint.Y<(beammaxy+1/_scale)).ToList();
                //RectangleBlocks = RectangleBlocks.Where(t => t.HasCenter && t.CenterPoint.X > (beamminx) && t.CenterPoint.X < (beammaxx) && t.CenterPoint.Y > (beamminy) && t.CenterPoint.Y < (beammaxy)).ToList();
            }
            foreach (var ps in ParseSolidLst)
            {
                var jb = new JwBlock(ps);
                var max=jb.BlockPoint.Select(t=>t.X).Max();
                var mix = jb.BlockPoint.Select(t => t.X).Min();
                var may=jb.BlockPoint.Select(t=>t.Y).Max();
                var miy = jb.BlockPoint.Select(t => t.Y).Min();
                JwAllPoints.AddRange(jb.BlockPoint);
                //if (_tempBeams.Where(t => t.Contains(jb.CenterPoint)).Count() > 0)
                //{

                //}
                _tempblocks.Add(jb);
               // double wc = 1000 / _scale;
               // if (max <= beammaxx+ wc && mix >= beamminx- wc && may <= beammaxy+ wc && miy >= beamminy- wc)
               // {
               //     _tempblocks.Add(jb);

               // }
               // else
               // {
               //     _yichublocks.Add(jb);
               //}

            }
            if (JWWBlockLst.Count > 0)
            {
                foreach (var bl in JWWBlockLst)
                {
                    int num = bl.m_nNumber;
                    if (_dictionarytempblocklst.Keys.Contains(num))
                    {
                        var zsolidlst = _dictionarytempblocklst[num];
                        if (zsolidlst.Count > 0)
                        {
                            foreach (var solidss in zsolidlst)
                            {
                                var jb = new JwBlock(solidss, bl);
                                //if(jb.)
                                var max = jb.BlockPoint.Select(t => t.X).Max();
                                var mix = jb.BlockPoint.Select(t => t.X).Min();
                                var may = jb.BlockPoint.Select(t => t.Y).Max();
                                var miy = jb.BlockPoint.Select(t => t.Y).Min();
                                JwAllPoints.AddRange(jb.BlockPoint);
                                _tempblocks.Add(jb);
                                //if (_tempBeams.Where(t => t.Contains(jb.CenterPoint)).Count() > 0)
                                //{

                                //}

                                //
                                _tempblocks.Add(jb);

                                //double wc = 200 / _scale;
                                //if (max <= beammaxx + wc && mix >= beamminx - wc && may <= beammaxy + wc && miy >= beamminy - wc)
                                //{
                                //    _tempblocks.Add(jb);
                                //}
                                //else
                                //{
                                //    _yichublocks.Add(jb);
                                //}
                            }
                        }
                    }
                }
            }
            
            List<JwBlock> sanjiaolst = _tempblocks.Where(t => t.ShapeType == JwBlockShapeType.Triangular).ToList();
            List<JwBlock> sanjiaolsts = _tempblocks.Where(t => t.ShapeType == JwBlockShapeType.Triangular).ToList();
            RectangleBlocks.AddRange(_tempblocks.Where(t=>t.ShapeType==JwBlockShapeType.Square).ToList());//添加已经有的矩形
            List<string> tempids = new List<string>();
            if(sanjiaolst.Count>0)
            {
                foreach (var jb in sanjiaolst)
                {

                    if (!tempids.Contains(jb.Id))
                    {
                        //tempids.Add(jb.Id);
                        //暂时忽略3个或者更多三角形拼凑
                        foreach (var q in sanjiaolsts)
                        {
                            if (jb.Id != q.Id)
                            {
                                var xjlst = jb.BlockPoint.Intersect(q.BlockPoint, new JwPointComparint()).ToList();
                                if (xjlst.Count == 2)
                                {
                                    if (!(xjlst[0].X == xjlst[1].X && xjlst[0].Y == xjlst[1].Y))
                                    {
                                        var hebingpoints = jb.BlockPoint.Union(q.BlockPoint, new JwPointComparint()).Distinct(new JwPointComparint()).ToList();
                                        if (hebingpoints.Count == 4)
                                        {
                                            tempids.Add(jb.Id);
                                            tempids.Add(q.Id);
                                            var jbb = new JwBlock(hebingpoints);
                                            //if (_tempBeams.Where(t => t.Contains(jb.CenterPoint)).Count() > 0)
                                            //{
                                            //    RectangleBlocks.Add(jbb);
                                            //}
                                            if (hebingpoints.Count == 4)
                                            {
                                                RectangleBlocks.Add(jbb);
                                            }
                                            //RectangleBlocks.Add(jbb);
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
            //&& pillarsplitstylext
            if (pillarsplitcolorxt )
            {
                var idlstss = tempids.Distinct().ToList();
                var leftlst= sanjiaolsts.Where(t=>!tempids.Contains(t.Id)).ToList();
                if (leftlst != null)
                {
                    _pillaredothersanjiao = leftlst;
                    //_pillaredothersanjiao.AddRange(_yichublocks);
                }
            }

            RectangleBlocks = RectangleBlocks.Where(t => t.ShapeType == JwBlockShapeType.Square).ToList();
            //RectangleBlocks= RectangleBlocks.Distinct(new JwBlockComparint()).ToList();
            //List<JwBlock> tempjbs = new List<JwBlock>();
            //foreach (var t in RectangleBlocks)
            //{
            //    //Mians.Where(t=>t.IsClosedLoop)
            //    if (_tempBeams?.Count > 0)
            //    {
            //        if (_tempBeams.Count(t => t.Contains(t.CenterPoint)) > 0)
            //        {
            //            tempjbs.Add(t);
            //        }
            //    }
            //}
            //RectangleBlocks = tempjbs;
            //foreach (var z in RectangleBlocks)
            //{
            //    var Q = Math.Round((z.TopRight.X - z.TopLeft.X), 0);
            //    var b = Math.Round((z.TopLeft.Y - z.BottomLeft.Y), 0);
            //    if (Q == b)
            //    {
            //        z.Iszhengfangxing = true;
            //    }
            //    z.ZhengfangCenter = new JWPoint
            //    {
            //        X = (z.TopRight.X - z.TopLeft.X) / 2,
            //        Y = (z.TopRight.Y - z.BottomRight.Y) / 2
            //    };

            //}


        }
      
        public void ParseSquareCreatePillar()
        {
            var quchong = RectangleBlocks.Distinct(new JwBlockComparint()).ToList();
            Pillars = new List<JwPillar>();

            List<JwPillar> _temppillars=new List<JwPillar>();
            if (RectangleBlocks.Count > 0)
            {
                
                tempsquareid = new List<string>();
                foreach (var block in quchong)
                {
                    if (!tempsquareid.Contains(block.Id))
                    {
                        JwPillar pillar = new JwPillar();
                        block.ParentPillar= pillar;
                        _temppillars.Add(pillar);
                        pillar.Blocks.Add(block);
                        tempsquareid.Add(block.Id);
                        DiguigetXianglin(block, pillar);
                    }
                }
                PillarCount = _temppillars.Count();
                if (PillarCount > 0)
                {
                    HasPillar = true;
                    int pcout = 0;
                    foreach (var item in _temppillars)
                    {
                        item.Blocks = item.Blocks.Distinct(new JwBlockComparint()).ToList();
                        item.PillarCode = string.Format("{0}-{1}", item.DirectionType.ToString(), pcout + 1);
                        pcout++;
                    }
                }
            }

            if (_temppillars?.Count > 0)
            {
                foreach (var item in _temppillars)
                {
                    item.squareParse();
                }
            }
            var cllst = _temppillars.Where(t => t.BlocksCount > 3).ToList();
            if (cllst.Count > 0)
            {
                foreach (var item in cllst)
                {
                    var xg = item.Blocks.GroupBy(t => t.CenterPoint.X);
                    var yg = item.Blocks.GroupBy(t => t.CenterPoint.Y);
                    int cxg = xg.Count();
                    int cyg= yg.Count();
                    if (cxg > cyg)
                    {
                        if (cyg == 1)
                        {
                            item.Blocks = item.Blocks.Distinct(new JwBlockComparint()).ToList();
                            if (item.Blocks.Count == 3)
                            {
                                item.BaseType = PillarBaseType.KPillar;
                                Pillars.Add(item);
                                tempsquareid.Add(item.Id);
                            }
                            //break;
                        }
                        foreach (var item6 in yg)
                        {
                            List<JwBlock> list5 = item6.Distinct(new JwBlockComparint()).ToList();
                            if (list5.Count == 3)
                            {
                                item.Blocks = list5;
                                item.BaseType = PillarBaseType.KPillar;
                                Pillars.Add(item);
                                tempsquareid.Add(item.Id);
                                break;
                            }
                        }
                    }
                    if (cxg == 1)
                    {
                        item.Blocks = item.Blocks.Distinct(new JwBlockComparint()).ToList();
                        if (item.Blocks.Count == 3)
                        {
                            item.BaseType = (PillarBaseType)1;
                            Pillars.Add(item);
                            tempsquareid.Add(item.Id);
                        }
                    }
                    foreach(var item7 in xg)
                    {
                        List<JwBlock> list6 = item7.Distinct(new JwBlockComparint()).ToList();
                        if (list6.Count == 3)
                        {
                            item.Blocks = list6;
                            item.BaseType = (PillarBaseType)1;
                            Pillars.Add(item);
                            tempsquareid.Add(item.Id);
                            break;
                        }
                    }

                }
            }

            var sinlelst = _temppillars.Where(t => t.BaseType == PillarBaseType.SinglePillar).ToList();
            var klst = _temppillars.Where(t => t.BaseType ==PillarBaseType.KPillar).ToList();
            foreach (var item in sinlelst)
            {
                var z = klst.Where(t => t.Contains(item.CenterPoint)).Count();
                if (!tempsquareid.Contains(item.Id)&&z == 0)
                {
                    Pillars.Add(item);
                }
            }
            foreach (JwPillar item8 in klst)
            {
                if (!tempsquareid.Contains(item8.Id))
                {
                    Pillars.Add(item8);
                }
            }

            //Pillars.AddRange(klst);

            SinglePillarCount = Pillars.Where(t => t.Blocks.Count(q => q.Iszhengfangxing) == 1).Count();
            SendMsg(string.Format("singlepillarcount is {0}{1}", SinglePillarCount, Environment.NewLine));
            KPillarCount = Pillars.Where(t => t.Blocks.Count == 3 && t.Blocks.Count(q => q.Iszhengfangxing) == 2).Count();
            SendMsg(string.Format("KPillarCount is {0}{1}", KPillarCount, Environment.NewLine));
        }

        private void judgePillarBeam()
        {
            if (Pillars?.Count > 0)
            {
                foreach (var pill in Pillars)
                {
                    foreach (var b in _tempBeams)
                    {
                        if (b.ContainsAnyPoint(pill.CenterPoints))
                        {
                            pill.HasBeam = true;
                            break;
                        }
                    }
                }
            }
            //if (Pillars.Count > 0)
            //{
                
            //}
            //_tempBeams
            
        }


        /// <summary>
        /// IsjiechuNotxianglin 去除相邻K柱 导致识别错误问题
        /// 2025年2月28日 进一步细化要求，相邻主要是为了判断K型 
        /// K型的图样决定了 正方形相邻长方形 两者之间中心点距离 应该是K宽的二分之
        /// </summary>
        /// <param name="block"></param>
        /// <param name="pillar"></param>
        private void DiguigetXianglin(JwBlock block, JwPillar pillar)
        {

            foreach (var other in RectangleBlocks)
            {
                if (!tempsquareid.Contains(other.Id))
                {
                    if(block.IsAdjacentTo(other))//2025年3月1日  修改相邻逻辑 根据形状具象化
                    //if(block.IntersectsWith(other))
                    //if (block.IsjiechuNotxianglin(other))
                    {
                        
                        //如果相邻的情况下 再判断中心点的距离
                        //2025年2月28日 增加判断相邻的 中心点距离 
                        tempsquareid.Add(other.Id);
                        if (blockdistanceisright(block, other))
                        {
                            
                            other.ParentPillar = pillar;
                            pillar.Blocks.Add(other);
                            DiguigetXianglin(other, pillar);
                        }
                    }
                }
            }
        }

        private bool blockdistanceisright(JwBlock block,JwBlock other)
        {
            //double ds=JwExtend.Distancewithscale(block.CenterPoint, other.CenterPoint);
            double ds = JwExtend.Distance(block.CenterPoint, other.CenterPoint);


            double wc = JwFileConsts.KErrorMargin / JwFileConsts.JwScale;

            double zc = JwFileConsts.KWidth / (JwFileConsts.JwScale*2);

            if (ds >= zc-wc && ds <= zc+wc)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private void ConvertXiantoMians()
        {
            beamxiantempids = new List<string>();
            Mians = new List<JWMian>();
            //Point 
            foreach (var xian in beamxians)
            {
                if (!xian.IsSelected)
                {
                    if (!beamxiantempids.Contains(xian.Id))
                    {
                        JWMian mian = new JWMian();
                        xian.IsSelected = true;
                        mian.Xians = new List<JwXian>
                    {
                        xian
                    };
                        //Mians.Add(mian);
                        var jweip = Getmian(xian, mian, xian.Pone);
                        if (xian.Ptwo == jweip)
                        {
                            mian.IsClosedLoop = true;
                        }
                        Mians.Add(mian);
                    }
                    //if (!beamxiantempids.Contains(xian.Id))
                    //{
                    //    JWMian mian = new JWMian();
                    //    xian.IsSelected = true;
                    //    mian.Xians = new List<JwXian>
                    //{
                    //    xian
                    //};
                    //    Mians.Add(mian);
                    //    Getmian(xian, mian);
                    //}

                }
            }

            _tempmians = Mians;
            Mians.ForEach(mian => { mian.XianCout = mian.Xians.Count; });

            //foreach(var mobj in Mians)
            //{
            //    var pslst=new List<JWPoint>();
                
            //    foreach (var xobj in mobj.Xians)
            //    {
            //        pslst.AddRange(xobj.GetXianPoints());
            //    }
                
            //}

            var z=Mians.Where(t=>t.IsClosedLoop).ToList();
            Mians = Mians.Where(t => t.Xians.Count==4).ToList();
            //ConverttoMianMsg = string.Format("4 beam {0},parse all beam {1}", Mians.Count, _tempmians.Count);
        }

        private JWPoint Getmian(JwXian xian, JWMian mian, JWPoint jp)
        {
            beamxiantempids.Add(xian.Id);
            var z = beamxianIds.Except(beamxiantempids).ToList();
            bool bzhi = false;
            JwXian cxunxian = new JwXian();
            JWPoint bjd = new JWPoint();
            foreach (var x in z)
            {
                var obj = beamxians.FirstOrDefault(t => t.Id == x);
                if (!object.ReferenceEquals(obj, null))
                //if (obj != null)
                {
                    JWPoint jd;
                    if (xian.IsLianjie(obj, out jd, out bjd))
                    {
                        if (!obj.IsSelected)
                        {
                            if (jd == jp)
                            {
                                bzhi = true;
                                cxunxian = obj;
                                //bool isadd = true;
                                obj.IsSelected = true;
                                mian.Xians.Add(obj);
                                
                                break;

                                //foreach (var xobj in mian.Xians)
                                //{
                                //    if (obj == xobj)
                                //    {
                                //        isadd = false;
                                //        break;
                                //    }
                                //}

                                //return jp;
                            }
                        }
                    }
                }
            }
            if (bzhi)
            {
                return Getmian(cxunxian, mian, bjd);
            }
            return jp;
        }
       
        public void ChangeQieGeSolis()
        {
            //&& pillarsplitstylext
            if (pillarsplitcolorxt )
            {
                if (_pillaredothersanjiao.Count > 0)
                {
                    _tempDirected = new List<JwDirected>();
                    foreach (var b in _pillaredothersanjiao)
                    {
                        JwDirected jd = new JwDirected(b);
                        if (jd.IsDirected)
                        {
                            _tempDirected.Add(jd);
                        }
                    }
                    Directeds = _tempDirected.Distinct(new JwDirectedComparint()).ToList();
                    if (Directeds != null&&Directeds.Count>0)
                    {
                        foreach (var qg in Directeds)
                        {
                            foreach (var bm in _tempBeams)
                            {
                                if (!bm.ContainsDirected(qg))
                                {
                                    if (bm.JieChuDirected(qg))
                                    //if (bm.Contains(qg.DirectPoint))
                                    {
                                        if (bm.DirectionType == BeamDirectionType.Horizontal)
                                        {
                                            if (qg.TaggDirect == TaggDirect.Up || qg.TaggDirect == TaggDirect.Down)
                                            {
                                                bm.HasQieGe = true;
                                                bm.QieGePoints.Add(qg.DirectPoint);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (qg.TaggDirect == TaggDirect.Left || qg.TaggDirect == TaggDirect.Right)
                                            {
                                                bm.HasQieGe = true;
                                                bm.QieGePoints.Add(qg.DirectPoint);
                                                break;
                                            }
                                        }

                                    }
                                }
                                
                            }
                        }
                    }
                    
                }
            }
            else
            {
                if (QieGeSolidLst.Count > 0)
                {
                    //Directeds = new List<JwDirected>();
                    foreach (var qiege in QieGeSolidLst)
                    {
                        JwDirected jd = new JwDirected(qiege);
                        if (jd.IsDirected)
                        {
                            _tempDirected.Add(jd);
                        }
                    }
                    Directeds = _tempDirected.Distinct(new JwDirectedComparint()).ToList();
                    foreach (var qg in Directeds)
                    {
                        foreach (var bm in _tempBeams)
                        {
                            if (!bm.ContainsDirected(qg))
                            {
                                if (bm.JieChuDirected(qg))
                                //if (bm.Contains(qg.DirectPoint))
                                {
                                    if (bm.DirectionType == BeamDirectionType.Horizontal)
                                    {
                                        if (qg.TaggDirect == TaggDirect.Up || qg.TaggDirect == TaggDirect.Down)
                                        {
                                            bm.HasQieGe = true;
                                            bm.QieGePoints.Add(qg.DirectPoint);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (qg.TaggDirect == TaggDirect.Left || qg.TaggDirect == TaggDirect.Right)
                                        {
                                            bm.HasQieGe = true;
                                            bm.QieGePoints.Add(qg.DirectPoint);
                                            break;
                                        }
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// 2025年4月6日 对beam的绝对起点 进行赋值
        /// </summary>
        private void CreateQieGeBeams()
        {
            var qiegebeams= _tempBeams.Where(t=>t.HasQieGe).ToList();
            
            
            foreach (var beam in qiegebeams)
            {
                Beams.AddRange(beam.BeamSplite());
            }
            var wuqiegebeams = _tempBeams.Where(t => !t.IsParentBeam).ToList();
            ParentQieGeBeam = qiegebeams;
            if (wuqiegebeams != null)
            {
                Beams.AddRange(wuqiegebeams);
            }
            BeamsCount = Beams.Count;
            HorizontalBeams = Beams.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
            HorizontalBeamsCount = HorizontalBeams.Count;
            VerticalBeams = Beams.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
            VerticalBeamsCount = VerticalBeams.Count;
            if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
            {
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("HorizontalBeams total:{0}", HorizontalBeams.Count) });
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("VerticalBeams:{0}", VerticalBeams.Count) });
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Beams:{0}", Beams.Count) });

            }
            //RowsPointY = HorizontalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            //ColumnPointX = VerticalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            //CenterPoints = Beams.Select(t => t.CenterPoint).OrderBy(t => t.X).ThenBy(t => t.Y).ToList();
            if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
            {
                //GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Horizontal rows Y total:{0}", RowsPointY.ToString()) });
                //GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Vertical rows X total:{0}", ColumnPointX.ToString()) });
                //GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Beams:{0}", Beams.Count) });

            }
            //createBeamAbsolutePD();
        }

        /// <summary>
        /// 2025年4月6日
        /// </summary>
        private void createBeamAbsolutePD()
        {
            foreach (var beam in Beams)
            {
                if(beam.DirectionType == BeamDirectionType.Horizontal)
                {
                    beam.AbsolutePD =Math.Round(beam.TopLeft.X,6);
                }
                if (beam.DirectionType == BeamDirectionType.Vertical) 
                {
                    beam.AbsolutePD=Math.Round(beam.BottomLeft.Y,6);
                }
            }
        }




        /// <summary>
        ///  初始化画布  四个角 不一定实际存在 应该是虚拟的 且对称的 改
        /// </summary>
        public JwCanvas CreateCanvas()
        {
            double minx = JwAllPoints.Select(t => t.X).Min();
            double maxx=JwAllPoints.Select(t=>t.X).Max();
            double miny = JwAllPoints.Select(t => t.Y).Min();
            double maxy=JwAllPoints.Select(t=>t.Y).Max();

            TopLeft = new JWPoint(minx,maxy);

            TopRight = new JWPoint(maxx, maxy);

            BottomLeft = new JWPoint(minx,miny);
            BottomRight = new JWPoint(maxx, miny);
            Width = TopRight.X - TopLeft.X;
            Height = TopLeft.Y - BottomLeft.Y;
            HasCanvas=true;
            jwCanvas = new JwCanvas(TopLeft, TopRight, BottomLeft, BottomRight, Beams, JwAllPoints,Width,Height,Pillars,ParentQieGeBeam);
            jwCanvas.LinkParts = AllLinkPart;
            jwCanvas.LianjieSingles=this.LianjieSingles;
            return jwCanvas;
        }


        public List<JwHoleData> _holeDatas = new List<JwHoleData>();

        public List<JwBeamVerticalData> _jwbvdatas = new List<JwBeamVerticalData>();

        public async void CreateData()
        {
            _subData = new JwProjectSubData();
            _subData.JwProjectMainDataId = _projectMaindata.Id;
            _subData.FloorName = _floorName;
            _subData.BCount = BBCount;
            _subData.BGCount = BGCount;
            _subData.PillarCount = PillarCount;
            _subData.KPillarCount=KPillarCount;
            _subData.SinglePillarCount = SinglePillarCount;
            _subData.Biaochi = _scale.ToString();
            _subData.BeamCount=Beams.Count;
            _subData.HorizontalBeamsCount=HorizontalBeams.Count;
            _subData.VerticalBeamsCount=VerticalBeams.Count;
            _subData.Location = TopLeft.ToPoint();
            _subData.Width = Width.Value;
            _subData.Height = Height.Value;
            _subData.Scale = this._scale;
            if (this.MarkBeam.HasValue)
            {
                _subData.MarkBeam = this.MarkBeam;
            }
            
            if(Beams.Count> 0)
            {
                foreach (var bm in Beams)
                {
                    JwBeamData beamData = bm.ToDbData();
                    beamData.JwProjectSubDataId = _subData.Id;
                    beamData.FloorName = _floorName;
                    //beamData.x
                    
                    if(bm.Holes.Count>0 )
                    {
                        foreach (var hd in bm.Holes)
                        {
                            JwHoleData jhd = hd.ToData();
                            jhd.JwBeamDataId = beamData.Id;
                           
                            _holeDatas.Add(jhd);
                        }
                    }
                    if(bm.Baifangs?.Count>0)
                    {
                        //记录败方数据
                        foreach(var bd in bm.Baifangs)
                        {
                            JwBeamVerticalData verticalData = bd.ToData();
                            //bd.ParentBeamId = beamData.Id;
                            verticalData.JwBeamDataId= beamData.Id;
                            _jwbvdatas.Add(verticalData);
                        }
                    }
                    _beamdatas.Add(beamData);
                    //beamData
                }
            }
            if (Pillars.Count>0)
            {
                foreach (var p in Pillars)
                {
                    JwPillarData jwPillarData = p.ToPillarData();
                    jwPillarData.JwProjectSubDataId = _subData.Id;
                    //jwPillarData.FloorName = _floorName;
                    _beampillarDatas.Add(jwPillarData);
                }
            }
            if (AllLinkPart.Count > 0)
            {
                foreach(var lp in AllLinkPart)
                {
                    JwLinkPartData partdata = lp.ToData();
                    partdata.JwProjectSubDataId= _subData.Id;
                    if (string.IsNullOrEmpty(lp.BeamId))
                    {
                        partdata.BeamId= string.Empty;
                    }
                    partdata.Scale=this._scale;
                    _linkPartDatas.Add(partdata);
                    //JwLinkPartData partData=
                }
            }

            //连接线保存
            if (LianjieSingles.Count > 0)
            {
                foreach(var lj in LianjieSingles)
                {

                    JwLianjieData lianjieData = lj.ToDbData();
                    lianjieData.JwProjectSubDataId = _subData.Id;
                    lianjieData.ProjectSubName = _subData.FloorName;
                    _lianjieDatas.Add(lianjieData);

                }
            }
        }

        public List<JwTouch> Touchs = new List<JwTouch>();

        /// <summary>
        /// 2025年6月24日 增加记录 胜败
        /// </summary>
        public List<JwBeamVertical> JwBeamVerticalLst = new List<JwBeamVertical>();

        /// <summary>
        /// 对beam 遍历 查询 垂直接触的梁
        /// 2025年5月21日修改JwBeamVertical 增加端类型等 用来适配链接线的识别
        /// 2025年6月22日 针对识别连接线  修改孔组措施
        /// </summary>
        public void JudgmentJieChu()
        {
            if (_tempBeams != null)
            {
                if (_tempBeams.Count > 0)
                {
                    HasBeam = true;
                    int i = 1;
                    //以下赋予编号
                    //_tempBeams.ForEach(t => {
                    //    t.BeamCode = string.Format("{0}-{1}", t.DirectionType.ToString(), i);
                    //    i++;
                    //});
                    //HorizontalBeams = Beams.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
                    //VerticalBeams = Beams.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
                    HorizontalBeams = _tempBeams.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
                    VerticalBeams = _tempBeams.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
                    RowsPointY = HorizontalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
                    ColumnPointX = VerticalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
                    var CenterPoints = _tempBeams.Select(t => t.CenterPoint).OrderBy(t => t.X).ThenBy(t => t.Y).ToList();
                    List<IGrouping<double, JwBeam>> shuipinggroup = HorizontalBeams.GroupBy(t => t.Center).OrderByDescending(t => t.Key).ToList();
                    double jxpd = JwFileConsts.JwJianxi / JwFileConsts.JwScale;
                    foreach (var q in shuipinggroup)
                    {
                        var groupbottomy = q.Max(t => t.BottomLeft.Y);
                        var grouptopy = q.Max(t => t.TopLeft.Y);
                        //2025年5月21日下面找败方为水平梁的败方（垂直）且是 水平梁的上方，即接触端为起始 
                        var chuizhishang = VerticalBeams.Where(t => t.BottomLeft.Y > q.Key && t.BottomLeft.Y < (grouptopy + jxpd)).ToList();
                        //double shuiymax = VerticalBeams.Select(t => t.BottomLeft.Y).Max();
                        if (chuizhishang.Count > 0)//该组需要往上找，
                        {
                            var lx = chuizhishang.Min(t => t.BottomLeft.X);
                            var f = q.Where(t => t.TopRight.X > lx).ToList();
                            foreach (var l in f)
                            {
                                foreach (var c in chuizhishang)
                                {
                                    if (c.Center > l.TopLeft.X && c.Center < l.TopRight.X)
                                    {

                                        //修改败方中心起点同时修改
                                        c.StartCenter = l.Center;
                                        double sy = c.BottomLeft.Y;//记录未修正之前的 设计图内 的 用来比较连接线
                                        c.ChangeStartCenter();

                                        JwBeamVertical vertical = new JwBeamVertical
                                        {
                                            Id=Guid.NewGuid().ToString(),
                                            Position = TaggDirect.Up,
                                            PositionPoint = new JWPoint(c.Center, q.Key),
                                            VerticalBeam = c,
                                            Center = c.Center,
                                            ParentBeamId = l.Id,
                                            LoserPortType = PortType.Start,
                                            IsShuipingLoser = false,
                                            InitialLoser = sy
                                        };
                                        l.Baifangs.Add(vertical);//记录 败方及他的位置
                                        var jwt = new JwTouch { WinnerBeam = l, JwBeamVertical = vertical, LoserBeam = c };
                                        Touchs.Add(jwt); 
                                        //处理端部孔组信息  JwKongZu类

                                        //胜方
                                        //JwKongZu sfkongzu = new JwKongZu
                                        //{
                                        //    KongNum = 4,
                                        //    SuoShuMian = KongzuSuoshuMian.All,
                                        //    Position = new JWPoint(c.Center, q.Key),
                                        //    BeamId = l.Id,
                                        //    Sourec = KongzuSuoshuMian.Center
                                        //};
                                        //l.AddHole(sfkongzu);
                                        var sflocation = new JWPoint(c.Center, l.Center);
                                        //2025年4月12日 修改中心点位置计算逻辑
                                        var pflocation = new JWPoint(c.Center, c.StartCenter + (JwFileConsts.Gjubian + JwFileConsts.GBianjuZhongxin) / JwFileConsts.JwScale);
                                        //var pflocation = new JWPoint(c.Center, c.BottomLeft.Y + JwFileConsts.Gjubian / JwFileConsts.JwScale);
                                        //JwKongZu bfkongzu = new JwKongZu
                                        //{
                                        //    KongNum = 2,
                                        //    SuoShuMian = KongzuSuoshuMian.All,
                                        //    Position = new JWPoint(c.Center, c.BottomLeft.Y+JwFileConsts.Gjubian/JwFileConsts.JwScale),
                                        //    Sourec = KongzuSuoshuMian.Center,
                                        //    BeamId = c.Id,
                                        //    Type = KongzuType.G
                                        //};
                                        ////.c.Kongzus.Add(bfkongzu);
                                        //c.AddHole(bfkongzu);
                                        var shengfangjw=l.AddAnyHoleReturn(sflocation, HoleCreateFrom.JieChu);

                                        jwt.JwHoleG = shengfangjw;
                                        c.AddAnyHole(pflocation, HoleCreateFrom.JieChuG, null, true, false);
                                        c.HasStartSide = true;
                                        c.StartTelosType = KongzuType.G;
                                        c.StartSide = new JwBeamSide
                                        {
                                            KongZu = c.Holes.Last(),
                                            SideType = KongzuType.G,
                                        };
                                        //胜方同时拥有 背面的B构建 
                                        JwLinkPart jbb = new JwLinkPart();
                                        jbb.Directed = TaggDirect.Up;
                                        jbb.GouJianType = GouJianType.BG;
                                        jbb.BujianName = "BG";
                                        jbb.BjCenterPoint = new JWPoint
                                        {
                                            X = Math.Round(c.Center,6),
                                            Y = Math.Round(l.Center,6)
                                        };
                                        jbb.ParentBeam = l;
                                        jbb.BeamId = l.Id;
                                        jbb.BBeam = c;
                                        var existbb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Up).ToList();
                                        if (existbb.Count > 0)
                                        {
                                            var linkchang = existbb.First();
                                            if (linkchang != null)
                                            {
                                                linkchang.GouJianType = GouJianType.BG;
                                                linkchang.BujianName = "BG";
                                                linkchang.ParentBeam = l;
                                                linkchang.BeamId = l.Id;
                                                linkchang.BBeam = c;
                                            }
                                        }
                                        else
                                        {
                                            l.LinkParts.Add(jbb);
                                            AllLinkPart.Add(jbb);
                                        }

                                        var lfb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Down).Count();
                                        if (lfb == 0)
                                        {
                                            JwLinkPart jbbeimian = new JwLinkPart();
                                            jbbeimian.Directed = TaggDirect.Down;
                                            jbbeimian.GouJianType = GouJianType.B;
                                            jbbeimian.BujianName = "B";
                                            jbbeimian.BjCenterPoint = jbb.BjCenterPoint;
                                            //jbbeimian.BjCenterPoint = new JWPoint
                                            //{
                                            //    X = Math.Round(c.Center, 6),
                                            //    Y = Math.Round(l.Center, 6)
                                            //};
                                            jbbeimian.ParentBeam = l;
                                            jbbeimian.BeamId = l.Id;

                                            l.LinkParts.Add(jbbeimian);

                                            AllLinkPart.Add(jbbeimian);
                                        }


                                    }
                                }
                            }
                        }

                        var chuizhixia = VerticalBeams.Where(t => t.TopLeft.Y < q.Key && t.TopLeft.Y > (groupbottomy - jxpd)).ToList();
                        if (chuizhixia.Count > 0)
                        {
                            var lx = chuizhixia.Min(t => t.TopLeft.X);
                            //进一步缩小范围 
                            var f = q.Where(t => t.TopRight.X > lx).ToList();
                            foreach (var l in f)
                            {
                                foreach (var c in chuizhixia)
                                {
                                    if (c.Center > l.TopLeft.X && c.Center < l.TopRight.X)
                                    {
                                        //修改终点中心
                                        c.EndCenter = l.Center;
                                        double sy = c.TopLeft.Y;//记录未修正之前的 设计图内 的 用来比较连接线
                                        c.ChangeEndCenter();

                                        JwBeamVertical vertical = new JwBeamVertical
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Position = TaggDirect.Down,
                                            PositionPoint = new JWPoint(c.Center, q.Key),
                                            VerticalBeam = c,
                                            Center = c.Center,
                                            ParentBeamId = l.Id,
                                            LoserPortType = PortType.End,
                                            IsShuipingLoser = false,
                                            InitialLoser = sy
                                        };
                                        l.Baifangs.Add(vertical);//记录 败方及他的位置
                                        var jwt = new JwTouch { WinnerBeam = l, JwBeamVertical = vertical, LoserBeam = c };
                                        Touchs.Add(jwt); ;
                                        //处理端部孔组信息  JwKongZu类

                                        //胜方

                                        var sflocation = new JWPoint(c.Center, l.Center);
                                        var pfloaction = new JWPoint(c.Center, c.EndCenter - (JwFileConsts.Gjubian + JwFileConsts.GBianjuZhongxin) / JwFileConsts.JwScale);
                                        //var pfloaction = new JWPoint(c.Center, c.TopLeft.Y - JwFileConsts.Gjubian / JwFileConsts.JwScale);
                                        //c.Kongzus.Add(bfkongzu);

                                        var shengfangjw = l.AddAnyHoleReturn(sflocation, HoleCreateFrom.JieChu);
                                        jwt.JwHoleG = shengfangjw;
                                        c.AddAnyHole(pfloaction, HoleCreateFrom.JieChuG, null, false, true);
                                        c.HasEndSide = true;
                                        c.EndTelosType = KongzuType.G;
                                        c.EndSide = new JwBeamSide
                                        {
                                            KongZu = c.Holes.Last(),
                                            SideType = KongzuType.G,
                                        };

                                        JwLinkPart jbb = new JwLinkPart();
                                        jbb.Directed = TaggDirect.Down;
                                        jbb.GouJianType = GouJianType.BG;
                                        jbb.BujianName = "BG";
                                        jbb.BjCenterPoint = new JWPoint
                                        {
                                            X = Math.Round(c.Center, 6),
                                            Y = Math.Round(l.Center, 6)
                                        };
                                        jbb.ParentBeam = l;
                                        jbb.BeamId = l.Id;
                                        jbb.BBeam = c;

                                        var existbb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Down).ToList();
                                        if (existbb.Count > 0)
                                        {
                                            var linkchang = existbb.First();
                                            if (linkchang != null)
                                            {
                                                linkchang.GouJianType = GouJianType.BG;
                                                linkchang.BujianName = "BG";
                                                linkchang.ParentBeam = l;
                                                linkchang.BeamId = l.Id;
                                                linkchang.BBeam = c;
                                            }
                                        }
                                        else
                                        {
                                            l.LinkParts.Add(jbb);
                                            AllLinkPart.Add(jbb);
                                        }

                                        var lfb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Up).Count();
                                        if (lfb == 0)
                                        {
                                            JwLinkPart jbbeimian = new JwLinkPart();
                                            jbbeimian.Directed = TaggDirect.Up;
                                            jbbeimian.GouJianType = GouJianType.B;
                                            jbbeimian.BujianName = "B";
                                            jbbeimian.BjCenterPoint = jbb.BjCenterPoint;
                                            //    = new JWPoint
                                            //{
                                            //    X = Math.Round(c.Center, 6),
                                            //    Y = Math.Round(l.Center, 6)
                                            //};
                                            jbbeimian.ParentBeam = l;
                                            jbbeimian.BeamId = l.Id;

                                            l.LinkParts.Add(jbbeimian);

                                            AllLinkPart.Add(jbbeimian);
                                        }


                                    }
                                }
                            }
                        }
                        //else
                        //{
                        //    //该组不用寻找
                        //}
                        //往下早 

                    }

                    List<IGrouping<double, JwBeam>> chuizhigroup = VerticalBeams.GroupBy(t => t.Center).OrderBy(t => t.Key).ToList();

                    foreach (var q in chuizhigroup)
                    {
                        var groupleft = q.Min(t => t.BottomLeft.X);
                        var grouprightx = q.Max(t => t.BottomRight.X);
                        //胜方为垂直梁，寻找左侧，接触为败方的end
                        var shuipingleft = HorizontalBeams.Where(t => t.TopRight.X < groupleft && t.TopRight.X > (groupleft - jxpd)).ToList();
                        if (shuipingleft.Count > 0)
                        {
                            var ly = shuipingleft.Max(t => t.TopLeft.Y);

                            var f = q.Where(t => t.BottomLeft.Y < ly).ToList();
                            foreach (var l in f)
                            {
                                foreach (var r in shuipingleft)
                                {
                                    if (r.Center > l.BottomLeft.Y && r.Center < l.TopLeft.Y)
                                    {

                                        //相对垂直的 左边的 败方的终点
                                        r.EndCenter = l.Center;
                                        double xjc = r.TopRight.X;
                                        r.ChangeEndCenter();

                                        JwBeamVertical vertical = new JwBeamVertical
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Position = TaggDirect.Left,
                                            PositionPoint = new JWPoint(q.Key, r.Center),
                                            VerticalBeam = r,
                                            Center = r.Center,
                                            ParentBeamId = l.Id,
                                            InitialLoser = xjc,
                                            IsShuipingLoser = true,
                                            LoserPortType = PortType.End
                                        };
                                        l.Baifangs.Add(vertical);//记录 败方及他的位置
                                        var jwt = new JwTouch { WinnerBeam = l, JwBeamVertical = vertical, LoserBeam = r };
                                        Touchs.Add(jwt) ;
                                        //处理端部孔组信息  JwKongZu类
                                        //胜方
                                        var sflocaiton = new JWPoint(l.Center, r.Center);
                                        var bflocation = new JWPoint(r.EndCenter - (JwFileConsts.Gjubian + JwFileConsts.GBianjuZhongxin) / JwFileConsts.JwScale, r.Center);
                                        //var bflocation = new JWPoint(r.TopRight!.X - JwFileConsts.Gjubian / JwFileConsts.JwScale, r.Center);
                                        var shengfangjw=l.AddAnyHoleReturn(sflocaiton, HoleCreateFrom.JieChu);
                                        jwt.JwHoleG = shengfangjw;
                                        r.AddAnyHole(bflocation, HoleCreateFrom.JieChuG, null, false, true);
                                        r.HasEndSide = true;
                                        r.EndTelosType = KongzuType.G;
                                        r.EndSide = new JwBeamSide
                                        {
                                            KongZu = r.Holes.Last(),
                                            SideType = KongzuType.G
                                        };


                                        JwLinkPart jbb = new JwLinkPart();
                                        jbb.Directed = TaggDirect.Left;
                                        jbb.GouJianType = GouJianType.BG;
                                        jbb.BujianName = "BG";
                                        jbb.BjCenterPoint = new JWPoint
                                        {
                                            X = Math.Round(l.Center,6),
                                            Y = Math.Round(r.Center,6)
                                        };
                                        jbb.ParentBeam = l;
                                        jbb.BeamId = l.Id;
                                        jbb.BBeam = r;
                                        var existbb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Left).ToList();
                                        if (existbb.Count > 0)
                                        {
                                            var linkchang = existbb.First();
                                            if (linkchang != null)
                                            {
                                                linkchang.GouJianType = GouJianType.BG;
                                                linkchang.BujianName = "BG";
                                                linkchang.ParentBeam = l;
                                                linkchang.BeamId = l.Id;
                                                linkchang.BBeam = r;
                                            }
                                        }
                                        else
                                        {
                                            l.LinkParts.Add(jbb);
                                            AllLinkPart.Add(jbb);
                                        }

                                        var lfb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Right).Count();
                                        if (lfb == 0)
                                        {
                                            JwLinkPart jbbeimian = new JwLinkPart();
                                            jbbeimian.Directed = TaggDirect.Right;
                                            jbbeimian.GouJianType = GouJianType.B;
                                            jbbeimian.BujianName = "B";
                                            jbbeimian.BjCenterPoint = jbb.BjCenterPoint;
                                            //    = new JWPoint
                                            //{
                                            //    X = Math.Round(q.Key,6),
                                            //    Y = Math.Round(r.Center,6)
                                            //};
                                            jbbeimian.ParentBeam = l;
                                            jbbeimian.BeamId = l.Id;

                                            l.LinkParts.Add(jbbeimian);

                                            AllLinkPart.Add(jbbeimian);
                                        }
                                    }
                                }
                            }
                        }
                        var shuipingright = HorizontalBeams.Where(t => t.TopLeft.X > grouprightx && t.TopLeft.X < (grouprightx + jxpd)).ToList();
                        if (shuipingright.Count > 0)
                        {
                            var ly = shuipingright.Max(t => t.TopLeft.Y);
                            var f = q.Where(t => t.BottomLeft.Y < ly).ToList();
                            foreach (var l in f)
                            {
                                foreach (var r in shuipingright)
                                {
                                    if (r.Center > l.BottomLeft.Y && r.Center < l.TopLeft.Y)
                                    {
                                        //败方在垂直梁的右边  败方的起点
                                        r.StartCenter = l.Center;
                                        double xys = r.TopLeft.X;
                                        r.ChangeStartCenter();

                                        JwBeamVertical vertical = new JwBeamVertical
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Position = TaggDirect.Right,
                                            PositionPoint = new JWPoint(q.Key, r.Center),
                                            VerticalBeam = r,
                                            Center = r.Center,
                                            IsShuipingLoser = true,
                                            InitialLoser = xys,
                                            ParentBeamId = l.Id,
                                            LoserPortType = PortType.Start
                                        };
                                        l.Baifangs.Add(vertical);//记录 败方及他的位置
                                        var jwt = new JwTouch { WinnerBeam = l, JwBeamVertical = vertical, LoserBeam = r };
                                        Touchs.Add(jwt) ;
                                        //处理端部孔组信息  JwKongZu类

                                        //胜方
                                        var sflocation = new JWPoint(l.Center, r.Center);
                                        var bfloaction = new JWPoint(r.StartCenter + (JwFileConsts.Gjubian+JwFileConsts.GBianjuZhongxin) / JwFileConsts.JwScale, r.Center);
                                        //                                        var bfloaction = new JWPoint(r.TopLeft.X + JwFileConsts.Gjubian / JwFileConsts.JwScale, r.Center);

                                        var shengfangjw=l.AddAnyHoleReturn(sflocation, HoleCreateFrom.JieChu);
                                        jwt.JwHoleG = shengfangjw;
                                        r.AddAnyHole(bfloaction, HoleCreateFrom.JieChuG, null, true, false);
                                        r.HasStartSide = true;
                                        r.StartTelosType = KongzuType.G;
                                        r.StartSide = new JwBeamSide
                                        {
                                            KongZu = r.Holes.Last(),
                                            SideType = KongzuType.G,
                                        };



                                        JwLinkPart jbb = new JwLinkPart();
                                        jbb.Directed = TaggDirect.Right;
                                        jbb.GouJianType = GouJianType.BG;
                                        jbb.BujianName = "BG";
                                        jbb.BjCenterPoint = new JWPoint
                                        {
                                            X = Math.Round(l.Center,6),
                                            Y = Math.Round(r.Center,6)
                                        };
                                        jbb.ParentBeam = l;
                                        jbb.BeamId = l.Id;
                                        jbb.BBeam = r;
                                        var existbb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Right).ToList();
                                        if (existbb.Count > 0)
                                        {
                                            var linkchang = existbb.First();
                                            if (linkchang != null)
                                            {
                                                linkchang.GouJianType = GouJianType.BG;
                                                linkchang.BujianName = "BG";
                                                linkchang.ParentBeam = l;
                                                linkchang.BeamId = l.Id;
                                                linkchang.BBeam = r;
                                            }
                                        }
                                        else
                                        {
                                            l.LinkParts.Add(jbb);
                                            AllLinkPart.Add(jbb);
                                        }
                                        var lfb = AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Left).Count();
                                        if (lfb == 0)
                                        {
                                            JwLinkPart jbbeimian = new JwLinkPart();
                                            jbbeimian.Directed = TaggDirect.Left;
                                            jbbeimian.GouJianType = GouJianType.B;
                                            jbbeimian.BujianName = "B";
                                            jbbeimian.BjCenterPoint = jbb.BjCenterPoint;
                                            //    = new JWPoint
                                            //{
                                            //    X = Math.Round(q.Key,6),
                                            //    Y = Math.Round(r.Center,6)
                                            //};
                                            jbbeimian.ParentBeam = l;
                                            jbbeimian.BeamId = l.Id;

                                            l.LinkParts.Add(jbbeimian);

                                            AllLinkPart.Add(jbbeimian);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            
        }

        public void panduanBlockGuishu()
        {
            //JwBeamDeepParse deepParse = new JwBeamDeepParse(Beams);

            

            RowsPointY = HorizontalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            ColumnPointX = VerticalBeams.Select(t => t.Center).OrderBy(t => t).ToList();

            List<IGrouping<double, JwBeam>> shuipinggroup = HorizontalBeams.GroupBy(t => t.BottomLeft.Y).OrderByDescending(t => t.Key).ToList();

            List<JwBlock> zfxblocks = new List<JwBlock>();

            foreach(var p in Pillars)
            {
                if (p.BaseType == PillarBaseType.SinglePillar)
                {
                    zfxblocks.AddRange(p.Blocks);
                }
                if (p.BaseType == PillarBaseType.KPillar)
                {
                    foreach(var b in p.Blocks)
                    {
                        if (b.Iszhengfangxing)
                        {
                            zfxblocks.Add(b);
                        }
                    }
                }
                //if(p.BaseType)
            }

            zfxblocks = zfxblocks.Distinct(new JwBlockComparint()).ToList();

            var wuqiegebeams = _tempBeams.Where(t => !t.IsParentBeam).ToList();

            List<string> yiguishublocks = new List<string>();

            foreach (var b in wuqiegebeams)
            {
                foreach (var a in zfxblocks)
                {

                    if (b.Contains(a.CenterPoint))
                    {
                        yiguishublocks.Add(a.Id);
                        b.ZhuBlocks.Add(JwShapeHelper.CreateNewBlock(a));
                        JwLinkPart jbb = new JwLinkPart();
                        jbb.Directed = b.DirectionType == BeamDirectionType.Horizontal ? TaggDirect.Up : TaggDirect.Left;
                        jbb.BujianName = "B";
                        jbb.GouJianType = GouJianType.B;
                        //jbb.BjCenterPoint = new JWPoint
                        //{
                        //    X = Math.Round(a.CenterPoint.X, 0),
                        //    Y = Math.Round(a.CenterPoint.Y, 0)
                        //};
                        jbb.BjCenterPoint = new JWPoint
                        {
                            X = Math.Round(a.CenterPoint.X, 6),
                            Y = Math.Round(a.CenterPoint.Y, 6)
                        };
                        jbb.ParentBeam = b;
                        jbb.BeamId = b.Id;

                        //JwKongZu kongzu = new JwKongZu
                        //{
                        //    KongNum = 4,
                        //    SuoShuMian = KongzuSuoshuMian.All,
                        //    Position = jbb.BjCenterPoint,
                        //    Sourec = KongzuSuoshuMian.Center,
                        //    BeamId = b.Id,
                        //    Type = KongzuType.B
                        //};
                        ////r.Kongzus.Add(bfkongzu);
                        //b.AddHole(kongzu);
                        b.AddAnyHole(jbb.BjCenterPoint, HoleCreateFrom.Pillar);

                        //IsEqualsWithError
                        if (AllLinkPart.Where(t => t.BjCenterPoint.IsEqualsWithError(jbb.BjCenterPoint)).Count() == 0)
                        //if (AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint).Count()== 0)
                        {
                            b.LinkParts.Add(jbb);
                            AllLinkPart.Add(jbb);
                            JwLinkPart jb1 = new JwLinkPart();
                            jb1.BujianName = "B";
                            jb1.GouJianType = GouJianType.B;
                            jb1.BeamId = b.Id;
                            jb1.Directed = b.DirectionType == BeamDirectionType.Horizontal ? TaggDirect.Down : TaggDirect.Right;
                            jb1.BjCenterPoint = jbb.BjCenterPoint;
                            jb1.ParentBeam = b;
                            b.LinkParts.Add(jb1);
                            AllLinkPart.Add(jb1);
                        }
                        
                            //如果存在BG构建 但是没有B匹配 
                        //    if (b.LinkParts.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.BujianName == "BG").Count() > 0)
                        //{
                        //    b.LinkParts.Add(jbb);
                        //    AllLinkPart.Add(jbb);
                        //}
                        //else
                        //{
                        //    b.LinkParts.Add(jbb);
                        //    AllLinkPart.Add(jbb);
                        //    JwLinkPart jb1 = new JwLinkPart();
                        //    jb1.BujianName = "B";
                        //    jb1.BeamId = b.Id;
                        //    jb1.BjCenterPoint = new JWPoint
                        //    {
                        //        X = Math.Round(a.CenterPoint.X, 2),
                        //        Y = Math.Round(a.CenterPoint.Y, 2)
                        //    };
                        //    jb1.ParentBeam = b;
                        //    b.LinkParts.Add(jb1);
                        //    AllLinkPart.Add(jb1);
                        //}
                    }
                }
                if (Pillars != null)
                {
                    foreach (var z in Pillars)
                    {
                        if (b.Contains(z.CenterPoint))
                        {
                            b.BeamPillars.Add(z);
                        }
                    }
                }
                
            }



            var idlst = _tempBeams.Select(t => t.Id).ToList();

            var weibeams = Beams.Where(t => !idlst.Contains(t.Id)).ToList();
            foreach (var b in weibeams)
            {
                foreach (var a in zfxblocks)
                {

                    if (b.Contains(a.CenterPoint))
                    {
                        b.ZhuBlocks.Add(JwShapeHelper.CreateNewBlock(a));
                        JwLinkPart jbb = new JwLinkPart();
                        jbb.Directed = b.DirectionType == BeamDirectionType.Horizontal ? TaggDirect.Up : TaggDirect.Left;
                        jbb.BujianName = "B";
                        jbb.GouJianType = GouJianType.B;
                        //jbb.BjCenterPoint = new JWPoint
                        //{
                        //    X = Math.Round(a.CenterPoint.X, 0),
                        //    Y = Math.Round(a.CenterPoint.Y, 0)
                        //};
                        jbb.BjCenterPoint = new JWPoint
                        {
                            X = a.CenterPoint.X,
                            Y = a.CenterPoint.Y
                        };
                        jbb.ParentBeam = b;
                        jbb.BeamId = b.Id;
                        if (AllLinkPart.Where(t => t.BjCenterPoint.IsEqualsWithError(jbb.BjCenterPoint)).Count() == 0)
                            //if (AllLinkPart.Where(t => t.BjCenterPoint == jbb.BjCenterPoint).Count() == 0)
                        {
                            b.LinkParts.Add(jbb);
                            AllLinkPart.Add(jbb);
                            JwLinkPart jb1 = new JwLinkPart();
                            jb1.BujianName = "B";
                            jb1.GouJianType = GouJianType.B;
                            jb1.BeamId = b.Id;
                            jb1.Directed = b.DirectionType == BeamDirectionType.Horizontal ? TaggDirect.Down : TaggDirect.Right;
                            jb1.BjCenterPoint = new JWPoint
                            {
                                X = a.CenterPoint.X,
                                Y = a.CenterPoint.Y
                            };
                            jb1.ParentBeam = b;
                            b.LinkParts.Add(jb1);
                            AllLinkPart.Add(jb1);
                        }
                        //JwKongZu kongzu = new JwKongZu
                        //{
                        //    KongNum = 4,
                        //    SuoShuMian = KongzuSuoshuMian.All,
                        //    Position = jbb.BjCenterPoint,
                        //    Sourec = KongzuSuoshuMian.Center,
                        //    BeamId = b.Id,
                        //    Type = KongzuType.B
                        //};
                        ////r.Kongzus.Add(bfkongzu);
                        //b.AddHole(kongzu);
                        b.AddAnyHole(jbb.BjCenterPoint, HoleCreateFrom.Pillar);

                        //如果存在BG构建 但是没有B匹配 
                        //    if (b.LinkParts.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.BujianName == "BG").Count() > 0)
                        //{
                        //    b.LinkParts.Add(jbb);
                        //    AllLinkPart.Add(jbb);
                        //}
                        //else
                        //{
                        //    b.LinkParts.Add(jbb);
                        //    AllLinkPart.Add(jbb);
                        //    JwLinkPart jb1 = new JwLinkPart();
                        //    jb1.BujianName = "B";
                        //    jb1.BeamId = b.Id;
                        //    jb1.BjCenterPoint = new JWPoint
                        //    {
                        //        X = Math.Round(a.CenterPoint.X, 2),
                        //        Y = Math.Round(a.CenterPoint.Y, 2)
                        //    };
                        //    jb1.ParentBeam = b;
                        //    b.LinkParts.Add(jb1);
                        //    AllLinkPart.Add(jb1);
                        //}
                    }
                }
                if (Pillars != null)
                {
                    foreach (var z in Pillars)
                    {
                        if (b.Contains(z.CenterPoint))
                        {
                            b.BeamPillars.Add(z);
                        }
                    }
                }
            }

            var wchulifk=zfxblocks.Where(t=>!yiguishublocks.Contains(t.Id)).ToList();
            var qgblst = Beams.Where(t => t.IsQiegeBeam).ToList();
            foreach(var bll in wchulifk)
            {
                foreach(var qgb in qgblst)
                {
                    //if(qgb.HasQiegeStart)
                    //{
                    //    if(bll.CenterPoint==qgb.StartXinPoint)
                    //    {
                    //        var b=qgb.Holes.First(t=>t.LocationCenter==bll.CenterPoint);
                    //        b!.changeByOther( HoleCreateFrom.Pillar);
                    //    }
                       
                    //}
                    //if(qgb.HasQiegeEnd)
                    //{
                    //    if (bll.CenterPoint == qgb.EndXinPoint)
                    //    {
                    //        var b = qgb.Holes.First(t => t.LocationCenter == bll.CenterPoint);
                    //        b!.changeByOther(HoleCreateFrom.Pillar);
                    //    }
                    //}
                }
            }


            var parentlsts= _tempBeams.Where(t => t.IsParentBeam).ToList();
            var cc= parentlsts.Sum(t=>t.LinkParts.Count(q=>q.IsLianjie));

            //统计独立
            if (Pillars?.Count > 0)
            {
                if (Pillars.Where(t => !t.HasBeam).Count() > 0)
                {
                    var wbpillars = Pillars.Where(t => !t.HasBeam);
                    foreach (var z in wbpillars)
                    {
                        foreach (var zb in z.Blocks)
                        {
                            if (zb.Iszhengfangxing)
                            {
                                JwLinkPart jb1 = new JwLinkPart();
                                jb1.BujianName = "B";
                                jb1.GouJianType = GouJianType.B;
                                //jb1.BeamId = b.Id;
                                jb1.Directed = TaggDirect.Down;
                                jb1.BjCenterPoint = new JWPoint
                                {
                                    X = zb.CenterPoint.X,
                                    Y = zb.CenterPoint.Y
                                };
                                jb1.IsNoBeam = true;
                                AllLinkPart.Add(jb1);
                                IndependentLinkPart.Add(jb1);
                                JwLinkPart jb2 = new JwLinkPart();
                                jb2.BujianName = "B";
                                jb2.GouJianType = GouJianType.B;
                                //jb1.BeamId = b.Id;
                                jb2.Directed = TaggDirect.Up;
                                jb2.BjCenterPoint = new JWPoint
                                {
                                    X = zb.CenterPoint.X,
                                    Y = zb.CenterPoint.Y
                                };
                                jb2.IsNoBeam = true;
                                AllLinkPart.Add(jb2);
                                IndependentLinkPart.Add(jb2);
                            }
                        }
                    }
                }
            }
        }

        public void panduanBeamduankou()
        {
            if (Beams.Count > 0)
            {
                foreach(var beam in Beams) 
                {
                    double lg = Math.Round((beam.DirectionType == BeamDirectionType.Horizontal ? beam.Width : beam.Height)* JwFileConsts.JwScale, 0) ;
                    beam.Length = lg;
                    string s = "B";
                    double jians = -50;
                    string e = "B";
                    double jiane = -50;
                    //if (beam.HasStartSide)
                    //{
                        s = beam.StartTelosType == KongzuType.Center ? "B" : beam.StartTelosType.ToString();
                        if (beam.StartTelosType == KongzuType.B)
                        {
                            jians = -50;
                        }
                        if (beam.StartTelosType == KongzuType.G)
                        {
                            jians = 55;
                        }
                        if (beam.StartTelosType == KongzuType.J)
                        {
                            jians = 3;
                        }
                        //jians = beam.StartTelosType == KongzuType.G ? 55 : 3;
                    //}
                    //if (beam.HasEndSide)
                    //{
                        e = beam.EndTelosType == KongzuType.Center ? "B" : beam.EndTelosType.ToString();
                        if (beam.EndTelosType == KongzuType.B)
                        {
                            jiane = -50;
                        }
                        if (beam.EndTelosType == KongzuType.G)
                        {
                            jiane = 55;
                        }
                        if (beam.EndTelosType == KongzuType.J)
                        {
                            jiane = 3;
                        }
                        //jiane = beam.EndTelosType == KongzuType.G ? 55 : 3;
                    //}
                    lg = lg + jians + jiane;
                    //2024年12月9日
                    beam.XXLength = Math.Round((beam.EndCenter - beam.StartCenter) * JwFileConsts.JwScale, 0);
                    int mkkk = 900;
                    if (MarkBeam.HasValue)
                    {
                        mkkk=MarkBeam.Value;
                    }
                    var mkkkk = mkkk / 2;
                    double y = lg % mkkkk;

                    int c = (int)lg / mkkkk;
                    if(y==0)
                    {
                        beam.BeamCode = string.Format("{0}{1}{2}", s, e, c);
                    }
                    else
                    {
                        beam.BeamCode = string.Format("{0}{1}{2}+{3}", s, e, c, Math.Round(y,0));
                    }
                    
                }
            }
        }

        public void ChangeJwwEnojiToText()
        {
            foreach (var moji in ParseMojiLst)
            {
                var jt = new JwTagg(moji);
                if (Pillars?.Count > 0 && jt.HasParseSuccess)
                {
                    jt.SelectOwnPillar(Pillars);
                }
                _jwwmojitaggs.Add(jt);
            }
        }

        /// <summary>
        /// 二次寻找扩大偏差值
        /// </summary>
        private void SecondExtendTextPillar()
        {
            if(Pillars?.Count > 0 && _jwwmojitaggs.Count > 0)
            {
                var spillarlst = Pillars.Where(t => !t.HasTag).ToList();
                var staggs=_jwwmojitaggs.Where(t=>!t.HasPillar).ToList();
                if (spillarlst.Count > 0 && staggs.Count > 0)
                {
                    var txwcha = 2*JwFileConsts.TextParseMaxDistance / JwFileConsts.JwScale;
                    var chuizhiwucha =2* JwFileConsts.TextParseChuizhiMaxDistance / JwFileConsts.JwScale;
                    foreach(var tg in staggs)
                    {
                        if (tg.DirectionType == BeamDirectionType.Horizontal)
                        {
                            var lst= Pillars.Where(t => t.DirectionType == tg.DirectionType&&!t.HasTag).ToList();
                            if(lst.Count > 0)
                            {
                                foreach (var pill in lst)
                                {
                                    //同向误差
                                    if (tg.Center.X <= pill.CenterPoint.X + txwcha && tg.Center.X >= pill.CenterPoint.X - txwcha)
                                    {
                                        //垂直向误差 水平的 统计向上的
                                        if (tg.Center.Y <= pill.TopLeft.Y + chuizhiwucha && tg.Center.Y >= pill.CenterPoint.Y)
                                        {
                                            tg.NearPillar = pill;
                                            tg.PillarId = pill.Id;
                                            pill.Tagg = tg;
                                            pill.TagId = tg.Id;
                                            pill.TagName = tg.Title;
                                            pill.HasTag = true;
                                            tg.HasPillar = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var lst = Pillars.Where(t => ((t.DirectionType == tg.DirectionType) || t.BaseType == PillarBaseType.SinglePillar) && !t.HasTag).ToList();
                            foreach (var pill in lst)
                            {
                                //同向误差
                                if (tg.Center.Y <= pill.CenterPoint.Y + txwcha && tg.Center.Y >= pill.CenterPoint.Y - txwcha)
                                {
                                    //垂直向误差 水平的 统计向上的
                                    if (tg.Center.X >= pill.TopLeft.X - chuizhiwucha && tg.Center.X <= pill.CenterPoint.X)
                                    {
                                        tg.NearPillar = pill;
                                        tg.PillarId = pill.Id;
                                        pill.Tagg = tg;
                                        pill.TagId = tg.Id;
                                        pill.TagName = tg.Title;
                                        pill.HasTag = true;
                                        tg.HasPillar = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// 水平 向下找  垂直 向右
        /// </summary>
        private void ThirdExtendTextPillar()
        {
            if (Pillars?.Count > 0 && _jwwmojitaggs.Count > 0)
            {
                var spillarlst = Pillars.Where(t => !t.HasTag).ToList();
                var staggs = _jwwmojitaggs.Where(t => !t.HasPillar).ToList();
                if (spillarlst.Count > 0 && staggs.Count > 0)
                {
                    var txwcha =  2*JwFileConsts.TextParseMaxDistance / JwFileConsts.JwScale;
                    var chuizhiwucha =2* JwFileConsts.TextParseChuizhiMaxDistance / JwFileConsts.JwScale;
                    foreach (var tg in staggs)
                    {
                        if (tg.DirectionType == BeamDirectionType.Horizontal)
                        {
                            var lst = Pillars.Where(t => t.DirectionType == tg.DirectionType && !t.HasTag).ToList();
                            if (lst.Count > 0)
                            {
                                foreach (var pill in lst)
                                {
                                    //同向误差
                                    if (tg.Center.X <= pill.CenterPoint.X + txwcha && tg.Center.X >= pill.CenterPoint.X - txwcha)
                                    {
                                        //垂直向误差 水平的 统计向上的
                                        if (tg.Center.Y <= pill.CenterPoint.Y  && tg.Center.Y >= pill.BottomLeft.Y- chuizhiwucha)
                                        {
                                            tg.NearPillar = pill;
                                            tg.PillarId = pill.Id;
                                            pill.Tagg = tg;
                                            pill.TagId = tg.Id;
                                            pill.TagName = tg.Title;
                                            pill.HasTag = true;
                                            tg.HasPillar = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var lst = Pillars.Where(t => ((t.DirectionType == tg.DirectionType) || t.BaseType == PillarBaseType.SinglePillar) && !t.HasTag).ToList();
                            foreach (var pill in lst)
                            {
                                //同向误差
                                if (tg.Center.Y <= pill.CenterPoint.Y + txwcha && tg.Center.Y >= pill.CenterPoint.Y - txwcha)
                                {
                                    //垂直向误差 水平的 统计向上的
                                    if (tg.Center.X <= pill.TopRight.X + chuizhiwucha && tg.Center.X >= pill.CenterPoint.X)
                                    {
                                        tg.NearPillar = pill;
                                        tg.PillarId = pill.Id;
                                        pill.Tagg = tg;
                                        pill.TagId = tg.Id;
                                        pill.TagName = tg.Title;
                                        pill.HasTag = true;
                                        tg.HasPillar = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LastTextPillar()
        {
            if (Pillars?.Count > 0 && _jwwmojitaggs.Count > 0)
            {
                var spillarlst = Pillars.Where(t => !t.HasTag).ToList();
                var staggs = _jwwmojitaggs.Where(t => !t.HasPillar).ToList();
                if (spillarlst.Count > 0 && staggs.Count > 0)
                {
                    var txwcha = 2 * JwFileConsts.TextParseMaxDistance / JwFileConsts.JwScale;
                    var chuizhiwucha = 2 * JwFileConsts.TextParseChuizhiMaxDistance / JwFileConsts.JwScale;
                    foreach (var tg in staggs)
                    {
                        if (tg.DirectionType == BeamDirectionType.Horizontal)
                        {
                            var lst = Pillars.Where(t => t.DirectionType == tg.DirectionType && !t.HasTag).ToList();
                            if (lst.Count > 0)
                            {
                                foreach (var pill in lst)
                                {
                                    //同向误差
                                    if (tg.Center.X <= pill.TopRight.X && tg.Center.X >= pill.TopLeft.X)
                                    {
                                        //垂直向误差 水平的 统计向上的
                                        if (tg.Center.Y <= pill.CenterPoint.Y && tg.Center.Y >= pill.BottomLeft.Y - chuizhiwucha)
                                        {
                                            tg.NearPillar = pill;
                                            tg.PillarId = pill.Id;
                                            pill.Tagg = tg;
                                            pill.TagId = tg.Id;
                                            pill.TagName = tg.Title;
                                            pill.HasTag = true;
                                            tg.HasPillar = true;
                                        }
                                        //垂直向误差 水平的 统计向上的
                                        else if (tg.Center.Y <= pill.TopLeft.Y + chuizhiwucha && tg.Center.Y >= pill.CenterPoint.Y)
                                        {
                                            tg.NearPillar = pill;
                                            tg.PillarId = pill.Id;
                                            pill.Tagg = tg;
                                            pill.TagId = tg.Id;
                                            pill.TagName = tg.Title;
                                            pill.HasTag = true;
                                            tg.HasPillar = true;
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            var lst = Pillars.Where(t => ((t.DirectionType == tg.DirectionType) || t.BaseType == PillarBaseType.SinglePillar) && !t.HasTag).ToList();
                            foreach (var pill in lst)
                            {
                                //同向误差
                                if (tg.Center.Y <= pill.TopLeft.Y && tg.Center.Y >= pill.BottomLeft.Y )
                                {
                                    //you
                                    if (tg.Center.X <= pill.TopRight.X + chuizhiwucha && tg.Center.X >= pill.CenterPoint.X)
                                    {
                                        tg.NearPillar = pill;
                                        tg.PillarId = pill.Id;
                                        pill.Tagg = tg;
                                        pill.TagId = tg.Id;
                                        pill.TagName = tg.Title;
                                        pill.HasTag = true;
                                        tg.HasPillar = true;
                                    }
                                    //垂直向误差 水平的 统计向上的
                                    else if (tg.Center.X >= pill.TopLeft.X - chuizhiwucha && tg.Center.X <= pill.CenterPoint.X)
                                    {
                                        tg.NearPillar = pill;
                                        tg.PillarId = pill.Id;
                                        pill.Tagg = tg;
                                        pill.TagId = tg.Id;
                                        pill.TagName = tg.Title;
                                        pill.HasTag = true;
                                        tg.HasPillar = true;
                                    }
                                }
                            }
                        }
                    }
                }

                var kpillars = Pillars.Where(t => t.BaseType == PillarBaseType.KPillar).ToList();
                if (kpillars.Count() > 0)
                {
                    var llst = kpillars.Select(t =>
                    {
                        if (t.DirectionType == BeamDirectionType.Horizontal)
                        {
                            return t.Width;
                        }
                        else
                        {
                            return t.Height;
                        }
                    }).ToList();
                    double defaultl = llst.First();
                    var kltype = Math.Round(defaultl * _scale, 0);
                    MarkBeam = Convert.ToInt32(kltype) - 100;

                }


            }



        }


        private void Revision()
        {
            foreach (var b in Beams)
            {
                if (b.DirectionType == BeamDirectionType.Horizontal)
                {
                    b.AbsolutePD = Math.Round(b.TopLeft.X, 6);//不用更改数据库从新生成间隔数据
                }
                else if(b.DirectionType==BeamDirectionType.Vertical)
                {
                    b.AbsolutePD = Math.Round(b.BottomLeft.Y, 6);//不用更改数据库从新生成间隔数据
                }
                double xcd = 0;
                foreach(var c in b.jwBeamMarks)
                {
                    xcd= xcd+(c.HasError ? c.PreCenterCorrect : c.PreCenterDistance);
                }

                var s = b.StartTelosType == KongzuType.Center ? "B" : b.StartTelosType.ToString();
               var e = b.EndTelosType == KongzuType.Center ? "B" : b.EndTelosType.ToString();
                b.XXLength =Math.Round( Math.Round(xcd, 2) * JwFileConsts.JwScale,0);
                double bml = b.XXLength;
                if (b.StartTelosType == KongzuType.B)
                {
                    bml = bml + 50 ;
                    
                }
                if (b.StartTelosType == KongzuType.G)
                {

                    bml = bml - 55;
                    //startx = this.xstartx + 55 / JwFileConsts.JwScale;
                }
                if (b.StartTelosType == KongzuType.J)
                {
                    bml = bml - 3 ;
                    //startx = this.xstartx + 3 / JwFileConsts.JwScale;
                }
                if (b.EndTelosType == KongzuType.B)
                {
                    bml = bml + 50;
                    //endx = xendx + 50 / JwFileConsts.JwScale;
                }
                if (b.EndTelosType == KongzuType.G)
                {
                    bml = bml - 55;
                    //endx = this.xendx - 55 / JwFileConsts.JwScale;
                }
                if (b.EndTelosType == KongzuType.J)
                {
                    bml = bml - 3 ;
                    //endx = xendx - 3 / JwFileConsts.JwScale;
                }
                b.WidthScale = bml;
                if (b.DirectionType == BeamDirectionType.Horizontal)
                {
                    b.Width = Math.Round(bml / JwFileConsts.JwScale, 2);
                }
                if (b.DirectionType == BeamDirectionType.Vertical)
                {
                    b.Height = Math.Round(bml / JwFileConsts.JwScale, 2);
                }
                int mkkk = 900;
                if (MarkBeam.HasValue)
                {
                    mkkk = MarkBeam.Value;
                }
                var mkkkk = mkkk / 2;
                double y = b.XXLength % mkkkk;

                int q = (int)b.XXLength / mkkkk;
                if (y == 0)
                {
                    b.BeamCode = string.Format("{0}{1}{2}", s, e, q);
                }
                else
                {
                    b.BeamCode = string.Format("{0}{1}{2}+{3}", s, e, q, Math.Round(y, 0));
                }
            }
        }

        List<JwDownPillarMark> tempmarks;
        public void parseDownPillars()
        {
            brParseBySetting();
             tempmarks = new List<JwDownPillarMark>();
            for (int i = 0; i < brXians.Count; i++)
            {
                var nowxian=brXians[i];
                if (!nowxian.IsSelected)
                {
                    for (int j = i + 1; j < brXians.Count; j++)
                    {
                        JwLineIntersector lineIntersector = new JwLineIntersector();
                        var erxian = brXians[j];
                        if (!nowxian.IsSelected && !erxian.IsSelected)
                        {
                            if (lineIntersector.ComputeIntersect(nowxian, brXians[j]) == 1)
                            {
                                nowxian.IsSelected = true;
                                erxian.IsSelected = true;
                                JwDownPillarMark pillarMark = new JwDownPillarMark();
                                pillarMark.Id = Guid.NewGuid().ToString();
                                pillarMark.CenterPoint = lineIntersector.IntersectionPoint;
                                pillarMark.Line1 = nowxian;
                                pillarMark.Line2 = erxian;
                                tempmarks.Add(pillarMark);
                                break;
                            }
                        }

                    }
                    nowxian.IsSelected = true;
                }
            }

            if (Beams?.Count > 0&&tempmarks.Count>0)
            {
                foreach (var beam in Beams)
                {
                    var bno = tempmarks.Where(t => !t.HasBeam).ToList();
                    if (bno?.Count > 0) {
                        foreach (var bq in bno)
                        {
                            if (beam.Contains(bq.CenterPoint))
                            {
                                bq.HasBeam = true;
                                bq.OwerBeam = beam;
                            }
                            
                        }
                    }
                }
            }

            if (tempmarks?.Count > 0) {
                foreach (var item in tempmarks)
                {
                    if (item.HasBeam)
                    {
                        if (item.OwerBeam.DirectionType == BeamDirectionType.Horizontal)
                        {
                            if (Math.Abs(item.CenterPoint.Y - item.OwerBeam.Center) <= 0.0005)
                            {
                                item.IsInBeamCenter = true;
                            }
                            //if (item.CenterPoint.Y == item.OwerBeam.Center)
                            //{
                            //    item.IsInBeamCenter = true;
                            //}
                        }
                        else
                        {
                            if (Math.Abs(item.CenterPoint.X - item.OwerBeam.Center) <= 0.0005)
                            {
                                item.IsInBeamCenter = true;
                            }
                        }
                    }
                }
            }

            //条件一 必须是在梁内，
            var centermarks = tempmarks.Where(t => t.IsInBeamCenter).ToList();
            //条件二 判断是否有上柱，待增加 柱类增加 是否有下柱标识
            //遍历所有柱，进行判断
            var blocks = Pillars.Select(t => t.Blocks).ToList();//所有pillar 的block
            List<JwBlock> alb= new List<JwBlock>();

            foreach(var p in Pillars)
            {
                alb.AddRange(p.Blocks);
            }
            if (alb.Count > 0)
            {
                foreach (var downitem in centermarks)
                {
                    //blocks.Where(t=>t.)
                    var z = alb.Count(t => t.Contains(downitem.CenterPoint));
                    if (z > 0)
                    {
                        downitem.HasPillar = true;
                    }

                }
            }

            var nopillarcentermarks=centermarks.Where(t=>!t.HasPillar).ToList();


            foreach (var beam in nopillarcentermarks) {

                JwLinkPart jbb = new JwLinkPart();
                jbb.Directed = beam.OwerBeam.DirectionType == BeamDirectionType.Horizontal ? TaggDirect.Up : TaggDirect.Left;
                jbb.BujianName = "B";
                jbb.GouJianType = GouJianType.B;
                //jbb.BjCenterPoint = new JWPoint
                //{
                //    X = Math.Round(a.CenterPoint.X, 0),
                //    Y = Math.Round(a.CenterPoint.Y, 0)
                //};
                jbb.BjCenterPoint = beam.CenterPoint;
                jbb.ParentBeam = beam.OwerBeam;
                jbb.BeamId = beam.OwerBeam.Id;
                jbb.IsDownPillar = true;

                if (AllLinkPart.Where(t => t.BjCenterPoint.IsEqualsWithError(jbb.BjCenterPoint)).Count() == 0)
                {
                    beam.OwerBeam.AddAnyHole(beam.CenterPoint, HoleCreateFrom.Pillar, null, false, false);
                    beam.OwerBeam.LinkParts.Add(jbb);
                    AllLinkPart.Add(jbb);
                    JwLinkPart jb1 = new JwLinkPart();
                    jb1.BujianName = "B";
                    jb1.GouJianType = GouJianType.B;
                    jb1.BeamId = beam.OwerBeam.Id;
                    jb1.IsDownPillar = true;
                    jb1.Directed = beam.OwerBeam.DirectionType == BeamDirectionType.Horizontal ? TaggDirect.Down : TaggDirect.Right;
                    jb1.BjCenterPoint = beam.CenterPoint;
                    jb1.ParentBeam = beam.OwerBeam;
                    beam.OwerBeam.LinkParts.Add(jb1);
                    AllLinkPart.Add(jb1);
                }
            }


            var zz = centermarks.Count;

        }

        private void StatisticalBBGquantity()
        {
            GouJianZongshu = AllLinkPart.Count;
            string allgjmsg = string.Format("BBG all count is :{0}{1}", GouJianZongshu, Environment.NewLine);
            SendMsg(allgjmsg);
            SendMsg(string.Format("duli bb is {0}{1}", IndependentLinkPart.Count, Environment.NewLine));
            BBCount = AllLinkPart.Where(t => t.GouJianType == GouJianType.B).Count();
            BGCount = AllLinkPart.Where(t => t.GouJianType == GouJianType.BG).Count();
            SendMsg(string.Format("bb is {0}{1}", BBCount, Environment.NewLine));
            SendMsg(string.Format("bg is {0}{1}", BGCount, Environment.NewLine));
        }

        private void Getmian(JwXian xian, JWMian mian)
        {
            beamxiantempids.Add(xian.Id);
            var z = beamxianIds.Except(beamxiantempids).ToList();
            foreach (var x in z)
            {
                var obj = beamxians.FirstOrDefault(t => t.Id == x);
                if (!object.ReferenceEquals(obj, null))
                //if (obj != null)
                {
                    if (xian.Isxiangjiao(obj))
                    {
                        if (!obj.IsSelected)
                        {

                            bool isadd = true;
                            foreach (var xobj in mian.Xians)
                            {
                                if (obj == xobj)
                                {
                                    isadd = false;
                                    break;
                                }
                            }
                            if (isadd)
                            {
                                obj.IsSelected = true;
                                mian.Xians.Add(obj);
                                Getmian(obj, mian);
                            }

                        }
                    }
                }

            }
        }

        /// <summary>
        /// 切割使用 增加到总的alllinkpart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLinkPartEvent(object sender, AddLinkPartArgs e)
        {
            if (AllLinkPart != null)
            {
                if(e.LinkPart != null)
                {
                    var c=AllLinkPart.Where(t=>t.BjCenterPoint==e.LinkPart.BjCenterPoint&&t.Directed==e.LinkPart.Directed).ToList();
                    if(c.Count()==0)
                    {
                        AllLinkPart.Add(e.LinkPart);
                    }
                }
            }
        }

        List<JwXian> Lianjies;

        private void lianjiexianByJwwsen()
        {
            Lianjies = new List<JwXian>();
            if (LianjieLst.Count > 0)
            {
                foreach (var sen in LianjieLst)
                {
                    JWPoint ps = new JWPoint(sen.m_start_x, sen.m_start_y);
                    JWPoint pe = new JWPoint(sen.m_end_x, sen.m_end_y);
                    JwXian j = new JwXian(ps, pe);
                    Lianjies.Add(j);
                }
            }
        }

        private List<JwChengduiXian> _tempchengduixians = new List<JwChengduiXian>();


        /// <summary>
        /// 先判断成对出现的线条
        /// </summary>
        private void parsenLianjie()
        {
            lianjiexianByJwwsen();

            tempmarks = new List<JwDownPillarMark>();

            if (Lianjies.Count > 0)
            {
                
                for (int i = 0; i < Lianjies.Count; i++)
                {
                    var nowxian = Lianjies[i];
                    if (!nowxian.IsSelected)
                    {
                        for (int j = i + 1; j < Lianjies.Count; j++)
                        {
                            JwLineIntersector lineIntersector = new JwLineIntersector();
                            var erxian = Lianjies[j];
                            if (!nowxian.IsSelected && !erxian.IsSelected)
                            {
                                if (lineIntersector.ComputeIntersect(nowxian, erxian) == 1)
                                {
                                    nowxian.IsSelected = true;
                                    erxian.IsSelected = true;
                                    JwChengduiXian z = new JwChengduiXian
                                    {
                                        XianOne = nowxian,
                                        XianTwo = erxian
                                    };
                                    z.Xians = new List<JwXian> { nowxian, erxian };
                                    _tempchengduixians.Add(z);
                                    break;
                                }
                            }

                        }
                        nowxian.IsSelected = true;
                    }
                }
            }
            if (_tempchengduixians.Count > 0)
            {
                findlianjie();


                var zq = LianjieSingles.Count;
            }

        }

        private List<string> _lianjiebeamids = new List<string>();

        /// <summary>
        /// 对成对的 交叉线进一步筛选
        /// </summary>
        private void findlianjie()
        {
            foreach(var p in _tempchengduixians)
            {
                //判断 duixian 是否是链接交叉 符合这些特性
                processChengduiXian(p);
            }
        }

        public List<JwLianjieSingle> LianjieSingles = new List<JwLianjieSingle>();


        /// <summary>
        /// 2025年6月5日 暂时走不下去
        /// </summary>
        /// <param name="jwChengduiXian"></param>
        private void processChengduiXian(JwChengduiXian jwChengduiXian)
        {
            JwLianjieSingle jwLianjie = findBeam(jwChengduiXian.XianOne);
            if (jwLianjie.IsCreateSuccess)
            {
                LianjieSingles.Add(jwLianjie);
            }
            JwLianjieSingle jwLianjies = findBeam(jwChengduiXian.XianTwo);
            if (jwLianjies.IsCreateSuccess)
            {
                LianjieSingles.Add(jwLianjies);
            }
        }

        /// <summary>
        /// 线的的两个点  使用字典存放 点 和 jwtouch  如果是两个 说明为链接线
        /// 
        /// 存在链接相互垂直的连接线
        /// 2025年6月17日 修改逻辑  起点为左 重点为右  不考虑Y坐标
        /// </summary>
        /// <param name="xian"></param>
        /// <returns></returns>
        private JwLianjieSingle findBeam(JwXian xian)
        {
            JwLianjieSingle jwLianjieSingle = new JwLianjieSingle();
            jwLianjieSingle.IsCreateSuccess = false;
            var xianpoints = xian.OrderByX();
            var fist = xianpoints.First();
            var last = xianpoints.Last();
            Dictionary<JWPoint, JwTouch> findtouchs = new Dictionary<JWPoint, JwTouch>();
            List<TouchType> fintouches = new List<TouchType>();
            List<JwPointBeam> jwPointBeams = new List<JwPointBeam>();
            bool islosershuiping = false;
            bool fone = false;
            bool isfirstlosershuiping = false;
            bool islastlosershuiping = false;
            bool ftwo = false;
            foreach (var b in Touchs)
            {
                //xian.Pone  
                //b.LoserBeam
                //判断  败方是否
                if (b.LoserBeam.ContainShenglue(fist) &&!fone)
                {
                    //增加一层过滤 区分start 和end端
                    var bb = Math.Round(b.JwBeamVertical.IsShuipingLoser ? fist.X : fist.Y,2);
                    islosershuiping = b.JwBeamVertical.IsShuipingLoser;
                    isfirstlosershuiping= b.JwBeamVertical.IsShuipingLoser;

                    //2025年6月21日增加精度处理
                    double jdcha = Math.Abs(b.JwBeamVertical.InitialLoser - bb);


                    if (jdcha<=0.05)
                    {
                        jwPointBeams.Add(touchHandle(b, fist, true));
                        //findtouchs.Add(xian.Pone, b);
                        fone = true;
                    }
                }
                else if (b.LoserBeam.ContainShenglue(last) &&!ftwo)
                {
                    var bb = Math.Round(b.JwBeamVertical.IsShuipingLoser ? last.X : last.Y,2);
                    islosershuiping = b.JwBeamVertical.IsShuipingLoser;
                    double jdcha = Math.Abs(b.JwBeamVertical.InitialLoser - bb);


                    if (jdcha <= 0.05)
                    {
                        jwPointBeams.Add(touchHandle(b, last, false));
                        ftwo = true;
                    }
                }
            }
            // 起始点已经判断完毕
            if (jwPointBeams.Count == 2)
            {
                //BeamDirectionType lianjiewindirect = BeamDirectionType.Horizontal;
                //if (islosershuiping)
                //{
                //    lianjiewindirect = BeamDirectionType.Vertical;
                //    findtouchs = findtouchs.OrderBy(t => t.Key.Y).ToDictionary(o => o.Key, o => o.Value);
                //}
                //else
                //{
                //    lianjiewindirect = BeamDirectionType.Horizontal;
                //    findtouchs = findtouchs.OrderBy(t => t.Key.X).ToDictionary(o => o.Key, o => o.Value);
                //}
                //var z = findtouchs.First();
                //var l = findtouchs.Last();
                ////起始点的方位， 如果败方垂直 则比较水平梁的 高低，此时如果起点Y大于终点Y 则起点位于梁的下方 反之位于上方
                ////             如果败方水平 则比较垂直梁的 左右 如果起点的X 大于总店X 则起点位于梁的左边 反之在左侧
                //ZhengfuType startdirect = ZhengfuType.Add;

                //ZhengfuType enddirect = ZhengfuType.Add;

                //if (islosershuiping)
                //{
                //    if (z.Key.X > l.Key.X)
                //    {
                //        startdirect = ZhengfuType.Reduce;
                //        enddirect = ZhengfuType.Add;
                //    }
                //    else
                //    {
                //        startdirect = ZhengfuType.Add;
                //        enddirect = ZhengfuType.Reduce;
                //    }
                //}
                //else
                //{
                //    if (z.Key.Y > l.Key.Y)
                //    {
                //        startdirect = ZhengfuType.Reduce;
                //        enddirect = ZhengfuType.Add;

                //    }
                //    else
                //    {
                //        startdirect = ZhengfuType.Add;
                //        enddirect = ZhengfuType.Reduce;
                //    }
                //}
                //JwPointBeam start = new JwPointBeam(z.Key, z.Value, true, startdirect);
                JwPointBeam start = jwPointBeams.FirstOrDefault(t => t.IsStart);
                JwPointBeam end = jwPointBeams.FirstOrDefault(t => t.IsEnd);

                jwLianjieSingle.Start = start;
                jwLianjieSingle.End = end;
                //jwLianjieSingle.DirectionType = lianjiewindirect;
                jwLianjieSingle.IsCreateSuccess = true;
            }
            return jwLianjieSingle;
        }

        /// <summary>
        /// 在判定touch上下左右的时候， 增加一层判断，判断beam的holes 距离此点的 置顶范围内
        /// </summary>
        /// <param name="touch"></param>
        /// <param name="point"></param>
        /// <param name="isstart"></param>
        /// <returns></returns>
        public JwPointBeam touchHandle(JwTouch touch,JWPoint point,bool isstart)
        {
            JwPointBeam jwPointBeam=new JwPointBeam(point,touch,isstart);

            //增加一层过滤 区分start 和end端
            var bb = Math.Round(touch.JwBeamVertical.IsShuipingLoser ? point.X : point.Y, 2);

            var bfc = Math.Round(touch.JwBeamVertical.Center, 2);

            if (touch.LoserBeam.DirectionType == BeamDirectionType.Horizontal)
            {
                //判断依据是否是84的两倍
                //增加判断顺序 first 判 是否未截断点 判断上下归属
                //2.判断是否大于2*84
                //3.确定需要增加孔组的 孔组
                //下面
                if (point.Y < touch.LoserBeam.Center)
                {
                    var pdlst = touch.WinnerBeam.Holes.Where(t => t.HoleCenter < point.Y).OrderByDescending(t => t.HoleCenter).ToList();
                    if (touch.WinnerBeam.HasQieGe)
                    {
                        jwPointBeam.Direct = ZhengfuType.Reduce;
                        touch.JwHoleG.HasPreLinkHole = true;
                        var f = touch.WinnerBeam.jwQiegeZus.Find(t => Math.Round(t.Qiegezb, 2) == bfc);
                        if (f != null)
                        {
                            //f.RJwBeam.EndSide.KongZu.HasPreLinkHole = true;
                            f.RJwBeam.holesorder();
                            f.RJwBeam.Holes.Last().HasPreLinkHole = true;
                        }
                    }
                    else
                    {
                        if (pdlst.Count > 0)
                        {
                            var p = pdlst.First();
                            var cha = Math.Abs(p.HoleCenter - point.Y) * JwFileConsts.JwScale;
                            if(cha> 2 * 84)
                            {
                                //需要添加 上下额外孔组的 孔组
                                //p.HasPreLinkHole = true;
                                jwPointBeam.Direct = ZhengfuType.Reduce;
                                touch.JwHoleG.HasPreLinkHole = true;
                            }
                            else
                            {
                                jwPointBeam.Direct = ZhengfuType.Reduce;
                                double realy= p.HoleCenter - (84 / JwFileConsts.JwScale);
                                jwPointBeam.RealPoint=new JWPoint(jwPointBeam.RealPoint.X, realy);
                            }
                            //获取需要添加 上下额外孔组的 孔组
                            //var f = pdlst.First();
                            //f.HasBhLinkHole = true;
                        }
                    }
                }
                else
                {
                    var pdlst = touch.WinnerBeam.Holes.Where(t => t.HoleCenter > point.Y).OrderBy(t => t.HoleCenter).ToList();
                    
                    if (touch.WinnerBeam.HasQieGe)
                    {
                        jwPointBeam.Direct = ZhengfuType.Add;
                        touch.JwHoleG.HasBhLinkHole = true;
                        var f = touch.WinnerBeam.jwQiegeZus.Find(t => Math.Round(t.Qiegezb, 2) == bfc);
                        if (f != null)
                        {
                            //f.AJwBeam.StartSide.KongZu.HasBhLinkHole = true;
                            f.AJwBeam.holesorder();
                            f.AJwBeam.Holes.First().HasBhLinkHole=true;
                        }
                    }
                    else
                    {
                        if (pdlst.Count > 0)
                        {
                            var p = pdlst.First();
                            var cha = Math.Abs(p.HoleCenter - point.Y) * JwFileConsts.JwScale;
                            if (cha > 2 * 84)
                            {
                                //需要添加 上下额外孔组的 孔组
                                //p.HasPreLinkHole = true;
                                jwPointBeam.Direct = ZhengfuType.Add;
                                touch.JwHoleG.HasBhLinkHole = true;
                            }
                            else
                            {
                                jwPointBeam.Direct = ZhengfuType.Add;
                                double realy = p.HoleCenter + (84 / JwFileConsts.JwScale);
                                jwPointBeam.RealPoint = new JWPoint(jwPointBeam.RealPoint.X, realy);
                            }
                            //获取需要添加 上下额外孔组的 孔组
                            //var f = pdlst.First();
                            //f.HasBhLinkHole = true;
                        }
                        else
                        {
                            //没有找到 说明在端部
                            jwPointBeam.Direct = ZhengfuType.Add;
                            touch.JwHoleG.HasBhLinkHole = true;
                        }
                    }
                }
            }
            else
            {
                if (point.X < touch.LoserBeam.Center)
                {
                    var pdlst = touch.WinnerBeam.Holes.Where(t => t.HoleCenter < point.X).OrderByDescending(t => t.HoleCenter).ToList();
                    if (touch.WinnerBeam.HasQieGe)
                    {
                        jwPointBeam.Direct = ZhengfuType.Reduce;
                        touch.JwHoleG.HasPreLinkHole = true;
                        var f = touch.WinnerBeam.jwQiegeZus.Find(t => Math.Round(t.Qiegezb, 2) == bfc);
                        if (f != null)
                        {
                            //f.RJwBeam.EndSide.KongZu.HasPreLinkHole = true;
                            f.RJwBeam.holesorder();
                            f.RJwBeam.Holes.Last().HasPreLinkHole = true;   
                        }
                    }
                    else
                    {
                        if (pdlst.Count > 0)
                        {
                            var p = pdlst.First();
                            var cha = Math.Abs(p.HoleCenter - point.X) * JwFileConsts.JwScale;
                            if (cha > 2 * 84)
                            {
                                //需要添加 上下额外孔组的 孔组
                                //p.HasPreLinkHole = true;
                                jwPointBeam.Direct = ZhengfuType.Reduce;
                                touch.JwHoleG.HasPreLinkHole = true;
                            }
                            else
                            {
                                jwPointBeam.Direct = ZhengfuType.Reduce;
                                double realX = p.HoleCenter - (84 / JwFileConsts.JwScale);
                                jwPointBeam.RealPoint = new JWPoint(realX, jwPointBeam.RealPoint.Y);
                            }
                            //获取需要添加 上下额外孔组的 孔组
                            //var f = pdlst.First();
                            //f.HasBhLinkHole = true;
                        }
                        else
                        {
                            //没有找到 说明在端部
                            jwPointBeam.Direct = ZhengfuType.Reduce;
                            touch.JwHoleG.HasPreLinkHole = true;
                        }
                        //jwPointBeam.Direct = ZhengfuType.Reduce;
                        //touch.JwHoleG.HasPreLinkHole = true;
                    }
                }
                else
                {
                    var pdlst = touch.WinnerBeam.Holes.Where(t => t.HoleCenter > point.X).OrderByDescending(t => t.HoleCenter).ToList();
                    
                    if (touch.WinnerBeam.HasQieGe)
                    {
                        touch.JwHoleG.HasBhLinkHole = true;
                        var f = touch.WinnerBeam.jwQiegeZus.Find(t => Math.Round(t.Qiegezb, 2) == bfc);
                        if (f != null)
                        {
                            //f.AJwBeam.StartSide.KongZu.HasBhLinkHole = true;
                            f.AJwBeam.holesorder();
                            f.AJwBeam.Holes.First().HasBhLinkHole = true;   
                        }
                        jwPointBeam.Direct = ZhengfuType.Add;
                       
                    }
                    else
                    {
                        if (pdlst.Count>0)
                        {
                            var p = pdlst.First();
                            var cha = Math.Abs(p.HoleCenter - point.X) * JwFileConsts.JwScale;
                            if (cha > 2 * 84)
                            {
                                //需要添加 上下额外孔组的 孔组
                                //p.HasPreLinkHole = true;
                                jwPointBeam.Direct = ZhengfuType.Add;
                                touch.JwHoleG.HasBhLinkHole = true;
                            }
                            else
                            {
                                jwPointBeam.Direct = ZhengfuType.Add;
                                //touch.JwHoleG.HasBhLinkHole = true;;
                                double realX = p.HoleCenter + (84 / JwFileConsts.JwScale);
                                jwPointBeam.RealPoint = new JWPoint(realX, jwPointBeam.RealPoint.Y);
                            }
                        }
                    }
                }
            }
            //处理

            return jwPointBeam;
        }


        private void judgehole(JwTouch touch, JWPoint point, bool isstart)
        {

        }
    }
}
