using AntdUI;
using AntdUI.Svg;
using JwCore;
using JwShapeCommon;
using Org.BouncyCastle.Asn1.Pkcs;
using RGBControls.Classes;
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
                new AntdUI.ContextMenuStripItemDivider()
            };
            base.InitData();
        }

        private void NewProjectMainPage_Load(object sender, EventArgs e)
        {
            this.dbContext?.Database.EnsureCreated();

            this.dbContext?.JwProjectMainDatas.Load();

            this.dbContext?.JwCustomerDatas.Load();
            //var z = this.uiDataGridView1.Columns["jwCustomerDataIdDataGridViewTextBoxColumn"] as DataGridViewComboBoxColumn;
            //z.DataSource = dbContext?.JwCustomerDatas.ToList();
            this.jwProjectMainDataBindingSource.DataSource = dbContext?.JwProjectMainDatas.Local.ToBindingList();
            this.projectmaintable.DataSource = this.jwProjectMainDataBindingSource;
        }

        private void projectmaintable_SelectIndexChanged(object sender, EventArgs e)
        {
            if (projectmaintable.SelectedIndex > 0)
            {
                var selecteddata = this.projectmaintable[projectmaintable.SelectedIndex - 1].record as JwProjectMainData;
                if (selecteddata != null)
                {
                    if (selecteddata.JwProjectSubDatas != null)
                    {
                        this.dbContext.Entry(selecteddata).Collection(e => e.JwProjectSubDatas).Load();
                        this.table1.DataSource = selecteddata.JwProjectSubDatas.ToList();
                    }
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

        AntdUI.IContextMenuStripItem[] menulist = { };

        JwProjectSubData selectedsubData;

        private void contextMenuStrip1_Opening(ContextMenuStripItem e)
        {
            if (e.Text.Equals("輸出-梁JW"))
            {
                if (selectedsubData != null)
                {
                    if (AntdUI.Modal.open(this, "ヒント", "プロジェクトデータをすべてエクスポートするかどうか") == DialogResult.OK)
                    {

                        SaveSubBeams(selectedsubData);
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


                    SaveSubTopCanvas(selectedsubData);
                }
            }
            if (e.Text.Equals("輸出-番付図下JW"))
            {
                if (selectedsubData != null)
                {
                    SaveSubBottomCanvas(selectedsubData);
                }
            }
            if (e.Text.Equals("ブレース施工図"))
            {
                if (selectedsubData != null)
                {
                    SaveSubCanvasLines(selectedsubData);
                }
            }

        }

        int nowhoverrow = -1;
        private void table1_CellHover(object sender, TableHoverEventArgs e)
        {
            nowhoverrow = e.RowIndex;
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

        private string _nowsavefold = "";



        #region 各类jww及施工图导出

        private void SaveSubBeams(JwProjectSubData data)
        {

            var maindata = this.dbContext.JwProjectMainDatas.First(t => t.Id == data.JwProjectMainDataId);

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

        private void SaveSubTopCanvas(JwProjectSubData data)
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

                this.dbContext.Entry(data).Collection(e => e.JwBeamDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwPillarDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwLinkPartDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwLianjieDatas).Load();
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                    }
                }
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst=jwCanvas.DrawShigong(true);
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

        private void SaveSubBottomCanvas(JwProjectSubData data)
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

                this.dbContext.Entry(data).Collection(e => e.JwBeamDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwPillarDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwLinkPartDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwLianjieDatas).Load();
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
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
        private void SaveSubCanvasLines(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            string flname=string.Format("{0}_ブレース施工図.jww", data.FloorName);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }

                this.dbContext.Entry(data).Collection(e => e.JwBeamDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwPillarDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwLinkPartDatas).Load();
                this.dbContext.Entry(data).Collection(e => e.JwLianjieDatas).Load();
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
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

        /// <summary>
        /// 双击项目行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectmaintable_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                WarningMsg("読み込み中");
                var z = projectmaintable[e.RowIndex - 1].record as JwProjectMainData;
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

        #region 新增项目


        private void button1_Click(object sender, EventArgs e)
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

        JwProjectMainData selectedmaindata;

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

        private void table1_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.RowIndex > -1)
            {

                var z = table1[e.RowIndex-1].record as JwProjectSubData;
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
}
