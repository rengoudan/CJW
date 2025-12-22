using JwCore;
using JwShapeCommon;
using RGBJWMain.Forms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Controls
{
    public partial class JwShowBeams : Control
    {
        public JwShowBeams()
        {
            _startpoint = new Point(10, 10);
            this.BackColor = Color.Black;
            InitializeComponent();
            GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent += controlSelectedEvent;
            this.MouseWheel += JwShowBeams_MouseWheel;
            this.MouseClick += JwShowBeams_MouseClick;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
       

        private void controlSelectedEvent(object sender, ControlSelectedSquareArgs e)
        {
            if (e.IsLianjie)
            {
                if(CanvasDraw != null && CanvasDraw.LianjieLines != null)
                {
                    var lianjies = CanvasDraw.LianjieLines;
                    if (lianjies.Count > 0)
                    {
                        foreach (var lj in lianjies)
                        {
                            if(lj.Id == e.Id&&lj.IsSelected)
                            {
                                
                            }
                            else
                            {
                                lj.IsSelected = false;
                                if (lj.Id==e.Id)
                                {
                                    lj.IsSelected = true;
                                }
                                Invalidate();
                            }
                        }
                        //Invalidate();
                    }
                    
                }
                
            }
            else
            {
                if (e.DrawShapeType == DrawShapeType.LinkPart)
                {
                    if(_canvasDraw != null)
                    {
                        if(_canvasDraw.links != null)
                        {
                            if (_canvasDraw.links.Count > 0)
                            {
                                foreach (var link in _canvasDraw.links)
                                {
                                    if (link.LinkPart.Id == e.Id)
                                    {
                                        link.IsSelected = true;
                                    }
                                    else
                                    {
                                        link.IsSelected = false;
                                    }
                                    Invalidate();
                                }
                            }
                        }
                        
                    }
                    
                }
                else
                {
                    if (HasCreated && _bounds != null)
                    {
                        if (_bounds.Count(t => t.ShapeType == e.DrawShapeType) > 0)
                        {
                            var fwlst = _bounds.Where(t => t.ShapeType == e.DrawShapeType).ToList();
                            foreach (var t in fwlst)
                            {
                                if (t.JwSquareBase.Id == e.Id && t.IsSelected)
                                {

                                }
                                else
                                {
                                    t.IsSelected = false;
                                    if (t.JwSquareBase.Id == e.Id)
                                    {
                                        t.IsSelected = true;
                                    }
                                    Invalidate();
                                }
                            }
                        }
                    }
                }
                
            }
        }

        private List<ControlDraw> _bounds;

        private JwCanvasDraw _canvasDraw;

        private bool HasCreated;

        public JwCanvasDraw CanvasDraw
        {
            get { return _canvasDraw; }
            set
            {
                _canvasDraw = value;
                if(_canvasDraw != null)
                {
                    _canvasDraw.Draw(Width, Height, 40, 40);
                    //_jwDrawShape.Draw();
                    if (_canvasDraw.controls != null)
                    {
                        HasCreated = true;  
                        _bounds = _canvasDraw.controls;
                        Invalidate();
                    }
                }
            }
        }

        public bool IsNewCanvas = false;

        private bool _showBeams;
        public bool ShowBeams
        {
            get
            {
                return _showBeams;
            }
            set
            {
                _showBeams = value;
                Invalidate();
            }
        }

        private bool _showmsg;
        public bool Showmsg
        {
            get
            {
                return _showmsg;
            }
            set
            {
                _showmsg = value;
                Invalidate();
            }
        }

        private bool _showPillar;
        public bool ShowPillar
        {
            get
            {
                return _showPillar;
            }
            set
            {
                _showPillar = value;
                Invalidate();
            }
        }

        private bool _showGoujian;
        public bool ShowGoujian
        {
            get { return _showGoujian; }

            set
            {
                _showGoujian = value;
                Invalidate();
            }
        }

        private bool _showGoujiantext;
        public bool ShowGoujiantext
        {
            get { return _showGoujiantext; }
            set
            {
                _showGoujiantext = value;
                Invalidate();
            }
        }

        private bool _showDownB;
        public bool ShowDownB
        {
            get => _showDownB;
            set
            {
                _showDownB = value;
                Invalidate() ;
            }
        }

        private bool _showFuzhu;
        public bool ShowFuzhu
        {
            get=> _showFuzhu;
            set
            { 
                _showFuzhu = value;
                Invalidate();
            }
        }

        public bool HasItems { get; set; }

        private Point _centerpoint;

        private double _minbeilv;
        private bool HasMinBeilv = false;   

        private string _changejwitemerror = "";

        private Point _startpoint;

        private double axisX = 0;
        private double axisY = 0;

        private bool _canCreatebeams = false;

        public JwBeam SelectedBeam { get; set; }

        public JwPillar SelectPillar { get; set; }

        /// <summary>
        /// 通用 同时使用pillar 和beam
        /// </summary>
        public ControlDraw? SelectedSquare { get;set; }


        Pen pens = new Pen(new SolidBrush(Color.DarkSeaGreen), 1);//线条的粗细

        /// <summary>
        /// 选中的线条颜色
        /// </summary>
        Pen penselected = new Pen(new SolidBrush(Color.OrangeRed), 2);//线条的粗细

        Pen pensr = new Pen(new SolidBrush(Color.Red), 1);//线条的粗细

        Pen penx = new Pen(new SolidBrush(Color.Gray), 0.5f);//线条的粗细

        Pen penlj = new Pen(new SolidBrush(Color.Purple), 0.5f);//线条的粗细

        Pen penljselected = new Pen(new SolidBrush(Color.White), 1f);//线条的粗细

        Pen penjt = new Pen(new SolidBrush(Color.Green), 0.5f);//线条的粗细

        int fontSize = 8;
        Font biaozhuFont;
        Font subdataFont;
        Font labelFont;
        Brush bushred = new SolidBrush(Color.Red);
        Brush bushwhite = new SolidBrush(Color.White);

        public void CreateBeams()
        {
            if(_canCreatebeams) 
            {
                pens.DashStyle = DashStyle.Solid;//线条的线型
                penx.DashStyle = DashStyle.DashDot;//线条的线型
                penjt.DashStyle = DashStyle.Dot;//线条的线型
                penjt.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                penjt.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                Invalidate();
            }
        }
        public void init()
        {
            _showPillar = true;
            _showBeams = true;
            _showFuzhu = true;
            _showGoujian = true;
        }

        private void drawControls(PaintEventArgs pe)
        {
            if(_bounds != null)
            {
                if(_showBeams)
                {
                    foreach (var b in _bounds)
                    {
                        if (b.ShapeType == DrawShapeType.Beam)
                        {
                            if (!b.IsTeshuBeam)
                            {
                                //pe.Graphics.DrawPolygon()
                                if (b.IsSelected)
                                {
                                    //Brush bush = new SolidBrush(Color.Red);
                                    //pe.Graphics.FillRectangle(bush, b.DrawRectangleF.Location.X, b.DrawRectangleF.Location.Y, b.DrawRectangleF.Width, b.DrawRectangleF.Height);
                                    pe.Graphics.DrawRectangle(penselected, b.DrawRectangleF.Location.X, b.DrawRectangleF.Location.Y, b.DrawRectangleF.Width, b.DrawRectangleF.Height);
                                }
                                else
                                {
                                    pe.Graphics.DrawRectangle(pens, b.DrawRectangleF.Location.X, b.DrawRectangleF.Location.Y, b.DrawRectangleF.Width, b.DrawRectangleF.Height);
                                }
                            }
                            else
                            {
                                if (b.IsSelected)
                                {
                                    pe.Graphics.DrawPolygon(penselected, b.DrawPoints.ToArray());
                                }
                                else
                                {
                                    pe.Graphics.DrawPolygon(pens, b.DrawPoints.ToArray());
                                }
                            }

                        }
                        


                        //pe.Graphics.DrawRectangle(pens, (int)beam.TopLeft.X, (int)beam.TopLeft.Y, (int)beam.Width, (int)beam.Height);
                    }
                }
                if(_showPillar)
                {
                    foreach (var b in _bounds)
                    {
                        if (b.ShapeType == DrawShapeType.Pillar)
                        {
                            if(b.IsSelected)
                            {
                                Brush bush = new SolidBrush(Color.AliceBlue);
                                pe.Graphics.FillRectangle(bush, b.DrawRectangleF.Location.X, b.DrawRectangleF.Location.Y, b.DrawRectangleF.Width, b.DrawRectangleF.Height);
                            }
                            else
                            {
                                Brush bush = new SolidBrush(Color.Yellow);
                                pe.Graphics.FillRectangle(bush, b.DrawRectangleF.Location.X, b.DrawRectangleF.Location.Y, b.DrawRectangleF.Width, b.DrawRectangleF.Height);
                            }
                           
                        }
                    }
                }
                
            }
            if (CanvasDraw != null)
            {

                if(CanvasDraw.Texts.Count > 0)
                {
                    if (_showmsg)
                    {
                        foreach (var text in CanvasDraw.Texts)
                        {
                            if (text.DirectionType == BeamDirectionType.Horizontal)
                            {
                                var stext = pe.Graphics.MeasureString(text.Text, this.Font);
                                var nl = new PointF(text.Location.X - stext.Width / 2, text.Location.Y);
                                pe.Graphics.DrawString(text.Text, this.Font, Brushes.White, nl);
                            }
                            else
                            {
                                StringFormat sf = new StringFormat();
                                sf.FormatFlags = StringFormatFlags.DirectionVertical;
                                pe.Graphics.DrawString(text.Text, this.Font, Brushes.White, text.Location, sf);
                            }
                        }
                    }
                }
                if (_showFuzhu)
                {
                    if (CanvasDraw.FuzhuXs.Count > 0)
                    {
                        foreach (var x in CanvasDraw.FuzhuXs)
                        {
                            pe.Graphics.DrawLine(penx, x, 0, x, Height);//箭头 s 点和 pr点
                        }
                    }
                    if (CanvasDraw.FuzhuYs.Count > 0)
                    {
                        foreach (var y in CanvasDraw.FuzhuYs)
                        {
                            pe.Graphics.DrawLine(penx, 0, y, Width, y);//箭头 s 点和 pr点
                        }
                    }
                    
                }
                if(_showGoujian)
                {
                    if (CanvasDraw.links.Count > 0)
                    {
                        int iindex = 1;
                        int bindex = 1;
                        int bgindex = 1;
                        foreach (var link in CanvasDraw.links)
                        {
                            var txtx = link.Bounds.First().Location.X;
                            var txty= link.Bounds.First().Location.Y;
                            string showtxt = "";
                            if (link.LinkPart.GouJianType == GouJianType.B)
                            {
                                showtxt = string.Format("{0}{1}", link.LinkPart.GouJianType.ToString(), bindex);
                                bindex++;
                            }
                            else
                            {
                                showtxt = string.Format("{0}{1}", link.LinkPart.GouJianType.ToString(), bgindex);
                                bgindex++;
                            }
                            if (link.LinkPart.Directed == TaggDirect.Up)
                            {
                                txty -= 18;
                            }
                            if(link.LinkPart.Directed == TaggDirect.Down)
                            {
                                txty += 18;
                            }
                            if (link.LinkPart.Directed == TaggDirect.Right)
                            {
                                txtx += 18;

                            }
                            if (link.LinkPart.Directed == TaggDirect.Left)
                            {
                                txtx -= 18;
                            }
                            Brush bush = new SolidBrush(Color.Red);

                            Brush bushselected = new SolidBrush(Color.Green);
                            foreach (var b in link.Bounds)
                            {
                                Brush sqwhite = new SolidBrush(Color.White);
                                if (link.LinkPart.IsDownPillar)
                                {
                                    if (_showDownB)
                                    {
                                        pe.Graphics.FillRectangle(sqwhite, b.Location.X, b.Location.Y, b.Width, b.Height);
                                    }
                                }
                                else
                                {
                                    if (link.IsSelected)
                                    {
                                        pe.Graphics.FillRectangle(bushselected, b.Location.X, b.Location.Y, b.Width, b.Height);
                                    }
                                    else
                                    {
                                        pe.Graphics.FillRectangle(bush, b.Location.X, b.Location.Y, b.Width, b.Height);
                                    }
                                    
                                }
                                
                                
                            }
                            if(_showGoujiantext)
                            {
                                using var font = new Font("Arial", 12, FontStyle.Regular);
                                pe.Graphics.DrawString(showtxt, font, Color.Yellow, txtx, txty);
                            }
                            
                            // pe.Graphics.FillPolygon(bush, link.Polygon.ToArray());//箭头 s 点和 pr点
                        }
                    }
                }
                if(CanvasDraw.LianjieLines.Count > 0)
                {
                    foreach(var ljl in CanvasDraw.LianjieLines)
                    {
                        if (ljl.IsSelected)
                        {
                                pe.Graphics.DrawLine(penljselected, ljl.DrawStart, ljl.DrawEnd);
                        }
                        else
                        {
                            pe.Graphics.DrawLine(penlj, ljl.DrawStart, ljl.DrawEnd);
                        }
                    }
                }
                
            }
            
            
        }

        protected override void OnAutoSizeChanged(EventArgs e)
        {
            base.OnAutoSizeChanged(e);
            Invalidate();
        }

        public override void Refresh()
        {
            base.Refresh();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.TranslateTransform(origin.X, origin.Y);
            pe.Graphics.ScaleTransform(scale, scale);
            drawControls(pe);
        }

        private bool HasMouseDownPoint =false;

        private Point MouseDownPoint;

        public bool IsBeamSelected = false; 
        bool nowisleftdown = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            nowisleftdown = false;
            if (e.Button != MouseButtons.Left)
            {
                //
                //nowisleftdown = true;
                return;
            }
            else
            {
                nowisleftdown = true;
            }
                MouseDownPoint = new Point(e.X, e.Y);
            HasMouseDownPoint = true;
        }

        public bool IsSelectedOneBeam = false;

        protected override void OnClick(EventArgs e)
        {
            
            if(HasCreated&& nowisleftdown)
            {
                var z = MouseDownPoint;


                float logicX = (MouseDownPoint.X - origin.X) / scale;
                float logicY = (MouseDownPoint.Y - origin.Y) / scale;
                PointF logicPoint = new PointF(logicX, logicY);


                bool re = false;
                IsSelectedOneBeam = false;
                var beamsbs = _bounds.Where(t => t.ShapeType == DrawShapeType.Beam).ToList();
                foreach (var beam in beamsbs)
                {
                    if (beam.DrawRectangleF.Contains(logicPoint))
                    {
                        //beam.NeedReDraw = true;
                        beam.IsNeed = !beam.IsNeed;
                        IsBeamSelected = beam.IsNeed;
                        if (beam.IsNeed)
                        {
                            SelectedBeam = beam.JwSquareBase as JwBeam;
                            NewJwBeamForm jsForm = new NewJwBeamForm(SelectedBeam);
                            jsForm.IsNewBeam = IsNewCanvas;
                            jsForm.Show();
                        }
                        else
                        {
                            SelectedBeam = null;
                        }
                        IsSelectedOneBeam = true;
                        re = true;
                        break;
                    }

                }
                if (re)
                {
                    Invalidate();

                }
            }
            base.OnClick(e);
        }
        bool candeletebeam = false;
        bool candeletepillar = false;   
        //AntdUI.IContextMenuStripItem[] menulist = { };
        List<AntdUI.IContextMenuStripItem> menulist = new List<AntdUI.IContextMenuStripItem>();
        ControlDraw _selectcontroldraw;
        LinkDrawModel _selectedlinkpart;
        bool candeletelinkpart = false;
        private void JwShowBeams_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menulist.Clear();
                float logicX = (e.X - origin.X) / scale;
                float logicY = (e.Y - origin.Y) / scale;
                PointF logicPoint = new PointF(logicX, logicY);
                bool re = false;
                IsSelectedOneBeam = false;
                var beamsbs = _bounds.Where(t => t.ShapeType == DrawShapeType.Beam).ToList();
                var bj= beamsbs.FirstOrDefault(t => t.DrawRectangleF.Contains(logicPoint));
                if(bj != null)
                {
                    bj.IsNeed = true;
                    _selectcontroldraw = bj;
                    SelectedBeam = bj.JwSquareBase as JwBeam;
                    candeletebeam = true;
                    //AntdUI.ContextMenuStrip.open(this, it =>
                    //{
                    //    RightKey(it);
                    //}, menulist);
                    menulist.Add(new AntdUI.ContextMenuStripItem("消去-梁"));
                }
                var pillarsbs = _bounds.Where(t => t.ShapeType == DrawShapeType.Pillar).ToList();
                if (pillarsbs.Count > 0)
                {
                    var pl= pillarsbs.FirstOrDefault(t => t.DrawRectangleF.Contains(logicPoint));
                    if (pl != null)
                    {
                        pl.IsNeed = true;
                        _selectcontroldraw = pl;
                        candeletepillar = true;
                        SelectPillar = pl.ParentSquareBase as JwPillar;
                        //AntdUI.ContextMenuStrip.open(this, it =>
                        //{
                        //    RightKey(it);
                        //}, menulist);
                        menulist.Add(new AntdUI.ContextMenuStripItem("消去-柱"));
                    }
                }
                
                if(CanvasDraw.links.Count > 0)
                {
                    var lp= CanvasDraw.links.FirstOrDefault(t => t.VirtualArea.Contains(logicPoint));
                    if (lp != null)
                    {
                        //lp.IsNeed = true;
                        //_selectcontroldraw = lp;
                        //AntdUI.ContextMenuStrip.open(this, it =>
                        //{
                        //    RightKey(it);
                        //}, menulist);
                        _selectedlinkpart= lp;
                        candeletelinkpart = true;
                        menulist.Add(new AntdUI.ContextMenuStripItem("消去-B/BG"));
                    }
                }


                if (candeletebeam|| candeletepillar)
                {
                    AntdUI.ContextMenuStrip.open(this, it =>
                    {
                        RightKey(it);
                    }, menulist.ToArray());
                }
                // 重置缩放和位置
                //AntdUI.ContextMenuStrip.Config config = new AntdUI.ContextMenuStrip.Config(this, RightKey, menulist);
                //config.Font = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold);
                //AntdUI.ContextMenuStrip.open(config);

            }
        }
        private void RightKey(AntdUI.ContextMenuStripItem it)
        {
            if (it.Text.Equals("消去-梁"))
            {

                if(GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent!=null)
                {
                    if(_selectcontroldraw != null)
                    {
                        _bounds.Remove(_selectcontroldraw);
                        Invalidate();
                        
                    }
                    if (candeletebeam && SelectedBeam != null)
                    {
                        ControlSelectedSquareArgs args = new ControlSelectedSquareArgs();
                        args.DrawShapeType = DrawShapeType.Beam;
                        args.Id = SelectedBeam.Id;
                        args.IsLianjie = false;
                        GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent(this, args);
                    }
                }
                // 执行对应的操作
                //MessageBox.Show("你点击了新增按钮！");
            }
            if (it.Text.Equals("消去-柱"))
            {
                if (GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent != null)
                {
                    if (_selectcontroldraw != null)
                    {
                        _bounds.Remove(_selectcontroldraw);
                        Invalidate();

                    }
                    if (candeletebeam && SelectedBeam != null)
                    {
                        ControlSelectedSquareArgs args = new ControlSelectedSquareArgs();
                        args.DrawShapeType = DrawShapeType.Pillar;
                        args.Id = SelectPillar.Id;
                        args.IsLianjie = false;
                        GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent(this, args);
                    }
                }
            }
            if (it.Text.Equals("消去-B/BG"))
            {
                if (GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent != null)
                {
                    if (_selectedlinkpart != null)
                    {
                        CanvasDraw.links.Remove(_selectedlinkpart);
                        //_bounds.Remove(_selectcontroldraw);
                        Invalidate();

                    }
                    if (candeletebeam && SelectedBeam != null)
                    {
                        ControlSelectedSquareArgs args = new ControlSelectedSquareArgs();
                        args.DrawShapeType = DrawShapeType.LinkPart;
                        args.Id = _selectedlinkpart.LinkPart.Id;
                        args.IsLianjie = false;
                        GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent(this, args);
                    }
                }
            }
            if (it.Text.Equals("删除"))
            {

            }
            if (it.Text.Equals("刷新"))
            {

            }
            if (it.Text.Equals("重置"))
            {

            }
        }

        private float scale = 1.0f;

        private PointF origin = new PointF(0, 0);

        // 添加缺失的 JwShowBeams_MouseWheel 事件处理方法
        private void JwShowBeams_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                // 可以根据需要实现缩放或其他功能，这里暂时留空
                // 例如：Invalidate();
                float oldScale = scale;
                scale += e.Delta > 0 ? 0.1f : -0.1f;
                scale = Math.Max(1f, scale); // 防止缩得太小
                                             // 以鼠标位置为中心进行缩放
                origin.X = e.X - (e.X - origin.X) * (scale / oldScale);
                origin.Y = e.Y - (e.Y - origin.Y) * (scale / oldScale);
                Invalidate();
            }
            else
            {
                if (GlobalEvent.GetGlobalEvent().WarningEvent != null)
                {
                    WarningArgs args = new WarningArgs();
                    args.WarningMsg = "Ctrlキーを押しながらマウスホイールをスクロールして、ズームインまたはズームアウトします。";
                    GlobalEvent.GetGlobalEvent().WarningEvent(this, args);
                }
            }
        }
    }
}
