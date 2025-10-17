using JwShapeCommon;
using JwwHelper;
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
    public partial class RGBJwControl : UserControl
    {

        double _minx;
        public double Minx
        {
            get { return _minx; }
            set { _minx = value; }
        }

        double _maxx;
        public double Maxx
        {
            get { return _maxx; }
            set
            {
                _maxx = value;
            }
        }
        double _miny;
        public double Miny
        {
            get => _miny;
            set
            {
                _miny = value;
            }
        }
        double _maxy;
        public double Maxy
        {
            get => _maxy;
            set
            {
                _maxy = value;
            }
        }
        List<JwwSen> _sens;
        public List<JwwSen> Sens
        {
            get => _sens;
            set
            {
                _sens = value;
            }
        }
        List<JwwSolid> _solid;
        public List<JwwSolid> Solid
        {
            get => _solid;
            set => _solid = value;
        }

        bool isdraw = false;
        public bool IsDraw
        {
            get { return isdraw; }
            set => isdraw = value;
        }
        List<int> _colors;
        public List<int> Colors
        {
            get => _colors;
            set => _colors = value;
        }
        Dictionary<int, Color> _colors2 = new Dictionary<int, Color>();
        List<JwBlock> _blocks;
        public List<JwBlock> Blocks
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
            }
        }

        public RGBJwControl()
        {
            InitializeComponent();
            uiSwitch1.Active = true;
            uiSwitch2.Active = true;
            rgbJwwShow1.init();
        }

        public void Draw()
        {
            rgbJwwShow1.Minx = _minx;
            rgbJwwShow1.Maxx = _maxx;
            rgbJwwShow1.Miny = _miny;
            rgbJwwShow1.Maxy = _maxy;
            rgbJwwShow1.Sens = _sens;
            rgbJwwShow1.Blocks = _blocks;
            rgbJwwShow1.Colors = _colors;
            rgbJwwShow1.Solid = _solid;
            rgbJwwShow1.createcolors();
            rgbJwwShow1.IsDraw = true;
            rgbJwwShow1.draw();
            Invalidate();
        }

        private void uiSwitch2_ValueChanged(object sender, bool value)
        {
            rgbJwwShow1.ShowShape = value;
        }

        private void uiSwitch1_ValueChanged(object sender, bool value)
        {
            rgbJwwShow1.ShowSen = value;
        }
    }
}
