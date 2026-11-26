using AntdUI;
using AntdUI.Svg;
using JwCore;
using JwServices;
using JwShapeCommon;
using JwwHelper;
using Org.BouncyCastle.Asn1.Pkcs;
using RGBControls.Classes;
using RGBControls.Controls;
using RGBControls.Forms;
using RGBJWMain.Forms;
using RGBJWMain.Pages;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Pages
{
    public partial class NewProjectMainPage : BasePage
    {
        public NewProjectMainPage()
        {
            InitializeComponent();
            Initform();
        }

        Form _parentForm;
        public NewProjectMainPage(Form form)
        {
            InitializeComponent();
            Initform();
            _parentForm = form;
        }

        private JwProjectMainService JwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        AntdUI.IContextMenuStripItem[] menulist = { };


        IContextMenuStripItem[] mainmenulist = { };

        JwProjectSubData selectedsubData;

        JwProjectMainData _selectedMainData;

        private void Initform()
        {
            projectmaintable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ProjectName", "工事名"),
                                new AntdUI.Column("Id", "状態") { Render = RenderProjectStatus }, // 2026年11月22日
                                new AntdUI.Column("Biaochi", "縮尺"),
                                new AntdUI.Column("BeamsNumber", "梁数"),
                                new AntdUI.Column("KPillarCount", "K 柱 トータル"),
                                new AntdUI.Column("SinglePillarCount", "単柱"),
                                new AntdUI.Column("CustomerName", "顧客名"),
                                new AntdUI.Column("BCount", "B"),
                                new AntdUI.Column("BGCount", "BG"),
                                new AntdUI.Column("FloorQuantity", "階数"),
                                new AntdUI.Column("ParsedQuantity", "解析数")

            };
            this.table1.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("FloorName", "階"),
                                new AntdUI.Column("ExportCount", "状態") { Render = RenderFloorStatus }, // 2026年11月22日
                                new AntdUI.Column("Biaochi", "縮尺"),
                                new AntdUI.Column("MarkBeam", "符号"),
                                new AntdUI.Column("LianjieCompentType", "BR8/VPL"),
                                new AntdUI.Column("MinimalStrategy", "穴あけ方式")
                                {
                                    Render = (value, record, i_row) =>
        {
            bool b = value is bool v && v;
            return b ? "穴あけ最小化" : "通常の穴あけ";   // true 显示 XX，false 显示 YY
        }
                                },
                                new AntdUI.Column("BeamCount", "梁数"),
                                new AntdUI.Column("HorizontalBeamsCount", "水平梁数"),
                                new AntdUI.Column("VerticalBeamsCount", "垂直梁数"),
                                new AntdUI.Column("KPillarCount", "K柱トータル"),
                                new AntdUI.Column("SinglePillarCount", "単柱"),
                                new AntdUI.Column("BCount", "Ｂ金物"),
                                new AntdUI.Column("BGCount", "BG金物"),
                                new AntdUI.Column("Id", "ID"),
                                new AntdUI.Column("CreationTime", "CreationTime"),
            };
            menulist = new AntdUI.IContextMenuStripItem[]
            {
                new AntdUI.ContextMenuStripItem("要確認の梁を表示", ""), // 2026年11月22日
                new AntdUI.ContextMenuStripItemDivider(),
                new AntdUI.ContextMenuStripItem("梁JW出力", "バッチ"),
                new AntdUI.ContextMenuStripItem("番付図上JW出力", ""),
                new AntdUI.ContextMenuStripItem("番付図下JW出力", ""),
                new AntdUI.ContextMenuStripItem("ブレース施工図", ""),
                new AntdUI.ContextMenuStripItemDivider(),
                new AntdUI.ContextMenuStripItem("梁製造CSV", ""),
                new AntdUI.ContextMenuStripItem("削除", ""),
            };

            //2026年4月17日 增加整个项目梁编号重新生成的功能
            mainmenulist = new IContextMenuStripItem[]
            {
                new ContextMenuStripItem("梁番号の一括付け直し",""),
                new ContextMenuStripItem("梁統計",""),
                new ContextMenuStripItem("3D 表示（建物全体）",""), // 2026年11月27日
                new ContextMenuStripItem("一括出力",""), // 2026年11月22日
                new ContextMenuStripItem("階数を変更",""), // 2026年11月22日
                new ContextMenuStripItem("削除","")
            };

            
            GlobalEvent.GetGlobalEvent().UpdateCodeEvent.Subscribe(JwProjectMainPage_UpdateCodeEvent);
            GlobalEvent.GetGlobalEvent().RefreshDataEvent += GlobalEvent_RefreshDataEvent;
            InitExportBar(); // 2026年11月22日 NewProjectMainPage.ExportBar.cs
            base.InitData();
        }

        #region gloal event
        private async void GlobalEvent_RefreshDataEvent(object? sender, EventArgs e)
        {
            await ReloadData();
        }

        private async Task JwProjectMainPage_UpdateCodeEvent(object? sender, UpdateCodeArgs e)
        {
            var z = await JwProjectMainService.ChangeBeamGongqu(e.Id, e.NewCode);
            var msg = $"梁番号:{z.BeamCode}、新しい工区コード:{e.NewCode}";
            this.SuccessModal(msg);
        }
        #endregion

        #region winform event   
        private async void NewProjectMainPage_Load(object sender, EventArgs e)
        {
            //this.jwProjectMainDataBindingSource.DataSource = JwProjectMainService.GetAllAsync().Result;
            //this.projectmaintable.DataSource = this.jwProjectMainDataBindingSource;
            await ReloadData();
        }

        private void NewProjectMainPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalEvent.GetGlobalEvent().UpdateCodeEvent.Unsubscribe(JwProjectMainPage_UpdateCodeEvent);
            GlobalEvent.GetGlobalEvent().RefreshDataEvent -= GlobalEvent_RefreshDataEvent;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (projectmaintable.SelectedIndex > 0)
            {
                var selectedmaindata = this.projectmaintable[projectmaintable.SelectedIndex - 1].record as JwProjectMainData;
                if (selectedmaindata != null)
                {
                    if (selectedmaindata.ProjectStatus == ProjectStatus.Budget)
                    {
                        string emsg = string.Format("{0}には予算が生成されているためアップロードできません", selectedmaindata.ProjectName);
                        UIMessageBox.ShowError(emsg);
                        return;
                    }
                    // 2026年11月22日 工事の階数分を取り込み済みなら、それ以上は取り込めない
                    var blocked = await JwProjectMainService.CheckImportAsync(selectedmaindata.Id);
                    if (blocked != null)
                    {
                        UIMessageBox.ShowWarning(blocked);
                        return;
                    }
                    var f = new OpenFileDialog();
                    f.Filter = "Jww Files|*.jww|Jws Files|*.jws|All Files|*.*";
                    f.Multiselect = true; // 2026年11月22日 複数の階をまとめて取り込める
                    if (f.ShowDialog() != DialogResult.OK) return;
                    // 2026年11月22日 簡易確認・複数ファイルに対応した共通の流れ（Classes/FloorImportFlow.cs）
                    int saved = await FloorImportFlow.ImportFilesAsync(this, selectedmaindata, f.FileNames);
                    if (saved > 0) await ReloadData();
                }
            }
        }

        /// <summary>
        /// 2026年11月22日「識別色確認」：原来只是读取文件（早期的测试代码，读完什么也不显示）。
        /// 改为打开「色分布の確認」（参照のみ）。工事を選択していれば、その顧客の保存済みの色設定と照合する。
        /// </summary>
        private async void button3_Click(object sender, EventArgs e)
        {
            using var f = new OpenFileDialog { Filter = "Jw_cad 図面 (*.jww)|*.jww", Title = "色分布を確認する図面を選択" };
            if (f.ShowDialog() != DialogResult.OK) return;

            var assignments = new List<ColorPreviewForm.Assignment>();
            JwProjectMainData? main = projectmaintable.SelectedIndex > 0
                ? projectmaintable[projectmaintable.SelectedIndex - 1].record as JwProjectMainData : null;
            try
            {
                if (main?.JwCustomerDataId != null)
                {
                    long cid = main.JwCustomerDataId.Value;
                    var c = await jwqitaService.FindCustDesignConstData(t => t.JwCustomerDataId == cid);
                    if (c != null)
                    {
                        assignments.Add(new("梁輪郭線", c.BeamParseColorNumber));
                        assignments.Add(new("梁符号の文字", c.BeamSymbolTextColorNumber));
                        assignments.Add(new("柱", c.BeamPillarParseColor));
                        assignments.Add(new("継手記号", c.BeamSplitParseColor));
                        assignments.Add(new("下階柱記号", c.DownPillarColorNumber));
                        assignments.Add(new("ブレース", c.LianjieColorNumber));
                    }
                }

                JwwColorStats stats = null!;
                await Progress(async () => stats = await Task.Run(() => JwwColorStats.Read(f.FileName)));
                string title = main != null ? $"{Path.GetFileName(f.FileName)}（{main.ProjectName} の顧客設定と照合）" : Path.GetFileName(f.FileName);
                using var dlg = new ColorPreviewForm(stats, title, assignments, allowAssign: false);
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                RGBControls.Errors.ErrorReporter.Report(ex, "図面を読み込めませんでした。");
            }
        }

        #endregion

        #region data event

        private async void projectmaintable_SelectIndexChanged(object sender, EventArgs e)
        {
            if (projectmaintable.SelectedIndex > 0)
            {
                _selectedMainData = this.projectmaintable[projectmaintable.SelectedIndex - 1].record as JwProjectMainData;
                if (_selectedMainData != null)
                {
                    await JwProjectMainService.LoadSubDataAsync(_selectedMainData);
                    //this.dbContext.Entry(selecteddata).Collection(e => e.JwProjectSubDatas).Load();
                    this.table1.DataSource = _selectedMainData.JwProjectSubDatas.ToList();
                    selectedsubData = null!; // 2026年11月22日 別の工事を選んだら階の選択を解除
                    UpdateExportBar();
                }
                HasSelectedRow = true;
                /*                SelectedRow = projectmaintable.DataSource[projectmaintable.SelectedIndex]*/
                ;
            }
            else
            {
                HasSelectedRow = false;
                SelectedRow = null;
            }
        }

        private async void contextMenuStrip1_Opening(ContextMenuStripItem e)
        {
            if (selectedsubData != null)
            {
                await JwProjectMainService.LoadSubCollectionAsync(selectedsubData);
            }
            if (e.Text.Equals("要確認の梁を表示") && selectedsubData != null)
            {
                await OpenFloorViewAsync(selectedsubData, needCheck: true); // 2026年11月22日
                return;
            }
            if (e.Text.Equals("梁JW出力"))
            {
                if (selectedsubData != null)
                {

                    if (AntdUI.Modal.open(this, "確認", "この階のすべての梁図をJWで出力しますか？") == DialogResult.OK)
                    {
                        await Progress(async () => { await SaveSubBeams(selectedsubData); });
                        if (!string.IsNullOrEmpty(_nowsavefold))
                        {
                            this.SuccessModal("梁図を出力しました：" + _nowsavefold);
                            //UIMessageBox.ShowSuccess("梁図を出力しました：" + _nowsavefold);
                        }
                    }
                }
            }
            if (e.Text.Equals("番付図上JW出力"))
            {
                if (selectedsubData != null)
                {
                    await Progress(async () => { await SaveSubTopCanvas(selectedsubData); });
                    //SaveSubTopCanvas(selectedsubData);
                }
            }
            if (e.Text.Equals("番付図下JW出力"))
            {
                if (selectedsubData != null)
                {
                    await Progress(async () => { await SaveSubBottomCanvas(selectedsubData); });
                    //SaveSubBottomCanvas(selectedsubData);
                }
            }
            if (e.Text.Equals("ブレース施工図"))
            {
                if (selectedsubData != null)
                {
                    if (selectedsubData.JwLianjieDatas.Count == 0)
                    {
                        UIMessageBox.ShowError("この階にはブレースデータがありません");
                        return;
                    }
                    await Progress(async () => { await SaveSubCanvasLines(selectedsubData); });
                    //SaveSubCanvasLines(selectedsubData);
                }
            }
            if (e.Text.Equals("削除"))
            {
                if (selectedsubData != null)
                {
                    var tishimsg = string.Format("選択した階データ {0} を削除しますか？", selectedsubData.FloorName);
                    if (AntdUI.Modal.open(this, "確認", tishimsg) == DialogResult.OK)
                    {
                        await Progress(async () =>
                        {
                            await JwProjectMainService.DeleteSubData(selectedsubData.Id);
                            await ReloadData();
                        });

                    }

                }
            }
            if (e.Text.Equals("梁製造CSV"))
            {
                if (selectedsubData != null)
                {
                    if (selectedsubData.JwBeamDatas.Count > 0)
                    {
                        foreach (var b in selectedsubData.JwBeamDatas)
                        {
                            await JwProjectMainService.LoadBeamCollectionAsync(b);
                        }
                        bool exported = false;
                        await Progress(() =>
                        {
                            exported = ExportCsv(selectedsubData);
                        });
                        if (exported) await CsvExportRecorder.RecordAsync(selectedsubData); // 2026年11月22日 状態列「CSV出力済」用
                    }

                }
            }
        }

        /// <summary>
        /// maintable的右键菜单，增加了整个项目梁编号重新生成和删除项目的功能
        /// </summary>
        /// <param name="e"></param>
        private async void contextMenuStrip2_Opening(ContextMenuStripItem e)
        {
            if (e.Text.Equals("一括出力"))
            {
                await BatchExportAsync(); // NewProjectMainPage.BatchExport.cs
                return;
            }
            if (e.Text.Equals("階数を変更"))
            {
                await ChangeFloorQuantityAsync(); // NewProjectMainPage.Floors.cs
                return;
            }
            if (e.Text.Equals("削除"))
            {
                if (_selectedMainData!=null)
                {
                        var tishimsg = string.Format("選択した工事 {0} を削除しますか？", _selectedMainData.ProjectName);
                        if (AntdUI.Modal.open(this, "確認", tishimsg) == DialogResult.OK)
                        {
                            await Progress(async () =>
                            {
                                await JwProjectMainService.DeletMain(_selectedMainData.Id);
                                await ReloadData();
                            });
                        }   
                }
            }
            if(e.Text.Equals("梁番号の一括付け直し"))
                {
                    if (_selectedMainData != null)
                    {
                        await Progress(async () =>
                        {
                             await JwProjectMainService.UnifyBeamCode(_selectedMainData.Id);
                            var msg = $"工事 {_selectedMainData.ProjectName} の梁番号を再生成しました。";
                            this.SuccessModal(msg);
                        });
                    }
            }
            if (e.Text.Equals("梁統計"))
            {
                if( _selectedMainData != null)
                {
                    ShowAllBeams showAll=new ShowAllBeams(_selectedMainData);
                    showAll.ShowDialog();
                }
            }
            if (e.Text.Equals("3D 表示（建物全体）")) // 2026年11月27日
            {
                if (_selectedMainData != null)
                {
                    using var f = new Building3DForm(_selectedMainData);
                    f.ShowDialog(this);
                }
            }
        }

        int nowhoverrow = -1;
        private void table1_CellHover(object sender, TableHoverEventArgs e)
        {
            nowhoverrow = e.RowIndex;
        }


        /// <summary>
        /// 双击项目行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void projectmaintable_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            //if (e.RowIndex > 0)
            //{
            //    ////WarningMsg("読み込み中");
            //    await Progress(async () =>
            //    {
            //        var z = projectmaintable[e.RowIndex - 1].record as JwProjectMainData;
            //        await JwProjectMainService.LoadSubDataAsync(z);
            //        if (z.JwProjectSubDatas.Count > 0)
            //        {
            //            foreach (var sub in z.JwProjectSubDatas)
            //            {
            //                await JwProjectMainService.LoadSubCollectionAsync(sub);
            //                if (sub.JwBeamDatas.Count > 0)
            //                {
            //                    foreach (var bd in sub.JwBeamDatas)
            //                    {
            //                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
            //                    }
            //                }
            //            }
            //            //ProjectDetail detail = new ProjectDetail(z);
            //            //detail.Show();
            //            SubsForm subsForm = new SubsForm(z);
            //            subsForm.ShowDialog();
            //        }
            //    });

            //}

            // 2026年11月22日 工事をダブルクリック → 階の作業台（SubsForm）を開く。原来双击没有任何反应。
            // 作業台は渡された工事の階・梁・穴を使うので、先に読み込んでから開く（上のコメントの処理と同じ）
            if (e.RowIndex <= 0) return;
            if (projectmaintable[e.RowIndex - 1].record is not JwProjectMainData main) return;
            await Progress(async () =>
            {
                await JwProjectMainService.LoadSubDataAsync(main);
                foreach (var sub in main.JwProjectSubDatas)
                {
                    await JwProjectMainService.LoadSubCollectionAsync(sub);
                    foreach (var bd in sub.JwBeamDatas)
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                }
            });
            using (var form = new SubsForm(main))
            {
                form.ShowDialog(this);
            }
            await ReloadData();
        }

        private void projectmaintable_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > 0)
                {
                    _selectedMainData = projectmaintable[e.RowIndex - 1].record as JwProjectMainData;
                    if (_selectedMainData != null)
                    {
                        AntdUI.ContextMenuStrip.open(this, it =>
                        {
                            contextMenuStrip2_Opening(it);
                        }, mainmenulist);
                    }
                    projectmaintable.SelectedIndex = e.RowIndex;
                }
            }
        }

        private async void table1_CellClick(object sender, TableClickEventArgs e)
        {
            // 2026年11月22日 状態列の「要確認」をクリックすると、その階の要確認の梁を表示
            if (await TryOpenNeedCheckFromStatusCellAsync(e)) return;
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > 0)
                {
                    selectedsubData = table1[e.RowIndex - 1].record as JwProjectSubData;
                    if (selectedsubData != null)
                    {
                        AntdUI.ContextMenuStrip.open(this, it =>
                        {
                            contextMenuStrip1_Opening(it);
                        }, menulist);
                    }
                    table1.SelectedIndex = e.RowIndex;
                }
            }
        }


        private async void table1_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            // 2026年11月22日 階画面を開く処理は NewProjectMainPage.Interaction.cs に移した（要確認の表示と共用）。
            // 原来的条件 e.RowIndex > -1 在双击表头（0）时会读取第 -1 行，改为 > 0
            if (e.RowIndex > 0 && table1[e.RowIndex - 1].record is JwProjectSubData z)
                await OpenFloorViewAsync(z, needCheck: false);
        }

        #endregion

        private async Task ReloadData()
        {
            await RefreshFloorChecksAsync(); // 2026年11月22日 状態列の集計（読み取りのみ）
            this.jwProjectMainDataBindingSource.DataSource = ApplyHomeFilter(await JwProjectMainService.GetAllAsync()); // 2026年11月24日 ワークベンチからの絞り込み
            this.projectmaintable.DataSource = this.jwProjectMainDataBindingSource;
            this.projectmaintable.Refresh();
            if (_selectedMainData != null)
            {
                var refreshedMainData = await JwProjectMainService.GetByIdAsync(_selectedMainData.Id);
                if (refreshedMainData != null)
                {
                    _selectedMainData = refreshedMainData;
                    await JwProjectMainService.LoadSubDataAsync(_selectedMainData);
                    this.table1.DataSource = _selectedMainData.JwProjectSubDatas.ToList();
                    this.table1.Refresh();
                }
                else
                {
                    this.table1.DataSource = null;
                    this.table1.Refresh();
                }
            }
            else
            {
                this.table1.DataSource = null;
                this.table1.Refresh();
            }
            UpdateExportBar(); // 2026年11月22日 再読み込みで選択が外れたらボタンも無効に
        }


        // 2026年11月21日 改为真正等待任务完成（见 SpinRunner）。async lambda 会优先匹配 Func<Task> 重载。
        private Task Progress(Func<Task> work) => SpinRunner.RunAsync(_parentForm, work);

        private Task Progress(Action action) => SpinRunner.RunAsync(_parentForm, action);

        private string _nowsavefold = "";


        #region 各类jww及施工图导出

        private async Task SaveSubBeams(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";

            if (RGBControls.Classes.ExportFolders.Show(dialog, RGBControls.Classes.ExportFolders.BeamJw) == DialogResult.OK)
            {
                await SaveSubBeamsTo(data, dialog.SelectedPath, clearProjectFolder: true);
            }
        }

        /// <summary>
        /// 2026年11月22日 選んだフォルダへ梁図を書き出す（一括出力と共用。中身は原来的处理）。
        /// clearProjectFolder：原来是把「工事名」文件夹整个删掉再建（单层导出保持不变）；一括出力では消さない。
        /// </summary>
        private async Task SaveSubBeamsTo(JwProjectSubData data, string selectedPath, bool clearProjectFolder)
        {
            var maindata = await JwProjectMainService.GetByIdAsync(data.JwProjectMainDataId);
            {
                //this.ShowProcessForm(200);
                string foldPath = selectedPath + "\\" + maindata.ProjectName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }
                else if (clearProjectFolder)
                {
                    Directory.Delete(foldPath, true);
                    Directory.CreateDirectory(foldPath);
                }
                _nowsavefold = foldPath;

                await JwProjectMainService.LoadSubCollectionAsync(data);
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                    }
                }
                string subpath = foldPath + "\\" + data.FloorName;
                if (!Directory.Exists(subpath))
                {
                    Directory.CreateDirectory(subpath);
                }
                //this.dbContext.Entry(sub).Collection(e => e.jwho).Load();
                JwCanvas jwCanvas = data.DataToCanvas();
                if (jwCanvas.Beams.Count > 0)
                {
                    var gbeams = jwCanvas.Beams.GroupBy(t => t.BeamCode).ToList();
                    foreach (var b in gbeams)
                    {
                        var bm = b.First();
                        string sl = "";
                        if (b.ToList().Count > 1)
                        {
                            sl = "(" + b.ToList().Count.ToString() + ")";
                        }
                        string wjm = string.Format("{0}{1}.jww", b.Key, sl);
                        NewJwBeamJwDraw jwDraw = new NewJwBeamJwDraw(bm);
                        jwDraw.CreateBeam();
                        if (jwDraw.Sens.Count > 0)
                        {
                            using var a = new JwwHelper.JwwWriter();

                            //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                            //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                            //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                            var headerpath = string.Format("template{0}.jww", data.Biaochi);
                            a.InitHeader(headerpath);
                            foreach (var s in jwDraw.Datas)
                            {
                                a.AddData(s);
                            }

                            //foreach(var b in jwDraw.Biaozhu)
                            //{
                            //    a.AddData(b);
                            //}
                            a.Write(subpath + "\\" + wjm);
                        }
                    }
                }
            }
        }

        private async Task SaveSubTopCanvas(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (RGBControls.Classes.ExportFolders.Show(dialog, RGBControls.Classes.ExportFolders.BanzukeTop) == DialogResult.OK)
            {
                await SaveSubTopCanvasTo(data, dialog.SelectedPath);
            }
        }

        /// <summary>2026年11月22日 選んだフォルダへ番付図（上）を書き出す（一括出力と共用。中身は原来的处理）。</summary>
        private async Task SaveSubTopCanvasTo(JwProjectSubData data, string selectedPath)
        {
            {
                string foldPath = selectedPath;// + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }

                await JwProjectMainService.LoadSubCollectionAsync(data);
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                    }
                }
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawShigong(true);
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                //a.InitHeader("template150.jww");
                var headerpath = string.Format("template{0}.jww", data.Biaochi);
                a.InitHeader(headerpath);
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + " 上F孔伏図.jww");
            }
        }

        private async Task SaveSubBottomCanvas(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (RGBControls.Classes.ExportFolders.Show(dialog, RGBControls.Classes.ExportFolders.BanzukeBottom) == DialogResult.OK)
            {
                await SaveSubBottomCanvasTo(data, dialog.SelectedPath);
            }
        }

        /// <summary>2026年11月22日 選んだフォルダへ番付図（下）を書き出す（一括出力と共用。中身は原来的处理）。</summary>
        private async Task SaveSubBottomCanvasTo(JwProjectSubData data, string selectedPath)
        {
            {
                string foldPath = selectedPath;// + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }

                await JwProjectMainService.LoadSubCollectionAsync(data);
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                    }
                }
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawShigong(false);
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                //a.InitHeader("template.jww");
                var headerpath = string.Format("template{0}.jww", data.Biaochi);
                a.InitHeader(headerpath);
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + " 下F孔伏図.jww");
            }
        }
        private async Task SaveSubCanvasLines(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (RGBControls.Classes.ExportFolders.Show(dialog, RGBControls.Classes.ExportFolders.Brace) == DialogResult.OK)
            {
                await SaveSubCanvasLinesTo(data, dialog.SelectedPath);
            }
        }

        /// <summary>2026年11月22日 選んだフォルダへブレース施工図を書き出す（一括出力と共用。中身は原来的处理）。</summary>
        private async Task SaveSubCanvasLinesTo(JwProjectSubData data, string selectedPath)
        {
            string flname = string.Format("{0}_ブレース施工図.jww", data.FloorName);
            {
                string foldPath = selectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }
                await JwProjectMainService.LoadSubCollectionAsync(data);
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                    }
                }
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawLinesToJww();
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                //a.InitHeader("template.jww");
                var headerpath = string.Format("template{0}.jww", data.Biaochi);
                a.InitHeader(headerpath);
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + flname);
            }
        }

        /// <summary>
        /// 2026年11月22日 CSV ファイルの書き込み（原来写在 ExportCsv 里的处理，一括出力と共用。編码处理不变）。
        /// </summary>
        private static void WriteCsvFile(string fName, string csvstr)
        {
            // 参数1：写入文件的文件名；参数2：写入文件的内容
            byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(csvstr);
            bs = Encoding.Convert(Encoding.GetEncoding("UTF-8"), Encoding.Default, bs);
            string q = Encoding.Default.GetString(bs);
            System.IO.File.WriteAllText(fName, q, Encoding.GetEncoding("Shift-JIS"));   // 向文件中写入内容
        }

        /// <returns>ファイルを書き出したら true（キャンセル・データなしは false）</returns>
        private bool ExportCsv(JwProjectSubData data)
        {
            bool written = false;
            var canvas = data.DataToCanvas();
            if (canvas != null && canvas.Beams.Count != 0)
            {
                var csvstr = canvas.ToProcessCsv();
                if (!string.IsNullOrEmpty(csvstr))
                {
                    // 2026年11月22日 出力前に内容を確認（CSV の内容は変えない）
                    if (!CsvExportConfirmForm.Confirm(this, data, csvstr)) return false;
                    SaveFileDialog saveDataSend = new SaveFileDialog();
                    // Environment.SpecialFolder.MyDocuments 表示在我的文档中
                    saveDataSend.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);   // 获取文件路径
                    saveDataSend.Filter = "*.csv|csv file";   // 设置文件类型为文本文件
                    saveDataSend.DefaultExt = ".csv";   // 默认文件的拓展名
                    saveDataSend.FileName = string.Format("{0}-3015-2.csv", data.FloorName);   // 文件默认名
                    if (RGBControls.Classes.ExportFolders.Show(saveDataSend, RGBControls.Classes.ExportFolders.Csv) == DialogResult.OK)   // 显示文件框，并且选择文件
                    {
                        string fName = saveDataSend.FileName;   // 获取文件名
                        WriteCsvFile(fName, csvstr);
                        written = true;
                        AntdUI.Modal.open(new AntdUI.Modal.Config(this, "完了", "CSVへのエクスポートが完了しました。", AntdUI.TType.Success)
                        {
                            OnButtonStyle = (id, btn) =>
                            {
                                btn.BackExtend = "135, #6253E1, #04BEFE";
                            },
                            CancelText = null,
                            OkText = "YES"
                        });
                    }
                }
            }
            return written;
        }

        #endregion

        #region 新增项目
        public JwqitaService jwqitaService => ServiceFactory.GetInstance().CreateJwqitaService();
        private async void button1_Click(object sender, EventArgs e) => await AddProjectAsync();

        /// <summary>工事を追加（ボタン・ワークベンチ共通。2026年11月24日 ワークベンチから完了を待てるよう Task に分離）</summary>
        private async Task AddProjectAsync()
        {
            UIEditOption option = new UIEditOption();
            option.AutoLabelWidth = true;
            option.Text = "追加";
            option.AddText("ProjectName", "工事名", null, true);
            option.AddText("SiteAddress", "建設現場住所", null, true);
            option.AddInteger("FloorQuantity", "階数", 1); // 2026年11月22日 取り込める階の上限になる（1 以上）
            //option.AddText("JwCustomerDataId", "顧客", null, false);
            //option.AddText("Telephone", "電話", null, false);


            //GetAllJwCustomerDatasAsync
            var cdlst = await jwqitaService.GetAllJwCustomerDatasAsync(null);
            var cuslst = cdlst;

            option.AddCombobox("JwCustomerDataId", "顧客", cuslst, "CompanyName", "Id", 0);
            //option.AddCombobox("Info", "关联", infoList, "Name", "Id", "2");
            //option.AddSwitch("Switch", "选择", false, "打开", "关闭");
            //option.AddComboTreeView("ComboTree", "选择", nodes, nodes[1].Nodes[1]);
            //option.AddComboCheckedListBox("checkedList", "选择", checkedItems, "CCC");
            UIEditForm frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += Frm_CheckedData;//校验
            frm.ShowDialog();
            if (frm.IsOK)
            {
                JwProjectMainData customerdata = new JwProjectMainData();
                customerdata.ProjectName = frm["ProjectName"].ToString();
                customerdata.SiteAddress = frm["SiteAddress"].ToString();
                customerdata.FloorQuantity = Convert.ToInt32(frm["FloorQuantity"].ToString());
                customerdata.Biaochi = "";
                customerdata.JwCustomerDataId = Convert.ToInt64(frm["JwCustomerDataId"].ToString());
                //customerdata.Telephone = frm["Telephone"].ToString();
                await JwProjectMainService.AddMainData(customerdata);
                await ReloadData();
            }
        }


        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Form["ProjectName"].ToString()))
            {
                e.Form.SetEditorFocus("ProjectName");
                ShowWarningTip("工事名を入力してください");
                return false;
            }
            // 2026年11月22日 階数は取り込める階の上限になるので 1 以上
            if (!int.TryParse(e.Form["FloorQuantity"]?.ToString(), out var floors) || floors < 1)
            {
                e.Form.SetEditorFocus("FloorQuantity");
                ShowWarningTip("階数は 1 以上を入力してください");
                return false;
            }
            //if (Convert.ToDouble(e.Form["UnitPrice"]) == 0)
            //{
            //    e.Form.SetEditorFocus("単価");
            //    ShowWarningTip("単価をゼロにすることはできません");
            //    return false;
            //}

            return true;
        }
        #endregion

        #region jww读取回调

        async Task OpenFile(String path, JwProjectMainData data)
        {
            try
            {
                if (Path.GetExtension(path).ToLower() == ".jww")
                {
                    JwProjectPathModel model = new JwProjectPathModel();
                    model.Path = path;
                    model.MainData = data;


                    CustomerDesignSetting jfsf = new CustomerDesignSetting(model);
                    if (jfsf.ShowDialog() == DialogResult.OK)
                    {
                        FileParseForm parseForm = new FileParseForm(model);
                        if (parseForm.ShowDialog() == DialogResult.OK)
                        {
                            await ReloadData();
                        }
                        //var s = model.FloorName;
                    }
                    //JwwReaderが読み込み用のクラス。
                    //using var reader = new JwwHelper.JwwReader();
                    ////Completedは読み込み完了時に実行される関数。
                    //reader.Read(path, Completed);
                    //var a = reader.Header.m_jwwDataVersion;

                }
                else if (Path.GetExtension(path) == ".jws")
                {
                    UIMessageBox.ShowError("JWSを処理できません");
                    ////jwsも読めますが、このプロジェクトでは確認用のコードがありません。
                    //using var a = new JwwHelper.JwsReader();
                    //a.Read(path, Completed2);
                }
            }
            catch (Exception exception)
            {
                RGBControls.Errors.ErrorReporter.Report(exception, "ファイルの処理中にエラーが発生しました。");
            }
        }


        List<double> jwwxs = new List<double>();

        List<double> jwwys = new List<double>();

        List<JwwSen> senes = new List<JwwSen>();

        List<JwwSolid> solides = new List<JwwSolid>();

        List<JwwTen> tenes = new List<JwwTen>();

        List<JwwMoji> mojies = new List<JwwMoji>();

        public List<JwwBlock> JWWBlockLst = new List<JwwBlock>();
        Dictionary<int, List<JwwSolid>> _dictionarytempblocklst = new Dictionary<int, List<JwwSolid>>();
        int nownumber = -1;
        void Completed(JwwHelper.JwwReader reader)
        {
            nownumber = -1;
            _dictionarytempblocklst = new Dictionary<int, List<JwwSolid>>();
            jwwxs = new List<double>();
            jwwys = new List<double>();
            senes = new List<JwwSen>();
            solides = new List<JwwSolid>();
            mojies = new List<JwwMoji>();
            var sb = new StringBuilder();
            var header = reader.Header;
            sb.AppendLine("Paper:" + header.m_nZumen);
            sb.AppendLine("Layers=============================================");
            for (var j = 0; j < 16; j++)
            {

                sb.AppendLine("Layer group " + j + " Name:" + header.m_aStrGLayName[j] + " Scale:" + header.m_adScale[j]);
                for (var i = 0; i < 16; i++)
                {
                    if (i % 2 == 1)
                    {
                        sb.AppendLine("  Layer " + i + " Name:" + header.m_aStrLayName[j][i]);
                    }
                    else
                    {
                        sb.Append("  Layer " + i + " Name:" + header.m_aStrLayName[j][i]);
                    }
                }
            }
            sb.AppendLine("Blocks=============================================");
            sb.AppendLine("Size of blocks:" + reader.GetBlockSize());
            //foreach (var s in reader.Blocks) {
            //    sb.AppendLine(s.ToString());
            //}

            sb.AppendLine("Shapes=============================================");
            var dataList = reader.DataList;

            foreach (var s in reader.DataListList)
            {
                _dictionarytempblocklst.Add(s.m_nNumber, new List<JwwSolid>());
                nownumber = s.m_nNumber;
                s.EnumerateDataList(jwblockread);
            }


            List<int> colors = new List<int>();
            foreach (var data in dataList)
            {
                string typename = data.GetType().Name;
                if (typename == "JwwSen")//线
                {
                    var sen = data as JwwSen;
                    if (sen != null)
                    {
                        colors.Add(sen.m_nPenColor);
                        senes.Add(sen);
                        jwwxs.Add(sen.m_end_x);
                        jwwxs.Add(sen.m_start_x);
                        jwwys.Add(sen.m_end_y);
                        jwwys.Add(sen.m_start_y);
                    }

                    //if(sen?.m_nPenColor==JwFileConsts.BeamParseColor.ColorNumber)
                    //{
                    //    ParseSenLst.Add(sen);
                    //}

                }
                if (typename == "JwwSolid")//块
                {

                    var solid = data as JwwSolid;
                    if (solid != null)
                    {
                        colors.Add(solid.m_nPenColor);
                        solides.Add(solid);
                    }

                    //jwwys.Add(solid.m_end_y);
                    //jwwys.Add(solid.m_start_y);
                    //jwwys.Add(solid.m_DPoint2_y);
                    //jwwys.Add(solid.m_DPoint3_y);
                    //jwwxs.Add(solid.m_start_x);
                    //jwwxs.Add(solid.m_end_x);
                    //jwwxs.Add(solid.m_DPoint2_x);
                    //jwwxs.Add(solid.m_DPoint3_x);
                }
                if (typename == "JwwMoji")//文字
                {
                    var moji = data as JwwMoji;
                    mojies.Add(moji);
                    //jwwxs.Add(moji.m_end_x);
                    //jwwxs.Add(moji.m_start_x);
                    //jwwys.Add(moji.m_end_y);
                    //jwwys.Add(moji.m_start_y);

                }
                if (typename == "JwwBlock")
                {
                    var block = data as JwwBlock;
                    JWWBlockLst.Add(block);
                }
            }
            List<JwBlock> _tempblocks = new List<JwBlock>();
            if (JWWBlockLst.Count > 0)
            {
                foreach (var bl in JWWBlockLst)
                {
                    int num = bl.m_nNumber;
                    if (_dictionarytempblocklst.Keys.Contains(num))
                    {
                        var zsolidlst = _dictionarytempblocklst[num];
                        if (zsolidlst.Count > 0)
                        {
                            foreach (var solidss in zsolidlst)
                            {
                                colors.Add(solidss.m_nPenColor);
                                _tempblocks.Add(new JwBlock(solidss, bl));
                            }
                        }
                    }
                }
            }


            double wx = 0;
            double hy = 0;
            double minx = 0;
            double maxx = 0;
            double miny = 0;
            double maxy = 0;
            if (jwwxs.Count > 0)
            {
                minx = jwwxs.Min();
                maxx = jwwxs.Max();
                wx = maxx - minx;
            }

            if (jwwys.Count > 0)
            {
                miny = jwwys.Min();

                maxy = jwwys.Max();

                hy = maxy - miny;
            }
            var colordistinct = colors.Distinct().ToList();
            var arrycolors = new System.Drawing.Color[] { System.Drawing.Color.Red, System.Drawing.Color.Green, System.Drawing.Color.Yellow, System.Drawing.Color.Blue, System.Drawing.Color.White, System.Drawing.Color.Purple, System.Drawing.Color.Orange, System.Drawing.Color.Sienna };
            using (RGBJWMain.Forms.Form1 f1 = new RGBJWMain.Forms.Form1(minx, maxx, miny, maxy, senes, solides, colordistinct, _tempblocks))
            {
                f1.ShowDialog();
            }

            //sb.AppendLine("Imagess=============================================");
            //var images = reader.Images;
            //foreach (var s in images)
            //{
            //    sb.Append(s.ImageName + "  ");
            //    sb.AppendLine(s.Size + "bytes");
            //}

        }

        bool jwblockread(JwwData jd)
        {
            if (nownumber != -1)
            {
                string typename = jd.GetType().Name;
                if (typename == "JwwBlock")
                {
                    var bl = jd as JwwBlock;
                    if (_dictionarytempblocklst.Keys.Contains(bl.m_nNumber))
                    {
                        _dictionarytempblocklst[nownumber] = _dictionarytempblocklst[bl.m_nNumber];
                    }
                }
                if (typename == "JwwSolid")
                {
                    var solid = jd as JwwSolid;
                    _dictionarytempblocklst[nownumber].Add(solid);
                    //if (jd.m_nPenColor == JwFileConsts.BeamPillarParseColor.ColorNumber)
                    //{

                    //}
                }
                //_tempblocklist.Add(jd);
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

       
    }
}
