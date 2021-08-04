using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using UVStudio.Properties;

namespace UVStudio
{
    public partial class TimeScanFrm : Form
    {
        PictureBox picOut;

        private string lanvalue;
        private int dgvcnt = 0;
        private string tpoint = CommonFun.GetAcc("tAccuracy");
        private string abspoint = CommonFun.GetAcc("absAccuracy");
        private SerialPort sp = new SerialPort();
        private ComStatus ComSta;
        private Thread dealth;
        private bool runptag = true;
        private List<TimeScan> currlist = new List<TimeScan>();
        private TimeMethod mpar = (TimeMethod)null;
        private PrintParams printpar = (PrintParams)null;
        private ShowParams showpar = (ShowParams)null;
        private List<XYRange> xyRangeList = new List<XYRange>();
        private int ZoomOut = 0;
        private float curx;
        private int ybx;
        private int tis = 0;
        private int calormea = 0;
        private bool autoscale = false;
        public List<string> rightlist = new List<string>();
        private Thread tdstart;
        private System.Timers.Timer timersend;
        private System.Windows.Forms.Timer tt = new System.Windows.Forms.Timer();
        private int tickcnt = 0;
        private int sampleno = 0;
        private List<string> slotno = new List<string>();
        private Point start;
        private int downmove = 0;
        private TimeScan sslive = new TimeScan();
        private Queue myque = new Queue();
        private string receive = "";
        private float left;
        private float right;
        private float top;
        private float bottom;
        private int FontSize = 10;
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

        private DataGridViewTextBoxColumn ColNo;
        private DataGridViewTextBoxColumn ColTime;
        private DataGridViewTextBoxColumn ColXGD;

