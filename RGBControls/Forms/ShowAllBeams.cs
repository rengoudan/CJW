using JwCore;
using JwServices;
using JwShapeCommon;
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
            InitializeComponent();
            initTable();
            // 2026年11月21日 原来在构造函数里"触发不等待"地加载数据，表格可能在数据到达前就已绑定空列表。
            // 改为在 ShowAllBeams_Load 中 await 加载完成后再绑定。
        }

        private async Task createData()
        {
            _beamHuiZongs = new List<JwBeamHuiZong>();
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
                        db.BeamDatas = group.ToList();
                        _beamHuiZongs.Add(db);
                        //listView1.Items.Add(item);
                    }
                });
            }
        }


        private void initTable()
        {
            table1.Columns = new AntdUI.ColumnCollection
            {
            new AntdUI.Column("InitialBeamCode", "初期化梁番号"),
                                new AntdUI.Column("BeamCode", "梁番号"),
                                new AntdUI.Column("BeamCount", "梁数"),
                                new AntdUI.Column("TotalLength", "梁長さ"),
                                new AntdUI.Column("TotalXXLength", "梁中心長さ"),
                                new AntdUI.Column("Remark", "詳細"),

            };
            table2.Columns = new AntdUI.ColumnCollection
            {
            new AntdUI.Column("BeamCode", "梁番号"),
                                new AntdUI.Column("FloorName", "階"),
                                new AntdUI.Column("GongQu", "工区"),


            };
        }

        private async void ShowAllBeams_Load(object sender, EventArgs e)
        {
            await createData();
            table1.DataSource = _beamHuiZongs;
        }

        private void table1_SelectIndexChanged(object sender, EventArgs e)
        {
            if (table1.SelectedIndex > 0)
            {
                var hz = table1[table1.SelectedIndex - 1].record as JwBeamHuiZong;
                if (hz.BeamDatas?.Count > 0)
                {
                    table2.DataSource = hz.BeamDatas;
                    showBeam(hz.BeamDatas.FirstOrDefault());
                }
                    

            }
            else
            {
                table2.DataSource = null;
            }
        }
        SingleBeamShow _singleBeamShow;
        private async void showBeam(JwBeamData jwBeam)
        {
            await JwProjectMainService.LoadBeamCollectionAsync(jwBeam);
            JwBeam beam=jwBeam.BeamDataToJwBeam();
            this.panel3.Controls.Remove(_singleBeamShow);
            var lst = beam.DrawToJwwwSingle();
            _singleBeamShow = new SingleBeamShow(lst);
            _singleBeamShow.Dock = DockStyle.Fill;
            this.panel3.Controls.Add(_singleBeamShow);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
