using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwLinkPart:ICanZoom,IChangeAxis
    {
        public string Id { get; set; }

        public string BujianName { get; set; }

        public JWPoint BjCenterPoint { get; set; }

        public string BeamId { get; set; }

        [Newtonsoft.Json.JsonIgnore()]
        /// <summary>
        /// 胜方
        /// </summary>
        public JwBeam ParentBeam { get; set; }

        /// <summary>
        /// 构建朝向
        /// </summary>
        public TaggDirect Directed { get; set; }

        public GouJianType GouJianType { get; set;}

        public GouJianCreateFrom CreateFrom { get; set; }

        /// <summary>
        /// 父梁 由切割产生
        /// </summary>
        public bool IsLianjie { get; set; }

        [Newtonsoft.Json.JsonIgnore()]
        public JwBeam BBeam { get; set; }

        public bool IsNoBeam { get; set; }

        public bool IsDownPillar { get; set; }

        public JwLinkPart()
        {
            Id=Guid.NewGuid().ToString();   
        }

        public JwLinkPartData ToData()
        {
            JwLinkPartData data= new JwLinkPartData();
            data.GouJianType = GouJianType;
            data.Directed = Directed;
            data.IsLianjie = IsLianjie;
            data.BeamId = BeamId;
            data.BujianName= BujianName;
            data.IsNoBeam = IsNoBeam;
            data.Location=BjCenterPoint.ToPoint();
            return data;
        }

        public void Zoom(double zoom)
        {
            BjCenterPoint.Zoom(zoom);
        }

        public void ChangeAxis(double x, double y)
        {
            BjCenterPoint.ChangeAxis(x, y);
        }
    }
}
