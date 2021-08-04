using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Timers;
using Microsoft.Win32;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using CsvHelper;

namespace UVStudio
{
    public partial class SpectrumScan : Form
    {
        public string photometric_mode, speed_measure;
        public int loop_measure;
        public string step_interval, time_interval;
        public string start_wl, cancel_wl;


        private int dgvcnt = 0;
        private List<SpectraScan> currlist = new List<SpectraScan>();
        private ShowParams showpar = (ShowParams)null;
        private MeasureParams mpar = (MeasureParams)null;
        private PrintParams printpar = (PrintParams)null;
        private List<XYRange> xyRangeList = new List<XYRange>();
        private int ZoomOut = 0;
        private int SquareMode = 0;
        private int ThreeDMode = 0;
        private List<SpectraScan> dlist = new List<SpectraScan>();
        private int dinter = 0;
        private int Peak = 0;
        private int valley = 0;
        private List<MeaureData> peakList = new List<MeaureData>();
        private List<MeaureData> valleyList = new List<MeaureData>();
        private List<MeaureData> selectlist = new List<MeaureData>();
        private List<MeaureData> flist = new List<MeaureData>();
        private float curx;
        private int ybx;
        private SerialPort sp = new SerialPort();
        private ComStatus ComSta;
        private Thread dealth;
        private bool runptag = true;
        private string lanvalue;
        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");
        private string conacc = CommonFun.GetAcc("ceAccuracy");
        public List<string> rightlist = new List<string>();
        private bool autoscale = false;
        private string scanwlpar = "";
        private string scanmosptpar = "";
        private Thread tdstart;

        private int tickcnt = 0;
        private int calormea = 1;
        private string slotno = "";
        private int currslotno = 0;
        private int stophappend = 0;
        private Point start;
        private int downmove = 0;
        private float left;
        private float right;
        private float top;
        private float bottom;
        private new float FontSize = 16F;
        private double xInt;
        private double yInt;
        private float xMax;
        private float xMin;
        private float yMax;
        private float yMin;
        private float xs;
        private float xe;
        private float ys;
        private float ye;
        private SpectraScan sslive = new SpectraScan();
        private Queue myque = new Queue();
        private int meacnt = 0;

        private DataGridViewImageColumn ColSign;
        private DataGridViewTextBoxColumn ColWL;
        private DataGridViewTextBoxColumn ColXGD;

        private DataGridViewImageColumn ColPSign;
        private DataGridViewTextBoxColumn ColPWL;
        private DataGridViewTextBoxColumn ColPXGD;

        private DataGridViewTextBoxColumn ColNo;
        private DataGridViewImageColumn ColDel;
        private DataGridViewTextBoxColumn ColColor;
        private DataGridViewTextBoxColumn ColBW;
        private DataGridViewTextBoxColumn ColEW;
        private DataGridViewTextBoxColumn ColSquare;



