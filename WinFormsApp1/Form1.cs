using JwData;
using JwShapeCommon;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public List<string> list = new List<string>();

        public List<JwXian> jwXians = new List<JwXian>();

        public List<JWPoint> points = new List<JWPoint>();

        public List<string> xianids = new List<string>();

        public List<JwXian> tempyx = new List<JwXian>();

        public List<JWMian> jWMians = new List<JWMian>();

        public List<string> tempid = new List<string>();

        public string parseLineType = "lc105";

        public SettingObject settingS;

        public ResultDto<List<JwProjectDto>> resultDto;

        public Form1()
        {
            settingS = new SettingObject();
            resultDto = getProjectData();
            InitializeComponent();
        }
        bool jsmingcheng = false;
        private async void button1_Click(object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //伏 図
            //軸組図
            string fileLocation = string.Format(@"{0}\JWC_TEMP.TXT", Application.StartupPath);
            string bb = comboBox1.SelectedItem.ToString();

            if (File.Exists(fileLocation))
            {
                string[] neirong = await File.ReadAllLinesAsync(fileLocation, System.Text.Encoding.GetEncoding("Shift-JIS"));
                List<string> lstneirong = neirong.ToList();
                lstneirong.ForEach(t =>
                {
                    textBox2.AppendText(t);
                    textBox2.AppendText("\r\n");
                });
                if (lstneirong.Count(t => t.Contains(bb)) > 0)
                {
                    jsmingcheng = true;
                    string qb = lstneirong.Find(t => t.Contains(bb));
                    var qbb = qb.Substring(qb.IndexOf("\"") + 1);
                    this.BeginInvoke(new Action(() =>
                    {
                        textBox1.Text = qbb;
                    }));
                }
                try
                {
                    var request = new RestRequest("/api/File/Upload", Method.Post);
                    request.AddFile("file", fileLocation);

                    //RestClient restClient = new RestClient("http://192.168.3.59:5000/");
                    RestClient restClient = new RestClient("https://localhost:44301/");
                    //calling server with restClient

                    RestResponse response = await restClient.ExecuteAsync(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var z = JsonConvert.DeserializeObject<ResultDto<string>>(response.Content);
                        jwParseSub.UploadToken = z.Result;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("出错啦:{0}", ex.Message));
                }
            }
            else
            {
                MessageBox.Show("PATH IS WRONG");
            }
        }

        public string tianchong { get; set; }
        private void button2_Click(object sender, EventArgs e)
        {
            //if (jsmingcheng)
            //{

            //}
            //else
            //{
            //    Mingcheng mingcheng = new Mingcheng();
            //    if (mingcheng.ShowDialog(this) == DialogResult.OK)
            //    {
            //        if (string.IsNullOrEmpty(tianchong))
            //        {
            //            textBox1.Text = tianchong;
            //        }
            //    }
            //}
            JwLine jwLine = new JwLine {
                Id = 1,
                Location = new NetTopologySuite.Geometries.Point(0, 0)
            };

            using (var db=new JwDataContext())
            {
                db.Add(jwLine);
                db.SaveChanges();
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //伏 図
            //軸組図
            string fileLocation = string.Format(@"{0}\JWC_TEMP.TXT", Application.StartupPath);
            //string bb = comboBox1.SelectedItem.ToString();
            DialogResult result = MessageBox.Show("if change parse setting", "", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                ParseSetting parse = new ParseSetting();
                if (parse.ShowDialog(this) == DialogResult.OK)
                {
                    parseBeams();
                }
            }
            else
            {
                parseBeams();
            }

        }

        public JwParseSub jwParseSub = new JwParseSub();

        public List<string> SlList = new List<string>();
        public List<string> CHlst = new List<string>();

        private void parseBeams()
        {
            string fileLocation = string.Format(@"{0}\JWC_TEMP.TXT", Application.StartupPath);
            //string bb = comboBox1.SelectedItem.ToString();
            if (File.Exists(fileLocation))
            {
                String line;
                StringBuilder sb = new StringBuilder();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                try
                {
                    string[] neirong = File.ReadAllLines(fileLocation, System.Text.Encoding.GetEncoding("Shift-JIS"));
                    //JwTempHelper jwTemp = new JwTempHelper(neirong, settingS);
                    jwParseSub.init(neirong);
                    jwParseSub.CreateBeam();
                    dataGridView1.DataSource = jwParseSub.Mians;

                    //jwParseSub.PareBeamByMian();

                    var z = jwParseSub.Beams;
                    var shuiping = jwParseSub.HorizontalBeams;
                    var cuizhi=jwParseSub.VerticalBeams;
                    JwBeamDeepParse deepParse=new JwBeamDeepParse(jwParseSub.Beams);

                    var ee = deepParse.HorizontalBeams;

                    //dataGridView1.data
                    // jwParseSub.ProjectId = Convert.ToInt64(comboBox2.SelectedValue.ToString());
                    //if (zq.Count > 0)
                    //{

                    //    jwParseSub.SubName = textBox1.Text;
                    //    //postProjectData();
                    //}
                    //if (CHlst.Count > 0)
                    //{
                    //    foreach (var slstr in CHlst)
                    //    {
                    //        jwParseSub.Tags.Add(new JwTagg(slstr));
                    //    }
                    //}
                    //if (jwParseSub.Tags.Count > 0)
                    //{
                    //    foreach (var t in jwParseSub.Tags)
                    //    {
                    //        t.SelectOwnPillar(jwParseSub.Pillars);
                    //    }
                    //}
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                MessageBox.Show("PATH IS WRONG");
            }
        }

        private void parseBeam()
        {
            string fileLocation = string.Format(@"{0}\JWC_TEMP.TXT", Application.StartupPath);
            string bb = comboBox1.SelectedItem.ToString();
            if (File.Exists(fileLocation))
            {
                String line;
                StringBuilder sb = new StringBuilder();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                try
                {
                    StreamReader sr = new StreamReader(fileLocation, Encoding.GetEncoding("Shift-JIS"));
                    //Read the first line of text
                    int qc = 0;
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        string pattern3 = @"^[A-Za-z!~~`#$%^&*()_+-=]";
                        bool z = Regex.IsMatch(line, pattern3);
                        if (!z)
                        {
                            sb.AppendLine(line);
                            list.Add(line);
                        }
                    }
                    bool islc105 = false;
                    //Continue to read until you reach end of file
                    while (line != null)
                    {
                        qc++;
                        //write the line to console window
                        //Console.WriteLine(line);
                        //Read the next line
                        line = sr.ReadLine();
                        if (line != null)
                        {
                            if (!islc105 && Regex.IsMatch(line, settingS.ParseColor))
                            {
                                islc105 = true;
                            }
                            else
                            {
                                string pattern3 = @"^[A-Za-z!~~`#$%^&*()_+-=]";
                                bool z = Regex.IsMatch(line, pattern3);
                                if (!z && islc105)
                                {
                                    sb.AppendLine(line);
                                    list.Add(line);
                                }
                                else
                                {
                                    if (Regex.IsMatch(line, "lt"))
                                    {

                                    }
                                    else
                                    {
                                        islc105 = false;
                                    }
                                }
                                if (Regex.IsMatch(line, @"^sl.*"))
                                {
                                    SlList.Add(line);
                                }
                                if (Regex.IsMatch(line, @"^ch.*"))
                                {
                                    CHlst.Add(line);
                                }
                            }
                        }
                    }
                    int bbbb = qc;
                    //close the file
                    sr.Close();
                    list = list.Distinct().ToList();
                    //sb.AppendLine(line);
                    //textBox1.Text = sb.ToString();
                    foreach (string z in list)
                    {
                        string z1 = z.TrimStart().TrimEnd();
                        JwXian j = new JwXian(z1);
                        jwXians.Add(j);
                        points.AddRange(j.GetXianPoints());
                    }
                    int ii = list.Count;
                    int i = jwXians.Count;

                    xianids = jwXians.Select(t => t.Id).ToList();

                    //Point 
                    foreach (var xian in jwXians)
                    {
                        if (!xian.IsSelected)
                        {
                            if (!tempid.Contains(xian.Id))
                            {
                                JWMian mian = new JWMian();
                                xian.IsSelected = true;
                                mian.Xians = new List<JwXian>
                    {
                        xian
                    };
                                jWMians.Add(mian);
                                Getmian(xian, mian);
                            }

                        }
                    }
                    foreach (var m in jWMians)
                    {
                        m.XianCout = m.Xians.Count;
                    }
                    var zq = jWMians.Where(t => t.XianCout == 4).ToList();
                    if(zq.Count>0)
                    {
                        var fm = zq.First();
                        var lst = fm.Xians.GetLinesPoints();
                        
                        var z = lst.GetLeftTopRightBottom();
                    }

                    if (SlList.Count > 0)
                    {
                        foreach(var slstr in SlList)
                        {
                            jwParseSub.Blocks.Add(new JwBlock(slstr));
                        }

                        if(jwParseSub.Blocks.Count > 0)
                        {
                            jwParseSub.ParseTriangularBlocks();
                            jwParseSub.ParseSquareCreatePillar();
                            
                        }
                        if(jwParseSub.Pillars.Count > 0)
                        {
                            foreach (var item in jwParseSub.Pillars)
                            {
                                item.squareParse();
                            }
                        }
                    }

                    dataGridView1.DataSource = zq;
                    //dataGridView1.data
                    jwParseSub.ProjectId = Convert.ToInt64(comboBox2.SelectedValue.ToString());
                    if (zq.Count > 0)
                    {
                        jwParseSub.Mians = zq;
                        jwParseSub.SubName = textBox1.Text;
                        //postProjectData();
                    }
                    if(CHlst.Count > 0)
                    {
                        foreach (var slstr in CHlst)
                        {
                            jwParseSub.Tags.Add(new JwTagg(slstr));
                        }
                    }
                    if(jwParseSub.Tags.Count> 0)
                    {
                        foreach(var t in jwParseSub.Tags)
                        {
                            t.SelectOwnPillar(jwParseSub.Pillars);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                MessageBox.Show("PATH IS WRONG");
            }
        }
        public void Getmian(JwXian xian, JWMian mian)
        {
            tempid.Add(xian.Id);
            var z = xianids.Except(tempid).ToList();
            foreach (var x in z)
            {
                var obj = jwXians.FirstOrDefault(t => t.Id == x);
                if(!object.ReferenceEquals(obj, null))
                //if (obj != null)
                {
                    if (xian.Isxiangjiao(obj))
                    {
                        if (!obj.IsSelected)
                        {
                            obj.IsSelected = true;
                            mian.Xians.Add(obj);
                            Getmian(obj, mian);
                        }
                    }
                }

            }
        }



        private ResultDto<List<JwProjectDto>> getProjectData()
        {
            //RestClient restClient = new RestClient("http://192.168.3.59:5000/");
            RestClient restClient = new RestClient("https://localhost:44301/");
            var request = new RestRequest("/api/services/app/JwProjects/GetProjects", Method.Get);
            RestResponse response = restClient.Execute(request);
            //ResultDto<JwProjectDto> dto = new ResultDto<JwProjectDto>(gou);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                //dto.Success = false;
                //MessageBox.Show("httpstatuscode fail");
                return null;
            }
            else
            {
                var dto = JsonConvert.DeserializeObject<ResultDto<List<JwProjectDto>>>(response.Content);

                dto.JsonArg = JsonConvert.SerializeObject(dto.Result);
                if (dto.Success)
                {
                    return dto;
                }
                else
                {
                    return null;
                }
                //MessageBox.Show("getszzhapi return fail");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (resultDto != null)
            {
                comboBox2.DataSource = resultDto.Result;
                comboBox2.ValueMember = "Id";
                comboBox2.DisplayMember = "ProjectName";
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            //jwParseSub.ProjectId = Convert.ToInt64(comboBox2.SelectedValue.ToString());

        }

        private void postProjectData()
        {
            RestClient restClient = new RestClient("http://192.168.3.59:5000/");
            //RestClient restClient = new RestClient("https://localhost:44301/");
            var request = new RestRequest("/api/services/app/JwProjectSubs/CreateProjectsub", Method.Post);
            request.AddJsonBody(jwParseSub);
            RestResponse response = restClient.Execute(request);
            //ResultDto<JwProjectDto> dto = new ResultDto<JwProjectDto>(gou);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                //dto.Success = false;
                MessageBox.Show("httpstatuscode fail");

            }
            else
            {
                MessageBox.Show("success");
                //var dto = JsonConvert.DeserializeObject<ResultDto<List<JwProjectDto>>>(response.Content);

                //dto.JsonArg = JsonConvert.SerializeObject(dto.Result);
                //if (dto.Success)
                //{

                //}
                //else
                //{

                //}
                //MessageBox.Show("getszzhapi return fail");
            }

        }
    }
}