using JwShapeCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Controls
{
    public partial class NewSingleBeamControl : Control
    {
        public NewSingleBeamControl()
        {
            InitializeComponent();
        }

        private JwBeam _showBeam;
        public JwBeam ShowBeam
        {
            get { return _showBeam; }
            set
            {
                _showBeam = value;

                _jwDrawShape=new NewJwBeamJwDraw(_showBeam);
                //_jwDrawShape.
            }
        }


        private NewJwBeamJwDraw _jwDrawShape;
        public NewJwBeamJwDraw JwDrawShape
        {
            get
            {
                return _jwDrawShape;
            }
            set
            {
                _jwDrawShape = value;
                
                ////_jwDrawShape.Draw();
                //if (_jwDrawShape.controls != null)
                //{
                //    _bounds = _jwDrawShape.controls;
                //    _lines = _jwDrawShape.Lines;
                //    Invalidate();
                //}
            }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
