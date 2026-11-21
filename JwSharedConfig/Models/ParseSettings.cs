namespace JwSharedConfig.Models
{
    /// <summary>
    /// 解析相关的本机设置（保存在 appsetting JSON 中，不涉及数据库结构）。
    /// </summary>
    public class ParseSettings
    {
        /// <summary>
        /// 2026年11月21日 试验开关：识别梁时排除"退化矩形"（4 个顶点中有坐标重复、面积为 0 的候选）。
        /// 默认关闭（与原有行为相同）。详见 JwFileHandle.Rectangles.cs。
        /// </summary>
        public bool ExcludeDegenerateRectangles { get; set; } = false;
    }
}
