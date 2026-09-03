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
                BaseCenter = new PointF(300, 400),
                Position = BeamEndPosition.上左,
                //Scale = 1.2f,
                //Offset = new PointF(50, 50)
            };

            shape.Draw(pe.Graphics, new Pen(Color.Yellow, 2), new Pen(Color.Yellow, 2));

            //Console.WriteLine(shape.Description);
        }
    }
}
