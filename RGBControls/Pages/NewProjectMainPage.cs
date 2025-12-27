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
using System.Data.Entity;
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

        JwProjectSubData selectedsubData;

        JwProjectMainData _selectedMainData;

        private void Initform()
        {
            projectmaintable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ProjectName", "工事名"),
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
                                new AntdUI.Column("Biaochi", "縮尺"),
                                new AntdUI.Column("MarkBeam", "符号"),
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
                new AntdUI.ContextMenuStripItem("輸出-梁JW", "バッチ"),
                new AntdUI.ContextMenuStripItem("輸出-番付図上JW", ""),
                new AntdUI.ContextMenuStripItem("輸出-番付図下JW", ""),
                new AntdUI.ContextMenuStripItem("ブレース施工図", ""),
                new AntdUI.ContextMenuStripItemDivider(),
                new AntdUI.ContextMenuStripItem("消去", ""),
            };
            GlobalEvent.GetGlobalEvent().UpdateCodeEvent.Subscribe(JwProjectMainPage_UpdateCodeEvent);
            GlobalEvent.GetGlobalEvent().RefreshDataEvent += GlobalEvent_RefreshDataEvent;
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

        private void button2_Click(object sender, EventArgs e)
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
                    var f = new OpenFileDialog();
                    f.Filter = "Jww Files|*.jww|Jws Files|*.jws|All Files|*.*";
                    if (f.ShowDialog() != DialogResult.OK) return;
                    OpenFile(f.FileName, selectedmaindata);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog();
            f.Filter = "Jww Files|*.jww|Jws Files|*.jws|All Files|*.*";
            if (f.ShowDialog() != DialogResult.OK) return;

            if (Path.GetExtension(f.FileName).ToLower() == ".jww")
            {
                //JwwReaderが読み込み用のクラス。
                using var reader = new JwwHelper.JwwReader();
                //Completedは読み込み完了時に実行される関数。
                reader.Read(f.FileName, Completed);
                //var a = reader.Header.m_jwwDataVersion;


            }
            else if (Path.GetExtension(f.FileName) == ".jws")
            {
                UIMessageBox.ShowError("JWSを処理できません");
                ////jwsも読めますが、このプロジェクトでは確認用のコードがありません。
                //using var a = new JwwHelper.JwsReader();
                //a.Read(path, Completed2);
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
            if (e.Text.Equals("輸出-梁JW"))
            {
                if (selectedsubData != null)
                {
                    if (AntdUI.Modal.open(this, "ヒント", "プロジェクトデータをすべてエクスポートするかどうか") == DialogResult.OK)
                    {
                        await Progress(async () => { await SaveSubBeams(selectedsubData); });
                        if (!string.IsNullOrEmpty(_nowsavefold))
                        {
                            this.SuccessModal("エクスポートされたビーム-->" + _nowsavefold);
                            //UIMessageBox.ShowSuccess("エクスポートされたビーム-->" + _nowsavefold);
                        }
                    }
                }
            }
            if (e.Text.Equals("輸出-番付図上JW"))
            {
                if (selectedsubData != null)
                {
                    await Progress(async () => { await SaveSubTopCanvas(selectedsubData); });
                    //SaveSubTopCanvas(selectedsubData);
                }
            }
            if (e.Text.Equals("輸出-番付図下JW"))
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
                    await Progress(async () => { await SaveSubCanvasLines(selectedsubData); });
                    //SaveSubCanvasLines(selectedsubData);
                }
            }
            if (e.Text.Equals("消去"))
            {
                if (selectedsubData != null)
                {
                    var tishimsg = string.Format("選択した階データ {0} を削除しますか？", selectedsubData.FloorName);
                    if (AntdUI.Modal.open(this, "ヒント", tishimsg) == DialogResult.OK)
                    {
                        await Progress(async () =>
                        {
                            await JwProjectMainService.DeleteSubData(selectedsubData.Id);
                            await ReloadData();
                        });

                    }

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
            if (e.RowIndex > 0)
            {
                //WarningMsg("読み込み中");
                await Progress(async() =>
                {
                    var z = projectmaintable[e.RowIndex - 1].record as JwProjectMainData;
                    await JwProjectMainService.LoadSubDataAsync(z);
                    if (z.JwProjectSubDatas.Count > 0)
                    {
                        foreach (var sub in z.JwProjectSubDatas)
                        {
                            await JwProjectMainService.LoadSubCollectionAsync(sub);
                            if (sub.JwBeamDatas.Count > 0)
                            {
                                foreach (var bd in sub.JwBeamDatas)
                                {
                                    await JwProjectMainService.LoadBeamCollectionAsync(bd);
                                }
                            }
                        }
                        ProjectDetail detail = new ProjectDetail(z);
                        detail.ShowDialog();
                    }
                });
            }
        }

        private void table1_CellClick(object sender, TableClickEventArgs e)
        {
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
            if (e.RowIndex > -1)
            {

                var z = table1[e.RowIndex - 1].record as JwProjectSubData;

                await JwProjectMainService.LoadSubCollectionAsync(z);

                if (z.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in z.JwBeamDatas)
                    {
                        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                        //this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                        //this.dbContext.Entry(bd).Collection(e => e.JwBeamVerticalDatas).Load();
                    }
                }
                //MessageBox.Show(z.CompanyName);
                if (z != null)
                {

                    //ShowJwCanvasForm showJw = new ShowJwCanvasForm();
                    //showJw.jwCanvas = canvas;
                    //showJw.ShowDialog();
                    ShowSubForm subForm = new ShowSubForm(z);
                    subForm.WindowState = FormWindowState.Maximized;
                    Sub sub = new Sub(z);
                    sub.Dock= DockStyle.Fill;
                    //sub.AutoSize = true;
                    subForm.Controls.Add(sub);

                    
                    
                    subForm.ShowDialog();
                }

            }
        }

        #endregion

        private async Task ReloadData()
        {
            this.jwProjectMainDataBindingSource.DataSource = await JwProjectMainService.GetAllAsync();
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
            }

        }


        private async Task Progress(Action action)
        {
            await AntdUI.Spin.open(_parentForm, AntdUI.Localization.Get("Loading2", "読み込み中..."), async config =>
            {
                Thread.Sleep(100);
                this.Invoke(() =>
                {
                    action();
                });
                for (int i = 0; i < 101; i++)
                {
                    config.Value = i / 100F;
                    config.Text = AntdUI.Localization.Get("Processing", "処理") + " " + i + "%";
                    Thread.Sleep(20);
                }
                Thread.Sleep(1000);
                config.Value = null;
                config.Text = AntdUI.Localization.Get("PleaseWait", "お待ちください。...");
                Thread.Sleep(2000);
            }, () =>
            {
                System.Diagnostics.Debug.WriteLine("仕上げる");
            });
        }

        private string _nowsavefold = "";


        #region 各类jww及施工图导出

        private async Task SaveSubBeams(JwProjectSubData data)
        {

            var maindata = await JwProjectMainService.GetByIdAsync(data.JwProjectMainDataId);

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //this.ShowProcessForm(200);
                string foldPath = dialog.SelectedPath + "\\" + maindata.ProjectName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }
                else
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
                            a.InitHeader("template.jww");
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
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
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
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + ".jww");
            }
        }

        private async Task SaveSubBottomCanvas(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
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
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + ".jww");
            }
        }
        private async Task SaveSubCanvasLines(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            string flname = string.Format("{0}_ブレース施工図.jww", data.FloorName);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
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
                a.InitHeader("template.jww");
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



        #endregion

        #region 新增项目

        private async void button1_Click(object sender, EventArgs e)
        {
            UIEditOption option = new UIEditOption();
            option.AutoLabelWidth = true;
            option.Text = "追加";
            option.AddText("ProjectName", "プロジェクト", null, true);
            option.AddText("SiteAddress", "建設現場住所", null, true);
            option.AddInteger("FloorQuantity", "階数", 0);
            //option.AddText("JwCustomerDataId", "顧客", null, false);
            //option.AddText("Telephone", "電話", null, false);

            var cuslst = dbContext.JwCustomerDatas.ToList();

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
            }
        }


        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Form["ProjectName"].ToString()))
            {
                e.Form.SetEditorFocus("ProjectName");
                ShowWarningTip("プロジェクトを空にすることはできません");
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

        void OpenFile(String path, JwProjectMainData data)
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
                            this.projectmaintable.Refresh();
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
                //textBox1.Text = "";
                //MessageBox.Show(exception.Message, "Error");
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
