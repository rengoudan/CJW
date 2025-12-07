using JwCore;
using JwShapeCommon;
using RGBJWMain.Forms;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Controls
{
    public partial class JwCanvasControl : UserControl
    {
        public JwCanvasControl()
        {

            InitializeComponent();
            this.MouseDown += JwCanvasControl_MouseDown;
        }

        bool isleftdown = false;

        public bool IsNewCanvas = false;    

        
        private void JwCanvasControl_MouseDown(object? sender, MouseEventArgs e)
        {
            isleftdown = false;
            if (e.Button == MouseButtons.Left)
            {
                isleftdown = true;
            }
            else
            {
                isleftdown = false;
            }
        }

        private JwCanvasDraw _canvasDraw;

        public JwCanvasDraw CanvasDraw
        {
            get { return _canvasDraw; }
            set
            {
                _canvasDraw = value;
                if (_canvasDraw != null)
                {
                    uiSwitch1.Enabled = true;
                    jwShowBeams1.init();
                    uiSwitch1.Active = true;
                    uiShowfuzhu.Active = true;
                    uiShowpillar.Active = true;
                    uiGoujian.Active = true;
                    uiSwitch2.Active = true;
                    uiSDown.Active = true;
                    jwShowBeams1.IsNewCanvas= IsNewCanvas;
                    jwShowBeams1.CanvasDraw = _canvasDraw;

                    Invalidate();
                    //_canvasDraw.Draw(Width, Height, 20, 20);
                    //_jwDrawShape.Draw();
                    if (_canvasDraw.controls != null)
                    {

                    }
                }
            }
        }

        public JwBeam SelectedBeam { get; set; }

        public bool BeamSelected { get; set; }

        private void uiSwitch1_ValueChanged(object sender, bool value)
        {
            jwShowBeams1.ShowBeams = value;
        }

        private void uiShowpillar_ValueChanged(object sender, bool value)
        {
            jwShowBeams1.ShowPillar = value;
        }

        private void uiShowfuzhu_ValueChanged(object sender, bool value)
        {
            jwShowBeams1.ShowFuzhu = value;
        }

        private void uiGoujian_ValueChanged(object sender, bool value)
        {
            jwShowBeams1.ShowGoujian = value;
        }

        //private void jwShowBeams1_Click(object sender, EventArgs e)
        //{
        //    //if (isleftdown)
        //    //{
        //        SelectedBeam = jwShowBeams1.SelectedBeam;
        //        if (SelectedBeam != null)
        //        {
        //            BeamSelected = true;
        //            //SelectBeamEvent(sender, e);

        //            var z = SelectedBeam;

        //            if (z != null)
        //            {
        //                //if (z.DirectionType == BeamDirectionType.Horizontal)
        //                //{
        //                //    z.AbsolutePD = z.TopLeft.X;
        //                //}
        //                //if (z.DirectionType == BeamDirectionType.Vertical)
        //                //{
        //                //    z.AbsolutePD = z.BottomLeft.Y;
        //                //}
        //                //var q = z.jwBeamMarks;
        //                //JwSingleBeamForm jsForm = new JwSingleBeamForm(z);

        //                ////jsForm.ShowBeam = js;
        //                ////jsForm.sha
        //                //jsForm.ShowDialog();

        //                NewJwBeamForm jsForm = new NewJwBeamForm(z);
        //                jsForm.Show();

        //            }

        //        }
        //    //}

        //}

        private void JwCanvasControl_Click(object sender, EventArgs e)
        {

        }

        public EventHandler SelectBeamEvent { get; set; }

        private void JwCanvasControl_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void JwCanvasControl_AutoSizeChanged(object sender, EventArgs e)
        {
            jwShowBeams1.Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();
            jwShowBeams1.Refresh();
        }

        public void jwToPng(string path)
        {
            var width = jwShowBeams1.Size.Width;
            var height = jwShowBeams1.Size.Height;
            using (var bmp = new Bitmap(width, height))
            {
                jwShowBeams1.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(path, ImageFormat.Png);
            }
        }

        private void uiSwitch2_ValueChanged(object sender, bool value)
        {
            jwShowBeams1.Showmsg = value;
        }

        private void uiSDown_ValueChanged(object sender, bool value)
        {
            jwShowBeams1.ShowDownB = value;
        }

        private void uiSwitch3_ValueChanged(object sender, bool value)
        {
            jwShowBeams1.ShowGoujiantext = value;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
