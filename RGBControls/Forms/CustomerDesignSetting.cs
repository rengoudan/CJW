using JwShapeCommon.Jwbase;
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
using JwCore;
using JwData;
using NPOI.POIFS.Crypt.Dsig;

namespace RGBJWMain.Forms
{
    public partial class CustomerDesignSetting : Base
    {
        public CustomerDesignSetting()
        {
            InitializeComponent();
        }
        private JwCustomerData? JwCustomer = null!;

        private JwCustDesignConstData? jwCustDesignConstData = null!;

        public CustomerDesignSetting(JwCustomerData customerData)
        {
            InitializeComponent();
            this.JwCustomer = customerData;
            this.uiMarkLabel6.Visible = false;
            this.uiTextBox1.Visible = false;  
            this.uiMarkLabel7.Visible = false;
            this.uiComboBox3.Visible = false;
        }

        private bool isloadbyparse = false;

        private string _path = "";
        private JwProjectPathModel? propn = new();

        public CustomerDesignSetting(JwProjectPathModel customerData)
        {
            InitializeComponent();
            if (customerData.MainData.JwCustomerDataId.HasValue)
            {
                this.JwCustomer = customerData.MainData.JwCustomerData;
            }
            isloadbyparse = true;
            _path = customerData.Path;
            propn = customerData;
        }



        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    List<JwColor> ls = JwFileConsts.GetJwColors();
        //    List<JwPenStyle> ps = JwFileConsts.GetJwPenStyles();

        //    uiCbBeamcolor.DataSource = ls;
        //    uiCbBeamcolor.DisplayMember = "JwColorName";
        //    uiCbBeamcolor.ValueMember = "ColorNumber";

        //    uiCbTextColor.DataSource = ls;
        //    uiCbTextColor.DisplayMember = "JwColorName";
        //    uiCbTextColor.ValueMember = "ColorNumber";

        //    uiCbpillarcolor.DataSource = ls;
        //    uiCbpillarcolor.DisplayMember = "JwColorName";
        //    uiCbpillarcolor.ValueMember = "ColorNumber";

        //    uiCbSplitcolor.DataSource = ls;
        //    uiCbSplitcolor.DisplayMember = "JwColorName";
        //    uiCbSplitcolor.ValueMember = "ColorNumber";

        //    uiComboBox2.DataSource = ps;
        //    uiComboBox2.DisplayMember = "JwPenStyleName";
        //    uiComboBox2.ValueMember = "StyleNumber";

        //    uiComboBox1.DataSource = ps;
        //    uiComboBox1.DisplayMember = "JwPenStyleName";
        //    uiComboBox1.ValueMember = "StyleNumber";
        //    //if (JwCustomer != null)
        //    //{
        //    //    var z= dbContext?.JwCustDesignConstDatas.ToList().Find(t => t.JwCustomerDataId == JwCustomer.Id);
        //    //    if(z != null)
        //    //    {
        //    //        jwCustDesignConstData = z;
        //    //        uiCbBeamcolor.SelectedValue = z.BeamParseColorNumber;
        //    //        uiCbTextColor.SelectedValue = z.BeamSymbolTextColorNumber;
        //    //        uiCbSplitcolor.SelectedValue = z.BeamSplitParseColor;
        //    //        uiComboBox2.SelectedValue = z.PillarPenStyle;
        //    //        uiComboBox1.SelectedValue = z.SplitPenStyle;
        //    //        uiNearSpliteMax.Value= z.NearSpliteMax;
        //    //    }
        //    //}

