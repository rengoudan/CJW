using JwCore;
using RGB.Jw.JW.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwParseSub
    {
        public List<JWMian> Mians { get; set; }
        private List<JWMian> _tempmians { get; set; }

        private List<JwXian> beamxians { get; set; }

        private List<JWPoint> beampoints { get; set; }

        public long ProjectId { get; set; }

        public string SubName { get; set; }

        public string Biaochi { get; set; }

        public int HorizontalBeamsCount { get; set; }

        public int VerticalBeamsCount { get; set; }

        public int BeamsCount { get; set; }

         /// <summary>
        /// 柱子总数
        /// </summary>
        public int PillarCount { get; set; }

        /// <summary>
        /// k
        /// </summary>
        public int KPillarCount { get; set; }

        public bool IsParseSubName { get; set; }

        public string UploadToken { get; set; }

        /// <summary>
        /// 初次识辨出的块
        /// </summary>
        public List<JwBlock> Blocks { get; set; }

        /// <summary>
        /// 四方块 之选定内容里的柱
        /// </summary>
        public List<JwBlock> SquareBlocks { get; set; }

        /// <summary>
        /// 识别后的柱
        /// </summary>
        public List<JwPillar> Pillars { get; set; }

        /// <summary>
        /// 识别后的标注
        /// </summary>
        public List<JwTagg> Tags { get; set; }

        /// <summary>
        /// 标注数量
        /// </summary>
        public int TagsCount { get; set; }

        public List<JwBeam> Beams { get; set; }

        public List<JwBeam> HorizontalBeams { get; set; }

        /// <summary>
        /// 垂直组
        /// </summary>
        public List<JwBeam> VerticalBeams { get; set; }

        public List<double> RowsPointY { get; set; }

        public List<double> ColumnPointX { get; set; }

        public List<JWPoint> CenterPoints { get; set; }

        public JwTempHelper jwTempHelper { get; set; }

        public SettingObject Settingobj { get; set; }
         
        public string DefaultBeamType { get; set; }

        public string BeamTypeStr { get; set; }

        private static JwParseSub _jwparsesub;

        public static JwParseSub GetJwparsesub()
        {
            if(_jwparsesub == null)
            {
                _jwparsesub = new JwParseSub();
            }
            return _jwparsesub;
        }

        public JwParseSub() 
        {
            Mians = new List<JWMian>();
            Blocks = new List<JwBlock>();
            SquareBlocks = new List<JwBlock>();
            Pillars = new List<JwPillar>();
            Tags = new List<JwTagg>();
        }

        /// <summary>
        /// 第一步
        /// </summary>
        /// <param name="readlines"></param>
        /// <param name="setting"></param>
        public void init(string[] readlines)
        {
            
            jwTempHelper = new JwTempHelper(readlines, Settingobj);
            parseSubName();
            //this.ParseBeamString(jwTempHelper.beampoints);//生成beam
            //this.PareBeamByMian();
        }

        /// <summary>
        /// 约定 空格 间隔标尺与名称
        /// </summary>
        private void parseSubName()
        {
            string str = "";
            if (jwTempHelper != null)
            {
                //jwTempHelper.s
                if(jwTempHelper.taggstrs.Count> 0)
                {
                    foreach (var item in jwTempHelper.taggstrs)
                    {
                        if (item.Contains("伏 図") || item.Contains("伏"))
                        {
                            str = item.TrimEnd().TrimEnd();
                            break;
                        }
                    }
                }
            }
            if(!string.IsNullOrEmpty(str))
            {
                int a = str.LastIndexOf(" ");
                string tem = str.Substring(0, a);
                int it = str.IndexOf('\"');
                SubName = str.Substring(it + 1, a - it).Trim();
                Biaochi = str.Substring(a + 1);
                IsParseSubName = true;
                if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
                {
                    GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("SubName:{0}", SubName) });
                    GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Biaochi:{0}", Biaochi) });
                }
            }
        }

        #region beam 
        public bool ParseBeamFlag { get; set; }
        public string pareBeamErrormsg { get; set; }
        public string ConverttoMianMsg { get; set; }

        /// <summary>
        /// 左上
        /// </summary>
        public JWPoint TopLeft { get; set; }

        /// <summary>
        /// 右下
        /// </summary>
        public JWPoint BottomRight { get; set; }


        private List<string> beamxiantempids;
        private List<string> beamxianIds;

        public void CreateBeam()
        {
            this.ParseBeamString(jwTempHelper.beampoints);//生成beam
            this.PareBeamByMian();
            CreateTopLeftBottomRight();
        }

        private void PareBeamByMian()
        {
            Beams = new List<JwBeam>();
            foreach (var item in Mians)
            {
                Beams.Add(new JwBeam(item));
            }
            BeamsCount=Beams.Count;
            HorizontalBeams = Beams.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
            HorizontalBeamsCount=HorizontalBeams.Count;
            VerticalBeams = Beams.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
            VerticalBeamsCount=VerticalBeams.Count;
            if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
            {
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("HorizontalBeams total:{0}", HorizontalBeams.Count) });
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("VerticalBeams:{0}", VerticalBeams.Count) });
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Beams:{0}", Beams.Count) });

            }
            RowsPointY = HorizontalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            ColumnPointX = VerticalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            CenterPoints = Beams.Select(t => t.CenterPoint).OrderBy(t => t.X).ThenBy(t => t.Y).ToList();
            if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
            {
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Horizontal rows Y total:{0}", RowsPointY.ToString()) });
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Vertical rows X total:{0}", ColumnPointX.ToString()) });
                //GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Beams:{0}", Beams.Count) });

            }
        }

        private void CreateTopLeftBottomRight()
        {
            if(Beams.Count > 0)
            {
                double topx = Beams.Select(t => t.TopLeft.X).Min();
                double topY = Beams.Select(t => t.TopLeft.Y).Max();
                TopLeft = new JWPoint(topx, topY);
                double bottomx = Beams.Select(t => t.BottomRight.X).Max();
                double bottomY = Beams.Select(t => t.BottomRight.Y).Min();
                BottomRight = new JWPoint(bottomx, bottomY);
            }
        }

        private void ParseBeamString(List<string> beamstrs)
        {
            if (beamstrs.Count > 0)
            {
                beamxians = new List<JwXian>();
                beampoints = new List<JWPoint>();
                beamxianIds = new List<string>();
                foreach (string z in beamstrs)
                {
                    string z1 = z.TrimStart().TrimEnd();
                    JwXian j = new JwXian(z1);
                    beamxianIds.Add(j.Id);
                    beamxians.Add(j);
                    beampoints.AddRange(j.GetXianPoints());
                }
                ConvertXiantoMians();
            }
            else
            {
                ParseBeamFlag = false;
                pareBeamErrormsg = "未读取到数据";
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
                        Mians.Add(mian);
                        Getmian(xian, mian);
                    }

                }
            }

            _tempmians = Mians;
            Mians.ForEach(mian => { mian.XianCout = mian.Xians.Count; });
            Mians = Mians.Where(t => t.XianCout == 4).ToList();
            ConverttoMianMsg = string.Format("4 beam {0},parse all beam {1}", Mians.Count, _tempmians.Count);
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
                            obj.IsSelected = true;
                            mian.Xians.Add(obj);
                            Getmian(obj, mian);
                        }
                    }
                }

            }
        }

        #endregion

        #region block
        public void CreateBlocks()
        {
            foreach (var sl in jwTempHelper.shapestrs)
            {
                this.Blocks.Add(new JwBlock(sl));
            }
            ParseTriangularBlocks();
            ParseSquareCreatePillar();
            if(Pillars.Count > 0)
            {
                if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
                {
                    GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Pillars total:{0}", Pillars.Count) });
                    //GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Vertical rows X total:{0}", ColumnPointX.ToString()) });
                    //GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Beams:{0}", Beams.Count) });
                }
            }
        }

        /// <summary>
        /// 处理有的设计师 两个三角表示正方形
        /// </summary>
        public void ParseTriangularBlocks()
        {
            List<JwBlock> sanjiaolst = Blocks.Where(t => t.ShapeType == JwBlockShapeType.Triangular).ToList();
            List<JwBlock> sanjiaolsts = Blocks.Where(t => t.ShapeType == JwBlockShapeType.Triangular).ToList();
            List<string> tempids = new List<string>();
            foreach (var jb in sanjiaolst)
            {
                if (!tempids.Contains(jb.Id))
                {
                    foreach (var q in sanjiaolsts)
                    {
                        if (jb.Id != q.Id)
                        {
                            if (jb.BlockPoint.Intersect(q.BlockPoint, new JwPointComparint()).ToList().Count == 2)
                            {
                                var hebingpoints = jb.BlockPoint.Union(q.BlockPoint, new JwPointComparint()).ToList();
                                if (hebingpoints.Count == 4)
                                {
                                    tempids.Add(q.Id);
                                    SquareBlocks.Add(new JwBlock(hebingpoints));
                                }
                            }
                        }
                    }
                }
            }

            List<JwBlock> zhengfangxing = SquareBlocks.Where(t => t.ShapeType == JwBlockShapeType.Square).ToList();
            foreach (var z in zhengfangxing)
            {
                var Q = Math.Round((z.TopRight.X - z.TopLeft.X), 0);
                var b= Math.Round((z.TopLeft.Y - z.BottomLeft.Y), 0);
                if (Q==b)
                {
                    z.Iszhengfangxing = true;
                    z.ZhengfangCenter = new JWPoint
                    {
                        X = (z.TopRight.X - z.TopLeft.X) / 2,
                        Y = (z.TopRight.Y - z.BottomRight.Y) / 2
                    };
                }
                SquareBlocks.Add(z);
            }

        }

        List<string> tempsquareid;

        public int SinglePillarCount { get; set; }

        /// <summary>
        /// 判断组合矩形
        /// </summary>
        public void ParseSquareCreatePillar()
        {
            if (SquareBlocks.Count > 0)
            {
                tempsquareid = new List<string>();
                foreach (var block in SquareBlocks)
                {
                    if (!tempsquareid.Contains(block.Id))
                    {
                        JwPillar pillar = new JwPillar();
                        Pillars.Add(pillar);
                        pillar.Blocks.Add(block);
                        tempsquareid.Add(block.Id);
                        DiguigetXianglin(block, pillar);
                    }
                }
                PillarCount = Pillars.Count();
                SinglePillarCount = Pillars.Where(t => t.Blocks.Count(q=>q.Iszhengfangxing)==1).Count();
                KPillarCount=Pillars.Where(t=>t.Blocks.Count == 3&&t.Blocks.Count(q=>q.Iszhengfangxing)==2).Count();

            }

            if(Pillars.Count > 0)
            {
                foreach (var item in Pillars)
                {
                    item.squareParse();
                }
            }
        }

        private void DiguigetXianglin(JwBlock block, JwPillar pillar)
        {

            foreach (var other in SquareBlocks)
            {
                if (!tempsquareid.Contains(other.Id))
                {
                    if (block.Isjiechu(other))
                    {
                        tempsquareid.Add(other.Id);
                        pillar.Blocks.Add(other);
                        DiguigetXianglin(other, pillar);
                    }
                }


            }
        }
        #endregion

        #region tag

        /// <summary>
        /// 在pillar之后
        /// </summary>
        public void CreateTagg()
        {
            if (jwTempHelper.taggstrs.Count > 0)
            {
                foreach (var slstr in jwTempHelper.taggstrs)
                {
                    if ((slstr.IndexOf("X") == -1) && slstr.IndexOf("Y")== -1)
                    {
                        Tags.Add(new JwTagg(slstr));
                    }
                    
                }
            }
            TagsCount = Tags.Count;
            if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
                GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("Tagg total:{0}", Tags.Count) });
            List<IGrouping<string,JwTagg>> tgroup=Tags.GroupBy(t=>t.Title).ToList();
            foreach (var group in tgroup)
            {
                if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
                    GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("{0} tagg total:{1}", group.Key, group.Count()) });
            }
            if (Tags.Count > 0&&Pillars.Count>0)
            {
                foreach (var tag in Tags)
                {
                    if (tag.Title.IndexOf("X")== -1)
                    {
                        tag.SelectOwnPillar(Pillars);
                    }
                    
                }
            }
            parseTaggPillar();
            List<IGrouping<string, JwPillar>> gpillars = Pillars.GroupBy(t => t.TagName).ToList();
            foreach(var group in gpillars)
            {
                if (GlobalEvent.GetGlobalEvent().ShowParseLogEvent != null)
                    GlobalEvent.GetGlobalEvent().ShowParseLogEvent(this, new ShowParseLogArgs { Msg = string.Format("{0}Pillars total:{1}", group.Key, group.Count()) });
            }

            parseTagRange();
        }

        public void parseTaggPillar()
        {
            var tlst = Tags.Where(t => string.IsNullOrEmpty(t.PillarId)).ToList();
            var plst = Pillars.Where(t => !t.HasTag).ToList();
            var shuipingtlst = tlst.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
            var shuipingplst = plst.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
            foreach (var z in shuipingtlst)
            {
                var q = shuipingplst.Where(t => t.BottomRight.Y - z.Origin.Y < 500).ToList();
                if (q.Count > 0)
                {
                    var qobj = q.OrderBy(t => Math.Abs(z.Origin.X - t.TopLeft.X)).First();
                    if (qobj.BlocksCount == 3)
                    {
                        if (z.Origin.X >= qobj.TopLeft.X && z.Origin.X <= qobj.TopRight.X)
                        {
                            z.NearPillar = qobj;
                            z.PillarId = qobj.Id;
                            qobj.Tagg = z;
                            qobj.TagId = z.Id;
                            qobj.TagName = z.Title;
                            qobj.HasTag = true;
                        }
                    }
                }

            }
            var chuizhitlst = tlst.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
            var chuizhiplst = plst.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
            foreach (var z in chuizhitlst)
            {
                var q = chuizhiplst.Where(t => z.Origin.X - t.TopRight.X < 500).ToList();
                if (q.Count > 0)
                {
                    var qobj = q.OrderBy(t => Math.Abs(z.Origin.Y - t.BottomRight.Y)).First();
                    if (qobj.BlocksCount == 3)
                    {
                        if (z.Origin.Y >= qobj.BottomRight.Y && z.Origin.X <= qobj.TopRight.Y)
                        {
                            z.NearPillar = qobj;
                            z.PillarId = qobj.Id;
                            qobj.Tagg = z;
                            qobj.TagId = z.Id;
                            qobj.TagName = z.Title;
                            qobj.HasTag = true;
                        }
                    }
                }
            }
        }

        public List<JwCustomerDesignTagClientDto> CustomerDesignTagClientDtos { get; set; }

        /// <summary>
        /// 判断识别出的文字 归属，设计图批注 还是设计图描述
        /// </summary>
        public void parseTagRange()
        {
            if (!object.ReferenceEquals(TopLeft, null) && !object.ReferenceEquals(BottomRight, null))
            {
                if (Tags.Count > 0)
                {
                    foreach (var tag in Tags)
                    {
                        if (tag.Origin.X >= TopLeft.X && tag.Origin.X <= BottomRight.X && tag.Origin.Y >= BottomRight.Y && tag.Origin.Y <= TopLeft.Y)
                        {
                            tag.TagRange = JwTagRange.CommentTag;
                        }
                        else
                        {
                            tag.TagRange = JwTagRange.DescribeTag;
                            //统一规范 例如 铁骨beam G1 类似 此处做匹配

                            if(CustomerDesignTagClientDtos.Count > 0)
                            {
                                var lst = CustomerDesignTagClientDtos.Where(t => t.TagRange == "DescribeTag").ToList();
                                if (lst.Count > 0)
                                {
                                    foreach(var t in lst)
                                    {
                                        if (tag.Title.Contains(t.ComponentsName) && tag.Title.Contains(t.DesignSymbol))
                                        {
                                            DefaultBeamType = t.DesignSymbol;
                                            BeamTypeStr = tag.Title;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(DefaultBeamType))
            {
                if(Beams.Count > 0)
                {
                    foreach(var  beam in Beams)
                    {
                        beam.BeamCode= DefaultBeamType;
                    }
                }
            }
        }

        #endregion


        public int BBCount { get; set; }

        public int BGCount { get; set; }

        public void parsePillarBeam()
        {
            JwBeamDeepParse deepParse = new JwBeamDeepParse(Beams);

            RowsPointY = HorizontalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            ColumnPointX = VerticalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            
            List<IGrouping<double, JwBeam>> shuipinggroup = HorizontalBeams.GroupBy(t => t.BottomLeft.Y).OrderByDescending(t => t.Key).ToList();

            var shuipingpillars = Pillars.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();

            foreach(var b in HorizontalBeams)
            {
                foreach(var a in shuipingpillars)
                {
                    if(a.TopLeft.X>=b.TopLeft.X&&a.TopRight.X<=b.TopRight.X&&a.TopLeft.Y<=b.TopLeft.Y&&a.BottomLeft.Y>=b.BottomLeft.Y)
                    {
                        b.BeamPillars.Add(a);
                        foreach(var c in a.Blocks)
                        {
                            if(c.Iszhengfangxing)
                            {
                                JwLinkPart jbb = new JwLinkPart();
                                jbb.BujianName = "B";
                                jbb.BjCenterPoint = new JWPoint
                                {
                                    X = c.ZhengfangCenter.X,
                                    Y = c.ZhengfangCenter.Y
                                };
                                jbb.ParentBeam = b;
                                jbb.BeamId = b.Id;
                                if (b.LinkParts.Where(t => t.BjCenterPoint == jbb.BjCenterPoint&&t.BujianName=="BG").Count() > 0)
                                {
                                    b.LinkParts.Add(jbb);
                                }
                                else
                                {
                                    b.LinkParts.Add(jbb);
                                    JwLinkPart jb1 = new JwLinkPart();
                                    jb1.BujianName = "B";
                                    jb1.BeamId = b.Id;
                                    jb1.BjCenterPoint = new JWPoint
                                    {
                                        X = c.ZhengfangCenter.X,
                                        Y = c.ZhengfangCenter.Y
                                    };
                                    jb1.ParentBeam = b;
                                    b.LinkParts.Add(jb1);
                                }
                            }
                        }
                    }
                }
            }

            var chuizhi=Pillars.Where(t=>t.DirectionType==BeamDirectionType.Vertical).ToList(); 
            foreach(var b in VerticalBeams)
            {
                foreach(var a in chuizhi)
                {
                    if (a.TopLeft.X >= b.TopLeft.X && a.TopRight.X <= b.TopRight.X && a.TopRight.Y <= b.TopRight.Y && a.BottomLeft.Y >= b.BottomLeft.Y)
                    {
                        b.BeamPillars.Add(a);
                        foreach (var c in a.Blocks)
                        {
                            if (c.Iszhengfangxing)
                            {
                                JwLinkPart jbb = new JwLinkPart();
                                jbb.BujianName = "B";
                                jbb.BjCenterPoint = new JWPoint
                                {
                                    X = Math.Round(c.ZhengfangCenter.X, 0),
                                    Y = Math.Round(c.ZhengfangCenter.Y, 0)
                                };
                                jbb.ParentBeam = b;
                                jbb.BeamId = b.Id;
                                if (b.LinkParts.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.BujianName == "BG").Count() > 0)
                                {
                                    b.LinkParts.Add(jbb);
                                }
                                else
                                {
                                    b.LinkParts.Add(jbb);
                                    JwLinkPart jb1 = new JwLinkPart();
                                    jb1.BujianName = "B";
                                    jb1.BeamId = b.Id;
                                    jb1.BjCenterPoint = new JWPoint
                                    {
                                        X = Math.Round(c.ZhengfangCenter.X, 0),
                                        Y = Math.Round(c.ZhengfangCenter.Y, 0)
                                    };
                                    jb1.ParentBeam = b;
                                    b.LinkParts.Add(jb1);
                                }
                            }
                        }
                    }
                }
            }
            BBCount = Beams.Sum(t => t.LinkParts.Count(q=>q.BujianName=="B"));
            BGCount = Beams.Sum(t => t.LinkParts.Count(q => q.BujianName == "BG"));

        }

    }
}
