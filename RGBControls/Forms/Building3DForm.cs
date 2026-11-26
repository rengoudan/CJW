using System.Drawing;
using JwBeam3D.Building;
using JwCore;
using JwServices;
using JwSharedConfig.Config;
using JwSharedConfig.Models;
using RGBControls.Classes;

namespace RGBControls.Forms
{
    /// <summary>
    /// 2026年11月27日 建物全体の 3D 表示（全階の梁を図面どおりに置き、階の間を単柱・K 柱でつなぐ）。表示のみ。データは書き換えない。
    /// ・階高：設定（appconfig.json、既定 2000 mm＝梁天端～上の階の梁天端）
    /// ・階の順：階名の数字（無ければ取り込んだ順）。↑↓で並べ替え可（工事ごとに記憶）
    /// ・位置合わせ：階ごとに図面の原点が違うため、下の階に合わせて自動で平行移動（柱↔叉線 → 通り芯 → 外形）。
    ///   合わないときは選んだ階の ΔX・ΔY を手で直せる（工事ごとに記憶）。「自動で合わせる」で手の調整を消す。
    /// </summary>
    public sealed class Building3DForm : Form
    {
        private readonly JwProjectMainData _main;
        private readonly string _mainKey;
        private readonly Building3DControl _view = new() { Dock = DockStyle.Fill };
        private readonly NumericUpDown _floorHeight;
        private readonly TrackBar _explode;
        private readonly Label _explodeText;
        private readonly CheckBox _showHoles;
        private readonly CheckBox _showLinks;
        private readonly CheckedListBox _floorList;
        private readonly NumericUpDown _dx, _dy;
        private readonly Label _alignText;

        private List<BuildingFloor> _floors = new();
        private List<FloorOffset> _offsets = new();
        private bool _loading = true;

        public Building3DForm(JwProjectMainData main)
        {
            _main = main;
            _mainKey = main.Id.ToString();
            float k = DeviceDpi / 96f;
            int S(int v) => (int)(v * k);

            Text = $"3D 表示（建物全体）　{main.ProjectName}";
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Microsoft Sans Serif", 10.5f); // UiFont が日本語 UI フォントへ置き換える
            ClientSize = new Size(S(1280), S(800));
            MinimumSize = new Size(S(900), S(600));
            ShowInTaskbar = false;

            var left = new TableLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = S(290),
                Padding = new Padding(S(10)),
                ColumnCount = 1,
                AutoScroll = true,
            };
            Label Caption(string t) => new() { Text = t, AutoSize = true, Margin = new Padding(0, S(8), 0, S(2)) };

            left.Controls.Add(Caption("階高 mm（梁天端～上の階の梁天端）"));
            _floorHeight = new NumericUpDown { Minimum = 300, Maximum = 20000, Increment = 100, Width = S(120), Value = (decimal)Math.Clamp(Settings.FloorHeight, 300, 20000) };
            _floorHeight.ValueChanged += (s, e) =>
            {
                if (_loading) return;
                Settings.FloorHeight = (double)_floorHeight.Value;
                SaveSettings();
                Render();
            };
            left.Controls.Add(_floorHeight);

            left.Controls.Add(Caption("階の間を広げて表示（表示のみ）"));
            _explode = new TrackBar { Minimum = 0, Maximum = 16, Value = 0, TickFrequency = 2, Width = S(250) };
            _explodeText = new Label { AutoSize = true, Text = "+0 mm" };
            _explode.ValueChanged += (s, e) => { _explodeText.Text = $"+{_explode.Value * 250} mm"; if (!_loading) Render(); };
            left.Controls.Add(_explode);
            left.Controls.Add(_explodeText);

            _showHoles = new CheckBox { Text = "梁の穴を表示", Checked = true, AutoSize = true, Margin = new Padding(0, S(8), 0, 0) };
            _showHoles.CheckedChanged += (s, e) => { if (!_loading) Render(); };
            left.Controls.Add(_showHoles);
            _showLinks = new CheckBox { Text = "連接線を表示（梁の左面）", Checked = true, AutoSize = true };
            _showLinks.CheckedChanged += (s, e) => { if (!_loading) Render(); };
            left.Controls.Add(_showLinks);

            left.Controls.Add(Caption("階（上が上の階）　☑＝表示"));
            _floorList = new CheckedListBox { Width = S(250), Height = S(200), CheckOnClick = true, IntegralHeight = false };
            _floorList.ItemCheck += (s, e) => { if (!_loading) BeginInvoke(new Action(Render)); };
            _floorList.SelectedIndexChanged += (s, e) => ShowSelectedOffset();
            left.Controls.Add(_floorList);

