namespace JwSharedConfig.Models
{
    /// <summary>
    /// 2026年11月22日 画面操作に関する本機の設定（appconfig.json に保存。データベースとは無関係）。
    /// </summary>
    public class UiSettings
    {
        /// <summary>出力の種類ごとに最後に選んだ保存先フォルダ（キー：出力の種類）。</summary>
        public Dictionary<string, string> ExportFolders { get; set; } = new();

        /// <summary>2026年11月23日 図の表示モード・レイヤー（キー：画面の種類 parse／floor／canvas）。</summary>
        public Dictionary<string, string> CanvasViews { get; set; } = new();

        /// <summary>2026年11月27日 建物全体の 3D 表示。</summary>
        public Building3DSettings Building3D { get; set; } = new();
    }

    public class Building3DSettings
    {
        /// <summary>階高 mm（梁天端～上の階の梁天端）。</summary>
        public double FloorHeight { get; set; } = 2000;

        /// <summary>工事ごとの設定（キー：工事 Id）。</summary>
        public Dictionary<string, Building3DProjectSettings> Projects { get; set; } = new();
    }

    public class Building3DProjectSettings
    {
        /// <summary>手で並べ替えた階の順（下から、階 Id）。空なら自動の順。</summary>
        public List<string> FloorOrder { get; set; } = new();

        /// <summary>手で調整した位置（キー：階 Id、値：[dx, dy] mm）。無い階は自動で合わせる。</summary>
        public Dictionary<string, double[]> Offsets { get; set; } = new();
    }
}
