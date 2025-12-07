using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JwCore;
using Sunny.UI;
using RGBJWMain.Forms;
using System.IO;
using JwShapeCommon;
using Org.BouncyCastle.Asn1.Pkcs;
using NPOI.POIFS.Crypt.Dsig;
using JwwHelper;
using SixLabors.ImageSharp;
using System.IdentityModel.Tokens.Jwt;
using RGBControls.Classes;

namespace RGBJWMain.Pages
{
    public partial class JwProjectMainPage : BasePage
    {
        public JwProjectMainPage()
        {
            InitializeComponent();
            base.InitData();
            GlobalEvent.GetGlobalEvent().UpdateCodeEvent += JwProjectMainPage_UpdateCodeEvent;
        }

        private void JwProjectMainPage_UpdateCodeEvent(object? sender, UpdateCodeArgs e)
        {
            var obj = this.dbContext?.JwBeamDatas.Find(e.Id);
            obj.GongQu=e.NewCode;
            this.dbContext?.SaveChanges();
            var msg = $"梁番号:{obj.BeamCode}、新しい工区コード:{e.NewCode}";
            this.SuccessModal(msg);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.dbContext?.Database.EnsureCreated();

            this.dbContext?.JwProjectMainDatas.Load();

            this.dbContext?.JwCustomerDatas.Load();
            var z = this.uiDataGridView1.Columns["jwCustomerDataIdDataGridViewTextBoxColumn"] as DataGridViewComboBoxColumn;
            z.DataSource = dbContext?.JwCustomerDatas.ToList();
            this.jwProjectMainDataBindingSource.DataSource = dbContext?.JwProjectMainDatas.Local.ToBindingList();
            this.uiDataGridView1.Refresh();
            //if (uiDataGridView1.SelectedIndex >= 0)
            //{
            //    HasSelectedRow = true;
            //    SelectedRow = uiDataGridView1.SelectedRows[0];
            //}
            uiDataGridView1.ClearSelection();
            //uiDataGridView1.Rows[0].Selected = false;
        }

        private void uiDataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (!e.Row.IsNewRow)
                e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        private void uiDataGridView1_SelectionChanged(object sender, EventArgs e)
        {


        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.dbContext!.SaveChanges();
            this.uiDataGridView1.Refresh();
            this.uiDataGridView2.Refresh();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            this.dbContext!.SaveChanges();
            this.uiDataGridView1.Refresh();
            this.uiDataGridView2.Refresh();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (!HasSelectedRow)
            {
                UIMessageBox.ShowError("プロジェクトを選択してください");
            }

            //ガベコレしないとunmanage側のString関係のオブジェクトでコンソールにメッセージが出るので
            //ここでガベコレしています。メッセージは出ていても問題はないそうですが、
            //デバッグ中のメモリリークの確認のためにここに入れました。無くても構いません。
            if (SelectedRow != null)
            {
                var maindata = SelectedRow.DataBoundItem as JwProjectMainData;
                //var z1 = maindata.JwProjectSubDatas.First();
                //if(z1 != null)
                //{
                //    var qqq = z1.Location;
                //}

                if (maindata.ProjectStatus == ProjectStatus.Budget)
                {
                    string emsg = string.Format("{0}には予算が生成されているためアップロードできません", maindata.ProjectName);
                    UIMessageBox.ShowError(emsg);
                    return;
                }
                var f = new OpenFileDialog();
                f.Filter = "Jww Files|*.jww|Jws Files|*.jws|All Files|*.*";
                if (f.ShowDialog() != DialogResult.OK) return;
                OpenFile(f.FileName, maindata);
            }

            //if (!maindata.JwCustomerDataId.HasValue)
            //{
            //    if (UIMessageDialog.ShowAskDialog(this, "ceshi"))
            //    {
            //        var z = "sdf";
            //    }
            //    else
            //    {
            //        var q = "";
            //    }
            //}
        }

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
                            this.uiDataGridView1.Refresh();
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

        private void uiDataGridView1_SelectIndexChange(object sender, int index)
        {
            if (index > -1)
            {
                if (!uiDataGridView1.Rows[index].IsNewRow)
                {
                    HasSelectedRow = true;
                    SelectedRow = uiDataGridView1.Rows[index];
                }
                else
                {
                    HasSelectedRow = false;
                    SelectedRow = null;
                }


            }

        }