            var orderRow = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0) };
            var upBtn = new Button { Text = "↑ 上へ", AutoSize = true };
            var downBtn = new Button { Text = "↓ 下へ", AutoSize = true };
            upBtn.Click += (s, e) => MoveFloor(+1);
            downBtn.Click += (s, e) => MoveFloor(-1);
            orderRow.Controls.Add(upBtn);
            orderRow.Controls.Add(downBtn);
            left.Controls.Add(orderRow);

            left.Controls.Add(Caption("位置合わせ（選んだ階）"));
            _alignText = new Label { AutoSize = true, MaximumSize = new Size(S(260), 0), ForeColor = Color.DimGray };
            left.Controls.Add(_alignText);
            NumericUpDown Offset() => new() { Minimum = -10000000, Maximum = 10000000, Increment = 50, DecimalPlaces = 0, Width = S(120) };
            _dx = Offset();
            _dy = Offset();
            var grid = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, Margin = new Padding(0, S(4), 0, 0) };
            grid.Controls.Add(new Label { Text = "ΔX mm", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            grid.Controls.Add(_dx, 1, 0);
            grid.Controls.Add(new Label { Text = "ΔY mm", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
            grid.Controls.Add(_dy, 1, 1);
            left.Controls.Add(grid);
            _dx.ValueChanged += (s, e) => OnManualOffset();
            _dy.ValueChanged += (s, e) => OnManualOffset();
            var autoBtn = new Button { Text = "自動で合わせる", AutoSize = true };
            autoBtn.Click += (s, e) => ResetSelectedOffset();
            left.Controls.Add(autoBtn);

            var viewBtn = new Button { Text = "視点を戻す", AutoSize = true, Margin = new Padding(0, S(14), 0, 0) };
            viewBtn.Click += (s, e) => _view.ResetCamera();
            left.Controls.Add(viewBtn);

            left.Controls.Add(new Label
            {
                AutoSize = true,
                MaximumSize = new Size(S(260), 0),
                ForeColor = Color.DimGray,
                Margin = new Padding(0, S(14), 0, 0),
                Text = "黄：梁の穴（CSV と同じ位置）。赤い穴は位置に問題があります。\n紫：連接線（始点・終点とも梁の左面）。\n橙の柱：上の階の叉線と位置が合いません。\n赤の印：下の階に柱が無い叉線です。\n位置合わせがずれている場合は ΔX・ΔY で調整してください。",
            });

            Controls.Add(_view);
            Controls.Add(left);
            Shown += async (s, e) => await LoadAsync();
        }

        private static Building3DSettings Settings => AppConfigManager.Current.Ui.Building3D;

        private Building3DProjectSettings Project
        {
            get
            {
                if (!Settings.Projects.TryGetValue(_mainKey, out var p))
                {
                    p = new Building3DProjectSettings();
                    Settings.Projects[_mainKey] = p;
                }
                return p;
            }
        }

        private static void SaveSettings()
        {
            try { AppConfigManager.Save(); }
            catch (Exception ex) { JwLogging.AppLog.Warn(ex, "建物 3D の設定を保存できません"); }
        }

        private async Task LoadAsync()
        {
            try
            {
                var service = ServiceFactory.GetInstance().CreateJwProjectMainService();
                List<(JwProjectSubData Sub, BuildingFloor Floor)> loaded = new();
                await SpinRunner.RunAsync(this, async () => { loaded = await Building3DMapper.LoadAsync(service, _main); });
                _floors = ApplySavedOrder(loaded.Select(x => x.Floor).ToList());
                if (_floors.Count == 0)
                {
                    _view.ShowBuilding(_floors, _offsets, Options());
                    return;
                }
                FillFloorList(selectIndex: 0);
                ComputeOffsets();
                _loading = false;
                ShowSelectedOffset();
                Render();
            }
            catch (Exception ex)
            {
                RGBControls.Errors.ErrorReporter.Report(ex, "建物の 3D 表示を準備できませんでした。");
            }
            finally
            {
                _loading = false;
            }
        }

        private List<BuildingFloor> ApplySavedOrder(List<BuildingFloor> floors)
        {
            var order = Project.FloorOrder;
            if (order.Count == 0) return floors;
            // 記憶した順にある階はその順、新しく増えた階は自動の順のまま後ろ（上）へ
            var byId = floors.ToDictionary(f => f.Id);
            var result = order.Where(byId.ContainsKey).Select(id => byId[id]).ToList();
            result.AddRange(floors.Where(f => !order.Contains(f.Id)));
            return result;
        }

        /// <summary>一覧は上の階を上に表示する（_floors は下の階から）。</summary>
        private int ListIndexOf(int floorIndex) => _floors.Count - 1 - floorIndex;
        private int FloorIndexOf(int listIndex) => _floors.Count - 1 - listIndex;

        private void FillFloorList(int selectIndex)
        {
            bool old = _loading;
            _loading = true;
            var hidden = new HashSet<string>();
            for (int i = 0; i < _floorList.Items.Count; i++)
                if (!_floorList.GetItemChecked(i) && _floorList.Items[i] is FloorItem fi) hidden.Add(fi.Floor.Id);
            _floorList.Items.Clear();
            for (int li = 0; li < _floors.Count; li++)
            {
                var f = _floors[FloorIndexOf(li)];
                _floorList.Items.Add(new FloorItem(f), !hidden.Contains(f.Id));
            }
            if (selectIndex >= 0 && selectIndex < _floorList.Items.Count) _floorList.SelectedIndex = selectIndex;
            _loading = old;
        }

        private sealed class FloorItem
        {
            public BuildingFloor Floor { get; }
            public FloorItem(BuildingFloor f) => Floor = f;
            public override string ToString() => $"{Floor.Name}　梁 {Floor.Beams.Count}・柱 {Floor.Pillars.Count}";
        }

        /// <summary>下の階から順に、手で直した階はその値、ほかは下の階に合わせて自動。</summary>
        private void ComputeOffsets()
        {
            var saved = Project.Offsets;
            _offsets = new List<FloorOffset>();
            for (int i = 0; i < _floors.Count; i++)
            {
                var f = _floors[i];
                if (saved.TryGetValue(f.Id, out var v) && v.Length >= 2)
                    _offsets.Add(new FloorOffset { Dx = v[0], Dy = v[1], IsManual = true, Method = "手で調整" });
                else if (i == 0)
                    _offsets.Add(new FloorOffset { Method = "基準の階" });
                else
                    _offsets.Add(FloorAligner.Align(_floors[i - 1], _offsets[i - 1], f));
            }
        }

        private BuildingViewOptions Options() => new()
        {
            FloorHeight = (double)_floorHeight.Value,
            Explode = _explode.Value * 250,
            VisibleFloors = VisibleIds(),
            ShowHoles = _showHoles.Checked,
            ShowLinks = _showLinks.Checked,
        };

        private HashSet<string> VisibleIds()
        {
            var set = new HashSet<string>();
            for (int i = 0; i < _floorList.Items.Count; i++)
                if (_floorList.GetItemChecked(i) && _floorList.Items[i] is FloorItem fi) set.Add(fi.Floor.Id);
            return set;
        }

        private void Render()
        {
            if (_floors.Count == 0 || _offsets.Count != _floors.Count) return;
            _view.ShowBuilding(_floors, _offsets, Options());
        }

        private int SelectedFloorIndex() => _floorList.SelectedIndex < 0 ? -1 : FloorIndexOf(_floorList.SelectedIndex);

        private void ShowSelectedOffset()
        {
            int i = SelectedFloorIndex();
            if (i < 0 || i >= _offsets.Count) { _alignText.Text = ""; return; }
            bool old = _loading;
            _loading = true;
            var o = _offsets[i];
            _dx.Value = (decimal)Math.Clamp(Math.Round(o.Dx), (double)_dx.Minimum, (double)_dx.Maximum);
            _dy.Value = (decimal)Math.Clamp(Math.Round(o.Dy), (double)_dy.Minimum, (double)_dy.Maximum);
            _alignText.Text = i == 0 && !o.IsManual ? "一番下の階（基準）" : $"{o.Method}";
            _loading = old;
        }

        private void OnManualOffset()
        {
            if (_loading) return;
            int i = SelectedFloorIndex();
            if (i < 0) return;
            Project.Offsets[_floors[i].Id] = new[] { (double)_dx.Value, (double)_dy.Value };
            SaveSettings();
            ComputeOffsets(); // 上の階（自動）はこの階に合わせ直す
            ShowSelectedOffset();
            Render();
        }

        private void ResetSelectedOffset()
        {
            int i = SelectedFloorIndex();
            if (i < 0) return;
            if (Project.Offsets.Remove(_floors[i].Id)) SaveSettings();
            ComputeOffsets();
            ShowSelectedOffset();
            Render();
        }

        /// <param name="dir">+1＝上の階へ、-1＝下の階へ。</param>
        private void MoveFloor(int dir)
        {
            int i = SelectedFloorIndex();
            int j = i + dir;
            if (i < 0 || j < 0 || j >= _floors.Count) return;
            (_floors[i], _floors[j]) = (_floors[j], _floors[i]);
            Project.FloorOrder = _floors.Select(f => f.Id).ToList();
            SaveSettings();
            FillFloorList(ListIndexOf(j));
            ComputeOffsets();
            ShowSelectedOffset();
            Render();
        }
    }
}
