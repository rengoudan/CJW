using JwShapeCommon;
using JwSharedConfig.Config;
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
            var config = AppConfigManager.Current;
            this.Hxnum = config.Csv.Hxnum;
            this.Hxjianju= config.Csv.Hxjianju;
            this.Zxnum = config.Csv.Zxnum;
            this.Zxjianju=config.Csv.Zxjianju;
            this.Kongjing= config.Csv.Kongjing;
            input1.Text = this.Hxnum.ToString();
            input2.Text = this.Hxjianju.ToString();
            input3.Text = this.Zxnum.ToString();
            input4.Text = this.Zxjianju.ToString();
            input5.Text = this.Kongjing.ToString();
            input1.TextChanged += Input1_TextChanged;
            input2.TextChanged += Input2_TextChanged;
            input3.TextChanged += Input3_TextChanged;
            input4.TextChanged += Input4_TextChanged;
            
            input5.TextChanged += Input5_TextChanged;
            //
        }

        private void Input5_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                _kongjing = Convert.ToDouble(input5.Text);
            }
            catch { }
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
        private double _kongjing;
        public double Kongjing
        {
            get { return _kongjing; }
            set
            {
                _kongjing = value;
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
            try
            {
                _hxjianju = Convert.ToDouble(input2.Text);
            }
            catch
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
            try
            {
                _zxjianju = Convert.ToDouble(input4.Text);
            }
            catch
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