        public SpectrumScan()
        {
            InitializeComponent();

            this.picCurve.Visible = false;

            ColSign = new DataGridViewImageColumn();
            ColSign.Name = "ColSign";
            ColSign.HeaderText = "";
            ColSign.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //ColSign.FillWeight = 60.9137f;
            //ComponentResourceManager.ApplyResources((object)this.ColSign, "ColSign");
            this.ColSign.ReadOnly = true;
            ColWL = new DataGridViewTextBoxColumn();
            ColWL.Name = "ColWL";
            ColWL.HeaderText = "λ";
            ColWL.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColWL.FillWeight = 119.54732f;
            this.ColWL.ReadOnly = true;
            ColXGD = new DataGridViewTextBoxColumn();
            ColXGD.Name = "ColXGD";
            ColXGD.HeaderText = "Abs";
            ColXGD.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColXGD.FillWeight = 119.54732f;
            this.ColXGD.ReadOnly = true;

            ColPSign = new DataGridViewImageColumn();
            ColPSign.Name = "ColPSign";
            ColPSign.HeaderText = "";
            ColPSign.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColPSign.FillWeight = 60.9137f;
            this.ColPSign.ReadOnly = true;
            ColPWL = new DataGridViewTextBoxColumn();
            ColPWL.Name = "ColPWL";
            ColPWL.HeaderText = "λ";
            ColPWL.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColPWL.FillWeight = 119.54732f;
            this.ColPWL.ReadOnly = true;
            ColPXGD = new DataGridViewTextBoxColumn();
            ColPXGD.Name = "ColPXGD";
            ColPXGD.HeaderText = "Abs";
            ColPXGD.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColPXGD.FillWeight = 119.54732f;
            this.ColPXGD.ReadOnly = true;

            ColNo = new DataGridViewTextBoxColumn();
            ColNo.Name = "ColNo";
            ColNo.HeaderText = "№";
            ColNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColNo.FillWeight = 15f;
            this.ColNo.ReadOnly = true;
            ColDel = new DataGridViewImageColumn();
            ColDel.Name = "ColDel";
            ColDel.HeaderText = "Удалить";
            ColDel.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColDel.FillWeight = 15f;
            this.ColDel.ReadOnly = true;
            ColColor = new DataGridViewTextBoxColumn();
            ColColor.Name = "ColColor";
            ColColor.HeaderText = "Цвет";
            ColColor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColColor.FillWeight = 13f;
            this.ColColor.ReadOnly = true;
            ColBW = new DataGridViewTextBoxColumn();
            ColBW.Name = "ColBW";
            ColBW.HeaderText = "Начальная λ";
            ColBW.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColBW.FillWeight = 24f;
            this.ColBW.ReadOnly = true;
            ColEW = new DataGridViewTextBoxColumn();
            ColEW.Name = "ColEW";
            ColEW.HeaderText = "Конечная λ";
            ColEW.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColEW.FillWeight = 24f;
            ColEW.ReadOnly = true;
            ColSquare = new DataGridViewTextBoxColumn();
            ColSquare.Name = "ColSquare";
            ColSquare.HeaderText = "Площадь";
            ColSquare.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColSquare.FillWeight = 15f;
            ColSquare.ReadOnly = true;

            this.dgvPoint.Columns.AddRange((DataGridViewColumn)this.ColPSign, (DataGridViewColumn)this.ColPWL, (DataGridViewColumn)this.ColPXGD);

            this.dataGridView1.Columns.AddRange((DataGridViewColumn)this.ColSign, (DataGridViewColumn)this.ColWL, (DataGridViewColumn)this.ColXGD);
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView2.Columns.AddRange((DataGridViewColumn)this.ColNo, (DataGridViewColumn)this.ColDel, (DataGridViewColumn)this.ColColor, (DataGridViewColumn)this.ColBW, (DataGridViewColumn)this.ColEW, (DataGridViewColumn)this.ColSquare);
            this.dgvcnt = this.dataGridView1.Height / 50 - 1;
            this.dataGridView1.Columns[0].DefaultCellStyle.NullValue = (object)null;
            this.dgvPoint.Columns[0].DefaultCellStyle.NullValue = (object)null;
            this.dataGridView2.Columns[1].DefaultCellStyle.NullValue = (object)null;
            this.dataGridView1.Rows.Add(this.dgvcnt);
            //this.dgvPoint.Rows.Add(this.dgvcnt);
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dgvPoint.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dgvPoint.AllowUserToAddRows = false;
            this.dataGridView1.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView2.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dgvPoint.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dgvPoint.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.picTop.BackColor = Color.Transparent;
            this.picTop.Parent = (Control)this.picCurve;
            this.dataGridView1.ColumnHeadersHeight = 50;
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView2.ColumnHeadersHeight = 50;
            this.dataGridView2.RowTemplate.Height = 50;
            this.dgvPoint.ColumnHeadersHeight = 50;
            this.dgvPoint.RowTemplate.Height = 50;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;

            this.dgvPoint.Columns[0].Width = 1;
            this.dataGridView1.Columns[0].Width = 1;
            this.dataGridView2.Columns[0].Width = 1;
            //this.lblPeak.Image = Image.FromFile("img/Peak.png");
            //this.lblPeak.BackgroundImage("img/Peak.png");

            ///   this.lblSetXY.BackgroundImage = Properties.Resources.Default;
            //    this.lblShowXY.BackgroundImage = Properties.Resources.InputXY;
            //    this.lblAutoXY.BackgroundImage = Properties.Resources.Auto;
            //    this.lblChose.BackgroundImage = Properties.Resources.Add;
            //   this.lblPeak.BackgroundImage = Properties.Resources.Peak;
            //  this.lblDel.BackgroundImage = Properties.Resources.Remove;
            //    this.lblZoomOut.BackgroundImage = Properties.Resources.Zoom;
            //   this.lblValley.BackgroundImage = Properties.Resources.Valley;
            //   this.lblLast.BackgroundImage = Properties.Resources.OPCur_back;
            //   this.lblNext.BackgroundImage = Properties.Resources.OPCur_next;
            //   this.lblRight.BackgroundImage = Properties.Resources.SearchNext;
            //  this.lblLeft.BackgroundImage = Properties.Resources.SearchBack;

            picTop.MouseDown += new MouseEventHandler(this.pibTop_MouseDown);
            picTop.MouseMove += new MouseEventHandler(this.pibTop_MouseMove);
            picTop.MouseUp += new MouseEventHandler(this.pibTop_MouseUp);

            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView2.EnableHeadersVisualStyles = false;
            this.dgvPoint.EnableHeadersVisualStyles = false;

            if (this.mpar == null)
            {
                this.mpar = new MeasureParams();
                try
                {
                    this.mpar.C_BeginWL = CommonFun.getXmlValue("MeasureParams", "C_BeginWL");
                    this.mpar.C_EndWL = CommonFun.getXmlValue("MeasureParams", "C_EndWL");
                    this.mpar.C_Interval = CommonFun.getXmlValue("MeasureParams", "C_Interval");
                    this.mpar.C_Intervals = CommonFun.getXmlValue("MeasureParams", "C_Intervals");
                    this.mpar.C_Mode = CommonFun.getXmlValue("MeasureParams", "C_Mode");
                    this.mpar.C_Mode = !(this.mpar.C_Mode == "Abs") ? CommonFun.GetLanText("T") : CommonFun.GetLanText("Abs");
                    this.mpar.C_StepLen = CommonFun.getXmlValue("MeasureParams", "C_StepLen");
                    this.mpar.C_ScanCNT = CommonFun.getXmlValue("MeasureParams", "C_ScanCNT");
                    this.mpar.C_ScanSpeedDM = CommonFun.getXmlValue("MeasureParams", "C_ScanSpeed");
                    this.mpar.C_ScanSpeed = CommonFun.GetLanText(this.mpar.C_ScanSpeedDM);
                    this.mpar.C_SLength = CommonFun.getXmlValue("MeasureParams", "C_SLength");
                    if (this.mpar.C_BeginWL == "")
                        this.mpar.C_BeginWL = "1100.0";
                    if (this.mpar.C_EndWL == "")
                        this.mpar.C_EndWL = "190.0";
                    if (this.mpar.C_Interval == "")
                        this.mpar.C_Interval = "0";
                    if (this.mpar.C_Intervals == "")
                        this.mpar.C_Intervals = "0";
                    if (this.mpar.C_StepLen == "")
                        this.mpar.C_StepLen = "1.0";
                    if (this.mpar.C_ScanCNT == "")
                        this.mpar.C_ScanCNT = "1";
                    if (this.mpar.C_ScanSpeed == "")
                    {
                        this.mpar.C_ScanSpeedDM = "Средне";
                        this.mpar.C_ScanSpeed = this.mpar.C_ScanSpeedDM;
                    }
                    if (this.mpar.C_SLength == "")
                        this.mpar.C_SLength = "10";
                }
                catch
                {
                    this.mpar.C_BeginWL = "1100.0";
                    this.mpar.C_EndWL = "190.0";
                    /*this.mpar.C_Interval = "10";
                    this.mpar.C_Intervals = "10";*/
                    this.mpar.C_Mode = CommonFun.GetLanText("Abs");
                    this.mpar.C_StepLen = "1.0";
                    this.mpar.C_ScanCNT = "1";
                    this.mpar.C_ScanSpeedDM = "Средне";
                    this.mpar.C_ScanSpeed = CommonFun.GetLanText(this.mpar.C_ScanSpeedDM);
                    this.mpar.C_SLength = "10";
                }
            }
            if (this.showpar == null)
            {
                this.showpar = new ShowParams();
                try
                {
                    this.showpar.AutoPrint = Convert.ToBoolean(CommonFun.getXmlValue("ShowParams", "AutoPrint"));
                    this.showpar.AutoSave = Convert.ToBoolean(CommonFun.getXmlValue("ShowParams", "AutoSave"));
                    this.showpar.AutoXY = Convert.ToBoolean(CommonFun.getXmlValue("ShowParams", "AutoXY"));
                    this.autoscale = this.showpar.AutoXY;
                    this.showpar.MulShow = Convert.ToInt32(CommonFun.getXmlValue("ShowParams", "MulShow"));
                    /*if (this.showpar.MulShow == 1)
                        this.showmode_image.Source = new BitmapImage(new Uri("img/Icon_DispAll.png"));
                    else
                        this.showmode_image.Source = new BitmapImage(new Uri("img/Icon_DispCurrent"));*/
                    this.showpar.xMax = (float)Convert.ToDouble(CommonFun.getXmlValue("ShowParams", "xMax"));
                    this.showpar.xMin = (float)Convert.ToDouble(CommonFun.getXmlValue("ShowParams", "xMin"));
                    this.showpar.yMax = (float)Convert.ToDouble(CommonFun.getXmlValue("ShowParams", "yMax"));
                    this.showpar.yMin = (float)Convert.ToDouble(CommonFun.getXmlValue("ShowParams", "yMin"));
                }
                catch
                {
                    this.showpar.AutoPrint = false;
                    this.showpar.AutoSave = false;
                    this.showpar.AutoXY = true;
                    this.autoscale = this.showpar.AutoXY;
                    this.showpar.MulShow = 0;
                    //this.showmode_image.Source = new BitmapImage(new Uri("img/Resources.Icon_DispCurrent"));
                    this.showpar.xMax = 1100f;
                    this.showpar.xMin = 190f;
                    this.showpar.yMax = 0.0001f;
                    this.showpar.yMin = -0.0001f;
                }
            }
            if (this.printpar == null)
            {
                this.printpar = new PrintParams();
                try
                {
                    this.printpar.Addtional = CommonFun.getXmlValue("PrintParams", "Addtional");
                    this.printpar.ComImage = CommonFun.getXmlValue("PrintParams", "ComImage");
                    this.printpar.Describtion = CommonFun.getXmlValue("PrintParams", "Describtion");
                    this.printpar.ShowAddtional = Convert.ToBoolean(CommonFun.getXmlValue("PrintParams", "ShowAddtional"));
                    this.printpar.ShowAllData = Convert.ToBoolean(CommonFun.getXmlValue("PrintParams", "ShowAllData"));
                    this.printpar.ShowComImage = Convert.ToBoolean(CommonFun.getXmlValue("PrintParams", "ShowComImage"));
                    this.printpar.ShowDes = Convert.ToBoolean(CommonFun.getXmlValue("PrintParams", "ShowDes"));
                    this.printpar.ShowInsAndUser = Convert.ToBoolean(CommonFun.getXmlValue("PrintParams", "ShowInsAndUser"));
                    this.printpar.ShowPeak = Convert.ToBoolean(CommonFun.getXmlValue("PrintParams", "ShowPeak"));
                    this.printpar.ShowValley = Convert.ToBoolean(CommonFun.getXmlValue("PrintParams", "ShowValley"));
                    this.printpar.Title = CommonFun.getXmlValue("PrintParams", "Title");
                }
                catch
                {
                    this.printpar.Addtional = "";
                    this.printpar.ComImage = "";
                    this.printpar.Describtion = "";
                    this.printpar.ShowAddtional = false;
                    this.printpar.ShowAllData = true;
                    this.printpar.ShowComImage = false;
                    this.printpar.ShowDes = false;
                    this.printpar.ShowInsAndUser = true;
                    this.printpar.ShowPeak = false;
                    this.printpar.ShowValley = false;
                    this.printpar.Title = "Title";
                }
                this.lblmode.Text = this.mpar.C_Mode;
            }
            /*DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = this.mpar.C_Mode;
            textColumn.Binding = new Binding("Binding C_Mode");*/
            //textColumn.Binding = new Binding("{Binding list}");
            // meisureGrid.Columns.Add(textColumn);
            //this.dataGridView1.Columns[0].HeaderText = "WL";
            this.dataGridView1.Columns[2].HeaderText = this.mpar.C_Mode;
            this.dataGridView1.Refresh();
            this.lblmode.Text = this.mpar.C_Mode;
            //      this.DrawLine();
            //   this.meisureGrid.Items.Refresh();
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("rightspemeasure"))
                    this.btnScan.Enabled = true;
                else
                    this.btnScan.Enabled = false;
                if (this.rightlist.Contains("rightspeblank"))
                    this.btnBlank.Enabled = true;
                else
                    this.btnBlank.Enabled = false;
                if (this.rightlist.Contains("rightspefindpv"))
                {
                    //  this.lblPeak.Enabled = true;
                    //   this.lblValley.Enabled = true;
                }
                else
                {
                    //   this.lblPeak.Enabled = false;
                    //   this.lblValley.Enabled = false;
                }
                /* if (this.rightlist.Contains("rightspeselectpoint"))
                     this.lblChose.Enabled = true;
                 else
                     this.lblChose.Enabled = false;*/
                /*if (CommonFun.GetAppConfig("LockSystem") == "true")
                    this.btnlock.Visible = true;
                else
                    this.btnlock.Visible = false;*/
            }
            /* else
                 this.btnlock.Visible = false;*/
            this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
            this.tdstart.Start();
            this.setstate(false);
            if (CommonFun.GetAppConfig("currentconnect") == "-1")
                this.btnBack.Enabled = true;
            ResetTempReDraw(0);
        }
        private void tt_Tick(object sender, EventArgs e)
        {
            ++this.tickcnt;
            if (this.tickcnt != 60)
                return;
            this.ComSta = ComStatus.END;
            this.tickcnt = 0;
            if (this.btnBlank.InvokeRequired)
                this.btnBlank.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
            else
                this.setstate(true);
        }
        private void setstate(bool status)
        {
            // this.btnBack.Enabled = status;
            if (sp.IsOpen == true)
            {
                this.btnBlank.Enabled = status;
                this.btnScan.Enabled = status;
                if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                {
                    if (this.rightlist.Contains("rightspemeasure") && status)
                        this.btnScan.Enabled = true;
                    else
                        this.btnScan.Enabled = false;
                    if (this.rightlist.Contains("rightspeblank") && status)
                        this.btnBlank.Enabled = true;
                    else
                        this.btnBlank.Enabled = false;
                    if (this.rightlist.Contains("rightspefindpv"))
                    {
                        //   this.lblPeak.Enabled = true;
                        //     this.lblValley.Enabled = true;
                    }
                    else
                    {
                        //      this.lblPeak.Enabled = false;
                        //       this.lblValley.Enabled = false;
                    }
                    //   if (this.rightlist.Contains("rightspeselectpoint"))
                    //    this.lblChose.Enabled = true;
                    // else
                    //     this.lblChose.Enabled = false;
                    /*if (CommonFun.GetAppConfig("LockSystem") == "true")
                        this.btnlock.Visible = true;
                    else
                        this.btnlock.Visible = false;*/
                }
                /*else
                    this.btnlock.Visible = false;*/
            }
        }

        private void tdstart_Elapsed()
        {
            if (!(CommonFun.GetAppConfig("currentconnect") != "-1"))
                return;
            this.dealth = new Thread(new ThreadStart(this.DealRecData));
            this.dealth.Start();
            try
            {
                this.sp.BaudRate = 38400;
                this.sp.PortName = "COM2";
                this.sp.DataBits = 8;
                this.sp.StopBits = StopBits.One;
                this.sp.Parity = Parity.None;
                this.sp.ReadTimeout = -1;
                this.sp.Open();
                this.sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);
                //sp.WriteLine("CONNECT \r\n");
                this.ComSta = ComStatus.BD_RATIO_FLUSH;
                this.sp.WriteLine("BD_RATIO_FLUSH \r\n");
                CommonFun.WriteSendLine("BD_RATIO_FLUSH");
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new Del_starttt(this.Starttt), (object)true);
                else
                    this.Starttt(true);
                this.scanwlpar = (Convert.ToDecimal(this.mpar.C_BeginWL) * 10M).ToString("f0") + " " + (Convert.ToDecimal(this.mpar.C_EndWL) * 10M).ToString("f0") + " " + (Convert.ToDecimal(this.mpar.C_StepLen) * 10M).ToString("f0");
                this.scanmosptpar = (!(this.mpar.C_ScanSpeed == "Быстро") ? (!(this.mpar.C_ScanSpeed == "Средне") ? (!(this.mpar.C_ScanSpeed == "Медленно") ? 4 : 3) : 2) : 1).ToString();
            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.ToString(), "Error");
                this.ComSta = ComStatus.END;
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
                else
                    this.setstate(true);
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new Del_starttt(this.Starttt), (object)false);
                else
                    this.Starttt(false);
            }
        }

        private void Starttt(bool status)
        {
            if (status)
            {
                this.tt.Enabled = true;
                this.tt.Start();
            }
            else
            {
                this.tt.Stop();
                this.tickcnt = 0;
            }
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (this.sp.IsOpen)
            {
                switch (this.ComSta)
                {
                    case ComStatus.Connect:
                    case ComStatus.SYSTATE:
                    case ComStatus.GETWL:
                    case ComStatus.SETCHP:
                    case ComStatus.SETSCANWL:
                    case ComStatus.SETMOSPD:
                    case ComStatus.MEASURE:
                    case ComStatus.CALBGND:
                    case ComStatus.SCANBASE:
                    case ComStatus.BD_RATIO_FLUSH:
                        this.myque.Enqueue((object)this.sp.ReadLine());
                        break;
                }
            }
            else
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
        }


        private void DealRecData()
        {
            try
            {
                while (this.runptag)
                {
                    if (this.myque.Count > 0)
                    {
                        string text1 = this.myque.Dequeue().ToString();
                        CommonFun.WriteLine(text1);
                        try
                        {
                            switch (this.ComSta)
                            {
                                case ComStatus.SETCHP:
                                    try
                                    {
                                        if (text1.Contains("*A#"))
                                        {
                                            this.ComSta = ComStatus.END;
                                            string[] strArray1 = new string[7];
                                            strArray1[0] = "SETSCANWL ";
                                            string[] strArray2 = strArray1;
                                            System.Decimal num = Convert.ToDecimal(this.mpar.C_BeginWL) * 10M;
                                            string str1 = num.ToString("f0");
                                            strArray2[1] = str1;
                                            strArray1[2] = " ";
                                            string[] strArray3 = strArray1;
                                            num = Convert.ToDecimal(this.mpar.C_EndWL) * 10M;
                                            string str2 = num.ToString("f0");
                                            strArray3[3] = str2;
                                            strArray1[4] = " ";
                                            string[] strArray4 = strArray1;
                                            num = Convert.ToDecimal(this.mpar.C_StepLen) * 10M;
                                            string str3 = num.ToString("f0");
                                            strArray4[5] = str3;
                                            strArray1[6] = "\r\n";
                                            string text2 = string.Concat(strArray1);
                                            this.ComSta = ComStatus.SETSCANWL;
                                            this.sp.WriteLine(text2);
                                            CommonFun.WriteSendLine(text2);
                                            break;
                                        }
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("errorretry") + ex.ToString(), "Error");
                                        this.ComSta = ComStatus.END;
                                        break;
                                    }
                                case ComStatus.SETSCANWL:
                                    if (text1.Contains("*A# " + this.scanwlpar))
                                    {
                                        int num = !(this.mpar.C_ScanSpeed == "Быстро") ? (!(this.mpar.C_ScanSpeed == "Средне") ? (!(this.mpar.C_ScanSpeed == "Медленно") ? 4 : 3) : 2) : 1;
                                        this.ComSta = ComStatus.SETMOSPD;
                                        this.sp.WriteLine("SETMOSPD " + num.ToString() + " \r\n");
                                        CommonFun.WriteSendLine("SETMOSPD " + num.ToString());
                                        break;
                                    }
                                    break;
                                case ComStatus.SETMOSPD:
                                    if (text1.Contains("*A# " + this.scanmosptpar))
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (this.calormea == 1)
                                        {
                                            this.ComSta = ComStatus.CALBGND;
                                            this.sp.WriteLine("scan_calbgnd 1 2\r\n");
                                            CommonFun.WriteSendLine("scan_calbgnd 1 2");
                                        }
                                        else
                                        {
                                            this.ComSta = ComStatus.MEASURE;
                                            this.sp.WriteLine("scan_measure 1 3\r\n");
                                            CommonFun.WriteSendLine("scan_measure 1 3");
                                            ++this.meacnt;
                                            ++this.currslotno;
                                        }
                                        break;
                                    }
                                    break;
                                case ComStatus.MEASURE:
                                    try
                                    {
                                        if (text1.Contains("*A# 1 3"))
                                        {
                                            this.sslive = new SpectraScan();
                                            this.sslive.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                                            this.sslive.Serials = CommonFun.GetAppConfig("serialno");
                                            this.sslive.MethodPar = this.mpar;
                                            this.sslive.C_name = this.getName(new OperateType?(), "");
                                            this.sslive.C_Operator = CommonFun.GetAppConfig("currentuser");
                                            this.sslive.color = this.GetColor();
                                            this.sslive.D_Time = DateTime.Now;
                                            this.sslive.Data = new List<MeaureData>();
                                            this.sslive.status = false;
                                            if (this.currlist.Count > 0)
                                            {
                                                this.sslive.IsShow = 1;
                                                this.currlist.Add(this.sslive);
                                                for (int index = this.currlist.Count - 1; index > 0; --index)
                                                {
                                                    this.currlist[index] = this.currlist[index - 1];
                                                    if (this.showpar.MulShow == 0)
                                                        this.currlist[index].IsShow = 0;
                                                }
                                                this.currlist[0] = this.sslive;
                                            }
                                            else
                                            {
                                                this.currlist.Add(this.sslive);
                                                this.sslive.IsShow = 1;
                                            }
                                            if (this.lblSample.InvokeRequired)
                                                this.lblSample.Invoke((Delegate)new Del_SamNameSet(this.SetSname));
                                            else
                                                this.lblSample.Text = this.sslive.C_name;
                                        }
                                        if (text1.Contains("*DAT"))
                                        {
                                            string str1 = text1;
                                            if (this.currlist.Count > 0)
                                            {
                                                if (this.currlist[0].Data == null)
                                                    this.currlist[0].Data = new List<MeaureData>();
                                            }
                                            else
                                            {
                                                this.sslive = new SpectraScan();
                                                this.sslive.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                                                this.sslive.Serials = CommonFun.GetAppConfig("serialno");
                                                this.sslive.MethodPar = this.mpar;
                                                this.sslive.C_name = this.getName(new OperateType?(), "");
                                                this.sslive.C_Operator = CommonFun.GetAppConfig("currentuser");
                                                this.sslive.color = this.GetColor();
                                                this.sslive.D_Time = DateTime.Now;
                                                this.sslive.Data = new List<MeaureData>();
                                                this.sslive.status = false;
                                                this.sslive.IsShow = 1;
                                                this.currlist.Add(this.sslive);
                                            }
                                            int startIndex = str1.IndexOf("*DAT") + 4;
                                            string input = str1.Substring(startIndex, str1.Length - startIndex);
                                            if (input.Contains("END"))
                                                input = input.Substring(0, input.Length - 4);
                                            string[] strArray = new Regex("DAT").Split(input);
                                            for (int index = 0; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                                            {
                                                MeaureData md = new MeaureData();
                                                md.xValue = (Convert.ToDecimal(strArray[index].Split(' ')[3]) / 10M).ToString("f1");
                                                if (Convert.ToDecimal(md.xValue) <= Convert.ToDecimal(this.mpar.C_BeginWL) && Convert.ToDecimal(md.xValue) >= Convert.ToDecimal(this.mpar.C_EndWL))
                                                {
                                                    md.YT = (float)Convert.ToDouble(strArray[index].Split(' ')[1]);
                                                    md.yABS = (double)md.YT > 0.0 ? (float)(2.0 - Math.Log10(Convert.ToDouble(md.YT))) : 10f;
                                                    // if (this.mpar.C_SLength != "10")
                                                    if (this.mpar.C_SLength != null && this.mpar.C_SLength != "10")
                                                    {
                                                        md.yABS *= (float)(Convert.ToDouble(10) / Convert.ToDouble(this.mpar.C_SLength));
                                                        md.YT = (float)Math.Pow(10.0, 2.0 - (double)md.yABS);
                                                    }
                                                    if ((Convert.ToDecimal(md.xValue) <= 195M || Convert.ToDecimal(md.xValue) >= 1050M) && (double)Math.Abs(md.yABS) <= 0.002)
                                                    {
                                                        md.yABS /= 3f;
                                                        md.YT = (float)Math.Pow(10.0, 2.0 - (double)md.yABS);
                                                    }
                                                    this.currlist[0].Data.Add(md);
                                                    if (this.dataGridView1.InvokeRequired)
                                                        this.dataGridView1.Invoke((Delegate)new Del_BindData(this.BindDataWhileScan), (object)md);
                                                    else
                                                        this.BindDataWhileScan(md);
                                                    if (this.lblWL.InvokeRequired)
                                                    {
                                                        string str2 = !(this.mpar.C_Mode == CommonFun.GetLanText("Abs")) ? md.YT.ToString(this.tacc) : md.yABS.ToString(this.absacc);
                                                        this.lblWL.Invoke((Delegate)new Del_SetWL(this.SetWL), (object)md.xValue, (object)str2);
                                                    }
                                                    else
                                                        this.lblWL.Text = md.xValue + " nm";
                                                    System.Decimal num = Convert.ToDecimal(this.mpar.C_EndWL) - Convert.ToDecimal(this.mpar.C_BeginWL);
                                                    int int32 = Convert.ToInt32((Convert.ToDecimal(md.xValue) - Convert.ToDecimal(this.mpar.C_BeginWL)) * 100M / num);
                                                    if (this.progressBar1.InvokeRequired)
                                                        this.Invoke((Delegate)new SetPos(this.SetTextMessage), (object)int32);
                                                    else
                                                        this.progressBar1.Value = Convert.ToInt32(int32);
                                                }
                                            }
                                            if (this.currlist[0].Data.Count % 50 == 0)
                                            {
                                                this.currlist[0].Data.OrderBy<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue));
                                                if (this.picCurve.InvokeRequired)
                                                {
                                                    this.picCurve.Invoke((Delegate)new Del_MeaData(this.DealMeaData));
                                                }
                                                else
                                                {
                                                    this.XYMaxMin();
                                                    if (this.showpar.AutoXY)
                                                        this.AddToRangeList(Convert.ToSingle(this.mpar.C_EndWL), Convert.ToSingle(this.mpar.C_BeginWL), this.yMin, this.yMax);
                                                    else
                                                        this.AddToRangeList(this.showpar.xMin, this.showpar.xMax, this.showpar.yMin, this.showpar.yMax);
                                                }
                                            }
                                        }
                                        if (text1.Contains("END"))
                                        {
                                            if (this.stophappend > 0)
                                            {
                                                this.meacnt = 0;
                                                this.currslotno = 0;
                                                this.slotno = "";
                                                this.calormea = 0;
                                                this.ComSta = ComStatus.END;
                                                this.stophappend = 0;
                                                if (this.btnScan.InvokeRequired)
                                                {
                                                    this.btnScan.Invoke((Delegate)new Del_SetStop(this.SetStop));
                                                }
                                                else
                                                {
                                                    this.btnScan.Text = CommonFun.GetLanText("measure");
                                                    this.setState(ComStatus.END);
                                                }
                                            }
                                            else
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.currlist[0].Data.OrderBy<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue));
                                                if (this.picCurve.InvokeRequired)
                                                    this.picCurve.Invoke((Delegate)new Del_MeaData(this.DealMeaData));
                                                else
                                                    this.DealMeaData();
                                                if (this.lblSample.InvokeRequired)
                                                    this.lblSample.Invoke((Delegate)new Del_SamNameSet(this.SetSname));
                                                else
                                                    this.lblSample.Text = this.currlist[0].C_name;
                                                if (this.showpar.AutoPrint)
                                                {
                                                    /*if (this.picCurve.InvokeRequired)
                                                        this.picCurve.Invoke((Delegate)new Del_AutoPrint(this.AutoPrint));
                                                    else
                                                        this.AutoPrint();*/
                                                }
                                                if (this.meacnt < (int)Convert.ToInt16(this.mpar.C_ScanCNT, new CultureInfo("en-US")))
                                                {
                                                    if (this.mpar.C_Interval != "")
                                                        Thread.Sleep((int)Convert.ToInt16(this.mpar.C_Interval, new CultureInfo("en-US")) * 1000);
                                                    this.ComSta = ComStatus.MEASURE;
                                                    this.sp.WriteLine("scan_measure 1 3\r\n");
                                                    CommonFun.WriteSendLine("scan_measure 1 3 again");
                                                    ++this.meacnt;
                                                }
                                                else
                                                {
                                                    int num1;
                                                    if (this.slotno.Length > 0)
                                                        num1 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                    else
                                                        num1 = 1;
                                                    if (num1 == 0)
                                                    {
                                                        this.meacnt = 0;
                                                        this.ComSta = ComStatus.SETCHP;
                                                        this.sp.WriteLine("SETCHP " + this.slotno.Split(',')[this.currslotno] + "\r\n");
                                                        CommonFun.WriteSendLine("SETCHP " + this.slotno.Split(',')[this.currslotno]);
                                                    }
                                                    else
                                                    {
                                                        CommonFun.showbox("Измерение завершено", "Information");

                                                        /*for(int i = 0; i < MyTableList.Count(); i++)
                                                        {
                                                            this.dataGridView1.Rows.Clear();
                                                            this.dataGridView1.Columns[2].HeaderText = this.mpar.C_Mode;
                                                            dataGridView1.Rows.Add();
                                                            this.dataGridView1.Rows[i].Cells["ColWL"].Value = MyTableList[i].WL;
                                                            this.dataGridView1.Rows[i].Cells["ColXGD"].Value = !(this.mpar.C_Mode == "Abs") ? MyTableList[i].TProcent : MyTableList[i].Abs;
                                                            this.dataGridView1.Rows[i].Tag = MyTableList[i];
                                                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[i].Cells[0];
                                                        }*/
                                                        /*for (int i = 0; i < currlist[0].Data.Count; i++)
                                                        {
                                                            this.dataGridView1.Rows.Add();
                                                            this.dataGridView1.Rows[i].Cells["ColWL"].Value = (object)currlist[0].Data[i].xValue;
                                                            this.dataGridView1.Rows[i].Cells["ColXGD"].Value = !(this.mpar.C_Mode == "Abs") ? (object)currlist[0].Data[i].YT.ToString(this.tacc) : (object)currlist[0].Data[i].yABS.ToString(this.absacc);
                                                            this.dataGridView1.Rows[i].Tag = (object)currlist[0].Data[i];
                                                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[i].Cells[0];
                                                        }*/

                                                        this.meacnt = 0;
                                                        this.currslotno = 0;
                                                        this.slotno = "";
                                                        this.ComSta = ComStatus.END;
                                                        if (this.btnScan.InvokeRequired)
                                                        {
                                                            this.btnScan.Invoke((Delegate)new Del_SetStop(this.SetStop));
                                                        }
                                                        else
                                                        {
                                                            this.btnScan.Text = CommonFun.GetLanText("measure");
                                                            this.setState(ComStatus.END);
                                                        }
                                                        if (CommonFun.GetAppConfig("GLPEnabled") == "true" && CommonFun.GetAppConfig("AutoSave") == "true" || CommonFun.GetAppConfig("GLPEnabled") == "false" && this.showpar.AutoSave)
                                                        {
                                                            //string sdir = CommonFun.GetUserDir(CommonFun.GetAppConfig("currentuser"));
                                                            string sdir = null;
                                                            if (sdir == null || sdir.Length == 0)
                                                                sdir = Environment.CurrentDirectory + "\\TestFile\\SpectrumScan\\";
                                                            bool savehappen = false;
                                                            string reasonOP = "";
                                                            string savefile = "";
                                                            List<SpectraScan> fslist = this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.D_savetime.HasValue)).ToList<SpectraScan>();
                                                            if (fslist.Count > 0 && CommonFun.GetAppConfig("GLPEnabled") == "true" && this.rightlist.Contains("rightspeES"))
                                                            {
                                                                /*using (InputWordFrm frm2 = new InputWordFrm())
                                                                {
                                                                    frm2.lbltitle.Text = CommonFun.GetLanText("esignreason");
                                                                    frm2.btnOK.Click += (EventHandler)((param0, param1) =>
                                                                    {
                                                                        if (frm2.txtValue.Text.Length <= 0)
                                                                        {
                                                                            CommonFun.showbox(CommonFun.GetLanText("reasonnull"), "Warning");
                                                                        }
                                                                        else
                                                                        {
                                                                            reasonOP = frm2.txtValue.Text;
                                                                            frm2.Close();
                                                                        }
                                                                    });
                                                                    int num2 = (int)frm2.ShowDialog();
                                                                }
                                                                if (reasonOP.Length <= 0)
                                                                    return;
                                                                using (PWDInputFrm frm3 = new PWDInputFrm())
                                                                {
                                                                    frm3.label4.Visible = false;
                                                                    frm3.btnOK.Click += (EventHandler)((param0, param1) =>
                                                                    {
                                                                        DataSet dataSet = SQLiteHelper.ExecuteDataSet("Data Source=programdb.db;Version=3;", "select * from  Users where C_name=@C_name and C_pwd=@C_pwd and I_logincnt=0", new object[2]
                                                                        {
                                        (object) CommonFun.GetAppConfig("currentuser"),
                                        (object) frm3.txtValue.Text
                                                                        });
                                                                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            string str1 = "Specturm" + DateTime.Now.ToString("yyyyMMddhhmmss");
                                                                            for (int index = 0; index < fslist.Count; ++index)
                                                                            {
                                                                                string str2 = str1 + "-" + (index + 1).ToString();
                                                                                string path = sdir + str2 + ".spe";
                                                                                if (!Directory.Exists(sdir))
                                                                                    Directory.CreateDirectory(sdir);
                                                                                CommonFun.setXmlValue("MeasureParams", "C_Precision", sdir);
                                                                                if (File.Exists(path) && (CommonFun.GetAppConfig("GLPEnabled") == "false" || CommonFun.GetAppConfig("GLPEnabled") == "true" && CommonFun.GetAppConfig("NoCoverData") == "true"))
                                                                                    str2 += "s";
                                                                                FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                                                                                BinaryFormatter binaryFormatter = new BinaryFormatter();
                                                                                try
                                                                                {
                                                                                    fslist[index].OperatorES = CommonFun.GetAppConfig("currentuser");
                                                                                    fslist[index].ESStatus = 1;
                                                                                    fslist[index].OperatorESTime = DateTime.Now;
                                                                                    fslist[index].status = true;
                                                                                    fslist[index].D_savetime = new DateTime?(DateTime.Now);
                                                                                    if (reasonOP.Length > 0)
                                                                                        fslist[index].C_reasonOP = reasonOP;
                                                                                    binaryFormatter.Serialize((Stream)fileStream, (object)fslist[index]);
                                                                                    fileStream.Close();
                                                                                    savehappen = true;
                                                                                    fslist[index].C_name = str2;
                                                                                    savefile = savefile + path + ",";
                                                                                }
                                                                                catch
                                                                                {
                                                                                    fslist[index].status = false;
                                                                                    fslist[index].D_savetime = new DateTime?();
                                                                                    fslist[index].OperatorES = "";
                                                                                    fslist[index].ESStatus = 0;
                                                                                    fileStream.Close();
                                                                                    CommonFun.showbox(CommonFun.GetLanText("saveerror"), "Error");
                                                                                    frm3.Close();
                                                                                    return;
                                                                                }
                                                                            }
                                                                            frm3.Close();
                                                                        }
                                                                        else
                                                                            CommonFun.showbox(CommonFun.GetLanText("pwderror"), "Error");
                                                                    });
                                                                    int num2 = (int)frm3.ShowDialog();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string str1 = "Specturm" + DateTime.Now.ToString("yyyyMMddhhmmss");
                                                                for (int index = 0; index < fslist.Count<SpectraScan>(); ++index)
                                                                {
                                                                    string str2 = str1 + "-" + (index + 1).ToString();
                                                                    string path = sdir + str2 + ".spe";
                                                                    if (!Directory.Exists(sdir))
                                                                        Directory.CreateDirectory(sdir);
                                                                    CommonFun.setXmlValue("MeasureParams", "C_Precision", sdir);
                                                                    if (File.Exists(path) && (CommonFun.GetAppConfig("GLPEnabled") == "false" || CommonFun.GetAppConfig("GLPEnabled") == "true" && CommonFun.GetAppConfig("NoCoverData") == "true"))
                                                                        str2 += "s";
                                                                    FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                                                                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                                                                    try
                                                                    {
                                                                        fslist[index].status = true;
                                                                        fslist[index].D_savetime = new DateTime?(DateTime.Now);
                                                                        binaryFormatter.Serialize((Stream)fileStream, (object)fslist[index]);
                                                                        fileStream.Close();
                                                                        savehappen = true;
                                                                        fslist[index].C_name = str2;
                                                                        savefile = savefile + path + ",";
                                                                    }
                                                                    catch
                                                                    {
                                                                        fslist[index].status = false;
                                                                        fslist[index].D_savetime = new DateTime?();
                                                                        fileStream.Close();
                                                                        CommonFun.showbox(CommonFun.GetLanText("saveerror"), "Error");
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            if (savehappen)
                                                            {
                                                                if (this.lblSample.InvokeRequired)
                                                                    this.lblSample.Invoke((Delegate)new WLScanFrm.Del_SamNameSet(this.SetSname));
                                                                else
                                                                    this.lblSample.Text = this.sslive.C_name;
                                                                CommonFun.InsertLog(CommonFun.GetLanText("specturm"), CommonFun.GetLanText("rightbcsj") + ":" + savefile, false);
                                                                if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                                                                    CommonFun.InsertLog(CommonFun.GetLanText("specturm"), CommonFun.GetLanText("filees") + ":" + savefile, false);*/
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                                        CommonFun.WriteSendLine("error，SCAN_STOPPING 0，" + ex.ToString());
                                        this.ComSta = ComStatus.END;
                                        this.meacnt = 0;
                                        this.currslotno = 0;
                                        this.slotno = "";
                                        if (this.btnScan.InvokeRequired)
                                            this.btnScan.Invoke((Delegate)new Del_SetStop(this.SetStop));
                                        else
                                            this.btnScan.Text = "Измерение";
                                        CommonFun.showbox(CommonFun.GetLanText("errorretry") + ex.ToString(), "Error");
                                        break;
                                    }
                                case ComStatus.CALBGND:
                                    try
                                    {
                                        if (text1.Contains("*CALDAT"))
                                        {
                                            string str1 = text1;
                                            int startIndex = str1.IndexOf("*CALDAT") + 7;
                                            string input = str1.Substring(startIndex, str1.Length - startIndex);
                                            if (input.Contains("END"))
                                                input = input.Substring(0, input.Length - 4);
                                            string[] strArray = new Regex("CALDAT").Split(input);
                                            string str2 = (Convert.ToDecimal(strArray[((IEnumerable<string>)strArray).Count<string>() - 1].Split(' ')[3]) / 10M).ToString("f1");
                                            if (this.lblWL.InvokeRequired)
                                            {
                                                this.lblWL.Invoke((Delegate)new Del_SetWL(this.SetWL), (object)str2, (object)"");
                                            }
                                            else
                                            {
                                                this.lblWL.Text = str2 + " нм";
                                                //   lbllocation.Invoke(() => this.lbllocation.Text = str2 + " нм");
                                            }
                                            System.Decimal num = Convert.ToDecimal(this.mpar.C_EndWL) - Convert.ToDecimal(this.mpar.C_BeginWL);
                                            int int32 = Convert.ToInt32((Convert.ToDecimal(str2) - Convert.ToDecimal(this.mpar.C_BeginWL)) * 100M / num);
                                            if (this.progressBar1.InvokeRequired)
                                                this.Invoke((Delegate)new SetPos(this.SetTextMessage), (object)int32);
                                            else
                                                this.progressBar1.Value = Convert.ToInt32(int32);
                                        }
                                        if (text1.Contains("END"))
                                        {
                                            if (this.stophappend > 0)
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.stophappend = 0;
                                                if (this.btnBlank.InvokeRequired)
                                                {
                                                    this.btnBlank.Invoke((Delegate)new Del_SetBlankLabel(this.Setblanklabel));
                                                    break;
                                                }
                                                this.btnBlank.Text = "Обнуление";
                                                //this.panel4.Visible = false;
                                                this.progressBar1.Value = 0;
                                                this.setState(ComStatus.END);
                                                break;
                                            }
                                            this.ComSta = ComStatus.END;
                                            if (this.btnBlank.InvokeRequired)
                                            {
                                                this.btnBlank.Invoke((Delegate)new Del_SetBlankLabel(this.Setblanklabel));
                                            }
                                            else
                                            {
                                                this.btnBlank.Text = "Обнуление";
                                                //this.panel4.Visible = false;
                                                this.progressBar1.Value = 0;
                                                this.setState(ComStatus.END);
                                            }
                                            CommonFun.showbox("Конец калибровки", "Information");
                                            break;
                                        }
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                                        //CommonFun.WriteSendLine("error，SCAN_STOPPING 0，" + ex.ToString());
                                        var st = new System.Diagnostics.StackTrace(ex, true);
                                        var frame = st.GetFrame(0);
                                        int line = frame.GetFileColumnNumber();
                                        CommonFun.WriteSendLine("error，SCAN_STOPPING 0，" + line.ToString());
                                        this.ComSta = ComStatus.END;
                                        if (this.btnBlank.InvokeRequired)
                                            this.btnBlank.Invoke((Delegate)new Del_SetBlankLabel(this.Setblanklabel));
                                        else
                                            this.btnBlank.Text = "Обнуление";
                                        CommonFun.showbox(CommonFun.GetLanText("stopmeasure"), "Error");
                                        break;
                                    }
                                case ComStatus.BD_RATIO_FLUSH:
                                    if (text1.Contains("RCVD"))
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (this.btnBlank.InvokeRequired)
                                            this.btnBlank.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
                                        else
                                            this.setstate(true);
                                        if (this.btnBlank.InvokeRequired)
                                            this.btnBlank.Invoke((Delegate)new Del_starttt(this.Starttt), (object)false);
                                        else
                                            this.Starttt(false);
                                        break;
                                    }
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("errorretry") + ex.ToString(), "Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.ToString(), "Error");
            }
        }


        private void btnSet_Click(object sender, EventArgs e)
        {
            tt.Stop();

            //this.mpar.C_BeginWL = 1100.ToString();


            SpectrumScanMethod spectrumScanMethod = new SpectrumScanMethod();
            spectrumScanMethod.ShowDialog();
            if (spectrumScanMethod.close == false)
            {

                if (Convert.ToDecimal(spectrumScanMethod.start_wl) >= Convert.ToDecimal(spectrumScanMethod.cancel_wl))
                {
                    this.mpar.C_BeginWL = spectrumScanMethod.start_wl;
                    this.mpar.C_EndWL = spectrumScanMethod.cancel_wl;
                }
                else
                {
                    this.mpar.C_EndWL = spectrumScanMethod.start_wl;
                    this.mpar.C_BeginWL = spectrumScanMethod.cancel_wl;
                }
                this.mpar.C_Interval = spectrumScanMethod.time_interval;
                this.mpar.C_Intervals = spectrumScanMethod.time_interval;

                photometric_mode = spectrumScanMethod.photometric_mode;
                if (photometric_mode == "Абсорбция (Abs)")
                {
                    lblmode.Text = "Abs";
                    this.mpar.C_Mode = "Abs";
                    ColPXGD.HeaderText = "Abs";
                }
                else
                {
                    lblmode.Text = "%T";
                    this.mpar.C_Mode = "%T";
                    ColPXGD.HeaderText = "%T";
                }
                //this.mpar.C_SLength = spectrumScanMethod.time_interval;
                this.mpar.C_SLength = spectrumScanMethod.optical_path;

                this.mpar.C_ScanSpeed = spectrumScanMethod.speed_measure;
                this.mpar.C_ScanSpeedDM = spectrumScanMethod.speed_measure;
                this.mpar.C_StepLen = spectrumScanMethod.step_interval;
                this.mpar.C_ScanCNT = spectrumScanMethod.loop_measure.ToString();

                this.mpar.D_Mtime = new DateTime?(DateTime.Now);
                //   this.mpar = smpar;
                if (this.currlist.Count > 0)
                {
                    this.dataGridView1.Rows.Clear();
                    for (int index = 0; index < this.currlist[0].Data.Count<MeaureData>(); ++index)
                    {
                        this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Cells["ColWL"].Value = (object)this.currlist[0].Data[index].xValue;
                        if (this.currlist[0].MethodPar.C_Mode == "Abs")
                        {
                            this.dataGridView1.Columns["ColXGD"].HeaderText = "Abs";
                            this.dataGridView1.Rows[index].Cells["ColXGD"].Value = (object)this.currlist[0].Data[index].yABS.ToString(this.absacc);
                        }
                        else
                        {
                            this.dataGridView1.Columns["ColXGD"].HeaderText = "T";
                            this.dataGridView1.Rows[index].Cells["ColXGD"].Value = (object)this.currlist[0].Data[index].YT.ToString(this.tacc);
                        }
                        this.dataGridView1.Rows[index].Tag = (object)this.currlist[0].Data[index];

                    }
                    if (this.currlist[0].Data.Count < this.dgvcnt)
                        this.dataGridView1.Rows.Add(this.dgvcnt - this.currlist[0].Data.Count);
                    this.ResetTempReDraw(1);
                }
                else
                    this.ResetTempReDraw(0);
                this.scanwlpar = (Convert.ToDecimal(this.mpar.C_BeginWL) * 10M).ToString("f0") + " " + (Convert.ToDecimal(this.mpar.C_EndWL) * 10M).ToString("f0") + " " + (Convert.ToDecimal(this.mpar.C_StepLen) * 10M).ToString("f0");
                this.scanmosptpar = (!(this.mpar.C_ScanSpeed == "Быстро") ? (!(this.mpar.C_ScanSpeed == "Средне") ? (!(this.mpar.C_ScanSpeed == "Медленно") ? 4 : 3) : 2) : 1).ToString();
                this.picCurve.Visible = true;
            }
            tt.Start();
        }

        private void BtnBlank_Click(object sender, EventArgs e)
        {
            /*if (this.mpar.C_methodoperator == null || this.mpar.C_methodoperator.Length <= 0)
           {
               CommonFun.showbox(CommonFun.GetLanText("nomethod"), "Error");
           }
           else
           {*/
            string errormsg = "";
            /* if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                 CommonFun.showbox(errormsg, "Error");
             else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                 CommonFun.showbox(errormsg, "Error");*/
            //this.tt.Stop();
            if (!this.sp.IsOpen)
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
            else if (this.btnBlank.Text.ToString() == CommonFun.GetLanText("stopblanking"))
            {
                if (this.ComSta == ComStatus.CALBGND)
                {
                    this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                    CommonFun.WriteSendLine("stop,SCAN_STOPPING 0");
                    this.stophappend = 1;
                    this.btnBlank.Text = "Остановить";
                }
                else
                {
                    this.ComSta = ComStatus.END;
                    this.setState(ComStatus.END);
                    this.btnBlank.Text = "Обнуление";
                }
            }
            else if (this.ComSta != ComStatus.END)
            {
                CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
            }
            else
            {
                //this.ResetTempReDraw(0);
                this.calormea = 1;
                if (CommonFun.GetAppConfig("EightSlot") == "true")
                {
                    /*using (SelectOneSlotFrm frm = new SelectOneSlotFrm())
                    {
                        frm.grblgp.Visible = false;
                        frm.grbxf.Visible = false;
                        frm.grbslot.Visible = true;
                        frm.btnOK.Click += (EventHandler)((param0, param1) =>
                        {
                            string str = "";
                            if (frm.rab1.Checked)
                                str = "1";
                            else if (frm.rab2.Checked)
                                str = "2";
                            else if (frm.rab3.Checked)
                                str = "3";
                            else if (frm.rab4.Checked)
                                str = "4";
                            else if (frm.rab5.Checked)
                                str = "5";
                            else if (frm.rab6.Checked)
                                str = "6";
                            else if (frm.rab7.Checked)
                                str = "7";
                            else if (frm.rab8.Checked)
                                str = "8";
                            frm.Close(); //this.Dispose();
                            this.ComSta = ComStatus.SETCHP;
                            this.sp.WriteLine("SETCHP " + str + "\r\n");
                            CommonFun.WriteSendLine("SETCHP " + str);
                            this.setState(ComStatus.CALBGND);
                            //this.panel4.Visible = true;
                           // this.lblProsess.Text = CommonFun.GetLanText("inblanking");
                            this.progressBar1.Value = 5;
                            this.btnBlank.Text = CommonFun.GetLanText("stopblanking");
                            CommonFun.InsertLog(CommonFun.GetLanText("specturm"), CommonFun.GetLanText("blanking"), false);
                        });
                        int num = (int)frm.ShowDialog();
                    }*/
                }
                else
                {
                    string[] strArray1 = new string[7]
                    {
              "SETSCANWL ",
              (Convert.ToDecimal(this.mpar.C_BeginWL) * 10M).ToString("f0"),
              " ",
              null,
              null,
              null,
              null
                    };
                    string[] strArray2 = strArray1;
                    System.Decimal num = Convert.ToDecimal(this.mpar.C_EndWL) * 10M;
                    string str1 = num.ToString("f0");
                    strArray2[3] = str1;
                    strArray1[4] = " ";
                    string[] strArray3 = strArray1;
                    num = Convert.ToDecimal(this.mpar.C_StepLen) * 10M;
                    string str2 = num.ToString("f0");
                    strArray3[5] = str2;
                    strArray1[6] = "\r\n";
                    string text = string.Concat(strArray1);
                    this.ComSta = ComStatus.SETSCANWL;
                    this.sp.WriteLine(text);
                    CommonFun.WriteSendLine(text);
                    //CommonFun.WriteSendLine("Проверка что задаем в калибровку: " + text);
                    this.setState(ComStatus.CALBGND);
                    ///this.lblProsess.Text = CommonFun.GetLanText("inblanking");
                    this.progressBar1.Value = 5;
                    this.btnBlank.Text = "Остановить";
                    CommonFun.InsertLog("Spectrum scan", "Blank", false);
                }
            }
            //}
        }
        private void ResetTempReDraw(int type)
        {
            this.Peak = 0;
            this.valley = 0;
            this.peakList.Clear();
            this.valleyList.Clear();
            this.selectlist.Clear();
            this.flist.Clear();
            this.xyRangeList.Clear();
            this.ZoomOut = 0;
            this.ThreeDMode = 0;
            this.dlist = new List<SpectraScan>();
            this.dinter = 0;
            if (type > 0)
            {
                ///MyTableList.Clear();
                if (this.currlist.Count > 0)
                {
                    this.lblSample.Text = this.currlist[0].C_name;
                    OperateType? opertype = this.currlist[0].opertype;
                    if ((opertype.GetValueOrDefault() != OperateType.Square ? 1 : (!opertype.HasValue ? 1 : 0)) != 0)
                    {
                        this.SquareMode = 0;
                        this.dataGridView2.Visible = false;
                        this.dgvPoint.Visible = false;
                        this.dataGridView1.Visible = true;
                    }
                    else
                    {
                        this.SquareMode = 1;
                        this.dataGridView2.Visible = true;
                        this.dgvPoint.Visible = false;
                        this.dataGridView1.Visible = false;
                    }
                }
                else
                    this.lblSample.Text = "";
                this.XYMaxMin();
                if (this.showpar.AutoXY)
                    this.AddToRangeList(this.xMin, this.xMax, this.yMin, this.yMax);
                else
                    this.AddToRangeList(this.showpar.xMin, this.showpar.xMax, this.showpar.yMin, this.showpar.yMax);
            }
            else
            {
                this.lblSample.Text = "";
                this.SquareMode = 0;
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Columns[2].HeaderText = this.mpar.C_Mode;
                this.lblmode.Text = this.mpar.C_Mode;
                this.picCurve.Image = (System.Drawing.Image)new Bitmap(this.picCurve.Width, this.picCurve.Height);
                this.picTop.Image = (System.Drawing.Image)new Bitmap(this.picTop.Width, this.picTop.Height);
                List<SpectraScan> currlist = this.currlist;
                this.currlist = new List<SpectraScan>();
                this.DrawLine();
                this.currlist = currlist;
            }
        }

        private void AddToRangeList(float x1, float x2, float y1, float y2)
        {
            int index1 = this.xyRangeList.FindIndex((Predicate<XYRange>)(s => s.Curr));
            if (index1 < this.xyRangeList.Count - 1)
            {
                int num = this.xyRangeList.Count<XYRange>();
                for (int index2 = index1 + 1; index2 < num; ++index2)
                    this.xyRangeList.RemoveAt(index1 + 1);
            }
            foreach (XYRange xyRange in this.xyRangeList)
            {
                if (xyRange.Curr)
                    xyRange.Curr = false;
            }
            this.xyRangeList.Add(new XYRange()
            {
                Curr = true,
                X1 = (double)x1 < (double)x2 ? x1 : x2,
                X2 = (double)x1 > (double)x2 ? x1 : x2,
                Y1 = (double)y1 < (double)y2 ? y1 : y2,
                Y2 = (double)y1 > (double)y2 ? y1 : y2
            });
            this.DrawLine();
        }



        private void XYMaxMin()
        {
            if (this.currlist.Count < 1)
                return;
            List<MeaureData> data = this.currlist[0].Data;
            this.XMaxMinStr(data, out this.xMax, out this.xMin);
            if (this.currlist[0].MethodPar.C_Mode == "Abs")
            {
                this.yMax = data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.yABS)).Max();
                this.yMin = data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.yABS)).Min();
            }
            else
            {
                this.yMax = data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.YT)).Max();
                this.yMin = data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.YT)).Min();
            }
            if (this.showpar.MulShow != 1)
                return;
            List<SpectraScan> list = this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => s.IsShow == 1)).ToList<SpectraScan>();
            if (list.Count > 0)
            {
                foreach (SpectraScan spectraScan in list)
                {
                    float xmax;
                    float xmin;
                    this.XMaxMinStr(spectraScan.Data, out xmax, out xmin);
                    float num1;
                    float num2;
                    if (this.currlist[0].MethodPar.C_Mode == "Abs")
                    {
                        num1 = spectraScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.yABS)).Max();
                        num2 = spectraScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.yABS)).Min();
                    }
                    else
                    {
                        num1 = spectraScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.YT)).Max();
                        num2 = spectraScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.YT)).Min();
                    }
                    if ((double)xmax > (double)this.xMax)
                        this.xMax = xmax;
                    if ((double)xmin < (double)this.xMin)
                        this.xMin = xmin;
                    if ((double)num1 > (double)this.yMax)
                        this.yMax = num1;
                    if ((double)num2 < (double)this.yMin)
                        this.yMin = num2;
                }
            }
        }

        private void XMaxMinStr(List<MeaureData> list, out float xmax, out float xmin)
        {
            xmin = (float)Convert.ToDouble(list[0].xValue);
            xmax = (float)Convert.ToDouble(list[0].xValue);
            for (int index = 0; index < list.Count; ++index)
            {
                float num = (float)Convert.ToDouble(list[index].xValue);
                if ((double)num < (double)xmin)
                    xmin = num;
                if ((double)num > (double)xmax)
                    xmax = num;
            }
        }

        private void pibTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.currlist.Count <= 0 || this.currlist[0].Data.Count <= 0)
                return;
            this.start = new Point(e.X, e.Y);
            this.downmove = 1;
        }

        private void pibTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.currlist.Count <= 0 || this.currlist[0].Data.Count <= 0)
                return;
            double num1 = ((double)e.X - (double)this.left) * ((double)this.xe - (double)this.xs) / ((double)this.right - (double)this.left) + (double)this.xs;
            float num2 = (float)(((double)this.bottom - (double)e.Y) * ((double)this.ye - (double)this.ys) / ((double)this.bottom - (double)this.top)) + this.ys;
            Bitmap bitmap = new Bitmap(this.picTop.Width, this.picCurve.Height);
            Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            if (this.ZoomOut == 1 && this.downmove == 1)
            {
                graphics.DrawLine(new Pen(Color.Black, 1f), this.start.X, this.start.Y, e.X, this.start.Y);
                graphics.DrawLine(new Pen(Color.Black, 1f), this.start.X, this.start.Y, this.start.X, e.Y);
                graphics.DrawLine(new Pen(Color.Black, 1f), this.start.X, e.Y, e.X, e.Y);
                graphics.DrawLine(new Pen(Color.Black, 1f), e.X, this.start.Y, e.X, e.Y);
            }
            if ((double)this.ybx >= (double)this.left)
                graphics.DrawLine(new Pen(Color.Red, 1f), (float)(this.ybx - 3), this.bottom - 3f, (float)(this.ybx - 3), this.top - 3f);
            this.picTop.Image = (System.Drawing.Image)bitmap;
            this.picTop.Refresh();
        }

        private void pibTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.currlist.Count <= 0 || this.currlist[0].Data.Count <= 0)
                return;
            this.downmove = 0;
            this.ybx = e.X;
            this.curx = (float)(((double)e.X - (double)this.left) * ((double)this.xe - (double)this.xs) / ((double)this.right - (double)this.left)) + this.xs;
            if (Convert.ToDecimal(this.currlist[0].MethodPar.C_StepLen) > 0.1M)
            {
                this.curx = Convert.ToSingle(this.currlist[0].Data[CommonFun.getNear(this.currlist[0].Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => Convert.ToSingle(s.xValue))).ToArray<float>(), this.curx)].xValue);
                this.ybx = (int)Convert.ToInt16((double)this.left + ((double)this.curx - (double)this.xs) * this.xInt);
            }
            int int16_1 = (int)Convert.ToInt16((float)(((double)e.X - (double)this.left) * ((double)this.xe - (double)this.xs) / ((double)this.right - (double)this.left)) + this.xs);
            float y2 = (float)(((double)this.bottom - (double)e.Y) * ((double)this.ye - (double)this.ys) / ((double)this.bottom - (double)this.top)) + this.ys;
            Bitmap bitmap = new Bitmap(this.picTop.Width, this.picTop.Height);
            Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            if (this.ZoomOut == 1)
            {
                if (Math.Abs(this.start.X - e.X) > 10 || Math.Abs(this.start.Y - e.Y) > 10)
                {
                    int int16_2 = (int)Convert.ToInt16((float)(((double)this.start.X - (double)this.left) * ((double)this.xe - (double)this.xs) / ((double)this.right - (double)this.left)) + this.xs);
                    float y1 = (float)(((double)this.bottom - (double)this.start.Y) * ((double)this.ye - (double)this.ys) / ((double)this.bottom - (double)this.top)) + this.ys;
                    this.AddToRangeList((float)int16_2, (float)int16_1, y1, y2);
                }
            }
            else if (this.SquareMode == 1)
            {
                if (this.currlist[0].Squaredata.Count == 0)
                    this.GenerateNewSquareData();
                if (this.dataGridView2.SelectedCells.Count > 0)
                {
                    int columnIndex = this.dataGridView2.SelectedCells[0].ColumnIndex;
                    int rowIndex = this.dataGridView2.SelectedCells[0].RowIndex;
                    this.curx = !(Convert.ToDecimal(this.currlist[0].MethodPar.C_StepLen) >= 1M) ? (float)Convert.ToDouble(this.curx.ToString("f1")) : (float)Convert.ToDouble(this.curx.ToString("f0"));
                    switch (columnIndex)
                    {
                        case 3:
                            this.dataGridView2.SelectedCells[0].Value = (object)this.curx.ToString("f1");
                            if (rowIndex > -1)
                            {
                                this.currlist[0].Squaredata[rowIndex].BeginWL = this.curx.ToString("f1");
                                if (this.currlist[0].Squaredata[rowIndex].BeginWL != null && this.currlist[0].Squaredata[rowIndex].EndWL != null)
                                    this.SquareDataCauDraw(rowIndex);
                                break;
                            }
                            break;
                        case 4:
                            this.dataGridView2.SelectedCells[0].Value = (object)this.curx.ToString("f1");
                            if (rowIndex > -1)
                            {
                                this.currlist[0].Squaredata[rowIndex].EndWL = this.curx.ToString("f1");
                                if (this.currlist[0].Squaredata[rowIndex].BeginWL != null && this.currlist[0].Squaredata[rowIndex].EndWL != null)
                                    this.SquareDataCauDraw(rowIndex);
                            }
                            break;
                    }
                    SquareData squareData = this.currlist[0].Squaredata[this.currlist[0].Squaredata.Count - 1];
                    if (squareData.BeginWL != null || squareData.EndWL != null)
                        this.GenerateNewSquareData();
                    this.dataGridView2.CurrentCell = this.dataGridView2.Rows[rowIndex].Cells[columnIndex];
                    this.dataGridView2.Rows[rowIndex].Cells[columnIndex].Selected = true;
                }
            }
            if ((double)this.ybx >= (double)this.left)
                graphics.DrawLine(new Pen(Color.Red, 1f), (float)(this.ybx - 3), this.bottom - 3f, (float)(this.ybx - 3), this.top - 3f);
            if (Convert.ToDecimal(this.currlist[0].MethodPar.C_StepLen) >= 1M)
            {
                if (this.currlist[0].MethodPar.C_Mode == "Abs")
                {
                    MeaureData q = this.currlist[0].Data.Where<MeaureData>((Func<MeaureData, bool>)(s =>
                    {
                        float single = Convert.ToSingle(s.xValue);
                        string str1 = single.ToString("f0");
                        single = Convert.ToSingle(this.curx);
                        string str2 = single.ToString("f0");
                        return str1 == str2;
                    })).ToList<MeaureData>().SingleOrDefault<MeaureData>();
                    if (q != null)
                    {
                        //  Label lbllocation = this.lbllocation;
                        float single = Convert.ToSingle(this.curx);
                        string str1 = single.ToString("f0");
                        single = Convert.ToSingle(q.yABS);
                        string str2 = single.ToString(this.absacc);
                        string str3 = str1 + " нм," + str2 + " Abs";
                        lblWL.Text = str3;
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s == q))].Cells[0];
                    }
                    else
                        this.lblWL.Text = "";
                }
                else
                {
                    MeaureData q = this.currlist[0].Data.Where<MeaureData>((Func<MeaureData, bool>)(s =>
                    {
                        float single = Convert.ToSingle(s.xValue);
                        string str1 = single.ToString("f0");
                        single = Convert.ToSingle(this.curx);
                        string str2 = single.ToString("f0");
                        return str1 == str2;
                    })).ToList<MeaureData>().SingleOrDefault<MeaureData>();
                    if (q != null)
                    {
                        //   Label lbllocation = this.lbllocation;
                        float single = Convert.ToSingle(this.curx);
                        string str1 = single.ToString("f0");
                        single = Convert.ToSingle(q.YT);
                        string str2 = single.ToString(this.tacc);
                        string str3 = str1 + " нм," + str2 + " %T";
                        lblWL.Text = str3;
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s == q))].Cells[0];
                    }
                    else
                        this.lblWL.Text = "";
                }
            }
            else if (this.currlist[0].MethodPar.C_Mode == "Abs")
            {
                MeaureData q = this.currlist[0].Data.Where<MeaureData>((Func<MeaureData, bool>)(s => s.xValue == Convert.ToSingle(this.curx).ToString("f1"))).ToList<MeaureData>().SingleOrDefault<MeaureData>();
                if (q != null)
                {
                    //   Label lbllocation = this.lbllocation;
                    float single = Convert.ToSingle(this.curx);
                    string str1 = single.ToString("f1");
                    single = Convert.ToSingle(q.yABS);
                    string str2 = single.ToString(this.absacc);
                    string str3 = str1 + " нм," + str2 + " Abs";
                    lblWL.Text = str3;
                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s == q))].Cells[0];
                }
                else
                    this.lblWL.Text = "";
            }
            else
            {
                MeaureData q = this.currlist[0].Data.Where<MeaureData>((Func<MeaureData, bool>)(s => s.xValue == Convert.ToSingle(this.curx).ToString("f1"))).ToList<MeaureData>().SingleOrDefault<MeaureData>();
                if (q != null)
                {
                    //     Label lbllocation = this.lbllocation;
                    float single = Convert.ToSingle(this.curx);
                    string str1 = single.ToString("f1");
                    single = Convert.ToSingle(q.YT);
                    string str2 = single.ToString(this.tacc);
                    string str3 = str1 + " нм," + str2 + " %T";
                    lblWL.Text = str3;
                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s == q))].Cells[0];
                }
                else
                    this.lblWL.Text = "";
            }
            this.picTop.Image = (System.Drawing.Image)bitmap;
            this.picTop.Refresh();
        }

        private void DrawLine()
        {
            if (this.mpar == null)
                return;
            Bitmap bitmap = new Bitmap(this.picCurve.Width, this.picCurve.Height);
            Graphics objGraphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            objGraphics.FillRectangle((Brush)new SolidBrush(SystemColors.Control), 0, 0, this.picCurve.Width, this.picCurve.Height);
            SizeF sizeF1 = objGraphics.MeasureString("0.0001", new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            SizeF sizeF2 = this.currlist.Count > 0 && this.currlist[0].Data.Count > 0 ? objGraphics.MeasureString(Convert.ToDouble(this.currlist[0].Data.Select<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue)).Min<string>()).ToString("f1"), new System.Drawing.Font("Segoe UI", (float)this.FontSize)) : objGraphics.MeasureString("1100.0", new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            this.left = sizeF2.Height + 20f + sizeF1.Width;
            this.right = (float)(this.picCurve.Width - 5);
            this.bottom = (float)((double)this.picCurve.Height - (double)sizeF2.Height * 2.0 - 20.0);
            this.top = sizeF2.Height + 20f;
            RectangleF rectangleF = new RectangleF(this.left, this.top, this.right - this.left, this.bottom - this.top);
            if (this.currlist.Count <= 0 || this.currlist[0].Data.Count <= 0)
            {
                if (this.mpar.C_BeginWL != "" && this.mpar.C_EndWL != "")
                {
                    this.xs = Convert.ToSingle(this.mpar.C_EndWL);
                    this.xe = Convert.ToSingle(this.mpar.C_BeginWL);
                }
                else
                {
                    this.xs = 190f;
                    this.xe = 1100f;
                }
                this.ys = this.showpar.yMin;
                this.ye = this.showpar.yMax;
                if (this.mpar.C_Mode == "Abs")
                    this.DrawScale(ref objGraphics, 0, "Abs");
                else
                    this.DrawScale(ref objGraphics, 0, "%T");
            }
            else
            {
                List<XYRange> list = this.xyRangeList.Where<XYRange>((Func<XYRange, bool>)(s => s.Curr)).ToList<XYRange>();
                if (list.Count < 1)
                {
                    CommonFun.showbox("norangedata", "Error");
                    return;
                }
                XYRange xyr = list[0];
                this.xs = xyr.X1;
                this.xe = xyr.X2;
                this.ys = xyr.Y1;
                this.ye = xyr.Y2;
                if (this.autoscale)
                {
                    if (this.mpar.C_Mode == "Abs")
                    {
                        string str = "0";
                        switch (this.absacc)
                        {
                            case "f6":
                                str = "0.000001";
                                break;
                            case "f5":
                                str = "0.00001";
                                break;
                            case "f4":
                                str = "0.0001";
                                break;
                            case "f3":
                                str = "0.001";
                                break;
                            case "f2":
                                str = "0.01";
                                break;
                            case "f1":
                                str = "0.1";
                                break;
                        }
                        if ((double)Math.Abs(this.ye) < (double)Convert.ToSingle(str))
                            this.ye = Convert.ToSingle(str);
                        if ((double)Math.Abs(this.ys) < (double)Convert.ToSingle(str))
                            this.ys = -Convert.ToSingle(str);
                        if ((double)Math.Abs(this.ye) > (double)Convert.ToSingle(str) || (double)Math.Abs(this.ys) > (double)Convert.ToSingle(str))
                        {
                            this.ye = (float)((1.8 * (double)this.ye - 0.2 * (double)this.ys) / 1.6);
                            this.ys = (float)((1.8 * (double)this.ys - 0.2 * (double)this.ye) / 1.6);
                        }
                    }
                    else
                    {
                        this.ye = (float)((1.8 * (double)this.ye - 0.2 * (double)this.ys) / 1.6);
                        this.ys = (float)((1.8 * (double)this.ys - 0.2 * (double)this.ye) / 1.6);
                    }
                }
                if (this.currlist[0].MethodPar.C_Mode == "Abs")
                    this.DrawScale(ref objGraphics, this.currlist[0].Data.Count<MeaureData>(), "Abs");
                else
                    this.DrawScale(ref objGraphics, this.currlist[0].Data.Count<MeaureData>(), "%T");
                List<SpectraScan> spectraScanList = new List<SpectraScan>();
                spectraScanList.Add(this.currlist[0]);
                if (this.showpar.MulShow == 1)
                    spectraScanList.AddRange((IEnumerable<SpectraScan>)this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => s.IsShow == 1)).ToList<SpectraScan>());
                foreach (SpectraScan spectraScan in spectraScanList)
                {
                    List<MeaureData> source = spectraScan.Data;
                    int index1 = source.FindIndex((Predicate<MeaureData>)(s => s.xValue.Equals(xyr.X1.ToString("f1"))));
                    int index2 = source.FindIndex((Predicate<MeaureData>)(s => s.xValue.Equals(xyr.X2.ToString("f1"))));
                    if ((double)xyr.X1 < (double)this.xMin)
                        index1 = source.FindIndex((Predicate<MeaureData>)(s => Convert.ToDouble(s.xValue) == Convert.ToDouble(this.xMin)));
                    if ((double)xyr.X2 > (double)this.xMax)
                        index2 = source.FindIndex((Predicate<MeaureData>)(s => Convert.ToDouble(s.xValue) == Convert.ToDouble(this.xMax)));
                    List<MeaureData> meaureDataList = new List<MeaureData>();
                    if (index1 == -1 || index2 == -1)
                    {
                        meaureDataList = source;
                    }
                    else
                    {
                        for (int index3 = Math.Min(index1, index2); index3 <= Math.Max(index2, index1); ++index3)
                            meaureDataList.Add(source[index3]);
                    }
                    if (meaureDataList.Count > 0)
                        source = meaureDataList;
                    else if (this.ZoomOut == 1)
                    {
                        this.xyRangeList.RemoveAt(this.xyRangeList.Count<XYRange>() - 1);
                        this.xyRangeList[this.xyRangeList.Count<XYRange>() - 1].Curr = true;
                        return;
                    }
                    if ((double)this.xe - (double)this.xs == 0.0 || (double)this.ye - (double)this.ys == 0.0)
                        return;
                    this.xInt = ((double)this.right - (double)this.left) / ((double)this.xe - (double)this.xs);
                    this.yInt = ((double)this.bottom - (double)this.top) / ((double)this.ye - (double)this.ys);
                    double num1 = (double)this.left + (Convert.ToDouble(source[source.Count<MeaureData>() - 1].xValue) - (double)this.xs) * this.xInt;
                    string cMode = this.currlist[0].MethodPar.C_Mode;
                    double num2 = !(cMode == "Abs") ? (double)this.bottom - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].YT) - (double)this.ys) * this.yInt : (double)this.bottom - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].yABS) - (double)this.ys) * this.yInt;
                    if (num2 < (double)this.top)
                        num2 = (double)this.top;
                    if (num2 > (double)this.bottom)
                        num2 = (double)this.bottom;
                    for (int index3 = source.Count<MeaureData>() - 2; index3 >= 0; --index3)
                    {
                        double num3 = (double)this.left + (Convert.ToDouble(source[index3].xValue) - (double)this.xs) * this.xInt;
                        double num4 = !(cMode == "Abs") ? (Convert.ToDouble(source[index3].YT) >= (double)this.ys ? (Convert.ToDouble(source[index3].YT) <= (double)this.ye ? (double)this.bottom - (Convert.ToDouble(source[index3].YT) - (double)this.ys) * this.yInt : (double)this.top) : (double)this.bottom) : (Convert.ToDouble(source[index3].yABS) >= (double)this.ys ? (Convert.ToDouble(source[index3].yABS) <= (double)this.ye ? (double)this.bottom - (Convert.ToDouble(source[index3].yABS) - (double)this.ys) * this.yInt : (double)this.top) : (double)this.bottom);
                        objGraphics.DrawLine(new Pen(spectraScan.color, 1f), (float)num1, (float)num2, (float)num3, (float)num4);
                        num1 = num3;
                        num2 = num4;
                    }
                }
                if (this.SquareMode != 1)
                    this.DrawPoint(ref objGraphics);
                if (this.SquareMode == 1)
                {
                    for (int index1 = 0; index1 < this.currlist[0].Squaredata.Count; ++index1)
                    {
                        SquareData sd = this.currlist[0].Squaredata[index1];
                        if (sd.BeginWL != null && sd.EndWL != null)
                        {
                            float num1 = this.left + (float)((Convert.ToDouble(sd.BeginWL) - Convert.ToDouble(this.xs)) * this.xInt);
                            float num2 = this.left + (float)((Convert.ToDouble(sd.EndWL) - Convert.ToDouble(this.xs)) * this.xInt);
                            int index2 = this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s.xValue == sd.BeginWL));
                            int index3 = this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s.xValue == sd.EndWL));
                            float num3 = (float)(((double)num1 < (double)num2 ? (double)num1 : (double)num2) + 4.0);
                            if (this.currlist[0].FromZero)
                            {
                                float y1 = this.bottom - (float)((0.0 - (double)this.ys) * this.yInt);
                                if (this.currlist[0].MethodPar.C_Mode == "Abs")
                                {
                                    float y2_1 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[index2].yABS) - (double)this.ys) * this.yInt);
                                    float y2_2 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[index3].yABS) - (double)this.ys) * this.yInt);
                                    objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num1, y1, num1, y2_1);
                                    objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num2, y1, num2, y2_2);
                                }
                                else
                                {
                                    float y2_1 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[index2].YT) - (double)this.ys) * this.yInt);
                                    float y2_2 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[index3].YT) - (double)this.ys) * this.yInt);
                                    objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num1, y1, num1, y2_1);
                                    objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num2, y1, num2, y2_2);
                                }
                                for (; (double)num3 <= ((double)num1 > (double)num2 ? (double)num1 : (double)num2); num3 += 4f)
                                {
                                    double xwl = Convert.ToDouble(num3 - this.left) / this.xInt + (double)this.xs;
                                    xwl = !(Convert.ToDecimal(this.currlist[0].MethodPar.C_StepLen) >= 1M) ? Convert.ToDouble(xwl.ToString("f1")) : Convert.ToDouble(xwl.ToString("f0"));
                                    int index4 = this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s.xValue == xwl.ToString("f1")));
                                    if (this.currlist[0].MethodPar.C_Mode == "Abs")
                                    {
                                        float y2 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[index4].yABS) - (double)this.ys) * this.yInt);
                                        objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num3, y1, num3, y2);
                                    }
                                    else
                                    {
                                        float y2 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[index4].YT) - (double)this.ys) * this.yInt);
                                        objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num3, y1, num3, y2);
                                    }
                                }
                            }
                            else
                            {
                                for (; (double)num3 <= ((double)num1 > (double)num2 ? (double)num1 : (double)num2); num3 += 4f)
                                {
                                    float num4 = Convert.ToSingle(num3 - this.left) / Convert.ToSingle(this.xInt) + this.xs;
                                    float target = !(Convert.ToDecimal(this.currlist[0].MethodPar.C_StepLen) >= 1M) ? Convert.ToSingle(num4.ToString("f1")) : Convert.ToSingle(num4.ToString("f0"));
                                    int near = CommonFun.getNear(this.currlist[0].Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => Convert.ToSingle(s.xValue))).ToArray<float>(), target);
                                    float y1 = this.bottom - (float)((Convert.ToDouble(sd.k * target + sd.b) - (double)this.ys) * this.yInt);
                                    if (this.currlist[0].MethodPar.C_Mode == "Abs")
                                    {
                                        float y2 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[near].yABS) - (double)this.ys) * this.yInt);
                                        objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num3, y1, num3, y2);
                                    }
                                    else
                                    {
                                        float y2 = this.bottom - (float)((Convert.ToDouble(this.currlist[0].Data[near].YT) - (double)this.ys) * this.yInt);
                                        objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num3, y1, num3, y2);
                                    }
                                }
                                float y1_1 = this.bottom - (float)(((double)sd.k * Convert.ToDouble(sd.BeginWL) + (double)sd.b - (double)this.ys) * this.yInt);
                                float y2_1 = this.bottom - (float)(((double)sd.k * Convert.ToDouble(sd.EndWL) + (double)sd.b - (double)this.ys) * this.yInt);
                                if (this.currlist[0].ShowZero)
                                    objGraphics.DrawLine(new Pen(sd.sqColor, 1f), num1, y1_1, num2, y2_1);
                            }
                            if (this.currlist[0].MethodPar.C_Mode == "Abs")
                            {
                                float num4 = this.bottom - (float)(((double)this.currlist[0].Data[index2].yABS - (double)this.ys) * this.yInt);
                                float num5 = this.bottom - (float)(((double)this.currlist[0].Data[index3].yABS - (double)this.ys) * this.yInt);
                                objGraphics.DrawString((index1 + 1).ToString(), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(num1 + (float)(((double)num2 - (double)num1) / 2.0), num4 + (float)(((double)num5 - (double)num4) / 2.0)));
                            }
                            else
                            {
                                float num4 = this.bottom - (float)(((double)this.currlist[0].Data[index2].YT - (double)this.ys) * this.yInt);
                                float num5 = this.bottom - (float)(((double)this.currlist[0].Data[index3].YT - (double)this.ys) * this.yInt);
                                objGraphics.DrawString((index1 + 1).ToString(), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(num1 + (float)(((double)num2 - (double)num1) / 2.0), num4 + (float)(((double)num5 - (double)num4) / 2.0)));
                            }
                        }
                    }
                }
            }
            this.picCurve.Image = (System.Drawing.Image)bitmap;
        }
        private void DrawPoint(ref Graphics objGraphics)
        {
            this.flist = new List<MeaureData>();
            if (this.peakList.Count > 0)
                this.flist.AddRange((IEnumerable<MeaureData>)this.peakList);
            if (this.valleyList.Count > 0)
                this.flist.AddRange((IEnumerable<MeaureData>)this.valleyList);
            if (this.selectlist.Count > 0)
                this.flist.AddRange((IEnumerable<MeaureData>)this.selectlist);
            this.flist = this.flist.OrderByDescending<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue)).ToList<MeaureData>();
            string cMode = this.currlist[0].MethodPar.C_Mode;
            if (this.flist.Count > 0)
            {
                this.dataGridView1.Visible = false;
                this.dgvPoint.Visible = true;
                this.dgvPoint.Columns["ColPXGD"].HeaderText = cMode;
                this.dgvPoint.Rows.Clear();
                for (int index = 0; index < this.flist.Count; ++index)
                {
                    MeaureData meaureData = this.flist[index];
                    this.dgvPoint.Rows.Add();
                    this.dgvPoint.Rows[index].Cells["ColPWL"].Value = (object)meaureData.xValue;
                    this.dgvPoint.Rows[index].Tag = (object)meaureData;
                    double num1;
                    if (cMode == "Abs")
                    {
                        this.dgvPoint.Rows[index].Cells["ColPXGD"].Value = (object)meaureData.yABS.ToString(this.absacc);
                        num1 = (double)this.bottom - (Convert.ToDouble(meaureData.yABS) - (double)this.ys) * this.yInt;
                    }
                    else
                    {
                        this.dgvPoint.Rows[index].Cells["ColPXGD"].Value = (object)meaureData.YT.ToString(this.tacc);
                        num1 = (double)this.bottom - (Convert.ToDouble(meaureData.YT) - (double)this.ys) * this.yInt;
                    }
                    double num2 = (double)this.left + (Convert.ToDouble(meaureData.xValue) - (double)this.xs) * this.xInt;
                    objGraphics.DrawEllipse(new Pen(Color.Black, 1f), Convert.ToInt32(num2) - 3, Convert.ToInt32(num1) - 3, 6, 6);
                    int? pv = meaureData.PV;
                    string str1;
                    if ((pv.GetValueOrDefault() != 0 ? 0 : (pv.HasValue ? 1 : 0)) != 0)
                    {
                        str1 = "";
                        string str2 = Convert.ToDecimal(meaureData.xValue).ToString("f1");
                        SizeF sizeF = objGraphics.MeasureString(str2, new System.Drawing.Font("Segoe UI", (float)this.FontSize));
                        objGraphics.DrawString(str2, new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Navy), new PointF((float)num2 - sizeF.Width / 2f, (float)(num1 - 20.0)));
                    }
                    else
                    {
                        pv = meaureData.PV;
                        if ((pv.GetValueOrDefault() != 1 ? 0 : (pv.HasValue ? 1 : 0)) != 0)
                        {
                            str1 = "";
                            string str2 = Convert.ToDecimal(meaureData.xValue).ToString("f1");
                            SizeF sizeF = objGraphics.MeasureString(str2, new System.Drawing.Font("Segoe UI", (float)this.FontSize));
                            objGraphics.DrawString(str2, new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.DarkBlue), new PointF((float)num2 - sizeF.Width / 2f, (float)(num1 + 10.0)));
                        }
                        else
                        {
                            str1 = "";
                            string str2 = Convert.ToDecimal(meaureData.xValue).ToString("f1");
                            objGraphics.MeasureString(str2, new System.Drawing.Font("Segoe UI", (float)this.FontSize));
                            objGraphics.DrawString(str2, new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.DarkGreen), new PointF((float)num2, (float)num1));
                        }
                    }
                }
                if (this.flist.Count >= this.dgvcnt)
                    return;
                this.dgvPoint.Rows.Add(this.dgvcnt - this.flist.Count);
            }
            else if (this.SquareMode != 1)
            {
                this.dgvPoint.Visible = false;
                this.dataGridView1.Visible = true;
            }
        }
        private void DrawScale(ref Graphics objGraphics, int listcnt, string C_mode)
        {
            objGraphics.FillRectangle((Brush)new SolidBrush(Color.White), this.left, this.top, this.right - this.left, this.bottom - this.top);
            objGraphics.DrawLine(new Pen(Color.Black, 1f), this.left, this.bottom, this.right, this.bottom);
            objGraphics.DrawLine(new Pen(Color.Black, 1f), this.left, this.top, this.right, this.top);
            objGraphics.DrawLine(new Pen(Color.Black, 1f), this.left, this.bottom, this.left, this.top);
            objGraphics.DrawLine(new Pen(Color.Black, 1f), this.right, this.bottom, this.right, this.top);
            float left = this.left;
            float y1 = this.bottom + 5f;
            objGraphics.DrawString(this.xs.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(left, y1));
            float x1 = this.right - objGraphics.MeasureString(this.xe.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)this.FontSize)).Width;
            objGraphics.DrawString(this.xe.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(x1, y1));
            SizeF sizeF1 = objGraphics.MeasureString("WL.(нм)", new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            SizeF sizeF2 = objGraphics.MeasureString(C_mode, new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            string format;
            if (this.showpar.AutoXY)
            {
                format = !(C_mode == "Abs") ? this.tacc : this.absacc;
            }
            else
            {
                string str1 = this.showpar.yMax.ToString("f10").TrimEnd('0');
                string str2 = this.showpar.yMin.ToString("f10").TrimEnd('0');
                string str3 = str1.Substring(str1.IndexOf('.') + 1);
                string str4 = str2.Substring(str2.IndexOf('.') + 1);
                format = str3.Length < str4.Length ? "f" + (object)str4.Length : "f" + (object)str3.Length;
            }
            SizeF sizeF3 = objGraphics.MeasureString(this.ys.ToString(format), new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            float x2 = this.left - sizeF3.Width;
            float y2 = this.bottom - sizeF3.Height / 2f;
            objGraphics.DrawString(this.ys.ToString(format), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(x2, y2));
            SizeF sizeF4 = objGraphics.MeasureString(this.ye.ToString(format), new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            float x3 = this.left - sizeF4.Width;
            float y3 = this.top - sizeF4.Height / 2f;
            objGraphics.DrawString(this.ye.ToString(format), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(x3, y3));
            for (int index = 1; index < 4; ++index)
            {
                Pen pen = new Pen(Color.Black, 1f);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                objGraphics.DrawLine(pen, this.left + (float)(((double)this.right - (double)this.left) * (double)index / 4.0), this.bottom, this.left + (float)(((double)this.right - (double)this.left) * (double)index / 4.0), this.top);
                objGraphics.DrawLine(pen, this.left, this.top + (float)(((double)this.bottom - (double)this.top) * (double)index / 4.0), this.right, this.top + (float)(((double)this.bottom - (double)this.top) * (double)index / 4.0));
            }
            float x4 = this.left + (float)(((double)this.right - (double)this.left - (double)sizeF1.Width) / 2.0);
            float y4 = this.bottom + 5f;
            objGraphics.DrawString("WL.(нм)", new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(x4, y4));
            float x5 = this.left - objGraphics.MeasureString(C_mode, new System.Drawing.Font("Segoe UI", (float)this.FontSize)).Width;
            float y5 = this.top + (float)(((double)this.bottom - (double)this.top - (double)sizeF2.Height) / 2.0);
            objGraphics.DrawString(C_mode, new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(x5, y5));
        }

        private void setState(ComStatus ss)
        {
            switch (ss)
            {
                case ComStatus.MEASURE:
                    this.btnScan.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = false;
                    this.btnBlank.Enabled = false;
                    // this.btnOperate.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnOpen.Enabled = false;
                    //this.btnSample.Enabled = false;
                    //this.btnSearch.Enabled = false;
                    //this.panel1.Enabled = false;
                    break;
                case ComStatus.CALBGND:
                    this.btnBlank.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = false;
                    this.btnScan.Enabled = false;
                    // this.btnOperate.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnOpen.Enabled = false;
                    //   this.btnSample.Enabled = false;
                    //    this.btnSearch.Enabled = false;
                    //this.panel1.Enabled = false;
                    break;
                case ComStatus.END:
                    this.btnScan.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = true;
                    this.btnBlank.Enabled = true;
                    //this.btnOperate.Enabled = true;
                    this.btnSave.Enabled = true;
                    this.btnOpen.Enabled = true;
                    //    this.btnSample.Enabled = true;
                    //   this.btnSearch.Enabled = true;
                    //this.panel1.Enabled = true;
                    break;
            }
        }


        private void BtnScan_Click(object sender, EventArgs e)
        {

        }

        private void BtnScale_Click(object sender, EventArgs e)
        {

        }

        private void BtnRetrieval_Click(object sender, EventArgs e)
        {

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            /*if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                //if (new DRMessageBoxFrm(CommonFun.GetLanText("exitconfirm"), "Warning").ShowDialog() != DialogResult.Yes)
                 //   return;
                if (this.currlist != null && this.currlist.Count > 0 && this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.status)).ToList<SpectraScan>().Count > 0)
                {
                    CommonFun.showbox(CommonFun.GetLanText("datasaveexit"), "Warning");
                    return;
                }
            }
            else if ((this.currlist == null || this.currlist.Count > 0) && (this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.status)).ToList<SpectraScan>().Count > 0 && new DRMessageBoxFrm(CommonFun.GetLanText("unsavedataexit"), "Warning").ShowDialog() == DialogResult.No))
                return;
            else if ((this.currlist == null || this.currlist.Count > 0) && (this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.status)).ToList<SpectraScan>().Count > 0))
                return;*/
            try
            {
                CommonFun.setXmlValue("MeasureParams", "C_BeginWL", this.mpar.C_BeginWL);
                CommonFun.setXmlValue("MeasureParams", "C_EndWL", this.mpar.C_EndWL);
                CommonFun.setXmlValue("MeasureParams", "C_Interval", this.mpar.C_Interval);
                CommonFun.setXmlValue("MeasureParams", "C_Intervals", this.mpar.C_Intervals);
                if (this.mpar.C_Mode == CommonFun.GetLanText("Abs"))
                    CommonFun.setXmlValue("MeasureParams", "C_Mode", "Abs");
                else
                    CommonFun.setXmlValue("MeasureParams", "C_Mode", "T");
                CommonFun.setXmlValue("MeasureParams", "C_ScanCNT", this.mpar.C_ScanCNT);
                CommonFun.setXmlValue("MeasureParams", "C_ScanSpeed", this.mpar.C_ScanSpeedDM);
                CommonFun.setXmlValue("MeasureParams", "C_SLength", this.mpar.C_SLength);
                CommonFun.setXmlValue("MeasureParams", "C_StepLen", this.mpar.C_StepLen);
                CommonFun.setXmlValue("ShowParams", "AutoPrint", this.showpar.AutoPrint.ToString());
                CommonFun.setXmlValue("ShowParams", "AutoSave", this.showpar.AutoSave.ToString());
                bool flag = this.showpar.AutoXY;
                CommonFun.setXmlValue("ShowParams", "AutoXY", flag.ToString());
                CommonFun.setXmlValue("ShowParams", "MulShow", this.showpar.MulShow.ToString());
                float num = this.showpar.xMax;
                CommonFun.setXmlValue("ShowParams", "xMax", num.ToString());
                num = this.showpar.xMin;
                CommonFun.setXmlValue("ShowParams", "xMin", num.ToString());
                num = this.showpar.yMax;
                CommonFun.setXmlValue("ShowParams", "yMax", num.ToString());
                num = this.showpar.yMin;
                CommonFun.setXmlValue("ShowParams", "yMin", num.ToString());
                /*CommonFun.setXmlValue("PrintParams", "Addtional", this.printpar.Addtional);
                CommonFun.setXmlValue("PrintParams", "ComImage", this.printpar.ComImage);
                CommonFun.setXmlValue("PrintParams", "Describtion", this.printpar.Describtion);
                flag = this.printpar.ShowAddtional;
                CommonFun.setXmlValue("PrintParams", "ShowAddtional", flag.ToString());
                flag = this.printpar.ShowComImage;
                CommonFun.setXmlValue("PrintParams", "ShowComImage", flag.ToString());
                flag = this.printpar.ShowDes;
                CommonFun.setXmlValue("PrintParams", "ShowDes", flag.ToString());
                flag = this.printpar.ShowInsAndUser;
                CommonFun.setXmlValue("PrintParams", "ShowInsAndUser", flag.ToString());
                flag = this.printpar.ShowPeak;
                CommonFun.setXmlValue("PrintParams", "ShowPeak", flag.ToString());
                flag = this.printpar.ShowValley;
                CommonFun.setXmlValue("PrintParams", "ShowValley", flag.ToString());
                CommonFun.setXmlValue("PrintParams", "Title", this.printpar.Title);*/
            }
            catch
            {
                CommonFun.setXmlValue("MeasureParams", "C_BeginWL", "650.0");
                CommonFun.setXmlValue("MeasureParams", "C_EndWL", "235.0");
                CommonFun.setXmlValue("MeasureParams", "C_Interval", "0");
                CommonFun.setXmlValue("MeasureParams", "C_Intervals", "0");
                CommonFun.setXmlValue("MeasureParams", "C_Mode", "T");
                CommonFun.setXmlValue("MeasureParams", "C_ScanCNT", "1");
                CommonFun.setXmlValue("MeasureParams", "C_ScanSpeed", "Средне");
                CommonFun.setXmlValue("MeasureParams", "C_SLength", "10");
                CommonFun.setXmlValue("MeasureParams", "C_StepLen", "1.0");
                CommonFun.setXmlValue("ShowParams", "AutoPrint", "False");
                CommonFun.setXmlValue("ShowParams", "AutoSave", "False");
                CommonFun.setXmlValue("ShowParams", "AutoXY", "True");
                CommonFun.setXmlValue("ShowParams", "MulShow", "0");
                CommonFun.setXmlValue("ShowParams", "xMax", "650.0");
                CommonFun.setXmlValue("ShowParams", "xMin", "235.0");
                CommonFun.setXmlValue("ShowParams", "yMax", "99.95");
                CommonFun.setXmlValue("ShowParams", "yMin", "100.05");
                /*CommonFun.setXmlValue("PrintParams", "Addtional", "");
                CommonFun.setXmlValue("PrintParams", "ComImage", "");
                CommonFun.setXmlValue("PrintParams", "Describtion", "");
                CommonFun.setXmlValue("PrintParams", "ShowAddtional", "False");
                CommonFun.setXmlValue("PrintParams", "ShowComImage", "False");
                CommonFun.setXmlValue("PrintParams", "ShowDes", "False");
                CommonFun.setXmlValue("PrintParams", "ShowInsAndUser", "True");
                CommonFun.setXmlValue("PrintParams", "ShowPeak", "False");
                CommonFun.setXmlValue("PrintParams", "ShowValley", "False");
                CommonFun.setXmlValue("PrintParams", "Title", "");*/
            }
            if (this.sp.IsOpen)
            {
                this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                CommonFun.WriteSendLine("Сканирование спектра, возврат，SCAN_STOPPING 0");
                this.sp.Close();
            }
            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();
            Hide();
            //      CommonFun.WriteLine("Получаем меню");
            MenuProgram menuProgram = new MenuProgram();
            //     CommonFun.WriteLine("Выводим меню");
            menuProgram.Show();
            //    CommonFun.WriteLine("Получаем родительское окно");
            this.Close(); this.Dispose();
        }

        private string getName(OperateType? type, string SourceName)
        {
            string name = "";
            if (!type.HasValue)
            {
                name = this.lblSample.Tag == null ? "SpectrumScan" : this.lblSample.Tag.ToString();
                int num = 0;
                if (this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.status)).Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.opertype.HasValue)).ToList<SpectraScan>().Count > 0)
                {
                    foreach (string str in this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.opertype.HasValue)).Select<SpectraScan, string>((Func<SpectraScan, string>)(s => s.C_name)))
                    {
                        if (str.LastIndexOf("-") >= 0)
                        {
                            try
                            {
                                int int16 = (int)Convert.ToInt16(str.Substring(str.LastIndexOf("-") + 1));
                                if (int16 > num)
                                    num = int16;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                name = num != 0 ? name + "-" + (num + 1).ToString() : CommonFun.getName(new OperateType?(), "");
            }
            else
            {
                OperateType? nullable1 = type;
                ref OperateType? local = ref nullable1;
                OperateType valueOrDefault = local.GetValueOrDefault();
                if (local.HasValue)
                {
                    switch (valueOrDefault)
                    {
                        case OperateType.Smooth:
                            name = SourceName + "-S";
                            goto label_34;
                        case OperateType.AVG:
                            name = SourceName + "-AVG";
                            goto label_34;
                        case OperateType.FirstDeri:
                            name = SourceName + "-1d";
                            goto label_34;
                        case OperateType.SecDeri:
                            name = SourceName + "-2d";
                            goto label_34;
                        case OperateType.ThirdDeri:
                            name = SourceName + "-3d";
                            goto label_34;
                        case OperateType.FourthDeri:
                            name = SourceName + "-4d";
                            goto label_34;
                        case OperateType.SAddR:
                            name = SourceName + "-AddR";
                            goto label_34;
                        case OperateType.SAddS:
                            name = SourceName + "-AddS";
                            goto label_34;
                        case OperateType.SSubR:
                            name = SourceName + "-SubR";
                            goto label_34;
                        case OperateType.SSubS:
                            name = SourceName + "-SubS";
                            goto label_34;
                        case OperateType.SMulR:
                            name = SourceName + "-MulR";
                            goto label_34;
                        case OperateType.SMulS:
                            name = SourceName + "-MulS";
                            goto label_34;
                        case OperateType.SDivR:
                            name = SourceName + "-DivR";
                            goto label_34;
                        case OperateType.SDivS:
                            name = SourceName + "-DivS";
                            goto label_34;
                        case OperateType.NC:
                            name = SourceName + "-NC";
                            goto label_34;
                        case OperateType.Square:
                            name = SourceName + "-Squa";
                            goto label_34;
                    }
                }
                name = SourceName;
            label_34:
                if (this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => !s.status)).Where<SpectraScan>((Func<SpectraScan, bool>)(s =>
                {
                    int num;
                    if (s.C_name.Contains(name) && s.opertype.HasValue)
                    {
                        OperateType? opertype = s.opertype;
                        OperateType? nullable = type;
                        num = opertype.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (opertype.HasValue == nullable.HasValue ? 1 : 0);
                    }
                    else
                        num = 0;
                    return num != 0;
                })).ToList<SpectraScan>().Count > 0)
                {
                    int count = this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s =>
                    {
                        int num;
                        if (s.C_name.Contains(name) && s.opertype.HasValue)
                        {
                            OperateType? opertype = s.opertype;
                            OperateType? nullable = type;
                            num = opertype.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (opertype.HasValue == nullable.HasValue ? 1 : 0);
                        }
                        else
                            num = 0;
                        return num != 0;
                    })).ToList<SpectraScan>().Count;
                    name = name + "-" + (count + 1).ToString();
                }
                else
                    name = CommonFun.getName(type, SourceName);
            }
            return name;
        }

        private System.Drawing.Color GetColor()
        {
            System.Drawing.Color color1 = System.Drawing.Color.Red;
            foreach (System.Drawing.Color color2 in CommonFun.Colorlist())
            {
                System.Drawing.Color cl = color2;
                if (this.currlist.Where<SpectraScan>((Func<SpectraScan, bool>)(s => s.color == cl)).ToList<SpectraScan>().Count == 0)
                {
                    color1 = cl;
                    break;
                }
            }
            return color1;
        }

        private void SetWL(string strwl, string wlvalue)
        {
            this.lblWL.Text = strwl + " нм";
            if (wlvalue != "")
                this.lblWL.Text = strwl + " нм," + wlvalue;
            else
                this.lblWL.Text = strwl + " нм";
        }

        private void SetStop()
        {
            this.btnScan.Text = "Измерение";
            //this.panel4.Visible = false;
            this.progressBar1.Value = 0;
            this.setState(ComStatus.END);
        }

        private void SetTextMessage(int ipos)
        {
            if (ipos > 100)
                ipos = 100;
            this.progressBar1.Value = Convert.ToInt32(ipos);
        }

        private void Setblanklabel()
        {
            this.btnBlank.Text = "Обнуление";
            //this.panel4.Visible = false;
            this.progressBar1.Value = 0;
            this.setState(ComStatus.END);
        }


        private void DealMeaData()
        {
            this.XYMaxMin();
            if (this.showpar.AutoXY)
                this.AddToRangeList(Convert.ToSingle(this.mpar.C_EndWL), Convert.ToSingle(this.mpar.C_BeginWL), this.yMin, this.yMax);
            else
                this.AddToRangeList(this.showpar.xMin, this.showpar.xMax, this.showpar.yMin, this.showpar.yMax);
            if (this.ComSta != ComStatus.END)
                return;
            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[0].Cells[0];
        }
        private void SetSname() => this.lblSample.Text = this.currlist[0].C_name;

        private delegate void Del_SetStop();

        private delegate void Del_SetBlankLabel();

        private delegate void Del_AutoPrint();

        private delegate void SetPos(int ipos);

        private delegate void Del_setstate(bool status);

        private delegate void Del_starttt(bool status);

        private delegate void Del_SetWL(string strwl, string value);

        private void BtnTable_Click(object sender, EventArgs e)
        {

        }

        //private readonly List<SpectrWLTable> _itemList = new List<SpectrWLTable>();
        //   public List<SpectrWLTable> MyTableList { get { return _itemList; } }

        private void BindDataWhileScan(MeaureData md)
        {
            try
            {
                this.dataGridView1.Rows.Add();
                int count = this.currlist[0].Data.Count;
                if (this.dataGridView1.Rows.Count < count)
                    return;
                this.dataGridView1.Rows[count - 1].Cells["ColWL"].Value = (object)md.xValue;
                this.dataGridView1.Rows[count - 1].Cells["ColXGD"].Value = !(this.mpar.C_Mode == "Abs") ? (object)md.YT.ToString(this.tacc) : (object)md.yABS.ToString(this.absacc);
                this.dataGridView1.Rows[count - 1].Tag = (object)md;
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[count - 1].Cells[0];

                // MyTableList.Add(new SpectrWLTable(MyTableList.Count() - 1, md.xValue, md.yABS.ToString(this.absacc), md.YT.ToString(this.tacc)));

                /*if(MyTableList.Count() > 100)
                {
                    for (int i = 0; i < MyTableList.Count(); i++)
                    {
                        //this.dataGridView1.Rows.Clear();
                        this.dataGridView1.Columns[2].HeaderText = this.mpar.C_Mode;
                        dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells["ColWL"].Value = MyTableList[i].WL;
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells["ColXGD"].Value = !(this.mpar.C_Mode == "Abs") ? MyTableList[this.dataGridView1.Rows.Count - 1].TProcent : MyTableList[i].Abs;
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Tag = MyTableList[this.dataGridView1.Rows.Count - 1];
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0];
                    }
                }*/
            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.ToString(), "Error");
            }


        }

        private delegate void Del_MeaData();

        private delegate void Del_BindData(MeaureData md);

        private delegate void Del_SamNameSet();

        string filepath;
        string pathTemp = System.IO.Path.GetTempPath();
        string extension = ".sscan";
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                CommonFun.showbox("Измерений не было, сохранять нечего", "Info");
            }
            else
            {
                using (SaveFrm save = new SaveFrm(extension, "Спектральный анализ"))
                {

                    save.btnOK.Click += ((param0_1, param1_1) =>
                    {


                        if (save.Name_file.Text.Length > 0)
                        {
                            filepath = Directory.GetCurrentDirectory() + @"\Сохраненные измерения";
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            filepath = filepath + @"\" + save.Name_file.Text + extension;

                            if (!File.Exists(filepath))
                            {
                                //save_name = filepath;
                                CreateXMLDocumentIzmerenieScan();
                                WriteXmlIzmerenieFR();
                                EncriptorFileBase64 encriptorFileBase64 = new EncriptorFileBase64(filepath, pathTemp);
                                save.Dispose();
                            }
                            else
                            {
                                CommonFun.showbox("Файл с таким именем уже существует.", "Info");
                            }

                        }
                    });
                    save.ShowDialog();
                }
            }
        }
        public void CreateXMLDocumentIzmerenieScan()
        {


            XmlTextWriter xtw = new XmlTextWriter(filepath, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("Data_Izmerenie");
            xtw.WriteEndDocument();
            xtw.Close();


        }

        public void WriteXmlIzmerenieFR()
        {
            XmlDocument xd = new XmlDocument();
            FileStream fs = new FileStream(filepath, FileMode.Open);
            xd.Load(fs);

            XmlNode Settings = xd.CreateElement("Settings");

            XmlNode C_BeginWL = xd.CreateElement("C_BeginWL"); //длина волны
            C_BeginWL.InnerText = this.mpar.C_BeginWL; // и значение
            Settings.AppendChild(C_BeginWL); // и указываем кому принадлежит 

            XmlNode C_EndWL = xd.CreateElement("C_EndWL"); //длина волны
            C_EndWL.InnerText = this.mpar.C_EndWL; // и значение
            Settings.AppendChild(C_EndWL); // и указываем кому принадлежит

            XmlNode C_Interval = xd.CreateElement("C_Interval"); //длина волны
            C_Interval.InnerText = this.mpar.C_Interval; // и значение
            Settings.AppendChild(C_Interval); // и указываем кому принадлежит 

            XmlNode C_Intervals = xd.CreateElement("C_Intervals"); //длина волны
            C_Intervals.InnerText = this.mpar.C_Intervals; // и значение
            Settings.AppendChild(C_Intervals); // и указываем кому принадлежит

            XmlNode C_Mode = xd.CreateElement("C_Mode"); //длина волны
            C_Mode.InnerText = this.mpar.C_Mode; // и значение
            Settings.AppendChild(C_Mode); // и указываем кому принадлежит 

            XmlNode C_SLength = xd.CreateElement("C_SLength"); //длина волны
            C_SLength.InnerText = this.mpar.C_SLength; // и значение
            Settings.AppendChild(C_SLength); // и указываем кому принадлежит 

            XmlNode C_ScanSpeed = xd.CreateElement("C_ScanSpeed"); //длина волны
            C_ScanSpeed.InnerText = this.mpar.C_ScanSpeed; // и значение
            Settings.AppendChild(C_ScanSpeed); // и указываем кому принадлежит 

            XmlNode C_ScanSpeedDM = xd.CreateElement("C_ScanSpeedDM"); //длина волны
            C_ScanSpeedDM.InnerText = this.mpar.C_ScanSpeed; // и значение
            Settings.AppendChild(C_ScanSpeedDM); // и указываем кому принадлежит 

            XmlNode C_StepLen = xd.CreateElement("C_StepLen"); //длина волны
            C_StepLen.InnerText = this.mpar.C_StepLen.ToString(); // и значение
            Settings.AppendChild(C_StepLen); // и указываем кому принадлежит 

            XmlNode C_ScanCNT = xd.CreateElement("C_ScanCNT"); //длина волны
            C_ScanCNT.InnerText = this.mpar.C_ScanCNT; // и значение
            Settings.AppendChild(C_ScanCNT); // и указываем кому принадлежит 


            XmlNode D_Mtime = xd.CreateElement("D_Mtime"); // дата создания настроек
            D_Mtime.InnerText = this.mpar.D_Mtime.ToString(); // и значение
            Settings.AppendChild(D_Mtime); // и указываем кому принадлежит

            xd.DocumentElement.AppendChild(Settings);

            XmlNode Izmerenie = xd.CreateElement("Izmerenie");

            XmlNode DateTime1 = xd.CreateElement("DateTime"); // дата создания градуировки
            DateTime1.InnerText = DateTime.Now.ToString(); // и значение
            Izmerenie.AppendChild(DateTime1); // и указываем кому принадлежит

            string[] HeaderCells = new string[dataGridView1.Columns.Count];
            string[,] Cells1 = new string[dataGridView1.Rows.Count, dataGridView1.Columns.Count];

            string[] HeaderCells_1 = new string[dataGridView2.Columns.Count];
            string[,] Cells1_1 = new string[dataGridView2.Rows.Count, dataGridView2.Columns.Count];

            string[] HeaderCells_2 = new string[dgvPoint.Columns.Count];
            string[,] Cells1_2 = new string[dgvPoint.Rows.Count, dgvPoint.Columns.Count];

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].HeaderText.ToString() != "")
                {
                    HeaderCells[i] = dataGridView1.Columns[i].HeaderText.ToString(); // и значение
                }
                else
                {
                    HeaderCells[i] = "-";
                }
            }

            for (int i = 0; i < dataGridView2.Columns.Count; i++)
            {
                if (dataGridView2.Columns[i].HeaderText.ToString() != "")
                {
                    HeaderCells_1[i] = dataGridView2.Columns[i].HeaderText.ToString(); // и значение
                }
                else
                {
                    HeaderCells_1[i] = "-";
                }
            }
            for (int i = 0; i < dgvPoint.Columns.Count; i++)
            {
                if (dgvPoint.Columns[i].HeaderText.ToString() != "")
                {
                    HeaderCells_2[i] = dgvPoint.Columns[i].HeaderText.ToString(); // и значение
                }
                else
                {
                    HeaderCells_2[i] = "-";
                }
            }

            XmlNode Izmerenie_table = xd.CreateElement("dataGridView1");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                XmlNode Cells2 = xd.CreateElement("Stroka");

                XmlAttribute attribute1 = xd.CreateAttribute("Nomer");
                attribute1.Value = Convert.ToString(i); // устанавливаем значение атрибута
                Cells2.Attributes.Append(attribute1); // добавляем атрибут

                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        Cells1[i, j] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    else
                        Cells1[i, j] = "-";

                    XmlNode HeaderCells1 = xd.CreateElement("Stolbec"); // Столбец
                    if (Cells1[i, j] != "")
                    {
                        HeaderCells1.InnerText = Cells1[i, j]; // и значение
                    }
                    else
                    {
                        HeaderCells1.InnerText = "-";
                    }
                    Cells2.AppendChild(HeaderCells1); // и указываем кому принадлежит
                    XmlAttribute attribute = xd.CreateAttribute("Header");
                    attribute.Value = HeaderCells[j]; // устанавливаем значение атрибута                    
                    HeaderCells1.Attributes.Append(attribute); // добавляем атрибут     
                }
                Izmerenie_table.AppendChild(Cells2);
            }
            xd.DocumentElement.AppendChild(Izmerenie_table);

            Izmerenie_table = xd.CreateElement("dataGridView2");
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                XmlNode Cells2 = xd.CreateElement("Stroka");

                XmlAttribute attribute1 = xd.CreateAttribute("Nomer");
                attribute1.Value = Convert.ToString(i); // устанавливаем значение атрибута
                Cells2.Attributes.Append(attribute1); // добавляем атрибут

                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                {
                    if (dataGridView2.Rows[i].Cells[j].Value != null)
                        Cells1_1[i, j] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                    else
                        Cells1_1[i, j] = "-";

                    XmlNode HeaderCells1 = xd.CreateElement("Stolbec"); // Столбец
                    if (Cells1_1[i, j] != "")
                    {
                        HeaderCells1.InnerText = Cells1_1[i, j]; // и значение
                    }
                    else
                    {
                        HeaderCells1.InnerText = "-";
                    }
                    Cells2.AppendChild(HeaderCells1); // и указываем кому принадлежит
                    XmlAttribute attribute = xd.CreateAttribute("Header");
                    attribute.Value = HeaderCells_1[j]; // устанавливаем значение атрибута                    
                    HeaderCells1.Attributes.Append(attribute); // добавляем атрибут     
                }
                Izmerenie_table.AppendChild(Cells2);
            }
            xd.DocumentElement.AppendChild(Izmerenie_table);


            Izmerenie_table = xd.CreateElement("dgvPoint");
            for (int i = 0; i < dgvPoint.Rows.Count; i++)
            {
                XmlNode Cells2 = xd.CreateElement("Stroka");

                XmlAttribute attribute1 = xd.CreateAttribute("Nomer");
                attribute1.Value = Convert.ToString(i); // устанавливаем значение атрибута
                Cells2.Attributes.Append(attribute1); // добавляем атрибут

                for (int j = 0; j < dgvPoint.Columns.Count; j++)
                {
                    if (dgvPoint.Rows[i].Cells[j].Value != null)
                        Cells1_2[i, j] = dgvPoint.Rows[i].Cells[j].Value.ToString();
                    else
                        Cells1_2[i, j] = "-";

                    XmlNode HeaderCells1 = xd.CreateElement("Stolbec"); // Столбец
                    if (Cells1_2[i, j] != "")
                    {
                        HeaderCells1.InnerText = Cells1_2[i, j]; // и значение
                    }
                    else
                    {
                        HeaderCells1.InnerText = "-";
                    }
                    Cells2.AppendChild(HeaderCells1); // и указываем кому принадлежит
                    XmlAttribute attribute = xd.CreateAttribute("Header");
                    attribute.Value = HeaderCells_2[j]; // устанавливаем значение атрибута                    
                    HeaderCells1.Attributes.Append(attribute); // добавляем атрибут     
                }
                Izmerenie_table.AppendChild(Cells2);
            }
            xd.DocumentElement.AppendChild(Izmerenie_table);


            fs.Close();         // Закрываем поток  
            xd.Save(filepath); // Сохраняем файл  

        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            Open_File();

        }
        bool shifrTrueFalse = false;
        public void Open_File()
        {
            // this.timer1.Stop();
            //ResetTempReDraw(0);
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Сохраненные измерения");

            OpenFrm openFrm = new OpenFrm(Directory.GetCurrentDirectory() + @"\Сохраненные измерения", ".sscan");
            openFrm.ShowDialog();
            if (openFrm.open_name != null)
            {
                filepath = Directory.GetCurrentDirectory() + "/Сохраненные измерения/" + openFrm.open_name;
                Decriptor64 decriptorfile = new Decriptor64(filepath, pathTemp, ref shifrTrueFalse);
                if (shifrTrueFalse == false)
                {
                    CommonFun.showbox("Формат файла не поддерживается!", "Info");
                    return;
                }
                else
                {
                    ResetTempReDraw(0);
                    dataGridView1.Rows.Clear();
                    dataGridView2.Rows.Clear();
                    dgvPoint.Rows.Clear();
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(pathTemp + "/" + openFrm.open_name);

                    System.Xml.Linq.XDocument xdoc = XDocument.Load(pathTemp + "/" + openFrm.open_name);
                    XmlNodeList nodes = xDoc.ChildNodes;
                    foreach (XElement IzmerenieElement in xdoc.Element("Data_Izmerenie").Elements("NumberIzmer"))
                    {
                        XElement countIzmer1Element = IzmerenieElement.Element("countIzmer1");

                        if (countIzmer1Element != null)
                        {
                            //  RowsCount = countIzmer1Element.Value;
                        }
                    }

                    // Читаем в цикле вложенные значения таблиц
                    foreach (XmlNode n in nodes)
                    {
                        if ("Data_Izmerenie".Equals(n.Name))
                        {

                            this.sslive = new SpectraScan();
                            this.sslive.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                            this.sslive.Serials = CommonFun.GetAppConfig("serialno");
                            this.sslive.MethodPar = this.mpar;
                            this.sslive.C_name = this.getName(new OperateType?(), "");
                            this.sslive.C_Operator = CommonFun.GetAppConfig("currentuser");
                            this.sslive.color = this.GetColor();
                            this.sslive.D_Time = DateTime.Now;
                            this.sslive.Data = new List<MeaureData>();
                            this.sslive.status = false;
                            this.sslive.IsShow = 1;

                            if (this.currlist.Count > 0)
                            {
                                //this.sslive.IsShow = 1;
                                this.currlist.Add(this.sslive);
                                for (int index = this.currlist.Count - 1; index > 0; --index)
                                {
                                    this.currlist[index] = this.currlist[index - 1];
                                    if (this.showpar.MulShow == 0)
                                        this.currlist[index].IsShow = 0;
                                }
                                this.currlist[0] = this.sslive;
                            }
                            else
                            {
                                this.currlist.Add(this.sslive);
                                //  this.sslive.IsShow = 1;
                            }
                            if (this.lblSample.InvokeRequired)
                                this.lblSample.Invoke((Delegate)new Del_SamNameSet(this.SetSname));
                            else
                                this.lblSample.Text = this.sslive.C_name;

                            for (XmlNode d = n.FirstChild; d != null; d = d.NextSibling)
                            {
                                if ("Settings".Equals(d.Name))
                                {
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("C_BeginWL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_BeginWL = k.FirstChild.Value;
                                            start_wl = this.mpar.C_BeginWL;
                                        }
                                        if ("C_EndWL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_EndWL = k.FirstChild.Value;
                                            cancel_wl = this.mpar.C_EndWL;
                                        }
                                        if ("C_Interval".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_Interval = k.FirstChild.Value;
                                            time_interval = this.mpar.C_Interval;
                                        }
                                        if ("C_Intervals".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_Intervals = k.FirstChild.Value;
                                        }
                                        if ("C_Mode".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_Mode = k.FirstChild.Value;

                                            if (this.mpar.C_Mode == "Abs")
                                            {
                                                lblmode.Text = "Abs";
                                                ColPXGD.HeaderText = "Abs";
                                                photometric_mode = "Абсорбция (Abs)";
                                            }
                                            else
                                            {
                                                lblmode.Text = "%T";
                                                ColPXGD.HeaderText = "%T";
                                                photometric_mode = "Коэффициент пропускания (%T)";
                                            }
                                        }
                                        if ("C_SLength".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_SLength = k.FirstChild.Value;
                                            // time_interval = this.mpar.C_SLength;
                                        }
                                        if ("C_ScanSpeed".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_ScanSpeed = k.FirstChild.Value;
                                            speed_measure = this.mpar.C_ScanSpeed;
                                        }
                                        if ("C_ScanSpeedDM".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_ScanSpeedDM = k.FirstChild.Value;
                                        }
                                        if ("C_StepLen".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_StepLen = k.FirstChild.Value;
                                            step_interval = this.mpar.C_StepLen;
                                        }
                                        if ("C_ScanCNT".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.C_ScanCNT = k.FirstChild.Value;
                                            loop_measure = Convert.ToInt32(this.mpar.C_ScanCNT);
                                        }
                                        if ("D_Mtime".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.D_Mtime = DateTime.Parse(k.FirstChild.Value);
                                           // loop_measure = Convert.ToInt32(this.mpar.D_Mtime);
                                        }

                                    }
                                }


                                // Обрабатываем в цикле только dataGridView1
                                if ("dataGridView1".Equals(d.Name))
                                {
                                    if (this.currlist.Count > 0)
                                    {
                                        if (this.currlist[0].Data == null)
                                            this.currlist[0].Data = new List<MeaureData>();
                                    }
                                    else
                                    {
                                        this.sslive = new SpectraScan();
                                        this.sslive.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                                        this.sslive.Serials = CommonFun.GetAppConfig("serialno");
                                        this.sslive.MethodPar = this.mpar;
                                        this.sslive.C_name = this.getName(new OperateType?(), "");
                                        this.sslive.C_Operator = CommonFun.GetAppConfig("currentuser");
                                        this.sslive.color = this.GetColor();
                                        this.sslive.D_Time = DateTime.Now;
                                        this.sslive.Data = new List<MeaureData>();
                                        this.sslive.status = false;
                                        this.sslive.IsShow = 1;

                                        this.currlist.Add(this.sslive);
                                    }
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("Stroka".Equals(k.Name))
                                        {
                                            int stolbec = 0;
                                            dataGridView1.Rows.Add(1);
                                            //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                            List<object> value_xml = new List<object>();
                                            List<object> attribute_xml = new List<object>();
                                            for (XmlNode m = k.FirstChild; m != null; m = m.NextSibling)
                                            {
                                                if ("Stolbec".Equals(m.Name) && m.FirstChild != null)
                                                {

                                                    value_xml.Add(m.FirstChild.Value);
                                                    XmlAttributeCollection attrColl = m.Attributes;
                                                    attribute_xml.Add(attrColl[0].Value);

                                                }
                                            }
                                            for (int i = 0; i < value_xml.Count(); i++)
                                            {

                                                if (value_xml[i].ToString() != "-")
                                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[i].Value = value_xml[i].ToString();
                                                //    else
                                                //      dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[j].Value = null;



                                            }
                                            dataGridView1.Columns[0].HeaderText = "";
                                            dataGridView1.Columns[1].HeaderText = attribute_xml[1].ToString();
                                            dataGridView1.Columns[2].HeaderText = attribute_xml[2].ToString();

                                            MeaureData md = new MeaureData();
                                            md.xValue = value_xml[1].ToString();
                                            md.yABS = (float)Convert.ToDouble(value_xml[2]);
                                            md.YT = (float)Convert.ToDouble(value_xml[2]);


                                            this.currlist[0].Data.Add(md);

                                        }

                                    }
                                    //this.currlist[0].Data.OrderBy<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue));
                                    if (this.picCurve.InvokeRequired)
                                    {
                                        this.picCurve.Invoke((Delegate)new Del_MeaData(this.DealMeaData));
                                    }
                                    else
                                    {
                                        this.XYMaxMin();
                                        if (this.showpar.AutoXY)
                                            this.AddToRangeList(Convert.ToSingle(this.mpar.C_EndWL), Convert.ToSingle(this.mpar.C_BeginWL), this.yMin, this.yMax);
                                        else
                                            this.AddToRangeList(this.showpar.xMin, this.showpar.xMax, this.showpar.yMin, this.showpar.yMax);
                                    }



                                }


                                // Обрабатываем в цикле только dataGridView2
                                if ("dataGridView2".Equals(d.Name))
                                {
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("Stroka".Equals(k.Name))
                                        {
                                            dataGridView2.Rows.Add(1);
                                            int stolbec = 0;
                                            //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                            List<object> value_xml = new List<object>();
                                            List<object> attribute_xml = new List<object>();
                                            for (XmlNode m = k.FirstChild; m != null; m = m.NextSibling)
                                            {
                                                if ("Stolbec".Equals(m.Name) && m.FirstChild != null)
                                                {

                                                    value_xml.Add(m.FirstChild.Value);
                                                    XmlAttributeCollection attrColl = m.Attributes;
                                                    attribute_xml.Add(attrColl[0].Value);

                                                }
                                            }
                                            for (int i = 0; i < value_xml.Count(); i++)
                                            {


                                                if (value_xml[i].ToString() != "-")
                                                    dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[i].Value = value_xml[i].ToString();
                                                //         else
                                                //           dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[j].Value = null;
                                            }
                                        }


                                    }

                                }
                                // Обрабатываем в цикле только dgvPoint
                                if ("dgvPoint".Equals(d.Name))
                                {
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("Stroka".Equals(k.Name))
                                        {
                                            int stolbec = 0;
                                            dgvPoint.Rows.Add(1);
                                            //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                            List<object> value_xml = new List<object>();
                                            List<object> attribute_xml = new List<object>();
                                            for (XmlNode m = k.FirstChild; m != null; m = m.NextSibling)
                                            {
                                                if ("Stolbec".Equals(m.Name) && m.FirstChild != null)
                                                {

                                                    value_xml.Add(m.FirstChild.Value);
                                                    XmlAttributeCollection attrColl = m.Attributes;
                                                    attribute_xml.Add(attrColl[0].Value);

                                                }
                                            }
                                            for (int i = 0; i < value_xml.Count(); i++)
                                            {


                                                if (value_xml[i].ToString() != "-")
                                                    dgvPoint.Rows[dgvPoint.Rows.Count - 1].Cells[i].Value = value_xml[i].ToString();
                                                //else
                                                //     dgvPoint.Rows[dgvPoint.Rows.Count - 1].Cells[j].Value = null;
                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                }



                // this.currlist[0] = this.sslive;
                this.currlist[0].C_Mode = this.mpar.C_Mode;
                this.currlist[0].C_Interval = this.mpar.C_Interval;
                this.currlist[0].C_Intervals = this.mpar.C_Intervals;
                this.currlist[0].C_BeginWL = this.mpar.C_BeginWL;
                this.currlist[0].C_EndWL = this.mpar.C_EndWL;
                this.currlist[0].C_SLength = this.mpar.C_SLength;
                this.currlist[0].C_SLength = this.mpar.C_SLength;
                this.currlist[0].C_ScanSpeed = this.mpar.C_ScanSpeed;
                this.currlist[0].C_ScanSpeedDM = this.mpar.C_ScanSpeedDM;
                this.currlist[0].C_StepLen = this.mpar.C_StepLen;
                this.currlist[0].C_ScanCNT = this.mpar.C_ScanCNT;
                this.currlist[0].Data.OrderBy<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue));
                if (this.picCurve.InvokeRequired)
                    this.picCurve.Invoke((Delegate)new Del_MeaData(this.DealMeaData));
                else
                    this.DealMeaData();
                if (this.lblSample.InvokeRequired)
                    this.lblSample.Invoke((Delegate)new Del_SamNameSet(this.SetSname));
                else
                    this.lblSample.Text = this.currlist[0].C_name;
                //this.currlist[0].status = "open";
                //ResetTempReDraw(1);
                this.scanwlpar = (Convert.ToDecimal(this.mpar.C_BeginWL) * 10M).ToString("f0") + " " + (Convert.ToDecimal(this.mpar.C_EndWL) * 10M).ToString("f0") + " " + (Convert.ToDecimal(this.mpar.C_StepLen) * 10M).ToString("f0");
                this.scanmosptpar = (!(this.mpar.C_ScanSpeed == "Быстро") ? (!(this.mpar.C_ScanSpeed == "Средне") ? (!(this.mpar.C_ScanSpeed == "Медленно") ? 4 : 3) : 2) : 1).ToString();

                this.picCurve.Visible = true;

            }
        }

        private void lblShowXY_Click(object sender, EventArgs e)
        {
            /*using (SetXYFrm frm = new SetXYFrm())
            {
                float num1;
                if (this.showpar.AutoXY)
                {
                    if (this.currlist.Count <= 0 || this.currlist[0].Data.Count <= 0)
                    {
                        frm.lblYMax.Text = this.showpar.yMax.ToString();
                        Label lblYmin = frm.lblYMin;
                        num1 = this.showpar.yMin;
                        string str = num1.ToString();
                        lblYmin.Text = str;
                    }
                    else
                    {
                        frm.lblYMax.Text = this.ye.ToString();
                        frm.lblYMin.Text = this.ys.ToString();
                    }
                }
                else
                {
                    frm.lblYMax.Text = this.showpar.yMax.ToString();
                    Label lblYmin = frm.lblYMin;
                    num1 = this.showpar.yMin;
                    string str = num1.ToString();
                    lblYmin.Text = str;
                }
                Label lblXmax = frm.lblXMax;
                num1 = this.showpar.xMax;
                string str1 = num1.ToString("f1");
                lblXmax.Text = str1;
                Label lblXmin = frm.lblXMin;
                num1 = this.showpar.xMin;
                string str2 = num1.ToString("f1");
                lblXmin.Text = str2;
                frm.btnOK.Click += (EventHandler)((param0, param1) =>
                {
                    try
                    {
                        float x1 = (float)Convert.ToDecimal(frm.lblXMin.Text);
                        float x2 = (float)Convert.ToDecimal(frm.lblXMax.Text);
                        float y1 = (float)Convert.ToDecimal(frm.lblYMin.Text);
                        float y2 = (float)Convert.ToDecimal(frm.lblYMax.Text);
                        this.autoscale = false;
                        this.AddToRangeList(x1, x2, y1, y2);
                        frm.Close(); //
                    }
                    catch
                    {
                        CommonFun.showbox("norangedata", "Error");
                    }
                });
                int num2 = (int)frm.ShowDialog();
            }*/
        }
        private void BindSuqarData()
        {
            this.dataGridView2.Rows.Clear();
            for (int index = 0; index < this.currlist[0].Squaredata.Count; ++index)
            {
                this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[index].Cells["ColNO"].Value = (object)(index + 1).ToString();
                this.dataGridView2.Rows[index].Cells["ColBW"].Value = (object)this.currlist[0].Squaredata[index].BeginWL;
                this.dataGridView2.Rows[index].Cells["ColEW"].Value = (object)this.currlist[0].Squaredata[index].EndWL;
                this.dataGridView2.Rows[index].Cells["ColSquare"].Value = (object)this.currlist[0].Squaredata[index].Square;
                this.dataGridView2.Rows[index].Cells["ColDel"].Value = (object)Properties.Resources.Delete;
                this.dataGridView2.Rows[index].Cells["ColColor"].Style = new DataGridViewCellStyle()
                {
                    BackColor = this.currlist[0].Squaredata[index].sqColor
                };
                this.dataGridView2.Rows[index].Tag = (object)this.currlist[0].Squaredata[index];
            }
            if (this.currlist[0].Squaredata.Count >= this.dgvcnt)
                return;
            this.dataGridView2.Rows.Add(this.dgvcnt - this.currlist[0].Squaredata.Count);
        }
        private void SquareDataCauDraw(int rowi)
        {
            SquareData sds = this.currlist[0].Squaredata[rowi];
            int index1 = this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s.xValue == Convert.ToDecimal(sds.BeginWL).ToString("f1")));
            int index2 = this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s.xValue == Convert.ToDecimal(sds.EndWL).ToString("f1")));
            if (index1 <= -1 || index2 <= -1)
                return;
            float num1 = (float)Convert.ToDouble(this.currlist[0].MethodPar.C_StepLen);
            float num2 = 0.0f;
            int index3 = index1 < index2 ? index1 : index2;
            if (this.currlist[0].FromZero)
            {
                for (; index3 <= (index1 > index2 ? index1 : index2); ++index3)
                {
                    if (this.currlist[0].MethodPar.C_Mode == "Abs")
                    {
                        float yAbs = this.currlist[0].Data[index3].yABS;
                        num2 += num1 * yAbs;
                    }
                    else
                    {
                        float yt = this.currlist[0].Data[index3].YT;
                        num2 += num1 * yt;
                    }
                }
                sds.Square = num2.ToString("f4");
                if ((double)this.yMin > 0.0)
                {
                    this.yMin = 0.0f;
                    this.AddToRangeList(this.xMin, this.xMax, this.yMin, this.yMax);
                }
                else
                    this.DrawLine();
            }
            else
            {
                float num3 = (float)Convert.ToDouble(this.currlist[0].Data[index1].xValue);
                float num4 = (float)Convert.ToDouble(this.currlist[0].Data[index2].xValue);
                float num5;
                float num6;
                if (this.currlist[0].MethodPar.C_Mode == "Abs")
                {
                    num5 = this.currlist[0].Data[index1].yABS;
                    num6 = this.currlist[0].Data[index2].yABS;
                }
                else
                {
                    num5 = this.currlist[0].Data[index1].YT;
                    num6 = this.currlist[0].Data[index2].YT;
                }
                float num7 = (float)(((double)num5 - (double)num6) / ((double)num3 - (double)num4));
                float num8 = num5 - (float)(((double)num5 - (double)num6) * (double)num3 / ((double)num3 - (double)num4));
                sds.k = num7;
                sds.b = num8;
                for (; index3 <= (index1 > index2 ? index1 : index2); ++index3)
                {
                    float num9 = num7 * (float)Convert.ToDouble(this.currlist[0].Data[index3].xValue) + num8;
                    if (this.currlist[0].MethodPar.C_Mode == "Abs")
                    {
                        float yAbs = this.currlist[0].Data[index3].yABS;
                        num2 += num1 * (yAbs - num9);
                    }
                    else
                    {
                        float yt = this.currlist[0].Data[index3].YT;
                        num2 += num1 * (yt - num9);
                    }
                }
                sds.Square = num2.ToString("f4");
                this.DrawLine();
            }
            this.BindSuqarData();
        }

        private void PibInOut_Click(object sender, EventArgs e)
        {

            if (this.currlist.Count < 1)
            {
                CommonFun.showbox("Nosample", "Error");
            }
            else
            {

                SpectraScan spectraScan = this.currlist[0];
                using (CurrSampleFrm frm = new CurrSampleFrm(this.currlist, this.showpar.MulShow))
                {
                    frm.btnDelete.Click += ((param0, param1) =>
                    {
                        if (new DRMessageBoxFrm("deleteconfirm", "Warning").ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                            return;
                        bool flag = false;
                        for (int index = 0; index < frm.dataGridView1.Rows.Count; ++index)
                        {
                            if (frm.dataGridView1.Rows[index].Tag != null && frm.dataGridView1.Rows[index].Cells["ColOP1"].Tag== (object)"on")
                            {
                                this.currlist.Remove((SpectraScan)frm.dataGridView1.Rows[index].Tag);
                                flag = true;
                            }
                        }
                        if (flag)
                            CommonFun.InsertLog(CommonFun.GetLanText("specturm"), CommonFun.GetLanText("logdelData"), false);
                        frm.ListBind();
                    });
                    frm.ShowDialog();
                }
                this.XYMaxMin();
                if (spectraScan != this.currlist[0])
                    this.ResetTempReDraw(1);
                else if (this.xyRangeList.Count > 0)
                    this.DrawLine();
                else
                    this.AddToRangeList(this.xMin, this.xMax, this.yMin, this.yMax);
                if (this.currlist.Count > 0)
                {
                    this.dataGridView1.Columns["ColXGD"].HeaderText = this.currlist[0].MethodPar.C_Mode;
                    this.dataGridView1.Columns["ColWL"].HeaderText = "λ";
                    this.dataGridView1.Rows.Clear();
                    for (int index = 0; index < this.currlist[0].Data.Count<MeaureData>(); ++index)
                    {
                        this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Cells["ColWL"].Value = (object)this.currlist[0].Data[index].xValue;
                        float num;
                        if (this.currlist[0].MethodPar.C_Mode == CommonFun.GetLanText("Abs"))
                        {
                            DataGridViewCell cell = this.dataGridView1.Rows[index].Cells["ColXGD"];
                            num = this.currlist[0].Data[index].yABS;
                            string str = num.ToString(this.absacc);
                            cell.Value = (object)str;
                        }
                        else
                        {
                            DataGridViewCell cell = this.dataGridView1.Rows[index].Cells["ColXGD"];
                            num = this.currlist[0].Data[index].YT;
                            string str = num.ToString(this.tacc);
                            cell.Value = (object)str;
                        }
                        this.dataGridView1.Rows[index].Tag = (object)this.currlist[0].Data[index];
                    }
                    if (this.currlist[0].Data.Count >= this.dgvcnt)
                        return;
                    this.dataGridView1.Rows.Add(this.dgvcnt - this.currlist[0].Data.Count);
                }
                else
                {
                    this.dataGridView1.Rows.Add(this.dgvcnt);
                    this.dataGridView1.Columns["ColXGD"].HeaderText = this.mpar.C_Mode;
                }
            }

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            /*if (this.mpar.C_methodoperator == null || this.mpar.C_methodoperator.Length <= 0)
            {
                CommonFun.showbox(CommonFun.GetLanText("nomethod"), "Error");
            }
            else
            {*/
            string errormsg = "";
            /*if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                CommonFun.showbox(errormsg, "Error");
            else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                CommonFun.showbox(errormsg, "Error");
            else*/
            if (!this.sp.IsOpen)
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
            else if (this.btnScan.Text.ToString() == CommonFun.GetLanText("stopmeasure"))
            {
                if (this.ComSta == ComStatus.MEASURE)
                {
                    this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                    CommonFun.WriteSendLine("stop,SCAN_STOPPING 0");
                    this.btnScan.Text = "Остановка";
                    this.stophappend = 0;
                }
                else
                {
                    this.meacnt = 0;
                    this.currslotno = 0;
                    this.slotno = "";
                    this.calormea = 0;
                    this.ComSta = ComStatus.END;
                    this.btnScan.Text = "Измерение";
                    this.setState(ComStatus.END);
                }
            }
            else if (this.ComSta != ComStatus.END)
            {
                CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
            }
            else
            {
                this.ResetTempReDraw(0);
                this.calormea = 2;
                if (CommonFun.GetAppConfig("EightSlot") == "true")
                {
                    /*using (ChoseSlotFrm frm = new ChoseSlotFrm())
                    {
                        frm.btnOK.Click += (EventHandler)((param0, param1) =>
                        {
                            if (frm.chk1.Checked)
                                this.slotno += "1,";
                            if (frm.chk2.Checked)
                                this.slotno += "2,";
                            if (frm.chk3.Checked)
                                this.slotno += "3,";
                            if (frm.chk4.Checked)
                                this.slotno += "4,";
                            if (frm.chk5.Checked)
                                this.slotno += "5,";
                            if (frm.chk6.Checked)
                                this.slotno += "6,";
                            if (frm.chk7.Checked)
                                this.slotno += "7,";
                            if (frm.chk8.Checked)
                                this.slotno += "8,";
                            if (this.slotno.Length <= 0)
                                return;
                            frm.Close(); //this.Dispose();
                            this.slotno = this.slotno.Substring(0, this.slotno.Length - 1);
                            this.ComSta = ComStatus.SETCHP;
                            this.sp.WriteLine("SETCHP " + this.slotno.Split(',')[0] + "\r\n");
                            CommonFun.WriteSendLine("SETCHP " + this.slotno.Split(',')[0]);
                            this.setState(ComStatus.MEASURE);
                            this.btnScan.Text = CommonFun.GetLanText("stopmeasure");
                            this.panel4.Visible = true;
                            this.lblProsess.Text = CommonFun.GetLanText("measureing");
                            this.progressBar1.Value = 5;
                            CommonFun.InsertLog(CommonFun.GetLanText("specturm"), CommonFun.GetLanText("measure"), false);
                        });
                        int num = (int)frm.ShowDialog();
                    }*/
                }
                else
                {
                    string[] strArray1 = new string[7]
                    {
              "SETSCANWL ",
              (Convert.ToDecimal(this.mpar.C_BeginWL) * 10M).ToString("f0"),
              " ",
              null,
              null,
              null,
              null
                    };
                    string[] strArray2 = strArray1;
                    System.Decimal num = Convert.ToDecimal(this.mpar.C_EndWL) * 10M;
                    string str1 = num.ToString("f0");
                    strArray2[3] = str1;
                    strArray1[4] = " ";
                    string[] strArray3 = strArray1;
                    num = Convert.ToDecimal(this.mpar.C_StepLen) * 10M;
                    string str2 = num.ToString("f0");
                    strArray3[5] = str2;
                    strArray1[6] = "\r\n";
                    string text = string.Concat(strArray1);
                    this.ComSta = ComStatus.SETSCANWL;
                    this.sp.WriteLine(text);
                    CommonFun.WriteSendLine(text);
                    this.setState(ComStatus.MEASURE);
                    this.btnScan.Text = "Остановить";
                    // this.panel4.Visible = true;
                    //this.lblProsess.Text = CommonFun.GetLanText("measureing");
                    this.progressBar1.Value = 5;
                    CommonFun.InsertLog("Spectrum scan", "Measure", false);
                }
            }
            //   }
        }

        private void SpectrumScan_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvPoint_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            //    this.timer1.Stop();
           // RegistryKey hkcr = Registry.ClassesRoot;
           // RegistryKey excelKey = hkcr.OpenSubKey("Excel.Application");
          //  bool excelInstalled = excelKey == null ? false : true;
          //  if (excelInstalled == true) {
                if (dataGridView1.Rows.Count == 0)
                {
                    CommonFun.showbox("Измерений не было, сохранять нечего", "Info");
                }
                else
                {
                    using (SaveFrm save = new SaveFrm(extension, "Режим сканирования спектра"))
                    {
                        save.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
                        {
                            if (save.Name_file.Text.Length > 0)
                            {
                                filepath = Directory.GetCurrentDirectory() + @"\Сохраненные протоколы";
                                if (!Directory.Exists(filepath))
                                {
                                    Directory.CreateDirectory(filepath);
                                }
                                filepath = filepath + @"\" + save.Name_file.Text + ".xlsx";

                                if (!File.Exists(filepath))
                                {
                                //save_name = filepath;
                                ExportToExcel_(filepath);
                                    save.Dispose();
                                }
                                else
                                {
                                    CommonFun.showbox("Файл с таким именем уже существует.", "Info");
                                }

                            }

                        });
                        save.ShowDialog();
                    }
                }
           // }
          //  this.timer1.Start();
            //ExportWord_();
        }
        private void ExportToExcel_(string sfd)
        {
            Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
            exApp.Application.Workbooks.Add(Type.Missing);

            exApp.Columns.ColumnWidth = 20;

            exApp.Cells[1, 1] = CommonFun.GetLanText("type");

            if (currlist[0].InstrumentsType == "P7")
                exApp.Cells[1, 2] = "УФ-6700";
            if (currlist[0].InstrumentsType == "P8")
                exApp.Cells[1, 2] = "УФ-6800";
            if (currlist[0].InstrumentsType == "P9")
                exApp.Cells[1, 2] = "УФ-6900";

            //exApp.Cells[1, 2] = currlist[0].InstrumentsType;
            exApp.Cells[1, 3] = CommonFun.GetLanText("serino");
            exApp.Cells[1, 4] = currlist[0].Serials;

            exApp.Cells[2, 1] = CommonFun.GetLanText("specturmrange") + "(нм)";
            exApp.Cells[2, 2] = CommonFun.GetAppConfig("Spectralbandwidth");
            exApp.Cells[2, 3] = CommonFun.GetLanText("lightswitch") + "(нм)";
            exApp.Cells[2, 4] = CommonFun.GetAppConfig("SwithWL");


            exApp.Cells[3, 1] = CommonFun.GetLanText("operatemode");
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                exApp.Cells[3, 2] = "GLP";
            else
                exApp.Cells[3, 2] = CommonFun.GetLanText("common");
            exApp.Cells[3, 3] = CommonFun.GetLanText("operators");
            exApp.Cells[3, 4] = currlist[0].C_Operator;

            //exApp.Cells[4, 1] = CommonFun.GetLanText("measuretime");
            //   exApp.Cells[4, 2] = MM.D_Mtime;

            exApp.Cells[5, 1] = CommonFun.GetLanText("photometricmode");
            exApp.Cells[5, 2] = this.mpar.C_Mode;
            exApp.Cells[6, 1] = CommonFun.GetLanText("scanrange") + "(нм)";
            exApp.Cells[6, 2] = this.mpar.C_BeginWL + "～" + this.mpar.C_EndWL;

            exApp.Cells[7, 1] = CommonFun.GetLanText("scanspeed");
            exApp.Cells[7, 2] = this.mpar.C_ScanSpeed;
            exApp.Cells[8, 1] = CommonFun.GetLanText("scaninterval") + "(нм)";
            exApp.Cells[8, 2] = this.mpar.C_StepLen;

            exApp.Cells[9, 1] = CommonFun.GetLanText("lightlength");
            exApp.Cells[9, 2] = this.mpar.C_SLength;

            //exApp.Cells[11, 1] = CommonFun.GetLanText("measurechart");

            List<SpectraScan> printlist = new List<SpectraScan>();
            for (int index = 0; index < currlist.Count; ++index)
            {
                printlist.Add(this.currlist[index]);
            }

            /*string str2 = "C:\\Temp\\curve.bmp";
            if (!Directory.Exists("C:\\Temp\\"))
                Directory.CreateDirectory("C:\\Temp\\");
            if (File.Exists(str2))
                File.Delete(str2);
            new Bitmap(this.picOut.Image).Save(str2);*/


            /*System.IO.MemoryStream ms = new System.IO.MemoryStream();
            picOut.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] ar = new byte[ms.Length];
            ms.Write(ar, 0, ar.Length);*/


            /*  byte[] pictureData = File.ReadAllBytes(str2);
              HSSFWorkbook hssfWorkbook = new HSSFWorkbook();
             int pictureIndex = hssfWorkbook.AddPicture(pictureData, PictureType.JPEG);

              Microsoft.Office.Interop.Excel.Range chartRange;
              Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
              Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
              object misValue = System.Reflection.Missing.Value;*/

            // xlWorkBook = exApp.Workbooks.Add(misValue);
            // xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            /* Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)exApp.Worksheets.Application.ChartObjects(Type.Missing);
             Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
             Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
             chartRange = xlWorkSheet.get_Range("A10", "F26");
             chartPage.SetSourceData(chartRange, misValue);
             chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;*/

            /*string str2 = "C:\\Temp\\curve.bmp";
            if (!Directory.Exists("C:\\Temp\\"))
                Directory.CreateDirectory("C:\\Temp\\");
            if (File.Exists(str2))
                File.Delete(str2);
            new Bitmap(this.picOut.Image).Save(str2);*/
            // chartPage.Export(str2, "BMP", misValue);

            //   picOut.Image = new Bitmap(str2);

            // exApp.Cells[10, 1] = 
            //    ISheet sheet = hssfWorkbook.CreateSheet("Sheet1");
            //  sheet.CreateDrawingPatriarch().CreatePicture((IClientAnchor)new HSSFClientAnchor(0, 0, 1023, 0, 0, 12 + 1, 10, 12 + 17), pictureIndex).Resize();
            //     exApp.Sheets.Add(sheet);

            exApp.Cells[13, 1] = "Таблица измерений";

            for (int i = 1; i < this.dataGridView1.Columns.Count; i++)
            {
                exApp.Cells[14, i] = this.dataGridView1.Columns[i].HeaderText;
            }
            //Thread.Sleep(500);
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                // Thread.Sleep(200);
                for (int j = 1; j < this.dataGridView1.Columns.Count; j++)
                {
                    exApp.Cells[i + 15, j] = this.dataGridView1.Rows[i].Cells[j].Value;
                }
            }

            exApp.ActiveWorkbook.SaveCopyAs(sfd);
            exApp.ActiveWorkbook.Saved = true;
            exApp.Visible = false;
            exApp.Quit();

        }

        private void DrawOutLine(List<SpectraScan> printlist, int peak, int valley)
        {
            this.picOut.Height = 380;
            this.picOut.Width = 700;
            if (printlist.Count <= 0 || printlist[0].Data.Count <= 0)
                return;
            Bitmap bitmap = new Bitmap(this.picOut.Width, this.picOut.Height);
            Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            graphics.FillRectangle((Brush)new SolidBrush(Color.White), 0, 0, this.picOut.Width, this.picOut.Height);
            SizeF sizeF1 = graphics.MeasureString(Convert.ToDouble(printlist[0].Data.Select<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue)).Min<string>()).ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            SizeF sizeF2 = graphics.MeasureString(printlist[0].Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.YT)).Max().ToString("f4"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float num1 = sizeF1.Height + 20f + sizeF2.Width;
            float num2 = (float)(this.picOut.Width - 5);
            float num3 = (float)((double)this.picOut.Height - (double)sizeF1.Height * 2.0 - 20.0);
            float num4 = sizeF1.Height + 20f;
            RectangleF rectangleF = new RectangleF(num1, num4, num2 - num1, num3 - num4);
            XYRange xyr = new XYRange();
            float num5;
            float num6;
            float num7;
            float num8;
            if (printlist.Count > 1)
            {
                List<XYRange> list = this.xyRangeList.Where<XYRange>((Func<XYRange, bool>)(s => s.Curr)).ToList<XYRange>();
                if (list.Count < 1)
                    return;
                xyr = list[0];
                num5 = xyr.X1;
                num6 = xyr.X2;
                num7 = xyr.Y1;
                num8 = xyr.Y2;
            }
            else
            {
                num5 = Convert.ToSingle(printlist[0].MethodPar.C_EndWL);
                num6 = Convert.ToSingle(printlist[0].MethodPar.C_BeginWL);
                if (printlist[0].MethodPar.C_Mode == CommonFun.GetLanText("Abs"))
                {
                    num7 = printlist[0].Data.Min<MeaureData>((Func<MeaureData, float>)(s => s.yABS));
                    num8 = printlist[0].Data.Max<MeaureData>((Func<MeaureData, float>)(s => s.yABS));
                }
                else
                {
                    num7 = printlist[0].Data.Min<MeaureData>((Func<MeaureData, float>)(s => s.YT));
                    num8 = printlist[0].Data.Max<MeaureData>((Func<MeaureData, float>)(s => s.YT));
                }
                xyr.X1 = num5;
                xyr.X2 = num6;
                xyr.Y1 = num7;
                xyr.Y2 = num8;
            }
            if (this.autoscale)
            {
                if (this.mpar.C_Mode == CommonFun.GetLanText("Abs"))
                {
                    string str = "0";
                    switch (this.absacc)
                    {
                        case "f6":
                            str = "0.000001";
                            break;
                        case "f5":
                            str = "0.00001";
                            break;
                        case "f4":
                            str = "0.0001";
                            break;
                        case "f3":
                            str = "0.001";
                            break;
                        case "f2":
                            str = "0.01";
                            break;
                        case "f1":
                            str = "0.1";
                            break;
                    }
                    if ((double)Math.Abs(num8) < (double)Convert.ToSingle(str))
                        num8 = Convert.ToSingle(str);
                    if ((double)Math.Abs(num7) < (double)Convert.ToSingle(str))
                        num7 = -Convert.ToSingle(str);
                    if ((double)Math.Abs(num8) > (double)Convert.ToSingle(str) || (double)Math.Abs(num7) > (double)Convert.ToSingle(str))
                    {
                        num8 = (float)((1.8 * (double)num8 - 0.2 * (double)num7) / 1.6);
                        num7 = (float)((1.8 * (double)num7 - 0.2 * (double)num8) / 1.6);
                    }
                }
                else
                {
                    num8 = (float)((1.8 * (double)num8 - 0.2 * (double)num7) / 1.6);
                    num7 = (float)((1.8 * (double)num7 - 0.2 * (double)num8) / 1.6);
                }
            }
            if ((double)num6 - (double)num5 == 0.0 || (double)num8 - (double)num7 == 0.0)
                return;
            double num9 = ((double)num2 - (double)num1) / ((double)num6 - (double)num5);
            double num10 = ((double)num3 - (double)num4) / ((double)num8 - (double)num7);
            graphics.DrawLine(new Pen(Color.Black, 1f), num1, num3, num2, num3);
            graphics.DrawLine(new Pen(Color.Black, 1f), num1, num4, num2, num4);
            graphics.DrawLine(new Pen(Color.Black, 1f), num1, num3, num1, num4);
            graphics.DrawLine(new Pen(Color.Black, 1f), num2, num3, num2, num4);
            float x1 = num1;
            float y1 = num3 + 5f;
            graphics.DrawString(num5.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x1, y1));
            SizeF sizeF3 = graphics.MeasureString(num6.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x2 = num2 - sizeF3.Width;
            graphics.DrawString(num6.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x2, y1));
            string format;
            if (this.showpar.AutoXY)
            {
                format = !(printlist[0].MethodPar.C_Mode == CommonFun.GetLanText("Abs")) ? this.tacc : this.absacc;
            }
            else
            {
                float num11 = this.showpar.yMax;
                string str1 = num11.ToString("f10").TrimEnd('0');
                num11 = this.showpar.yMin;
                string str2 = num11.ToString("f10").TrimEnd('0');
                string str3 = str1.Substring(str1.IndexOf('.') + 1);
                string str4 = str2.Substring(str2.IndexOf('.') + 1);
                format = str3.Length < str4.Length ? "f" + (object)str4.Length : "f" + (object)str3.Length;
            }
            SizeF sizeF4 = graphics.MeasureString(num7.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x3 = num1 - sizeF4.Width;
            float y2 = num3 - sizeF4.Height / 2f;
            graphics.DrawString(this.ys.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x3, y2));
            SizeF sizeF5 = graphics.MeasureString(num8.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x4 = num1 - sizeF5.Width;
            float y3 = num4 - sizeF5.Height / 2f;
            graphics.DrawString(num8.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x4, y3));
            for (int index = 1; index < 4; ++index)
            {
                Pen pen = new Pen(Color.Black, 1f);
                pen.DashStyle = DashStyle.Dot;
                graphics.DrawLine(pen, num1 + (float)(((double)num2 - (double)num1) * (double)index / 4.0), num3, num1 + (float)(((double)num2 - (double)num1) * (double)index / 4.0), num4);
                graphics.DrawLine(pen, num1, num4 + (float)(((double)num3 - (double)num4) * (double)index / 4.0), num2, num4 + (float)(((double)num3 - (double)num4) * (double)index / 4.0));
            }
            float x5 = num1 + (float)(((double)num2 - (double)num1 - (double)sizeF1.Width) / 2.0);
            float y4 = num3 + 5f;
            graphics.DrawString("WL.(нм)", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
            if (printlist[0].MethodPar.C_Mode == CommonFun.GetLanText("Abs"))
            {
                float x6 = num1 - graphics.MeasureString("Abs", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2))).Width;
                float y5 = num4 + (float)(((double)num3 - (double)num4 - (double)sizeF2.Height) / 2.0);
                graphics.DrawString("Abs", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
            }
            else
            {
                float x6 = num1 - graphics.MeasureString("%T", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2))).Width;
                float y5 = num4 + (float)(((double)num3 - (double)num4 - (double)sizeF2.Height) / 2.0);
                graphics.DrawString("%T", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
            }
            foreach (SpectraScan spectraScan in printlist)
            {
                List<MeaureData> source = spectraScan.Data;
                int index1 = source.FindIndex((Predicate<MeaureData>)(s => s.xValue.Equals(xyr.X1.ToString("f1"))));
                int index2 = source.FindIndex((Predicate<MeaureData>)(s => s.xValue.Equals(xyr.X2.ToString("f1"))));
                List<MeaureData> meaureDataList = new List<MeaureData>();
                if (index1 == -1 || index2 == -1)
                {
                    meaureDataList = source;
                }
                else
                {
                    for (int index3 = Math.Min(index1, index2); index3 <= Math.Max(index2, index1); ++index3)
                        meaureDataList.Add(source[index3]);
                }
                if (meaureDataList.Count > 0)
                    source = meaureDataList;
                double num11 = (double)num1 + (Convert.ToDouble(source[source.Count<MeaureData>() - 1].xValue) - (double)num5) * num9;
                string str1 = spectraScan.MethodPar.C_Mode == null || !(spectraScan.MethodPar.C_Mode != "") ? this.mpar.C_Mode : spectraScan.MethodPar.C_Mode;
                double num12 = !(str1 == CommonFun.GetLanText("Abs")) ? (double)num3 - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].YT) - (double)num7) * num10 : (double)num3 - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].yABS) - (double)num7) * num10;
                if (num12 < (double)num4)
                    num12 = (double)num4;
                if (num12 > (double)num3)
                    num12 = (double)num3;
                for (int index3 = source.Count<MeaureData>() - 2; index3 >= 0; --index3)
                {
                    double num13 = (double)num1 + (Convert.ToDouble(source[index3].xValue) - (double)num5) * num9;
                    double num14 = !(str1 == CommonFun.GetLanText("Abs")) ? (Convert.ToDouble(source[index3].YT) >= (double)num7 ? (Convert.ToDouble(source[index3].YT) <= (double)num8 ? (double)num3 - (Convert.ToDouble(source[index3].YT) - (double)num7) * num10 : (double)num4) : (double)num3) : (Convert.ToDouble(source[index3].yABS) >= (double)num7 ? (Convert.ToDouble(source[index3].yABS) <= (double)num8 ? (double)num3 - (Convert.ToDouble(source[index3].yABS) - (double)num7) * num10 : (double)num4) : (double)num3);
                    graphics.DrawLine(new Pen(spectraScan.color, 1f), (float)num11, (float)num12, (float)num13, (float)num14);
                    num11 = num13;
                    num12 = num14;
                }
                if (spectraScan == this.currlist[0])
                {
                    if (peak == 1)
                    {
                        foreach (MeaureData meaureData in spectraScan.Data.Where<MeaureData>((Func<MeaureData, bool>)(s =>
                        {
                            int? pv = s.PV;
                            return pv.GetValueOrDefault() == 0 && pv.HasValue;
                        })))
                        {
                            double num13 = !(str1 == CommonFun.GetLanText("Abs")) ? (double)num3 - (Convert.ToDouble(meaureData.YT) - (double)num7) * num10 : (double)num3 - (Convert.ToDouble(meaureData.yABS) - (double)num7) * num10;
                            double num14 = (double)num1 + (Convert.ToDouble(meaureData.xValue) - (double)num5) * num9;
                            graphics.DrawEllipse(new Pen(Color.Black, 1f), Convert.ToInt32(num14) - 3, Convert.ToInt32(num13) - 3, 4, 4);
                            string str2 = Convert.ToDecimal(meaureData.xValue).ToString("f1");
                            SizeF sizeF6 = graphics.MeasureString(str2, new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
                            graphics.DrawString(str2, new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Navy), new PointF((float)num14, (float)num13 - sizeF6.Width));
                        }
                    }
                    SizeF sizeF7;
                    if (valley == 1)
                    {
                        foreach (MeaureData meaureData in spectraScan.Data.Where<MeaureData>((Func<MeaureData, bool>)(s =>
                        {
                            int? pv = s.PV;
                            return pv.GetValueOrDefault() == 1 && pv.HasValue;
                        })))
                        {
                            double num13 = !(str1 == CommonFun.GetLanText("Abs")) ? (double)num3 - (Convert.ToDouble(meaureData.YT) - (double)num7) * num10 : (double)num3 - (Convert.ToDouble(meaureData.yABS) - (double)num7) * num10;
                            double num14 = (double)num1 + (Convert.ToDouble(meaureData.xValue) - (double)num5) * num9;
                            graphics.DrawEllipse(new Pen(Color.Black, 1f), Convert.ToInt32(num14) - 3, Convert.ToInt32(num13) - 3, 4, 4);
                            string str2 = Convert.ToDecimal(meaureData.xValue).ToString("f1");
                            sizeF7 = graphics.MeasureString(str2, new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
                            graphics.DrawString(str2, new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.SkyBlue), new PointF((float)num14, (float)num13));
                        }
                    }
                    if (spectraScan.Data.Where<MeaureData>((Func<MeaureData, bool>)(s =>
                    {
                        int? pv = s.PV;
                        return pv.GetValueOrDefault() == 2 && pv.HasValue;
                    })).ToList<MeaureData>().Count > 0)
                    {
                        foreach (MeaureData meaureData in spectraScan.Data.Where<MeaureData>((Func<MeaureData, bool>)(s =>
                        {
                            int? pv = s.PV;
                            return pv.GetValueOrDefault() == 2 && pv.HasValue;
                        })))
                        {
                            double num13 = !(str1 == CommonFun.GetLanText("Abs")) ? (double)num3 - (Convert.ToDouble(meaureData.YT) - (double)num7) * num10 : (double)num3 - (Convert.ToDouble(meaureData.yABS) - (double)num7) * num10;
                            double num14 = (double)num1 + (Convert.ToDouble(meaureData.xValue) - (double)num5) * num9;
                            graphics.DrawEllipse(new Pen(Color.Black, 1f), Convert.ToInt32(num14) - 3, Convert.ToInt32(num13) - 3, 4, 4);
                            string str2 = Convert.ToDecimal(meaureData.xValue).ToString("f1");
                            sizeF7 = graphics.MeasureString(str2, new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
                            graphics.DrawString(str2, new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Green), new PointF((float)num14, (float)num13));
                        }
                    }
                }
                OperateType? opertype = spectraScan.opertype;
                if ((opertype.GetValueOrDefault() != OperateType.Square ? 0 : (opertype.HasValue ? 1 : 0)) != 0)
                {
                    for (int index3 = 0; index3 < spectraScan.Squaredata.Count; ++index3)
                    {
                        SquareData sd = spectraScan.Squaredata[index3];
                        float num13 = num1 + (float)((Convert.ToDouble(sd.BeginWL) - Convert.ToDouble(num5)) * num9);
                        float num14 = num1 + (float)((Convert.ToDouble(sd.EndWL) - Convert.ToDouble(num5)) * num9);
                        int index4 = this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s.xValue == sd.BeginWL));
                        int index5 = this.currlist[0].Data.FindIndex((Predicate<MeaureData>)(s => s.xValue == sd.EndWL));
                        float num15 = (float)(((double)num13 < (double)num14 ? (double)num13 : (double)num14) + 4.0);
                        if (this.currlist[0].FromZero)
                        {
                            float y1_1 = num3 - (float)((0.0 - (double)num7) * num10);
                            if (str1 == CommonFun.GetLanText("Abs"))
                            {
                                float y2_1 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[index4].yABS) - (double)num7) * num10);
                                float y2_2 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[index5].yABS) - (double)num7) * num10);
                                graphics.DrawLine(new Pen(sd.sqColor, 1f), num13, y1_1, num13, y2_1);
                                graphics.DrawLine(new Pen(sd.sqColor, 1f), num14, y1_1, num14, y2_2);
                            }
                            else
                            {
                                float y2_1 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[index4].YT) - (double)num7) * num10);
                                float y2_2 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[index5].YT) - (double)num7) * num10);
                                graphics.DrawLine(new Pen(sd.sqColor, 1f), num13, y1_1, num13, y2_1);
                                graphics.DrawLine(new Pen(sd.sqColor, 1f), num14, y1_1, num14, y2_2);
                            }
                            for (; (double)num15 <= ((double)num13 > (double)num14 ? (double)num13 : (double)num14); num15 += 4f)
                            {
                                float num16 = Convert.ToSingle(num15 - num1) / Convert.ToSingle(num9) + num5;
                                float target = !(Convert.ToDecimal(this.currlist[0].MethodPar.C_StepLen) >= 1M) ? Convert.ToSingle(num16.ToString("f1")) : Convert.ToSingle(num16.ToString("f0"));
                                int near = CommonFun.getNear(this.currlist[0].Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => Convert.ToSingle(s.xValue))).ToArray<float>(), target);
                                if (str1 == CommonFun.GetLanText("Abs"))
                                {
                                    float y2_1 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[near].yABS) - (double)num7) * num10);
                                    graphics.DrawLine(new Pen(sd.sqColor, 1f), num15, y1_1, num15, y2_1);
                                }
                                else
                                {
                                    float y2_1 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[near].YT) - (double)num7) * num10);
                                    graphics.DrawLine(new Pen(sd.sqColor, 1f), num15, y1_1, num15, y2_1);
                                }
                            }
                        }
                        else
                        {
                            for (; (double)num15 <= ((double)num13 > (double)num14 ? (double)num13 : (double)num14); num15 += 4f)
                            {
                                float num16 = Convert.ToSingle(num15 - num1) / Convert.ToSingle(num9) + num5;
                                float target = !(Convert.ToDecimal(this.currlist[0].MethodPar.C_StepLen) >= 1M) ? Convert.ToSingle(num16.ToString("f1")) : Convert.ToSingle(num16.ToString("f0"));
                                int near = CommonFun.getNear(this.currlist[0].Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => Convert.ToSingle(s.xValue))).ToArray<float>(), target);
                                float num17 = sd.k * target + sd.b;
                                float y1_1 = num3 - (float)((Convert.ToDouble(num17) - (double)num7) * num10);
                                if (str1 == CommonFun.GetLanText("Abs"))
                                {
                                    float y2_1 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[near].yABS) - (double)num7) * num10);
                                    graphics.DrawLine(new Pen(sd.sqColor, 1f), num15, y1_1, num15, y2_1);
                                }
                                else
                                {
                                    float y2_1 = num3 - (float)((Convert.ToDouble(this.currlist[0].Data[near].YT) - (double)num7) * num10);
                                    graphics.DrawLine(new Pen(sd.sqColor, 1f), num15, y1_1, num15, y2_1);
                                }
                            }
                            float y1_2 = num3 - (float)(((double)sd.k * Convert.ToDouble(sd.BeginWL) + (double)sd.b - (double)num7) * num10);
                            float y2_2 = num3 - (float)(((double)sd.k * Convert.ToDouble(sd.EndWL) + (double)sd.b - (double)num7) * num10);
                            if (this.currlist[0].ShowZero)
                                graphics.DrawLine(new Pen(sd.sqColor, 1f), num13, y1_2, num14, y2_2);
                        }
                        if (str1 == CommonFun.GetLanText("Abs"))
                        {
                            float num16 = num3 - (float)(((double)this.currlist[0].Data[index4].yABS - (double)num7) * num10);
                            float num17 = num3 - (float)(((double)this.currlist[0].Data[index5].yABS - (double)num7) * num10);
                            graphics.DrawString((index3 + 1).ToString(), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(num13 + (float)(((double)num14 - (double)num13) / 2.0), num16 + (float)(((double)num17 - (double)num16) / 2.0)));
                        }
                        else
                        {
                            float num16 = num3 - (float)(((double)this.currlist[0].Data[index4].YT - (double)num7) * num10);
                            float num17 = num3 - (float)(((double)this.currlist[0].Data[index5].YT - (double)num7) * num10);
                            graphics.DrawString((index3 + 1).ToString(), new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(num13 + (float)(((double)num14 - (double)num13) / 2.0), num16 + (float)(((double)num17 - (double)num16) / 2.0)));
                        }
                    }
                }
            }
            this.picOut.Image = (System.Drawing.Image)bitmap;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                CommonFun.showbox("Измерений не было, печатать нечего", "Info");
            }
            else
            {
                string str = "";
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    str += PrinterSettings.InstalledPrinters[i] + "\n";
                }
                if (str != "")
                {
                    prinPage = 0;
                    strcountScan = 0;
                    realwidth = 0;
                    realheight = 0;
                    width = 0;
                    height = 0;
                    PrintPreviewDialogSelectPrinter printPreviewDialogSelectPrinter = new PrintPreviewDialogSelectPrinter();
                    printPreviewDialogSelectPrinter.Document = printScanSpectrum;
                    printPreviewDialogSelectPrinter.ShowDialog();
                }
            }
        }
        int prinPage;
        int strcountScan;
        int realwidth;
        int realheight;
        int width;
        int height;
        int height1;
        private void printScanSpectrum_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (prinPage <= 0)
            {
                e.Graphics.DrawString("Протокол выполнения измерений\n       в Сканирующем режиме\n\n", new System.Drawing.Font("Times New Roman", 20, FontStyle.Bold), Brushes.Black, 180, 50);

                e.Graphics.DrawString(CommonFun.GetLanText("type"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 115);
                if(currlist[0].InstrumentsType == "P7")
                    e.Graphics.DrawString("УФ-6700", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 115);
                if(currlist[0].InstrumentsType == "P8")
                    e.Graphics.DrawString("УФ-6800", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 115);
                if(currlist[0].InstrumentsType == "P9")
                    e.Graphics.DrawString("УФ-6900", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 115);

                e.Graphics.DrawString(CommonFun.GetLanText("serino"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 135);
                e.Graphics.DrawString(currlist[0].Serials, new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 135);

                e.Graphics.DrawString(CommonFun.GetLanText("specturmrange") + "(нм)", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 155);
                e.Graphics.DrawString(CommonFun.GetAppConfig("Spectralbandwidth"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 155);

                e.Graphics.DrawString(CommonFun.GetLanText("lightswitch") + "(нм)", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 175);
                e.Graphics.DrawString(CommonFun.GetAppConfig("SwithWL"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 175);

                e.Graphics.DrawString(CommonFun.GetLanText("operatemode"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 195);
                if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                    e.Graphics.DrawString("GLP", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 195);
                else
                    e.Graphics.DrawString(CommonFun.GetLanText("common"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 195);

                e.Graphics.DrawString(CommonFun.GetLanText("operators"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 215);
                e.Graphics.DrawString(currlist[0].C_Operator, new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 215);

                e.Graphics.DrawString(CommonFun.GetLanText("photometricmode"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 235);
                e.Graphics.DrawString(this.mpar.C_Mode, new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 235);

                e.Graphics.DrawString(CommonFun.GetLanText("scanrange") + "(нм)", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 255);
                e.Graphics.DrawString(this.mpar.C_BeginWL + "～" + this.mpar.C_EndWL, new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 255);

                e.Graphics.DrawString(CommonFun.GetLanText("scanspeed"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 275);
                e.Graphics.DrawString(this.mpar.C_ScanSpeed, new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 275);

                e.Graphics.DrawString(CommonFun.GetLanText("scaninterval") + "(нм)", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 295);
                e.Graphics.DrawString(this.mpar.C_StepLen, new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 295);

                e.Graphics.DrawString(CommonFun.GetLanText("lightlength"), new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 315);
                e.Graphics.DrawString(this.mpar.C_SLength, new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 400, 315);

                height = height + 355;

                e.Graphics.DrawString("График измерений\n\n",
                  new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, 335);

                List<SpectraScan> printlist = new List<SpectraScan>();
                for (int index = 0; index < currlist.Count; ++index)
                {
                    printlist.Add(this.currlist[index]);
                }
                DrawOutLine(printlist, 0, 0);

                //height = this.picOut.Height;
                Bitmap bmp = new Bitmap(picTop.Width, picTop.Height);
                picOut.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, picTop.Width, picTop.Height));
                e.Graphics.DrawImage(bmp, 15, 355);
                height = height + this.picOut.Height + 20;

                e.Graphics.DrawString("Таблица результатов измерений\n\n",
               new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, height);

                realwidth = dataGridView1.Columns[1].Width + 5;
                realheight = height + 35;
                width = 100;
                height1 = dataGridView1.Rows[0].Height + 5;
                for (int z = 1; z < dataGridView1.Columns.Count; z++)
                {
                    e.Graphics.FillRectangle(Brushes.White, realwidth, realheight, width, height1);
                    e.Graphics.DrawRectangle(Pens.Black, realwidth, realheight, width, height1);
                    e.Graphics.DrawString(dataGridView1.Columns[z].HeaderText, new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, realwidth, realheight);
                    realwidth = realwidth + width;
                }
                realwidth = dataGridView1.Columns[1].Width + 5;
                realheight = realheight + 20;

                while (strcountScan < dataGridView1.Rows.Count)
                {
                    for (int j = 1; j < dataGridView1.Columns.Count; j++)
                    {
                        e.Graphics.FillRectangle(Brushes.White, realwidth, realheight, width, height1);
                        e.Graphics.DrawRectangle(Pens.Black, realwidth, realheight, width, height1);
                        e.Graphics.DrawString(dataGridView1.Rows[strcountScan].Cells[j].Value.ToString(), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, realwidth, realheight);
                        realwidth = realwidth + width;

                    }
                    realwidth = dataGridView1.Columns[1].Width + 5;
                    realheight = realheight + 20;

                    if (realheight > e.MarginBounds.Height)
                    {
                        height = 100;
                        e.HasMorePages = true;
                        //   strcountScan++;
                        e.Graphics.DrawString("Страница " + (prinPage + 1), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 300, 1100);
                        prinPage++;
                        return;
                    }
                    else
                    {
                        e.HasMorePages = false;

                    }
                    // strcountScan++;

                    strcountScan++;
                }
            }
            else
            {
                realwidth = dataGridView1.Columns[1].Width + 5;
                realheight = 80;
                width = 100;
                height1 = dataGridView1.Rows[0].Height + 5;
                for (int z = 1; z < dataGridView1.Columns.Count; z++)
                {
                    e.Graphics.FillRectangle(Brushes.White, realwidth, realheight, width, height1);
                    e.Graphics.DrawRectangle(Pens.Black, realwidth, realheight, width, height1);
                    e.Graphics.DrawString(dataGridView1.Columns[z].HeaderText, new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, realwidth, realheight);
                    realwidth = realwidth + width;
                }
                realwidth = dataGridView1.Columns[1].Width + 5;
                realheight = realheight + 20;
                strcountScan = strcountScan + 1;
                while (strcountScan < dataGridView1.Rows.Count)
                {
                    for (int j = 1; j < dataGridView1.Columns.Count; j++)
                    {
                        e.Graphics.FillRectangle(Brushes.White, realwidth, realheight, width, height1);
                        e.Graphics.DrawRectangle(Pens.Black, realwidth, realheight, width, height1);
                        e.Graphics.DrawString(dataGridView1.Rows[strcountScan].Cells[j].Value.ToString(), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, realwidth, realheight);
                        realwidth = realwidth + width;

                    }
                    realwidth = dataGridView1.Columns[1].Width + 5;
                    realheight = realheight + 20;

                    if (realheight > e.MarginBounds.Height)
                    {
                        height = 100;
                        e.HasMorePages = true;
                        //   strcountScan++;
                        e.Graphics.DrawString("Страница " + (prinPage + 1), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 300, 1100);
                        prinPage++;
                        return;
                    }
                    else
                    {
                        e.HasMorePages = false;

                    }
                    strcountScan++;
                }
            }

            realheight = realheight + 20;
            e.Graphics.DrawString("Дата:", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, realheight + 20);
            e.Graphics.DrawString(this.mpar.D_Mtime.ToString(), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 85, realheight + 20);
            e.Graphics.DrawString("Время начала:", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 180, realheight + 20);
            e.Graphics.DrawString(" _______ ", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 290, realheight + 20);

            e.Graphics.DrawString("Время окончания:", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 380, realheight + 20);
            e.Graphics.DrawString(" _______ ", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 530, realheight + 20);
            e.Graphics.DrawString("Исполнитель:", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, realheight + 50);
            e.Graphics.DrawString(" _______________________ /", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 160, realheight + 50);
            e.Graphics.DrawString("Руководитель:", new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, 25, realheight + 80);
            e.Graphics.DrawString(" _______________________ /", new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 160, realheight + 80);
            if (prinPage > 0)
            {
                e.Graphics.DrawString("Страница " + (prinPage + 1), new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 300, 1100);
            }
            prinPage = 0;
            strcountScan = 0;
            // KinPrintCancel(sender, e);
        }

        private void btnCSV_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                CommonFun.showbox("Измерений не было, печатать нечего", "Info");
            }
            else
            {
                using (SaveFrm save = new SaveFrm(extension, "Режим сканирования спектра"))
                {
                    save.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
                    {
                        if (save.Name_file.Text.Length > 0)
                        {
                            filepath = Directory.GetCurrentDirectory() + @"\Сохраненные протоколы";
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            filepath = filepath + @"\" + save.Name_file.Text + ".csv";

                            if (!File.Exists(filepath))
                            {
                                //save_name = filepath;
                                ExportToCSV_(filepath);
                                save.Dispose();
                                CommonFun.showbox("Файл успешно экспортирован в формат CSV", "Info");
                            }
                            else
                            {
                                CommonFun.showbox("Файл с таким именем уже существует.", "Info");
                            }

                        }

                    });
                    save.ShowDialog();
                }
            }
        }

        private void ExportToCSV_(string filepath)
        {
            using (Stream stream = File.OpenWrite(filepath))
            {
                stream.SetLength(0);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string string_csv = CommonFun.GetLanText("type") + ";";
                    if (currlist[0].InstrumentsType == "P7")
                        string_csv += "УФ-6700";
                    if (currlist[0].InstrumentsType == "P8")
                        string_csv += "УФ-6800";
                    if (currlist[0].InstrumentsType == "P9")
                        string_csv += "УФ-6900";
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("serino") + ";" + currlist[0].Serials;
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("specturmrange") + "(нм)" + ";" + CommonFun.GetAppConfig("Spectralbandwidth");
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("lightswitch") + "(нм)" + ";" + CommonFun.GetAppConfig("SwithWL");
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("operatemode") + ";";
                    if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                        string_csv += "GLP";
                    else
                        string_csv += CommonFun.GetLanText("common");
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("operators") + ";" + currlist[0].C_Operator;
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("photometricmode") + ";" + this.mpar.C_Mode;
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("scanrange") + "(нм)" + ";" + this.mpar.C_BeginWL + "～" + this.mpar.C_EndWL;
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("scanspeed") + ";" + this.mpar.C_ScanSpeed;
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("scaninterval") + "(нм)" + ";" + this.mpar.C_StepLen;
                    writer.WriteLine(string_csv);

                    string_csv = CommonFun.GetLanText("lightlength") + ";" + this.mpar.C_SLength;
                    writer.WriteLine(string_csv);

                    string lineHeader = "";
                    for (int i = 1; i < this.dataGridView1.Columns.Count; i++)
                    {
                        lineHeader += this.dataGridView1.Columns[i].HeaderText;
                        if (i != this.dataGridView1.Columns.Count - 1)
                            lineHeader += ";";
                    }
                    writer.WriteLine(lineHeader);
                    //Thread.Sleep(500);
                    for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                    {
                        string line = "";
                        // Thread.Sleep(200);
                        for (int j = 1; j < this.dataGridView1.Columns.Count; j++)
                        {
                            string line_ = this.dataGridView1.Rows[i].Cells[j].Value.ToString();
                            line += line_ + ";";
                        }

                        writer.WriteLine(line);
                    }
                    writer.Flush();
                }
            }
        }

        private void GenerateNewSquareData()
        {
            this.currlist[0].Squaredata.Add(new SquareData()
            {
                sqColor = Color.Blue
            });
            this.BindSuqarData();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = this.dataGridView1.CurrentCell.RowIndex;
            if (this.dataGridView1.Rows[rowIndex].Tag == null || (this.currlist.Count <= 0 || this.currlist[0].Data.Count<MeaureData>() <= 0))
                return;
            this.TopDraw(this.currlist[0].Data[rowIndex]);
            this.dataGridView1.Rows[rowIndex].Selected = true;
            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[rowIndex].Cells[0];
        }

        private void TopDraw(MeaureData cumd)
        {
            Bitmap bitmap = new Bitmap(this.picTop.Width, this.picTop.Height);
            Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            double num = (double)this.left + (Convert.ToDouble(cumd.xValue) - (double)this.xs) * this.xInt;
            this.curx = (float)Convert.ToDecimal(cumd.xValue);
            this.ybx = Convert.ToInt32(num);
            if ((double)this.curx >= (double)this.xMin)
                graphics.DrawLine(new Pen(Color.Red, 1f), (float)num - 3f, this.bottom - 3f, (float)num - 3f, this.top - 3f);
            if (this.currlist[0].MethodPar.C_Mode != null)
            {
                if (this.currlist[0].MethodPar.C_Mode == "Abs")
                    this.lblWL.Text = cumd.xValue + " нм," + cumd.yABS.ToString(this.absacc) + " Abs";
                else
                    this.lblWL.Text = cumd.xValue + " нм," + cumd.YT.ToString(this.tacc) + " %T";
            }
            else if (this.mpar.C_Mode == "Abs")
                this.lblWL.Text = cumd.xValue + " нм," + cumd.yABS.ToString(this.absacc) + " Abs";
            else
                this.lblWL.Text = cumd.xValue + " нм," + cumd.YT.ToString(this.tacc) + " %T";
            this.picTop.Image = (System.Drawing.Image)bitmap;
            this.picTop.Refresh();
        }
    }

    public class ExportCSV
    {
        public string type { get; set; }
        public string serino { get; set; }
        public string specturmrange { get; set; }
        public string lightswitch { get; set; }
        public string operatemode { get; set; }
        public string operators { get; set; }
        public string photometricmode { get; set; }
        public string scanrange { get; set; }
        public string scanspeed { get; set; }
        public string scaninterval { get; set; }
        public string lightlength { get; set; }
    }

    /* public class SpectrumScanTabel
     {
         public SpectrumScanTabel() { }
         public SpectrumScanTabel(List<object> array)
         {
             this.WL = Convert.ToString(array[0]);
             this.C_Mode = Convert.ToString(array[1]);
         }
         public SpectrumScanTabel(string WL, string C_Mode)
         {           
             this.WL = WL;
             this.C_Mode = C_Mode;
         }
         private string _value_wl;
         public string WL { get { return _value_wl; } set { _value_wl = value; } }
         private string _value_mode;
         public string C_Mode { get { return _value_mode; } set { _value_mode = value; } }
     }
     public class SpectrWLTable
     {
         public SpectrWLTable() { }
         public SpectrWLTable(List<object> array)
         {
             this.Number = Convert.ToInt32(array[0]);
             this.WL = Convert.ToString(array[1]);
             this.Abs = Convert.ToString(array[2]);
             this.TProcent = Convert.ToString(array[3]);
         }
         public SpectrWLTable(int Number, string WL, string Abs, string TProcent)
         {
             this.Number = Number;
             this.WL = WL;
             this.Abs = Abs;
             this.TProcent = TProcent;
         }

         private int _value_number;
         public int Number { get { return _value_number; } set { _value_number = value; } }
         private string _value_name;
         private string _value_wl;
         public string WL { get { return _value_wl; } set { _value_wl = value; } }
         private string _value_abs;
         public string Abs { get { return _value_abs; } set { _value_abs = value; } }
         private string _value_tprocent;
         public string TProcent { get { return _value_tprocent; } set { _value_tprocent = value; } }

     }*/
}