        //}

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (JwCustomer != null && jwCustDesignConstData == null)
            {
                if (jwCustDesignConstData == null)
                {
                    JwCustDesignConstData data = new JwCustDesignConstData();
                    var z = uiCbBeamcolor.SelectedItem as JwColor;
                    data.BeamParseColorNumber = z.ColorNumber;
                    var z1 = uiCbTextColor.SelectedItem as JwColor;
                    data.BeamSymbolTextColorNumber = z1.ColorNumber;
                    var z2 = uiCbSplitcolor.SelectedItem as JwColor;
                    data.BeamSplitParseColor = z2.ColorNumber;
                    var z3 = uiCbpillarcolor.SelectedItem as JwColor;
                    data.BeamPillarParseColor = z3.ColorNumber;
                    var z4 = uiComboBox2.SelectedItem as JwPenStyle;
                    data.PillarPenStyle = z4.StyleNumber;
                    var z5 = uiComboBox1.SelectedItem as JwPenStyle;
                    data.SplitPenStyle = z5.StyleNumber;
                    data.NearSpliteMax = uiNearSpliteMax.Value;
                    var  z6=uidownpillarcolor.SelectedItem as JwColor;
                    data.DownPillarColorNumber = z6.ColorNumber;

                    var z7 = uiComboBox4.SelectedItem as JwColor;
                    jwCustDesignConstData.LianjieColorNumber = z7.ColorNumber;
                    if (JwCustomer != null)
                    {
                        data.JwCustomerDataId = JwCustomer.Id;
                    }
                    dbContext.JwCustDesignConstDatas.Add(data);
                }
                else
                {
                    var z = uiCbBeamcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamParseColorNumber = z.ColorNumber;
                    var z1 = uiCbTextColor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamSymbolTextColorNumber = z1.ColorNumber;
                    var z2 = uiCbSplitcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamSplitParseColor = z2.ColorNumber;
                    var z3 = uiCbpillarcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamPillarParseColor = z3.ColorNumber;
                    var z4 = uiComboBox2.SelectedItem as JwPenStyle;
                    jwCustDesignConstData.PillarPenStyle = z4.StyleNumber;
                    var z5 = uiComboBox1.SelectedItem as JwPenStyle;
                    jwCustDesignConstData.SplitPenStyle = z5.StyleNumber;
                    jwCustDesignConstData.NearSpliteMax = uiNearSpliteMax.Value;

                    var z6 = uidownpillarcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.DownPillarColorNumber = z6.ColorNumber;


                    var z7 = uiComboBox4.SelectedItem as JwColor;
                    jwCustDesignConstData.LianjieColorNumber = z7.ColorNumber;
                    if (JwCustomer != null)
                    {
                        jwCustDesignConstData.JwCustomerDataId = JwCustomer.Id;
                    }
                }

                dbContext.SaveChanges();
            }
            else
            {
                if(JwCustomer != null)
                {
                    var z = uiCbBeamcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamParseColorNumber = z.ColorNumber;
                    var z1 = uiCbTextColor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamSymbolTextColorNumber = z1.ColorNumber;
                    var z2 = uiCbSplitcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamSplitParseColor = z2.ColorNumber;
                    var z3 = uiCbpillarcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.BeamPillarParseColor = z3.ColorNumber;
                    var z4 = uiComboBox2.SelectedItem as JwPenStyle;
                    jwCustDesignConstData.PillarPenStyle = z4.StyleNumber;
                    var z5 = uiComboBox1.SelectedItem as JwPenStyle;
                    jwCustDesignConstData.SplitPenStyle = z5.StyleNumber;
                    jwCustDesignConstData.NearSpliteMax = uiNearSpliteMax.Value;

                    var z6 = uidownpillarcolor.SelectedItem as JwColor;
                    jwCustDesignConstData.DownPillarColorNumber = z6.ColorNumber;

                    var z7 = uiComboBox4.SelectedItem as JwColor;
                    jwCustDesignConstData.LianjieColorNumber = z7.ColorNumber;


                    if (JwCustomer != null)
                    {
                        jwCustDesignConstData.JwCustomerDataId = JwCustomer.Id;
                    }
                }
                dbContext.SaveChanges();
            }
            if ((int)uiCbBeamcolor.SelectedValue != -1)
            {
                JwFileConsts.BeamParseColor = uiCbBeamcolor.SelectedItem as JwColor;
            }
            if ((int)uiCbTextColor.SelectedValue != -1)
            {
                JwFileConsts.BeamSymbolTextColor = uiCbTextColor.SelectedItem as JwColor;
            }
            if ((int)uiCbSplitcolor.SelectedValue != -1)
            {
                JwFileConsts.BeamSplitParseColor = uiCbSplitcolor.SelectedItem as JwColor;
            }
            if ((int)uiCbpillarcolor.SelectedValue != -1)
            {
                JwFileConsts.BeamPillarParseColor = uiCbpillarcolor.SelectedItem as JwColor;
            }

            if ((int)uiComboBox2.SelectedValue != -1)
            {
                JwFileConsts.PillarPenStyle = uiComboBox2.SelectedItem as JwPenStyle;
            }

