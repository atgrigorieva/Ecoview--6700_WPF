using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Web.UI;
using Application = System.Windows.Application;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Path = System.IO.Path;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для Quantion.xaml
    /// </summary>
    public partial class Quantion : Window
    {
        public string[] wl_mass;
        public string step_interval;
        public string count_measure;
        public string optical_path;
        public string curveEquation;
        public string fittingMethod;
        public string count_standard_sample;
        public string measureunit;
        public bool zero_intercept = false;
        public string measure_method;
        public string calibration_method;
        private QuaMethod QM;
        private QuaMethod subQM;
        private QuaPrintParams printpar = (QuaPrintParams)null;
        private SerialPort sp = new SerialPort();
        private ComStatus ComSta;
        private Thread dealth;
        private bool runptag = true;
        private bool SubScan = false;
        private string lanvalue;
        public List<string> rightlist = new List<string>();
        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");
        private string conacc = CommonFun.GetAcc("ceAccuracy");
        private Thread tdstart;
        private System.Windows.Forms.Timer tt = new System.Windows.Forms.Timer();
        private int tickcnt = 0;
        private int calormea = 0;
        private string slotno = "";
        private int currslotno = 0;
        private bool stophappened = false;
        private List<QuaMeaData> bsmvalue;

        private int meacnt = 0;
        private int mcnt = 0;
        private Sample sslive = new Sample();
        private Queue myque = new Queue();
        private string receive = "";
        private List<MeaureData> mdlists = new List<MeaureData>();
        private int endcnt = 0;
        private string scanwlpar = "";
        private int currindex = -1;
        private int sourcex;
        private int sourcey;

        private QuantitationNewMethodNext smfrm;
        public Quantion()
        {
            InitializeComponent();
            smfrm = new QuantitationNewMethodNext(QM);
            if (CommonFun.GetAppConfig("RaceMode") == "true")
            {
                this.QM = new QuaMethod();
                this.QM.Page = 1;
                this.QM.QPar = new QuaParmas();
                this.QM.QPar.MeasureMethodName = "Одноволновое";
                this.QM.QPar.MeasureMethod = CommonFun.GetByName(this.QM.QPar.MeasureMethodName);
                this.QM.QPar.MeasureMethodName = this.QM.QPar.MeasureMethod.C_name;
                this.QM.QPar.CabMethodDM = "Ввод коэффициентов уравнения";
                this.QM.QPar.CabMethod = this.QM.QPar.CabMethodDM;
                this.QM.QPar.EConvert = false;
                this.QM.QPar.Cuvettemath = false;
                this.QM.QPar.Equation = "C=f(Abs)";
                this.QM.QPar.FittingDM = "Линейная";
                this.QM.QPar.Fitting = this.QM.QPar.FittingDM;
                this.QM.QPar.Length = "10";
                this.QM.QPar.Limits = "";
                this.QM.QPar.MCnt = 1;
                this.QM.QPar.SamCnt = 0;
                this.QM.QPar.Unit = "";
                this.QM.QPar.WL = "546.0";
                this.QM.QPar.ZeroB = false;
                this.QM.K0 = 0M;
                this.QM.K1 = 0.1M;
                this.QM.K2 = 0M;
                this.QM.K3 = 0M;
                this.QM.AFCS = " Abs = 0.1000 * C";
                this.QM.K10 = 0M;
                this.QM.K11 = 10M;
                this.QM.K12 = 0M;
                this.QM.K13 = 0M;
                this.QM.CFCS = "C=10.0000*A";
                this.QM.R = new System.Decimal?();
            }
            else
            {
                if (this.QM == null)
                {
                    this.QM = new QuaMethod();
                    try
                    {
                        this.QM.Page = 1;
                        this.QM.QPar = new QuaParmas();
                        this.QM.QPar.MeasureMethodName = CommonFun.getXmlValue("QuaParams", "MeaMethod");
                        this.QM.QPar.MeasureMethod = CommonFun.GetByName(this.QM.QPar.MeasureMethodName);
                        this.QM.QPar.MeasureMethodName = this.QM.QPar.MeasureMethod.C_name;
                        this.QM.QPar.CabMethodDM = CommonFun.getXmlValue("QuaParams", "CapMethod");
                        this.QM.QPar.CabMethod = this.QM.QPar.CabMethodDM;
                        this.QM.QPar.EConvert = Convert.ToBoolean(CommonFun.getXmlValue("QuaParams", "EConvert"));
                        this.QM.QPar.Cuvettemath = CommonFun.getXmlValue("QuaParams", "Bsmjz") != null && Convert.ToBoolean(CommonFun.getXmlValue("QuaParams", "Bsmjz"));
                        this.QM.QPar.Equation = CommonFun.getXmlValue("QuaParams", "Equation");
                        this.QM.QPar.FittingDM = CommonFun.getXmlValue("QuaParams", "Fitting");
                        this.QM.QPar.Fitting = this.QM.QPar.FittingDM;
                        this.QM.QPar.Length = CommonFun.getXmlValue("QuaParams", "Length");
                        this.QM.QPar.Limits = CommonFun.getXmlValue("QuaParams", "Threshold");
                        this.QM.QPar.MCnt = Convert.ToInt32(CommonFun.getXmlValue("QuaParams", "MCnt"));
                        this.QM.QPar.SamCnt = Convert.ToInt32(CommonFun.getXmlValue("QuaParams", "SamCnt"));
                        this.QM.QPar.Unit = CommonFun.getXmlValue("QuaParams", "Unit");
                        this.QM.QPar.WL = CommonFun.getXmlValue("QuaParams", "WL");
                        this.QM.QPar.ZeroB = Convert.ToBoolean(CommonFun.getXmlValue("QuaParams", "ZeroB"));
                        this.QM.K0 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K0"));
                        this.QM.K1 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K1"));
                        this.QM.K2 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K2"));
                        this.QM.K3 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K3"));
                        this.QM.AFCS = CommonFun.getXmlValue("QuaParams", "AFCS");
                        this.QM.K10 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K10"));
                        this.QM.K11 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K11"));
                        this.QM.K12 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K12"));
                        this.QM.K13 = Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "K13"));
                        this.QM.CFCS = CommonFun.getXmlValue("QuaParams", "CFCS");
                        this.QM.R = CommonFun.getXmlValue("QuaParams", "R") == null || CommonFun.getXmlValue("QuaParams", "R").Length <= 0 ? new System.Decimal?() : new System.Decimal?(Convert.ToDecimal(CommonFun.getXmlValue("QuaParams", "R")));
                        string xmlValue = CommonFun.getXmlValue("QuaParams", "Sample");
                        if (xmlValue.Length > 0)
                        {
                            this.QM.SamList = new List<Sample>();
                            try
                            {
                                string str1 = xmlValue;
                                char[] chArray = new char[1] { ';' };
                                foreach (string str2 in str1.Split(chArray))
                                {
                                    Sample sample = new Sample();
                                    if (((IEnumerable<string>)str2.Split(',')).Count<string>() > 1)
                                    {
                                        sample.ND = new System.Decimal?(Convert.ToDecimal(str2.Split(',')[0]));
                                        sample.XGD = new System.Decimal?(Convert.ToDecimal(str2.Split(',')[1]));
                                        this.QM.SamList.Add(sample);
                                    }
                                }
                            }
                            catch
                            {
                                CommonFun.showbox("loadparerror", "Error");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.QM.Page = 1;
                        this.QM.QPar = new QuaParmas();
                        this.QM.QPar.MeasureMethodName = "Одноволновое";
                        this.QM.QPar.MeasureMethod = CommonFun.GetByName(this.QM.QPar.MeasureMethodName);
                        this.QM.QPar.MeasureMethodName = this.QM.QPar.MeasureMethod.C_name;
                        this.QM.QPar.CabMethodDM = "Ввод коэффициентов уравнения";
                        this.QM.QPar.CabMethod = this.QM.QPar.CabMethodDM;
                        this.QM.QPar.EConvert = false;
                        this.QM.QPar.Cuvettemath = false;
                        this.QM.QPar.Equation = "C=f(Abs)";
                        this.QM.QPar.FittingDM = "Линейная";
                        this.QM.QPar.Fitting = this.QM.QPar.FittingDM;
                        this.QM.QPar.Length = "10";
                        this.QM.QPar.Limits = "";
                        this.QM.QPar.MCnt = 1;
                        this.QM.QPar.SamCnt = 0;
                        this.QM.QPar.Unit = "";
                        this.QM.QPar.WL = "546.0";
                        this.QM.QPar.ZeroB = false;
                        this.QM.K0 = 0M;
                        this.QM.K1 = 0.1M;
                        this.QM.K2 = 0M;
                        this.QM.K3 = 0M;
                        this.QM.AFCS = " Abs = 0.1000 * C";
                        this.QM.K10 = 0M;
                        this.QM.K11 = 10M;
                        this.QM.K12 = 0M;
                        this.QM.K13 = 0M;
                        this.QM.CFCS = "C=10.0000*A";
                        this.QM.R = new System.Decimal?();
                    }
                }
            }

            if (this.QM.QPar.Equation == "Abs=f(C)")
                this.lblfor.Content = this.QM.AFCS;
            else
                this.lblfor.Content = this.QM.CFCS;
            this.lblfor.Content = this.lblfor.Content + ";" + this.QM.QPar.MeasureMethod.C_gs;
            this.lblUnit.Content = this.QM.QPar.Unit;
            this.Cleardata();
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("rightquameasure"))
                    this.btnScan.IsEnabled = true;
                else
                    this.btnScan.IsEnabled = false;
                if (this.rightlist.Contains("rightquablank"))
                    this.btnBlank.IsEnabled = true;
                else
                    this.btnBlank.IsEnabled = false;
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
                this.btnBack.IsEnabled = true;
            // this.picOut.Height = 240;
            //this.picOut.Width = 320;
            this.tt.Interval = 1000;
            this.tt.Tick += new EventHandler(this.tt_Tick);
        }
        private void tt_Tick(object sender, EventArgs e)
        {
            ++this.tickcnt;
            if (this.tickcnt != 60)
                return;
            this.ComSta = ComStatus.END;
            if (!this.btnBlank.Dispatcher.CheckAccess())
                this.btnBlank.Dispatcher.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
            else
                this.setstate(true);
        }

        private void setstate(bool status)
        {
            this.btnBack.IsEnabled = status;
            this.btnBlank.IsEnabled = status;
            this.btnScan.IsEnabled = status;
            /*this.btnJCC.Enabled = status;
            if (this.btnJCC.Enabled)
                this.btnJCC.Enabled = this.QM.QPar.Cuvettemath;*/
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("rightquameasure") && status)
                    this.btnScan.IsEnabled = true;
                else
                    this.btnScan.IsEnabled = false;
                if (this.rightlist.Contains("rightquablank") && status)
                    this.btnBlank.IsEnabled = true;
                else
                    this.btnBlank.IsEnabled = false;
                /*if (CommonFun.GetAppConfig("LockSystem") == "true")
                    this.btnlock.Visible = true;
                else
                    this.btnlock.Visible = false;*/
            }
            /* else
                 this.btnlock.Visible = false;*/
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
               // sp.WriteLine("CONNECT \r\n");
                CommonFun.WriteSendLine("定量 ");
                this.sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);
                this.ComSta = ComStatus.BD_RATIO_FLUSH;
                this.sp.WriteLine("BD_RATIO_FLUSH \r\n");
                CommonFun.WriteSendLine("BD_RATIO_FLUSH");
                if (!this.btnBlank.Dispatcher.CheckAccess())
                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_starttt(this.Starttt), (object)true);
                else
                    this.Starttt(true);
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
                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_starttt(this.Starttt), (object)false);
                else
                    this.Starttt(false);
            }
        }
        private void DealRecData()
        {
            while (this.runptag)
            {
                if (this.myque.Count > 0)
                {
                    string str1 = this.myque.Dequeue().ToString();
                    try
                    {
                        switch (this.ComSta)
                        {
                            case ComStatus.SETCHP:
                                try
                                {
                                    if (str1.Contains("*A#"))
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (this.calormea == 1)
                                        {
                                            if (this.SubScan)
                                                this.SendBlankCmd(this.subQM);
                                            else
                                                this.SendBlankCmd(this.QM);
                                        }
                                        else
                                        {
                                            if (this.SubScan)
                                                this.SendScanCmd(this.subQM);
                                            else
                                                this.SendScanCmd(this.QM);
                                            ++this.currslotno;
                                        }
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox("errorretry" + ex.ToString(), "Error");
                                    this.ComSta = ComStatus.END;
                                    this.currslotno = 0;
                                    this.calormea = 0;
                                    break;
                                }
                            case ComStatus.SETSCANWL:
                                if (str1.Contains("*A# " + this.scanwlpar))
                                {
                                    this.ComSta = ComStatus.SETMOSPD;
                                    this.sp.WriteLine("SETMOSPD 2 \r\n");
                                    CommonFun.WriteSendLine("SETMOSPD 2");
                                    break;
                                }
                                break;
                            case ComStatus.SETMOSPD:
                                if (str1.Contains("*A# 2"))
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
                                CommonFun.WriteLine(str1);
                                this.receive += str1;
                                try
                                {
                                    int num1;
                                    int int16_1;
                                    int int16_2;
                                    if (this.SubScan)
                                    {
                                        if (this.smfrm.QM.QPar.MeasureMethodName == "Границы")
                                        {
                                            if (str1.Contains("*DAT"))
                                            {
                                                int startIndex = str1.IndexOf("*DAT") + 4;
                                                str1 = str1.Substring(startIndex, str1.Length - startIndex);
                                                if (str1.Contains("END"))
                                                    str1 = str1.Substring(0, str1.Length - 4);
                                                string[] strArray = new Regex("DAT").Split(str1);
                                                for (int index = 0; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                                                {
                                                    MeaureData meaureData = new MeaureData()
                                                    {
                                                        xValue = (Convert.ToDecimal(strArray[index].Split(' ')[3]) / 10M).ToString("f1"),
                                                        YT = (float)Convert.ToDouble(strArray[index].Split(' ')[1])
                                                    };
                                                    meaureData.yABS = (double)meaureData.YT > 0.01 ? (float)(2.0 - Math.Log10(Convert.ToDouble(meaureData.YT))) : 4f;
                                                    if (this.smfrm.QM.QPar.Length != "10" && this.smfrm.QM.QPar.EConvert)
                                                    {
                                                        meaureData.yABS *= (float)(Convert.ToDouble(10) / Convert.ToDouble(this.smfrm.QM.QPar.Length));
                                                        meaureData.YT = (float)Math.Pow(10.0, 2.0 - (double)meaureData.yABS);
                                                    }
                                                    this.mdlists.Add(meaureData);
                                                }
                                            }
                                            if (str1.Contains("END"))
                                            {
                                                if (this.stophappened)
                                                {
                                                    this.meacnt = 0;
                                                    this.currslotno = 0;
                                                    this.calormea = 0;
                                                    this.ComSta = ComStatus.END;
                                                    this.stophappened = true;
                                                    if (!this.smfrm.btnMeasure.Dispatcher.CheckAccess())
                                                        this.smfrm.btnMeasure.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)true);
                                                    else
                                                        this.SetMealable(true);
                                                    this.SubScan = false;
                                                }
                                                else
                                                {
                                                    this.ComSta = ComStatus.END;
                                                    float num2 = (float)Convert.ToDouble(this.smfrm.QM.QPar.WL.Split(',')[2]) * 10;
                                                    float num3 = 0.0f;
                                                    for (int index = 0; index < this.mdlists.Count; ++index)
                                                    {
                                                        float yAbs = this.mdlists[index].yABS;
                                                        num3 += num2 * yAbs;
                                                    }
                                                    int rowIndex = this.smfrm.dataGridView1.SelectedCells[0].RowIndex;
                                                    if (rowIndex >= 0 && rowIndex < this.smfrm.QM.SamList.Count<Sample>())
                                                    {
                                                        this.smfrm.QM.SamList[rowIndex].XGD = new System.Decimal?(Convert.ToDecimal(num3.ToString(this.absacc)));
                                                        this.smfrm.QM.SamList[rowIndex].D_sj = new DateTime?(DateTime.Now);
                                                        if (this.smfrm.dataGridView1.InvokeRequired)
                                                            this.smfrm.dataGridView1.Invoke((Delegate)new Del_BindData(this.LoadStandardData), (object)0);
                                                        else
                                                            this.LoadStandardData(0);
                                                    }
                                                    int num4;
                                                    if (this.slotno.Length > 0)
                                                        num4 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                    else
                                                        num4 = 1;
                                                    if (num4 == 0)
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
                                                        this.receive = "";
                                                        this.ComSta = ComStatus.END;
                                                        this.mcnt = 0;
                                                        this.mdlists = new List<MeaureData>();
                                                        if (!this.smfrm.btnMeasure.Dispatcher.CheckAccess())
                                                            this.smfrm.btnMeasure.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)true);
                                                        else
                                                            this.SetMealable(true);
                                                        this.SubScan = false;
                                                        CommonFun.showbox("Измерение закончено", "Information");
                                                    }
                                                }
                                                break;
                                            }
                                            break;
                                        }
                                        if (str1.Contains("END"))
                                        {
                                            if (this.stophappened)
                                            {
                                                this.stophappened = false;
                                                this.currslotno = 0;
                                                this.receive = "";
                                                this.meacnt = 0;
                                                this.ComSta = ComStatus.END;
                                                if (!this.smfrm.btnMeasure.Dispatcher.CheckAccess())
                                                    this.smfrm.btnMeasure.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)true);
                                                else
                                                    this.SetMealable(true);
                                                this.SubScan = false;
                                            }
                                            else if (this.calormea == 3)
                                            {
                                                if (((IEnumerable<string>)this.slotno.Split(',')).Count<string>() > 1)
                                                {
                                                    int num2 = this.currslotno * 100 / ((IEnumerable<string>)this.slotno.Split(',')).Count<string>();
                                                    if (!this.smfrm.progressBar1.Dispatcher.CheckAccess())
                                                        this.smfrm.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)num2);
                                                    else
                                                        this.smfrm.progressBar1.Value = Convert.ToInt32(num2);
                                                }
                                                else
                                                {
                                                    int num2 = this.meacnt * 100 / ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>();
                                                    if (!this.smfrm.progressBar1.Dispatcher.CheckAccess())
                                                        this.smfrm.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)num2);
                                                    else
                                                        this.smfrm.progressBar1.Value = Convert.ToInt32(num2);
                                                }
                                                if (this.meacnt < ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>())
                                                {
                                                    this.ComSta = ComStatus.END;
                                                    SerialPort sp = this.sp;
                                                    System.Decimal num2 = Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[this.meacnt]) * 10M;
                                                    string text = "measure 1 2 " + num2.ToString("f0") + "\r\n";
                                                    sp.WriteLine(text);
                                                    this.ComSta = ComStatus.MEASURE;
                                                    num2 = Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[this.meacnt]) * 10M;
                                                    CommonFun.WriteSendLine("measure 1 2 " + num2.ToString("f0"));
                                                    ++this.meacnt;
                                                }
                                                else
                                                {
                                                    this.receive = this.receive.Replace("*DAT", "&");
                                                    string[] strArray1 = this.receive.Split('&');
                                                    for (int index = 0; index < ((IEnumerable<string>)strArray1).Count<string>(); ++index)
                                                    {
                                                        int num2 = strArray1[index].IndexOf("END");
                                                        if (num2 > 0)
                                                        {
                                                            this.receive = strArray1[index].Substring(1, num2 - 1);
                                                            string[] strArray2 = this.receive.Split(' ');
                                                            QuaMeaData quaMeaData = new QuaMeaData();
                                                            System.Decimal num3 = Convert.ToDecimal(Convert.ToDouble(10) / Convert.ToDouble(this.smfrm.QM.QPar.Length));
                                                            quaMeaData.Value = Convert.ToDouble(strArray2[0]) > 0.01 ? (2.0 - Math.Log10(Convert.ToDouble(strArray2[0]))).ToString(this.absacc) : 4.ToString(this.absacc);
                                                            if (this.smfrm.QM.QPar.Length != "10" && this.smfrm.QM.QPar.EConvert)
                                                                quaMeaData.Value = (Convert.ToDecimal(quaMeaData.Value) * num3).ToString(this.absacc);
                                                            quaMeaData.WL = (int)Convert.ToInt16(strArray2[2]);
                                                            quaMeaData.Slot = Convert.ToInt32(this.slotno.Split(',')[this.currslotno - 1]);
                                                            this.bsmvalue.Add(quaMeaData);
                                                        }
                                                    }
                                                    int num4;
                                                    if (this.slotno.Length > 0)
                                                        num4 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                    else
                                                        num4 = 1;
                                                    if (num4 == 0)
                                                    {
                                                        this.meacnt = 0;
                                                        this.mcnt = 0;
                                                        this.receive = "";
                                                        if (CommonFun.GetAppConfig("EightSlot") == "false")
                                                            CommonFun.showbox("moveto" + " " + this.slotno.Split(',')[this.currslotno] + " " + "slot", "Warning");
                                                        this.ComSta = ComStatus.SETCHP;
                                                        this.sp.WriteLine("SETCHP " + this.slotno.Split(',')[this.currslotno] + "\r\n");
                                                        CommonFun.WriteSendLine("SETCHP " + this.slotno.Split(',')[this.currslotno]);
                                                    }
                                                    else
                                                    {
                                                        this.currslotno = 0;
                                                        this.receive = "";
                                                        this.mcnt = 0;
                                                        this.meacnt = 0;
                                                        this.ComSta = ComStatus.END;
                                                        if (!this.smfrm.btnMeasure.Dispatcher.CheckAccess())
                                                            this.smfrm.btnMeasure.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)true);
                                                        else
                                                            this.SetMealable(true);
                                                        string msg = "";
                                                        foreach (QuaMeaData quaMeaData in this.bsmvalue)
                                                            msg = msg + (object)quaMeaData.Slot + "号槽差：" + quaMeaData.Value + "\r\n";
                                                        CommonFun.showbox(msg, "Information");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int num2 = this.mcnt * 100 / this.smfrm.QM.QPar.MCnt;
                                                if (!this.smfrm.progressBar1.Dispatcher.CheckAccess())
                                                    this.smfrm.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)num2);
                                                else
                                                    this.smfrm.progressBar1.Value = num2;
                                                if (this.mcnt < this.smfrm.QM.QPar.MCnt)
                                                {
                                                    this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[this.meacnt - 1]) * 10M).ToString("f0") + "\r\n");
                                                    this.ComSta = ComStatus.MEASURE;
                                                    ++this.mcnt;
                                                }
                                                else
                                                {
                                                    this.mcnt = 0;
                                                    if (this.meacnt < ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>())
                                                    {
                                                        this.ComSta = ComStatus.END;
                                                        SerialPort sp = this.sp;
                                                        System.Decimal num3 = Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[this.meacnt]) * 10M;
                                                        string text = "measure 1 2 " + num3.ToString("f0") + "\r\n";
                                                        sp.WriteLine(text);
                                                        this.ComSta = ComStatus.MEASURE;
                                                        num3 = Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[this.meacnt]) * 10M;
                                                        CommonFun.WriteSendLine("smfrm,measure 1 2" + num3.ToString("f0"));
                                                        ++this.meacnt;
                                                        ++this.mcnt;
                                                    }
                                                    else
                                                    {
                                                        num1 = this.currslotno - 1;
                                                        int index1;
                                                        if (this.smfrm.QM.QPar.Cuvettemath && CommonFun.GetAppConfig("EightSlot") == "false")
                                                        {
                                                            if (!this.Dispatcher.CheckAccess())
                                                            {
                                                                IAsyncResult asyncResult = (IAsyncResult)this.Dispatcher.BeginInvoke((Delegate)new Del_Selectslot(this.Showslotbox), (object)this.slotno);
                                                                asyncResult.AsyncWaitHandle.WaitOne(-1);
                                                                index1 = (int)asyncResult.AsyncState;
                                                            }
                                                            else
                                                                index1 = this.Showslotbox(this.slotno);
                                                        }
                                                        else
                                                            index1 = 0;
                                                        this.receive = this.receive.Replace("*DAT", "&");
                                                        string[] strArray1 = this.receive.Split('&');
                                                        List<QuaMeaData> source1 = new List<QuaMeaData>();
                                                        for (int index2 = 0; index2 < ((IEnumerable<string>)strArray1).Count<string>(); ++index2)
                                                        {
                                                            int num3 = strArray1[index2].IndexOf("END");
                                                            if (num3 > 0)
                                                            {
                                                                this.receive = strArray1[index2].Substring(1, num3 - 1);
                                                                string[] strArray2 = this.receive.Split(' ');
                                                                QuaMeaData md = new QuaMeaData();
                                                                System.Decimal num4 = Convert.ToDecimal(Convert.ToDouble(10) / Convert.ToDouble(this.smfrm.QM.QPar.Length));
                                                                md.WL = (int)Convert.ToInt16(strArray2[2]);
                                                                int num5;
                                                                if (this.slotno.Length > 0)
                                                                    num5 = ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() <= 0 ? 1 : 0;
                                                                else
                                                                    num5 = 1;
                                                                if (num5 == 0)
                                                                    md.Slot = Convert.ToInt32(this.slotno.Split(',')[index1]);
                                                                else
                                                                    md.Slot = 0;
                                                                if (Convert.ToDouble(strArray2[0]) <= 0.01)
                                                                {
                                                                    md.Value = (4M * num4).ToString(this.absacc);
                                                                }
                                                                else
                                                                {
                                                                    md.Value = (2.0 - Math.Log10(Convert.ToDouble(strArray2[0]))).ToString(this.absacc);
                                                                    if (this.smfrm.QM.QPar.Length != "10" && this.smfrm.QM.QPar.EConvert)
                                                                        md.Value = (Convert.ToDecimal(md.Value) * num4).ToString(this.absacc);
                                                                    if (this.smfrm.QM.QPar.Cuvettemath)
                                                                    {
                                                                        List<string> list = this.bsmvalue.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == md.WL && s.Slot == md.Slot)).Select<QuaMeaData, string>((Func<QuaMeaData, string>)(v => v.Value)).ToList<string>();
                                                                        if (list.Count > 0)
                                                                        {
                                                                            string str2 = md.Value;
                                                                            md.Value = (Convert.ToSingle(md.Value) - Convert.ToSingle(list[0])).ToString(this.absacc);
                                                                            CommonFun.showbox("原始测量值：" + str2 + "，槽差值：" + list[0] + "。最终结果：" + md.Value, "Information");
                                                                        }
                                                                    }
                                                                }
                                                                source1.Add(md);
                                                            }
                                                        }
                                                        if (this.smfrm.QM.MeasreList == null)
                                                            this.smfrm.QM.MeasreList = new List<Sample>();
                                                        System.Decimal[] numArray = new System.Decimal[((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>()];
                                                        int index3 = 0;
                                                        while (true)
                                                        {
                                                            if (index3 < ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>())
                                                            {
                                                                int wl1 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[index3]) * 10M);
                                                                if (source1.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)) != null)
                                                                {
                                                                    IEnumerable<System.Decimal> source2 = source1.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)).Select<QuaMeaData, System.Decimal>((Func<QuaMeaData, System.Decimal>)(v => Convert.ToDecimal(v.Value)));
                                                                    System.Decimal num3 = source2.Sum() / (System.Decimal)source2.Count<System.Decimal>();
                                                                    numArray[index3] = num3;
                                                                }
                                                                ++index3;
                                                            }
                                                            else
                                                                break;
                                                        }
                                                        string str3;
                                                        if (this.smfrm.QM.QPar.MeasureMethodName == "Одноволновое")
                                                            str3 = numArray[0].ToString(this.absacc);
                                                        else if (this.smfrm.QM.QPar.MeasureMethodName == "Двухволновое")
                                                        {
                                                            System.Decimal num3 = Convert.ToDecimal(this.smfrm.QM.QPar.R);
                                                            int16_1 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[0]) * 10M);
                                                            int16_2 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[1]) * 10M);
                                                            System.Decimal num4 = numArray[0];
                                                            System.Decimal num5 = numArray[1];
                                                            if (num5 == 0M)
                                                                num5 = 0.000001M;
                                                            str3 = (num3 * num4 / num5).ToString();
                                                        }
                                                        else if (this.smfrm.QM.QPar.MeasureMethodName == "doublem")
                                                        {
                                                            int16_1 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[0]) * 10M);
                                                            int16_2 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[1]) * 10M);
                                                            str3 = (numArray[0] - numArray[1]).ToString();
                                                        }
                                                        else if (this.smfrm.QM.QPar.MeasureMethodName == "Трехволновое")
                                                        {
                                                            int int16_3 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[0]) * 10M);
                                                            int int16_4 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[1]) * 10M);
                                                            int int16_5 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[2]) * 10M);
                                                            System.Decimal num3 = numArray[0];
                                                            System.Decimal num4 = numArray[1];
                                                            System.Decimal num5 = numArray[2];
                                                            str3 = (num3 - num4 - (System.Decimal)(int16_3 / 10 - int16_4 / 10) * (num4 - num5) / (System.Decimal)(int16_4 / 10 - int16_5 / 10)).ToString();
                                                        }
                                                        else
                                                            str3 = this.Caculate(this.smfrm.QM.QPar.MeasureMethod.C_jsgs, ((IEnumerable<System.Decimal>)numArray).ToList<System.Decimal>(), this.smfrm.QM.QPar.MeasureMethod.wl);
                                                        int rowIndex = this.smfrm.dataGridView1.SelectedCells[0].RowIndex;
                                                        if (rowIndex >= 0 && rowIndex < this.smfrm.QM.SamList.Count<Sample>())
                                                        {
                                                            this.smfrm.QM.SamList[rowIndex].Avalue = numArray;
                                                            if (str3 == "--")
                                                            {
                                                                this.smfrm.QM.SamList[rowIndex].XGD = new System.Decimal?(-1M);
                                                            }
                                                            else
                                                            {
                                                                try
                                                                {
                                                                    this.smfrm.QM.SamList[rowIndex].XGD = new System.Decimal?(Convert.ToDecimal(Convert.ToSingle(str3)));
                                                                }
                                                                catch
                                                                {
                                                                    this.smfrm.QM.SamList[rowIndex].XGD = new System.Decimal?(-1M);
                                                                }
                                                            }
                                                            this.smfrm.QM.SamList[rowIndex].D_sj = new DateTime?(DateTime.Now);
                                                            if (this.smfrm.dataGridView1.InvokeRequired)
                                                                this.smfrm.dataGridView1.Invoke((Delegate)new Del_BindData(this.LoadStandardData), (object)0);
                                                            else
                                                                this.LoadStandardData(0);
                                                        }
                                                        int num6;
                                                        if (CommonFun.GetAppConfig("EightSlot") == "true" && this.slotno.Length > 0)
                                                            num6 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                        else
                                                            num6 = 1;
                                                        if (num6 == 0)
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
                                                            this.receive = "";
                                                            this.meacnt = 0;
                                                            this.ComSta = ComStatus.END;
                                                            if (!this.smfrm.btnMeasure.Dispatcher.CheckAccess())
                                                                this.smfrm.btnMeasure.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)true);
                                                            else
                                                                this.SetMealable(true);
                                                            this.SubScan = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    if (this.QM.QPar.MeasureMethodName == "Границы")
                                    {
                                        if (str1.Contains("*DAT"))
                                        {
                                            int startIndex = str1.IndexOf("*DAT") + 4;
                                            str1 = str1.Substring(startIndex, str1.Length - startIndex);
                                            if (str1.Contains("END"))
                                                str1 = str1.Substring(0, str1.Length - 4);
                                            string[] strArray = new Regex("DAT").Split(str1);
                                            for (int index = 0; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                                            {
                                                MeaureData meaureData = new MeaureData()
                                                {
                                                    xValue = (Convert.ToDecimal(strArray[index].Split(' ')[3]) / 10M).ToString("f1"),
                                                    YT = (float)Convert.ToDouble(strArray[index].Split(' ')[1])
                                                };
                                                meaureData.yABS = (double)meaureData.YT > 0.01 ? (float)(2.0 - Math.Log10(Convert.ToDouble(meaureData.YT))) : 4f;
                                                if (this.QM.QPar.Length != "10" && this.QM.QPar.EConvert)
                                                {
                                                    meaureData.yABS *= (float)(Convert.ToDouble(10) / Convert.ToDouble(this.QM.QPar.Length));
                                                    meaureData.YT = (float)Math.Pow(10.0, 2.0 - (double)meaureData.yABS);
                                                }
                                                this.mdlists.Add(meaureData);
                                            }
                                        }
                                        if (str1.Contains("END"))
                                        {
                                            if (this.stophappened)
                                            {
                                                this.meacnt = 0;
                                                this.mcnt = 0;
                                                this.currslotno = 0;
                                                this.slotno = "";
                                                this.calormea = 0;
                                                this.ComSta = ComStatus.END;
                                                this.stophappened = false;
                                                if (!this.btnScan.Dispatcher.CheckAccess())
                                                    this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)false);
                                                else
                                                    this.SetMealable(false);
                                            }
                                            else
                                            {
                                                this.ComSta = ComStatus.END;
                                                float num2 = (float)Convert.ToDouble(this.QM.QPar.WL.Split(',')[2]);
                                                float num3 = 0.0f;
                                                for (int index = 0; index < this.mdlists.Count; ++index)
                                                {
                                                    float yAbs = this.mdlists[index].yABS;
                                                    num3 += num2 * yAbs;
                                                }
                                                if (this.QM.MeasreList == null)
                                                    this.QM.MeasreList = new List<Sample>();
                                                int esStatus1 = this.QM.ESStatus;
                                                if (this.QM.ESStatus > 0)
                                                {
                                                    this.QM.MeasreList = new List<Sample>();
                                                    this.QM.ESStatus = 0;
                                                    this.QM.C_name = "";
                                                    this.QM.D_time = new DateTime?();
                                                }
                                                else
                                                {
                                                    DateTime? nullable1 = this.QM.D_time;
                                                    if (nullable1.HasValue && this.QM.MeasreList.Count > 0)
                                                    {
                                                        int esStatus2 = this.QM.ESStatus;
                                                        if (this.QM.ESStatus > 0)
                                                        {
                                                            this.QM.MeasreList = new List<Sample>();
                                                            this.QM.ESStatus = 0;
                                                            this.QM.C_name = "";
                                                            QuaMethod qm = this.QM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            qm.D_time = nullable2;
                                                        }
                                                        else if (CommonFun.GetAppConfig("currentuser") != this.QM.C_operator)
                                                        {
                                                            this.QM.MeasreList = new List<Sample>();
                                                            this.QM.ESStatus = 0;
                                                            this.QM.C_name = "";
                                                            QuaMethod qm = this.QM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            qm.D_time = nullable2;
                                                        }
                                                        else
                                                        {
                                                            QuaMethod qm = this.QM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            qm.D_time = nullable2;
                                                        }
                                                    }
                                                }
                                                this.QM.C_name = "";
                                                this.sslive = new Sample();
                                                if (this.QM.C_head != null && this.QM.C_head.Length > 0)
                                                    this.sslive.C_bz = this.QM.C_head + "-" + (this.QM.MeasreList.Count<Sample>() + 1).ToString();
                                                this.sslive.XGD = new System.Decimal?(Convert.ToDecimal(num3));
                                                Sample sslive1 = this.sslive;
                                                System.Decimal k13 = this.QM.K13;
                                                System.Decimal? nullable3 = this.sslive.XGD;
                                                nullable3 = nullable3.HasValue ? new System.Decimal?(k13 * nullable3.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal? nullable4 = this.sslive.XGD;
                                                nullable3 = nullable3.HasValue & nullable4.HasValue ? new System.Decimal?(nullable3.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new System.Decimal?();
                                                nullable4 = this.sslive.XGD;
                                                nullable3 = nullable3.HasValue & nullable4.HasValue ? new System.Decimal?(nullable3.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal k12 = this.QM.K12;
                                                nullable4 = this.sslive.XGD;
                                                nullable4 = nullable4.HasValue ? new System.Decimal?(k12 * nullable4.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal? nullable5 = this.sslive.XGD;
                                                nullable4 = nullable4.HasValue & nullable5.HasValue ? new System.Decimal?(nullable4.GetValueOrDefault() * nullable5.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal? nullable6;
                                                if (!(nullable3.HasValue & nullable4.HasValue))
                                                {
                                                    nullable5 = new System.Decimal?();
                                                    nullable6 = nullable5;
                                                }
                                                else
                                                    nullable6 = new System.Decimal?(nullable3.GetValueOrDefault() + nullable4.GetValueOrDefault());
                                                nullable3 = nullable6;
                                                System.Decimal k11 = this.QM.K11;
                                                nullable4 = this.sslive.XGD;
                                                System.Decimal? nullable7;
                                                if (!nullable4.HasValue)
                                                {
                                                    nullable5 = new System.Decimal?();
                                                    nullable7 = nullable5;
                                                }
                                                else
                                                    nullable7 = new System.Decimal?(k11 * nullable4.GetValueOrDefault());
                                                nullable4 = nullable7;
                                                System.Decimal? nullable8;
                                                if (!(nullable3.HasValue & nullable4.HasValue))
                                                {
                                                    nullable5 = new System.Decimal?();
                                                    nullable8 = nullable5;
                                                }
                                                else
                                                    nullable8 = new System.Decimal?(nullable3.GetValueOrDefault() + nullable4.GetValueOrDefault());
                                                nullable3 = nullable8;
                                                System.Decimal k10 = this.QM.K10;
                                                System.Decimal? nullable9;
                                                if (!nullable3.HasValue)
                                                {
                                                    nullable4 = new System.Decimal?();
                                                    nullable9 = nullable4;
                                                }
                                                else
                                                    nullable9 = new System.Decimal?(nullable3.GetValueOrDefault() + k10);
                                                sslive1.ND = nullable9;
                                                Sample sslive2 = this.sslive;
                                                nullable3 = this.sslive.ND;
                                                System.Decimal? nullable10 = new System.Decimal?(Convert.ToDecimal(nullable3.Value.ToString(this.conacc)));
                                                sslive2.ND = nullable10;
                                                this.sslive.D_sj = new DateTime?(DateTime.Now);
                                                this.QM.MeasreList.Add(this.sslive);
                                                this.currindex = this.QM.MeasreList.Count - 1;
                                               // if (!this.lblValue.Dispatcher.CheckAccess())
                                                //    this.lblValue.Dispatcher.Invoke((Delegate)new Del_BindData(this.LoadMeaData), (object)(this.QM.MeasreList.Count - 1));
                                             //   else
                                                this.LoadMeaData(current_serias);
                                                this.receive = "";
                                                this.mdlists = new List<MeaureData>();
                                                this.ComSta = ComStatus.END;
                                                if (this.mcnt < (int)Convert.ToInt16(this.QM.QPar.MCnt))
                                                {
                                                    this.sp.WriteLine("scan_measure 1 3\r\n");
                                                    this.ComSta = ComStatus.MEASURE;
                                                    CommonFun.WriteSendLine("scan_measure 1 3");
                                                    ++this.mcnt;
                                                }
                                                else
                                                {
                                                    int num4;
                                                    if (this.slotno.Length > 0)
                                                        num4 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                    else
                                                        num4 = 1;
                                                    if (num4 == 0)
                                                    {
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
                                                        this.ComSta = ComStatus.END;
                                                        if (!this.btnScan.Dispatcher.CheckAccess())
                                                            this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)false);
                                                        else
                                                            this.btnScan.Content = "Измерение";
                                                        CommonFun.showbox("Измерения окончены", "Information");
                                                        this.mcnt = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (str1.Contains("END"))
                                    {
                                        if (this.stophappened)
                                        {
                                            this.stophappened = false;
                                            this.currslotno = 0;
                                            this.mcnt = 0;
                                            this.receive = "";
                                            this.meacnt = 0;
                                            this.ComSta = ComStatus.END;
                                            if (!this.btnScan.Dispatcher.CheckAccess())
                                                this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)false);
                                            else
                                                this.SetMealable(false);
                                        }
                                        else if (this.calormea == 3)
                                        {
                                            if (((IEnumerable<string>)this.slotno.Split(',')).Count<string>() > 1)
                                            {
                                                int num2 = this.currslotno * 100 / ((IEnumerable<string>)this.slotno.Split(',')).Count<string>();
                                                if (!this.progressBar1.Dispatcher.CheckAccess())
                                                    this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)num2);
                                                else
                                                    this.progressBar1.Value = Convert.ToInt32(num2);
                                            }
                                            else
                                            {
                                                int num2 = this.meacnt * 100 / ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>();
                                                if (!this.progressBar1.Dispatcher.CheckAccess())
                                                    this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)num2);
                                                else
                                                    this.progressBar1.Value = Convert.ToInt32(num2);
                                            }
                                            if (this.meacnt < ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>())
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.meacnt]) * 10M).ToString("f0") + "\r\n");
                                                this.ComSta = ComStatus.MEASURE;
                                                CommonFun.WriteSendLine("measure 1 2 " + (Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.meacnt]) * 10M).ToString("f0"));
                                                ++this.meacnt;
                                            }
                                            else
                                            {
                                                this.receive = this.receive.Replace("*DAT", "&");
                                                string[] strArray1 = this.receive.Split('&');
                                                for (int index = 0; index < ((IEnumerable<string>)strArray1).Count<string>(); ++index)
                                                {
                                                    int num2 = strArray1[index].IndexOf("END");
                                                    if (num2 > 0)
                                                    {
                                                        this.receive = strArray1[index].Substring(1, num2 - 1);
                                                        string[] strArray2 = this.receive.Split(' ');
                                                        QuaMeaData quaMeaData = new QuaMeaData();
                                                        System.Decimal num3 = Convert.ToDecimal(Convert.ToDouble(10) / Convert.ToDouble(this.QM.QPar.Length));
                                                        quaMeaData.Value = Convert.ToDouble(strArray2[0]) > 0.01 ? (2.0 - Math.Log10(Convert.ToDouble(strArray2[0]))).ToString(this.absacc) : 4.ToString(this.absacc);
                                                        if (this.QM.QPar.Length != "10" && this.QM.QPar.EConvert)
                                                            quaMeaData.Value = (Convert.ToDecimal(quaMeaData.Value) * num3).ToString(this.absacc);
                                                        quaMeaData.WL = (int)Convert.ToInt16(strArray2[2]);
                                                        quaMeaData.Slot = Convert.ToInt32(this.slotno.Split(',')[this.currslotno - 1]);
                                                        this.bsmvalue.Add(quaMeaData);
                                                    }
                                                }
                                                int num4;
                                                if (this.slotno.Length > 0)
                                                    num4 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                else
                                                    num4 = 1;
                                                if (num4 == 0)
                                                {
                                                    this.meacnt = 0;
                                                    this.mcnt = 0;
                                                    this.receive = "";
                                                    if (CommonFun.GetAppConfig("EightSlot") == "false")
                                                        CommonFun.showbox("moveto" + " " + this.slotno.Split(',')[this.currslotno] + " " + "slot", "Warning");
                                                    this.ComSta = ComStatus.SETCHP;
                                                    this.sp.WriteLine("SETCHP " + this.slotno.Split(',')[this.currslotno] + "\r\n");
                                                    CommonFun.WriteSendLine("SETCHP " + this.slotno.Split(',')[this.currslotno]);
                                                }
                                                else
                                                {
                                                    this.currslotno = 0;
                                                    this.receive = "";
                                                    this.mcnt = 0;
                                                    this.meacnt = 0;
                                                    this.ComSta = ComStatus.END;
                                                    if (!this.btnScan.Dispatcher.CheckAccess())
                                                        this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)false);
                                                    else
                                                        this.SetMealable(false);
                                                    string msg = "";
                                                    foreach (QuaMeaData quaMeaData in this.bsmvalue)
                                                        msg = msg + (object)quaMeaData.Slot + "号槽差：" + quaMeaData.Value + "\r\n";
                                                    CommonFun.showbox(msg, "Information");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int num2 = this.meacnt * 100 / ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>();
                                            if (!this.progressBar1.Dispatcher.CheckAccess())
                                                this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)num2);
                                            else
                                                this.progressBar1.Value = Convert.ToInt32(num2);
                                            if (this.meacnt < ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>())
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.meacnt]) * 10M).ToString("f0") + "\r\n");
                                                this.ComSta = ComStatus.MEASURE;
                                                CommonFun.WriteSendLine("measure 1 2 " + (Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.meacnt]) * 10M).ToString("f0"));
                                                ++this.meacnt;
                                            }
                                            else
                                            {
                                                num1 = this.currslotno - 1;
                                                int index1;
                                                if (this.QM.QPar.Cuvettemath && CommonFun.GetAppConfig("EightSlot") == "false")
                                                {
                                                    if (!this.Dispatcher.CheckAccess())
                                                    {
                                                        IAsyncResult asyncResult = (IAsyncResult)this.Dispatcher.BeginInvoke(new Del_Selectslot(this.Showslotbox), (object)this.slotno);
                                                        asyncResult.AsyncWaitHandle.WaitOne(-1);
                                                        index1 = (int)asyncResult.AsyncState;
                                                    }
                                                    else
                                                        index1 = this.Showslotbox(this.slotno);
                                                }
                                                else
                                                    index1 = 0;
                                                this.receive = this.receive.Replace("*DAT", "&");
                                                string[] strArray1 = this.receive.Split('&');
                                                List<QuaMeaData> source1 = new List<QuaMeaData>();
                                                for (int index2 = 0; index2 < ((IEnumerable<string>)strArray1).Count<string>(); ++index2)
                                                {
                                                    int num3 = strArray1[index2].IndexOf("END");
                                                    if (num3 > 0)
                                                    {
                                                        this.receive = strArray1[index2].Substring(1, num3 - 1);
                                                        string[] strArray2 = this.receive.Split(' ');
                                                        QuaMeaData md = new QuaMeaData();
                                                        System.Decimal num4 = Convert.ToDecimal(Convert.ToDouble(10) / Convert.ToDouble(this.QM.QPar.Length));
                                                        md.WL = (int)Convert.ToInt16(strArray2[2]);
                                                        int num5;
                                                        if (this.slotno.Length > 0)
                                                            num5 = ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() <= 0 ? 1 : 0;
                                                        else
                                                            num5 = 1;
                                                        if (num5 == 0)
                                                            md.Slot = Convert.ToInt32(this.slotno.Split(',')[index1]);
                                                        else
                                                            md.Slot = 0;
                                                        md.Value = Convert.ToDouble(strArray2[0]) > 0.01 ? (2.0 - Math.Log10(Convert.ToDouble(strArray2[0]))).ToString(this.absacc) : 4.ToString(this.absacc);
                                                        if (this.QM.QPar.Length != "10" && this.QM.QPar.EConvert)
                                                            md.Value = (Convert.ToDecimal(md.Value) * num4).ToString(this.absacc);
                                                        if (this.QM.QPar.Cuvettemath)
                                                        {
                                                            List<string> list = this.bsmvalue.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == md.WL && s.Slot == md.Slot)).Select<QuaMeaData, string>((Func<QuaMeaData, string>)(v => v.Value)).ToList<string>();
                                                            if (list.Count > 0)
                                                            {
                                                                string str2 = md.Value;
                                                                md.Value = (Convert.ToSingle(md.Value) - Convert.ToSingle(list[0])).ToString(this.absacc);
                                                                CommonFun.showbox("原始测量值：" + str2 + "，槽差值：" + list[0] + "。最终结果：" + md.Value, "Information");
                                                            }
                                                        }
                                                        source1.Add(md);
                                                    }
                                                }
                                                if (this.QM.MeasreList == null)
                                                    this.QM.MeasreList = new List<Sample>();
                                                int esStatus1 = this.QM.ESStatus;
                                                if (this.QM.ESStatus > 0)
                                                {
                                                    this.QM.MeasreList = new List<Sample>();
                                                    this.QM.ESStatus = 0;
                                                    this.QM.C_name = "";
                                                    this.QM.D_time = new DateTime?();
                                                }
                                                else
                                                {
                                                    DateTime? nullable1 = this.QM.D_time;
                                                    if (nullable1.HasValue && this.QM.MeasreList.Count > 0)
                                                    {
                                                        int esStatus2 = this.QM.ESStatus;
                                                        if (this.QM.ESStatus > 0)
                                                        {
                                                            this.QM.MeasreList = new List<Sample>();
                                                            this.QM.ESStatus = 0;
                                                            this.QM.C_name = "";
                                                            QuaMethod qm = this.QM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            qm.D_time = nullable2;
                                                        }
                                                        else if (CommonFun.GetAppConfig("currentuser") != this.QM.C_operator)
                                                        {
                                                            this.QM.MeasreList = new List<Sample>();
                                                            this.QM.ESStatus = 0;
                                                            this.QM.C_name = "";
                                                            QuaMethod qm = this.QM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            qm.D_time = nullable2;
                                                        }
                                                        else
                                                        {
                                                            QuaMethod qm = this.QM;
                                                            nullable1 = new DateTime?();
                                                            DateTime? nullable2 = nullable1;
                                                            qm.D_time = nullable2;
                                                        }
                                                    }
                                                }
                                                this.sslive = new Sample();
                                                if (this.QM.C_head != null && this.QM.C_head.Length > 0)
                                                    this.sslive.C_bz = this.QM.C_head + "-" + (this.QM.MeasreList.Count<Sample>() + 1).ToString();
                                                System.Decimal[] numArray = new System.Decimal[((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>()];
                                                int index3 = 0;
                                                while (true)
                                                {
                                                    if (index3 < ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>())
                                                    {
                                                        int wl1 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[index3]) * 10M);
                                                        if (source1.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)) != null)
                                                        {
                                                            IEnumerable<System.Decimal> source2 = source1.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)).Select<QuaMeaData, System.Decimal>((Func<QuaMeaData, System.Decimal>)(v => Convert.ToDecimal(v.Value)));
                                                            System.Decimal num3 = source2.Sum() / (System.Decimal)source2.Count<System.Decimal>();
                                                            numArray[index3] = num3;
                                                        }
                                                        ++index3;
                                                    }
                                                    else
                                                        break;
                                                }
                                                this.sslive.Avalue = numArray;
                                                if (this.QM.QPar.MeasureMethodName == "Одноволновое")
                                                    this.sslive.XGD = new System.Decimal?(Convert.ToDecimal(numArray[0].ToString(this.absacc)));
                                                else if (this.QM.QPar.MeasureMethodName == "Двухволновое")
                                                {
                                                    System.Decimal num3 = Convert.ToDecimal(this.QM.QPar.R);
                                                    int16_1 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[0]) * 10M);
                                                    int16_2 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[1]) * 10M);
                                                    System.Decimal num4 = numArray[0];
                                                    System.Decimal num5 = numArray[1];
                                                    if (num5 == 0M)
                                                        num5 = 0.000001M;
                                                    this.sslive.XGD = new System.Decimal?(num3 * num4 / num5);
                                                }
                                                else if (this.QM.QPar.MeasureMethodName == "doublem")
                                                {
                                                    int16_1 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[0]) * 10M);
                                                    int16_2 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[1]) * 10M);
                                                    this.sslive.XGD = new System.Decimal?(numArray[0] - numArray[1]);
                                                }
                                                else if (this.QM.QPar.MeasureMethodName == "Трехволновое")
                                                {
                                                    int int16_3 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[0]) * 10M);
                                                    int int16_4 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[1]) * 10M);
                                                    int int16_5 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL.Split(',')[2]) * 10M);
                                                    System.Decimal num3 = numArray[0];
                                                    System.Decimal num4 = numArray[1];
                                                    System.Decimal num5 = numArray[2];
                                                    this.sslive.XGD = new System.Decimal?(num3 - num4 - (System.Decimal)(int16_3 / 10 - int16_4 / 10) * (num4 - num5) / (System.Decimal)(int16_4 / 10 - int16_5 / 10));
                                                }
                                                else
                                                {
                                                    string str2 = this.Caculate(this.QM.QPar.MeasureMethod.C_jsgs, ((IEnumerable<System.Decimal>)numArray).ToList<System.Decimal>(), this.QM.QPar.MeasureMethod.wl);
                                                    if (str2 == "--")
                                                    {
                                                        this.sslive.XGD = new System.Decimal?(-1M);
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            this.sslive.XGD = new System.Decimal?(Convert.ToDecimal(Convert.ToSingle(str2)));
                                                        }
                                                        catch
                                                        {
                                                            this.sslive.XGD = new System.Decimal?(-1M);
                                                        }
                                                    }
                                                }
                                                Sample sslive1 = this.sslive;
                                                System.Decimal k13 = this.QM.K13;
                                                System.Decimal? nullable3 = this.sslive.XGD;
                                                nullable3 = nullable3.HasValue ? new System.Decimal?(k13 * nullable3.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal? nullable4 = this.sslive.XGD;
                                                nullable3 = nullable3.HasValue & nullable4.HasValue ? new System.Decimal?(nullable3.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new System.Decimal?();
                                                nullable4 = this.sslive.XGD;
                                                nullable3 = nullable3.HasValue & nullable4.HasValue ? new System.Decimal?(nullable3.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal k12 = this.QM.K12;
                                                nullable4 = this.sslive.XGD;
                                                nullable4 = nullable4.HasValue ? new System.Decimal?(k12 * nullable4.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal? nullable5 = this.sslive.XGD;
                                                nullable4 = nullable4.HasValue & nullable5.HasValue ? new System.Decimal?(nullable4.GetValueOrDefault() * nullable5.GetValueOrDefault()) : new System.Decimal?();
                                                System.Decimal? nullable6;
                                                if (!(nullable3.HasValue & nullable4.HasValue))
                                                {
                                                    nullable5 = new System.Decimal?();
                                                    nullable6 = nullable5;
                                                }
                                                else
                                                    nullable6 = new System.Decimal?(nullable3.GetValueOrDefault() + nullable4.GetValueOrDefault());
                                                nullable3 = nullable6;
                                                System.Decimal k11 = this.QM.K11;
                                                nullable4 = this.sslive.XGD;
                                                System.Decimal? nullable7;
                                                if (!nullable4.HasValue)
                                                {
                                                    nullable5 = new System.Decimal?();
                                                    nullable7 = nullable5;
                                                }
                                                else
                                                    nullable7 = new System.Decimal?(k11 * nullable4.GetValueOrDefault());
                                                nullable4 = nullable7;
                                                System.Decimal? nullable8;
                                                if (!(nullable3.HasValue & nullable4.HasValue))
                                                {
                                                    nullable5 = new System.Decimal?();
                                                    nullable8 = nullable5;
                                                }
                                                else
                                                    nullable8 = new System.Decimal?(nullable3.GetValueOrDefault() + nullable4.GetValueOrDefault());
                                                nullable3 = nullable8;
                                                System.Decimal k10 = this.QM.K10;
                                                System.Decimal? nullable9;
                                                if (!nullable3.HasValue)
                                                {
                                                    nullable4 = new System.Decimal?();
                                                    nullable9 = nullable4;
                                                }
                                                else
                                                    nullable9 = new System.Decimal?(nullable3.GetValueOrDefault() + k10);
                                                sslive1.ND = nullable9;
                                                Sample sslive2 = this.sslive;
                                                nullable3 = this.sslive.ND;
                                                System.Decimal num6 = nullable3.Value;
                                                System.Decimal? nullable10 = new System.Decimal?(Convert.ToDecimal(num6.ToString(this.conacc)));
                                                sslive2.ND = nullable10;
                                                this.sslive.D_sj = new DateTime?(DateTime.Now);
                                                this.QM.MeasreList.Add(this.sslive);
                                                this.currindex = this.QM.MeasreList.Count - 1;
                                               // if (!this.lblValue.Dispatcher.CheckAccess())
                                             //       this.lblValue.Dispatcher.Invoke((Delegate)new Del_BindData(this.LoadMeaData), (object)(this.QM.MeasreList.Count - 1));
                                              //  else
                                               this.LoadMeaData(current_serias);
                                                if (this.mcnt < this.QM.QPar.MCnt)
                                                {
                                                    this.meacnt = 0;
                                                    SerialPort sp = this.sp;
                                                    num6 = Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.meacnt]) * 10M;
                                                    string text = "measure 1 2 " + num6.ToString("f0") + "\r\n";
                                                    sp.WriteLine(text);
                                                    this.ComSta = ComStatus.MEASURE;
                                                    num6 = Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.meacnt]) * 10M;
                                                    CommonFun.WriteSendLine("measure 1 2 " + num6.ToString("f0"));
                                                    ++this.meacnt;
                                                    ++this.mcnt;
                                                }
                                                else
                                                {
                                                    int num3;
                                                    if (CommonFun.GetAppConfig("EightSlot") == "true" && this.slotno.Length > 0)
                                                        num3 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                    else
                                                        num3 = 1;
                                                    if (num3 == 0)
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
                                                        this.mcnt = 0;
                                                        this.receive = "";
                                                        this.meacnt = 0;
                                                        this.ComSta = ComStatus.END;
                                                        if (!this.btnScan.Dispatcher.CheckAccess())
                                                            this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)false);
                                                        else
                                                            this.btnScan.Content = "Измерение";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox("Ошибка измерения", "Error");
                                    CommonFun.WriteSendLine("定量，测量错误，详细信息：" + ex.ToString());
                                    this.ComSta = ComStatus.END;
                                    CommonFun.WriteSendLine("error，");
                                    if (this.SubScan)
                                    {
                                        if (!this.smfrm.btnMeasure.Dispatcher.CheckAccess())
                                            this.smfrm.btnMeasure.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)true);
                                        else
                                            this.smfrm.btnMeasure.Content = "Измерение";
                                    }
                                    else if (!this.btnScan.Dispatcher.CheckAccess())
                                        this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)false);
                                    else
                                        this.btnScan.Content = "Измерение";
                                    this.currslotno = 0;
                                    this.calormea = 0;
                                    this.receive = "";
                                    this.mcnt = 0;
                                    this.meacnt = 0;
                                    this.SubScan = false;
                                    break;
                                }
                            case ComStatus.CALBGND:
                                CommonFun.WriteLine(str1);
                                try
                                {
                                    if (this.SubScan)
                                    {
                                        if (this.smfrm.QM.QPar.MeasureMethodName == "Границы")
                                        {
                                            if (str1.Contains("*CALDAT"))
                                            {
                                                int startIndex = str1.IndexOf("*CALDAT") + 7;
                                                str1 = str1.Substring(startIndex, str1.Length - startIndex);
                                                if (str1.Contains("END"))
                                                    str1 = str1.Substring(0, str1.Length - 7);
                                                string[] strArray = new Regex("CALDAT").Split(str1);
                                                string str2 = (Convert.ToDecimal(strArray[((IEnumerable<string>)strArray).Count<string>() - 1].Split(' ')[3]) / 10M).ToString("f1");
                                                System.Decimal num = Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[1]) * 10M - Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[0]) * 10M;
                                                int int32 = Convert.ToInt32((Convert.ToDecimal(str2) - Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[0])) * 10M / num);
                                                if (!this.smfrm.progressBar1.Dispatcher.CheckAccess())
                                                    this.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)int32);
                                                else
                                                    this.smfrm.progressBar1.Value = Convert.ToInt32(int32);
                                            }
                                            if (str1.Contains("END"))
                                            {
                                                if (this.stophappened)
                                                {
                                                    this.ComSta = ComStatus.END;
                                                    this.stophappened = false;
                                                    if (!this.smfrm.btnXL.Dispatcher.CheckAccess())
                                                        this.smfrm.btnXL.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)true);
                                                    else
                                                        this.Setblanklable(true);
                                                    this.SubScan = false;
                                                }
                                                else
                                                {
                                                    this.ComSta = ComStatus.END;
                                                    if (!this.smfrm.btnXL.Dispatcher.CheckAccess())
                                                        this.smfrm.btnXL.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)true);
                                                    else
                                                        this.Setblanklable(true);
                                                    this.SubScan = false;
                                                    CommonFun.showbox("Обнуление закончено", "Information");
                                                }
                                                break;
                                            }
                                            break;
                                        }
                                        if (str1.Contains("END"))
                                        {
                                            if (this.stophappened)
                                            {
                                                this.ComSta = ComStatus.END;
                                                if (!this.smfrm.btnXL.Dispatcher.CheckAccess())
                                                    this.smfrm.btnXL.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)true);
                                                else
                                                    this.Setblanklable(true);
                                                this.endcnt = 0;
                                                this.SubScan = false;
                                                this.stophappened = false;
                                            }
                                            else
                                            {
                                                ++this.endcnt;
                                                int num1 = this.endcnt * 100 / ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>();
                                                if (!this.smfrm.progressBar1.Dispatcher.CheckAccess())
                                                    this.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)num1);
                                                else
                                                    this.smfrm.progressBar1.Value = Convert.ToInt32(num1);
                                                if (this.endcnt < ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>())
                                                {
                                                    this.ComSta = ComStatus.END;
                                                    SerialPort sp = this.sp;
                                                    System.Decimal num2 = Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[this.endcnt]) * 10M;
                                                    string text = "calbgnd 1 1 " + num2.ToString("f0") + "\r\n";
                                                    sp.WriteLine(text);
                                                    num2 = Convert.ToDecimal(this.smfrm.QM.QPar.WL.Split(',')[this.endcnt]) * 10M;
                                                    CommonFun.WriteSendLine("calbgnd 1 1 " + num2.ToString("f0"));
                                                    this.ComSta = ComStatus.CALBGND;
                                                }
                                                else
                                                {
                                                    this.ComSta = ComStatus.END;
                                                    if (!this.smfrm.btnXL.Dispatcher.CheckAccess())
                                                        this.smfrm.btnXL.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)true);
                                                    else
                                                        this.Setblanklable(true);
                                                    this.endcnt = 0;
                                                    this.SubScan = false;
                                                    this.stophappened = false;
                                                    CommonFun.showbox("Каблировка закончена", "Information");
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    if (this.QM.QPar.MeasureMethodName == "Границы")
                                    {
                                        if (str1.Contains("*CALDAT"))
                                        {
                                            int startIndex = str1.IndexOf("*CALDAT") + 7;
                                            str1 = str1.Substring(startIndex, str1.Length - startIndex);
                                            if (str1.Contains("END"))
                                                str1 = str1.Substring(0, str1.Length - 7);
                                            string[] strArray = new Regex("CALDAT").Split(str1);
                                            string str2 = (Convert.ToDecimal(strArray[((IEnumerable<string>)strArray).Count<string>() - 1].Split(' ')[3]) / 10M).ToString("f1");
                                            System.Decimal num = Convert.ToDecimal(this.QM.QPar.WL.Split(',')[1]) * 10M - Convert.ToDecimal(this.QM.QPar.WL.Split(',')[0]) * 10M;
                                            int int32 = Convert.ToInt32((Convert.ToDecimal(str2) - Convert.ToDecimal(this.QM.QPar.WL.Split(',')[0])) * 10M / num);
                                            if (!this.progressBar1.Dispatcher.CheckAccess())
                                                this.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)int32);
                                            else
                                                this.progressBar1.Value = Convert.ToInt32(int32);
                                        }
                                        if (str1.Contains("END"))
                                        {
                                            if (this.stophappened)
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.stophappened = false;
                                                if (!this.btnBlank.Dispatcher.CheckAccess())
                                                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)false);
                                                else
                                                    this.Setblanklable(false);
                                            }
                                            else
                                            {
                                                this.ComSta = ComStatus.END;
                                                if (!this.btnBlank.Dispatcher.CheckAccess())
                                                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)false);
                                                else
                                                    this.Setblanklable(false);
                                                CommonFun.showbox("Обнуление закончена", "Information");
                                            }
                                        }
                                    }
                                    else if (str1.Contains("END"))
                                    {
                                        if (this.stophappened)
                                        {
                                            this.ComSta = ComStatus.END;
                                            if (!this.btnBlank.Dispatcher.CheckAccess())
                                                this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)false);
                                            else
                                                this.Setblanklable(false);
                                            this.endcnt = 0;
                                            this.SubScan = false;
                                            this.stophappened = false;
                                        }
                                        else
                                        {
                                            ++this.endcnt;
                                            int ipos = this.endcnt * 100 / ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>();
                                            if (!this.progressBar1.Dispatcher.CheckAccess())
                                                this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetPos(this.SetTextMessage), (object)ipos);
                                            else
                                                this.SetTextMessage(ipos);
                                            if (this.endcnt < ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>())
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.endcnt]) * 10M).ToString("f0") + "\r\n");
                                                CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(this.QM.QPar.WL.Split(',')[this.endcnt]) * 10M).ToString("f0"));
                                                this.ComSta = ComStatus.CALBGND;
                                            }
                                            else
                                            {
                                                this.ComSta = ComStatus.END;
                                                if (!this.btnBlank.Dispatcher.CheckAccess())
                                                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)false);
                                                else
                                                    this.Setblanklable(false);
                                                this.endcnt = 0;
                                                this.SubScan = false;
                                                this.stophappened = false;
                                                CommonFun.showbox("Обнуление закончено", "Information");
                                            }
                                        }
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox("errorstopblank" + "," + ex.ToString(), "Error");
                                    CommonFun.WriteSendLine("定量,error，");
                                    this.ComSta = ComStatus.END;
                                    if (this.SubScan)
                                    {
                                        if (!this.smfrm.btnXL.Dispatcher.CheckAccess())
                                            this.smfrm.btnXL.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)true);
                                        else
                                            this.smfrm.btnXL.Content = "Обнуление";
                                    }
                                    else if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)false);
                                    else
                                        this.btnBlank.Content = "Обнуление";
                                    this.currslotno = 0;
                                    this.calormea = 0;
                                    this.receive = "";
                                    this.SubScan = false;
                                    this.endcnt = 0;
                                    this.stophappened = false;
                                    break;
                                }
                            case ComStatus.BD_RATIO_FLUSH:
                                if (str1.Contains("RCVD"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
                                    else
                                        this.setstate(true);
                                    if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_starttt(this.Starttt), (object)false);
                                    else
                                        this.Starttt(false);
                                    break;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonFun.showbox("errorretry" + ex.ToString(), "Error");
                    }
                }
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
                    case ComStatus.SETCHP:
                    case ComStatus.SETSCANWL:
                    case ComStatus.SETMOSPD:
                    case ComStatus.MEASURE:
                    case ComStatus.CALBGND:
                    case ComStatus.SCANBASE:
                    case ComStatus.BD_RATIO_FLUSH:
                        int bytesToRead = this.sp.BytesToRead;
                        string text1 = this.sp.ReadLine();
                        this.myque.Enqueue((object)text1);
                        CommonFun.WriteReceiveMsg(text1);
                        for (int index = 1; text1.Length < bytesToRead - index; ++index)
                        {
                            string text2 = this.sp.ReadLine();
                            this.myque.Enqueue((object)text2);
                            CommonFun.WriteReceiveMsg(text2);
                            text1 += text2;
                        }
                        break;
                }
            }
            else
                CommonFun.showbox("opencom", "Warning");
        }

        private void SendBlankCmd(QuaMethod qm)
        {
            if (qm.QPar.MeasureMethodName == "Границы")
            {
                string str1 = qm.QPar.WL.Split(',')[0];
                string str2 = qm.QPar.WL.Split(',')[1];
                string str3 = qm.QPar.WL.Split(',')[2];
                if (Convert.ToDecimal(str1) * 10M < 190M || Convert.ToDecimal(str1) * 10M > 1100M || Convert.ToDecimal(str2) * 10M < 190M || Convert.ToDecimal(str2) * 10M > 1100M)
                    CommonFun.showbox("errordata", "Error");
                else if (str3 == "-1")
                {
                    CommonFun.showbox("errordata", "Error");
                }
                else
                {
                    if (Convert.ToDecimal(str1) < Convert.ToDecimal(str2))
                    {
                        string str4 = str1;
                        str1 = str2;
                        str2 = str4;
                    }
                    string[] strArray1 = new string[5]
                    {
            (Convert.ToDecimal(str1) * 10M).ToString("f0"),
            " ",
            (Convert.ToDecimal(str2) * 10M).ToString("f0"),
            " ",
            null
                    };
                    string[] strArray2 = strArray1;
                    System.Decimal num = Convert.ToDecimal(str3) * 10M;
                    string str5 = num.ToString("f0");
                    strArray2[4] = str5;
                    this.scanwlpar = string.Concat(strArray1);
                    this.ComSta = ComStatus.SETSCANWL;
                    SerialPort sp = this.sp;
                    string[] strArray3 = new string[7];
                    strArray3[0] = "setscanwl  ";
                    string[] strArray4 = strArray3;
                    num = Convert.ToDecimal(str1) * 10M;
                    string str6 = num.ToString("f0");
                    strArray4[1] = str6;
                    strArray3[2] = " ";
                    string[] strArray5 = strArray3;
                    num = Convert.ToDecimal(str2) * 10M;
                    string str7 = num.ToString("f0");
                    strArray5[3] = str7;
                    strArray3[4] = " ";
                    string[] strArray6 = strArray3;
                    num = Convert.ToDecimal(str3) * 10M;
                    string str8 = num.ToString("f0");
                    strArray6[5] = str8;
                    strArray3[6] = "\r\n";
                    string text = string.Concat(strArray3);
                    sp.WriteLine(text);
                    string[] strArray7 = new string[6];
                    strArray7[0] = "setscanwl  ";
                    string[] strArray8 = strArray7;
                    num = Convert.ToDecimal(str1) * 10M;
                    string str9 = num.ToString("f0");
                    strArray8[1] = str9;
                    strArray7[2] = " ";
                    string[] strArray9 = strArray7;
                    num = Convert.ToDecimal(str2) * 10M;
                    string str10 = num.ToString("f0");
                    strArray9[3] = str10;
                    strArray7[4] = " ";
                    string[] strArray10 = strArray7;
                    num = Convert.ToDecimal(str3) * 10M;
                    string str11 = num.ToString("f0");
                    strArray10[5] = str11;
                    CommonFun.WriteSendLine(string.Concat(strArray7));
                    if (!this.btnBack.Dispatcher.CheckAccess())
                        this.btnBack.Dispatcher.Invoke((Delegate)new Del_SetState(this.SetState), (object)ComStatus.CALBGND);
                    else
                        this.SetState(ComStatus.CALBGND);
                }
            }
            else
            {
                string[] strArray = qm.QPar.WL.Split(',');
                try
                {
                    this.ComSta = ComStatus.CALBGND;
                    for (int index = 0; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                    {
                        //CommonFun.showbox(strArray[index], "Info");
                        if (Convert.ToDecimal(strArray[index]) < 190M || Convert.ToDecimal(strArray[index]) > 1100M)
                        {
                            CommonFun.showbox("methoderror", "Error");
                            if (this.SubScan)
                            {
                                if (!this.smfrm.btnXL.Dispatcher.CheckAccess())
                                {
                                    this.smfrm.btnXL.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)true);
                                    return;
                                }
                                this.Setblanklable(true);
                                return;
                            }
                            if (!this.btnBlank.Dispatcher.CheckAccess())
                            {
                                this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)false);
                                return;
                            }
                            this.Setblanklable(false);
                            return;
                        }
                    }
                    this.ComSta = ComStatus.CALBGND;
                    SerialPort sp = this.sp;
                    System.Decimal num = Convert.ToDecimal(strArray[0]) * 10M;
                    string text = "calbgnd 1 1 " + num.ToString("f0") + "\r\n";
                    sp.WriteLine(text);
                    num = Convert.ToDecimal(strArray[0]) * 10M;
                    CommonFun.WriteSendLine("calbgnd 1 1 " + num.ToString("f0"));
                    if (!this.btnBack.Dispatcher.CheckAccess())
                        this.btnBack.Dispatcher.Invoke((Delegate)new Del_SetState(this.SetState), (object)ComStatus.CALBGND);
                    else
                        this.SetState(ComStatus.CALBGND);
                }
                catch (Exception ex)
                {
                    CommonFun.showbox("errorstopblank" + "," + ex.ToString(), "Error");
                    this.ComSta = ComStatus.END;
                    if (this.SubScan)
                    {
                        if (!this.smfrm.btnXL.Dispatcher.CheckAccess())
                            this.smfrm.btnXL.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)true);
                        else
                            this.Setblanklable(true);
                    }
                    else if (!this.btnBlank.Dispatcher.CheckAccess())
                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_SetBlankLable(this.Setblanklable), (object)false);
                    else
                        this.Setblanklable(false);
                }
            }
        }

        private void SendScanCmd(QuaMethod qm)
        {
            if (qm.QPar.MeasureMethodName == "Границы")
            {
                this.ComSta = ComStatus.MEASURE;
                this.sp.WriteLine("scan_measure 1 3\r\n");
                CommonFun.WriteSendLine("scan_measure 1 3");
                ++this.mcnt;
                if (!this.btnBack.Dispatcher.CheckAccess())
                    this.btnBack.Dispatcher.Invoke((Delegate)new Del_SetState(this.SetState), (object)ComStatus.MEASURE);
                else
                    this.SetState(ComStatus.MEASURE);
            }
            else
            {
                bool flag = true;
                string wl;
                if (qm.QPar.MeasureMethodName == "Одноволновое")
                {
                    wl = qm.QPar.WL;
                    if (Convert.ToDecimal(wl) < 190M || Convert.ToDecimal(wl) > 1100M)
                        flag = false;
                }
                else if (qm.QPar.MeasureMethodName == "Двухволновое" || qm.QPar.MeasureMethodName == "doublem")
                {
                    wl = qm.QPar.WL.Split(',')[0];
                    string str = qm.QPar.WL.Split(',')[1];
                    if (Convert.ToDecimal(wl) < 190M || Convert.ToDecimal(wl) > 1100M || Convert.ToDecimal(str) < 190M || Convert.ToDecimal(str) > 1100M)
                        flag = false;
                    if (qm.QPar.MeasureMethodName == "Двухволновое")
                    {
                        try
                        {
                            Convert.ToDecimal(qm.QPar.R);
                        }
                        catch
                        {
                            flag = false;
                        }
                    }
                }
                else if (qm.QPar.MeasureMethodName == "Трехволновое")
                {
                    wl = qm.QPar.WL.Split(',')[0];
                    string str1 = qm.QPar.WL.Split(',')[1];
                    string str2 = qm.QPar.WL.Split(',')[2];
                    if (Convert.ToDecimal(wl) < 190M || Convert.ToDecimal(wl) > 1100M || (Convert.ToDecimal(str1) < 190M || Convert.ToDecimal(str1) > 1100M) || Convert.ToDecimal(str2)  < 190M || Convert.ToDecimal(str2) > 1100M)
                        flag = false;
                }
                else
                {
                    string[] strArray = qm.QPar.WL.Split(',');
                    for (int index = 0; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                    {
                        if (Convert.ToDecimal(strArray[index]) < 190M || Convert.ToDecimal(strArray[index]) > 1100M)
                        {
                            flag = false;
                            break;
                        }
                    }
                    wl = strArray[0];
                }
                if (flag)
                {
                    this.ComSta = ComStatus.MEASURE;
                    SerialPort sp = this.sp;
                    System.Decimal num = Convert.ToDecimal(wl) * 10M;
                    string text = "measure 1 2 " + num.ToString("f0") + "\r\n";
                    sp.WriteLine(text);
                    num = Convert.ToDecimal(wl) * 10M;
                    CommonFun.WriteSendLine("measure 1 2 " + num.ToString("f0"));
                    ++this.meacnt;
                    ++this.mcnt;
                    if (!this.btnBack.Dispatcher.CheckAccess())
                        this.btnBack.Dispatcher.Invoke((Delegate)new Del_SetState(this.SetState), (object)ComStatus.MEASURE);
                    else
                        this.SetState(ComStatus.MEASURE);
                }
                else
                {
                    CommonFun.showbox("methoderror", "Error");
                    this.meacnt = 0;
                    this.mcnt = 0;
                    this.ComSta = ComStatus.END;
                    if (this.SubScan)
                    {
                        if (!this.smfrm.btnMeasure.Dispatcher.CheckAccess())
                            this.smfrm.btnMeasure.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)true);
                        else
                            this.SetMealable(true);
                    }
                    else if (!this.btnScan.Dispatcher.CheckAccess())
                        this.btnScan.Dispatcher.Invoke((Delegate)new Del_SetMeasureLable(this.SetMealable), (object)false);
                    else
                        this.SetMealable(false);
                }
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
        private void Set_Wl_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Meisure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            /*if (this.QM.C_methodcreator == null || this.QM.C_methodcreator.Length <= 0)
            {
                CommonFun.showbox(CommonFun.GetLanText(this.lanvalue, "nomethod"), "Error");
            }
            else
            {*/
            string errormsg = "";
            /*if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                CommonFun.showbox(errormsg, "Error");
            else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
            {
                CommonFun.showbox(errormsg, "Error");
            }
            else
            {*/
            int esStatus = this.QM.ESStatus;
            /*if (this.QM.ESStatus > 0 && new DRMessageBoxFrm(CommonFun.GetLanText(this.lanvalue, "esdatawarning"), "Warning").ShowDialog() == DialogResult.No)
                return;*/
            if (!this.sp.IsOpen)
                CommonFun.showbox("opencom", "Warning");
            else if (this.btnScan.Content.ToString() == "Измерения")
            {
                if (this.QM.QPar.MeasureMethodName == "Границы")
                {
                    this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                    CommonFun.WriteSendLine("stop,SCAN_STOPPING 0");
                    this.btnScan.IsEnabled = false;
                }
                else
                {
                    this.btnScan.IsEnabled = false;
                    this.stophappened = true;
                    CommonFun.WriteSendLine("stop,");
                    this.meacnt = 0;
                    this.mcnt = 0;
                    this.endcnt = 0;
                }
            }
            else if (this.ComSta != ComStatus.END)
            {
                CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
            }
            else
            {
                this.calormea = 2;
                current_serias = 0;
                this.lblNo.Content = (current_serias).ToString("D4") + "/" + this.QM.QPar.MCnt.ToString("D4");
                if (CommonFun.GetAppConfig("EightSlot") == "true")
                {
                    this.slotno = "";
                    /* using (ChoseSlotFrm frm = new ChoseSlotFrm())
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
                             frm.Close(); frm.Dispose();
                             this.slotno = this.slotno.Substring(0, this.slotno.Length - 1);
                             this.ComSta = ComStatus.SETCHP;
                             this.sp.WriteLine("SETCHP " + this.slotno.Split(',')[0] + "\r\n");
                             CommonFun.WriteSendLine("SETCHP " + this.slotno.Split(',')[0]);
                         });
                         int num = (int)frm.ShowDialog();
                     }*/
                }
                else
                    this.SendScanCmd(this.QM);
                if (this.ComSta != ComStatus.END)
                {
                    this.QM.InstrumentsType = CommonFun.GetAppConfig("modelnumber");
                    this.QM.Serials = CommonFun.GetAppConfig("serialno");
                    this.SubScan = false;
                    this.btnScan.Content = "Измерения";
                    CommonFun.InsertLog("Quantitation", "Measure standard sample", false);
                    array = new string[1][];
                    array[0] = new string[QM.QPar.WL.Count()];

                   /* for (int i = 0; i < QM.QPar.WL.Count(); i++)
                    {
                        DataGridTextColumn textColumn = new DataGridTextColumn();
                        textColumn.Header = Convert.ToDecimal(Convert.ToDecimal(wl_mass[i]) / 10M, new CultureInfo("en-US")).ToString("f1");
                        //textColumn.Binding = new System.Windows.Data.Binding(string.Format("WlArray[0][{0}]", i));
                        // textColumn.Binding = new Binding("{Binding list}");
                        meisureGrid.Columns.Add(textColumn);


                    }*/
                }
            }
        }



      
        private void Open_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            tt.Stop();
            Open_File();
            tt.Start();
        }
        bool shifrTrueFalse = false;
        public void Open_File()
        {
         
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Сохраненные измерения");

            OpenFrm openFrm = new OpenFrm(Directory.GetCurrentDirectory() + @"\Сохраненные измерения", ".qua");
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
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(pathTemp + "/" + openFrm.open_name);

                    XDocument xdoc = XDocument.Load(pathTemp + "/" + openFrm.open_name);
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
                                if ("IzmerenieParametr".Equals(d.Name))
                                {
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("MeasureMethodName".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.MeasureMethodName = k.FirstChild.Value;
                                        }

                                        if ("WL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.WL = k.FirstChild.Value;
                                        }

                                        if ("CountMea".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.MCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("CountStandard".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.SamCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("Length".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.Length = k.FirstChild.Value;
                                        }

                                        if ("Equation".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.Equation = k.FirstChild.Value;
                                        }
                                        if ("Fitting".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.Fitting = k.FirstChild.Value;
                                        }

                                        if ("Zero".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.ZeroB = Convert.ToBoolean(k.FirstChild.Value);
                                        }
                                        if ("CabMethod".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.CabMethod = k.FirstChild.Value;
                                        }

                                        if ("CabMethodMD".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.CabMethod = k.FirstChild.Value;
                                        }

                                        if ("Unit".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.Unit = k.FirstChild.Value;
                                            this.lblUnit.Content = this.QM.QPar.Unit;
                                        }

                                        if ("Square".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.MeasureMethod.Square = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                    }
                                }

                                if ("IzmerenieGrad".Equals(d.Name))
                                {
                                    this.QM.SamList = new List<Sample>();
                                    int str = 0;
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if (this.QM.QPar.CabMethod != "Ввод коэффициентов уравнения")
                                        {
                                            if ("Stroka".Equals(k.Name))
                                            {
                                                //List<object> value_xml = new List<object>();
                                                //this.QM.SamList = new List<Sample>();
                                                for (XmlNode l = k.FirstChild; l != null; l = l.NextSibling)
                                                {
                                                    if (this.QM.SamList == null)
                                                        this.QM.SamList = new List<Sample>();
                                                    if (this.QM.SamList.Count<Sample>() > this.QM.QPar.SamCnt)
                                                        this.QM.SamList = new List<Sample>();
                                                    while (this.QM.SamList.Count<Sample>() < this.QM.QPar.SamCnt)
                                                        this.QM.SamList.Add(new Sample()
                                                        {
                                                            IsExclude = false
                                                        });
                                                    if ("ND".Equals(l.Name) && l.FirstChild != null)
                                                    {
                                                        this.QM.SamList[str].ND = Convert.ToDecimal(l.FirstChild.Value, new CultureInfo("en-US"));
                                                    }
                                                    if ("XGD".Equals(l.Name) && l.FirstChild != null)
                                                    {
                                                        this.QM.SamList[str].XGD = Convert.ToDecimal(l.FirstChild.Value, new CultureInfo("en-US"));
                                                    }
                                                    if ("IsExclude".Equals(l.Name) && l.FirstChild != null)
                                                    {
                                                        this.QM.SamList[str].IsExclude = Convert.ToBoolean(l.FirstChild.Value, new CultureInfo("en-US"));
                                                    }
                                                    if ("C_bz".Equals(l.Name) && l.FirstChild != null)
                                                    {
                                                        this.QM.SamList[str].C_bz = l.FirstChild.Value;
                                                    }
                                                    
                                                    if ("D_sj".Equals(l.Name) && l.FirstChild != null)
                                                    {
                                                        this.QM.SamList[str].D_sj = Convert.ToDateTime(l.FirstChild.Value, new CultureInfo("en-US"));
                                                    }


                                                    /*if ("Avalue".Equals(l.Name) && l.FirstChild != null)
                                                    {
                                                        this.QM.SamList[str].Avalue = new decimal[] { Convert.ToDecimal(l.FirstChild.Value.Split(',')) };
                                                    }*/
                                                    if ("Avalue".Equals(l.Name) && l.FirstChild != null)
                                                    {
                                                        for (XmlNode a = l.FirstChild; a != null; a = a.NextSibling)
                                                        {
                                                            if (a.FirstChild != null)
                                                            {
                                                                string[] avalue = new string[a.FirstChild.Value.Split(',').Count()];
                                                                avalue = a.FirstChild.Value.Split(',');
                                                                this.QM.SamList[str].Avalue = new decimal[avalue.Count()];
                                                                for (int i = 0; i < avalue.Count(); i++)
                                                                {
                                                                    this.QM.SamList[str].Avalue[i] = Convert.ToDecimal(avalue[i], new CultureInfo("en-US"));
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //this.QM.SamList.Add(l.FirstChild.Value);
                                                    //this.QM.SamList.Add(Convert.ToDecimal(l.FirstChild.Value));                                                    
                                                }
                                                //this.QM.SamList.Add(new Sample(value_xml));
                                                str++;
                                            }
                                        }
                                        if ("K0".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K0 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("K1".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K1 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("K2".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K2 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("K3".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K3 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("K10".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K10 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("K11".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K11 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("K12".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K12 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("K13".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.K13 = Convert.ToDecimal(k.FirstChild.Value);
                                        }
                                        if ("R".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.R = k.FirstChild.Value;
                                        }
                                        if (this.QM.QPar.Equation == "C=f(Abs)")
                                        {
                                            if ("CFCS".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.CFCS = k.FirstChild.Value;
                                                this.lblfor.Content = this.QM.CFCS;
                                            }

                                        }
                                        else
                                        {
                                            if ("AFCS".Equals(k.Name) && k.FirstChild != null)
                                            {
                                            
                                                this.QM.AFCS = k.FirstChild.Value;
                                                this.lblfor.Content = this.QM.AFCS;
                                            }
                                        }

                                    }
                                }

                                    // Обрабатываем в цикле только Stroka
                                if ("Stroka".Equals(d.Name))
                                {
                                    int stolbec = 0;
                                    //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                    List<object> value_xml = new List<object>();
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("Stolbec".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            value_xml.Add(k.FirstChild.Value);
                                        }
                                    }

                                    MyTableList.Add(new QuaWLTable(value_xml));
                                    meisureGrid.ItemsSource = _itemList;

                                }
                            }
                        }
                    }

                    File.Delete(pathTemp + "/" + openFrm.open_name);
                    LoadMeaData(0);
                }
            }
          
        }


        private void BtnBlank_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            /*if (this.QM.C_methodcreator == null || this.QM.C_methodcreator.Length <= 0)
            {
                CommonFun.showbox(CommonFun.GetLanText(this.lanvalue, "nomethod"), "Error");
            }
            else
            {*/
            string errormsg = "";
            /* if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                 CommonFun.showbox(errormsg, "Error");
             else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                 CommonFun.showbox(errormsg, "Error");
             else*/
            if (!this.sp.IsOpen)
                CommonFun.showbox("opencom", "Warning");
            else if (this.btnBlank.Content.ToString() == "Калибровка")
            {
                if (this.QM.QPar.MeasureMethodName == "Границы")
                {
                    this.btnScan.IsEnabled = false;
                    this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                    CommonFun.WriteSendLine("stop,SCAN_STOPPING 0");
                }
                else
                {
                    this.btnBlank.IsEnabled = false;
                    this.stophappened = true;
                    CommonFun.WriteSendLine("stop,");
                    this.meacnt = 0;
                    this.endcnt = 0;
                }
            }
            else if (this.ComSta != ComStatus.END)
            {
                this.btnBlank.IsEnabled = false;
                this.stophappened = true;
                CommonFun.WriteSendLine("stop,");
                this.meacnt = 0;
            }
            else
            {
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
                            frm.Close(); frm.Dispose();
                            this.ComSta = ComStatus.SETCHP;
                            this.sp.WriteLine("SETCHP " + str + "\r\n");
                            CommonFun.WriteSendLine("SETCHP " + str);
                        });
                        int num = (int)frm.ShowDialog();
                    }*/
                }
                else
                    this.SendBlankCmd(this.QM);
                if (this.ComSta != ComStatus.END)
                {
                    this.SubScan = false;
                    this.endcnt = 0;
                    this.btnBlank.Content = "Обнуление";
                    CommonFun.InsertLog("Quantitation", "Обнуление", false);
                }

            }
        }



        private void CbxAll_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CbxAll_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Name_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           // this.timer1.Stop();
            if (!this.sp.IsOpen)
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");


            /*KeyBoard keyBoard = new KeyBoard("Ввод названия образца", (sender as TextBox).Text);
            keyBoard.ShowDialog();
             (sender as TextBox).Text = keyBoard.text_string;*/
            using (KeyBoard frm2 = new KeyBoard("", (sender as System.Windows.Controls.TextBox).Text))
            {
                // frm2.lbltitle.Text = "reason";
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    (sender as System.Windows.Controls.TextBox).Text = frm2.txtValue.Text;

                    frm2.Close();

                });
                Convert.ToInt32(frm2.ShowDialog());
            }

          //  timer1.Start();
        }
        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".qua";
        private void Save_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if (MyTableList.Count == 0)
            {
                CommonFun.showbox("Измерений не было, сохранять нечего", "Info");
            }
            else
            {
                using (SaveFrm save = new SaveFrm(extension, "Измерение по градуировке")) {

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

            XmlNode IzmerenieParametr = xd.CreateElement("IzmerenieParametr");

            XmlNode MeasureMethodName = xd.CreateElement("MeasureMethodName"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.MeasureMethodName; // и значение
            IzmerenieParametr.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            XmlNode WL = xd.CreateElement("WL"); //длина волны
            WL.InnerText = this.QM.QPar.WL; // и значение
            IzmerenieParametr.AppendChild(WL); // и указываем кому принадлежит 

            XmlNode CountMea = xd.CreateElement("CountMea"); //кол-во измерений
            CountMea.InnerText = this.QM.QPar.MCnt.ToString(); // и значение
            IzmerenieParametr.AppendChild(CountMea); // и указываем кому принадлежит 

            XmlNode CountStandard = xd.CreateElement("CountStandard"); //количество образцов
            CountStandard.InnerText = this.QM.QPar.SamCnt.ToString(); // и значение
            IzmerenieParametr.AppendChild(CountStandard); // и указываем кому принадлежит 

            XmlNode Length = xd.CreateElement("Length"); //длина луча
            Length.InnerText = this.QM.QPar.Length; // и значение
            IzmerenieParametr.AppendChild(Length); // и указываем кому принадлежит  

            XmlNode Equation = xd.CreateElement("Equation"); //уравнение
            Equation.InnerText = this.QM.QPar.Equation; // и значение
            IzmerenieParametr.AppendChild(Equation); // и указываем кому принадлежит            

            XmlNode Fitting = xd.CreateElement("Fitting"); //формула
            Fitting.InnerText = this.QM.QPar.Fitting; // и значение
            IzmerenieParametr.AppendChild(Fitting); // и указываем кому принадлежит       

            XmlNode Zero = xd.CreateElement("Zero"); //нулевая точка
            Zero.InnerText = this.QM.QPar.ZeroB.ToString(); // и значение
            IzmerenieParametr.AppendChild(Zero); // и указываем кому принадлежит    

            XmlNode CabMethod = xd.CreateElement("CabMethod"); //обнуление
            CabMethod.InnerText = this.QM.QPar.CabMethod; // и значение
            IzmerenieParametr.AppendChild(CabMethod); // и указываем кому принадлежит        

            XmlNode CabMethodMD = xd.CreateElement("CabMethodMD"); //обнуление
            CabMethod.InnerText = this.QM.QPar.CabMethod; // и значение
            IzmerenieParametr.AppendChild(CabMethod); // и указываем кому принадлежит      

            XmlNode Unit = xd.CreateElement("Unit"); //ед.измерения
            Unit.InnerText = this.QM.QPar.Unit; // и значение
            IzmerenieParametr.AppendChild(Unit); // и указываем кому принадлежит       

            XmlNode Square = xd.CreateElement("Square"); //ед.измерения
            Square.InnerText = this.QM.QPar.MeasureMethod.Square.ToString(); // и значение
            IzmerenieParametr.AppendChild(Square); // и указываем кому принадлежит   

            xd.DocumentElement.AppendChild(IzmerenieParametr);

            XmlNode IzmerenieGrad = xd.CreateElement("IzmerenieGrad");

            if (this.QM.QPar.CabMethod != "Ввод коэффициентов уравнения")
            {

                // string[] HeaderCells = new string[this.QM.QPar.SamCnt];
                // string[,] Cells1 = new string[this.QM.SamList.Count(), 5];

                for (int i = 0; i < this.QM.QPar.SamCnt; i++)
                {
                    XmlNode Cells2 = xd.CreateElement("Stroka");

                    XmlNode attribute2 = xd.CreateElement("ND");//Концетрация
                    attribute2.InnerText = Convert.ToString(this.QM.SamList[i].ND); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute2); // добавляем атрибут

                    XmlNode attribute3 = xd.CreateElement("XGD"); //Абсорбция
                    attribute3.InnerText = Convert.ToString(this.QM.SamList[i].XGD); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute3); // добавляем атрибут

                    XmlNode attribute4 = xd.CreateElement("IsExclude");
                    attribute4.InnerText = Convert.ToString(this.QM.SamList[i].IsExclude); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute4); // добавляем атрибут

                    XmlNode attribute5 = xd.CreateElement("C_bz");
                    attribute5.InnerText = Convert.ToString(this.QM.SamList[i].C_bz); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute5); // добавляем атрибут

                    XmlNode attribute6 = xd.CreateElement("D_sj");
                    attribute6.InnerText = Convert.ToString(this.QM.SamList[i].D_sj); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute6); // добавляем атрибут

                    XmlNode attribute7 = xd.CreateElement("Avalue");
                    attribute7.InnerText = Convert.ToString(this.QM.SamList[i].Avalue); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute7); // добавляем атрибут



                    IzmerenieGrad.AppendChild(Cells2);
                }



            }

            XmlNode K0 = xd.CreateElement("K0"); //длина луча
            K0.InnerText = this.QM.K0.ToString(); // и значение
            IzmerenieGrad.AppendChild(K0); // и указываем кому принадлежит  

            XmlNode K1 = xd.CreateElement("K1"); //длина луча
            K1.InnerText = this.QM.K1.ToString(); // и значение
            IzmerenieGrad.AppendChild(K1); // и указываем кому принадлежит  

            XmlNode K2 = xd.CreateElement("K2"); //длина луча
            K2.InnerText = this.QM.K1.ToString(); // и значение
            IzmerenieGrad.AppendChild(K2); // и указываем кому принадлежит  

            XmlNode K3 = xd.CreateElement("K3"); //длина луча
            K3.InnerText = this.QM.K3.ToString(); // и значение
            IzmerenieGrad.AppendChild(K3); // и указываем кому принадлежит  

            XmlNode K10 = xd.CreateElement("K10"); //длина луча
            K10.InnerText = this.QM.K10.ToString(); // и значение
            IzmerenieGrad.AppendChild(K10); // и указываем кому принадлежит  

            XmlNode K12 = xd.CreateElement("K12"); //длина луча
            K12.InnerText = this.QM.K12.ToString(); // и значение
            IzmerenieGrad.AppendChild(K12); // и указываем кому принадлежит  

            XmlNode K11 = xd.CreateElement("K11"); //длина луча
            K11.InnerText = this.QM.K11.ToString(); // и значение
            IzmerenieGrad.AppendChild(K11); // и указываем кому принадлежит  

            XmlNode K13 = xd.CreateElement("K13"); //длина луча
            K13.InnerText = this.QM.K13.ToString(); // и значение
            IzmerenieGrad.AppendChild(K13); // и указываем кому принадлежит        

            XmlNode R = xd.CreateElement("R"); //
            if (this.QM.QPar.R != null)
            {
                R.InnerText = this.QM.QPar.R.ToString(); // и значение
            }
            else
            {
                R.InnerText = ""; // и значение
            }
            IzmerenieGrad.AppendChild(R); // и указываем кому принадлежит  


            if (this.QM.QPar.Equation == "C=f(Abs)")
            {
                XmlNode CFCS = xd.CreateElement("CFCS"); //
                CFCS.InnerText = this.QM.CFCS.ToString(); // и значение
                IzmerenieGrad.AppendChild(CFCS); // и указываем кому принадлежит  
            }
            else
            {
                XmlNode AFCS = xd.CreateElement("AFCS"); //
                AFCS.InnerText = this.QM.AFCS.ToString(); // и значение
                IzmerenieGrad.AppendChild(AFCS); // и указываем кому принадлежит  
            }

            xd.DocumentElement.AppendChild(IzmerenieGrad);

            XmlNode Izmerenie = xd.CreateElement("Izmerenie");

            XmlNode DateTime1 = xd.CreateElement("DateTime"); // дата создания градуировки
            DateTime1.InnerText = DateTime.Now.ToString(); // и значение
            Izmerenie.AppendChild(DateTime1); // и указываем кому принадлежит

            int countIzmer = meisureGrid.Items.Count - 1;
            XmlNode countIzmer1 = xd.CreateElement("countIzmer1");
            countIzmer1.InnerText = Convert.ToString(countIzmer);
            Izmerenie.AppendChild(countIzmer1);
            xd.DocumentElement.AppendChild(Izmerenie);

            string[] HeaderCells = new string[meisureGrid.Columns.Count - 1];
            string[,] Cells1 = new string[meisureGrid.Items.Count, 6];


            for (int i = 0; i < meisureGrid.Items.Count; i++)
            {
                XmlNode Cells2 = xd.CreateElement("Stroka");

                XmlAttribute attribute1 = xd.CreateAttribute("Nomer");
                attribute1.Value = Convert.ToString(i); // устанавливаем значение атрибута
                Cells2.Attributes.Append(attribute1); // добавляем атрибут

                for (int j = 0; j < meisureGrid.Columns.Count - 1; j++)
                {

                    try
                    {
                        TextBlock x = meisureGrid.Columns[j].GetCellContent(meisureGrid.Items[i]) as TextBlock;
                        if(x != null)
                            Cells1[i, j] = x.Text;
                        else
                            if (MyTableList.ElementAt(i).Name != null)
                                Cells1[i, j] = MyTableList.ElementAt(i).Name;
                            else
                            Cells1[i, j] = "";
                    }
                    catch
                    {
                        Cells1[i, j] = MyTableList.ElementAt(i).Name;
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
            using (QuantationMethod frm = new QuantationMethod(QM, wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method))
            {
                frm.btnNew.PreviewMouseDown += ((param0_1, param1_1) =>
                {
                    using (QuantitationNewMethod frm1 = new QuantitationNewMethod(QM, wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method, true))
                    {
                        QuaMethod nqm = new QuaMethod();
                        nqm.QPar = new QuaParmas();
                        nqm.Page = 1;
                        frm1.Loaded += (RoutedEventHandler)((param0_2, param1_2) =>
                        {

                        });


                        frm1.Next.PreviewMouseDown += ((param0_2, param1_2) =>
                        {
                            try
                            {
                                /*  if (nqm.QPar.MeasureMethodName != null && nqm.QPar.MeasureMethodName.Length > 0 && nqm.QPar.MeasureMethodName != CommonFun.GetByName(frm1.measure_method.Trim().Replace(" >", "")))
                                       nqm.SamList = new List<Sample>();
                                   if (nqm.QPar.CabMethod != null && nqm.QPar.CabMethod.Length > 0 && nqm.QPar.CabMethod != frm1.lblCabMethodV.Content)
                                      nqm.SamList = new List<Sample>();*/
                                nqm.SamList = new List<Sample>();
                                nqm.QPar.CabMethod = frm1.lblCabMethodV.Content.ToString().Trim().Replace(" >", "");
                                nqm.QPar.CabMethodDM = nqm.QPar.CabMethod;
                                nqm.QPar.MeasureMethodName = frm1.measure_method.Trim().Replace(" >", "");
                                nqm.QPar.MeasureMethod = CommonFun.GetByName(this.QM.QPar.MeasureMethodName);
                                nqm.QPar.MeasureMethodNameDM = this.QM.QPar.MeasureMethodName;
                                int? square = nqm.QPar.MeasureMethod.Square;
                                int? nullable;
                                for(int i = 0; i < frm1.wl_mass.Count(); i++)
                                {
                                    frm1.wl_mass[i] = frm1.wl_mass[i].Replace(",", ".");
                                }
                                nqm.QPar.WL = string.Join(",", frm1.wl_mass);
                                /*switch (nqm.QPar.MeasureMethodName)
                                {
                                    case ("Одноволновое"):
                                        nqm.QPar.WL = smfrm.wl_1.Content;
                                        break;
                                    case ("Двухволновое"):
                                        wl_1.Content = wl_mass[0].Replace('.', ',');
                                        wl_2.Content = wl_mass[1].Replace('.', ',');
                                        this.step_interval = "0";
                                        Lambda1.IsEnabled = true;
                                        Lambda2.IsEnabled = true;
                                        Lambda3.IsEnabled = false;
                                        break;
                                    case ("Трехволновое"):
                                        wl_1.Content = wl_mass[0].Replace('.', ',');
                                        wl_2.Content = wl_mass[1].Replace('.', ',');
                                        wl_3.Content = wl_mass[2];
                                        this.step_interval = "0";
                                        Lambda1.IsEnabled = true;
                                        Lambda2.IsEnabled = true;
                                        Lambda3.IsEnabled = true;
                                        break;
                                    case ("Границы"):
                                        wl_1.Content = wl_mass[0].Replace('.', ',');
                                        wl_2.Content = wl_mass[1].Replace('.', ',');
                                        this.step_interval = step_interval;
                                        Lambda1.IsEnabled = true;
                                        Lambda2.IsEnabled = true;
                                        Lambda3.IsEnabled = true;
                                        break;
                                }*/
                                nullable = nqm.QPar.MeasureMethod.WLCnt;

                                nqm.QPar.EConvert = false;
                                nqm.QPar.Cuvettemath = false;
                                nqm.QPar.Equation = frm1.curveEquation.Trim().Replace(" >", "");
                                nqm.QPar.Fitting = frm1.fittingMethod.Trim().Replace(" >", "");
                                nqm.QPar.FittingDM = frm1.fittingMethod.Trim().Replace(" >", "");
                                nqm.QPar.Length = frm1.optical_path.Trim().Replace(" >", "");
                                nqm.QPar.Limits = "";
                                nqm.QPar.MCnt = (int)Convert.ToInt16(frm1.count_measure.Trim().Replace(" >", ""));
                                nqm.QPar.SamCnt = (int)Convert.ToInt16(frm1.count_standard_sample.Trim().Replace(" >", ""));

                               // nqm.QPar.SamCnt = frm1.wl_mass.Count();
                                nqm.QPar.Unit = frm1.measureunit.Trim().Replace(">", "");
                                nqm.QPar.ZeroB = frm1.zero_intercept;
                                if (this.QM.QPar.Equation == "C=f(Abs)")
                                    this.lblfor.Content = this.QM.CFCS;
                                else
                                    this.lblfor.Content = this.QM.AFCS; 

                            }
                            catch (Exception ex)
                            {
                                CommonFun.showbox("inputerror", "Error");
                                return;
                            }
                            using (this.smfrm = new QuantitationNewMethodNext(nqm))
                            {
                                this.smfrm.Loaded += (RoutedEventHandler)((param0_3, param1_3) =>
                                {
                                    this.smfrm.QM = nqm;
                                    this.smfrm.GenerateWindow(nqm);

                                    /* if (nqm.QPar.CabMethod == "inputr")
                                     {

                                         this.smfrm.dataGridView1.Visible = false;
                                         this.smfrm.panel3.Visible = true;
                                         this.smfrm.btnMeasure.IsEnabled = false;
                                         this.smfrm.btnXL.IsEnabled = false;
                                         //this.smfrm.btnjcc.Enabled = false;
                                         if (nqm.QPar.ZeroB)
                                         {
                                             this.smfrm.lblK0.Content = "0";
                                             this.smfrm.K0Grid.IsEnabled = false;
                                             this.smfrm.lblK0.IsEnabled = false;
                                         }
                                         if (nqm.QPar.Fitting == "Линейная")
                                         {
                                             this.smfrm.lblK1.IsEnabled = true;
                                             this.smfrm.lblK2.IsEnabled = false;
                                             this.smfrm.lblK3.IsEnabled = false;

                                             this.smfrm.K1Grid.IsEnabled = true;
                                             this.smfrm.K2Grid.IsEnabled = false;
                                             this.smfrm.K3Grid.IsEnabled = false;
                                             if (nqm.QPar.Equation == "C=f(Abs)")
                                                 this.smfrm.lblEquationCurve.Content = "C=K1*A+K0";
                                             else
                                                 this.smfrm.lblEquationCurve.Content = "A=K1*C+K0";
                                         }
                                         else if (nqm.QPar.Fitting == "Квадратичная")
                                         {
                                             this.smfrm.lblK1.IsEnabled = true;
                                             this.smfrm.lblK2.IsEnabled = true;
                                             this.smfrm.lblK3.IsEnabled = false;

                                             this.smfrm.K1Grid.IsEnabled = true;
                                             this.smfrm.K2Grid.IsEnabled = true;
                                             this.smfrm.K3Grid.IsEnabled = false;
                                             if (nqm.QPar.Equation == "C=f(Abs)")
                                                 this.smfrm.lblEquationCurve.Content = "C=K2*A^2+K1*A+K0";
                                             else
                                                 this.smfrm.lblEquationCurve.Content = "A=K2*C^2+K1*C+K0";
                                         }
                                         else if (nqm.QPar.Fitting == "Кубическая")
                                         {
                                             this.smfrm.lblK1.IsEnabled = true;
                                             this.smfrm.lblK2.IsEnabled = true;
                                             this.smfrm.lblK3.IsEnabled = false;

                                             this.smfrm.K1Grid.IsEnabled = true;
                                             this.smfrm.K2Grid.IsEnabled = true;
                                             this.smfrm.K3Grid.IsEnabled = true;
                                             if (nqm.QPar.Equation == "C=f(Abs)")
                                                 this.smfrm.lblEquationCurve.Content = "C=K3*A^3+K2*A^2+K1*A+K0";
                                             else
                                                 this.smfrm.lblEquationCurve.Content = "A=K3*C^3+K2*C^2+K1*C+K0";
                                         }
                                     }
                                     else if (nqm.QPar.CabMethod == "measrues")
                                     {

                                         this.smfrm.dataGridView1.Visible = true;
                                         this.smfrm.panel3.Visible = false;
                                         this.smfrm.lblequation.Visible = false;

                                         this.smfrm.dataGridView1.Columns[1].ReadOnly = true;
                                         this.smfrm.dataGridView1.Columns[2].ReadOnly = true;
                                         this.smfrm.GenerateNewSampleData();
                                     }
                                     else if (nqm.QPar.CabMethod == "inputs")
                                     {

                                         this.smfrm.dataGridView1.Visible = true;
                                         this.smfrm.panel3.Visible = false;
                                         this.smfrm.btnMeasure.Enabled = false;
                                         this.smfrm.btnXL.Enabled = false;
                                       //  this.smfrm.btnjcc.Enabled = false;
                                         this.smfrm.GenerateNewSampleData();
                                         this.smfrm.dataGridView1.Columns[1].ReadOnly = true;
                                     }*/


                                    this.smfrm.DrawCurve(nqm);



                                    //  this.smfrm.Window_Loaded(param0_3, param1_3);
                                });
                                this.smfrm.btnBack.PreviewMouseDown += ((param0_3, param1_3) => nqm = this.smfrm.QM);
                                this.smfrm.btnLast.PreviewMouseDown += ((param0_3, param1_3) => nqm = this.smfrm.QM);
                                this.smfrm.btnOK.PreviewMouseDown += ((param0_3, param1_3) =>
                                {
                                    if (this.smfrm.QM.QPar.CabMethod != "inputr")
                                    {
                                        foreach (Sample sam in this.smfrm.QM.SamList)
                                        {
                                            System.Decimal? nullable = sam.ND;
                                            int num;
                                            if (nullable.HasValue)
                                            {
                                                nullable = sam.XGD;
                                                num = nullable.HasValue ? 1 : 0;
                                            }
                                            else
                                                num = 0;
                                            if (num == 0)
                                            {
                                                CommonFun.showbox("Incompletedata", "Error");
                                                return;
                                            }
                                        }
                                    }
                                    if (this.smfrm.QM.QPar.Equation == "Abs=f(C)" && this.smfrm.QM.K0 == 0M && (this.smfrm.QM.K1 == 0M && this.smfrm.QM.K2 == 0M) && this.smfrm.QM.K3 == 0M)
                                        CommonFun.showbox("errorformula", "Error");
                                    else if (this.smfrm.QM.QPar.Equation == "C=f(Abs)" && this.smfrm.QM.K10 == 0M && (this.smfrm.QM.K11 == 0M && this.smfrm.QM.K12 == 0M) && this.smfrm.QM.K13 == 0M)
                                    {
                                        CommonFun.showbox("errorformula", "Error");
                                    }
                                    else
                                    {
                                        frm.QPar = this.smfrm.QM;
                                        frm.ShowQm();
                                        // frm.DrawCurve();
                                        frm.btnOK.IsEnabled = true;
                                        frm.Save.IsEnabled = true;
                                        this.smfrm.Close(); frm.Dispose();
                                        frm1.Close(); frm1.Dispose();
                                        CommonFun.InsertLog("Quantitation", "Update method", false);
                                    }
                                });
                                this.smfrm.btnMeasure.PreviewMouseDown += ((param0_3, param1_3) =>
                                {
                                    string errormsg = "";
                                    /*if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                                        CommonFun.showbox(errormsg, "Error");
                                    else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                                        CommonFun.showbox(errormsg, "Error");
                                    else*/
                                    if (!this.sp.IsOpen)
                                    {
                                        CommonFun.showbox("opencom", "Warning");
                                    }
                                    else
                                    {
                                        if (this.smfrm.btnMeasure.Content.ToString() == "Измеряем")
                                            return;
                                        if (this.smfrm.dataGridView1.Rows[this.smfrm.dataGridView1.SelectedCells[0].RowIndex].Tag == null)
                                        {
                                            CommonFun.showbox("movecursor", "Error");
                                        }
                                        else
                                        {
                                            this.SubScan = true;
                                            this.calormea = 2;
                                            if (CommonFun.GetAppConfig("EightSlot") == "true")
                                            {
                                                this.slotno = "";
                                                this.subQM = nqm;
                                                /*using (ChoseSlotFrm frm2 = new ChoseSlotFrm())
                                                {
                                                    frm2.btnOK.Click += (EventHandler)((param0_4, param1_4) =>
                                                    {
                                                        if (frm2.chk1.Checked)
                                                            this.slotno += "1,";
                                                        if (frm2.chk2.Checked)
                                                            this.slotno += "2,";
                                                        if (frm2.chk3.Checked)
                                                            this.slotno += "3,";
                                                        if (frm2.chk4.Checked)
                                                            this.slotno += "4,";
                                                        if (frm2.chk5.Checked)
                                                            this.slotno += "5,";
                                                        if (frm2.chk6.Checked)
                                                            this.slotno += "6,";
                                                        if (frm2.chk7.Checked)
                                                            this.slotno += "7,";
                                                        if (frm2.chk8.Checked)
                                                            this.slotno += "8,";
                                                        if (this.slotno.Length <= 0)
                                                            return;
                                                        frm2.Close();
                                                        this.slotno = this.slotno.Substring(0, this.slotno.Length - 1);
                                                        // ISSUE: reference to a compiler-generated field
                                                        // ISSUE: reference to a compiler-generated field
                                                        if (((IEnumerable<string>)this.slotno.Split(',')).Count<string>() > this.CS\u0024\u003C\u003E8__locals108.nqm.SamList.Count<Sample>())
                            {
                                                            this.slotno = "";
                                                            CommonFun.showbox(CommonFun.GetLanText(this.lanvalue, "dismatch"), "Warning");
                                                        }
                            else
                                                        {
                                                            this.ComSta = ComStatus.SETCHP;
                                                            this.sp.WriteLine("SETCHP " + this.slotno.Split(',')[0] + "\r\n");
                                                            CommonFun.WriteSendLine("SETCHP " + this.slotno.Split(',')[0]);
                                                        }
                                                    });
                                                    int num = (int)frm2.ShowDialog();
                                                }*/
                                            }
                                            else
                                                this.SendScanCmd(nqm);
                                            if (this.ComSta != ComStatus.END)
                                            {
                                                this.smfrm.btnMeasure.Content = "Измеряем";
                                                CommonFun.InsertLog("Quantitation", "Measure standard sample", false);
                                            }
                                        }
                                    }
                                });

                                this.smfrm.btnXL.PreviewMouseDown += ((param0_3, param1_3) =>
                                {
                                    string errormsg = "";
                                    /*if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                                        CommonFun.showbox(errormsg, "Error");
                                    else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                                        CommonFun.showbox(errormsg, "Error");
                                    else*/
                                    if (!this.sp.IsOpen)
                                        CommonFun.showbox("opencom", "Warning");
                                    else if (this.smfrm.btnXL.Content.ToString() == "Калибровка")
                                    {
                                        if (this.smfrm.QM.QPar.MeasureMethodName == "Границы")
                                        {
                                            this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                                            CommonFun.WriteSendLine("stop,SCAN_STOPPING 0");
                                        }
                                        else
                                        {
                                            this.smfrm.btnXL.IsEnabled = false;
                                            this.stophappened = true;
                                            CommonFun.WriteSendLine("stop,");
                                            this.meacnt = 0;
                                            this.endcnt = 0;
                                        }
                                    }
                                    else if (this.ComSta != ComStatus.END)
                                    {
                                        this.smfrm.btnXL.IsEnabled = false;
                                        this.stophappened = true;
                                        CommonFun.WriteSendLine("stop,");
                                        this.meacnt = 0;
                                        this.endcnt = 0;
                                    }
                                    else
                                    {
                                        this.SubScan = true;
                                        this.calormea = 1;
                                        if (CommonFun.GetAppConfig("EightSlot") == "true")
                                        {
                                            this.subQM = nqm;
                                            /* using (SelectOneSlotFrm frm2 = new SelectOneSlotFrm())
                                             {
                                                 frm2.grblgp.Visible = false;
                                                 frm2.grbxf.Visible = false;
                                                 frm2.grbslot.Visible = true;
                                                 frm2.btnOK.Click += (EventHandler)((param0_4, param1_4) =>
                                                 {
                                                     string str = "";
                                                     if (frm2.rab1.Checked)
                                                         str = "1";
                                                     else if (frm2.rab2.Checked)
                                                         str = "2";
                                                     else if (frm2.rab3.Checked)
                                                         str = "3";
                                                     else if (frm2.rab4.Checked)
                                                         str = "4";
                                                     else if (frm2.rab5.Checked)
                                                         str = "5";
                                                     else if (frm2.rab6.Checked)
                                                         str = "6";
                                                     else if (frm2.rab7.Checked)
                                                         str = "7";
                                                     else if (frm2.rab8.Checked)
                                                         str = "8";
                                                     frm2.Close();
                                                     this.ComSta = ComStatus.SETCHP;
                                                     this.sp.WriteLine("SETCHP " + str + "\r\n");
                                                     CommonFun.WriteSendLine("SETCHP " + str);
                                                 });
                                                 int num = (int)frm2.ShowDialog();
                                             }*/
                                        }
                                        else
                                            this.SendBlankCmd(nqm);
                                        if (this.ComSta != ComStatus.END)
                                        {
                                            this.smfrm.btnXL.Content = "Калибровка";
                                            CommonFun.InsertLog("Quantitation", "Measure standard sample" + "-" + "Blank", false);
                                        }
                                    }
                                });
                                /*this.smfrm.btnjcc.PreviewMouseDown += (RoutedEventHandler)((param0_3, param1_3) =>
                                {
                                    string msg = "";

                                    this.subQM = nqm;

                                    if (this.smfrm.QM.QPar.Cuvettemath)
                                    {

                                    }

                                });*/
                                /*this.smfrm.btnCancel.PreviewMouseDown += ((param0_3, param1_3) =>
                                {
                                    this.stophappened = true;
                                    if (this.smfrm.QM.QPar.MeasureMethodName == "Границы")
                                    {
                                        this.sp.WriteLine("SCAN_STOPPING 0\r\n");
                                        CommonFun.WriteSendLine("stop,SCAN_STOPPING 0");
                                    }
                                    else
                                    {
                                        CommonFun.WriteSendLine("stop,");
                                        this.meacnt = 0;
                                        this.endcnt = 0;
                                        this.mcnt = 0;
                                        this.slotno = "";
                                        //this.smfrm.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "instoping");
                                        this.smfrm.progressBar1.Value = 0;
                                        //       this.smfrm.panel4.Visible = false;
                                    }
                                    if (this.calormea == 1)
                                        this.smfrm.btnXL.Content = "Остановить";
                                    else if (this.calormea == 2)
                                    {
                                        this.smfrm.btnMeasure.Content = "Остановить";
                                    }
                                    else
                                    {
                                        if (this.calormea != 3)
                                            return;
                                        //  this.smfrm.btnjcc.Text = CommonFun.GetLanText(this.lanvalue, "instoping");
                                    }
                                });*/
                                int num1 = Convert.ToInt32(this.smfrm.ShowDialog());
                            }
                        });
                        int num2 = Convert.ToInt32(frm1.ShowDialog());
                    }
                });
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                    {
                        DateTime? dTime;
                        int num;
                        if (this.QM.MeasreList != null && this.QM.MeasreList.Count > 0)
                        {
                            dTime = this.QM.D_time;
                            num = dTime.HasValue ? 1 : 0;
                        }
                        else
                            num = 1;
                        if (num == 0)
                        {
                            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                            {
                                CommonFun.showbox("fileunsave", "Warning");
                                return;
                            }
                            dTime = this.QM.D_time;
                            if (!dTime.HasValue && this.QM.MeasreList != null && this.QM.MeasreList.Count != 0)
                            {
                                // if (new DRMessageBoxFrm("unsavedataexit", "Warning").ShowDialog() == DialogResult.No)
                                //     return;
                                if (frm.QPar.C_methodcreator == null || frm.QPar.C_methodcreator.Length <= 0)
                                {
                                    if (CommonFun.GetAppConfig("GLPEnabled") == "true" && frm.QPar.MethodCreatorES == null)
                                    {
                                        CommonFun.showbox("savemethodwithes", "Error");
                                        return;
                                    }
                                    frm.QPar.C_methodcreator = CommonFun.GetAppConfig("currentuser");
                                    frm.QPar.D_MTime = new DateTime?(DateTime.Now);
                                }
                                else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && (frm.QPar.MethodCreatorES == null || frm.QPar.MethodCreatorES.Length <= 0))
                                {
                                    CommonFun.showbox("noesmethod", "Error");
                                    return;
                                }
                                this.QM = frm.QPar;
                                CommonFun.InsertLog("Quantitation", "Update method", false);
                                this.QM.MeasreList = new List<Sample>();
                                if (this.QM.QPar.Equation == "Abs=f(C)")
                                    this.lblfor.Content = this.QM.AFCS;
                                else
                                    this.lblfor.Content = this.QM.CFCS;
                                this.lblfor.Content = this.lblfor.Content + ";" + this.QM.QPar.MeasureMethod.C_gs;
                                this.lblUnit.Content = this.QM.QPar.Unit;
                                //  this.btnJCC.Enabled = this.QM.QPar.Cuvettemath;
                                this.Cleardata();
                            }
                        }
                        else
                        {
                            if (frm.QPar.C_methodcreator == null || frm.QPar.C_methodcreator.Length <= 0)
                            {
                                if (CommonFun.GetAppConfig("GLPEnabled") == "true" && frm.QPar.MethodCreatorES == null)
                                {
                                    CommonFun.showbox("savemethodwithes", "Error");
                                    return;
                                }
                                frm.QPar.C_methodcreator = CommonFun.GetAppConfig("currentuser");
                                frm.QPar.D_MTime = new DateTime?(DateTime.Now);
                            }
                            else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && (frm.QPar.MethodCreatorES == null || frm.QPar.MethodCreatorES.Length <= 0))
                            {
                                CommonFun.showbox("noesmethod", "Error");
                                return;
                            }
                            this.QM = frm.QPar;
                            CommonFun.InsertLog("Quantitation", "Update method", false);
                            this.QM.MeasreList = new List<Sample>();
                            if (this.QM.QPar.Equation == "Abs=f(C)")
                                this.lblfor.Content = this.QM.AFCS;
                            else
                                this.lblfor.Content = this.QM.CFCS;
                            this.lblfor.Content = this.lblfor.Content + ";" + this.QM.QPar.MeasureMethod.C_gs;
                           this.lblUnit.Content = this.QM.QPar.Unit;
                            // this.btnJCC.Enabled = this.QM.QPar.Cuvettemath;
                            this.Cleardata();
                        }
                        frm.Close(); frm.Dispose();
                    });



               



                frm.btnBack.PreviewMouseDown += ((param0, param1) => frm.Close());
                int num3 = Convert.ToInt32(frm.ShowDialog());
            }
        }

        private void SetMealable(bool subscan)
        {
            if (subscan)
            {
                this.smfrm.btnMeasure.Content = "Измерение";
                /*this.smfrm.btnjcc.Text = "measureslot";
                if (this.smfrm.panel4.Visible)
                {*/
                this.smfrm.progressBar1.Value = 100;
                // this.smfrm.panel4.Visible = false;
                this.smfrm.progressBar1.Value = 0;
                // }
                this.setState(ComStatus.END);
            }
            else
            {
                this.btnScan.Content = "Измерение";
                // this.btnJCC.Text = CommonFun.GetLanText(this.lanvalue, "measureslot");
                this.setState(ComStatus.END);
                /*if (this.panel4.Visible)
                {*/
                this.progressBar1.Value = 000;
                //  this.panel4.Visible = false;
                this.progressBar1.Value = 0;
                // }
            }
        }

        private void LoadStandardData(int index)
        {
            int num1 = 0;
            if (this.smfrm.dataGridView1.SelectedCells != null && this.smfrm.dataGridView1.SelectedCells.Count > 0)
                num1 = this.smfrm.dataGridView1.SelectedCells[0].RowIndex;
            /*this.smfrm.dataGridView1.Rows.Clear();
            for (int index1 = 0; index1 < this.smfrm.QM.SamList.Count; ++index1)
            {
                this.smfrm.dataGridView1.Rows.Add();
                if (this.smfrm.QM.SamList[index1].IsExclude)
                {
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Value = Properties.Resources.UI_DB_Check_Checked;
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Tag = "on";
                }
                else
                {
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Value = Properties.Resources.UI_DB_Check_Unchecked;
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Tag = "off";
                }
                this.smfrm.dataGridView1.Rows[index1].Cells["ColNO"].Value = (object)(index1 + 1).ToString();
                System.Decimal? nullable = this.smfrm.QM.SamList[index1].XGD;
                System.Decimal num2;
                if (nullable.HasValue)
                {
                    DataGridViewCell cell = this.smfrm.dataGridView1.Rows[index1].Cells["ColXGD"];
                    nullable = this.smfrm.QM.SamList[index1].XGD;
                    num2 = nullable.Value;
                    string str = num2.ToString(this.absacc);
                    cell.Value = (object)str;
                }
                nullable = this.smfrm.QM.SamList[index1].ND;
                if (nullable.HasValue)
                {
                    DataGridViewCell cell = this.smfrm.dataGridView1.Rows[index1].Cells["ColND"];
                    nullable = this.smfrm.QM.SamList[index1].ND;
                    num2 = nullable.Value;
                    string str = num2.ToString(this.conacc);
                    cell.Value = (object)str;
                }
                this.smfrm.dataGridView1.Rows[index1].Cells["ColBZ"].Value = (object)this.smfrm.QM.SamList[index1].C_bz;
                this.smfrm.dataGridView1.Rows[index1].Tag = (object)this.smfrm.QM.SamList[index1];
            }
            if (this.smfrm.QM.SamList.Count < this.smfrm.dgvcnt)
                this.smfrm.dataGridView1.Rows.Add(this.smfrm.dgvcnt - this.smfrm.QM.SamList.Count);
            if (num1 + 1 > this.smfrm.dataGridView1.Rows.Count)
                return;
            this.smfrm.dataGridView1.Rows[num1 + 1].Selected = true;*/
            if (this.smfrm.QM.SamList[num1].IsExclude)
            {
                this.smfrm.dataGridView1.Rows[num1].Cells["ColPC"].Value = Properties.Resources.UI_DB_Check_Checked;
                this.smfrm.dataGridView1.Rows[num1].Cells["ColPC"].Tag = "on";
            }
            else
            {
                this.smfrm.dataGridView1.Rows[num1].Cells["ColPC"].Value = Properties.Resources.UI_DB_Check_Unchecked;
                this.smfrm.dataGridView1.Rows[num1].Cells["ColPC"].Tag = "off";
            }
            this.smfrm.dataGridView1.Rows[num1].Cells["ColNO"].Value = (object)(num1 + 1).ToString();
            System.Decimal? nullable = this.smfrm.QM.SamList[num1].XGD;
            System.Decimal num2;
            if (nullable.HasValue)
            {
                DataGridViewCell cell = this.smfrm.dataGridView1.Rows[num1].Cells["ColXGD"];
                nullable = this.smfrm.QM.SamList[num1].XGD;
                num2 = nullable.Value;
                string str = num2.ToString(this.absacc);
                cell.Value = (object)str;
            }
            nullable = this.smfrm.QM.SamList[num1].ND;
            if (nullable.HasValue)
            {
                DataGridViewCell cell = this.smfrm.dataGridView1.Rows[num1].Cells["ColND"];
                nullable = this.smfrm.QM.SamList[num1].ND;
                num2 = nullable.Value;
                string str = num2.ToString(this.conacc);
                cell.Value = (object)str;
            }
            this.smfrm.dataGridView1.Rows[num1].Cells["ColBZ"].Value = (object)this.smfrm.QM.SamList[num1].C_bz;
            this.smfrm.dataGridView1.Rows[num1].Tag = (object)this.smfrm.QM.SamList[num1];
        
            //if (this.smfrm.QM.SamList.Count< this.smfrm.dgvcnt)
             //   this.smfrm.dataGridView1.Rows.Add(this.smfrm.dgvcnt - this.smfrm.QM.SamList.Count);
            if (num1 + 1 == this.smfrm.dataGridView1.Rows.Count)
                return;
            this.smfrm.dataGridView1.Rows[num1 + 1].Selected = true;
        }
        private void btnBack_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

           // this.lblValue.Visibility = Visibility.Visible;
            this.lblUnit.Visibility = Visibility.Visible;


            this.btnScan.IsEnabled = true;


            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                // if (new DRMessageBoxFrm("exitconfirm", "Warning").ShowDialog() != DialogResult.Yes)
                //     return;
                if (!this.QM.D_time.HasValue && this.QM.MeasreList != null && this.QM.MeasreList.Count != 0)
                {
                    CommonFun.showbox("datasaveexit", "Warning");
                    return;
                }
            }
            /* else if (!this.QM.D_time.HasValue && (this.QM.MeasreList != null && this.QM.MeasreList.Count != 0 && new DRMessageBoxFrm("unsavedataexit", "Warning").ShowDialog() == DialogResult.No))
                 return;*/
            try
            {
                if (this.QM.QPar.MeasureMethod.C_DM != "")
                    CommonFun.setXmlValue("QuaParams", "MeaMethod", this.QM.QPar.MeasureMethod.C_DM);
                else
                    CommonFun.setXmlValue("QuaParams", "MeaMethod", this.QM.QPar.MeasureMethodName);
                CommonFun.setXmlValue("QuaParams", "CapMethod", this.QM.QPar.CabMethodDM);
                CommonFun.setXmlValue("QuaParams", "EConvert", this.QM.QPar.EConvert.ToString());
                CommonFun.setXmlValue("QuaParams", "Equation", this.QM.QPar.Equation);
                bool flag = this.QM.QPar.Cuvettemath;
                CommonFun.setXmlValue("QuaParams", "Bsmjz", flag.ToString());
                CommonFun.setXmlValue("QuaParams", "Fitting", this.QM.QPar.FittingDM);
                CommonFun.setXmlValue("QuaParams", "Length", this.QM.QPar.Length.ToString());
                CommonFun.setXmlValue("QuaParams", "Threshold", this.QM.QPar.Limits);
                int num1 = this.QM.QPar.MCnt;
                CommonFun.setXmlValue("QuaParams", "MCnt", num1.ToString());
                num1 = this.QM.QPar.SamCnt;
                CommonFun.setXmlValue("QuaParams", "SamCnt", num1.ToString());
                CommonFun.setXmlValue("QuaParams", "Unit", this.QM.QPar.Unit);
                CommonFun.setXmlValue("QuaParams", "WL", this.QM.QPar.WL);
                flag = this.QM.QPar.ZeroB;
                CommonFun.setXmlValue("QuaParams", "ZeroB", flag.ToString());
                System.Decimal num2 = this.QM.K0;
                CommonFun.setXmlValue("QuaParams", "K0", num2.ToString());
                num2 = this.QM.K1;
                CommonFun.setXmlValue("QuaParams", "K1", num2.ToString());
                num2 = this.QM.K2;
                CommonFun.setXmlValue("QuaParams", "K2", num2.ToString());
                num2 = this.QM.K3;
                CommonFun.setXmlValue("QuaParams", "K3", num2.ToString());
                CommonFun.setXmlValue("QuaParams", "AFCS", this.QM.AFCS);
                num2 = this.QM.K10;
                CommonFun.setXmlValue("QuaParams", "K10", num2.ToString());
                num2 = this.QM.K11;
                CommonFun.setXmlValue("QuaParams", "K11", num2.ToString());
                num2 = this.QM.K12;
                CommonFun.setXmlValue("QuaParams", "K12", num2.ToString());
                num2 = this.QM.K13;
                CommonFun.setXmlValue("QuaParams", "K13", num2.ToString());
                CommonFun.setXmlValue("QuaParams", "CFCS", this.QM.CFCS);
                CommonFun.setXmlValue("QuaParams", "R", this.QM.R.ToString());
                if (this.QM.SamList != null && this.QM.SamList.Count > 0)
                {
                    string xmlValue = "";
                    foreach (Sample sam in this.QM.SamList)
                        xmlValue = xmlValue + (object)sam.ND + "," + (object)sam.XGD + ";";
                    CommonFun.setXmlValue("QuaParams", "Sample", xmlValue);
                }
                else
                    CommonFun.setXmlValue("QuaParams", "Sample", "");
                CommonFun.setXmlValue("QuaPrintParams", "Addtional", this.printpar.Addtional);
                CommonFun.setXmlValue("QuaPrintParams", "ComImage", this.printpar.ComImage);
                CommonFun.setXmlValue("QuaPrintParams", "Describtion", this.printpar.Describtion);
                flag = this.printpar.ShowAddtional;
                CommonFun.setXmlValue("QuaPrintParams", "ShowAddtional", flag.ToString());
                flag = this.printpar.ShowComImage;
                CommonFun.setXmlValue("QuaPrintParams", "ShowComImage", flag.ToString());
                flag = this.printpar.ShowCurve;
                CommonFun.setXmlValue("QuaPrintParams", "ShowCurve", flag.ToString());
                flag = this.printpar.ShowDes;
                CommonFun.setXmlValue("QuaPrintParams", "ShowDes", flag.ToString());
                flag = this.printpar.ShowInsAndUser;
                CommonFun.setXmlValue("QuaPrintParams", "ShowInsAndUser", flag.ToString());
                flag = this.printpar.ShowMeasure;
                CommonFun.setXmlValue("QuaPrintParams", "ShowMeasure", flag.ToString());
                flag = this.printpar.ShowStandardData;
                CommonFun.setXmlValue("QuaPrintParams", "ShowStandardData", flag.ToString());
                flag = this.printpar.ShowStandardCurve;
                CommonFun.setXmlValue("QuaPrintParams", "ShowStandardCurve", flag.ToString());
                CommonFun.setXmlValue("QuaPrintParams", "Title", this.printpar.Title);
            }
            catch
            {
                CommonFun.setXmlValue("QuaParams", "MeaMethod", "doublem");
                CommonFun.setXmlValue("QuaParams", "CapMethod", "Ввод коэффициентов уравнения");
                CommonFun.setXmlValue("QuaParams", "EConvert", "False");
                CommonFun.setXmlValue("QuaParams", "Equation", "C=f(Abs)");
                CommonFun.setXmlValue("QuaParams", "Fitting", "Линейная");
                CommonFun.setXmlValue("QuaParams", "Length", "10");
                CommonFun.setXmlValue("QuaParams", "Threshold", "");
                CommonFun.setXmlValue("QuaParams", "MCnt", "1");
                CommonFun.setXmlValue("QuaParams", "SamCnt", "0");
                CommonFun.setXmlValue("QuaParams", "Unit", "");
                CommonFun.setXmlValue("QuaParams", "WL", "546.0");
                CommonFun.setXmlValue("QuaParams", "ZeroB", "False");
                CommonFun.setXmlValue("QuaParams", "K0", "0.1");
                CommonFun.setXmlValue("QuaParams", "K1", "0");
                CommonFun.setXmlValue("QuaParams", "K2", "0");
                CommonFun.setXmlValue("QuaParams", "K3", "0");
                CommonFun.setXmlValue("QuaParams", "AFCS", "Abs = 0.1000 * C");
                CommonFun.setXmlValue("QuaParams", "K10", "10");
                CommonFun.setXmlValue("QuaParams", "K11", "0");
                CommonFun.setXmlValue("QuaParams", "K12", "0");
                CommonFun.setXmlValue("QuaParams", "K13", "0");
                CommonFun.setXmlValue("QuaParams", "CFCS", "C=10.0000*A");
                CommonFun.setXmlValue("QuaParams", "R", "");
                CommonFun.setXmlValue("QuaParams", "Sample", "");
                CommonFun.setXmlValue("QuaPrintParams", "Addtional", "");
                CommonFun.setXmlValue("QuaPrintParams", "ComImage", "");
                CommonFun.setXmlValue("QuaPrintParams", "Describtion", "");
                CommonFun.setXmlValue("QuaPrintParams", "ShowAddtional", "False");
                CommonFun.setXmlValue("QuaPrintParams", "ShowComImage", "False");
                CommonFun.setXmlValue("QuaPrintParams", "ShowCurve", "False");
                CommonFun.setXmlValue("QuaPrintParams", "ShowDes", "False");
                CommonFun.setXmlValue("QuaPrintParams", "ShowInsAndUser", "False");
                CommonFun.setXmlValue("QuaPrintParams", "ShowMeasure", "False");
                CommonFun.setXmlValue("QuaPrintParams", "ShowStandardData", "False");
                CommonFun.setXmlValue("QuaPrintParams", "ShowStandardCurve", "False");
                CommonFun.setXmlValue("QuaPrintParams", "Title", "");
            }
            if (this.sp.IsOpen)
            {
                CommonFun.WriteSendLine("back，");
                this.sp.Close();
            }
            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();
            // this.Close();
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

        private void SetTextMessage(int ipos)
        {
            if (ipos > 100)
                ipos = 100;
            if (this.SubScan)
                this.smfrm.progressBar1.Value = Convert.ToInt32(ipos);
            else
                this.progressBar1.Value = Convert.ToInt32(ipos);
        }
        private void Cleardata()
        {
           // this.lblValue.Content = "";
            MyTableList.Clear();
            meisureGrid.ItemsSource = _itemList;
            meisureGrid.Items.Refresh();
            /*meisureGrid.Dispatcher.Invoke(() =>
            {
                
                //   meisureGrid.Columns.Clear();
                // meisureGrid.Items.Clear();
                //  meisureGrid.Items.Refresh();
            });*/

        }
        private int Showslotbox(string slotcoll)
        {
            int inx = 0;
            /*using (ItemSelectFrm selectslotfrm = new ItemSelectFrm())
            {
                selectslotfrm.lbltitle.Text = CommonFun.GetLanText(this.lanvalue, "selectslot");
                string[] strArray = slotcoll.Split(',');
                for (int index = 0; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                {
                    Label lbl1 = new Label();
                    lbl1.AutoSize = false;
                    lbl1.BackColor = Color.Transparent;
                    lbl1.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    lbl1.Font = new System.Drawing.Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
                    int y = 1 + index * 70;
                    lbl1.Location = new Point(10, y);
                    lbl1.Size = new Size(580, 60);
                    lbl1.TabIndex = index;
                    lbl1.TextAlign = ContentAlignment.MiddleLeft;
                    lbl1.Text = strArray[index];
                    lbl1.Tag = (object)index.ToString();
                    lbl1.MouseClick += (MouseEventHandler)((param0, param1) =>
                    {
                        inx = Convert.ToInt32(lbl1.Tag);
                        selectslotfrm.Close(); frm.Dispose();
                    });
                    lbl1.MouseMove += (MouseEventHandler)((param0, param1) => lbl1.BackColor = SystemColors.Control);
                    lbl1.MouseLeave += (EventHandler)((param0, param1) => lbl1.BackColor = Color.Transparent);
                    Label label = new Label();
                    label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    label.Size = new Size(580, 2);
                    label.Location = new Point(10, y + 65);
                    selectslotfrm.panel1.Controls.Add((Control)lbl1);
                    selectslotfrm.panel1.Controls.Add((Control)label);
                }
                int num = (int)selectslotfrm.ShowDialog();
            }*/
            return inx;
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
                //  ScriptControl scriptControl = (ScriptControl)new ScriptControlClass();
                //     scriptControl.Language = "JavaScript";
                //  return scriptControl.Eval(Expression).ToString();
                return Expression.ToString();
            }
            catch
            {
                return "--";
            }
        }
        private int current_serias = 0;
        private void LoadMeaData(int index)
        {
            if (index < 0)
                return;
            if (this.QM.MeasreList == null || this.QM.MeasreList.Count == 0)
            {
                lblNo.Dispatcher.Invoke(() =>
                {
                    this.lblNo.Content = "0001";
                });
            }
            else
            {
                lblNo.Dispatcher.Invoke(() =>
                {
                    this.lblNo.Content = (index+1).ToString("D4") + "/" + this.QM.QPar.MCnt.ToString("D4");
                });
                if (index > this.QM.QPar.MCnt)
                    index = this.QM.QPar.MCnt;
                this.QM.QPar.WL.Split(',');
                int? nullable1 = this.QM.QPar.MeasureMethod.Square;
                System.Decimal? nullable2;
                System.Decimal num;
                //  string str = this.QM.QPar.Equation.ToString() != "C=f(Abs)" ? "A" : "T";
                /*if (this.QM.QPar.Equation.ToString() != "C=f(Abs)")
                    array[0][index] = this.QM.MeasreList[index].XGD.Value.ToString(this.absacc);
                else
                    array[0][index] = this.QM.MeasreList[index].XGD.Value.ToString(this.tacc);*/
                
                if ((index + 1) % this.QM.QPar.MCnt == 0)
                {
                    //Application.Current.Dispatcher.Invoke(() => MyTableList.Add(new QuaWLTable(array)));
                    if (this.QM.QPar.Equation.ToString() != "C=f(Abs)")
                    {

                        //MyTableList.Add(new QuaWLTable((meisureGrid.Items.Count + 1), "Образец " + (meisureGrid.Items.Count + 1), this.QM.QPar.WL.Split(',')[this.meacnt - 1], this.QM.MeasreList[index].XGD.Value.ToString(CommonFun.GetAcc("absAccuracy")), this.QM.MeasreList[index].ND.Value.ToString(CommonFun.GetAcc("absAccuracy")), DateTime.Now.ToString(), false));
                        meisureGrid.Dispatcher.Invoke(() =>
                        {
                            MyTableList.Add(new QuaWLTable((meisureGrid.Items.Count + 1), "Образец " + (meisureGrid.Items.Count + 1), this.QM.QPar.WL.Split(',')[this.meacnt - 1], this.QM.MeasreList[this.QM.MeasreList.Count - 1].XGD.Value.ToString(this.absacc), this.QM.MeasreList[this.QM.MeasreList.Count - 1].ND.ToString(), DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("ru-RU")), false));
                        });
                    }
                    else
                    {
                        // MyTableList.Add(new QuaWLTable((meisureGrid.Items.Count + 1), "Образец " + (meisureGrid.Items.Count + 1), this.QM.QPar.WL.Split(',')[this.meacnt - 1], this.QM.MeasreList[index].XGD.Value.ToString(CommonFun.GetAcc("ceAccuracy")), this.QM.MeasreList[index].ND.Value.ToString(CommonFun.GetAcc("ceAccuracy")), DateTime.Now.ToString(), false));
                        meisureGrid.Dispatcher.Invoke(() =>
                        {
                            MyTableList.Add(new QuaWLTable((meisureGrid.Items.Count + 1), "Образец " + (meisureGrid.Items.Count + 1), this.QM.QPar.WL.Split(',')[this.meacnt - 1], this.QM.MeasreList[this.QM.MeasreList.Count - 1].XGD.Value.ToString(this.tacc), this.QM.MeasreList[this.QM.MeasreList.Count - 1].ND.ToString(), DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("ru-RU")), false));
                        });
                    }

                meisureGrid.Dispatcher.Invoke(() =>
                    {
                        meisureGrid.ItemsSource = _itemList;
                        meisureGrid.Items.Refresh();
                    });
                }
                else
                {
                    current_serias++;
                }

                // Label lblAvalue = this.lblAvalue;
                /*  nullable2 = this.QM.MeasreList[index].XGD;
                  num = nullable2.Value;
                  string str1 = num.ToString(this.absacc);
                  lblAvalue.Content = str1;
                  nullable2 = this.QM.MeasreList[index].ND;
                  if ((nullable2.GetValueOrDefault() != -1M ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0)
                  {
                      //Label lblValue = this.lblValue;
                      nullable2 = this.QM.MeasreList[index].ND;
                      string str2 = nullable2.ToString();
                      lblValue.Content = str2;
                  }
                  else
                      this.lblValue.Content = "----";*/
                //this.lbltimeV.Text = this.QM.MeasreList[index].D_sj.ToString();
                // this.lblremarkV.Text = this.QM.MeasreList[index].C_bz;
                this.btnScan.Dispatcher.Invoke(() =>
                {
                    this.btnScan.Content = "Измерение";
                });
              
               /* if (index + 1 == this.QM.QPar.MCnt)
                {
                    index = 0;
                }*/
            }

            /*this.lblNo.Content = (index + 1).ToString("D4") + "/" + this.QM.MeasreList.Count.ToString("D4");

            if (index > this.QM.MeasreList.Count)
                index = this.QM.MeasreList.Count<Sample>();
            this.QM.QPar.WL.Split(',');
            int? nullable1 = this.QM.QPar.MeasureMethod.Square;
            System.Decimal? nullable2;
            System.Decimal num;
            string str = this.QM.QPar.Equation.ToString() != "C=f(Abs)" ? "A" : "T";
            if (this.QM.QPar.Equation.ToString() != "C=f(Abs)")
                array[0][index] = this.QM.MeasreList[index].XGD.Value.ToString(this.absacc);
            else
                array[0][index] = this.QM.MeasreList[index].XGD.Value.ToString(this.tacc);

            //if (index == meisureGrid.Columns.Count - 1)
            // {
            //Application.Current.Dispatcher.Invoke(() => MyTableList.Add(new QuaWLTable(array)));
            if (this.QM.QPar.Equation.ToString() != "C=f(Abs)")

                MyTableList.Add(new QuaWLTable((meisureGrid.Items.Count + 1), "Образец " + (meisureGrid.Items.Count + 1), this.QM.QPar.WL.Split(',')[this.meacnt - 1], this.QM.MeasreList[index].XGD.Value.ToString(this.absacc), this.QM.MeasreList[index].ND.Value.ToString(this.absacc), DateTime.Now.ToString(), false));
            else
                MyTableList.Add(new QuaWLTable((meisureGrid.Items.Count + 1), "Образец " + (meisureGrid.Items.Count + 1), this.QM.QPar.WL.Split(',')[this.meacnt - 1], this.QM.MeasreList[index].XGD.Value.ToString(this.tacc), this.QM.MeasreList[index].ND.Value.ToString(this.tacc), DateTime.Now.ToString(), false));

            meisureGrid.Dispatcher.Invoke(() =>
            {
                meisureGrid.ItemsSource = _itemList;
                meisureGrid.Items.Refresh();
            });
            //  }

            // Label lblAvalue = this.lblAvalue;
            nullable2 = this.QM.MeasreList[index].XGD;
            num = nullable2.Value;
            string str1 = num.ToString(this.absacc);
            //  lblAvalue.Text = str1;
            nullable2 = this.QM.MeasreList[index].ND;
            if ((nullable2.GetValueOrDefault() != -1M ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0)
            {
                //Label lblValue = this.lblValue;
                nullable2 = this.QM.MeasreList[index].ND;
                string str2 = nullable2.ToString();
                //lblValue.Content = str2;
            }
            else
              //  this.lblValue.Content = "----";
            //this.lbltimeV.Text = this.QM.MeasreList[index].D_sj.ToString();
            // this.lblremarkV.Text = this.QM.MeasreList[index].C_bz;
            this.btnScan.Content = "Измерение";*/

        }
        private void Setblanklable(bool subscan)
        {
            if (subscan)
            {
                this.smfrm.btnXL.Content = "Обнуление";
                /* if (this.smfrm.panel4.Visible)
                 {*/
                this.smfrm.progressBar1.Value = 100;
                //   this.smfrm.panel4.Visible = false;
                this.smfrm.progressBar1.Value = 0;
                // }
                this.setState(ComStatus.END);
            }
            else
            {
                this.btnBlank.Content = "Обнуление";
                this.setState(ComStatus.END);
                /*if (this.panel4.Visible)
                {*/
                this.progressBar1.Value = 000;
                //this.panel4.Visible = false;
                this.progressBar1.Value = 0;
                //}
            }
        }
        private void SetState(ComStatus ss)
        {
            this.setState(ss);
            if (this.SubScan)
            {
                //  this.smfrm.panel4.Visible = true;
                this.smfrm.progressBar1.Value = 10;
                /*if (this.ComSta == ComStatus.MEASURE)
                    this.smfrm.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "measureing");
                else
                    this.smfrm.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "inblanking");*/
            }
            else
            {
                //this.panel4.Visible = true;
                this.progressBar1.Value = 00;
                /*if (this.ComSta == ComStatus.MEASURE)
                    this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "measureing");
                else
                    this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "inblanking");*/
            }
        }

        private void setState(ComStatus ss)
        {
            switch (ss)
            {
                case ComStatus.MEASURE:
                    if (this.SubScan)
                    {
                        this.smfrm.btnBack.IsEnabled = false;
                        this.smfrm.btnLast.IsEnabled = false;
                        this.smfrm.btnMeasure.IsEnabled = true;
                        this.smfrm.btnOK.IsEnabled = false;
                        this.smfrm.btnXL.IsEnabled = false;
                        break;
                    }
                    this.btnScan.IsEnabled = true;
                    this.btnSet.IsEnabled = true;
                    this.btnBack.IsEnabled = false;
                    this.btnBlank.IsEnabled = false;
                    //this.btnOperate.Enabled = false;
                    // this.pibOut.Enabled = false;
                    break;
                case ComStatus.CALBGND:
                    if (this.SubScan)
                    {
                        this.smfrm.btnBack.IsEnabled = false;
                        this.smfrm.btnLast.IsEnabled = false;
                        this.smfrm.btnMeasure.IsEnabled = false;
                        this.smfrm.btnOK.IsEnabled = false;
                        this.smfrm.btnXL.IsEnabled = true;
                        break;
                    }
                    this.btnBlank.IsEnabled = true;
                    this.btnSet.IsEnabled = true;
                    this.btnBack.IsEnabled = false;
                    this.btnScan.IsEnabled = false;
                    //this.btnOperate.Enabled = false;
                    //  this.pibOut.Enabled = false;
                    break;
                case ComStatus.END:
                    if (this.SubScan)
                    {
                        this.smfrm.btnBack.IsEnabled = true;
                        this.smfrm.btnLast.IsEnabled = true;
                        this.smfrm.btnMeasure.IsEnabled = true;
                        this.smfrm.btnOK.IsEnabled = true;
                        this.smfrm.btnXL.IsEnabled = true;
                        break;
                    }
                    this.btnScan.IsEnabled = true;
                    this.btnSet.IsEnabled = true;
                    this.btnBack.IsEnabled = true;
                    this.btnBlank.IsEnabled = true;
                    // this.btnOperate.Enabled = true;
                    // this.pibOut.Enabled = true;
                    break;
            }
        }

        private delegate void Del_setstate(bool status);

        private delegate void Del_starttt(bool status);

        private delegate void Del_BindData(int index);

        private delegate void Del_InputSData(int index);

        private delegate void Del_SetBlankLable(bool subscan);

        private delegate void Del_SetMeasureLable(bool subscan);

        private delegate void Del_SetWl(string wl);

        private delegate void Del_SetPos(int pos);

        private delegate int Del_Selectslot(string slotcoll);

        private delegate void Del_SetState(ComStatus ss);

        private readonly ObservableCollection<QuaWLTable> _itemList = new ObservableCollection<QuaWLTable>();
        public ObservableCollection<QuaWLTable> MyTableList { get { return _itemList; } }
        string[][] array;


        private void BtnScan1_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            MyTableList.Add(new QuaWLTable(meisureGrid.Items.Count + 1, "Образец " + (meisureGrid.Items.Count + 1), "546.0", "0.0000", "0.0256", DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("ru-RU")), false));
            MyTableList.Add(new QuaWLTable(meisureGrid.Items.Count + 1, "Образец " + (meisureGrid.Items.Count + 1), "546.0", "0.0802", "0.8013", DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("ru-RU")), false));
            MyTableList.Add(new QuaWLTable(meisureGrid.Items.Count + 1, "Образец " + (meisureGrid.Items.Count + 1), "546.0", "0.1083", "1.0731", DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("ru-RU")), false));
            meisureGrid.ItemsSource = _itemList;
            meisureGrid.Items.Refresh();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyTableList)
            {
                item.BooleanFlag = true;
                meisureGrid.Items.Refresh();

            }
        }

        private void UnheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyTableList)
            {
                item.BooleanFlag = false;
                meisureGrid.Items.Refresh();
            }
        }

        private void DeleteRow_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

            foreach (var x in MyTableList.ToList())
            {
                if (x.BooleanFlag)
                {
                    MyTableList.Remove(x);
                }
            }
            meisureGrid.Items.Refresh();

        }
    }

    public class QuaWLTable
    {
        public QuaWLTable() { }
        public QuaWLTable(List<object> array)
        {
            this.Number = Convert.ToInt32(array[0]);
            this.Name = Convert.ToString(array[1]);
            this.WL = Convert.ToString(array[2]);
            this.Abs = Convert.ToString(array[3]);
            this.TProcent = Convert.ToString(array[4]);
            this.DateTime_ = Convert.ToString(array[5]);
            this.BooleanFlag = false;
        }
        public QuaWLTable(int Number, string Name, string WL, string Abs, string TProcent, string DateTime_, bool BooleanFlag)
        {
            this.Number = Number;
            this.Name = Name;
            this.WL = WL;
            this.Abs = Abs;
            this.TProcent = TProcent;
            this.DateTime_ = DateTime_;
            this.BooleanFlag = BooleanFlag;
        }

        private int _value_number;
        public int Number { get { return _value_number; } set { _value_number = value; } }
        private string _value_name;
        public string Name { get { return _value_name; } set { _value_name = value; } }
        private string _value_wl;
        public string WL { get { return _value_wl; } set { _value_wl = value; } }
        private string _value_abs;
        public string Abs { get { return _value_abs; } set { _value_abs = value; } }
        private string _value_tprocent;
        public string TProcent { get { return _value_tprocent; } set { _value_tprocent = value; } }
        private string _value_datetime;
        public string DateTime_ { get { return _value_datetime; } set { _value_datetime = value; } }
        private bool _value_checkrow;
        public bool BooleanFlag { get { return _value_checkrow; } set { _value_checkrow = value; } }

    }
}

