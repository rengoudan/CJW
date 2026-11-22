using JwCore;
using JwServices;
using RGBControls.Controls;
using RGBJWMain.Forms;
using RGBJWMain.Pages;
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
    public partial class SubsForm : AntdUI.Window
    {

        private JwProjectMainService JwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        public SubsForm()
        {
            InitializeComponent();
        }

        private JwProjectMainData _mainData;
        public SubsForm(JwProjectMainData mainData)
        {
            InitializeComponent();
            _mainData = mainData;
        }



        private void SubsForm_Load(object sender, EventArgs e)
        {
            if (_mainData != null)
            {
                var tl = string.Format("{0}JW設計分析作業台", _mainData.ProjectName);
                this.pageHeader1.Text = tl;
                var page = new AntdUI.TabPage()
                {
                    Name = "main",
                    Text = _mainData.ProjectName
                };
                var poform = new ProjectOverview(_mainData);
                poform.TopLevel = false;
                poform.FormBorderStyle = FormBorderStyle.None;
                poform.Dock = DockStyle.Fill;
                page.Controls.Add(poform);

                tabs1.Pages.Add(page);
                poform.Show();
                //var z = new TabPage();
                //z.Controls.Add(new ProjectOverview());
                initload();
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void initload()
        {
            var initialTabs = new List<(string, string, Func<UserControl>)>();
            foreach (var s in _mainData.JwProjectSubDatas)
            {
                initialTabs.Add((s.Id, s.FloorName, () => new Sub(s)));
            }
            InitializeTabs(initialTabs);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="tabDefinitions"></param>
        public void InitializeTabs(IEnumerable<(string Key, string Title, Func<UserControl> Factory)> tabDefinitions)
        {
            foreach (var (key, title, factory) in tabDefinitions)
            {
                var control = factory.Invoke();
                control.Dock = DockStyle.Fill;

                var page = new AntdUI.TabPage()
                {
                    Name = key,
                    Text = title
                };
                page.Controls.Add(control);
                tabs1.Pages.Add(page);
            }

            if (tabs1.Pages.Count > 0)
            {
                tabs1.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            if (_mainData != null)
            {
                if (_mainData.ProjectStatus == ProjectStatus.Budget)
                {
                    string emsg = string.Format("{0}には予算が生成されているためアップロードできません", _mainData.ProjectName);
                    UIMessageBox.ShowError(emsg);
                    return;
                }
                // 2026年11月22日 工事の階数分を取り込み済みなら、それ以上は取り込めない
                var blocked = await JwProjectMainService.CheckImportAsync(_mainData.Id);
                if (blocked != null)
                {
                    UIMessageBox.ShowWarning(blocked);
                    return;
                }
                var f = new OpenFileDialog();
                f.Filter = "Jww Files|*.jww|Jws Files|*.jws|All Files|*.*";
                f.Multiselect = true; // 2026年11月22日 複数の階をまとめて取り込める
                if (f.ShowDialog() != DialogResult.OK) return;
                // 2026年11月22日 簡易確認・複数ファイルに対応した共通の流れ（Classes/FloorImportFlow.cs）
                int saved = await RGBControls.Classes.FloorImportFlow.ImportFilesAsync(this, _mainData, f.FileNames);
                if (saved > 0) AddNewFloorTabs();
            }
        }

        /// <summary>
        /// 2026年11月22日 取り込んだ階のタブだけを追加する。
        /// 原来调用 initload()，会把已有楼层的标签页再加一遍（重复）。
        /// </summary>
        private void AddNewFloorTabs()
        {
            var existing = new HashSet<string>();
            foreach (var p in tabs1.Pages) if (p.Name != null) existing.Add(p.Name);
            var added = _mainData.JwProjectSubDatas
                .Where(s => !existing.Contains(s.Id))
                .Select(s => (s.Id, s.FloorName ?? "", (Func<UserControl>)(() => new Sub(s))))
                .ToList();
            if (added.Count > 0) InitializeTabs(added);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

        }

        void OpenFile(String path, JwProjectMainData data)
        {
            try
            {
                if (Path.GetExtension(path).ToLower() == ".jww")
                {
                    JwProjectPathModel model = new JwProjectPathModel();
                    model.Path = path;
                    model.MainData = data;


                    CustomerDesignSetting jfsf = new CustomerDesignSetting(model);
                    if (jfsf.ShowDialog() == DialogResult.OK)
                    {
                        FileParseForm parseForm = new FileParseForm(model);
                        if (parseForm.ShowDialog() == DialogResult.OK)
                        {
                            initload();
                        }
                        //var s = model.FloorName;
                    }
                    //JwwReaderが読み込み用のクラス。
                    //using var reader = new JwwHelper.JwwReader();
                    ////Completedは読み込み完了時に実行される関数。
                    //reader.Read(path, Completed);
                    //var a = reader.Header.m_jwwDataVersion;

                }
                else if (Path.GetExtension(path) == ".jws")
                {
                    UIMessageBox.ShowError("JWSを処理できません");
                    ////jwsも読めますが、このプロジェクトでは確認用のコードがありません。
                    //using var a = new JwwHelper.JwsReader();
                    //a.Read(path, Completed2);
                }
            }
            catch (Exception exception)
            {
                RGBControls.Errors.ErrorReporter.Report(exception, "ファイルの処理中にエラーが発生しました。");
            }
        }
    }

    public class TabManager
    {
        private readonly TabControl _tabControl;

        public TabManager(TabControl tabControl)
        {
            _tabControl = tabControl ?? throw new ArgumentNullException(nameof(tabControl));
        }

        public void LoadTab(string tabKey, string tabTitle, Func<UserControl> controlFactory)
        {
            foreach (TabPage page in _tabControl.TabPages)
            {
                if (page.Name == tabKey)
                {
                    _tabControl.SelectedTab = page;
                    return;
                }
            }

            var control = controlFactory.Invoke();
            control.Dock = DockStyle.Fill;

            var newPage = new TabPage(tabTitle)
            {
                Name = tabKey
            };
            newPage.Controls.Add(control);

            _tabControl.TabPages.Add(newPage);
            _tabControl.SelectedTab = newPage;
        }

        public void InitializeTabs(IEnumerable<(string Key, string Title, Func<UserControl> Factory)> tabDefinitions)
        {
            foreach (var (key, title, factory) in tabDefinitions)
            {
                var control = factory.Invoke();
                control.Dock = DockStyle.Fill;

                var page = new TabPage(title)
                {
                    Name = key
                };
                page.Controls.Add(control);
                _tabControl.TabPages.Add(page);
            }

            if (_tabControl.TabPages.Count > 0)
            {
                _tabControl.SelectedIndex = 0;
            }
        }
    }

}
