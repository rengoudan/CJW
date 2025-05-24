using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{

    /// <summary>
    /// 项目状态
    /// </summary>
    public enum ProjectStatus
    {
         Create=0,
         Parsed=1,
         Budget=2
    }

    /// <summary>
    /// 预算类型 0为 图纸识别 自动添加 1 自动识别 缺少预算项目 2 为手动增加
    /// </summary>
    public enum BudgetFrom
    {
        自動識別 = 0,
        識別失敗 = 1,
        カスタム予算 = 2
    }

    /// <summary>
    /// 操作分成两种 0 整个项目 1 具体楼层
    /// </summary>
    public enum OperateLevel
    {
        Project = 0,
        Sub = 1
    }


    public enum OperateType
    {
        预算 = 0,
        生成设计图 = 1
    }

    
    public enum MaterialType
    {
        梁 = 0,
        柱 = 1,
        金物 = 2
    }
    public enum DrawShapeType
    {
        None = 0,
        Beam = 1,
        Block = 2,
        Pillar = 3,
        Hole = 4
    }

    /// <summary>
    /// beam方向 右上为
    /// </summary>
    public enum BeamDirectionType
    {
        Horizontal = 0,
        Vertical = 1,
        Youshang = 2,
        YouXia = 3
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
    /// 构建创建原因
    /// 0只是由 g产生 的bg
    /// 1 由上柱子
    /// 2 由下柱
    /// 3 上下都有柱
    /// </summary>
    public enum GouJianCreateFrom
    {
        OnlyG=0,
        UpPillar=1,
        DownPillar=2,
        BothPillar=3
    }


    /// <summary>
    /// 2025年4月10日
    /// 孔组类型 0为 非两端的孔组  》0则为端部类型
    /// 增加BC 为>=150时候B端为完整4个孔
    /// 增加BP 为<150的时候B端为部分2个
    /// BCBP孔组形态都为4个完整，BP 为首的时候右侧缺少垂直两个孔 为末的时候左侧缺少垂直两个孔
    /// </summary>
    public enum KongzuType
    {
        Center = 0,
        B = 1,
        J = 2,
        G = 3,
        BC = 4,
        BP = 5
    }

    public enum KongzuSuoshuMian
    {
        All = 0,
        Top = 1,
        Center = 2,
        Bottom = 3
    }

    public enum PillarBaseType
    {
        SinglePillar = 0,
        KPillar = 1
    }

    /// <summary>
    /// 指示孔源自 哪 柱 上面 G 中间 链接？待确定影响哪里
    /// </summary>
    public enum HoleCreateFrom
    {
        Pillar = 0,
        JieChuG = 1,
        FengeJ = 2,
        JieChu=3//指 胜方 打孔
    }

    /// <summary>
    /// 梁端类型
    /// </summary>
    public enum PortType
    {
        Start = 0,
        End = 1
    }

    /// <summary>
    /// 用来取代  链接 上下左右
    /// </summary>
    public enum ZhengfuType
    {
        Reduce = -1,
        Add = 1
    }
}