        private void uiDataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (!uiDataGridView2.Rows[e.RowIndex].IsNewRow)
                {
                    var z = uiDataGridView2.Rows[e.RowIndex].DataBoundItem as JwProjectSubData;
                    this.dbContext.Entry(z).Collection(e => e.JwBeamDatas).Load();
                    this.dbContext.Entry(z).Collection(e => e.JwPillarDatas).Load();
                    this.dbContext.Entry(z).Collection(e => e.JwLinkPartDatas).Load();
                    this.dbContext.Entry(z).Collection(e => e.JwLianjieDatas).Load();


                    if (z.JwBeamDatas.Count > 0)
                    {
                        foreach (var bd in z.JwBeamDatas)
                        {
                            this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                            this.dbContext.Entry(bd).Collection(e => e.JwBeamVerticalDatas).Load();
                        }
                    }
                    //MessageBox.Show(z.CompanyName);
                    if (z != null)
                    {
                        JwCanvas canvas = z.DataToCanvas();
                        canvas.JwProjectSubData = z;
                        ShowJwCanvasForm showJw = new ShowJwCanvasForm();
                        showJw.jwCanvas = canvas;
                        showJw.ShowDialog();
                    }
                }
            }

        }


        private int lastselected = -1;
        private void uiDataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (uiDataGridView1.CurrentRow != null)
            //{
            //    if (e.RowIndex == uiDataGridView1.SelectedIndex)
            //    {
            //        uiDataGridView1.ClearSelection();
            //        uiDataGridView1.SelectedIndex = -1;
            //    }
            //}
            if (e.RowIndex >= 0)
            {
                if (!uiDataGridView1.Rows[e.RowIndex].IsNewRow)
                {
                    HasSelectedRow = true;
                    SelectedRow = uiDataGridView1.Rows[e.RowIndex];
                    var category = (JwProjectMainData)this.uiDataGridView1.Rows[e.RowIndex].DataBoundItem;
                    if (category != null)
                    {
                        if (category.JwProjectSubDatas != null)
                        {
                            try
                            {
                                this.dbContext.Entry(category).Collection(e => e.JwProjectSubDatas).Load();

                            }
                            catch (Exception ex)
                            {

                            }

                        }

                    }
                }
                else
                {
                    HasSelectedRow = false;
                    SelectedRow = null;
                }
            }
        }

        private void uiDataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            if (!IsClose)
            {
                if (this.uiDataGridView1.CurrentRow != null)
                {
                    if (!this.uiDataGridView1.CurrentRow.IsNewRow)
                    {
                        if (this.dbContext != null)
                        {
                            var category = (JwProjectMainData)this.uiDataGridView1.CurrentRow.DataBoundItem;
                            if (category != null)
                            {
                                if (category.JwProjectSubDatas != null)
                                {
                                    try
                                    {
                                        this.dbContext.Entry(category).Collection(e => e.JwProjectSubDatas).Load();

                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }

                            }
                        }
                    }
                    uiButton2.Enabled = true;
                }
            }

        }

        private void JwProjectMainPage_Shown(object sender, EventArgs e)
        {


        }

        #region 窗口新增
        private void uiSymbolButton2_Click(object sender, EventArgs e)
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
                this.dbContext.JwProjectMainDatas.Add(customerdata);
                this.dbContext.SaveChanges();
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

        #region 操作批量生成梁
        /// <summary>
        /// 输出梁设计图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isMainclick)
            {
                if (!object.ReferenceEquals(_selecteddata, null))
                {
                    this.dbContext.Entry(_selecteddata).Collection(e => e.JwProjectSubDatas).Load();
                    if (_selecteddata.JwProjectSubDatas.Count > 0)
                    {
                        if (UIMessageBox.ShowAsk("プロジェクトデータをすべてエクスポートするかどうか"))
                        {

                            SaveBeams(_selecteddata);
                            Thread.Sleep(2000);
                            this.HideProcessForm();

                            if (!string.IsNullOrEmpty(_nowsavefold))
                            {
                                UIMessageBox.ShowSuccess("エクスポートされたビーム-->" + _nowsavefold);
                            }
                        }
                    }
                    else
                    {
                        UIMessageBox.ShowError("まだ分​​析データがありません");
                    }
                }
            }
            if (isSubclick)
            {
                if (!object.ReferenceEquals(_selectedsubdata, null))
                {
                    if (UIMessageBox.ShowAsk("プロジェクトデータをすべてエクスポートするかどうか"))
                    {

                        SaveSubBeams(_selectedsubdata);
                        Thread.Sleep(2000);
                        this.HideProcessForm();

                        if (!string.IsNullOrEmpty(_nowsavefold))
                        {
                            UIMessageBox.ShowSuccess("エクスポートされたビーム-->" + _nowsavefold);
                        }
                    }
                }
            }

        }


        private JwProjectMainData _selecteddata;

        private JwProjectSubData _selectedsubdata;

        private bool isMainclick = false;

        private bool isSubclick = false;

        private void uiDataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    _selecteddata = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwProjectMainData;
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                    isMainclick = true;
                }
                //}
                //else
                //{
                //    if (e.RowIndex >= 0)
                //    {
                //        uiDataGridView1.ClearSelection();
                //        HasSelectedRow = true;
                //        uiDataGridView1.Rows[e.RowIndex].Selected = true;
                //        SelectedRow = uiDataGridView1.Rows[e.RowIndex];
                //        contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                //    }
                //}
            }
        }

        private string _nowsavefold = "";

        private void SaveBeams(JwProjectMainData data)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.ShowProcessForm(200);
                string foldPath = dialog.SelectedPath + "\\" + data.ProjectName;
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
                foreach (var sub in data.JwProjectSubDatas)
                {

                    this.dbContext.Entry(sub).Collection(e => e.JwBeamDatas).Load();
                    this.dbContext.Entry(sub).Collection(e => e.JwPillarDatas).Load();
                    this.dbContext.Entry(sub).Collection(e => e.JwLinkPartDatas).Load();
                    if (sub.JwBeamDatas.Count > 0)
                    {
                        foreach (var bd in sub.JwBeamDatas)
                        {
                            this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                        }
                    }
                    string subpath = foldPath + "\\" + sub.FloorName;
                    if (!Directory.Exists(subpath))
                    {
                        Directory.CreateDirectory(subpath);
                    }
                    //this.dbContext.Entry(sub).Collection(e => e.jwho).Load();
                    JwCanvas jwCanvas = sub.DataToCanvas();
                    //foreach(var bj in jwCanvas.Beams)
                    //{

                    //}
                    if (jwCanvas.Beams.Count > 0)
                    {
                        var gbeams = jwCanvas.Beams.GroupBy(t => t.BeamCode).ToList();
                        foreach (var b in gbeams)
                        {
                            var bm = b.First();
                            string sl = "";
                            if (b.ToList().Count > 1)
                            {
                                sl = b.ToList().Count.ToString();
                            }
                            string wjm = string.Format("{0}{1}.jww", b.Key, sl);
                            //JwBeamJwDraw jwDraw = new JwBeamJwDraw(bm);
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
                        //foreach (var b in jwCanvas.Beams)
                        //{
                        //    JwBeamJwDraw jwDraw = new JwBeamJwDraw(b);
                        //    jwDraw.Draw();
                        //    if (jwDraw.Sens.Count > 0)
                        //    {
                        //        using var a = new JwwHelper.JwwWriter();

                        //        //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                        //        //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                        //        //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                        //        a.InitHeader("template.jww");
                        //        foreach (var s in jwDraw.Datas)
                        //        {
                        //            a.AddData(s);
                        //        }

                        //        //foreach(var b in jwDraw.Biaozhu)
                        //        //{
                        //        //    a.AddData(b);
                        //        //}
                        //        a.Write(subpath + "\\" + b.BeamCode + ".jww");
                        //    }
                        //}
                    }

                }
            }
        }

        private void SaveSubBeams(JwProjectSubData data)
        {

            var maindata = this.dbContext.JwProjectMainDatas.First(t => t.Id == data.JwProjectMainDataId);

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.ShowProcessForm(200);
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


                this.dbContext.Entry(data).Collection(e => e.JwBeamDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwPillarDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwLinkPartDatas).Load();
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
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

        private void uiDataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    _selectedsubdata = uiDataGridView2.Rows[e.RowIndex].DataBoundItem as JwProjectSubData;
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                    isSubclick = true;
                }
                //}
                //else
                //{
                //    if (e.RowIndex >= 0)
                //    {
                //        uiDataGridView1.ClearSelection();
                //        HasSelectedRow = true;
                //        uiDataGridView1.Rows[e.RowIndex].Selected = true;
                //        SelectedRow = uiDataGridView1.Rows[e.RowIndex];
                //        contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                //    }
                //}
            }
        }
        #endregion

        #region 预算生成方法


        private void CreateBudgetData(long mainid)
        {
            var subids = dbContext.JwProjectSubDatas.Where(t => t.JwProjectMainDataId == mainid).Select(t => t.Id).ToList();
            var beams = dbContext.JwBeamDatas.Where(t => subids.Contains(t.JwProjectSubDataId)).ToList();
            var pillars = dbContext.JwPillarDatas.Where(t => subids.Contains(t.JwProjectSubDataId)).ToList();
            var links = dbContext.JwLinkPartDatas.Where(t => subids.Contains(t.JwProjectSubDataId)).ToList();

            var beamlength = beams.Sum(t => t.Length);
            JwBudgetSubData beambudget = new JwBudgetSubData();
            //beambudget.UnitPrice=

        }

        #endregion

        private void uiSymbolButton3_Click(object sender, EventArgs e)
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

        private void uiDataGridView2_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (hasdeleted)
            {
                if (deleteitem != null)
                {

                    this.dbContext!.SaveChanges();
                    this.uiDataGridView1.Refresh();
                    this.uiDataGridView2.Refresh();

                    //deleteitem.JwProjectMainDataId
                    //var jwmd= this.dbContext.JwProjectMainDatas.FirstOrDefault(t=>t.Id==deleteitem.JwProjectMainDataId);

                }
            }
        }

        JwProjectSubData deleteitem;
        bool hasdeleted = false;

        private void uiDataGridView2_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            deleteitem = e.Row.DataBoundItem as JwProjectSubData;
            if (deleteitem != null)
            {
                hasdeleted = true;
            }
        }

        private void uiDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (!uiDataGridView1.Rows[e.RowIndex].IsNewRow)
                {
                    var z = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwProjectMainData;
                    this.dbContext.Entry(z).Collection(e => e.JwProjectSubDatas).Load();
                    if (z.JwProjectSubDatas.Count > 0)
                    {
                        foreach (var sub in z.JwProjectSubDatas)
                        {
                            this.dbContext.Entry(sub).Collection(e => e.JwBeamDatas).Load();
                            this.dbContext.Entry(sub).Collection(e => e.JwPillarDatas).Load();
                            this.dbContext.Entry(sub).Collection(e => e.JwLinkPartDatas).Load();
                            this.dbContext.Entry(sub).Collection(e => e.JwLianjieDatas).Load();
                            if (sub.JwBeamDatas.Count > 0)
                            {
                                foreach (var bd in sub.JwBeamDatas)
                                {
                                    this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                                    this.dbContext.Entry(bd).Collection(e => e.JwBeamVerticalDatas).Load();
                                }
                            }
                        }
                        ProjectDetail detail = new ProjectDetail(z);
                        detail.ShowDialog();
                    }
                }
            }
        }

        private void 改訂ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JwMainEdit edit = new JwMainEdit();
            edit.jwCustomerDatas = dbContext.JwCustomerDatas.ToList();
            edit.Render();
            edit.JwProjectMainData = _selecteddata;
            edit.ShowDialog();
            if (edit.IsOK)
            {
                dbContext.JwProjectMainDatas.Update(edit.JwProjectMainData);
                this.uiDataGridView1.Refresh();
                //this.ShowSuccessDialog(frm.Person.ToString());
            }
            edit.Dispose();
        }

        private void 詳細ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selecteddata != null)
            {
                this.dbContext.Entry(_selecteddata).Collection(e => e.JwProjectSubDatas).Load();
                if (_selecteddata.JwProjectSubDatas.Count > 0)
                {
                    foreach (var sub in _selecteddata.JwProjectSubDatas)
                    {
                        this.dbContext.Entry(sub).Collection(e => e.JwBeamDatas).Load();
                        this.dbContext.Entry(sub).Collection(e => e.JwPillarDatas).Load();
                        this.dbContext.Entry(sub).Collection(e => e.JwLinkPartDatas).Load();
                        this.dbContext.Entry(sub).Collection(e => e.JwLianjieDatas).Load();
                        if (sub.JwBeamDatas.Count > 0)
                        {
                            foreach (var bd in sub.JwBeamDatas)
                            {
                                this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                                this.dbContext.Entry(bd).Collection(e => e.JwBeamVerticalDatas).Load();
                            }
                        }
                    }
                    ProjectDetail detail = new ProjectDetail(_selecteddata);
                    detail.ShowDialog();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click_1(object sender, EventArgs e)
        {
            var qmain = this.uiDataGridView1.Rows[this.uiDataGridView1.SelectedIndex].DataBoundItem as JwProjectMainData;
            if (qmain != null)
            {
                this.dbContext.Entry(qmain).Collection(e => e.JwProjectSubDatas).Load();
                if (qmain.JwProjectSubDatas.Count > 0)
                {
                    foreach (var sub in qmain.JwProjectSubDatas)
                    {
                        this.dbContext.Entry(sub).Collection(e => e.JwBeamDatas).Load();
                        this.dbContext.Entry(sub).Collection(e => e.JwPillarDatas).Load();
                        this.dbContext.Entry(sub).Collection(e => e.JwLinkPartDatas).Load();
                        this.dbContext.Entry(sub).Collection(e => e.JwLianjieDatas).Load();
                        if (sub.JwBeamDatas.Count > 0)
                        {
                            foreach (var bd in sub.JwBeamDatas)
                            {
                                this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                                this.dbContext.Entry(bd).Collection(e => e.JwBeamVerticalDatas).Load();
                            }
                        }
                    }
                    ProjectDetail detail = new ProjectDetail(qmain);
                    detail.ShowDialog();
                }
            }
        }

        private void uiDataGridView1_SelectIndexChange_1(object sender, int index)
        {
            if (!IsClose)
            {
                if (this.uiDataGridView1.SelectedIndex != -1)
                {
                    this.uiButton2.Enabled = true;
                }
                else
                {
                    this.uiButton2.Enabled = false;
                }

            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowAskDialog("本当に削除しますか?"))
            {

            }
        }
    }
}
 