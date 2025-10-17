using JwCore;
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

namespace RGBJWMain.Forms
{
    public partial class JwMainEdit : UIEditForm
    {
        public JwMainEdit()
        {
            InitializeComponent();
        }

        public List<JwCustomerData> jwCustomerDatas;

        private JwProjectMainData _jwProjectMainData;

        private void JwMainEdit_Load(object sender, EventArgs e)
        {
            if (jwCustomerDatas != null)
            {
                this.uiComboBox1.DataSource = jwCustomerDatas;
                this.uiComboBox1.DisplayMember = "CompanyName";
                this.uiComboBox1.ValueMember = "Id";
            }
            
        }

        public JwProjectMainData JwProjectMainData
        {
            get
            {
                if (_jwProjectMainData == null)
                {
                    _jwProjectMainData = new JwProjectMainData();
                }
                _jwProjectMainData.ProjectName = this.uiTextBox1.Text;
                _jwProjectMainData.SiteAddress= this.uiTextBox2.Text;
                _jwProjectMainData.JwCustomerDataId = (long)this.uiComboBox1.SelectedValue;
                _jwProjectMainData.FloorQuantity= Convert.ToInt32(this.uiTextBox3.Text);
                return _jwProjectMainData;
            }
            set
            {
                _jwProjectMainData = value;
                if (_jwProjectMainData != null)
                {
                    this.uiTextBox1.Text = _jwProjectMainData.ProjectName;
                    //this.txtBiaochi.Text = _jwProjectMainData.Biaochi ?? string.Empty;
                    this.uiTextBox2.Text = _jwProjectMainData.SiteAddress;
                    this.uiTextBox3.Text = _jwProjectMainData.FloorQuantity.ToString();
                    if (jwCustomerDatas != null)
                    {
                        this.uiComboBox1.DataSource = jwCustomerDatas;
                        this.uiComboBox1.DisplayMember = "CompanyName";
                        this.uiComboBox1.ValueMember = "Id";
                    }
                }
            }
        }

    }
}
