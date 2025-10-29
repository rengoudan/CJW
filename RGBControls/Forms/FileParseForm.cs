using JwCore;
using JwData;
using JwShapeCommon;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Forms
{
    public partial class FileParseForm : UIForm2
    {
        public JwDataContext? dbContext;
        public FileParseForm()
        {
            InitializeComponent();
        }

        JwProjectPathModel? jwProjectPathModel = null!;
        string _path;
        public FileParseForm(JwProjectPathModel model)
        {
            InitializeComponent();
            jwProjectPathModel = model;
            GlobalEvent.GetGlobalEvent().ShowParseLogEvent += ShowParseLog;
        }


        private void FileParseForm_Load(object sender, EventArgs e)
        {
            dbContext = ContextFactory.GetContext();
            if (jwProjectPathModel != null)
            {
                if (!string.IsNullOrEmpty(jwProjectPathModel.Path))
                {
                    _path = jwProjectPathModel.Path;
                    this.ShowProcessForm(200);
                    readjww();

                    //ThreadStart start = new ThreadStart(readjww);
                    //Thread thread = new Thread(start);
                    //thread.Priority = ThreadPriority.Highest;
                    //thread.IsBackground = true; //关闭窗体继续执行
                    //thread.Start();
                }
            }
        }
        JwFileHandle? _jwFileHandle = null!;

        private void readjww()
        {
            _jwFileHandle = new JwFileHandle(jwProjectPathModel);
            _jwFileHandle.ReadJwFile();

            var bb = JwFileConsts.BeamParseColor;
            var bc = JwFileConsts.BeamSymbolTextColor;
            //jwfh.ParseBySetting();
            //jwfh.ChangeJwXianFromJwwSen();
            //var sl = jwfh.SenLst;
            //jwfh.ChangePillarFromJwwSolid();
            //jwfh.ParseSquareCreatePillar();
            //jwfh.ChangeJwwEnojiToText();
            //jwfh.ChangeQieGeSolis();
            _jwFileHandle.FollowTheStep();
            var jc = _jwFileHandle.CreateCanvas();
            JwCanvasDraw canvasDraw = new JwCanvasDraw(jc);
            if (jwCanvasControl1.InvokeRequired)
            {
                this.HideProcessForm();
                jwCanvasControl1.Invoke(() =>
                {
                    jwCanvasControl1.CanvasDraw = canvasDraw;
                    //jwCanvasControl1.Click += JwCanvasControl1_Click;
                    jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
                });
            }
            else
            {
                this.HideProcessForm();
                jwCanvasControl1.CanvasDraw = canvasDraw;
                //jwCanvasControl1.Click += JwCanvasControl1_Click;
                jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
            }
            if (uiSymbolButton1.InvokeRequired)
            {
                uiSymbolButton1.Invoke(() => { uiSymbolButton1.Enabled = true; });
            }
            else
            {
                uiSymbolButton1.Enabled = true;
            }
            //jwShowBeams1.Canvas = jc;
            //jwShowBeams1.CreateBeams();
            //jwShowBeams1.CanvasDraw = canvasDraw;
            //jwShowBeams1.Click += JwShowBeams1_Click;
            //jwfh.sss();
            //分割需要去除重复
            //var lst = jwfh.SolidLst.Select(t => t.m_nPenColor).ToList().Distinct().ToList();
            //var s = jwfh.RectangleBlocks;
        }

        private void ShowParseLog(object sender, ShowParseLogArgs e)
        {
            string mg = string.Format("{0}{1}", e.Msg, Environment.NewLine);
            if (uiTextBox1.InvokeRequired)
            {
                uiTextBox1.BeginInvoke(() =>
                {
                    uiTextBox1.AppendText(mg);
                });
            }
            else
            {

                if (e.ShowTime && e.UpdateTime.HasValue)
                {

                }
                //uiTextBox1.AppendText(mg);
                uiTextBox1.BeginInvoke(() =>
                {
                    uiTextBox1.AppendText(mg);
                });
            }
        }

        private void JwCanvas_Click(object? sender, EventArgs e)
        {
            if (jwCanvasControl1.BeamSelected)
            {
                var z = jwCanvasControl1.SelectedBeam;

                if (z != null)
                {
                    //JwSingleBeamForm jsForm = new JwSingleBeamForm(z);

                    ////jsForm.ShowBeam = js;
                    ////jsForm.sha
                    //jsForm.ShowDialog();
                    NewJwBeamForm j=new NewJwBeamForm(z);
                    j.ShowDialog();
                }
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (_jwFileHandle != null)
            {
                _jwFileHandle.CreateData();
                if (_jwFileHandle._subData != null)
                {
                    string fn = _jwFileHandle._subData.FloorName;
                    var sbd = dbContext?.JwProjectSubDatas.ToList().Find(t => t.FloorName == fn&&t.JwProjectMainDataId== jwProjectPathModel.MainData.Id);
                    if (sbd == null)
                    {
                        SaveData();
                    }
                    else
                    {
                        UIMessageBox.ShowError("このフロアは識別されアップロードされました");
                    }
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void SaveData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (_jwFileHandle._subData != null)
                    {

                        _jwFileHandle._subData.DefaultBeamXHId = jwProjectPathModel.MaterialData.Id;
                        _jwFileHandle._subData.DefaultBeamXHName = jwProjectPathModel.MaterialData.GeneralTitle;
                        dbContext?.JwProjectSubDatas.Add(_jwFileHandle._subData);
                        if (_jwFileHandle._beamdatas.Count > 0)
                        {
                            foreach (var pl in _jwFileHandle._beamdatas)
                            {
                                pl.BeamXHId = jwProjectPathModel.MaterialData.Id;
                                pl.BeamXHName = jwProjectPathModel.MaterialData.GeneralTitle;
                                dbContext?.JwBeamDatas.Add(pl);

                            }
                        }
                        if (_jwFileHandle._jwbvdatas.Count > 0)
                        {
                            foreach (var bv in _jwFileHandle._jwbvdatas)
                            {
                                dbContext?.JwBeamVerticalDatas.Add(bv);
                            }
                        }
                        if (_jwFileHandle._holeDatas.Count > 0)
                        {
                            foreach (var hd in _jwFileHandle._holeDatas)
                            {
                                dbContext?.JwHoleDatas.Add(hd);
                            }
                        }
                        if (_jwFileHandle._beampillarDatas.Count > 0)
                        {
                            foreach (var pd in _jwFileHandle._beampillarDatas)
                            {
                                dbContext?.JwPillarDatas.Add(pd);
                            }
                        }
                        if (_jwFileHandle._linkPartDatas.Count > 0)
                        {
                            foreach (var lp in _jwFileHandle._linkPartDatas)
                            {
                                dbContext?.JwLinkPartDatas.Add(lp);
                            }
                        }
                        if (_jwFileHandle._lianjieDatas.Count > 0)
                        {
                            foreach (var jlj in _jwFileHandle._lianjieDatas)
                            {
                                dbContext?.JwLianjieDatas.Add(jlj);
                            }
                        }
                        var md = dbContext.JwProjectMainDatas.Find(jwProjectPathModel.MainData.Id);
                        md.BCount += _jwFileHandle._subData.BCount;
                        md.BGCount += _jwFileHandle._subData.BGCount;
                        if (md.BeamsNumber.HasValue)
                        {
                            md.BeamsNumber += _jwFileHandle._subData.BeamCount;
                        }
                        else
                        {
                            md.BeamsNumber = _jwFileHandle._subData.BeamCount;
                        }

                        md.PillarCount += _jwFileHandle._subData.PillarCount;
                        md.KPillarCount += _jwFileHandle._subData.KPillarCount;
                        md.SinglePillarCount += _jwFileHandle._subData.SinglePillarCount;
                        md.ParsedQuantity += 1;
                        dbContext?.SaveChanges();
                    }
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    if (_jwFileHandle._subData != null)
                    {

                        _jwFileHandle._subData.DefaultBeamXHId = jwProjectPathModel.MaterialData.Id;
                        _jwFileHandle._subData.DefaultBeamXHName = jwProjectPathModel.MaterialData.GeneralTitle;
                        dbContext?.JwProjectSubDatas.Add(_jwFileHandle._subData);
                        if (_jwFileHandle._beamdatas.Count > 0)
                        {
                            foreach (var pl in _jwFileHandle._beamdatas)
                            {
                                pl.BeamXHId = jwProjectPathModel.MaterialData.Id;
                                pl.BeamXHName = jwProjectPathModel.MaterialData.GeneralTitle;
                                dbContext?.JwBeamDatas.Add(pl);

                            }
                        }
                        if (_jwFileHandle._jwbvdatas.Count > 0)
                        {
                            foreach (var bv in _jwFileHandle._jwbvdatas)
                            {
                                dbContext?.JwBeamVerticalDatas.Add(bv);
                            }
                        }
                        if (_jwFileHandle._holeDatas.Count > 0)
                        {
                            foreach (var hd in _jwFileHandle._holeDatas)
                            {
                                dbContext?.JwHoleDatas.Add(hd);
                            }
                        }
                        if (_jwFileHandle._beampillarDatas.Count > 0)
                        {
                            foreach (var pd in _jwFileHandle._beampillarDatas)
                            {
                                dbContext?.JwPillarDatas.Add(pd);
                            }
                        }
                        if (_jwFileHandle._linkPartDatas.Count > 0)
                        {
                            foreach (var lp in _jwFileHandle._linkPartDatas)
                            {
                                dbContext?.JwLinkPartDatas.Add(lp);
                            }
                        }
                        if (_jwFileHandle._lianjieDatas.Count > 0)
                        {
                            foreach (var jlj in _jwFileHandle._lianjieDatas)
                            {
                                dbContext?.JwLianjieDatas.Add(jlj);
                            }
                        }
                        var md = dbContext.JwProjectMainDatas.Find(jwProjectPathModel.MainData.Id);
                        md.BCount += _jwFileHandle._subData.BCount;
                        md.BGCount += _jwFileHandle._subData.BGCount;
                        if (md.BeamsNumber.HasValue)
                        {
                            md.BeamsNumber += _jwFileHandle._subData.BeamCount;
                        }
                        else
                        {
                            md.BeamsNumber = _jwFileHandle._subData.BeamCount;
                        }

                        md.PillarCount += _jwFileHandle._subData.PillarCount;
                        md.KPillarCount += _jwFileHandle._subData.KPillarCount;
                        md.SinglePillarCount += _jwFileHandle._subData.SinglePillarCount;
                        md.ParsedQuantity += 1;
                        dbContext?.SaveChanges();
                    }
                }));
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //ContextFactory.DisposeContext();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
