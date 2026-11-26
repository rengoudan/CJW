using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace JwBeam3D.Building
{
    /// <summary>2026年11月27日 建物全体の 3D 表示（WinForms 用）。</summary>
    public sealed class Building3DControl : UserControl
    {
        private readonly Building3DView _view = new();
        private bool _shownOnce;

        public Building3DControl()
        {
            Controls.Add(new ElementHost { Dock = DockStyle.Fill, Child = _view });
        }

        /// <summary>表示する。最初の 1 回だけ視点を全体が見える位置にする。</summary>
        public void ShowBuilding(IReadOnlyList<BuildingFloor> floors, IReadOnlyList<FloorOffset> offsets, BuildingViewOptions options)
        {
            try
            {
                if (floors.Count == 0)
                {
                    _view.ShowMessage("表示できる階がありません。");
                    return;
                }
                _view.Show(floors, offsets, options, resetCamera: !_shownOnce);
                _shownOnce = true;
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "建物 3D 表示に失敗");
                _view.ShowMessage("3D 表示に失敗しました: " + ex.Message);
            }
        }

        public void ResetCamera() => _view.ResetCamera();
    }
}
