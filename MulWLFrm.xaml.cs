using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using Path = System.IO.Path;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для MulWLFrm.xaml
    /// </summary>
    public partial class MulWLFrm : Window
    {
        private MulMethod MM;
        private QuaPrintParams printpar = (QuaPrintParams)null;
        private System.Timers.Timer tdwait = new System.Timers.Timer(15000.0);
        private SerialPort sp = new SerialPort();
        private ComStatus ComSta;
        private Thread dealth;
        private bool runptag = true;
        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");
        private string conacc = CommonFun.GetAcc("ceAccuracy");
        private Thread tdstart;
        DispatcherTimer tt = new DispatcherTimer();
        private Queue myque = new Queue();
        private int tickcnt = 0;

        public string[] wl_mass;
        public string count_wl = "3";
        public string count_measure = "3";
        public string optical_path = "10";
        public string photometric_mode;
        DispatcherTimer timer1 = new DispatcherTimer();

        public List<string> rightlist = new List<string>();
        private int calormea = 1;
        private string slotno = "";
        private int currslotno = 0;
        private int meacnt = 0;
        private int mcnt = 0;
        private bool stophappened = false;
        private int endcnt = 0;
        private int currindex = -1;
        private int sourcex;
        private int sourcey;
        private string receive = "";
        private MulData sslive = new MulData();

        private int current_serias = 0;

        public MulWLFrm()
        {
            InitializeComponent();
         
            StartProgram();
        }

        public void StartProgram()
        {
            try
            {
                if (this.MM == null)
                {
                    this.MM = new MulMethod();
                    try
                    {
                        this.MM.C_mode = CommonFun.getXmlValue("MulMethod", "C_mode");
                        this.MM.MeasureMethodName = CommonFun.getXmlValue("MulMethod", "MeaMethod");
                        if (this.MM.MeasureMethodName != "")
                        {
                            this.MM.MeasureMethod = CommonFun.GetByName(this.MM.MeasureMethodName);
                            if (this.MM.MeasureMethod.C_name != null)
                                this.MM.MeasureMethodName = this.MM.MeasureMethod.C_gs;
                            else
                                this.MM.MeasureMethod = (QuaMeaMethod)null;
                        }
                        else
                            this.MM.MeasureMethod = (QuaMeaMethod)null;
                        this.MM.WLCnt = Convert.ToString(CommonFun.getXmlValue("MulMethod", "WLCnt"));
                        this.MM.WL = CommonFun.getXmlValue("MulMethod", "WL");
                        this.MM.EConvert = Convert.ToBoolean(CommonFun.getXmlValue("MulMethod", "EConvert"));
                        this.MM.Length = CommonFun.getXmlValue("MulMethod", "Length");
                        this.MM.MCnt = Convert.ToInt32(CommonFun.getXmlValue("MulMethod", "MCnt"));
                    }
                    catch (Exception ex)
                    {
                        this.MM.C_mode = "Abs";
                        this.MM.MeasureMethodName = "";
                        this.MM.MeasureMethod = (QuaMeaMethod)null;
                        this.MM.WLCnt = 1.ToString();
                        this.MM.WL = "546.0";
                        this.MM.EConvert = false;
                        this.MM.Length = "10";
                        this.MM.MCnt = 1;
                    }
                }
                if (this.printpar == null)
                {
                    this.printpar = new QuaPrintParams();
                    try
                    {
                        this.printpar.ComImage = CommonFun.getXmlValue("MulPrintParams", "ComImage");
                        this.printpar.ShowComImage = Convert.ToBoolean(CommonFun.getXmlValue("MulPrintParams", "ShowComImage"));
                        this.printpar.Addtional = CommonFun.getXmlValue("MulPrintParams", "Addtional");
                        this.printpar.Describtion = CommonFun.getXmlValue("MulPrintParams", "Describtion");
                        this.printpar.ShowAddtional = Convert.ToBoolean(CommonFun.getXmlValue("MulPrintParams", "ShowAddtional"));
                        this.printpar.ShowDes = Convert.ToBoolean(CommonFun.getXmlValue("MulPrintParams", "ShowDes"));
                        this.printpar.ShowInsAndUser = Convert.ToBoolean(CommonFun.getXmlValue("MulPrintParams", "ShowInsAndUser"));
                        this.printpar.Title = CommonFun.getXmlValue("MulPrintParams", "Title");
                    }
                    catch
                    {
                        this.printpar.ComImage = "";
                        this.printpar.ShowComImage = false;
                        this.printpar.Addtional = "";
                        this.printpar.Describtion = "";
                        this.printpar.ShowAddtional = false;
                        this.printpar.ShowDes = false;
                        this.printpar.ShowInsAndUser = true;
                        this.printpar.Title = "Title";
                    }
                }
                if (this.MM.C_mode == ("Abs"))
                {
                    this.lblmode.Content = ("Abs");
                    // this.lblUnit.Text = "Abs";
                }
                else
                {
                    this.lblmode.Content = ("%T");
                    //this.lblUnit.Text = "%T";
                }
                if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                {
                    if (this.rightlist.Contains("rightmulmeasure"))
                        this.btnScan.IsEnabled = true;
                    else
                        this.btnScan.IsEnabled = false;
                    if (this.rightlist.Contains("rightmulblank"))
                        this.btnBlank.IsEnabled = true;
                    else
                        this.btnBlank.IsEnabled = false;
                
                }
                this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
                this.tdstart.Start();
                this.setstate(false);
                if (CommonFun.GetAppConfig("currentconnect") == "-1")
                    this.btnBack.IsEnabled = true;
                this.tt.Interval = TimeSpan.FromMilliseconds(1000);
                this.tt.Tick += new EventHandler(this.tt_Tick);
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }
        private void tt_Tick(object sender, EventArgs e)
        {
            ++this.tickcnt;
            if (this.tickcnt != 60)
                return;
            this.ComSta = ComStatus.END;
            this.tickcnt = 0;
            this.tt.Stop();
            if (!this.btnBlank.Dispatcher.CheckAccess())
                this.btnBlank.Dispatcher.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
            else
                this.setstate(true);
        }
        private void setstate(bool status)
        {
            //this.btnBack.IsEnabled = status;
            this.btnBlank.IsEnabled = status;
            this.btnScan.IsEnabled = status;
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("rightmulmeasure") && status)
                    this.btnScan.IsEnabled = true;
                else
                    this.btnScan.IsEnabled = false;
                if (this.rightlist.Contains("rightmulblank") && status)
                    this.btnBlank.IsEnabled = true;
                else
                    this.btnBlank.IsEnabled = false;
                
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

                sp.BaudRate = 38400;
                sp.PortName = "COM2";
                sp.DataBits = 8;
                sp.StopBits = StopBits.One;
                sp.Parity = Parity.None;
                sp.ReadTimeout = -1;
                sp.RtsEnable = true;
                sp.Handshake = Handshake.None;
                sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);
                sp.Open();
               // sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);
                ComSta = ComStatus.BD_RATIO_FLUSH;
                sp.WriteLine("BD_RATIO_FLUSH \r\n");
                CommonFun.WriteSendLine("BD_RATIO_FLUSH");
                if (!this.btnBlank.Dispatcher.CheckAccess())
                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_start(this.Start), (object)true);
                else
                    this.Start(true);
            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.Message.ToString(), "Error");
                this.ComSta = ComStatus.END;
                if (!this.btnBlank.Dispatcher.CheckAccess())
                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
                else
                    this.setstate(true);
                if (!this.btnBlank.Dispatcher.CheckAccess())
                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_start(this.Start), (object)false);
                else
                    this.Start(false);
            }
        }

        private void DealRecData()
        {
            while (this.runptag)
            {
                if (this.myque.Count > 0)
                {
                    string text = this.myque.Dequeue().ToString();
                    try
                    {
                        switch (this.ComSta)
                        {
                            case ComStatus.SETCHP:
                                try
                                {
                                    if (text.Contains("*A#"))
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (this.calormea == 1)
                                        {
                                            this.SendBlankCmd(0);
                                        }
                                        else
                                        {
                                            this.SendScanCmd(0);
                                            ++this.currslotno;
                                        }
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox(("errorretry") + ex.ToString(), "Error");
                                    this.ComSta = ComStatus.END;
                                    this.currslotno = 0;
                                    this.slotno = "";
                                    this.calormea = 0;
                                    break;
                                }

                            case ComStatus.CALBGND:
                                CommonFun.WriteLine(text);
                                try
                                {
                                    if (text.Contains("END"))
                                    {
                                        if (!this.lblWL.Dispatcher.CheckAccess())
                                            this.lblWL.Dispatcher.Invoke((Delegate)new Del_SetWl(this.SetWL), (object)wl_mass[this.endcnt]);
                                        else
                                            this.SetWL(wl_mass[this.endcnt]);
                                        if (this.stophappened)
                                        {
                                            this.ComSta = ComStatus.END;
                                            if (!this.btnBlank.Dispatcher.CheckAccess())
                                                this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable));
                                            else
                                                this.Setblanklable();
                                            this.endcnt = 0;
                                            this.stophappened = false;
                                        }
                                        else
                                        {
                                            ++this.endcnt;
                                            int vv = this.endcnt * 100 / wl_mass.Count();
                                            if (!this.progressBar1.Dispatcher.CheckAccess())
                                                this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetProgressbar(this.SetProgressbarValue), (object)vv);
                                            else
                                                this.SetProgressbarValue(vv);
                                            if (this.endcnt < wl_mass.Count<string>())
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(wl_mass[this.endcnt]) *10M).ToString("f0") + "\r\n");
                                                CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(wl_mass[this.endcnt]) * 10M).ToString("f0"));
                                                this.ComSta = ComStatus.CALBGND;
                                            }
                                            else
                                            {
                                                this.ComSta = ComStatus.END;
                                                if (!this.btnBlank.Dispatcher.CheckAccess())
                                                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable));
                                                else
                                                    this.Setblanklable();
                                                this.endcnt = 0;
                                                this.stophappened = false;
                                                CommonFun.showbox(("Обнуление завершено"), "Информация");
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox(("Ошибка остановки обнуления") + "," + ex.ToString(), "Ошибка");
                                    CommonFun.WriteSendLine("error,");
                                    this.ComSta = ComStatus.END;
                                    if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable));
                                    else
                                        this.Setblanklable();
                                    this.endcnt = 0;
                                    this.currslotno = 0;
                                    this.slotno = "";
                                    this.calormea = 0;
                                    this.stophappened = false;
                                    break;

                                }

                            case ComStatus.BD_RATIO_FLUSH:
                                if (text.Contains("RCVD"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
                                    else
                                        this.setstate(true);
                                    if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_start(this.Start), (object)false);
                                    else
                                        this.Start(false);
                                    break;
                                }
                                break;

                            case ComStatus.MEASURE:
                                CommonFun.WriteLine(text);
                                this.receive += text;
                                try
                                {
                                    if (text.Contains("END"))
                                    {
                                        if (!this.lblWL.Dispatcher.CheckAccess())
                                            this.lblWL.Dispatcher.Invoke((Delegate)new Del_SetWl(this.SetWL), (object)wl_mass[this.meacnt - 1]);
                                        else
                                            this.SetWL(wl_mass[this.meacnt]);
                                        if (this.stophappened)
                                        {
                                            this.currslotno = 0;
                                            this.slotno = "";
                                            this.mcnt = 0;
                                            this.receive = "";
                                            this.meacnt = 0;
                                            this.ComSta = ComStatus.END;
                                            this.stophappened = false;
                                            if (!this.btnScan.Dispatcher.CheckAccess())
                                                this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable));
                                            else
                                                this.SetMealable();
                                        }
                                        else
                                        {
                                           // ++this.meacnt;
                                            int vv = this.meacnt * 100 / wl_mass.Count();
                                            if (!this.progressBar1.Dispatcher.CheckAccess())
                                                this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetProgressbar(this.SetProgressbarValue), (object)vv);
                                            else
                                                this.SetProgressbarValue(vv);
                                            if (this.meacnt < wl_mass.Count())
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(wl_mass[this.meacnt]) * 10M).ToString("f0") + "\r\n");
                                                CommonFun.WriteSendLine("measure 1 2 " + (Convert.ToDecimal(wl_mass[this.meacnt]) * 10M).ToString("f0"));
                                                this.ComSta = ComStatus.MEASURE;
                                                ++this.meacnt;
                                            }
                                            else
                                            {
                                                this.receive = this.receive.Replace("*DAT", "&");
                                                string[] strArray1 = this.receive.Split('&');
                                                List<QuaMeaData> source = new List<QuaMeaData>();
                                                for (int index = 0; index < wl_mass.Count(); index++)
                                                {
                                                    int num1 = strArray1[index + 1].IndexOf("END");
                                                    if (num1 > 0)
                                                    {
                                                        this.receive = strArray1[index + 1].Substring(1, num1 - 1);
                                                        string[] strArray2 = this.receive.Split(' ');
                                                        QuaMeaData quaMeaData1 = new QuaMeaData();
                                                        if (lblmode.Dispatcher.Invoke(() => this.lblmode.Content.ToString() == ("Abs")))
                                                        {
                                                            quaMeaData1.Value = Convert.ToDouble(strArray2[0]) > 0.01 ? (2.0 - Math.Log10(Convert.ToDouble(strArray2[0]))).ToString(this.absacc) : 4.ToString(this.absacc);
                                                            if (this.MM.Length != "10" && this.MM.EConvert)
                                                                quaMeaData1.Value = (Convert.ToDouble(quaMeaData1.Value) * (Convert.ToDouble(10) / Convert.ToDouble(this.MM.Length))).ToString(this.absacc);
                                                            quaMeaData1.WL = (int)Convert.ToInt16(strArray2[2]);
                                                            quaMeaData1.Slot = (int)Convert.ToInt16(strArray2[1]);
                                                            source.Add(quaMeaData1);
                                                        }
                                                        else
                                                        {
                                                            if (this.MM.Length != "10" && this.MM.EConvert)
                                                            {
                                                                if (Convert.ToDouble(strArray2[0]) <= 0.01)
                                                                {
                                                                    quaMeaData1.Value = (4f * Convert.ToSingle(10) / Convert.ToSingle(this.MM.Length)).ToString(this.absacc);
                                                                    quaMeaData1.Value = Math.Pow(10.0, 2.0 - Convert.ToDouble(quaMeaData1.Value)).ToString(this.tacc);
                                                                }
                                                                else
                                                                {
                                                                    QuaMeaData quaMeaData2 = quaMeaData1;
                                                                    double num2 = 2.0 - Math.Log10(Convert.ToDouble(strArray2[0]));
                                                                    string str1 = num2.ToString(this.tacc);
                                                                    quaMeaData2.Value = str1;
                                                                    QuaMeaData quaMeaData3 = quaMeaData1;
                                                                    num2 = Convert.ToDouble(quaMeaData1.Value) * (Convert.ToDouble(10) / Convert.ToDouble(this.MM.Length));
                                                                    string str2 = num2.ToString(this.tacc);
                                                                    quaMeaData3.Value = str2;
                                                                    QuaMeaData quaMeaData4 = quaMeaData1;
                                                                    num2 = Math.Pow(10.0, 2.0 - Convert.ToDouble(quaMeaData1.Value));
                                                                    string str3 = num2.ToString(this.tacc);
                                                                    quaMeaData4.Value = str3;
                                                                }
                                                            }
                                                            else
                                                                quaMeaData1.Value = strArray2[0];
                                                            quaMeaData1.WL = (int)Convert.ToInt16(strArray2[2]);
                                                            quaMeaData1.Slot = (int)Convert.ToInt16(strArray2[1]);
                                                            source.Add(quaMeaData1);
                                                        }
                                                    }
                                                }
                                                if (this.MM.MeasreList == null)
                                                    this.MM.MeasreList = new List<MulData>();
                                                int esStatus1 = this.MM.ESStatus;
                                                if (this.MM.ESStatus > 0)
                                                {
                                                    this.MM.MeasreList = new List<MulData>();
                                                    this.MM.ESStatus = 0;
                                                    this.MM.C_name = "";
                                                    this.MM.D_time = new DateTime?();
                                                }
                                                else
                                                {
                                                    DateTime? nullable1 = this.MM.D_time;
                                                    if (nullable1.HasValue && this.MM.MeasreList.Count > 0)
                                                    {
                                                        int esStatus2 = this.MM.ESStatus;
                                                        if (this.MM.ESStatus > 0)
                                                        {
                                                            this.MM.MeasreList = new List<MulData>();
                                                            this.MM.ESStatus = 0;
                                                            this.MM.C_name = "";
                                                            MulMethod mm = this.MM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            mm.D_time = nullable2;
                                                        }
                                                        else if (CommonFun.GetAppConfig("currentuser") != this.MM.C_Operator)
                                                        {
                                                            this.MM.MeasreList = new List<MulData>();
                                                            this.MM.ESStatus = 0;
                                                            this.MM.C_name = "";
                                                            MulMethod mm = this.MM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            mm.D_time = nullable2;
                                                        }
                                                        else
                                                        {
                                                            MulMethod mm = this.MM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            mm.D_time = nullable2;
                                                        }
                                                    }
                                                }
                                                this.sslive = new MulData();
                                                if (this.MM.C_head != null && this.MM.C_head.Length > 0)
                                                    this.sslive.C_bz = this.MM.C_head + "-" + (this.MM.MeasreList.Count<MulData>() + 1).ToString();
                                                this.sslive.Avalue = new System.Decimal[wl_mass.Count()];
                                                int index1 = 0;
                                                while (true)
                                                {
                                                    if (index1 < wl_mass.Count())
                                                    {
                                                        int wl1 = 0;
                                                        wl1 = (int)Convert.ToInt16(Convert.ToDecimal(wl_mass[index1]) * 10M) ;
                                                        IEnumerable<QuaMeaData> source_ = source.Where(s => s.WL == wl1);
                                                        foreach(QuaMeaData s in source_)
                                                        {
                                                            double num = Convert.ToDouble(s.Value);
                                                            if (num == 0.0)
                                                                num = 1E-05;
                                                            this.sslive.Avalue[index1] = Convert.ToDecimal(num);
                                                        }
                                                        /*if (source.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)) != null)
                                                        {
                                                            double num = Convert.ToDouble(source.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)).ToList<QuaMeaData>()[0].Value);
                                                            if (num == 0.0)
                                                                num = 1E-05;
                                                            this.sslive.Avalue[index1] = Convert.ToDecimal(num);
                                                        }*/
                                                        ++index1;
                                                    }
                                                    else
                                                        break;
                                                }
                                                if (this.MM.MeasureMethod != null && this.MM.MeasureMethod.WLCnt.HasValue)
                                                {
                                                    string str = this.Caculate(this.MM.MeasureMethod.C_jsgs, ((IEnumerable<System.Decimal>)this.sslive.Avalue).ToList<System.Decimal>(), this.MM.MeasureMethod.wl);
                                                    this.sslive.SJJG = !(str == "--") ? Convert.ToSingle(str) : -1f;
                                                }
                                                else
                                                    this.sslive.SJJG = -1f;
                                                this.sslive.SJJG = Convert.ToSingle(this.sslive.SJJG.ToString("f6"));
                                                this.sslive.D_MTime = new DateTime?(DateTime.Now);
                                                this.MM.MeasreList.Add(this.sslive);
                                                this.currindex = this.MM.MeasreList.Count - 1;
                                                this.LoadMeaData(current_serias);
                                                if (this.mcnt < this.MM.MCnt)
                                                {
                                                    this.meacnt = 0;
                                                    this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(wl_mass[this.meacnt]) * 10M).ToString("f0") + "\r\n");
                                                    CommonFun.WriteSendLine("measure 1 2 " + (Convert.ToDecimal(wl_mass[this.meacnt]) * 10M).ToString("f0"));
                                                    this.ComSta = ComStatus.MEASURE;
                                                    ++this.meacnt;
                                                    ++this.mcnt;
                                                }
                                                else
                                                {
                                                    int num;
                                                    if (this.slotno.Length > 0)
                                                        num = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                    else
                                                        num = 1;
                                                    if (num == 0)
                                                    {
                                                        this.meacnt = 0;
                                                        this.mcnt = 0;
                                                        this.receive = "";
                                                        this.ComSta = ComStatus.SETCHP;
                                                        this.sp.WriteLine("SETCHP " + this.slotno.Split(',')[this.currslotno] + "\r\n");
                                                        CommonFun.WriteSendLine("SETCHP " + this.slotno.Split(',')[this.currslotno]);
                                                    }
                                                    else
                                                    {
                                                        this.currslotno = 0;
                                                        this.slotno = "";
                                                        this.mcnt = 0;
                                                        this.receive = "";
                                                        this.meacnt = 0;
                                                        this.ComSta = ComStatus.END;
                                                        if (!this.btnScan.Dispatcher.CheckAccess())
                                                            this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable));
                                                        else
                                                            this.SetMealable();
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
                                    CommonFun.showbox("Ошибка, Измерение завершено, " + ex.ToString(), "Ошибка");
                                    CommonFun.WriteSendLine("error,," + ex.ToString());
                                    this.ComSta = ComStatus.END;
                                    if (!this.btnScan.Dispatcher.CheckAccess())
                                        this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable));
                                    else
                                        this.SetMealable();
                                    this.receive = "";
                                    this.mcnt = 0;
                                    this.meacnt = 0;
                                    this.currslotno = 0;
                                    this.slotno = "";
                                    this.calormea = 0;
                                    break;

                                }
                                
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonFun.showbox(("errorretry") + ex.ToString(), "Error");

                    }
                }
            }
        }
        private string Caculate(string formu, List<System.Decimal> abs, List<int> wlist)
        {
            List<object> objectList = new List<object>();
            foreach (System.Decimal ab in abs)
            {
                if (ab < 0M)
                    objectList.Add((object)("(" + (object)ab + ")"));
                else
                    objectList.Add((object)ab);
            }
            try
            {
                string Expression = "";
                switch (wlist.Count<int>())
                {
                    case 1:
                        Expression = string.Format(formu, objectList[wlist[0] - 1]);
                        break;
                    case 2:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1]);
                        break;
                    case 3:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1]);
                        break;
                    case 4:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1]);
                        break;
                    case 5:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1]);
                        break;
                    case 6:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1]);
                        break;
                    case 7:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1]);
                        break;
                    case 8:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1]);
                        break;
                    case 9:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1]);
                        break;
                    case 10:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1]);
                        break;
                    case 11:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1]);
                        break;
                    case 12:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1]);
                        break;
                    case 13:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1]);
                        break;
                    case 14:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1], objectList[wlist[13] - 1]);
                        break;
                    case 15:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1], objectList[wlist[13] - 1], objectList[wlist[14] - 1]);
                        break;
                    case 16:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1], objectList[wlist[13] - 1], objectList[wlist[14] - 1], objectList[wlist[15] - 1]);
                        break;
                    case 17:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1], objectList[wlist[13] - 1], objectList[wlist[14] - 1], objectList[wlist[15] - 1], objectList[wlist[16] - 1]);
                        break;
                    case 18:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1], objectList[wlist[13] - 1], objectList[wlist[14] - 1], objectList[wlist[15] - 1], objectList[wlist[16] - 1], objectList[wlist[17] - 1]);
                        break;
                    case 19:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1], objectList[wlist[13] - 1], objectList[wlist[14] - 1], objectList[wlist[15] - 1], objectList[wlist[16] - 1], objectList[wlist[17] - 1], objectList[wlist[18] - 1]);
                        break;
                    case 20:
                        Expression = string.Format(formu, objectList[wlist[0] - 1], objectList[wlist[1] - 1], objectList[wlist[2] - 1], objectList[wlist[3] - 1], objectList[wlist[4] - 1], objectList[wlist[5] - 1], objectList[wlist[6] - 1], objectList[wlist[7] - 1], objectList[wlist[8] - 1], objectList[wlist[9] - 1], objectList[wlist[10] - 1], objectList[wlist[11] - 1], objectList[wlist[12] - 1], objectList[wlist[13] - 1], objectList[wlist[14] - 1], objectList[wlist[15] - 1], objectList[wlist[16] - 1], objectList[wlist[17] - 1], objectList[wlist[18] - 1], objectList[wlist[19] - 1]);
                        break;
                }
                /*MSScriptControl.ScriptControl scriptControl = (MSScriptControl.ScriptControl)new MSScriptControl.ScriptControlClass();
                scriptControl.Language = "JavaScript";
                return scriptControl.Eval(Expression).ToString();
                */
                return Expression.ToString();
            }
            catch
            {
                return "--";
            }
        }
        private void SendBlankCmd(int iteration_blank)
        {
            string[] strArray = wl_mass;
            try
            {
                /*if (Convert.ToDecimal(strArray[iteration_blank]) < 190M || Convert.ToDecimal(strArray[iteration_blank]) > 1100M)
                    {
                        CommonFun.showbox(("errordata"), "Error");
                        CommonFun.showbox("errordata");
                        if (!this.btnBlank.Dispatcher.CheckAccess())
                        {
                            this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable));
                            return;
                        }
                        this.Setblanklable();
                        return;
                    }*/
                
                this.ComSta = ComStatus.CALBGND;
                this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(strArray[iteration_blank]) * 10M).ToString("f0") + "\r\n");
                    CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(strArray[iteration_blank]) * 10M).ToString("f0"));
                    if (!this.btnBack.Dispatcher.CheckAccess())
                        this.btnBack.Dispatcher.Invoke((Delegate)new Del_SetState(this.SetState));
                    else
                        this.SetState();
                
            }
            catch (Exception ex)
            {
                CommonFun.showbox(("errorstopblank") + "," + ex.ToString(), "Error");
              //  CommonFun.showbox("errorstopblank, " + ex.ToString());
                this.ComSta = ComStatus.END;
                if (!this.btnBlank.Dispatcher.CheckAccess())
                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable));
                else
                    this.Setblanklable();
            }
        }
        private void SendScanCmd(int iteration_measure)
        {
            string[] strArray = wl_mass;
          /*if (Convert.ToDecimal(strArray[iteration_measure]) < 190M || Convert.ToDecimal(strArray[iteration_measure]) > 1100M)
                {
                    CommonFun.showbox(("errordata"), "Error");
                    CommonFun.showbox("errordata");
                    if (!this.btnScan.Dispatcher.CheckAccess())
                    {
                        this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable));
                        return;
                    }
                    this.SetMealable();
                    return;
                }
            */
            try
            {
                /*this.ComSta = ComStatus.MEASURE;
                //SerialPort sp = this.sp;
                System.Decimal num = Convert.ToDecimal(strArray[iteration_measure]);
                    string text = "measure 1 2 " + num.ToString("f0") + "\r\n";
                    sp.WriteLine(text);
                 //  num = Convert.ToDecimal(strArray[iteration_measure]);
                    CommonFun.WriteSendLine("measure 1 2 " + num.ToString("f0"));
                    ++this.meacnt;
                    ++this.mcnt;
                    if (!this.btnBack.Dispatcher.CheckAccess())
                        this.btnBack.Dispatcher.Invoke((Delegate)new Del_SetState(this.SetState));
                    else
                        this.SetState();*/
                this.ComSta = ComStatus.MEASURE;
                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(strArray[iteration_measure]) * 10M).ToString("f0") + "\r\n");
                //CommonFun.WriteSendLine("measure 1 2 " + (Convert.ToDecimal(strArray[iteration_measure])).ToString("f0"));
                ++this.meacnt;
                ++this.mcnt;
                if (!this.btnBack.Dispatcher.CheckAccess())
                    this.btnBack.Dispatcher.Invoke((Delegate)new Del_SetState(this.SetState));
                else
                    this.SetState();

            }
            catch (Exception ex)
            {
                //CommonFun.showbox(("errorstopmeasure") + "," + ex.ToString(), "Error");
                CommonFun.showbox("errorstopmeasure, " + ex.ToString(), "Error");
                this.meacnt = 0;
                this.mcnt = 0;
                this.ComSta = ComStatus.END;
                if (!this.btnScan.Dispatcher.CheckAccess())
                    this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable));
                else
                    this.SetMealable();
            }
        }
        private void Setblanklable()
        {
            this.btnBlank.IsEnabled = true;
            this.btnBlank.Content = ("Обнуление");
            // this.panel4.Visible = false;
            this.progressBar1.Value = 0;
            this.setState();
        }
        private void SetState()
        {
            this.setState();
        }
        private void SetMealable()
        {
            this.btnScan.IsEnabled = true;
            this.btnScan.Content = ("Измерение");
            // this.panel4.Visible = false;
            this.progressBar1.Value = 0;
            this.setState();
        }
        private void setState()
        {
            switch (this.ComSta)
            {
                case ComStatus.MEASURE:
                    this.btnScan.IsEnabled = true;
                    //  this.btnSet.IsEnabled = true;
                    this.btnBack.IsEnabled = false;
                    this.btnBlank.IsEnabled = false;
                    //  this.btnOperate.Enabled = false;
                    break;
                case ComStatus.CALBGND:
                    this.btnBlank.IsEnabled = true;
                    //  this.btnSet.Enabled = true;
                    this.btnBack.IsEnabled = false;
                    this.btnScan.IsEnabled = false;
                    //this.btnOperate.Enabled = false;
                    break;
                case ComStatus.END:
                    this.btnScan.IsEnabled = true;
                    //   this.btnSet.Enabled = true;
                    this.btnBack.IsEnabled = true;
                    this.btnBlank.IsEnabled = true;
                    // this.btnOperate.Enabled = true;
                    break;
            }
        }

        public void Start(bool status)
        {
            if (status)
            {
                this.tt.IsEnabled = true;
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
            try
            {
                if (this.sp.IsOpen)
                {
                    switch (this.ComSta)
                    {
                        case ComStatus.Connect:
                            this.sp.ReadLine();
                            this.ComSta = ComStatus.END;
                            break;
                        case ComStatus.SYSTATE:
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
                    CommonFun.showbox(("opencom"), "Warning");
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }
        private void Open_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Open_File();
        }
        bool shifrTrueFalse = false;
        public void Open_File()
        {
           // this.timer1.Stop();
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Сохраненные измерения");

            OpenFrm openFrm = new OpenFrm(Directory.GetCurrentDirectory() + @"\Сохраненные измерения", ".mvlr");
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
                    MyTableList.Clear();
                    meisureGrid.Items.Refresh();
                    meisureGrid.Columns.Clear();
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

                    // Читаем в цикле вложенные значения Stroka
                    foreach (XmlNode n in nodes)
                    {
                        if ("Data_Izmerenie".Equals(n.Name))
                        {
                            for (XmlNode d = n.FirstChild; d != null; d = d.NextSibling)
                            {
                                if ("Settings".Equals(d.Name))
                                {
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if("Count_Wl".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            count_wl = k.FirstChild.Value;
                                        }
                                        if ("CountMeasure".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            count_measure = k.FirstChild.Value;
                                        }

                                        if ("OpticalPath".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            optical_path = k.FirstChild.Value;
                                        }

                                        if ("PhotometricMode".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            photometric_mode = k.FirstChild.Value;
                                        }

                                       
                                    }
                                }
                                if ("MulMethod".Equals(d.Name))
                                {
                                    MulMethod mulMethod = new MulMethod();
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("CMode".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mulMethod.C_mode = k.FirstChild.Value;
                                        }
                                        if ("WLCnt_".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mulMethod.WLCnt = k.FirstChild.Value;
                                        }
                                        if ("MCnt_".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mulMethod.MCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("Length_".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mulMethod.Length = k.FirstChild.Value;
                                        }
                                        if ("CName_".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mulMethod.C_name = k.FirstChild.Value;
                                        }
                                        if ("CMethodCreate_".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mulMethod.C_methodcreator = k.FirstChild.Value;
                                        }
                                        if("WLMass_".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            mulMethod.WL = k.FirstChild.Value;
                                         ///   wl_mass = new string[Convert.ToInt32(mulMethod.WLCnt)];
                                            wl_mass = mulMethod.WL.Split(',');
                                        }
                        }
                                }
                                // Обрабатываем в цикле только Stroka
                                if ("Stroka".Equals(d.Name))
                                {
                                    int stolbec = 0;
                                    //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                    List<object> value_xml = new List<object>();
                                    List<object> attribute_xml = new List<object>();
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("Stolbec".Equals(k.Name) && k.FirstChild != null)
                                        {

                                             value_xml.Add(k.FirstChild.Value);
                                             XmlAttributeCollection attrColl = k.Attributes;
                                            attribute_xml.Add(attrColl[0].Value);
                                         //   attribute_xml.Add(k.Attributes("Headers").ToString());
                                            //array = new string[1][];
                                            //   array[0] = new string[wl_mass.Count()];
                                        }
                                    }

                                    array = new string[1][];
                                    array[0] = new string[value_xml.Count()];
                                    for(int i = 0; i < value_xml.Count(); i++)
                                    {
                                        array[0][i] = value_xml[i].ToString();
                                    }

                                    if (meisureGrid.Columns.Count == 0)
                                    {
                                        for (int i = 0; i < attribute_xml.Count(); i++)
                                        {
                                            DataGridTextColumn textColumn = new DataGridTextColumn();
                                            textColumn.Header = Convert.ToDecimal(Convert.ToDecimal(attribute_xml[i].ToString()), new CultureInfo("en-US")).ToString("f1");
                                            textColumn.Binding = new Binding(string.Format("WlArray[0][{0}]", i));
                                            // textColumn.Binding = new Binding("{Binding list}");
                                            meisureGrid.Columns.Add(textColumn);


                                        }
                                    }


                                    MyTableList.Add(new MulWlTable(array));
                                    meisureGrid.ItemsSource = _itemList;

                                }
                            }
                        }
                    }

                    File.Delete(pathTemp + "/" + openFrm.open_name);
                }
            }
           // this.timer1.Start();
        }

        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".mvlr";
        private void Save_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.timer1.Stop();
            if (MyTableList.Count == 0)
            {
                CommonFun.showbox("Измерений не было, сохранять нечего", "Info");
            }
            else
            {
                using (SaveFrm save = new SaveFrm(extension, "Мультиволновой режим"))
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
            this.timer1.Start();
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

            XmlNode Count_Wl = xd.CreateElement("Count_Wl");
            Count_Wl.InnerText = count_wl.ToString(); // и значение
            Settings.AppendChild(Count_Wl); // и указываем кому принадлежит

            XmlNode WLMass = xd.CreateElement("WLMass");
            WLMass.InnerText = string.Join(",", wl_mass); // и значение
            Settings.AppendChild(WLMass); // и указываем кому принадлежит

            XmlNode CountMeasure = xd.CreateElement("CountMeasure");
            CountMeasure.InnerText = count_measure.ToString(); // и значение
            Settings.AppendChild(CountMeasure); // и указываем кому принадлежит

            XmlNode OpticalPath = xd.CreateElement("OpticalPath");
            OpticalPath.InnerText = optical_path.ToString(); // и значение
            Settings.AppendChild(OpticalPath); // и указываем кому принадлежит

            XmlNode PhotometricMode = xd.CreateElement("PhotometricMode");
            PhotometricMode.InnerText = photometric_mode.ToString(); // и значение
            Settings.AppendChild(PhotometricMode); // и указываем кому принадлежит

            xd.DocumentElement.AppendChild(Settings);

            XmlNode MulMethod_ = xd.CreateElement("MulMethod");

            XmlNode CMode = xd.CreateElement("CMode");
            CMode.InnerText = this.MM.C_mode.ToString(); // и значение
            MulMethod_.AppendChild(CMode); // и указываем кому принадлежит

            XmlNode WLCnt_ = xd.CreateElement("WLCnt_");
            WLCnt_.InnerText = this.MM.WLCnt.ToString(); // и значение
            MulMethod_.AppendChild(WLCnt_); // и указываем кому принадлежит

            XmlNode MCnt_ = xd.CreateElement("MCnt_");
            MCnt_.InnerText = this.MM.MCnt.ToString(); // и значение
            MulMethod_.AppendChild(MCnt_); // и указываем кому принадлежит

            XmlNode Length_ = xd.CreateElement("Length_");
            Length_.InnerText = this.MM.Length.ToString(); // и значение
            MulMethod_.AppendChild(Length_); // и указываем кому принадлежит

            XmlNode CName_ = xd.CreateElement("CName_");
            CName_.InnerText = this.MM.C_name.ToString(); // и значение
            MulMethod_.AppendChild(CName_); // и указываем кому принадлежит

            XmlNode CMethodCreate_ = xd.CreateElement("CMethodCreate_");
            CMethodCreate_.InnerText = this.MM.C_methodcreator.ToString(); // и значение
            MulMethod_.AppendChild(CMethodCreate_); // и указываем кому принадлежит

            XmlNode WLMass_ = xd.CreateElement("WLMass_");
            WLMass_.InnerText = this.MM.WL.ToString(); // и значение
            MulMethod_.AppendChild(WLMass_); // и указываем кому принадлежит

            xd.DocumentElement.AppendChild(MulMethod_);

            XmlNode Izmerenie = xd.CreateElement("Izmerenie");

            XmlNode DateTime1 = xd.CreateElement("DateTime"); // дата создания градуировки
            DateTime1.InnerText = DateTime.Now.ToString(); // и значение
            Izmerenie.AppendChild(DateTime1); // и указываем кому принадлежит           

            int countIzmer = meisureGrid.Items.Count - 1;
            XmlNode countIzmer1 = xd.CreateElement("countIzmer1");
            countIzmer1.InnerText = Convert.ToString(countIzmer);
            Izmerenie.AppendChild(countIzmer1);

            xd.DocumentElement.AppendChild(Izmerenie);

            string[] HeaderCells = new string[meisureGrid.Columns.Count];
            string[,] Cells1 = new string[meisureGrid.Items.Count, meisureGrid.Columns.Count];


            for (int i = 0; i < meisureGrid.Items.Count; i++)
            {
                XmlNode Cells2 = xd.CreateElement("Stroka");

                XmlAttribute attribute1 = xd.CreateAttribute("Nomer");
                attribute1.Value = Convert.ToString(i); // устанавливаем значение атрибута
                Cells2.Attributes.Append(attribute1); // добавляем атрибут

                for (int j = 0; j < meisureGrid.Columns.Count; j++)
                {

                    try
                    {
                        TextBlock x = meisureGrid.Columns[j].GetCellContent(meisureGrid.Items[i]) as TextBlock;
                        // Cells1[i, j] = x.Text;
                        if (x != null)
                            Cells1[i, j] = x.Text;
                        else
                             if (MyTableList.ElementAt(i) != null)
                            Cells1[i, j] = MyTableList.ElementAt(i).WlArray[0][j];
                        else
                            Cells1[i, j] = "";
                    }
                    catch
                    {
                        Cells1[i, j] = MyTableList.ElementAt(i).WlArray[0][j];
                    }

                    HeaderCells[j] = meisureGrid.Columns[j].Header.ToString();

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


                xd.DocumentElement.AppendChild(Cells2);
            }
            fs.Close();         // Закрываем поток  
            xd.Save(filepath); // Сохраняем файл  

        }

        private void Method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            // SettingsMulWlFrm settingsMulWlFrm = new SettingsMulWlFrm(wl_mass, photometric_mode, count_wl, count_measure, optical_path);
            using(SettingsMulWlFrm settingsMulWlFrm = new SettingsMulWlFrm())
            {
                settingsMulWlFrm.CloseSettings.PreviewMouseDown += ((param0, param1) =>
                {
                    settingsMulWlFrm.Close();
                    settingsMulWlFrm.Dispose();
                });
                settingsMulWlFrm.Finish.PreviewMouseDown += ((param0, param1) =>
                {
                    bool error = settingsMulWlFrm.ValueWL();
                    if (error == true)
                    {
                        meisureGrid.Dispatcher.Invoke(() => {
                            meisureGrid.Columns.Clear();
                            MyTableList.Clear();
                            meisureGrid.Items.Refresh();
                            meisureGrid.Columns.Clear();
                        });
                        count_wl = settingsMulWlFrm.count_wl;
                        wl_mass = settingsMulWlFrm.wl_mass;
                        count_measure = settingsMulWlFrm.count_measure;
                        optical_path = settingsMulWlFrm.optical_path;
                        photometric_mode = settingsMulWlFrm.photometric_mode;
                        if (photometric_mode == "Абсорбция (Abs)")
                        {
                            lblmode.Content = "Abs";
                        }
                        else
                        {
                            lblmode.Content = "%T";
                        }



                        for (int i = 0; i < wl_mass.Count(); i++)
                        {
                            DataGridTextColumn textColumn = new DataGridTextColumn();
                            textColumn.Header = Convert.ToDecimal(Convert.ToDecimal(wl_mass[i]), new CultureInfo("en-US")).ToString("f1");
                            textColumn.Binding = new Binding(string.Format("WlArray[0][{0}]", i));
                            // textColumn.Binding = new Binding("{Binding list}");
                            meisureGrid.Columns.Add(textColumn);


                        }

                        if (this.MM.C_methodcreator == null || this.MM.C_methodcreator.Length <= 0)
                        {
                            MulMethod mulMethod = new MulMethod();
                            mulMethod.C_mode = lblmode.Content.ToString();
                            mulMethod.WLCnt = Convert.ToInt32(settingsMulWlFrm.count_wl).ToString();
                            mulMethod.MCnt = Convert.ToInt32(settingsMulWlFrm.count_measure);
                            mulMethod.Length = Convert.ToInt32(settingsMulWlFrm.optical_path).ToString();
                            mulMethod.C_mode = settingsMulWlFrm.photometric_mode;
                            mulMethod.C_name = settingsMulWlFrm.photometric_mode;

                            try
                            {
                                for (int i = 0; i < wl_mass.Count(); i++)
                                {
                                    if (i != 0)
                                    {
                                        mulMethod.WL += Convert.ToDecimal(wl_mass[i], new CultureInfo("en-US")).ToString();
                                    }
                                    else
                                    {
                                        mulMethod.WL = Convert.ToDecimal(wl_mass[i], new CultureInfo("en-US")).ToString();
                                    }
                                    if (i != wl_mass.Count() - 1)
                                    {
                                        mulMethod.WL += ",";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                CommonFun.showbox(ex.ToString(), "Error");
                                return;
                            }

                            /*if (frm.lblMMV.Tag != null)
                            {
                                mulMethod.MeasureMethod = (QuaMeaMethod)frm.lblMMV.Tag;
                                mulMethod.MeasureMethodName = mulMethod.MeasureMethod.C_gs;
                            }*/

                            mulMethod.C_methodcreator = CommonFun.GetAppConfig("currentuser");

                            this.MM = mulMethod;

                        }
                        for (int i = 0; i < wl_mass.Count(); i++)
                        {
                            wl_mass[i] = Convert.ToDecimal(Convert.ToDecimal(wl_mass[i]), new CultureInfo("en-US")).ToString();
                        }
                        settingsMulWlFrm.Close(); settingsMulWlFrm.Dispose();
                    }
                });
                settingsMulWlFrm.ShowDialog();
            }

        }

        private void LoadMeaData(int index)
        {
            try
            {
                if (index < 0)
                    return;
                if (this.MM.MeasreList == null || this.MM.MeasreList.Count == 0)
                {
                    this.lblNo.Content = "0001";
                }
                else
                {
                    lblNo.Dispatcher.Invoke(() =>
                    {
                        //if (!this.lblNo.Dispatcher.CheckAccess())
                            this.lblNo.Content = (index+1).ToString("D4") + "/" + this.MM.MCnt.ToString("D4");
                    });
                    if (index > this.MM.MCnt)
                        index = this.MM.MeasreList.Count<MulData>();
                    this.MM.WL.Split(',');
                    string str = lblmode.Dispatcher.Invoke(() => !(lblmode.Content.ToString() == ("Abs")) ? "T" : "A");
                   // if (lblmode.Dispatcher.Invoke(() => this.lblmode.Content.ToString() == ("Abs")))
                     //   array[0][index] = this.MM.MeasreList[index].Avalue[index].ToString(this.absacc);
                   // else
                  //      array[0][index] = this.MM.MeasreList[index].Avalue[index].ToString(this.tacc);

                    //if (index == meisureGrid.Columns.Count - 1)
                    if((index + 1) % this.MM.MCnt == 0)
                    {
                       // array[0][] = new string[]
                        for (int i = 0; i < this.meisureGrid.Columns.Count; i++)
                        {
                            if (lblmode.Dispatcher.Invoke(() => this.lblmode.Content.ToString() == ("Abs")))
                                array[0][i] = this.MM.MeasreList[index].Avalue[i].ToString(this.absacc);
                            else
                                array[0][i] = this.MM.MeasreList[index].Avalue[i].ToString(this.tacc);
                        }
                        Application.Current.Dispatcher.Invoke(() => MyTableList.Add(new MulWlTable(array)));
                        meisureGrid.Dispatcher.Invoke(() =>
                        {
                            meisureGrid.ItemsSource = _itemList;
                        });
                    }
                    else
                    {
                        current_serias++;
                    }
                }
            }
            catch(Exception ex)
            {
                CommonFun.showbox("Print measure, " + ex.ToString(), "Error");
                CommonFun.WriteLine(ex.ToString());
            }
        }
        private void BtnBlank_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            /*if (this.MM.C_methodcreator == null || this.MM.C_methodcreator.Length <= 0)
            {
                CommonFun.showbox(("nomethod"), "Error");
            }
            else
            {*/
                if (!this.sp.IsOpen)
                    CommonFun.showbox(("opencom"), "Warning");
                else if (this.ComSta != ComStatus.END)
                {
                    this.btnBlank.IsEnabled = false;
                    this.stophappened = true;
                    CommonFun.WriteSendLine("stop,");
                    this.meacnt = 0;
                }
                else
                {
                    if (CommonFun.GetAppConfig("EightSlot") == "true")
                    {
                        this.calormea = 1;
                    }
                    else
                        this.SendBlankCmd(0);
                    if (this.ComSta != ComStatus.END)
                    {
                        this.endcnt = 0;
                        this.btnBlank.Content = ("Остановить");
                        CommonFun.InsertLog("MultiWavelength", "Обнуление", false);
                    }
                }
            //}
        }
        private void Home_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
         //   if (CommonFun.GetAppConfig("GLPEnabled") == "true")
           // {
             //   if (new DRMessageBoxFrm((this.lanvalue, "exitconfirm"), "Warning").ShowDialog() != DialogResult.Yes)
              //      return;
              /*  if (!this.MM.D_time.HasValue)
                {
                    if (this.MM.MeasreList != null && this.MM.MeasreList.Count != 0)
                    {
                        CommonFun.showbox(("datasaveexit"), "Warning");
                        return;
                    }
                }*/
         //   }
           // else if (!this.MM.D_time.HasValue && (this.MM.MeasreList != null && this.MM.MeasreList.Count != 0 && new DRMessageBoxFrm(("unsavedataexit"), "Warning").ShowDialog() == DialogResult.No))
          //      return;
            try
            {
                if (this.MM.C_mode == ( "Abs"))
                    CommonFun.setXmlValue("MulMethod", "C_mode", "Abs");
                else
                    CommonFun.setXmlValue("MulMethod", "C_mode", "T");
                CommonFun.setXmlValue("MulMethod", "WL", this.MM.WL);
                CommonFun.setXmlValue("MulMethod", "WLCnt", this.MM.WLCnt.ToString());
                if (this.MM.MeasureMethod != null)
                    CommonFun.setXmlValue("MulMethod", "MeaMethod", this.MM.MeasureMethod.C_name);
                else
                    CommonFun.setXmlValue("MulMethod", "MeaMethod", "");
                CommonFun.setXmlValue("MulMethod", "EConvert", this.MM.EConvert.ToString());
                CommonFun.setXmlValue("MulMethod", "Length", this.MM.Length.ToString());
                CommonFun.setXmlValue("MulMethod", "MCnt", this.MM.MCnt.ToString());
               /* CommonFun.setXmlValue("MulPrintParams", "Addtional", this.printpar.Addtional);
                CommonFun.setXmlValue("MulPrintParams", "ComImage", this.printpar.ComImage);
                CommonFun.setXmlValue("MulPrintParams", "Describtion", this.printpar.Describtion);
                bool flag = this.printpar.ShowAddtional;
                CommonFun.setXmlValue("MulPrintParams", "ShowAddtional", flag.ToString());
                flag = this.printpar.ShowComImage;
                CommonFun.setXmlValue("MulPrintParams", "ShowComImage", flag.ToString());
                flag = this.printpar.ShowDes;
                CommonFun.setXmlValue("MulPrintParams", "ShowDes", flag.ToString());
                flag = this.printpar.ShowInsAndUser;
                CommonFun.setXmlValue("MulPrintParams", "ShowInsAndUser", flag.ToString());
                CommonFun.setXmlValue("MulPrintParams", "Title", this.printpar.Title);*/
            }
            catch
            {
                CommonFun.setXmlValue("MulMethod", "C_mode", "Abs");
                CommonFun.setXmlValue("MulMethod", "WL", "546.0");
                CommonFun.setXmlValue("MulMethod", "WLCnt", "1");
                CommonFun.setXmlValue("MulMethod", "MeaMethod", "");
                CommonFun.setXmlValue("MulMethod", "EConvert", "False");
                CommonFun.setXmlValue("MulMethod", "Length", "10");
                CommonFun.setXmlValue("MulMethod", "Threshold", "");
                CommonFun.setXmlValue("MulMethod", "MCnt", "1");
                /*CommonFun.setXmlValue("MulPrintParams", "Addtional", "");
                CommonFun.setXmlValue("MulPrintParams", "ComImage", "");
                CommonFun.setXmlValue("MulPrintParams", "Describtion", "");
                CommonFun.setXmlValue("MulPrintParams", "ShowAddtional", "False");
                CommonFun.setXmlValue("MulPrintParams", "ShowComImage", "False");
                CommonFun.setXmlValue("MulPrintParams", "ShowDes", "False");
                CommonFun.setXmlValue("MulPrintParams", "ShowInsAndUser", "False");
                CommonFun.setXmlValue("MulPrintParams", "Title", "");*/
            }
            if (this.sp.IsOpen)
            {
                CommonFun.WriteSendLine("back,");
                this.sp.Close();
            }
            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();
            //this.Close();
            //     CommonFun.WriteLine("Скрываем окно");
            Hide();
            //      CommonFun.WriteLine("Получаем меню");
            MenuProgram menuProgram = new MenuProgram();
            //     CommonFun.WriteLine("Выводим меню");
            menuProgram.Show();
            //    CommonFun.WriteLine("Получаем родительское окно");
            Window parentWindow = Window.GetWindow(this);
            //     CommonFun.WriteLine("Закрываем родительское окно");
            parentWindow.Close();
        }
        private readonly ObservableCollection<MulWlTable> _itemList = new ObservableCollection<MulWlTable>();
        public ObservableCollection<MulWlTable> MyTableList { get { return _itemList; } }
        string[][] array;
        private void Meisure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            /*if (this.MM.C_methodcreator == null || this.MM.C_methodcreator.Length <= 0)
            {
                CommonFun.showbox(("nomethod"), "Error");
            }
            else
            {*/
            string errormsg = "";
            int esStatus = this.MM.ESStatus;
            //if (this.MM.ESStatus > 0 && new DRMessageBoxFrm(("esdatawarning"), "Warning").ShowDialog() == DialogResult.No)
            //    return;
            if (!this.sp.IsOpen)
                CommonFun.showbox(("opencom"), "Warning");
            else if (this.btnScan.Content.ToString() == ("Остановить"))
            {
                this.btnScan.IsEnabled = false;
                this.stophappened = true;
                CommonFun.WriteSendLine("stop,");
            }
            else
            {
                this.MM.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                this.MM.Serials = CommonFun.GetAppConfig("serialno");
                CommonFun.InsertLog("MultiWavelength", "Measure standard sample", false);
                this.btnScan.Content = ("Остановить");
                current_serias = 0;
                lblNo.Dispatcher.Invoke(() =>
                {
                    //if (!this.lblNo.Dispatcher.CheckAccess())
                    this.lblNo.Content = (current_serias).ToString("D4") + "/" + this.MM.MCnt.ToString("D4");
                });
                if (CommonFun.GetAppConfig("EightSlot") == "true")
                {
                    this.calormea = 2;
                }
                else
                {
                    array = new string[1][];
                    array[0] = new string[wl_mass.Count()];
                    this.SendScanCmd(0);
                    
                }
            }

            //}
        }
        private delegate void Del_setstate(bool status);

        private delegate void Del_start(bool status);

        private delegate void Del_BindData(int index);

        private delegate void Del_SetBlankLable();

        private delegate void Del_SetMeasureLable();

        private delegate void Del_SetWl(string wl);

        private delegate void Del_SetProgressbar(int value);

        private delegate void Del_SetState();
        private void SetProgressbarValue(int vv) {
            progressBar1.Dispatcher.Invoke(() =>
                this.progressBar1.Value = vv);

        }


        private void SetWL(string wl)
        {
            lblWL.Dispatcher.Invoke(() =>
                 this.lblWL.Content = (Convert.ToDecimal(wl)).ToString("f1") + " нм");
        }
    }

    public class MulWlTable
    {
        public MulWlTable() { }
        public MulWlTable(string[][] array)
        {
            this.WlArray = array;
        }

        public string[][] WlArray { get; set; }

    }
}
