using JwCore;
using JwwHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public  class JwTagg
    {
        public string Id { get; set; }
        public string RawString { get; set; }
        public BeamDirectionType DirectionType { get; set; }

        public string Title { get; set; }

        public JWPoint Origin { get; set; }

        public JWPoint Terminal { get; set; }

        public JWPoint Center { get; set; } 

        public bool HasParseSuccess { get; set; }

        [Newtonsoft.Json.JsonIgnore()]
        public JwPillar? NearPillar { get; set; }

        public string PillarId { get; set; }

        public bool HasPillar { get; set; }

        public JwTagg(string chstr)
        {
            Id=Guid.NewGuid().ToString();
            RawString = chstr;
            parseStr(chstr);
        }

        public JwTagg(JwwMoji jwwMoji)
        {
            if (jwwMoji != null)
            {
                double cx=0;
                double cy=0;
                Origin = new JWPoint(jwwMoji.m_start_x, jwwMoji.m_start_y);
                Terminal = new JWPoint(jwwMoji.m_end_x, jwwMoji.m_end_y);
                if (Origin.X == Terminal.X)
                {
                    cx = Terminal.X;
                    cy = Terminal.Y - (Terminal.Y - Origin.Y) / 2;
                    DirectionType = BeamDirectionType.Vertical;
                }
                if(Origin.Y == Terminal.Y)
                {
                    cy = Terminal.Y;
                    cx = Terminal.X - (Terminal.X - Origin.X) / 2;
                    DirectionType = BeamDirectionType.Horizontal;
                }
                Center= new JWPoint(cx, cy);
                Title = jwwMoji.m_string;
                HasParseSuccess = true;
                Id = Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// 書式：ch x1 y1 x2 y2 "文字列
        /// </summary>
        /// <param name="str"></param>
        private void parseStr(string str)
        {
            var q = str.Split(" ").ToList();
            if (q.Count() < 6)
            {
                HasParseSuccess = false;    
            }
            else
            {
                if (q[0] != "ch")
                {
                    HasParseSuccess = false;
                }
                else
                {
                    Origin=new JWPoint(q[1], q[2]);
                    if (q[3] == "0")
                    {
                        DirectionType = BeamDirectionType.Vertical;
                    }
                    if (q[4] =="0")
                    {
                        DirectionType = BeamDirectionType.Horizontal;
                    }
                    HasParseSuccess=true;
                    Title = q[5].Substring(1);
                }
            }
        }

        /// <summary>
        /// 约定  水平在top 垂直在左
        /// 增加文字中点 和所标识对象中间距离最大偏差值
        /// 增加通向 偏差值 比如水平 需要比较Y 值   垂直需要比较X偏差
        /// 判断常量位置
        /// </summary>
        /// <param name="pillars"></param>
        public void SelectOwnPillar(List<JwPillar> pillars)
        {
            var txwcha = JwFileConsts.TextParseMaxDistance / JwFileConsts.JwScale;
            var chuizhiwucha=JwFileConsts.TextParseChuizhiMaxDistance/ JwFileConsts.JwScale;
            if (DirectionType == BeamDirectionType.Horizontal)
            {
                var lst = pillars.Where(t => (t.DirectionType == DirectionType) && !t.HasTag).ToList();
                foreach(var pill in lst)
                {
                    //同向误差
                    if(Center.X<= pill.CenterPoint.X + txwcha && Center.X >= pill.CenterPoint.X - txwcha)
                    {
                        //垂直向误差 水平的 统计向上的
                        if (Center.Y <= pill.CenterPoint.Y + chuizhiwucha && Center.Y >= pill.CenterPoint.Y)
                        {
                            NearPillar = pill;
                            PillarId = NearPillar.Id;
                            pill.Tagg = this;
                            pill.TagId = Id;
                            pill.TagName = Title;
                            pill.HasTag = true;
                            HasPillar = true;
                        }
                    }
                }
            }
            else
            {
                var lst = pillars.Where(t => ((t.DirectionType == DirectionType)||t.BaseType==PillarBaseType.SinglePillar) && !t.HasTag).ToList();
                foreach (var pill in lst)
                {
                    //同向误差
                    if (Center.Y <= pill.CenterPoint.Y + txwcha && Center.Y >= pill.CenterPoint.Y - txwcha)
                    {
                        //垂直 规则为标注在柱的右边
                        if (Center.X >= pill.CenterPoint.X - chuizhiwucha && Center.X <= pill.CenterPoint.X)
                        {
                            NearPillar = pill;
                            PillarId = NearPillar.Id;
                            pill.Tagg = this;
                            pill.TagId = Id;
                            pill.TagName = Title;
                            pill.HasTag = true;
                            HasPillar = true;
                        }
                    }
                }
            }
            //var lst = pillars.Where(t => t.DirectionType == DirectionType&&!t.HasTag).ToList();
            //if(DirectionType == BeamDirectionType.Vertical)
            //{
            //    //var q=lst.Where(t => ((t.TopLeft.X - Origin.X) < 10)||((Origin.X-t.TopRight.X)<10)).ToList();
            //    var q = lst.Where(t => ((t.TopLeft.X - Origin.X) < 10)).ToList();
            //    if (q.Count >0)
            //    {
            //        //var zz = q.OrderBy().ToList();
            //        foreach (var z in q)
            //        {
            //            if (Origin.Y >= z.BottomRight.Y && Origin.Y <= z.TopRight.Y)
            //            {
            //                NearPillar = z;
            //                PillarId = NearPillar.Id;
            //                z.Tagg = this;
            //                z.TagId = Id;
            //                z.TagName = Title;
            //                z.HasTag = true;
            //                break;
            //            }
            //        }
            //    }
            //    //else if (q.Count > 0)
            //    //{
            //    //    var z = q.First();
            //    //    if (Origin.Y >= z.BottomRight.Y && Origin.Y <= z.TopRight.Y)
            //    //    {
            //    //        NearPillar = z;
            //    //        PillarId = NearPillar.Id;
            //    //        z.Tagg = this;
            //    //        z.TagId = Id;
            //    //        z.TagName = Title;
            //    //        z.HasTag = true;
            //    //    }
            //    //    //PillarId = NearPillar.Id;
            //    //}
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    //var q = lst.Where(t => (Origin.Y - t.TopLeft.Y) < 10||(t.BottomLeft.Y-Origin.Y)<10).ToList();
            //    var q = lst.Where(t => (Origin.Y - t.TopLeft.Y) < 10).ToList();
            //    if (q.Count() > 0)
            //    {
            //        var zz= q.ToList();
            //        foreach (var z in zz)
            //        {
            //            if (Origin.X >= z.BottomLeft.X && Origin.X <= z.TopRight.X)
            //            {
            //                NearPillar = z;
            //                PillarId = NearPillar.Id;
            //                z.Tagg = this;
            //                z.TagId = Id;
            //                z.TagName = Title;
            //                z.HasTag = true;
            //                break;
            //            }
            //        }
                    
            //        //PillarId = NearPillar.Id;
            //    }
            //    //else if(q.Count> 0)
            //    //{
            //    //    var z = q.First();
            //    //    if (Origin.X >= z.BottomLeft.X && Origin.X <= z.TopRight.X)
            //    //    {
            //    //        NearPillar = z;
            //    //        PillarId = NearPillar.Id;
            //    //        z.Tagg = this;
            //    //        z.TagId = Id;
            //    //        z.TagName = Title;
            //    //        z.HasTag = true;
            //    //    }
            //    //    //NearPillar = q.First();
            //    //    //PillarId = NearPillar.Id;
            //    //}
            //    else
            //    {

            //    }
            //}
            //if(!object.ReferenceEquals(NearPillar,null))
            //{
            //    NearPillar.Tagg = this;
            //    NearPillar.TagId = Id;
            //    NearPillar.TagName = Title;
            //}
            
        }

        public JwTagRange TagRange { get; set; }
        
    }
}
