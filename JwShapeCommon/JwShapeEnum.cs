using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{

    public enum DrawShapeType
    {
        None = 0,
        Beam = 1,
        Block = 2,
        Pillar = 3
    }

    /// <summary>
    /// beam方向 右上为
    /// </summary>
    public enum BeamDirectionType
    {
        Horizontal = 0, 
        Vertical = 1,
        Youshang=2,
        YouXia=3
    }

    /// <summary>
    /// 块形状 sl 意味的形状
    /// </summary>
    public enum JwBlockShapeType
    {
        Triangular = 0,
        Square = 1,
        Polygon = 2,
    }
    /// <summary>
    /// 描述识别的文字的 类型
    /// 分为 设计图批注 批注只考虑beam 及pillar
    /// 设计图描述 
    /// </summary>
    public enum JwTagRange
    {
        CommentTag = 0,
        DescribeTag = 1
    }

    /// <summary>
    /// 指示三角 指向分割的方向 不考虑斜向的梁
    /// 构建的朝向也可以使用这个
    /// </summary>
    public enum TaggDirect
    {
        Up = -1,
        Down = 1,
        Left = -2,
        Right = 2
    }

    /// <summary>
    /// 构建的类型
    /// </summary>
    public enum GouJianType
    {
        None = 0,
        B = 1,
        BG = 2
    }


    /// <summary>
    /// 孔组类型 0为 非两端的孔组  》0则为端部类型
    /// </summary>
    public enum KongzuType
    {
        Center = 0,
        B = 1,
        J = 2,
        G = 3,
    }

    public enum KongzuSuoshuMian
    {
        All=0,
        Top = 1,
        Center = 2,
        Bottom = 3
    }
}
