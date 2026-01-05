using JwShapeCommon;
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

namespace RGBControls.Forms
{
    public partial class CsvSettings : UserControl
    {
        Form form;
        public CsvSettings(Form form)
        {
            InitializeComponent();
            this.form = form;
            input1.TextChanged += Input1_TextChanged;
            input2.TextChanged += Input2_TextChanged;
            input3.TextChanged += Input3_TextChanged;
            input4.TextChanged += Input4_TextChanged;
            input1.Text = JwFileConsts.CsvHxNum.ToString();
            input2.Text = JwFileConsts.CsvHxJianju.ToString();
            input3.Text = JwFileConsts.CsvZxNum.ToString();
            input4.Text = JwFileConsts.CsvZxJianju.ToString();

        }

        private int _hxnum;
        public int Hxnum
        {
            get { return _hxnum; }
            set { _hxnum = value; }
        }

        private double _hxjianju;
        public double Hxjianju
        {
            get { return _hxjianju; }
            set { _hxjianju = value; }
        }

        private int _zxnum;
        public int Zxnum
        {
            get { return _zxnum; }
            set { _zxnum = value; }
        }

        private double _zxjianju;

        public double Zxjianju
        {
            get { return _zxjianju; }
            set { _zxjianju = value; }
        }

        private double _ytiaozheng;
        public double Ytiaozheng
        {
            get { return _ytiaozheng; }
            set
            {
                _ytiaozheng = value;
            }
        }

        private void Input1_TextChanged(object? sender, EventArgs e)
        {
            if (input1.Text.IsNumber())
            {
                _hxnum = Convert.ToInt32(input1.Text);
            }
            else
            {
                _hxnum = 1;
            }
        }

        private void Input2_TextChanged(object? sender, EventArgs e)
        {
            if (input2.Text.IsNumber())
            {
                _hxjianju = Convert.ToDouble(input2.Text);
            }
            else
            {
                _hxjianju = 0.0;
            }
        }


        private void Input3_TextChanged(object? sender, EventArgs e)
        {
            if (input3.Text.IsNumber())
            {
                _zxnum = Convert.ToInt32(input3.Text);
            }
            else
            {
                _zxnum = 1;
            }
        }

        private void Input4_TextChanged(object? sender, EventArgs e)
        {
            if (input4.Text.IsNumber())
            {
                _zxjianju = Convert.ToDouble(input4.Text);
            }
            else
            {
                _zxjianju = 0.0;
            }
        }

        private void uiDoubleUpDown1_ValueChanged(object sender, double value)
        {
            _ytiaozheng = uiDoubleUpDown1.Value;
        }
    }
}
