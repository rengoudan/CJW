using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace JwCore
{
    public class JwSquareBaseData:BaseGuidEntityData
    {

        //一个点 也够用
        public Point? Location { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }  

        /// <summary>
        /// 从jww 读取的比例
        /// </summary>
        public double Scale { get; set; }

        public BeamDirectionType DirectionType { get; set; }

        //public Point? TopLeft { get; set; }

        //public Point? TopRight { get; set;}

        //public Point? BottomLeft { get; set; }

        //public Point? BottomRight { get; set; }
        
    }
}
