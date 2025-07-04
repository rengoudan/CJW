using JwCore;
using JwData;
using JwShapeCommon;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Pages
{
    public partial class ProjectOverview : UIPage
    {
        public JwDataContext? dbContext;
        public ProjectOverview()
        {
            InitializeComponent();
        }

        private JwProjectMainData _projectMainData;

        public ProjectOverview(JwProjectMainData projectMainData)
        {
            InitializeComponent();
            dbContext = ContextFactory.GetContext();
            _projectMainData = projectMainData;
            init();
        }

        private void init()
        {
            if (_projectMainData != null)
            {
                //this.uiLine1.Text = _projectMainData.ProjectName;
                this.Text = string.Format("{0} -プロジェクトの詳細", _projectMainData.ProjectName);
                this.uiMarkLabel2.Text = _projectMainData.JwProjectSubDatas.Count.ToString();
            }
        }

        /// <summary>
        /// 输出梁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (_projectMainData != null)
            {
                SaveBeams(_projectMainData);
            }
        }

        /// <summary>
        /// 导出连接线excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 增加新项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {

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

                    //this.dbContext.Entry(sub).Collection(e => e.JwBeamDatas).Load();
                    //this.dbContext.Entry(sub).Collection(e => e.JwPillarDatas).Load();
                    //this.dbContext.Entry(sub).Collection(e => e.JwLinkPartDatas).Load();
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

        private void ProjectOverview_Load(object sender, EventArgs e)
        {
            if (_projectMainData!=null)
            {
                this.jwProjectSubDataBindingSource.DataSource = _projectMainData.JwProjectSubDatas;
                //this.jwBeamDatasBindingSource.DataSource = _projectMainData.JwProjectSubDatas;
                ObservableCollectionListSource<JwBeamData> beamDatas = new ObservableCollectionListSource<JwBeamData>();
                ObservableCollectionListSource<JwPillarData> pillarDatas = new ObservableCollectionListSource<JwPillarData>();
                ObservableCollectionListSource<JwLinkPartData> linkDatas = new ObservableCollectionListSource<JwLinkPartData>();
                foreach (var sub in _projectMainData.JwProjectSubDatas)
                {
                    if (sub.JwBeamDatas != null && sub.JwBeamDatas.Count > 0)
                    {
                        foreach (var bd in sub.JwBeamDatas)
                        {
                            beamDatas.Add(bd);
                        }
                    }
                    if (sub.JwPillarDatas != null && sub.JwPillarDatas.Count > 0)
                    {
                        foreach (var pd in sub.JwPillarDatas)
                        {
                            pillarDatas.Add(pd);
                        }
                    }
                    if (sub.JwLinkPartDatas != null && sub.JwLinkPartDatas.Count > 0)
                    {
                        foreach (var ld in sub.JwLinkPartDatas)
                        {
                            linkDatas.Add(ld);
                        }
                    }
                }
                this.jwBeamDatasBindingSource.DataSource= beamDatas;
                this.jwPillarDatasBindingSource.DataSource = pillarDatas;
                this.jwLianjieDatasBindingSource.DataSource = linkDatas;
                //this.uiDataGridView4.DataSource = beamDatas;
            }
        }


    }
}
