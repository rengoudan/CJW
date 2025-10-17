using JwShapeCommon;
using JwwHelper;
using RGBJWMain.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double _minx;
        double _maxx;
        double _miny;
        double _maxy;
        List<JwwSen> _sens;
        List<JwwSolid> _solid;

        bool isdraw = false;

        List<int> _colors;
        List<JwBlock> _blocks;

        public Form1(double minx, double maxx, double miny, double maxy, List<JwwSen> sens, List<JwwSolid> solids,List<int> colors,List<JwBlock> blocks)
        {
            InitializeComponent(); _minx = minx;
            _maxx = maxx;
            _miny = miny;
            _maxy = maxy;
            _sens = sens;
            _solid = solids;
            isdraw = true;
            _colors = colors;
            _blocks = blocks;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            if(isdraw)
            {
                RGBJwControl rGBJwControl = new RGBJwControl();
                rGBJwControl.Minx = _minx;
                rGBJwControl.Maxx = _maxx;
                rGBJwControl.Miny = _miny;
                rGBJwControl.Maxy = _maxy;
                rGBJwControl.Sens = _sens;
                rGBJwControl.Blocks = _blocks;
                rGBJwControl.Colors = _colors;
                rGBJwControl.Solid = _solid;
                rGBJwControl.Dock = DockStyle.Fill;
                rGBJwControl.Location = new Point(0, 0);
                rGBJwControl.TabIndex = 0;
                rGBJwControl.IsDraw = true;
                Controls.Add(rGBJwControl);
                rGBJwControl.Draw();
                //RGBJwwShow rgbJwwShow1 = new RGBJwwShow(_minx, _maxx, _miny, _maxy, _sens, _solid,_colors, _blocks);
                //rgbJwwShow1.Dock = DockStyle.Fill;
                //rgbJwwShow1.Location = new Point(0, 0);
                //rgbJwwShow1.Name = "rgbJwwShow1";
                //rgbJwwShow1.Size = new Size(800, 450);
                //rgbJwwShow1.TabIndex = 0;
                //Controls.Add(rgbJwwShow1);
                //rgbJwwShow1.draw();
            }
        }
    }
}