            if ((int)uiComboBox1.SelectedValue != -1)
            {
                JwFileConsts.SplitPenStyle = uiComboBox1.SelectedItem as JwPenStyle;
            }
            if ((int)uidownpillarcolor.SelectedValue != -1){
                var qq= uidownpillarcolor.SelectedItem as JwColor;
                JwFileConsts.BRParseColore = qq.ColorNumber;
            }
            if ((int)uiComboBox4.SelectedValue != -1)
            {
                
                JwFileConsts.LianjieParseColor = uiComboBox4.SelectedItem as JwColor;
            }
            

            JwFileConsts.NearSpliteMax = uiNearSpliteMax.Value;
            propn.FloorName = uiTextBox1.Text;
            propn.MaterialData = uiComboBox3.SelectedItem as JwMaterialData;
            JwFileConsts.MaxBeamScope = JwFileConsts.MaxBeamScope + uiDoubleUpDown1.Value;

            
            DialogResult = DialogResult.OK;
        }

        private void CustomerDesignSetting_Load(object sender, EventArgs e)
        {
            //base.OnLoad(e);
            dbContext = ContextFactory.GetContext();
            List<JwColor> ls = JwFileConsts.GetJwColors();
            List<JwPenStyle> ps = JwFileConsts.GetJwPenStyles();

            uiCbBeamcolor.DataSource = ls;
            uiCbBeamcolor.DisplayMember = "JwColorName";
            uiCbBeamcolor.ValueMember = "ColorNumber";

            uiCbTextColor.DataSource = ls;
            uiCbTextColor.DisplayMember = "JwColorName";
            uiCbTextColor.ValueMember = "ColorNumber";

            uidownpillarcolor.DataSource = ls;
            uidownpillarcolor.DisplayMember = "JwColorName";
            uidownpillarcolor.ValueMember = "ColorNumber";

            uiCbpillarcolor.DataSource = ls;
            uiCbpillarcolor.DisplayMember = "JwColorName";
            uiCbpillarcolor.ValueMember = "ColorNumber";

            uiCbSplitcolor.DataSource = ls;
            uiCbSplitcolor.DisplayMember = "JwColorName";
            uiCbSplitcolor.ValueMember = "ColorNumber";

            uiComboBox2.DataSource = ps;
            uiComboBox2.DisplayMember = "JwPenStyleName";
            uiComboBox2.ValueMember = "StyleNumber";

            uiComboBox1.DataSource = ps;
            uiComboBox1.DisplayMember = "JwPenStyleName";
            uiComboBox1.ValueMember = "StyleNumber";

            uiComboBox4.DataSource = ls;
            uiComboBox4.DisplayMember = "JwColorName";
            uiComboBox4.ValueMember = "ColorNumber";


            if (JwCustomer != null)
            {
                var z = dbContext?.JwCustDesignConstDatas.ToList().Find(t => t.JwCustomerDataId == JwCustomer.Id);
                if (z != null)
                {
                    jwCustDesignConstData = z;
                    uiCbBeamcolor.SelectedValue = z.BeamParseColorNumber;
                    uiCbTextColor.SelectedValue = z.BeamSymbolTextColorNumber;
                    uiCbpillarcolor.SelectedValue = z.BeamPillarParseColor;
                    uiCbSplitcolor.SelectedValue = z.BeamSplitParseColor;
                    uiComboBox2.SelectedValue = z.PillarPenStyle;
                    uiComboBox1.SelectedValue = z.SplitPenStyle;
                    uiNearSpliteMax.Value = z.NearSpliteMax;
                    uidownpillarcolor.SelectedValue = z.DownPillarColorNumber;
                    uiComboBox4.SelectedValue = z.LianjieColorNumber;
                }
            }
            if (isloadbyparse)
            {
                uiTextBox1.Visible = true;
                uiMarkLabel6.Visible = true;

                string floorname = Path.GetFileNameWithoutExtension(_path);
                propn.FloorName = floorname;
                uiTextBox1.Text= floorname; 
               //dbContext.JwMaterialDatas.Where(t=>t.MaterialType==MaterialType.梁).ToList();
                var medlst = dbContext.JwMaterialDatas.Where(t => t.MaterialType == MaterialType.梁).ToList();
                if (medlst.Count > 0)
                {
                    uiComboBox3.DataSource = medlst;
                    uiComboBox3.DisplayMember = "MaterialName";
                    uiComboBox3.ValueMember = "Id";
                }
                else
                {
                    UIMessageBox.ShowError("基礎データがまだ整備されていない");
                    this.Close();
                }
            }

           

        }
    }
}