        public TimeScanFrm()
        {
            InitializeComponent();

           

            this.btnPause.Visible = false;
            this.btncancel.Visible = false;

            this.ColNo = new DataGridViewTextBoxColumn();
            this.ColTime = new DataGridViewTextBoxColumn();
            this.ColXGD = new DataGridViewTextBoxColumn();

            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();

            gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewCellStyle1.BackColor = SystemColors.Control;
            gridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 16F);
            gridViewCellStyle1.ForeColor = SystemColors.WindowText;
            gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
            this.dataGridView1.Columns.AddRange((DataGridViewColumn)this.ColNo, (DataGridViewColumn)this.ColTime, (DataGridViewColumn)this.ColXGD);
            gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewCellStyle2.BackColor = SystemColors.Window;
            gridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 16F);
            gridViewCellStyle2.ForeColor = SystemColors.ControlText;
            gridViewCellStyle2.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
            gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = gridViewCellStyle2;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 16f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.ColNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColNo.DefaultCellStyle = gridViewCellStyle3;
            this.ColNo.Name = "ColNo";
            ColNo.HeaderText = "№";
            this.ColNo.ReadOnly = true;
            this.ColTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColTime.DefaultCellStyle = gridViewCellStyle4;
            this.ColTime.FillWeight = 119.5432f;
            this.ColTime.Name = "ColTime";
            ColTime.HeaderText = "Время";
            this.ColTime.ReadOnly = true;
            this.ColXGD.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColXGD.DefaultCellStyle = gridViewCellStyle5;
            this.ColXGD.FillWeight = 119.5432f;
            this.ColXGD.Name = "ColXGD";
            ColXGD.HeaderText = "Abs";
            this.ColXGD.ReadOnly = true;
        }

        private void TimeScanFrm_Load(object sender, EventArgs e)
        {

           // var culture = CultureInfo.InvariantCulture;
            this.dgvcnt = this.dataGridView1.Height / 50 - 1;
            this.dataGridView1.Columns[0].DefaultCellStyle.NullValue = (object)null;
           // this.dataGridView1.Rows.Add(this.dgvcnt);
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView1.RowsDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12f, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12f, FontStyle.Regular);

            this.dataGridView1.EnableHeadersVisualStyles = false;

            this.picTop.BackColor = Color.Transparent;
            this.picTop.Parent = (Control)this.picCurve;
            if (this.mpar == null)
            {
                this.mpar = new TimeMethod();
                try
                {
                    this.mpar.C_mode = CommonFun.getXmlValue("TimeMethod", "C_mode");
                    this.mpar.C_mode = !(this.mpar.C_mode == "Abs") ? CommonFun.GetLanText("T") : CommonFun.GetLanText("Abs");
                    this.mpar.WL = CommonFun.getXmlValue("TimeMethod", "WL");
                    this.mpar.Time = CommonFun.getXmlValue("TimeMethod", "Time");
                    this.mpar.Interval = CommonFun.getXmlValue("TimeMethod", "Interval");
                    this.mpar.Length = CommonFun.getXmlValue("TimeMethod", "Length");
                    this.mpar.EConvert = Convert.ToBoolean(CommonFun.getXmlValue("TimeMethod", "EConvert"));
                }
                catch
                {
                    this.mpar.C_mode = CommonFun.GetLanText("Abs");
                    this.mpar.WL = "546.0";
                    this.mpar.Time = "1200";
                    this.mpar.Interval = "1";
                    this.mpar.Length = "10";
                    this.mpar.EConvert = false;
                }
            }
            if (this.showpar == null)
            {
                this.showpar = new ShowParams();
                try
                {
                    this.showpar.AutoPrint = Convert.ToBoolean(CommonFun.getXmlValue("TimeMethod", "AutoPrint"));
                    this.showpar.AutoSave = Convert.ToBoolean(CommonFun.getXmlValue("TimeMethod", "AutoSave"));
                    this.showpar.AutoXY = Convert.ToBoolean(CommonFun.getXmlValue("TimeMethod", "AutoXY"));
                    this.autoscale = this.showpar.AutoXY;
                    this.showpar.MulShow = Convert.ToInt32(CommonFun.getXmlValue("TimeMethod", "MulShow"));
                   /* if (this.showpar.MulShow == 1)
                        this.btnshowmode.BackgroundImage = (System.Drawing.Image)Resources.Icon_DispCurrent;
                    else
                        this.btnshowmode.BackgroundImage = (System.Drawing.Image)Resources.Icon_DispAll;*/
                    this.showpar.xMax = (float)Convert.ToDouble(CommonFun.getXmlValue("TimeMethod", "xMax"));
                    this.showpar.xMin = (float)Convert.ToDouble(CommonFun.getXmlValue("TimeMethod", "xMin"));
                    this.showpar.yMax = (float)Convert.ToDouble(CommonFun.getXmlValue("TimeMethod", "yMax"));
                    this.showpar.yMin = (float)Convert.ToDouble(CommonFun.getXmlValue("TimeMethod", "yMin"));
                }
                catch
                {
                    this.showpar.AutoPrint = false;
                    this.showpar.AutoSave = false;
                    this.showpar.AutoXY = true;
                    this.autoscale = this.showpar.AutoXY;
                    this.showpar.MulShow = 0;
                    //this.btnshowmode.BackgroundImage = (System.Drawing.Image)Resources.Icon_DispAll;
                    this.showpar.xMax = 1200f;
                    this.showpar.xMin = 0.0f;
                    this.showpar.yMax = 0.0001f;
                    this.showpar.yMin = -0.0001f;
                }
            }

            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ColumnHeadersHeight = 50;

            // this.lblWL.Text = Convert.ToDecimal(this.mpar.WL, culture).ToString("f1") + " nm";
            this.lblWL.Text = Convert.ToDecimal(this.mpar.WL).ToString("f1") + " nm";
            this.DrawLine();
            this.dataGridView1.Columns[2].HeaderText = this.mpar.C_mode;
            this.lblmode.Text = this.mpar.C_mode;
            this.dataGridView1.Refresh();
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("righttimmeasure"))
                    this.btnScan.Enabled = true;
                else
                    this.btnScan.Enabled = false;
                if (this.rightlist.Contains("righttimblank"))
                    this.btnBlank.Enabled = true;
                else
                    this.btnBlank.Enabled = false;
            }
            
            this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
            this.tdstart.Start();
            this.setstate(false);
            if (CommonFun.GetAppConfig("currentconnect") == "-1")
                this.btnBack.Enabled = true;
            this.timersend = new System.Timers.Timer();
            this.timersend.Elapsed += new ElapsedEventHandler(this.timersend_Elapsed);
            this.tt.Interval = 1000;
            this.tt.Tick += new EventHandler(this.tt_Tick);
        }
        private void tt_Tick(object sender, EventArgs e)
        {
            ++this.tickcnt;
            if (this.tickcnt != 60)
                return;
            this.tickcnt = 0;
            this.ComSta = ComStatus.END;
            if (this.btnBlank.InvokeRequired)
                this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_setstate(this.setstate), (object)true);
            else
                this.setstate(true);
        }

        private void timersend_Elapsed(object sender, ElapsedEventArgs e)
        {
            ++this.tis;
            if ((System.Decimal)this.tis * Convert.ToDecimal(this.mpar.Interval) <= (System.Decimal)Convert.ToInt32(this.mpar.Time))
            {
                if (this.slotno.Count > 1)
                {
                    if (this.lblSample.InvokeRequired)
                        this.lblSample.Invoke((Delegate)new TimeScanFrm.Del_SamNameSet(this.SetSname), (object)this.currlist[this.slotno.Count<string>() - 1].C_name);
                    else
                        this.lblSample.Text = this.currlist[this.slotno.Count<string>() - 1].C_name;
                    this.sampleno = 1;
                    this.ComSta = ComStatus.SETCHP;
                    this.sp.WriteLine("SETCHP " + this.slotno[0] + "\r\n");
                    CommonFun.WriteSendLine("SETCHP " + this.slotno[0]);
                }
                else
                {
                    this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(this.mpar.WL) * 10M).ToString("f0") + "\r\n");
                    this.ComSta = ComStatus.MEASURE;
                    CommonFun.WriteSendLine("timer，measure 1 2 " + (Convert.ToDecimal(this.mpar.WL) * 10M).ToString("f0"));
                    CommonFun.WriteSendLine("tis:" + this.tis.ToString() + "," + this.mpar.Time);
                }
            }
            else
                this.timersend.Stop();
        }

        private void setstate(bool status)
        {
            this.btnBack.Enabled = status;
            this.btnBlank.Enabled = status;
            this.btnScan.Enabled = status;
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("righttimmeasure") && status)
                    this.btnScan.Enabled = true;
                else
                    this.btnScan.Enabled = false;
                if (this.rightlist.Contains("righttimblank") && status)
                    this.btnBlank.Enabled = true;
                else
                    this.btnBlank.Enabled = false;
               
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
                this.sp.DataBits = 8;
                this.sp.StopBits = StopBits.One;
                this.sp.PortName = "COM2";
                this.sp.Parity = Parity.None;
                this.sp.ReadTimeout = -1;
                this.sp.Open();
                CommonFun.WriteSendLine("Динамика ");
                this.sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);
                this.ComSta = ComStatus.BD_RATIO_FLUSH;
                this.sp.WriteLine("BD_RATIO_FLUSH \r\n");
                CommonFun.WriteSendLine("BD_RATIO_FLUSH");
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_starttt(this.Starttt), (object)true);
                else
                    this.Starttt(true);
            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.Message.ToString(), "Error");
                this.ComSta = ComStatus.END;
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_setstate(this.setstate), (object)true);
                else
                    this.setstate(true);
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_starttt(this.Starttt), (object)false);
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
        private void splitContainer1_Panel1_SizeChanged(object sender, EventArgs e) => this.DrawLine();

        private void timer1_Tick(object sender, EventArgs e)
        {
            ++this.tis;
            if ((System.Decimal)this.tis * Convert.ToDecimal(this.mpar.Interval) <= (System.Decimal)Convert.ToInt32(this.mpar.Time))
            {
                if (this.slotno.Count > 1)
                {
                    this.lblSample.Text = this.currlist[this.slotno.Count<string>() - 1].C_name;
                    this.sampleno = 1;
                    this.ComSta = ComStatus.SETCHP;
                    this.sp.WriteLine("SETCHP " + this.slotno[0] + "\r\n");
                    CommonFun.WriteSendLine("SETCHP " + this.slotno[0]);
                }
                else
                {
                    this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(this.mpar.WL) * 10M).ToString("f0") + "\r\n");
                    this.ComSta = ComStatus.MEASURE;
                    CommonFun.WriteSendLine("timer，measure 1 2 " + (Convert.ToDecimal(this.mpar.WL) * 10M).ToString("f0"));
                    CommonFun.WriteSendLine("tis:" + this.tis.ToString() + "," + this.mpar.Time);
                }
            }
            else
                this.timer1.Stop();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            /*if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (new DRMessageBoxFrm(CommonFun.GetLanText("exitconfirm"), "Warning").ShowDialog() != DialogResult.Yes)
                    return;
                if (this.currlist != null && this.currlist.Count > 0 && this.currlist.Where<TimeScan>((Func<TimeScan, bool>)(s => !s.status)).ToList<TimeScan>().Count > 0)
                {
                    CommonFun.showbox(CommonFun.GetLanText("datasaveexit"), "Warning");
                    return;
                }
            }
            else if ((this.currlist == null || this.currlist.Count > 0) && (this.currlist.Where<TimeScan>((Func<TimeScan, bool>)(s => !s.status)).ToList<TimeScan>().Count > 0 && new DRMessageBoxFrm(CommonFun.GetLanText("unsavedataexit"), "Warning").ShowDialog() == DialogResult.No))
                return;*/
            try
            {
                CommonFun.setXmlValue("TimeMethod", "WL", this.mpar.WL);
                CommonFun.setXmlValue("TimeMethod", "Interval", this.mpar.Interval);
                CommonFun.setXmlValue("TimeMethod", "Time", this.mpar.Time);
                CommonFun.setXmlValue("TimeMethod", "Length", this.mpar.Length.ToString());
                CommonFun.setXmlValue("TimeMethod", "EConvert", this.mpar.EConvert.ToString());
                if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                    CommonFun.setXmlValue("TimeMethod", "C_mode", "Abs");
                else
                    CommonFun.setXmlValue("TimeMethod", "C_mode", "T");
                CommonFun.setXmlValue("TimeMethod", "AutoPrint", this.showpar.AutoPrint.ToString());
                CommonFun.setXmlValue("TimeMethod", "AutoSave", this.showpar.AutoSave.ToString());
                bool flag = this.showpar.AutoXY;
                CommonFun.setXmlValue("TimeMethod", "AutoXY", flag.ToString());
                CommonFun.setXmlValue("TimeMethod", "MulShow", this.showpar.MulShow.ToString());
                float num = this.showpar.xMax;
                CommonFun.setXmlValue("TimeMethod", "xMax", num.ToString());
                num = this.showpar.xMin;
                CommonFun.setXmlValue("TimeMethod", "xMin", num.ToString());
                num = this.showpar.yMax;
                CommonFun.setXmlValue("TimeMethod", "yMax", num.ToString());
                num = this.showpar.yMin;
                CommonFun.setXmlValue("TimeMethod", "yMin", num.ToString());
                /*CommonFun.setXmlValue("TimePrintParams", "Addtional", this.printpar.Addtional);
                CommonFun.setXmlValue("TimePrintParams", "Describtion", this.printpar.Describtion);
                flag = this.printpar.ShowAddtional;
                CommonFun.setXmlValue("TimePrintParams", "ShowAddtional", flag.ToString());
                flag = this.printpar.ShowCurve;
                CommonFun.setXmlValue("TimePrintParams", "ShowCurve", flag.ToString());
                flag = this.printpar.ShowDes;
                CommonFun.setXmlValue("TimePrintParams", "ShowDes", flag.ToString());
                flag = this.printpar.ShowInsAndUser;
                CommonFun.setXmlValue("TimePrintParams", "ShowInsAndUser", flag.ToString());
                CommonFun.setXmlValue("TimePrintParams", "Title", this.printpar.Title);*/
            }
            catch
            {
                CommonFun.setXmlValue("TimeMethod", "WL", "546.0");
                CommonFun.setXmlValue("TimeMethod", "Interval", "1");
                CommonFun.setXmlValue("TimeMethod", "Time", "180");
                CommonFun.setXmlValue("TimeMethod", "Length", "10");
                CommonFun.setXmlValue("TimeMethod", "EConvert", "False");
                CommonFun.setXmlValue("TimeMethod", "C_mode", "Abs");
                CommonFun.setXmlValue("TimeMethod", "AutoPrint", "False");
                CommonFun.setXmlValue("TimeMethod", "AutoSave", "False");
                CommonFun.setXmlValue("TimeMethod", "AutoXY", "True");
                CommonFun.setXmlValue("TimeMethod", "MulShow", "0");
                CommonFun.setXmlValue("TimeMethod", "xMax", "180");
                CommonFun.setXmlValue("TimeMethod", "xMin", "0");
                CommonFun.setXmlValue("TimeMethod", "yMax", "0.0015");
                CommonFun.setXmlValue("TimeMethod", "yMin", "-0.0015");
                /*CommonFun.setXmlValue("TimePrintParams", "Addtional", "");
                CommonFun.setXmlValue("TimePrintParams", "Describtion", "");
                CommonFun.setXmlValue("TimePrintParams", "ShowAddtional", "False");
                CommonFun.setXmlValue("TimePrintParams", "ShowCurve", "True");
                CommonFun.setXmlValue("TimePrintParams", "ShowDes", "False");
                CommonFun.setXmlValue("TimePrintParams", "ShowInsAndUser", "True");
                CommonFun.setXmlValue("TimePrintParams", "Title", "");*/
            }
            if (this.sp.IsOpen)
            {
                CommonFun.WriteSendLine("Временное сканирование，выход");
                this.sp.Close();
            }
            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();
           // this.Close();
            this.Hide();
            //      CommonFun.WriteLine("Получаем меню");
            MenuProgram menuProgram = new MenuProgram();
            //     CommonFun.WriteLine("Выводим меню");
            menuProgram.Show();
            //    CommonFun.WriteLine("Получаем родительское окно");
            this.Close();
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            tt.Stop();
            using (TimeScanMethodFrm frm = new TimeScanMethodFrm())
            {
                try
                {
                    TimeMethod smpar = new TimeMethod();
                    frm.Save.IsEnabled = false;
                    frm.Finish.IsEnabled = false;
                    frm.LeftSettings.IsEnabled = false;
                    frm.RightSettings.IsEnabled = false;

                    frm.New_method.PreviewMouseDown += ((param_0, param_1) => {
                        //smpar = new TimeMethod();
                        frm.Save.IsEnabled = true;
                        frm.Finish.IsEnabled = true;
                        frm.LeftSettings.IsEnabled = true;
                        frm.RightSettings.IsEnabled = true;
                    });

                    frm.Finish.PreviewMouseDown += ((param0, param1) =>
                    {
                        try
                        {
                            if (smpar.C_methodoperator == null || smpar.C_methodoperator.Length <= 0)
                            {
                                smpar.C_mode = frm.lblModeV.Content.ToString().Replace(" >", "");
                                smpar.Interval = frm.lblIntervalV.Content.ToString().Replace(" >", "");
                                smpar.Length = frm.lblLengthV.Content.ToString().Replace(" >", "");
                                smpar.Time = frm.lblTimeV.Content.ToString().Replace(" >", "");
                                smpar.WL = frm.lblWLV.Content.ToString().Replace(" >", "");
                                smpar.EConvert = !(frm.pibEConvert.Tag.ToString() == "off");
                                if (frm.pibAutoXY.Tag.ToString() == "on")
                                {
                                    this.showpar.AutoXY = true;
                                    this.autoscale = true;
                                }
                                else
                                {
                                    this.showpar.AutoXY = false;
                                    this.autoscale = false;
                                    this.showpar.xMin = 0.0f;
                                    this.showpar.xMax = (float)Convert.ToInt32(smpar.Time);
                                    if ((double)Convert.ToSingle(frm.lblYMaxV.Content.ToString().Replace(" >", "")) > (double)Convert.ToSingle(frm.lblYMinV.Content.ToString().Replace(" >", "")))
                                    {
                                        this.showpar.yMax = Convert.ToSingle(frm.lblYMaxV.Content.ToString().Replace(" >", ""));
                                        this.showpar.yMin = Convert.ToSingle(frm.lblYMinV.Content.ToString().Replace(" >", ""));
                                    }
                                    else
                                    {
                                        this.showpar.yMax = Convert.ToSingle(frm.lblYMinV.Content.ToString().Replace(" >", ""));
                                        this.showpar.yMin = Convert.ToSingle(frm.lblYMaxV.Content.ToString().Replace(" >", ""));
                                    }
                                }
                                //this.showpar.AutoPrint = !(frm.pibAutoPrint.Tag.ToString() == "off");
                               // this.showpar.AutoSave = !(frm.pibAutoSave.Tag.ToString() == "off");
                                if (CommonFun.GetAppConfig("GLPEnabled") == "true" && smpar.ParamsCreatorES == null)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("savemethodwithes"), "Error");
                                    return;
                                }
                                smpar.spar = this.showpar;
                                smpar.C_methodoperator = CommonFun.GetAppConfig("currentuser");
                                smpar.D_time = new DateTime?(DateTime.Now);
                            }
                            else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && (smpar.ParamsCreatorES == null || smpar.ParamsCreatorES.Length <= 0))
                            {
                                CommonFun.showbox(CommonFun.GetLanText("noesmethod"), "Error");
                                return;
                            }
                            this.mpar = smpar;
                            frm.Close();
                            this.dataGridView1.Columns["ColXGD"].HeaderText = this.mpar.C_mode;
                            this.lblmode.Text = this.mpar.C_mode;
                            if (this.currlist.Count > 0)
                            {
                                this.dataGridView1.Rows.Clear();
                                for (int index = 0; index < this.currlist[0].Data.Count<MeaureData>(); ++index)
                                {
                                    this.dataGridView1.Rows.Add();
                                    this.dataGridView1.Rows[index].Cells["ColNo"].Value = (object)(index + 1).ToString();
                                    this.dataGridView1.Rows[index].Cells["ColTime"].Value = (object)this.currlist[0].Data[index].xValue;
                                    this.dataGridView1.Rows[index].Cells["ColXGD"].Value = !(this.mpar.C_mode == CommonFun.GetLanText("Abs")) ? (object)this.currlist[0].Data[index].YT.ToString(this.tpoint) : (object)this.currlist[0].Data[index].yABS.ToString(this.abspoint);
                                    this.dataGridView1.Rows[index].Tag = (object)this.currlist[0].Data[index];
                                }
                                if (this.currlist[0].Data.Count < this.dgvcnt)
                                    this.dataGridView1.Rows.Add(this.dgvcnt - this.currlist[0].Data.Count);
                                this.ResetTempReDraw(1);
                            }
                        }
                        catch
                        {
                            CommonFun.showbox(CommonFun.GetLanText("parerror"), "Error");
                            return;
                        }
                        try
                        {
                            CommonFun.InsertLog(CommonFun.GetLanText("timescan"), CommonFun.GetLanText("logupdateM"), false);
                        }
                        catch
                        {
                            CommonFun.WriteLine("Сканирование времени, подтвердите ошибку вставки журнала метода");
                        }
                    });
                    frm.ShowDialog();
                }
                catch{

                }
            }
            tt.Start();
        }

        private void btnBlank_Click(object sender, EventArgs e)
        {
            if (this.mpar.C_methodoperator == null || this.mpar.C_methodoperator.Length <= 0)
            {
                CommonFun.showbox(CommonFun.GetLanText("nomethod"), "Error");
            }
            else
            {
                string errormsg = "";
                if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                    CommonFun.showbox(errormsg, "Error");
                else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                    CommonFun.showbox(errormsg, "Error");
                else if (!this.sp.IsOpen)
                    CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                else if (this.ComSta == ComStatus.CALBGND)
                {
                    CommonFun.WriteSendLine("stop，");
                    this.ComSta = ComStatus.END;
                    this.btnBlank.Text = CommonFun.GetLanText("blanking");
                    this.progressBar1.Visible = false;
                    this.setState(ComStatus.END);
                }
                else if (this.ComSta != ComStatus.END)
                {
                    CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
                }
                else
                {
                    this.ResetTempReDraw(0);
                    if (CommonFun.GetAppConfig("EightSlot") == "true")
                    {
                        this.calormea = 1;
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
                                frm.Close();
                                this.ComSta = ComStatus.SETCHP;
                                this.sp.WriteLine("SETCHP " + str + "\r\n");
                                CommonFun.WriteSendLine("SETCHP " + str);
                                this.setState(ComStatus.CALBGND);
                               // this.panel4.Visible = true;
                               // this.lblProsess.Text = CommonFun.GetLanText("inblanking");
                                this.progressBar1.Value = 5;
                                this.btnBlank.Text = CommonFun.GetLanText("stopblanking");
                                CommonFun.InsertLog(CommonFun.GetLanText("timescan"), CommonFun.GetLanText("blanking"), false);
                            });
                            int num = (int)frm.ShowDialog();
                        }*/
                    }
                    else
                    {
                        this.btnBlank.Text = CommonFun.GetLanText("blanking");
                        //this.panel4.Visible = true;
                        this.btnPause.Visible = false;
                        //this.lblProsess.Text = CommonFun.GetLanText("inblanking");
                        this.progressBar1.Value = 5;
                        string wl = this.mpar.WL;
                        this.ComSta = ComStatus.CALBGND;
                        SerialPort sp = this.sp;
                        System.Decimal num = Convert.ToDecimal(wl) * 10M;
                        string text = "calbgnd 1 1 " + num.ToString("f0") + "\r\n";
                        sp.WriteLine(text);
                        num = Convert.ToDecimal(wl) * 10M;
                        CommonFun.WriteSendLine("calbgnd 1 1 " + num.ToString("f0"));
                        CommonFun.InsertLog(CommonFun.GetLanText("timescan"), CommonFun.GetLanText("blanking"), false);
                        this.setState(ComStatus.CALBGND);
                    }
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (this.mpar.C_methodoperator == null || this.mpar.C_methodoperator.Length <= 0)
            {
                CommonFun.showbox(CommonFun.GetLanText("nomethod"), "Error");
            }
            else
            {
                string errormsg = "";
                if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                    CommonFun.showbox(errormsg, "Error");
                else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                    CommonFun.showbox(errormsg, "Error");
                else if (!this.sp.IsOpen)
                    CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                else if (this.btnScan.Text == CommonFun.GetLanText("stopmeasure"))
                {
                    CommonFun.WriteSendLine("stop,");
                    this.ComSta = ComStatus.END;
                    this.receive = "";
                    this.btnScan.Text = CommonFun.GetLanText("measure");
                    this.setState(ComStatus.END);
                    this.timersend.Stop();
                    this.sampleno = 0;
                    this.slotno = new List<string>();
                }
                else if (this.ComSta != ComStatus.END)
                {
                    CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
                }
                else
                {
                    this.ResetTempReDraw(0);
                    this.tis = 0;
                    if (CommonFun.GetAppConfig("EightSlot") == "true")
                    {
                        this.calormea = 2;
                       /* using (ChoseSlotFrm frm = new ChoseSlotFrm())
                        {
                            frm.btnOK.Click += (EventHandler)((param0, param1) =>
                            {
                                this.slotno = new List<string>();
                                if (frm.chk1.Checked)
                                    this.slotno.Add("1");
                                if (frm.chk2.Checked)
                                    this.slotno.Add("2");
                                if (frm.chk3.Checked)
                                    this.slotno.Add("3");
                                if (frm.chk4.Checked)
                                    this.slotno.Add("4");
                                if (frm.chk5.Checked)
                                    this.slotno.Add("5");
                                if (frm.chk6.Checked)
                                    this.slotno.Add("6");
                                if (frm.chk7.Checked)
                                    this.slotno.Add("7");
                                if (frm.chk8.Checked)
                                    this.slotno.Add("8");
                                if (this.slotno.Count <= 0)
                                    return;
                                if (this.slotno.Count > 1)
                                {
                                    IEnumerable<int> source = this.slotno.Select<string, int>((Func<string, int>)(x => Convert.ToInt32(x)));
                                    int num = source.Max() - source.Min();
                                    string interval = this.mpar.Interval;
                                    if (this.slotno.Count * 2 > Convert.ToInt32(interval) || num > Convert.ToInt32(interval))
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("timerequire"), "Error");
                                        return;
                                    }
                                    CommonFun.showbox(CommonFun.GetLanText("timerequire"), "Warning");
                                }
                                this.btnScan.Text = CommonFun.GetLanText("stopmeasure");
                                this.panel4.Visible = true;
                                this.btnPause.Visible = true;
                                this.lblProsess.Text = CommonFun.GetLanText("measureing");
                                this.progressBar1.Value = 5;
                                frm.Close();
                                for (int index1 = 0; index1 < this.slotno.Count<string>(); ++index1)
                                {
                                    this.sslive = new TimeScan();
                                    this.sslive.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                                    this.sslive.MethodPar = this.mpar;
                                    this.sslive.C_name = this.getName("");
                                    this.sslive.C_Operator = CommonFun.GetAppConfig("currentuser");
                                    this.sslive.color = this.GetColor();
                                    this.sslive.D_Time = DateTime.Now;
                                    this.sslive.Data = new List<MeaureData>();
                                    this.sslive.status = false;
                                    this.sslive.IsShow = 1;
                                    if (this.slotno.Count > 1)
                                        this.showpar.MulShow = 1;
                                    if (this.currlist.Count > 0)
                                    {
                                        this.currlist.Add(this.sslive);
                                        for (int index2 = this.currlist.Count - 1; index2 > 0; --index2)
                                        {
                                            this.currlist[index2] = this.currlist[index2 - 1];
                                            if (this.showpar.MulShow == 0)
                                                this.currlist[index2].IsShow = 0;
                                        }
                                        this.currlist[0] = this.sslive;
                                    }
                                    else
                                        this.currlist.Add(this.sslive);
                                }
                                this.lblSample.Text = this.currlist[this.slotno.Count<string>() - 1].C_name;
                                this.lblSample.Visible = true;
                                this.sampleno = 1;
                                this.ComSta = ComStatus.SETCHP;
                                this.sp.WriteLine("SETCHP " + this.slotno[0] + "\r\n");
                                CommonFun.WriteSendLine("SETCHP " + this.slotno[0]);
                                this.setState(ComStatus.MEASURE);
                                CommonFun.InsertLog(CommonFun.GetLanText("timescan"), CommonFun.GetLanText("measure"), false);
                            });
                            int num1 = (int)frm.ShowDialog();
                        }*/
                    }
                    else
                    {
                        this.btnScan.Text = CommonFun.GetLanText("stopmeasure");
                        //this.panel4.Visible = true;
                        this.btnPause.Visible = true;
                        //this.lblProsess.Text = CommonFun.GetLanText("measureing");
                        this.progressBar1.Value = 5;
                        this.sslive = new TimeScan();
                        this.sslive.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                        this.sslive.Serials = CommonFun.GetAppConfig("serialno");
                        this.sslive.MethodPar = this.mpar;
                        //this.sslive.C_name = this.getName("");
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
                            this.currlist.Add(this.sslive);
                        this.lblSample.Text = this.sslive.C_name;
                        this.lblSample.Visible = true;
                        SerialPort sp = this.sp;
                        System.Decimal num = Convert.ToDecimal(this.mpar.WL) * 10M;
                        string text = "measure 1 2 " + num.ToString("f0") + "\r\n";
                        sp.WriteLine(text);
                        this.ComSta = ComStatus.MEASURE;
                        num = Convert.ToDecimal(this.mpar.WL) * 10M;
                        CommonFun.WriteSendLine("measure 1 2 " + num.ToString("f0"));
                        this.timersend.Interval = Convert.ToDouble(Convert.ToDecimal(this.mpar.Interval) * 1000M);
                        this.timersend.Start();
                        CommonFun.InsertLog(CommonFun.GetLanText("timescan"), CommonFun.GetLanText("measure"), false);
                        this.setState(ComStatus.MEASURE);
                    }
                }
            }
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
                    //this.btnOperate.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnOpen.Enabled = false;
                   // this.btnSample.Enabled = false;
                   // this.btnSearch.Enabled = false;
                    this.lblSample.Enabled = false;
                    this.panel1.Enabled = false;
                    break;
                case ComStatus.CALBGND:
                    this.btnBlank.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = false;
                    this.btnScan.Enabled = false;
                    //this.btnOperate.Enabled = false;
                    //this.btnSample.Enabled = false;
                    //this.btnSearch.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnOpen.Enabled = false;
                    this.lblSample.Enabled = false;
                    this.panel1.Enabled = false;
                    break;
                case ComStatus.END:
                    this.btnScan.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = true;
                    this.btnBlank.Enabled = true;
                    /*this.btnOperate.Enabled = true;
                    this.btnSample.Enabled = true;
                    this.btnSearch.Enabled = true;*/
                    this.btnSave.Enabled = true;
                    this.btnOpen.Enabled = true;
                    this.lblSample.Enabled = true;
                    this.panel1.Enabled = true;
                    break;
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
                    case ComStatus.MEASURE:
                    case ComStatus.CALBGND:
                    case ComStatus.SCANBASE:
                    case ComStatus.BD_RATIO_FLUSH:
                        string text = this.sp.ReadLine();
                        this.myque.Enqueue((object)text);
                        CommonFun.WriteReceiveMsg(text);
                        break;
                }
            }
            else
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
        }

        private void DealRecData()
        {
            while (this.runptag)
            {
                Queue queue = Queue.Synchronized(this.myque);
                if (queue.Count > 0)
                {
                    string text1 = queue.Dequeue().ToString();
                    try
                    {
                        switch (this.ComSta)
                        {
                            case ComStatus.SETCHP:
                                if (this.calormea == 1)
                                {
                                    this.ComSta = ComStatus.CALBGND;
                                    SerialPort sp = this.sp;
                                    System.Decimal num = Convert.ToDecimal(this.mpar.WL) * 10M;
                                    string text2 = "calbgnd 1 1 " + num.ToString("f0") + "\r\n";
                                    sp.WriteLine(text2);
                                    num = Convert.ToDecimal(this.mpar.WL) * 10M;
                                    CommonFun.WriteSendLine("calbgnd 1 1 " + num.ToString("f0"));
                                    break;
                                }
                                this.ComSta = ComStatus.MEASURE;
                                SerialPort sp1 = this.sp;
                                System.Decimal num1 = Convert.ToDecimal(this.mpar.WL) * 10M;
                                string text3 = "measure 1 2 " + num1.ToString("f0") + "\r\n";
                                sp1.WriteLine(text3);
                                num1 = Convert.ToDecimal(this.mpar.WL) * 10M;
                                CommonFun.WriteSendLine("measure 1 2 " + num1.ToString("f0"));
                                if (this.btnBack.InvokeRequired)
                                {
                                    this.btnBack.Invoke((Delegate)new TimeScanFrm.Del_starttimer(this.Starttimer));
                                }
                                else
                                {
                                    this.timersend.Interval = (double)Convert.ToInt32(Convert.ToDecimal(this.mpar.Interval) * 1000M);
                                    this.timersend.Start();
                                }
                                break;
                            case ComStatus.MEASURE:
                                CommonFun.WriteLine(text1);
                                this.receive += text1;
                                try
                                {
                                    if (text1.Contains("END"))
                                    {
                                        if (!this.receive.Contains("*CALDAT"))
                                        {
                                            string receive = this.receive;
                                            this.receive = "";
                                            int startIndex = receive.IndexOf("*DAT") + 5;
                                            int num2 = receive.IndexOf("END");
                                            if (startIndex < receive.Length && num2 > 0 && num2 > startIndex)
                                            {
                                                string[] strArray = receive.Substring(startIndex, num2 - startIndex).Split(' ');
                                                MeaureData md = new MeaureData()
                                                {
                                                    YT = (float)Convert.ToDecimal(strArray[0])
                                                };
                                                md.yABS = (double)md.YT > 0.01 ? (float)(2.0 - Math.Log10(Convert.ToDouble(md.YT))) : 4f;
                                                if (this.mpar.Length != "10" && this.mpar.EConvert)
                                                {
                                                    md.yABS *= (float)(Convert.ToDouble(10) / Convert.ToDouble(this.mpar.Length));
                                                    md.YT = (float)Math.Pow(10.0, 2.0 - (double)md.yABS);
                                                }
                                                if (this.slotno.Count > 1)
                                                    this.currlist[this.slotno.Count - this.sampleno].Data.Add(md);
                                                else
                                                    this.currlist[0].Data.Add(md);
                                                if (this.dataGridView1.InvokeRequired)
                                                    this.dataGridView1.Invoke((Delegate)new TimeScanFrm.Del_BindData(this.BindDataWhileScan), (object)md);
                                                else
                                                    this.BindDataWhileScan(md);
                                                if (this.picCurve.InvokeRequired)
                                                {
                                                    this.picCurve.Invoke((Delegate)new TimeScanFrm.Del_MeaData(this.DealMeaData));
                                                }
                                                else
                                                {
                                                    this.XYMaxMin();
                                                    if (this.showpar.AutoXY)
                                                        this.AddToRangeList(0.0f, Convert.ToSingle(this.mpar.Time), this.yMin, this.yMax);
                                                    else
                                                        this.AddToRangeList(this.showpar.xMin, this.showpar.xMax, this.showpar.yMin, this.showpar.yMax);
                                                }
                                            }
                                            if (this.sampleno < this.slotno.Count)
                                            {
                                                ++this.sampleno;
                                                this.ComSta = ComStatus.SETCHP;
                                                this.sp.WriteLine("SETCHP " + this.slotno[this.sampleno - 1] + "\r\n");
                                                CommonFun.WriteSendLine("SETCHP " + this.slotno[this.sampleno - 1]);
                                            }
                                            else
                                            {
                                                int int32 = Convert.ToInt32((System.Decimal)this.tis * Convert.ToDecimal(this.mpar.Interval) * 100M / (System.Decimal)Convert.ToInt32(this.mpar.Time));
                                                if (this.progressBar1.InvokeRequired)
                                                    this.Invoke((Delegate)new TimeScanFrm.SetPos(this.SetTextMessage), (object)int32);
                                                else
                                                    this.progressBar1.Value = Convert.ToInt32(int32);
                                                if ((System.Decimal)(this.tis + 1) * Convert.ToDecimal(this.mpar.Interval) > (System.Decimal)Convert.ToInt32(this.mpar.Time))
                                                {
                                                    this.ComSta = ComStatus.END;
                                                    if (this.picCurve.InvokeRequired)
                                                    {
                                                        this.picCurve.Invoke((Delegate)new TimeScanFrm.Del_MeaData(this.DealMeaData));
                                                    }
                                                    else
                                                    {
                                                        this.XYMaxMin();
                                                        if (this.showpar.AutoXY)
                                                            this.AddToRangeList(0.0f, Convert.ToSingle(this.mpar.Time), this.yMin, this.yMax);
                                                        else
                                                            this.AddToRangeList(this.showpar.xMin, this.showpar.xMax, this.showpar.yMin, this.showpar.yMax);
                                                    }
                                                    CommonFun.showbox(CommonFun.GetLanText("measurefinish"), "Information");
                                                    if (this.btnScan.InvokeRequired)
                                                        this.btnScan.Invoke((Delegate)new TimeScanFrm.Del_SetStop(this.SetStop));
                                                    else
                                                        this.SetStop();
                                                    this.slotno = new List<string>();
                                                    this.sampleno = 0;
                                                   
                                                }
                                                this.sampleno = 1;
                                            }
                                        }
                                        else
                                            this.receive = "";
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("errorstopmeasure") + ex.ToString(), "Error");
                                    CommonFun.WriteSendLine("error，");
                                    this.ComSta = ComStatus.END;
                                    this.receive = "";
                                    this.timersend.Stop();
                                    if (this.btnScan.InvokeRequired)
                                        this.btnScan.Invoke((Delegate)new TimeScanFrm.Del_SetStop(this.SetStop));
                                    else
                                        this.SetStop();
                                    this.slotno = new List<string>();
                                    this.sampleno = 0;
                                    break;
                                }
                            case ComStatus.CALBGND:
                                CommonFun.WriteLine(text1);
                                try
                                {
                                    if (text1.Contains("END"))
                                    {
                                        this.ComSta = ComStatus.END;
                                        this.receive = "";
                                        if (this.btnBlank.InvokeRequired)
                                            this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_SetBlankLabel(this.Setblanklabel));
                                        else
                                            this.Setblanklabel();
                                        CommonFun.showbox(CommonFun.GetLanText("blankfinish"), "Information");
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("errorstopblank") + ex.ToString(), "Error");
                                    CommonFun.WriteSendLine("error，");
                                    this.ComSta = ComStatus.END;
                                    this.receive = "";
                                    if (this.btnBlank.InvokeRequired)
                                    {
                                        this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_SetBlankLabel(this.Setblanklabel));
                                        break;
                                    }
                                    this.Setblanklabel();
                                    break;
                                }
                            case ComStatus.BD_RATIO_FLUSH:
                                if (text1.Contains("RCVD"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (this.btnBlank.InvokeRequired)
                                        this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_setstate(this.setstate), (object)true);
                                    else
                                        this.setstate(true);
                                    if (this.btnBlank.InvokeRequired)
                                        this.btnBlank.Invoke((Delegate)new TimeScanFrm.Del_starttt(this.Starttt), (object)false);
                                    else
                                        this.Starttt(false);
                                    break;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errorstopblank") + ex.ToString(), "Error");
                    }
                }
            }
        }
        private void Setblanklabel()
        {
            this.progressBar1.Value = 100;
            this.btnBlank.Text = CommonFun.GetLanText("blanking");
           // this.panel4.Visible = false;
            this.lblWL.Text = Convert.ToDecimal(this.mpar.WL).ToString("f1") + " nm";
            this.setState(ComStatus.END);
        }

        private void SetStop()
        {
            this.progressBar1.Value = 100;
            this.btnScan.Text = CommonFun.GetLanText("measure");
         //   this.panel4.Visible = false;
            if (this.ComSta == ComStatus.END)
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[0].Cells[0];
            this.setState(ComStatus.END);
        }

        private void SetTextMessage(int ipos)
        {
            if (ipos > 100)
                ipos = 100;
            this.progressBar1.Value = Convert.ToInt32(ipos);
        }

        private void SetSname(string name) => this.lblSample.Text = this.currlist[0].C_name;

        private void Starttimer()
        {
            this.timersend.Interval = Convert.ToDouble(Convert.ToDecimal(this.mpar.Interval) * 1000M);
            this.timersend.Start();
        }

        private void DealMeaData()
        {
            this.XYMaxMin();
            if (this.showpar.AutoXY)
                this.AddToRangeList(0.0f, Convert.ToSingle(this.mpar.Time), this.yMin, this.yMax);
            else
                this.AddToRangeList(this.showpar.xMin, this.showpar.xMax, this.showpar.yMin, this.showpar.yMax);
            if (this.ComSta != ComStatus.END)
                return;
            this.btnScan.Text = CommonFun.GetLanText("measure");
            this.lblSample.Text = this.currlist[0].C_name;
            this.lblSample.Visible = true;
        }
        private void BindDataWhileScan(MeaureData md)
        {
            if (this.slotno.Count > 1)
            {
                this.lblSample.Text = this.currlist[this.slotno.Count<string>() - this.sampleno].C_name;
                this.dataGridView1.Rows.Clear();
                for (int index = 0; index < this.currlist[this.slotno.Count - this.sampleno].Data.Count; ++index)
                {
                    this.dataGridView1.Rows.Add();
                    MeaureData meaureData = this.currlist[this.slotno.Count - this.sampleno].Data[index];
                    this.dataGridView1.Rows[index].Cells["ColNo"].Value = (object)(index + 1);
                    meaureData.xValue = (Convert.ToDecimal(this.mpar.Interval) * (System.Decimal)index).ToString("f1");
                    this.dataGridView1.Rows[index].Cells["ColTime"].Value = (object)meaureData.xValue;
                    if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                    {
                        this.dataGridView1.Rows[index].Cells["ColXGD"].Value = (object)meaureData.yABS.ToString(this.abspoint);
                        this.lbllocation.Text = meaureData.xValue + "s," + meaureData.yABS.ToString(this.abspoint) + "A";
                    }
                    else
                    {
                        this.dataGridView1.Rows[index].Cells["ColXGD"].Value = (object)meaureData.YT.ToString(this.tpoint);
                        this.lbllocation.Text = meaureData.xValue + "s," + meaureData.YT.ToString(this.tpoint) + "T";
                    }
                    this.dataGridView1.Rows[index].Tag = (object)meaureData;
                }
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.currlist[this.slotno.Count - this.sampleno].Data.Count - 1].Cells[0];
            }
            else
            {
                this.dataGridView1.Rows.Add();
                int count = this.currlist[0].Data.Count;
                if (this.dataGridView1.Rows.Count < count)
                    return;
                this.dataGridView1.Rows[count - 1].Cells["ColNo"].Value = (object)count;
                md.xValue = (Convert.ToDecimal(this.mpar.Interval) * (System.Decimal)(count - 1)).ToString("f1");
                this.dataGridView1.Rows[count - 1].Cells["ColTime"].Value = (object)md.xValue;
                if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                {
                    this.dataGridView1.Rows[count - 1].Cells["ColXGD"].Value = (object)md.yABS.ToString(this.abspoint);
                    this.lbllocation.Text = md.xValue + "s," + md.yABS.ToString(this.abspoint) + "A";
                }
                else
                {
                    this.dataGridView1.Rows[count - 1].Cells["ColXGD"].Value = (object)md.YT.ToString(this.tpoint);
                    this.lbllocation.Text = md.xValue + "s," + md.YT.ToString(this.tpoint) + "T";
                }
                this.dataGridView1.Rows[count - 1].Tag = (object)md;
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[count - 1].Cells[0];
            }
        }
        private void DrawLine()
        {
            if (this.mpar == null)
                return;
            Bitmap bitmap = new Bitmap(this.picCurve.Width, this.picCurve.Height);
            Graphics objGraphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            objGraphics.FillRectangle((Brush)new SolidBrush(SystemColors.Control), 1, 1, this.picCurve.Width - 2, this.picCurve.Height - 2);
            SizeF sizeF1 = objGraphics.MeasureString("0.0001", new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            SizeF sizeF2 = this.currlist.Count > 0 && this.currlist[0].Data.Count > 0 ? objGraphics.MeasureString(Convert.ToDouble(this.currlist[0].Data.Select<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue)).Max<string>()).ToString("f1"), new System.Drawing.Font("Segoe UI", (float)this.FontSize)) : objGraphics.MeasureString("100.0", new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            this.left = sizeF2.Height + 20f + sizeF1.Width;
            this.right = (float)(this.picCurve.Width - 5);
            this.bottom = (float)((double)this.picCurve.Height - (double)sizeF2.Height * 2.0 - 20.0);
            this.top = sizeF2.Height + 20f;
            RectangleF rectangleF = new RectangleF(this.left, this.top, this.right - this.left, this.bottom - this.top);
            if (this.currlist.Count <= 0 || this.currlist[0].Data.Count <= 0)
            {
                this.xs = 0.0f;
                this.xe = Convert.ToSingle(this.mpar.Time);
                this.ys = this.showpar.yMin;
                this.ye = this.showpar.yMax;
                if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                    this.DrawScale(ref objGraphics, 0, "Abs");
                else
                    this.DrawScale(ref objGraphics, 0, "T");
            }
            else
            {
                List<XYRange> list = this.xyRangeList.Where<XYRange>((Func<XYRange, bool>)(s => s.Curr)).ToList<XYRange>();
                if (list.Count < 1)
                {
                    CommonFun.showbox(CommonFun.GetLanText("norangedata"), "Error");
                    return;
                }
                XYRange xyr = list[0];
                this.xs = xyr.X1;
                this.xe = xyr.X2;
                this.ys = xyr.Y1;
                this.ye = xyr.Y2;
                if (this.autoscale)
                {
                    if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                    {
                        string str = "0";
                        switch (this.abspoint)
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
                        string str = "0";
                        switch (this.tpoint)
                        {
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
                        if ((double)this.ye - (double)this.ys < 2.0 * (double)Convert.ToSingle(str))
                        {
                            this.ye += 2f * Convert.ToSingle(str);
                            this.ys -= 2f * Convert.ToSingle(str);
                        }
                        else if ((double)this.ye - (double)this.ys < 4.0 * (double)Convert.ToSingle(str))
                        {
                            this.ye += Convert.ToSingle(str);
                            this.ys -= Convert.ToSingle(str);
                        }
                        else
                        {
                            this.ye = (float)((1.8 * (double)this.ye - 0.2 * (double)this.ys) / 1.6);
                            this.ys = (float)((1.8 * (double)this.ys - 0.2 * (double)this.ye) / 1.6);
                        }
                    }
                }
                if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                    this.DrawScale(ref objGraphics, this.currlist[0].Data.Count<MeaureData>(), "Abs");
                else
                    this.DrawScale(ref objGraphics, this.currlist[0].Data.Count<MeaureData>(), "T");
                List<TimeScan> timeScanList = new List<TimeScan>();
                timeScanList.Add(this.currlist[0]);
                if (this.showpar.MulShow == 1)
                    timeScanList.AddRange((IEnumerable<TimeScan>)this.currlist.Where<TimeScan>((Func<TimeScan, bool>)(s => s.IsShow == 1)).ToList<TimeScan>());
                foreach (TimeScan timeScan in timeScanList)
                {
                    List<MeaureData> source = timeScan.Data;
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
                    double num2 = !(this.mpar.C_mode == CommonFun.GetLanText("Abs")) ? (double)this.bottom - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].YT) - (double)this.ys) * this.yInt : (double)this.bottom - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].yABS) - (double)this.ys) * this.yInt;
                    if (num2 < (double)this.top)
                        num2 = (double)this.top;
                    if (num2 > (double)this.bottom)
                        num2 = (double)this.bottom;
                    for (int index3 = source.Count<MeaureData>() - 2; index3 >= 0; --index3)
                    {
                        double num3 = (double)this.left + (Convert.ToDouble(source[index3].xValue) - (double)this.xs) * this.xInt;
                        double num4 = !(this.mpar.C_mode == CommonFun.GetLanText("Abs")) ? (Convert.ToDouble(source[index3].YT) > (double)this.ys ? (Convert.ToDouble(source[index3].YT) < (double)this.ye ? (double)this.bottom - (Convert.ToDouble(source[index3].YT) - (double)this.ys) * this.yInt : (double)this.top) : (double)this.bottom) : (Convert.ToDouble(source[index3].yABS) > (double)this.ys ? (Convert.ToDouble(source[index3].yABS) < (double)this.ye ? (double)this.bottom - (Convert.ToDouble(source[index3].yABS) - (double)this.ys) * this.yInt : (double)this.top) : (double)this.bottom);
                        objGraphics.DrawLine(new Pen(timeScan.color, 1f), (float)num1, (float)num2, (float)num3, (float)num4);
                        num1 = num3;
                        num2 = num4;
                    }
                }
            }
            this.picCurve.Image = (System.Drawing.Image)bitmap;
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
            SizeF sizeF1 = objGraphics.MeasureString(CommonFun.GetLanText("time") + "(s)", new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            SizeF sizeF2 = objGraphics.MeasureString(C_mode, new System.Drawing.Font("Segoe UI", (float)this.FontSize));
            string format;
            if (this.showpar.AutoXY)
            {
                format = !(C_mode == "Abs") ? this.tpoint : this.abspoint;
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
                pen.DashStyle = DashStyle.Dot;
                objGraphics.DrawLine(pen, this.left + (float)(((double)this.right - (double)this.left) * (double)index / 4.0), this.bottom, this.left + (float)(((double)this.right - (double)this.left) * (double)index / 4.0), this.top);
                objGraphics.DrawLine(pen, this.left, this.top + (float)(((double)this.bottom - (double)this.top) * (double)index / 4.0), this.right, this.top + (float)(((double)this.bottom - (double)this.top) * (double)index / 4.0));
            }
            float x4 = this.left + (float)(((double)this.right - (double)this.left - (double)sizeF1.Width) / 2.0);
            float y4 = this.bottom + 5f;
            objGraphics.DrawString(CommonFun.GetLanText("time") + "(s)", new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(x4, y4));
            float x5 = this.left - objGraphics.MeasureString(C_mode, new System.Drawing.Font("Segoe UI", (float)this.FontSize)).Width;
            float y5 = this.top + (float)(((double)this.bottom - (double)this.top - (double)sizeF2.Height) / 2.0);
            objGraphics.DrawString(C_mode, new System.Drawing.Font("Segoe UI", (float)this.FontSize), (Brush)new SolidBrush(Color.Black), new PointF(x5, y5));
        }

        private void DrawOutLine(TimeScan ts)
        {
            this.picOut.Height = 240;
            this.picOut.Width = 420;
            if (ts.Data.Count <= 0)
                return;
            Bitmap bitmap = new Bitmap(this.picOut.Width, this.picOut.Height);
            Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            graphics.FillRectangle((Brush)new SolidBrush(Color.White), 0, 0, this.picOut.Width, this.picOut.Height);
            SizeF sizeF1 = graphics.MeasureString(Convert.ToDouble(ts.Data.Select<MeaureData, string>((Func<MeaureData, string>)(s => s.xValue)).Max<string>()).ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float num1 = (float)((double)sizeF1.Height * 2.0 + 20.0);
            float num2 = (float)(this.picOut.Width - 5);
            float num3 = (float)this.picOut.Height - sizeF1.Height * 2f;
            float num4 = sizeF1.Height + 5f;
            RectangleF rectangleF = new RectangleF(num1, num4, num2 - num1, num3 - num4);
            List<XYRange> list = this.xyRangeList.Where<XYRange>((Func<XYRange, bool>)(s => s.Curr)).ToList<XYRange>();
            if (list.Count < 1)
                return;
            XYRange xyr = list[0];
            float x1 = xyr.X1;
            float x2 = xyr.X2;
            float num5 = xyr.Y1;
            float num6 = xyr.Y2;
            if (this.autoscale && this.autoscale)
            {
                if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                {
                    string str = "0";
                    switch (this.abspoint)
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
                    if ((double)Math.Abs(num6) < (double)Convert.ToSingle(str))
                        num6 = Convert.ToSingle(str);
                    if ((double)Math.Abs(num5) < (double)Convert.ToSingle(str))
                        num5 = -Convert.ToSingle(str);
                    if ((double)Math.Abs(num6) > (double)Convert.ToSingle(str) || (double)Math.Abs(num5) > (double)Convert.ToSingle(str))
                    {
                        num6 = (float)((1.8 * (double)num6 - 0.2 * (double)num5) / 1.6);
                        num5 = (float)((1.8 * (double)num5 - 0.2 * (double)num6) / 1.6);
                    }
                }
                else
                {
                    string str = "0";
                    switch (this.tpoint)
                    {
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
                    if ((double)num6 - (double)num5 < 2.0 * (double)Convert.ToSingle(str))
                    {
                        num6 += 2f * Convert.ToSingle(str);
                        num5 -= 2f * Convert.ToSingle(str);
                    }
                    else if ((double)num6 - (double)num5 < 4.0 * (double)Convert.ToSingle(str))
                    {
                        num6 += Convert.ToSingle(str);
                        num5 -= Convert.ToSingle(str);
                    }
                    else
                    {
                        num6 = (float)((1.8 * (double)this.ye - 0.2 * (double)this.ys) / 1.6);
                        num5 = (float)((1.8 * (double)this.ys - 0.2 * (double)this.ye) / 1.6);
                    }
                }
            }
            if ((double)x2 - (double)x1 == 0.0 || (double)num6 - (double)num5 == 0.0)
                return;
            double num7 = ((double)num2 - (double)num1) / ((double)x2 - (double)x1);
            double num8 = ((double)num3 - (double)num4) / ((double)num6 - (double)num5);
            graphics.DrawLine(new Pen(Color.Black, 1f), num1, num3, num2, num3);
            graphics.DrawLine(new Pen(Color.Black, 1f), num1, num4, num2, num4);
            graphics.DrawLine(new Pen(Color.Black, 1f), num1, num3, num1, num4);
            graphics.DrawLine(new Pen(Color.Black, 1f), num2, num3, num2, num4);
            float x3 = num1;
            float y1 = num3 + 5f;
            graphics.DrawString(x1.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x3, y1));
            SizeF sizeF2 = graphics.MeasureString(x2.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x4 = num2 - sizeF2.Width;
            graphics.DrawString(x2.ToString("f1"), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x4, y1));
            string format = this.abspoint;
            if (!this.autoscale)
            {
                string str1 = this.showpar.yMax.ToString("f10").TrimEnd('0');
                string str2 = this.showpar.yMin.ToString("f10").TrimEnd('0');
                string str3 = str1.Substring(str1.IndexOf('.') + 1);
                string str4 = str2.Substring(str2.IndexOf('.') + 1);
                format = str3.Length < str4.Length ? "f" + (object)str4.Length : "f" + (object)str3.Length;
            }
            SizeF sizeF3 = graphics.MeasureString(num5.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x5 = num1 - sizeF3.Width;
            float y2 = num3 - sizeF3.Height / 2f;
            graphics.DrawString(num5.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x5, y2));
            SizeF sizeF4 = graphics.MeasureString(num6.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x6 = num1 - sizeF4.Width;
            float y3 = num4 - sizeF4.Height / 2f;
            graphics.DrawString(num6.ToString(format), new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x6, y3));
            for (int index = 1; index < 4; ++index)
            {
                Pen pen = new Pen(Color.Black, 1f);
                pen.DashStyle = DashStyle.Dot;
                graphics.DrawLine(pen, num1 + (float)(((double)num2 - (double)num1) * (double)index / 4.0), num3, num1 + (float)(((double)num2 - (double)num1) * (double)index / 4.0), num4);
                graphics.DrawLine(pen, num1, num4 + (float)(((double)num3 - (double)num4) * (double)index / 4.0), num2, num4 + (float)(((double)num3 - (double)num4) * (double)index / 4.0));
            }
            SizeF sizeF5 = graphics.MeasureString(CommonFun.GetLanText("time") + "(s)", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x7 = num1 + (float)(((double)num2 - (double)num1 - (double)sizeF5.Width) / 2.0);
            float y4 = num3 + 5f;
            graphics.DrawString(CommonFun.GetLanText("time") + "(s)", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x7, y4));
            SizeF sizeF6 = graphics.MeasureString("Abs", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)));
            float x8 = num1 - sizeF6.Width;
            float y5 = num4 + (float)(((double)num3 - (double)num4 - (double)sizeF6.Height) / 2.0);
            if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                graphics.DrawString("Abs", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x8, y5));
            else if (this.mpar.C_mode == CommonFun.GetLanText("T"))
                graphics.DrawString("%T", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x8, y5));
            else
                graphics.DrawString("E", new System.Drawing.Font("Segoe UI", (float)(this.FontSize - 2)), (Brush)new SolidBrush(Color.Black), new PointF(x8, y5));
            List<MeaureData> source = ts.Data;
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
            double num9 = (double)num1 + (Convert.ToDouble(source[source.Count<MeaureData>() - 1].xValue) - (double)x1) * num7;
            double num10 = !(this.mpar.C_mode == CommonFun.GetLanText("Abs")) ? (double)num3 - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].YT) - (double)num5) * num8 : (double)num3 - (Convert.ToDouble(source[source.Count<MeaureData>() - 1].yABS) - (double)num5) * num8;
            if (num10 < (double)num4)
                num10 = (double)num4;
            if (num10 > (double)num3)
                num10 = (double)num3;
            for (int index3 = source.Count<MeaureData>() - 2; index3 >= 0; --index3)
            {
                double num11 = (double)num1 + (Convert.ToDouble(source[index3].xValue) - (double)x1) * num7;
                double num12 = !(this.mpar.C_mode == CommonFun.GetLanText("Abs")) ? (Convert.ToDouble(source[index3].YT) >= (double)num5 ? (Convert.ToDouble(source[index3].YT) <= (double)num6 ? (double)num3 - (Convert.ToDouble(source[index3].YT) - (double)num5) * num8 : (double)num4) : (double)num3) : (Convert.ToDouble(source[index3].yABS) >= (double)num5 ? (Convert.ToDouble(source[index3].yABS) <= (double)num6 ? (double)num3 - (Convert.ToDouble(source[index3].yABS) - (double)num5) * num8 : (double)num4) : (double)num3);
                graphics.DrawLine(new Pen(ts.color, 1f), (float)num9, (float)num10, (float)num11, (float)num12);
                num9 = num11;
                num10 = num12;
            }
            this.picOut.Image = (System.Drawing.Image)bitmap;
        }

        private void XYMaxMin()
        {
            if (this.currlist.Count < 1)
                return;
            List<MeaureData> data = this.currlist[0].Data;
            if (data.Count < 1)
                return;
            this.XMaxMinStr(data, out this.xMax, out this.xMin);
            if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
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
            List<TimeScan> list = this.currlist.Where<TimeScan>((Func<TimeScan, bool>)(s => s.IsShow == 1)).ToList<TimeScan>();
            if (list.Count > 0)
            {
                foreach (TimeScan timeScan in list)
                {
                    float xmax;
                    float xmin;
                    this.XMaxMinStr(timeScan.Data, out xmax, out xmin);
                    float num1;
                    float num2;
                    if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                    {
                        num1 = timeScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.yABS)).Max();
                        num2 = timeScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.yABS)).Min();
                    }
                    else
                    {
                        num1 = timeScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.YT)).Max();
                        num2 = timeScan.Data.Select<MeaureData, float>((Func<MeaureData, float>)(s => s.YT)).Min();
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
            this.xMin = 0.0f;
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

        private void ResetTempReDraw(int type)
        {
            this.xyRangeList.Clear();
            this.ZoomOut = 0;
            this.lblmode.Text = this.mpar.C_mode;
            this.lblWL.Text = Convert.ToDecimal(this.mpar.WL).ToString("f1") + " nm";
            if (type > 0)
            {
                if (this.currlist.Count > 0)
                {
                    this.lblSample.Text = this.currlist[0].C_name;
                    this.lblSample.Visible = true;
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
                this.dataGridView1.Rows.Clear();
               // this.dataGridView1.Rows.Add(this.dgvcnt);
                this.dataGridView1.Columns["ColXGD"].HeaderText = this.mpar.C_mode;
                this.picCurve.Image = (System.Drawing.Image)new Bitmap(this.picCurve.Width, this.picCurve.Height);
                List<TimeScan> currlist = this.currlist;
                this.currlist = new List<TimeScan>();
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
        private Color GetColor()
        {
            Color color1 = Color.Red;
            foreach (Color color2 in CommonFun.Colorlist())
            {
                Color cl = color2;
                if (this.currlist.Where<TimeScan>((Func<TimeScan, bool>)(s => s.color == cl)).ToList<TimeScan>().Count == 0)
                {
                    color1 = cl;
                    break;
                }
            }
            return color1;
        }

        private void TopDraw(MeaureData cumd)
        {
            Bitmap bitmap = new Bitmap(this.picTop.Width, this.picCurve.Height);
            Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            this.curx = (float)Convert.ToDecimal(cumd.xValue);
            double num = (double)this.left + (Convert.ToDouble(cumd.xValue) - (double)this.xs) * this.xInt;
            this.ybx = Convert.ToInt32(num);
            if ((double)this.curx >= 0.0)
                graphics.DrawLine(new Pen(Color.Red, 1f), (float)num - 3f, this.bottom - 3f, (float)num - 3f, this.top - 3f);
            if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                this.lbllocation.Text = cumd.xValue + "s," + cumd.yABS.ToString(this.abspoint);
            else
                this.lbllocation.Text = cumd.xValue + "s," + cumd.YT.ToString(this.tpoint);
            this.picTop.Image = (System.Drawing.Image)bitmap;
            this.picTop.Refresh();
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

        private void pibInOut_Click(object sender, EventArgs e)
        {
            if (this.currlist.Count < 1)
            {
                CommonFun.showbox(CommonFun.GetLanText("Nosample"), "Error");
            }
            else
            {
                TimeScan timeScan = this.currlist[0];
                using (CurrTSFrm frm = new CurrTSFrm(this.currlist, this.showpar.MulShow))
                {
                    frm.btnDelete.Click += ((param0, param1) =>
                    {
                        if (new DRMessageBoxFrm(CommonFun.GetLanText("deleteconfirm"), "Warning").ShowDialog() != DialogResult.Yes)
                            return;
                        bool flag = false;
                        for (int index = 0; index < frm.dataGridView1.Rows.Count; ++index)
                        {
                            if (frm.dataGridView1.Rows[index].Tag != null && frm.dataGridView1.Rows[index].Cells["ColOP1"].Tag.ToString() == "on")
                            {
                                this.currlist.Remove((TimeScan)frm.dataGridView1.Rows[index].Tag);
                                flag = true;
                            }
                        }
                        if (flag)
                            CommonFun.InsertLog(CommonFun.GetLanText("timescan"), CommonFun.GetLanText("logdelData"), false);
                        frm.ListBind();
                    });
                    frm.ShowDialog();
                }
                this.XYMaxMin();
                if (timeScan != this.currlist[0])
                    this.ResetTempReDraw(1);
                else if (this.xyRangeList.Count > 0)
                    this.DrawLine();
                else
                    this.AddToRangeList(this.xMin, this.xMax, this.yMin, this.yMax);
                if (this.currlist.Count > 0)
                {
                    this.dataGridView1.Columns["ColXGD"].HeaderText = this.mpar.C_mode;
                    this.dataGridView1.Rows.Clear();
                    for (int index = 0; index < this.currlist[0].Data.Count<MeaureData>(); ++index)
                    {
                        this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Cells["ColNo"].Value = (object)(index + 1);
                        this.dataGridView1.Rows[index].Cells["ColTime"].Value = (object)this.currlist[0].Data[index].xValue;
                        float num;
                        if (this.mpar.C_mode == CommonFun.GetLanText("Abs"))
                        {
                            DataGridViewCell cell = this.dataGridView1.Rows[index].Cells["ColXGD"];
                            num = this.currlist[0].Data[index].yABS;
                            string str = num.ToString(this.abspoint);
                            cell.Value = (object)str;
                        }
                        else
                        {
                            DataGridViewCell cell = this.dataGridView1.Rows[index].Cells["ColXGD"];
                            num = this.currlist[0].Data[index].YT;
                            string str = num.ToString(this.tpoint);
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
                 //   this.dataGridView1.Rows.Add(this.dgvcnt);
                    this.dataGridView1.Columns["ColXGD"].HeaderText = this.mpar.C_mode;
                }
            }
        }
        private void btncancel_Click(object sender, EventArgs e)
        {
            if (this.ComSta == ComStatus.CALBGND)
            {
                CommonFun.WriteSendLine("stop");
                this.btnBlank.Text = CommonFun.GetLanText("blanking");
               // this.panel4.Visible = false;
                this.setState(ComStatus.END);
                this.ComSta = ComStatus.END;
            }
            else if (this.btnScan.Text == CommonFun.GetLanText("stopmeasure"))
            {
                CommonFun.WriteSendLine("stop,");
                this.receive = "";
                this.btnScan.Text = CommonFun.GetLanText("measure");
               // this.panel4.Visible = false;
                this.setState(ComStatus.END);
                this.timersend.Stop();
                this.ComSta = ComStatus.END;
                this.slotno = new List<string>();
                this.sampleno = 0;
            }
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (this.btnPause.Text == CommonFun.GetLanText("pause"))
            {
                this.timersend.Stop();
                this.btnPause.Text = CommonFun.GetLanText("resume");
                this.btnPause.Image = (System.Drawing.Image)Resources.UI_DB_Button_Measure_bule;
            }
            else
            {
                this.timersend.Stop();
                this.btnPause.Text = CommonFun.GetLanText("pause");
                this.btnPause.Image = (System.Drawing.Image)Resources.Button_Pause;
            }
        }

        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".tmscn";
        private void btnSave_Click(object sender, EventArgs e)
        {

            using (SaveFrm save = new SaveFrm(extension, "Временное сканирование"))
            {
                save.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
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
                            CreateXMLDocumentSettingsMulWl();
                            WriteXmlSettingsMulWl();
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
        public void CreateXMLDocumentSettingsMulWl()
        {

            XmlTextWriter xtw = new XmlTextWriter(filepath, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("Data_Izmerenie");
            xtw.WriteEndDocument();
            xtw.Close();

        }
        public void WriteXmlSettingsMulWl()
        {
            XmlDocument xd = new XmlDocument();
            FileStream fs = new FileStream(filepath, FileMode.Open);
            xd.Load(fs);
            XmlNode Settings = xd.CreateElement("Settings");

            XmlNode DateTime1 = xd.CreateElement("DateTime"); // дата создания настройки
            DateTime1.InnerText = DateTime.Now.ToString(); // и значение
            Settings.AppendChild(DateTime1); // и указываем кому принадлежит

            XmlNode C_modeDM = xd.CreateElement("C_modeDM"); // дата создания настройки
            C_modeDM.InnerText = mpar.C_mode;// и значение
            Settings.AppendChild(C_modeDM); // и указываем кому принадлежит 

            XmlNode Interval = xd.CreateElement("Interval"); // дата создания настройки
            Interval.InnerText = mpar.Interval;// и значение
            Settings.AppendChild(Interval); // и указываем кому принадлежит

            XmlNode Length = xd.CreateElement("Length"); // дата создания настройки
            Length.InnerText = mpar.Length;// и значение
            Settings.AppendChild(Length); // и указываем кому принадлежит

            XmlNode Time = xd.CreateElement("Time"); // дата создания настройки
            Time.InnerText = mpar.Time;// и значение
            Settings.AppendChild(Time); // и указываем кому принадлежит

            XmlNode WL = xd.CreateElement("WL"); // дата создания настройки
            WL.InnerText = mpar.WL;// и значение
            Settings.AppendChild(WL); // и указываем кому принадлежит

            XmlNode EConvert = xd.CreateElement("EConvert"); // дата создания настройки
            EConvert.InnerText = mpar.EConvert.ToString();// и значение
            Settings.AppendChild(EConvert); // и указываем кому принадлежит            

            XmlNode AutoXY = xd.CreateElement("AutoXY"); // дата создания настройки
            AutoXY.InnerText = this.mpar.spar.AutoXY.ToString();// и значение          
            Settings.AppendChild(AutoXY); // и указываем кому принадлежит

            XmlNode yMax = xd.CreateElement("yMax"); // дата создания настройки
            yMax.InnerText = this.mpar.spar.xMax.ToString();// и значение
            Settings.AppendChild(yMax); // и указываем кому принадлежит

            XmlNode yMin = xd.CreateElement("yMin"); // дата создания настройки
            yMin.InnerText = this.mpar.spar.xMin.ToString();// и значение
            Settings.AppendChild(yMin); // и указываем кому принадлежит         

            XmlNode D_time = xd.CreateElement("D_time"); // дата создания настройки
            D_time.InnerText = mpar.D_time.ToString();// и значение
            Settings.AppendChild(D_time); // и указываем кому принадлежит

            xd.DocumentElement.AppendChild(Settings);

            string[] HeaderCells = new string[dataGridView1.Columns.Count];
            string[,] Cells1 = new string[dataGridView1.Rows.Count, dataGridView1.Columns.Count];


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

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Сохраненные измерения");

            OpenFrm openFrm = new OpenFrm(Directory.GetCurrentDirectory() + @"\Сохраненные измерения", extension);
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

                    System.Xml.XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(pathTemp + "/" + openFrm.open_name);

                    XDocument xdoc = XDocument.Load(pathTemp + "/" + openFrm.open_name);
                    XmlNodeList nodes = xDoc.ChildNodes;

                    foreach (XmlNode n in nodes)
                    {
                        if ("Data_Izmerenie".Equals(n.Name))
                        {
                            for (XmlNode d = n.FirstChild; d != null; d = d.NextSibling)
                            {
                                if ("Settings".Equals(d.Name))
                                {
                                    //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("C_modeDM".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mpar.C_mode = k.FirstChild.Value;
                                            mpar.C_modeDM = mpar.C_mode;
                                        }
                                        if ("Interval".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mpar.Interval = k.FirstChild.Value;
                                        }
                                        if ("Length".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mpar.Length = k.FirstChild.Value;
                                        }
                                        if ("Time".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mpar.Time = k.FirstChild.Value;
                                        }
                                        if ("WL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mpar.WL = k.FirstChild.Value;
                                        }
                                      
                                        if ("EConvert".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mpar.EConvert = Convert.ToBoolean(k.FirstChild.Value);
                                        }
                                        if ("AutoXY".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.showpar.AutoXY = Convert.ToBoolean(k.FirstChild.Value);
                                            if (this.showpar.AutoXY == true)
                                            {
                                                this.autoscale = true;
                                            }
                                            else
                                            {
                                                this.autoscale = false;
                                                this.showpar.xMax = Convert.ToSingle(mpar.Time);
                                                this.showpar.xMin = 0.0f;
                                            }
                                        }

                                        if ("yMax".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.showpar.yMax = Convert.ToSingle(k.FirstChild.Value);
                                        }

                                        if ("yMin".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.showpar.yMin = Convert.ToSingle(k.FirstChild.Value);
                                        }

                                     
                                        if ("D_time".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.mpar.D_time = new DateTime?(Convert.ToDateTime(k.FirstChild.Value));
                                        }

                                    }
                                    mpar.spar = this.showpar;
                                }
                                // Обрабатываем в цикле только dataGridView1
                                if ("dataGridView1".Equals(d.Name))
                                {

                                    this.dataGridView1.Rows.Clear();
                                    if (this.currlist.Count > 0)
                                    {
                                        if (this.currlist[0].Data == null)
                                            this.currlist[0].Data = new List<MeaureData>();

                                        /*if (this.currlist[0].MethodPar.C_mode == CommonFun.GetLanText("kinetics"))
                                        {
                                            this.dataGridView1.Columns["ColSL"].Visible = false;
                                            this.dataGridView1.Columns["ColPJ"].Visible = false;
                                        }
                                        else
                                        {
                                            this.dataGridView1.Columns["ColSL"].Visible = true;
                                            this.dataGridView1.Columns["ColPJ"].Visible = true;
                                        }*/
                                    }
                                    else
                                    {
                                        this.sslive = new TimeScan();
                                        this.sslive.C_mode = this.mpar.C_mode;
                                        this.sslive.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                                        this.sslive.Serials = CommonFun.GetAppConfig("serialno");
                                        this.sslive.MethodPar = this.mpar;
                                        this.sslive.C_name = this.getName("");
                                        this.sslive.C_Operator = CommonFun.GetAppConfig("currentuser");
                                        this.sslive.color = this.GetColor();
                                        this.sslive.D_Time = this.mpar.D_time.Value;
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
                                            // dataGridView1.Rows.Add(1,0,0,0,0);
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
                                            /*for (int i = 0; i < value_xml.Count(); i++)
                                            {

                                                if (value_xml[i].ToString() != "-")
                                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[i].Value = value_xml[i].ToString();
                                                //    else
                                                //      dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[j].Value = null;



                                            }*/
                                            try
                                            {

                                                MeaureData md = new MeaureData();
                                                md.xValue = value_xml[1].ToString();
                                                md.yABS = (float)Convert.ToDouble(value_xml[2].ToString());
                                                md.Rate = value_xml[3].ToString();
                                                md.PJ = value_xml[4].ToString();
                                                currlist[0].Data.Add(md);
                                                /*this.dataGridView1.Rows.Add();
                                                int count = this.currlist[0].Data.Count;
                                                if (this.dataGridView1.Rows.Count < count)
                                                    return;
                                                this.dataGridView1.Rows[count - 1].Cells["ColNo"].Value = (object)count;
                                                md.xValue = (Convert.ToDecimal(this.mpar.Interval) * (System.Decimal)(count - 1)).ToString("f1");
                                                this.dataGridView1.Rows[count - 1].Cells["ColTime"].Value = (object)md.xValue;
                                                this.dataGridView1.Rows[count - 1].Cells["ColXGD"].Value = (object)md.yABS.ToString(this.abspoint);
                                                this.lbllocation.Text = md.xValue + "s," + md.yABS.ToString(this.abspoint) + "A";
                                                this.dataGridView1.Rows[count - 1].Cells["ColSL"].Value = (object)md.Rate;
                                                this.dataGridView1.Rows[count - 1].Cells["ColPJ"].Value = (object)md.PJ;
                                                this.dataGridView1.Rows[count - 1].Tag = (object)md;
                                                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[count - 1].Cells[0];*/
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            /*md.xValue = value_xml[1].ToString();
                                            md.yABS = Convert.ToSingle(value_xml[2]);
                                            md.YT = Convert.ToSingle(value_xml[2]);*/




                                        }

                                    }


                                    TimeScan timeScan = this.currlist[0];
                                    this.XYMaxMin();
                                    if (timeScan != this.currlist[0])
                                        this.ResetTempReDraw(1);
                                    else if (this.xyRangeList.Count > 0)
                                        this.DrawLine();
                                    else
                                        this.AddToRangeList(this.xMin, this.xMax, this.yMin, this.yMax);
                                    if (this.currlist.Count > 0)
                                    {
                                        
                                        this.dataGridView1.Rows.Clear();
                                        for (int index = 0; index < this.currlist[0].Data.Count<MeaureData>(); ++index)
                                        {
                                            this.dataGridView1.Rows.Add();
                                            this.dataGridView1.Rows[index].Cells["ColNo"].Value = (object)(index + 1);
                                            this.dataGridView1.Rows[index].Cells["ColTime"].Value = (object)this.currlist[0].Data[index].xValue;
                                            this.dataGridView1.Rows[index].Cells["ColXGD"].Value = (object)this.currlist[0].Data[index].yABS.ToString(this.abspoint);
                                            
                                            this.dataGridView1.Rows[index].Tag = (object)this.currlist[0].Data[index];
                                        }
                                        if (this.currlist[0].Data.Count >= this.dgvcnt)
                                            return;
                                     //   this.dataGridView1.Rows.Add(this.dgvcnt - this.currlist[0].Data.Count);
                                    }
                                    else
                                    {
                                    //   this.dataGridView1.Rows.Add(this.dgvcnt);
                                     
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private string getName(string SourceName)
        {
            string str1 = "Kinetics";
            int num = 0;
            if (this.currlist.Where<TimeScan>((Func<TimeScan, bool>)(s => !s.status)).ToList<TimeScan>().Count > 0)
            {
                foreach (string str2 in this.currlist.Select<TimeScan, string>((Func<TimeScan, string>)(s => s.C_name)))
                {
                    if (str2.LastIndexOf("-") >= 0)
                    {
                        try
                        {
                            int int16 = (int)Convert.ToInt16(str2.Substring(str2.LastIndexOf("-") + 1));
                            if (int16 > num)
                                num = int16;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Сохраненные измерения");
                string path = Directory.GetCurrentDirectory() + @"\Сохраненные измерения\TimeScan";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                foreach (FileSystemInfo file in new DirectoryInfo(path).GetFiles())
                {
                    string name = file.Name;
                    if (name.Contains(str1))
                    {
                        string str2 = name.Substring(0, name.LastIndexOf('.'));
                        try
                        {
                            int int16 = (int)Convert.ToInt16(((IEnumerable<string>)str2.Split('-')).Last<string>());
                            if (int16 >= num)
                                num = int16;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return str1 + "-" + (num + 1).ToString();
        }

        private delegate void SetPos(int ipos);

        private delegate void Del_setstate(bool status);

        private delegate void Del_starttt(bool status);

        private delegate void Del_SetWL(string str);

        private delegate void Del_MeaData();

        private delegate void Del_BindData(MeaureData md);

        private delegate void Del_SetStop();

        private delegate void Del_SetBlankLabel();

        private delegate void Del_starttimer();

        private delegate void Del_SamNameSet(string name);

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //    private delegate void Del_AutoPrint();
    }
}
