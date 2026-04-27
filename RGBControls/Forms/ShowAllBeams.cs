using JwCore;
using JwServices;
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
    public partial class ShowAllBeams : AntdUI.Window
    {
        private JwProjectMainService JwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        JwProjectMainData _selectedMainData;
        List<JwBeamHuiZong> _beamHuiZongs = new List<JwBeamHuiZong>();

        public ShowAllBeams()
        {
            InitializeComponent();
        }

        public ShowAllBeams(JwProjectMainData mainData)
        {
            _selectedMainData = mainData;
            createData();
            InitializeComponent();
        }

        private async void createData()
        {
            if (_selectedMainData != null)
            {
                var beams = await JwProjectMainService.GetBeamDatasByMainAsync(_selectedMainData.Id);
                beams.GroupBy(t => t.BeamCode).ToList().ForEach(group =>
                {
                    var beamCode = group.Key;
                    var beamData = group.FirstOrDefault();
                    if (beamData != null)
                    {
                        JwBeamHuiZong db = new JwBeamHuiZong();
                        db.BeamCode = beamCode;
                        db.BeamCount = group.Count();
                        db.BeamSignature = beamData.BeamSignature;
                        db.InitialBeamCode = beamData.InitialBeamCode;
                        db.TotalLength = beamData.Length;
                        db.TotalXXLength = beamData.XXLength;
                        group.GroupBy(t => t.FloorName).ToList().ForEach(floorGroup =>
                        {
                            var floorName = floorGroup.Key;
                            var floorCount = floorGroup.Count();
                            db.Remark += $"{floorName}:{floorCount}根;";
                        });
                        _beamHuiZongs.Add(db);
                        //listView1.Items.Add(item);
                    }
                });
            }
        }


        private void initTable()
        {
            table1.Columns= new AntdUI.ColumnCollection
            {
            new AntdUI.Column("ProjectName", "工事名"),
                                new AntdUI.Column("Biaochi", "縮尺"),
                                new AntdUI.Column("BeamsNumber", "梁数"),
                                new AntdUI.Column("KPillarCount", "K 柱 トータル"),
                                new AntdUI.Column("SinglePillarCount", "単柱"),
                                new AntdUI.Column("CustomerName", "顧客名"),
                                new AntdUI.Column("BCount", "B"),
                                new AntdUI.Column("BGCount", "BG"),
                                new AntdUI.Column("FloorQuantity", "階数"),
                                new AntdUI.Column("ParsedQuantity", "解析数")

            };

        }

        private void ShowAllBeams_Load(object sender, EventArgs e)
        {
            table1.DataSource = _beamHuiZongs;

        }
    }
}
