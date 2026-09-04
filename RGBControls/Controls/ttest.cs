using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Controls
{
    public partial class ttest : Control
    {
        public ttest()
        {
            this.BackColor = Color.Black;
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var shape = new BeamEndContour
            {
                BaseCenter = new PointF(0, 0),   // 工程坐标系
                Position = BeamEndPosition.上右,
                Scale = 1.0f,
                Offset = new PointF(300, 300)    // 屏幕坐标偏移
            };

            shape.DrawWithCircles(
        pe.Graphics,
        new Pen(Color.Yellow, 1),
        new Pen(Color.Cyan, 1)
    );
            //Console.WriteLine(shape.Description);
        }
    }
}
