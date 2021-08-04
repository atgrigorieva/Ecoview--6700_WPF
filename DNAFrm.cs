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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using UVStudio.Properties;

namespace UVStudio
{
    public partial class DNAFrm : Form
    {
        private QuaMethod QM;
        private QuaMethod subQM;
        private QuaPrintParams printpar = (QuaPrintParams)null;
        private SerialPort sp = new SerialPort();
        private ComStatus ComSta;
        private Thread dealth;
        private bool runptag = true;
        private bool SubScan = false;
        private string lanvalue;
        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");
        private string conacc = CommonFun.GetAcc("ceAccuracy");
        public List<string> rightlist = new List<string>();
        private StandardMeasureFrm smfrm = new StandardMeasureFrm();
        private Thread tdstart;
        private System.Windows.Forms.Timer tt = new System.Windows.Forms.Timer();
        private int tickcnt = 0;
        private int calormea = 1;
        private string slotno = "";
        private int currslotno = 0;
        private int meacnt = 0;
        private int mcnt = 0;
        private DNAMeasureData sslive = new DNAMeasureData();
        private Queue myque = new Queue();
        private string receive = "";
        private List<MeaureData> mdlists = new List<MeaureData>();
        private int endcnt = 0;
        private int currindex = -1;
        private int sourcex;
        private int sourcey;
        public DNAFrm()
        {
            InitializeComponent();
        }
        private void DNAFrm_Load(object sender, EventArgs e)
        {

            if (this.QM == null)
            {
                this.QM = new QuaMethod();
                this.QM.QPar = new QuaParmas();
                this.QM.Page = 2;
                try
                {
                    this.QM.QPar.MeasureMethodNameDM = CommonFun.getXmlValue("DNAParams", "MeaMethod");
                    this.QM.QPar.MeasureMethodName = CommonFun.GetLanText(this.QM.QPar.MeasureMethodNameDM);
                    this.QM.QPar.BackWL = CommonFun.getXmlValue("DNAParams", "BackWL");
                    this.QM.QPar.WL = CommonFun.getXmlValue("DNAParams", "WL");
                    this.QM.QPar.R = CommonFun.getXmlValue("DNAParams", "R");
                    this.QM.QPar.Length = CommonFun.getXmlValue("DNAParams", "Length");
                    this.QM.QPar.EConvert = Convert.ToBoolean(CommonFun.getXmlValue("DNAParams", "EConvert"));
                    this.QM.QPar.Limits = CommonFun.getXmlValue("DNAParams", "Threshold");
                    this.QM.QPar.MCnt = Convert.ToInt32(CommonFun.getXmlValue("DNAParams", "MCnt"));
                    this.QM.QPar.Unit = CommonFun.getXmlValue("DNAParams", "Unit");
                    if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                    {
                        this.lblValue2.Visible = true;
                        this.lblUnit2.Visible = true;
                    }
                    else
                    {
                        this.lblValue2.Visible = false;
                        this.lblUnit2.Visible = false;
                        this.QM.QPar.SamCnt = Convert.ToInt32(CommonFun.getXmlValue("DNAParams", "SamCnt"));
                        this.QM.QPar.FittingDM = CommonFun.getXmlValue("DNAParams", "Fitting");
                        this.QM.QPar.Fitting = CommonFun.GetLanText(this.QM.QPar.FittingDM);
                        this.QM.QPar.Equation = CommonFun.getXmlValue("DNAParams", "Equation");
                        this.QM.QPar.CabMethodDM = CommonFun.getXmlValue("DNAParams", "CapMethod");
                        this.QM.QPar.CabMethod = CommonFun.GetLanText(this.QM.QPar.CabMethodDM);
                        this.QM.QPar.ZeroB = Convert.ToBoolean(CommonFun.getXmlValue("DNAParams", "ZeroB"));
                        this.QM.K0 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K0"));
                        this.QM.K1 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K1"));
                        this.QM.K2 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K2"));
                        this.QM.K3 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K3"));
                        this.QM.AFCS = CommonFun.getXmlValue("DNAParams", "AFCS");
                        this.QM.K10 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K10"));
                        this.QM.K11 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K11"));
                        this.QM.K12 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K12"));
                        this.QM.K13 = Convert.ToDecimal(CommonFun.getXmlValue("DNAParams", "K13"));
                        this.QM.CFCS = CommonFun.getXmlValue("DNAParams", "CFCS");
                        string xmlValue = CommonFun.getXmlValue("DNAParams", "Sample");
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
                                CommonFun.showbox(CommonFun.GetLanText("loadparerror"), "Error");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.QM.QPar.MeasureMethodNameDM = "dna1";
                    this.QM.QPar.MeasureMethodName = CommonFun.GetLanText(this.QM.QPar.MeasureMethodNameDM);
                    this.QM.QPar.BackWL = "";
                    this.QM.QPar.WL = "260.0,280.0";
                    this.QM.QPar.R = "62.9,36.0,1552.0,757.3";
                    this.QM.QPar.Length = "10";
                    this.QM.QPar.EConvert = false;
                    this.QM.QPar.Limits = "";
                    this.QM.QPar.MCnt = 1;
                    this.QM.QPar.Unit = "";
                    this.lblValue2.Visible = true;
                    this.lblUnit2.Visible = true;
                }
            }
            if (this.printpar == null)
            {
                this.printpar = new QuaPrintParams();
                try
                {
                    this.printpar.Addtional = CommonFun.getXmlValue("DNAPrintParams", "Addtional");
                    this.printpar.ComImage = CommonFun.getXmlValue("DNAPrintParams", "ComImage");
                    this.printpar.Describtion = CommonFun.getXmlValue("DNAPrintParams", "Describtion");
                    this.printpar.ShowAddtional = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowAddtional"));
                    this.printpar.ShowComImage = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowComImage"));
                    this.printpar.ShowCurve = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowCurve"));
                    this.printpar.ShowDes = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowDes"));
                    this.printpar.ShowInsAndUser = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowInsAndUser"));
                    this.printpar.ShowMeasure = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowMeasure"));
                    this.printpar.ShowStandardCurve = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowStandardCurve"));
                    this.printpar.ShowStandardData = Convert.ToBoolean(CommonFun.getXmlValue("DNAPrintParams", "ShowStandardData"));
                    this.printpar.Title = CommonFun.getXmlValue("DNAPrintParams", "Title");
                }
                catch
                {
                    this.printpar.Addtional = "";
                    this.printpar.ComImage = "";
                    this.printpar.Describtion = "";
                    this.printpar.ShowAddtional = false;
                    this.printpar.ShowComImage = false;
                    this.printpar.ShowCurve = false;
                    this.printpar.ShowDes = false;
                    this.printpar.ShowInsAndUser = true;
                    this.printpar.ShowMeasure = false;
                    this.printpar.ShowStandardCurve = false;
                    this.printpar.ShowStandardData = false;
                    this.printpar.Title = "Title";
                }
            }
            if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1"))
            {
                if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                    this.lblfor.Text = "C(DNA)=62.9*(A1-Aref)-36.0*(A2-Aref); C(Protein)=1552*(A2-Aref)-757.3*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                else
                    this.lblfor.Text = "C(DNA)=62.9*A1-36.0*A2; C(Protein)=1552*A2-757.3*A2; Ratio=A1/A2";
            }
            else if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
            {
                if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                    this.lblfor.Text = "C(DNA)=49.1*(A1-Aref)-3.48*(A2-Aref); C(Protein)=183*(A2-Aref)-75.8*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                else
                    this.lblfor.Text = "C(DNA)=49.1*A1-3.48*A2; C(Protein)=183*A2-75.8*A2; Ratio=A1/A2";
            }
            else if (this.QM.QPar.Equation == "Abs=f(C)")
                this.lblfor.Text = this.QM.AFCS;
            else
                this.lblfor.Text = this.QM.CFCS;
            this.lblUnit1.Text = this.QM.QPar.Unit;
            this.lblUnit2.Text = this.QM.QPar.Unit;
            this.Cleardata();
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("rightdnameasure"))
                    this.btnScan.Enabled = true;
                else
                    this.btnScan.Enabled = false;
                if (this.rightlist.Contains("rightdnablank"))
                    this.btnBlank.Enabled = true;
                else
                    this.btnBlank.Enabled = false;
                /*if (CommonFun.GetAppConfig("LockSystem") == "true")
                    this.btnlock.Visible = true;
                else
                    this.btnlock.Visible = false;*/
            }
            this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
            this.tdstart.Start();
            this.setstate(false);
            if (CommonFun.GetAppConfig("currentconnect") == "-1")
                this.btnBack.Enabled = true;
            this.picOut.Height = 240;
            this.picOut.Width = 320;
            this.tt.Interval = 1000;
            this.tt.Tick += new EventHandler(this.tt_Tick);


        }

        private void tt_Tick(object sender, EventArgs e)
        {
            ++this.tickcnt;
            if (this.tickcnt != 60)
                return;
            this.ComSta = ComStatus.END;
            this.tickcnt = 0;
            if (this.btnBlank.InvokeRequired)
                this.btnBlank.Invoke((Delegate)new DNAFrm.Del_setstate(this.setstate), (object)true);
            else
                this.setstate(true);
        }

        private void setstate(bool status)
        {
            this.btnBack.Enabled = status;
            this.btnBlank.Enabled = status;
            this.btnScan.Enabled = status;
            if (!(CommonFun.GetAppConfig("GLPEnabled") == "true"))
                return;
            if (this.rightlist.Contains("rightdnameasure") && status)
                this.btnScan.Enabled = true;
            else
                this.btnScan.Enabled = false;
            if (this.rightlist.Contains("rightdnablank") && status)
                this.btnBlank.Enabled = true;
            else
                this.btnBlank.Enabled = false;
            /*if (CommonFun.GetAppConfig("LockSystem") == "true")
                this.btnlock.Visible = true;
            else
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
                this.sp.DataBits = 8;
                this.sp.StopBits = StopBits.One;
                this.sp.PortName = "COM2";
                this.sp.Parity = Parity.None;
                this.sp.ReadTimeout = -1;
                this.sp.Open();
                CommonFun.WriteSendLine("Порт открыт");
                this.sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);
                this.ComSta = ComStatus.BD_RATIO_FLUSH;
                this.sp.WriteLine("BD_RATIO_FLUSH \r\n");
                CommonFun.WriteSendLine("BD_RATIO_FLUSH");
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new DNAFrm.Del_starttt(this.Starttt), (object)true);
                else
                    this.Starttt(true);
            }
            catch (Exception ex)
            {
                CommonFun.showbox(CommonFun.GetLanText("loadparerror"), "Error");
                this.ComSta = ComStatus.END;
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new DNAFrm.Del_setstate(this.setstate), (object)true);
                else
                    this.setstate(true);
                if (this.btnBlank.InvokeRequired)
                    this.btnBlank.Invoke((Delegate)new DNAFrm.Del_starttt(this.Starttt), (object)false);
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

        private void Cleardata()
        {
            this.lblValue1.Text = "";
            this.lblValue2.Text = "";
            this.lbltimeV.Text = "";
            this.lblremarkV.Text = "";
            if (this.QM.QPar.MeasureMethodName.Contains("ДНК"))
            {
                this.lblA1.Text = "A1\r\n" + this.QM.QPar.WL.Split(',')[0];
                this.lblA2.Text = "A2\r\n" + this.QM.QPar.WL.Split(',')[1];
                if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                    this.lblA3.Text = "Aref\r\n" + this.QM.QPar.BackWL;
                else
                    this.lblA3.Text = "Aref";
                this.lblA4.Text = "Ratio";
            }
            else
            {
                this.lblA1.Text = "A1\r\n" + this.QM.QPar.WL;
                this.lblA2.Text = "";
                this.lblA3.Text = "";
                this.lblA4.Text = "";
            }
            this.lblA1V.Text = "";
            this.lblA2V.Text = "";
            this.lblA3V.Text = "";
            this.lblA4V.Text = "";
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (this.pibCurve.Visible)
            {
                if (this.QM.QPar.MeasureMethodName.Contains("ДНК"))
                {
                    this.lblValue1.Visible = true;
                    this.lblUnit1.Visible = true;
                    this.lblValue2.Visible = true;
                    this.lblUnit2.Visible = true;
                    this.panel3.Visible = true;
                    this.pibCurve.Visible = false;
                    this.btnScan.Enabled = true;
                    this.btnBack.Image = (System.Drawing.Image)Resources.Icon_Home;
                }
                else
                {
                    this.lblValue1.Visible = true;
                    this.lblUnit1.Visible = true;
                    this.panel3.Visible = true;
                    this.pibCurve.Visible = false;
                    this.btnScan.Enabled = true;
                    this.btnBack.Image = (System.Drawing.Image)Resources.Icon_Home;
                }
            }
            else
            {
                /*if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                {
                    if (new DRMessageBoxFrm(CommonFun.GetLanText("exitconfirm"), "Warning").ShowDialog() != DialogResult.Yes)
                        return;
                    if (!this.QM.D_time.HasValue)
                    {
                        if (this.QM.DNAMeaList != null && this.QM.DNAMeaList.Count != 0)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("datasaveexit"), "Warning");
                            return;
                        }
                    }
                }
                else if (!this.QM.D_time.HasValue && (this.QM.DNAMeaList != null && this.QM.DNAMeaList.Count != 0 && new DRMessageBoxFrm(CommonFun.GetLanText("unsavedataexit"), "Warning").ShowDialog() == DialogResult.No))
                    return;*/
                try
                {
                    CommonFun.setXmlValue("DNAParams", "MeaMethod", this.QM.QPar.MeasureMethodNameDM);
                    CommonFun.setXmlValue("DNAParams", "WL", this.QM.QPar.WL);
                    CommonFun.setXmlValue("DNAParams", "BackWL", this.QM.QPar.BackWL);
                    CommonFun.setXmlValue("DNAParams", "R", this.QM.QPar.R);
                    bool flag = this.QM.QPar.EConvert;
                    CommonFun.setXmlValue("DNAParams", "EConvert", flag.ToString());
                    CommonFun.setXmlValue("DNAParams", "Length", this.QM.QPar.Length.ToString());
                    CommonFun.setXmlValue("DNAParams", "Threshold", this.QM.QPar.Limits);
                    int num1 = this.QM.QPar.MCnt;
                    CommonFun.setXmlValue("DNAParams", "MCnt", num1.ToString());
                    CommonFun.setXmlValue("DNAParams", "Unit", this.QM.QPar.Unit);
                    if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                    {
                        CommonFun.setXmlValue("DNAParams", "CapMethod", "");
                        CommonFun.setXmlValue("DNAParams", "Equation", "");
                        CommonFun.setXmlValue("DNAParams", "Fitting", "");
                        CommonFun.setXmlValue("DNAParams", "SamCnt", "");
                        CommonFun.setXmlValue("DNAParams", "ZeroB", "");
                        CommonFun.setXmlValue("DNAParams", "K0", "");
                        CommonFun.setXmlValue("DNAParams", "K1", "");
                        CommonFun.setXmlValue("DNAParams", "K2", "");
                        CommonFun.setXmlValue("DNAParams", "K3", "");
                        CommonFun.setXmlValue("DNAParams", "AFCS", "");
                        CommonFun.setXmlValue("DNAParams", "K10", "");
                        CommonFun.setXmlValue("DNAParams", "K11", "");
                        CommonFun.setXmlValue("DNAParams", "K12", "");
                        CommonFun.setXmlValue("DNAParams", "K13", "");
                        CommonFun.setXmlValue("DNAParams", "CFCS", "");
                        CommonFun.setXmlValue("DNAParams", "Sample", "");
                    }
                    else
                    {
                        CommonFun.setXmlValue("DNAParams", "CapMethod", this.QM.QPar.CabMethodDM);
                        CommonFun.setXmlValue("DNAParams", "Equation", this.QM.QPar.Equation);
                        CommonFun.setXmlValue("DNAParams", "Fitting", this.QM.QPar.FittingDM);
                        num1 = this.QM.QPar.SamCnt;
                        CommonFun.setXmlValue("DNAParams", "SamCnt", num1.ToString());
                        flag = this.QM.QPar.ZeroB;
                        CommonFun.setXmlValue("DNAParams", "ZeroB", flag.ToString());
                        CommonFun.setXmlValue("DNAParams", "K0", this.QM.K0.ToString());
                        System.Decimal num2 = this.QM.K1;
                        CommonFun.setXmlValue("DNAParams", "K1", num2.ToString());
                        num2 = this.QM.K2;
                        CommonFun.setXmlValue("DNAParams", "K2", num2.ToString());
                        num2 = this.QM.K3;
                        CommonFun.setXmlValue("DNAParams", "K3", num2.ToString());
                        CommonFun.setXmlValue("DNAParams", "AFCS", this.QM.AFCS);
                        num2 = this.QM.K10;
                        CommonFun.setXmlValue("DNAParams", "K10", num2.ToString());
                        num2 = this.QM.K11;
                        CommonFun.setXmlValue("DNAParams", "K11", num2.ToString());
                        num2 = this.QM.K12;
                        CommonFun.setXmlValue("DNAParams", "K12", num2.ToString());
                        num2 = this.QM.K13;
                        CommonFun.setXmlValue("DNAParams", "K13", num2.ToString());
                        CommonFun.setXmlValue("DNAParams", "CFCS", this.QM.CFCS);
                        if (this.QM.SamList != null && this.QM.SamList.Count > 0)
                        {
                            string xmlValue = "";
                            foreach (Sample sam in this.QM.SamList)
                                xmlValue = xmlValue + (object)sam.ND + "," + (object)sam.XGD + ";";
                            CommonFun.setXmlValue("DNAParams", "Sample", xmlValue);
                        }
                        else
                            CommonFun.setXmlValue("DNAParams", "Sample", "");
                    }
                    /*CommonFun.setXmlValue("DNAPrintParams", "Addtional", this.printpar.Addtional);
                    CommonFun.setXmlValue("DNAPrintParams", "ComImage", this.printpar.ComImage);
                    CommonFun.setXmlValue("DNAPrintParams", "Describtion", this.printpar.Describtion);
                    flag = this.printpar.ShowAddtional;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowAddtional", flag.ToString());
                    flag = this.printpar.ShowComImage;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowComImage", flag.ToString());
                    flag = this.printpar.ShowCurve;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowCurve", flag.ToString());
                    flag = this.printpar.ShowDes;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowDes", flag.ToString());
                    flag = this.printpar.ShowInsAndUser;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowInsAndUser", flag.ToString());
                    flag = this.printpar.ShowMeasure;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowMeasure", flag.ToString());
                    flag = this.printpar.ShowStandardData;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowStandardData", flag.ToString());
                    flag = this.printpar.ShowStandardCurve;
                    CommonFun.setXmlValue("DNAPrintParams", "ShowStandardCurve", flag.ToString());
                    CommonFun.setXmlValue("DNAPrintParams", "Title", this.printpar.Title);*/
                }
                catch
                {
                    CommonFun.setXmlValue("DNAParams", "MeaMethod", "dna2");
                    CommonFun.setXmlValue("DNAParams", "WL", "260.0,230.0");
                    CommonFun.setXmlValue("DNAParams", "BackWL", "");
                    CommonFun.setXmlValue("DNAParams", "R", "49.1,3.48,183.0,75.8");
                    CommonFun.setXmlValue("DNAParams", "EConvert", "False");
                    CommonFun.setXmlValue("DNAParams", "Length", "10");
                    CommonFun.setXmlValue("DNAParams", "Threshold", "");
                    CommonFun.setXmlValue("DNAParams", "MCnt", "1");
                    CommonFun.setXmlValue("DNAParams", "Unit", "");
                    CommonFun.setXmlValue("DNAParams", "CapMethod", "");
                    CommonFun.setXmlValue("DNAParams", "Equation", "");
                    CommonFun.setXmlValue("DNAParams", "Fitting", "");
                    CommonFun.setXmlValue("DNAParams", "SamCnt", "");
                    CommonFun.setXmlValue("DNAParams", "ZeroB", "");
                    CommonFun.setXmlValue("DNAParams", "K0", "");
                    CommonFun.setXmlValue("DNAParams", "K1", "");
                    CommonFun.setXmlValue("DNAParams", "K2", "");
                    CommonFun.setXmlValue("DNAParams", "K3", "");
                    CommonFun.setXmlValue("DNAParams", "AFCS", "");
                    CommonFun.setXmlValue("DNAParams", "K10", "");
                    CommonFun.setXmlValue("DNAParams", "K11", "");
                    CommonFun.setXmlValue("DNAParams", "K12", "");
                    CommonFun.setXmlValue("DNAParams", "K13", "");
                    CommonFun.setXmlValue("DNAParams", "CFCS", "");
                    CommonFun.setXmlValue("DNAParams", "Sample", "");
                    /*CommonFun.setXmlValue("DNAPrintParams", "Addtional", "");
                    CommonFun.setXmlValue("DNAPrintParams", "ComImage", "");
                    CommonFun.setXmlValue("DNAPrintParams", "Describtion", "");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowAddtional", "False");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowComImage", "False");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowCurve", "False");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowDes", "False");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowInsAndUser", "True");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowMeasure", "False");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowStandardData", "False");
                    CommonFun.setXmlValue("DNAPrintParams", "ShowStandardCurve", "False");
                    CommonFun.setXmlValue("DNAPrintParams", "Title", "");*/
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
                this.Hide();
                //      CommonFun.WriteLine("Получаем меню");
                MenuProgram menuProgram = new MenuProgram();
                //     CommonFun.WriteLine("Выводим меню");
                menuProgram.Show();
                //    CommonFun.WriteLine("Получаем родительское окно");
                this.Close();
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            using (MeathodQuaFrm frm = new MeathodQuaFrm(this.QM))
            {
                if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                {
                    if (this.rightlist.Contains("rightdnaopenmethod"))
                    {
                        //frm.btnLoad.Enabled = true;
                        frm.btnSave.Enabled = true;
                        frm.btnOpen.Enabled = true;
                    }
                    else
                    {
                        //frm.btnLoad.Enabled = false;
                        frm.btnSave.Enabled = false;
                        frm.btnOpen.Enabled = false;
                    }
                    if (this.rightlist.Contains("rightdnaeditmethod"))
                        frm.btnNew.Enabled = true;
                    else
                        frm.btnNew.Enabled = false;
                }
                //if (!this.btnOperate.Enabled)
                //    frm.btnOK.Enabled = false;
                frm.btnNew.Click += (EventHandler)((param0_1, param1_1) =>
                {
                    using (DNANewMethodFrm frm1 = new DNANewMethodFrm())
                    {
                        if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                        {
                            // if (this.rightlist.Contains("rightdnasavemethod"))
                            //   frm1.btnSavePar.Enabled = true;
                            // else
                            //      frm1.btnSavePar.Enabled = false;
                            //  if (this.rightlist.Contains("rightdnaopenmethod"))
                            //      frm1.btnLoadPar.Enabled = true;
                            //  else
                            //       frm1.btnLoadPar.Enabled = false;
                        }
                        QuaMethod nqm = new QuaMethod();
                        nqm.Page = 2;
                        nqm.QPar = new QuaParmas();

                        frm1.Load += (EventHandler)((param0_2, param1_2) =>
                        {
                            frm1.lblMMV.Text = this.QM.QPar.MeasureMethodName;
                            frm1.lblMMV.Tag = (object)this.QM.QPar.MeasureMethodNameDM;
                            if (((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>() > 1)
                            {
                                frm1.lblwl1.Text = this.QM.QPar.WL.Split(',')[0];
                                frm1.lblwl2.Text = this.QM.QPar.WL.Split(',')[1];
                                frm1.lblwl2.Enabled = true;
                                frm1.label5.Enabled = true;
                            }
                            else
                            {
                                frm1.lblwl1.Text = this.QM.QPar.WL;
                                frm1.lblwl2.Text = "";
                                frm1.lblwl2.Enabled = false;
                                frm1.label5.Enabled = false;
                            }
                            if (this.QM.QPar.BackWL.Length > 0)
                            {
                                frm1.pibBackWL.BackgroundImage = (System.Drawing.Image)Resources.UI_DB_Switcher_On;
                                frm1.pibBackWL.Tag = (object)"true";
                                frm1.pibBackWL.Enabled = true;
                                frm1.lblBackWL.Enabled = true;
                                frm1.label6.Enabled = true;
                                frm1.lblwl3.Enabled = true;
                                frm1.lblwl3.Text = this.QM.QPar.BackWL;
                            }
                            else
                            {
                                frm1.pibBackWL.BackgroundImage = (System.Drawing.Image)Resources.UI_DB_Switcher_Off;
                                frm1.pibBackWL.Tag = (object)"false";
                                frm1.lblwl3.Text = "";
                                frm1.label6.Enabled = false;
                                frm1.lblBackWL.Enabled = false;
                                frm1.lblwl3.Enabled = false;
                            }
                            frm1.lblRV.Text = this.QM.QPar.R;
                            Label lblCntV = frm1.lblCntV;
                            int num = this.QM.QPar.MCnt;
                            string str1 = num.ToString();
                            lblCntV.Text = str1;
                            frm1.lblLengthV.Text = this.QM.QPar.Length.ToString();
                            frm1.lblUnitV.Text = this.QM.QPar.Unit;
                            if (this.QM.QPar.Limits.Length > 0)
                            {
                                if (((IEnumerable<string>)this.QM.QPar.Limits.Split(',')).Count<string>() > 1)
                                    frm1.lblThresholdV.Text = CommonFun.GetLanText("maxv") + ":" + this.QM.QPar.Limits.Split(',')[0] + "," + CommonFun.GetLanText("minv") + ":" + this.QM.QPar.Limits.Split(',')[1];
                                else
                                    frm1.lblThresholdV.Text = this.QM.QPar.Limits;
                                //frm1.lblLimitsV.Text = this.QM.QPar.Limits;
                            }
                            else
                            {
                                frm1.lblThresholdV.Text = "";
                                //   frm1.lblLimitsV.Text = "";
                            }
                            if (this.QM.QPar.EConvert)
                            {
                                frm1.pibEConvert.BackgroundImage = (System.Drawing.Image)Resources.UI_DB_Switcher_On;
                                frm1.pibEConvert.Tag = (object)"true";
                            }
                            else
                            {
                                frm1.pibEConvert.BackgroundImage = (System.Drawing.Image)Resources.UI_DB_Switcher_Off;
                                frm1.pibEConvert.Tag = (object)"off";
                            }
                            if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                            {
                                frm1.panelright.Visible = false;
                                frm1.btnOK.Visible = false;
                                frm1.btnFinish.Visible = true;
                            }
                            else
                            {
                                frm1.panelright.Visible = true;
                                frm1.btnOK.Visible = true;
                                frm1.btnFinish.Visible = false;
                                frm1.lblfittingV.Text = this.QM.QPar.Fitting;
                                frm1.lblfittingV.Tag = (object)this.QM.QPar.FittingDM;
                                frm1.lblequationV.Text = this.QM.QPar.Equation;
                                frm1.lblCabMethodV.Text = this.QM.QPar.CabMethod;
                                frm1.lblCabMethodV.Tag = (object)this.QM.QPar.CabMethodDM;
                                if (this.QM.QPar.CabMethodDM == "inputr")
                                {
                                    frm1.lblequationV.Enabled = false;
                                    frm1.lblequation.Enabled = false;
                                    frm1.label16.Enabled = false;
                                    frm1.panel9.Enabled = false;
                                }
                                else
                                {
                                    frm1.lblequationV.Enabled = true;
                                    frm1.lblequation.Enabled = true;
                                    frm1.label16.Enabled = true;
                                    frm1.panel9.Enabled = true;
                                }
                                Label lblSamCntV = frm1.lblSamCntV;
                                num = this.QM.QPar.SamCnt;
                                string str2 = num.ToString();
                                lblSamCntV.Text = str2;
                                if (this.QM.QPar.ZeroB)
                                {
                                    frm1.pibZeroB.BackgroundImage = (System.Drawing.Image)Resources.UI_DB_Switcher_On;
                                    frm1.pibZeroB.Tag = (object)"true";
                                }
                                else
                                {
                                    frm1.pibZeroB.BackgroundImage = (System.Drawing.Image)Resources.UI_DB_Switcher_Off;
                                    frm1.pibZeroB.Tag = (object)"off";
                                }
                            }
                        });

                        frm1.btnOK.Click += (EventHandler)((param0_2, param1_2) =>
                        {
                            try
                            {
                                if (nqm.QPar.MeasureMethodName != null && nqm.QPar.MeasureMethodName.Length > 0 && nqm.QPar.MeasureMethodName != frm1.lblMMV.Text)
                                    nqm.SamList = new List<Sample>();
                                if (nqm.QPar.CabMethod != null && nqm.QPar.CabMethod.Length > 0 && nqm.QPar.CabMethod != frm1.lblCabMethodV.Text)
                                    nqm.SamList = new List<Sample>();
                                nqm.QPar.MeasureMethodName = frm1.lblMMV.Text;
                                nqm.QPar.MeasureMethodNameDM = frm1.lblMMV.Tag.ToString();
                                if (nqm.QPar.MeasureMethodName.Contains("ДНК"))
                                {
                                    try
                                    {
                                        Convert.ToDecimal(frm1.lblwl1.Text);
                                        Convert.ToDecimal(frm1.lblwl2.Text);
                                    }
                                    catch
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("wlerror"), "Error");
                                        return;
                                    }
                                    if (((IEnumerable<string>)frm1.lblRV.Text.Split(',')).Count<string>() != 4)
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("rerror"), "Error");
                                        return;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        Convert.ToDecimal(frm1.lblwl1.Text);
                                    }
                                    catch
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("parerror"), "Error");
                                        return;
                                    }
                                }
                                if (frm1.lblCabMethodV.Text == "" || frm1.lblequationV.Text == "" || frm1.lblfittingV.Text == "")
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("parerror"), "Error");
                                    return;
                                }
                                if (frm1.lblCabMethodV.Tag.ToString() != "inputr" && frm1.lblSamCntV.Text == "")
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("smcnterror"), "Error");
                                    return;
                                }
                                nqm.QPar.BackWL = !(frm1.pibBackWL.Tag.ToString() == "off") ? frm1.lblwl3.Text : "";
                                nqm.QPar.R = frm1.lblRV.Text;
                                nqm.QPar.WL = !frm1.lblwl2.Enabled ? frm1.lblwl1.Text : frm1.lblwl1.Text + "," + frm1.lblwl2.Text;
                                nqm.QPar.EConvert = !(frm1.pibEConvert.Tag.ToString() == "off");
                                nqm.QPar.MCnt = (int)Convert.ToInt16(frm1.lblCntV.Text);
                                nqm.QPar.Length = frm1.lblLengthV.Text;
                                //nqm.QPar.Limits = frm1.lblThresholdV.Text.ToString();
                                nqm.QPar.Limits = this.QM.QPar.Limits.Replace(CommonFun.GetLanText("maxv") + ":", "").Replace(CommonFun.GetLanText("minv") + ":", "");
                                nqm.QPar.Unit = frm1.lblUnitV.Text;
                                nqm.QPar.CabMethod = frm1.lblCabMethodV.Text;
                                nqm.QPar.CabMethodDM = frm1.lblCabMethodV.Tag.ToString();
                                nqm.QPar.Equation = frm1.lblequationV.Text;
                                nqm.QPar.Fitting = frm1.lblfittingV.Text;
                                nqm.QPar.FittingDM = frm1.lblfittingV.Tag.ToString();
                                nqm.QPar.SamCnt = !(frm1.lblSamCntV.Text != "") ? 0 : (int)Convert.ToInt16(frm1.lblSamCntV.Text);
                                if (frm1.pibZeroB.Tag.ToString() == "off")
                                {
                                    nqm.QPar.ZeroB = false;
                                }
                                else
                                {
                                    nqm.QPar.ZeroB = true;
                                    nqm.K0 = 0M;
                                }
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                                return;
                            }
                            using (this.smfrm = new StandardMeasureFrm())
                            {
                                this.smfrm.Load += (EventHandler)((param0_3, param1_3) =>
                                {
                                    this.smfrm.QM = nqm;
                                    if (nqm.QPar.CabMethod == CommonFun.GetLanText("inputr"))
                                    {
                                        if (nqm.Page == 1)
                                            this.smfrm.lblTitle.Text = CommonFun.GetLanText("quainputr");
                                        else
                                            this.smfrm.lblTitle.Text = CommonFun.GetLanText("proteininputr");
                                        this.smfrm.dataGridView1.Visible = false;
                                        this.smfrm.panel3.Visible = true;
                                        this.smfrm.btnMeasure.Enabled = false;
                                        this.smfrm.btnXL.Enabled = false;
                                        if (nqm.QPar.ZeroB)
                                        {
                                            this.smfrm.lblK0V.Text = "0";
                                            /* this.smfrm.lblK0.Enabled = false;
                                             this.smfrm.lblK0V.Enabled = false;*/
                                            this.smfrm.lblK20_.Enabled = false;
                                        }
                                        if (nqm.QPar.Fitting == CommonFun.GetLanText("linear"))
                                        {
                                            /*this.smfrm.lblK1V.Enabled = true;
                                            this.smfrm.lblK2V.Enabled = false;
                                            this.smfrm.lblK3V.Enabled = false;
                                            this.smfrm.lblK1.Enabled = true;
                                            this.smfrm.lblK2.Enabled = false;
                                            this.smfrm.lblK3.Enabled = false;
                                            this.smfrm.label6.Enabled = true;
                                            this.smfrm.label7.Enabled = false;
                                            this.smfrm.label9.Enabled = false;*/

                                            // this.smfrm.lblK20_.Enabled = true;
                                            this.smfrm.lblK21_.Enabled = true;
                                            this.smfrm.lblK22_.Enabled = false;
                                            this.smfrm.lblK23_.Enabled = false;

                                            if (nqm.QPar.Equation == "C=f(Abs)")
                                                this.smfrm.lblequation.Text = "C=K1*A+K0";
                                            else
                                                this.smfrm.lblequation.Text = "A=K1*C+K0";
                                        }
                                        else if (nqm.QPar.Fitting == CommonFun.GetLanText("squar"))
                                        {
                                            /*this.smfrm.lblK1V.Enabled = true;
                                            this.smfrm.lblK2V.Enabled = true;
                                            this.smfrm.lblK3V.Enabled = false;
                                            this.smfrm.lblK1.Enabled = true;
                                            this.smfrm.lblK2.Enabled = true;
                                            this.smfrm.lblK3.Enabled = false;
                                            this.smfrm.label6.Enabled = true;
                                            this.smfrm.label7.Enabled = true;
                                            this.smfrm.label9.Enabled = false;*/

                                            //   this.smfrm.lblK20_.Enabled = false;
                                            this.smfrm.lblK21_.Enabled = true;
                                            this.smfrm.lblK22_.Enabled = true;
                                            this.smfrm.lblK23_.Enabled = false;

                                            if (nqm.QPar.Equation == "C=f(Abs)")
                                                this.smfrm.lblequation.Text = "C=K2*A^2+K1*A+K0";
                                            else
                                                this.smfrm.lblequation.Text = "A=K2*C^2+K1*C+K0";
                                        }
                                        else if (nqm.QPar.Fitting == CommonFun.GetLanText("qube"))
                                        {
                                            /*this.smfrm.lblK1V.Enabled = true;
                                            this.smfrm.lblK2V.Enabled = true;
                                            this.smfrm.lblK3V.Enabled = true;
                                            this.smfrm.lblK1.Enabled = true;
                                            this.smfrm.lblK2.Enabled = true;
                                            this.smfrm.label6.Enabled = true;
                                            this.smfrm.label7.Enabled = true;
                                            this.smfrm.label9.Enabled = true;*/

                                            // this.smfrm.lblK20_.Enabled = false;
                                            this.smfrm.lblK21_.Enabled = true;
                                            this.smfrm.lblK22_.Enabled = true;
                                            this.smfrm.lblK23_.Enabled = true;

                                            if (nqm.QPar.Equation == "C=f(Abs)")
                                                this.smfrm.lblequation.Text = "C=K3*A^3+K2*A^2+K1*A+K0";
                                            else
                                                this.smfrm.lblequation.Text = "A=K3*C^3+K2*C^2+K1*C+K0";
                                        }
                                    }
                                    else if (nqm.QPar.CabMethod == CommonFun.GetLanText("measrues"))
                                    {
                                        if (nqm.Page == 1)
                                            this.smfrm.lblTitle.Text = CommonFun.GetLanText("quameas");
                                        else
                                            this.smfrm.lblTitle.Text = CommonFun.GetLanText("proteinmeas");
                                        this.smfrm.dataGridView1.Visible = true;
                                        this.smfrm.panel3.Visible = false;
                                        this.smfrm.lblequation.Visible = false;
                                        this.smfrm.dataGridView1.Columns[1].ReadOnly = true;
                                        this.smfrm.dataGridView1.Columns[2].ReadOnly = true;
                                        this.smfrm.GenerateNewSampleData();
                                    }
                                    else if (nqm.QPar.CabMethod == CommonFun.GetLanText("inputs"))
                                    {
                                        if (nqm.Page == 1)
                                            this.smfrm.lblTitle.Text = CommonFun.GetLanText("quainputs");
                                        else
                                            this.smfrm.lblTitle.Text = CommonFun.GetLanText("proteininputs");
                                        this.smfrm.dataGridView1.Visible = true;
                                        this.smfrm.panel3.Visible = false;
                                        this.smfrm.GenerateNewSampleData();
                                        this.smfrm.dataGridView1.Columns[1].ReadOnly = true;
                                    }
                                    this.smfrm.DrawCurve(nqm);
                                });

                                this.smfrm.btnBack.Click += ((param0_3, param1_3) => nqm = this.smfrm.QM);
                                this.smfrm.btnLast.Click += ((param0_3, param1_3) =>
                                {
                                    // nqm = this.smfrm.QM;
                                    this.smfrm.Close();
                                });
                                //this.smfrm.btnLast.Click += ((param0_3, param1_3) => nqm = this.smfrm.QM);
                                this.smfrm.btnOK.Click += ((param0_3, param1_3) =>
                                {
                                    if (this.smfrm.QM.QPar.CabMethod != CommonFun.GetLanText("inputr"))
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
                                                CommonFun.showbox(CommonFun.GetLanText("Incompletedata"), "Error");
                                                return;
                                            }
                                        }
                                    }
                                    if (this.smfrm.QM.QPar.Equation == "Abs=f(C)" && this.smfrm.QM.K0 == 0M && (this.smfrm.QM.K1 == 0M && this.smfrm.QM.K2 == 0M) && this.smfrm.QM.K3 == 0M)
                                        CommonFun.showbox(CommonFun.GetLanText("errorformula"), "Error");
                                    else if (this.smfrm.QM.QPar.Equation == "C=f(Abs)" && this.smfrm.QM.K10 == 0M && (this.smfrm.QM.K11 == 0M && this.smfrm.QM.K12 == 0M) && this.smfrm.QM.K13 == 0M)
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("errorformula"), "Error");
                                    }
                                    else
                                    {
                                        frm.QPar = this.smfrm.QM;
                                        frm.ShowQm();
                                        frm.btnOK.Enabled = true;
                                        frm.btnSave.Enabled = true;
                                        smfrm.Close();
                                        frm1.Close();
                                        CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("logupdateM"), false);
                                    }
                                });

                                this.smfrm.btnMeasure.Click += (EventHandler)((param0_3, param1_3) =>
                                {
                                    string errormsg = "";
                                    if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                                        CommonFun.showbox(errormsg, "Error");
                                    else if (!this.sp.IsOpen)
                                        CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                                    else if (this.smfrm.btnMeasure.Text == CommonFun.GetLanText("stopmeasure"))
                                    {
                                        CommonFun.WriteSendLine("DNA, Измерьте стандартный образец, нажмите, чтобы остановить измерение");
                                        this.ComSta = ComStatus.END;
                                        this.smfrm.btnMeasure.Text = CommonFun.GetLanText("measure");
                                        this.meacnt = 0;
                                        this.smfrm.progressBar1.Visible = false;
                                        this.smfrm.progressBar1.Value = 0;
                                        this.slotno = "";
                                        this.currslotno = 0;
                                        this.calormea = 0;
                                        this.setState();
                                        this.SubScan = false;
                                    }
                                    else if (this.smfrm.dataGridView1.Rows[this.smfrm.dataGridView1.SelectedCells[0].RowIndex].Tag == null)
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("movecursor"), "Error");
                                    }
                                    else
                                    {
                                        this.SubScan = true;
                                        this.calormea = 2;
                                        if (CommonFun.GetAppConfig("EightSlot") == "true")
                                        {
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
                                                    if (((IEnumerable<string>)this.slotno.Split(',')).Count<string>() > this.CS\u0024\u003C\u003E8__localsf5.nqm.SamList.Count<Sample>())
                          {
                                                        this.slotno = "";
                                                        CommonFun.showbox(CommonFun.GetLanText("dismatch"), "Warning");
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
                                            this.smfrm.btnMeasure.Text = CommonFun.GetLanText("stopmeasure");
                                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("measrues"), false);
                                        }
                                    }
                                });
                                this.smfrm.btnXL.Click += (EventHandler)((param0_3, param1_3) =>
                                {
                                    string errormsg = "";
                                    if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                                        CommonFun.showbox(errormsg, "Error");
                                    else if (!this.sp.IsOpen)
                                        CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                                    else if (this.ComSta == ComStatus.CALBGND)
                                    {
                                        CommonFun.WriteSendLine("DNA，Измерьте стандартный образец, нажмите, чтобы остановить калибровку");
                                        this.ComSta = ComStatus.END;
                                        this.smfrm.btnXL.Text = CommonFun.GetLanText("blanking");
                                        this.endcnt = 0;
                                        this.SubScan = false;
                                    }
                                    else if (this.ComSta != ComStatus.END)
                                    {
                                        CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
                                    }
                                    else
                                    {
                                        this.SubScan = true;
                                        this.calormea = 1;
                                        if (CommonFun.GetAppConfig("EightSlot") == "true")
                                        {
                                            this.subQM = nqm;
                                            /*using (SelectOneSlotFrm frm2 = new SelectOneSlotFrm())
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
                                            this.smfrm.btnXL.Text = CommonFun.GetLanText("stopblanking");
                                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("measrues") + "-" + CommonFun.GetLanText("blanking"), false);
                                        }
                                    }

                                });

                                this.smfrm.ShowDialog();
                            }

                        });
                        frm1.btnFinish.Click += ((param0_2, param1_2) =>
                        {
                            if (!frm1.lblMMV.Text.Contains("ДНК"))
                                return;
                            try
                            {
                                nqm.QPar.MeasureMethodName = frm1.lblMMV.Text;
                                nqm.QPar.MeasureMethodNameDM = frm1.lblMMV.Tag.ToString();
                                nqm.QPar.WL = !frm1.lblwl2.Enabled ? frm1.lblwl1.Text : frm1.lblwl1.Text + "," + frm1.lblwl2.Text;
                                nqm.QPar.R = frm1.lblRV.Text;
                                nqm.QPar.BackWL = !(frm1.pibBackWL.Tag.ToString() == "on") ? "" : frm1.lblwl3.Text;
                                nqm.QPar.Length = frm1.lblLengthV.Text;
                                nqm.QPar.EConvert = !(frm1.pibEConvert.Tag.ToString() == "off");
                                nqm.QPar.MCnt = (int)Convert.ToInt16(frm1.lblCntV.Text);
                                nqm.QPar.Limits = this.QM.QPar.Limits.Replace(CommonFun.GetLanText("maxv") + ":", "").Replace(CommonFun.GetLanText("minv") + ":", "");
                                nqm.QPar.Unit = frm1.lblUnitV.Text;
                                frm.QPar = nqm;
                                frm1.Close();
                                frm.btnOK.Enabled = true;
                                frm.btnSave.Enabled = true;
                                frm.ShowQm();
                                CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("logupdateM"), false);
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("parerror"), "Error");
                            }
                        });
                        frm1.ShowDialog();
                    }
                });

                frm.btnOK.Click += (EventHandler)((param0, param1) =>
                {
                    DateTime? dTime;
                    int num;
                    if (this.QM.DNAMeaList != null && this.QM.DNAMeaList.Count > 0)
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
                            CommonFun.showbox(CommonFun.GetLanText("fileunsave"), "Warning");
                            return;
                        }
                        dTime = this.QM.D_time;
                        if (!dTime.HasValue && this.QM.DNAMeaList != null && this.QM.DNAMeaList.Count != 0)
                        {
                            if (new DRMessageBoxFrm(CommonFun.GetLanText("unsavedataexit"), "Warning").ShowDialog() == DialogResult.No)
                                return;
                            if (frm.QPar.C_methodcreator == null || frm.QPar.C_methodcreator.Length <= 0)
                            {
                                if (CommonFun.GetAppConfig("GLPEnabled") == "true" && frm.QPar.MethodCreatorES == null)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("savemethodwithes"), "Error");
                                    return;
                                }
                                frm.QPar.C_methodcreator = CommonFun.GetAppConfig("currentuser");
                                frm.QPar.D_MTime = new DateTime?(DateTime.Now);
                            }
                            else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && (frm.QPar.MethodCreatorES == null || frm.QPar.MethodCreatorES.Length <= 0))
                            {
                                CommonFun.showbox(CommonFun.GetLanText("noesmethod"), "Error");
                                return;
                            }
                            this.QM = frm.QPar;
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("logupdateM"), false);
                            if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                            {
                                this.lblValue2.Visible = true;
                                this.lblUnit2.Visible = true;
                            }
                            else
                            {
                                this.lblValue2.Visible = false;
                                this.lblUnit2.Visible = false;
                            }
                            if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1"))
                            {
                                if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                    this.lblfor.Text = "C(DNA)=62.9*(A1-Aref)-36.0*(A2-Aref); C(Protein)=1552*(A2-Aref)-757.3*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                                else
                                    this.lblfor.Text = "C(DNA)=62.9*A1-36.0*A2; C(Protein)=1552*A2-757.3*A2; Ratio=A1/A2";
                            }
                            else if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                            {
                                if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                    this.lblfor.Text = "C(DNA)=49.1*(A1-Aref)-3.48*(A2-Aref); C(Protein)=183*(A2-Aref)-75.8*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                                else
                                    this.lblfor.Text = "C(DNA)=49.1*A1-3.48*A2; C(Protein)=183*A2-75.8*A2; Ratio=A1/A2";
                            }
                            else if (this.QM.QPar.Equation == "Abs=f(C)")
                                this.lblfor.Text = this.QM.AFCS;
                            else
                                this.lblfor.Text = this.QM.CFCS;
                            this.lblUnit1.Text = this.QM.QPar.Unit;
                            this.lblUnit2.Text = this.QM.QPar.Unit;
                            this.Cleardata();
                        }
                    }
                    else
                    {
                        if (frm.QPar.C_methodcreator == null || frm.QPar.C_methodcreator.Length <= 0)
                        {
                            if (CommonFun.GetAppConfig("GLPEnabled") == "true" && frm.QPar.MethodCreatorES == null)
                            {
                                CommonFun.showbox(CommonFun.GetLanText("savemethodwithes"), "Error");
                                return;
                            }
                            frm.QPar.C_methodcreator = CommonFun.GetAppConfig("currentuser");
                            frm.QPar.D_MTime = new DateTime?(DateTime.Now);
                        }
                        else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && (frm.QPar.MethodCreatorES == null || frm.QPar.MethodCreatorES.Length <= 0))
                        {
                            CommonFun.showbox(CommonFun.GetLanText("noesmethod"), "Error");
                            return;
                        }
                        this.QM = frm.QPar;
                        CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("logupdateM"), false);
                        if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                        {
                            this.lblValue2.Visible = true;
                            this.lblUnit2.Visible = true;
                        }
                        else
                        {
                            this.lblValue2.Visible = false;
                            this.lblUnit2.Visible = false;
                        }
                        if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1"))
                        {
                            if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                this.lblfor.Text = "C(DNA)=62.9*(A1-Aref)-36.0*(A2-Aref); C(Protein)=1552*(A2-Aref)-757.3*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                            else
                                this.lblfor.Text = "C(DNA)=62.9*A1-36.0*A2; C(Protein)=1552*A2-757.3*A2; Ratio=A1/A2";
                        }
                        else if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                        {
                            if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                this.lblfor.Text = "C(DNA)=49.1*(A1-Aref)-3.48*(A2-Aref); C(Protein)=183*(A2-Aref)-75.8*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                            else
                                this.lblfor.Text = "C(DNA)=49.1*A1-3.48*A2; C(Protein)=183*A2-75.8*A2; Ratio=A1/A2";
                        }
                        else if (this.QM.QPar.Equation == "Abs=f(C)")
                            this.lblfor.Text = this.QM.AFCS;
                        else
                            this.lblfor.Text = this.QM.CFCS;
                        this.lblUnit1.Text = this.QM.QPar.Unit;
                        this.lblUnit2.Text = this.QM.QPar.Unit;
                        this.Cleardata();
                    }
                    frm.Close();
                });
                frm.btnBack.Click += (EventHandler)((param0, param1) => frm.Close());
                frm.ShowDialog();
            }
        }

        private void btnBlank_Click(object sender, EventArgs e)
        {
            if (this.QM.C_methodcreator == null || this.QM.C_methodcreator.Length <= 0)
            {
                CommonFun.showbox(CommonFun.GetLanText("nomethod"), "Error");
            }
            else
            {
                string errormsg = "";
                /*if (CommonFun.GetAppConfig("RaceMode") == "true" && !DongleMgr.VerifyDongle(out errormsg, "5131AFFD", "DEA172BD99A88EDB"))
                    CommonFun.showbox(errormsg, "Error");
                else if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
                    CommonFun.showbox(errormsg, "Error");*/
                if (!this.sp.IsOpen)
                    CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                else if (this.ComSta == ComStatus.CALBGND)
                {
                    CommonFun.WriteSendLine("DNA，Нажмите, чтобы остановить обнуление");
                    this.ComSta = ComStatus.END;
                    this.btnBlank.Text = CommonFun.GetLanText("blanking");
                    this.progressBar1.Visible = false;
                    this.progressBar1.Value = 10;
                    this.setState();
                    this.meacnt = 0;
                }
                else if (this.ComSta != ComStatus.END)
                {
                    CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
                }
                else
                {
                    if (CommonFun.GetAppConfig("EightSlot") == "true")
                    {
                        this.calormea = 1;
                        /* using (SelectOneSlotFrm frm = new SelectOneSlotFrm())
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
                        this.btnBlank.Text = CommonFun.GetLanText("stopblanking");
                        CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("blanking"), false);
                    }
                }
            }

        }
        private void btnScan_Click(object sender, EventArgs e)
        {
            if (this.QM.C_methodcreator == null || this.QM.C_methodcreator.Length <= 0)
            {
                CommonFun.showbox(CommonFun.GetLanText("nomethod"), "Error");
            }
            else
            {
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
                if (this.QM.ESStatus > 0 && new DRMessageBoxFrm(CommonFun.GetLanText("esdatawarning"), "Warning").ShowDialog() == DialogResult.No)
                    return;
                if (!this.sp.IsOpen)
                    CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                else if (this.btnScan.Text == CommonFun.GetLanText("stopmeasure"))
                {
                    CommonFun.WriteSendLine("DNA，Нажмите, чтобы остановить измерение");
                    this.ComSta = ComStatus.END;
                    this.btnScan.Text = CommonFun.GetLanText("measure");
                    this.progressBar1.Visible = false;
                    this.progressBar1.Value = 10;
                    this.slotno = "";
                    this.currslotno = 0;
                    this.calormea = 0;
                    this.setState();
                    this.meacnt = 0;
                }
                else if (this.ComSta != ComStatus.END)
                {
                    CommonFun.showbox(CommonFun.GetLanText("waitforcmd"), "Warning");
                }
                else
                {
                    if (CommonFun.GetAppConfig("EightSlot") == "true")
                    {
                        this.calormea = 2;
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
                                frm.Close();
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
                        this.btnScan.Text = CommonFun.GetLanText("stopmeasure");
                        CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("measure"), false);
                    }
                }
            }
        }
        private void pibOut_Click(object sender, EventArgs e)
        {
            if (this.QM.QPar.MeasureMethodName.Contains("ДНК"))
            {
                if (this.lblValue2.Visible)
                {
                    this.lblValue1.Visible = false;
                    this.lblUnit1.Visible = false;
                    this.lblValue2.Visible = false;
                    this.lblUnit2.Visible = false;
                    this.panel3.Visible = false;
                    this.pibCurve.Visible = true;
                    this.btnScan.Enabled = false;
                    this.btnBack.Image = (System.Drawing.Image)Resources.UI_DB_Return;
                    this.DrawCurve(this.QM, this.pibCurve, false);
                }
                else
                {
                    this.lblValue1.Visible = true;
                    this.lblUnit1.Visible = true;
                    this.lblValue2.Visible = true;
                    this.lblUnit2.Visible = true;
                    this.panel3.Visible = true;
                    this.pibCurve.Visible = false;
                    this.btnScan.Enabled = true;
                    this.btnBack.Image = (System.Drawing.Image)Resources.Icon_Home;
                }
            }
            else if (this.lblValue1.Visible)
            {
                this.lblValue1.Visible = false;
                this.lblUnit1.Visible = false;
                this.panel3.Visible = false;
                this.pibCurve.Visible = true;
                this.btnScan.Enabled = false;
                this.btnBack.Image = (System.Drawing.Image)Resources.UI_DB_Return;
                this.DrawCurve(this.QM, this.pibCurve, false);
            }
            else
            {
                this.lblValue1.Visible = true;
                this.lblUnit1.Visible = true;
                this.panel3.Visible = true;
                this.pibCurve.Visible = false;
                this.btnScan.Enabled = true;
                this.btnBack.Image = (System.Drawing.Image)Resources.Icon_Home;
            }
        }
        private void setState()
        {
            switch (this.ComSta)
            {
                case ComStatus.MEASURE:
                    if (this.SubScan)
                    {
                        this.smfrm.btnBack.Enabled = false;
                        this.smfrm.btnLast.Enabled = false;
                        this.smfrm.btnMeasure.Enabled = true;
                        this.smfrm.btnOK.Enabled = false;
                        this.smfrm.btnXL.Enabled = false;
                        break;
                    }
                    this.btnScan.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = false;
                    this.btnBlank.Enabled = false;
                    //this.btnOperate.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnOpen.Enabled = false;
                    this.pibOut.Enabled = false;
                    break;
                case ComStatus.CALBGND:
                    if (this.SubScan)
                    {
                        this.smfrm.btnBack.Enabled = false;
                        this.smfrm.btnLast.Enabled = false;
                        this.smfrm.btnMeasure.Enabled = false;
                        this.smfrm.btnOK.Enabled = false;
                        this.smfrm.btnXL.Enabled = true;
                        break;
                    }
                    this.btnBlank.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = false;
                    this.btnScan.Enabled = false;
                    //this.btnOperate.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnOpen.Enabled = false;
                    this.pibOut.Enabled = false;
                    break;
                case ComStatus.END:
                    if (this.SubScan)
                    {
                        this.smfrm.btnBack.Enabled = true;
                        this.smfrm.btnLast.Enabled = true;
                        this.smfrm.btnMeasure.Enabled = true;
                        this.smfrm.btnOK.Enabled = true;
                        this.smfrm.btnXL.Enabled = true;
                        break;
                    }
                    this.btnScan.Enabled = true;
                    this.btnSet.Enabled = true;
                    this.btnBack.Enabled = true;
                    this.btnBlank.Enabled = true;
                    //this.btnOperate.Enabled = true;
                    this.btnSave.Enabled = true;
                    this.btnOpen.Enabled = true;
                    this.pibOut.Enabled = true;
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
                    case ComStatus.SETCHP:
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
                                    CommonFun.showbox(CommonFun.GetLanText("errorretry") + ex.ToString(), "Error");
                                    this.ComSta = ComStatus.END;
                                    this.currslotno = 0;
                                    this.slotno = "";
                                    this.calormea = 0;
                                    break;
                                }
                            case ComStatus.MEASURE:
                                CommonFun.WriteLine(text);
                                this.receive += text;
                                try
                                {
                                    System.Decimal num1;
                                    if (this.SubScan)
                                    {
                                        List<string> list = ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).ToList<string>();
                                        if (this.smfrm.QM.QPar.BackWL.Length > 0)
                                            list.Add(this.smfrm.QM.QPar.BackWL);
                                        if (text.Contains("END"))
                                        {
                                            if (this.meacnt < list.Count<string>())
                                            {
                                                this.ComSta = ComStatus.END;
                                                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(list[this.meacnt]) * 10M).ToString("f0") + "\r\n");
                                                CommonFun.WriteSendLine("DNA, Измерение стандартных образцов, measure 1 2 " + (Convert.ToDecimal(list[this.meacnt]) * 10M).ToString("f0"));
                                                this.ComSta = ComStatus.MEASURE;
                                                ++this.meacnt;
                                            }
                                            else
                                            {
                                                if (this.smfrm.QM.SamList == null)
                                                    this.smfrm.QM.SamList = new List<Sample>();
                                                this.receive = this.receive.Replace("*DAT", "&");
                                                string[] strArray1 = this.receive.Split('&');
                                                List<QuaMeaData> source = new List<QuaMeaData>();
                                                for (int index = 0; index < ((IEnumerable<string>)strArray1).Count<string>(); ++index)
                                                {
                                                    int num2 = strArray1[index].IndexOf("END");
                                                    if (num2 > 0)
                                                    {
                                                        this.receive = strArray1[index].Substring(1, num2 - 1);
                                                        string[] strArray2 = this.receive.Split(' ');
                                                        QuaMeaData quaMeaData = new QuaMeaData();
                                                        quaMeaData.Value = Convert.ToDouble(strArray2[0]) > 0.01 ? (2.0 - Math.Log10(Convert.ToDouble(strArray2[0]))).ToString(this.absacc) : 4.ToString(this.absacc);
                                                        if (this.smfrm.QM.QPar.Length != "10" && this.smfrm.QM.QPar.EConvert)
                                                            quaMeaData.Value = (Convert.ToDouble(quaMeaData.Value) * (Convert.ToDouble(10) / Convert.ToDouble(this.smfrm.QM.QPar.Length))).ToString(this.absacc);
                                                        quaMeaData.WL = (int)Convert.ToInt16(strArray2[2]);
                                                        quaMeaData.Slot = (int)Convert.ToInt16(strArray2[1]);
                                                        source.Add(quaMeaData);
                                                    }
                                                }
                                                int wl1 = (int)Convert.ToInt16(Convert.ToDecimal(this.smfrm.QM.QPar.WL) * 10M);
                                                num1 = 0M;
                                                System.Decimal num3;
                                                try
                                                {
                                                    num3 = Convert.ToDecimal(source.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)).ToList<QuaMeaData>()[0].Value);
                                                }
                                                catch
                                                {
                                                    int num2 = (int)MessageBox.Show(CommonFun.GetLanText("errordata"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                                    CommonFun.WriteSendLine("DNA，Ошибка измерения, стоп");
                                                    this.ComSta = ComStatus.END;
                                                    if (this.smfrm.btnMeasure.InvokeRequired)
                                                        this.smfrm.btnMeasure.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)true);
                                                    else
                                                        this.SetMealable(true);
                                                    this.receive = "";
                                                    this.mcnt = 0;
                                                    this.meacnt = 0;
                                                    this.currslotno = 0;
                                                    this.slotno = "";
                                                    this.calormea = 0;
                                                    this.SubScan = false;
                                                    return;
                                                }
                                                int rowIndex = this.smfrm.dataGridView1.SelectedCells[0].RowIndex;
                                                if (rowIndex >= 0 && rowIndex < this.smfrm.QM.SamList.Count<Sample>())
                                                {
                                                    this.smfrm.QM.SamList[rowIndex].XGD = new System.Decimal?(Convert.ToDecimal(num3.ToString(this.absacc)));
                                                    this.smfrm.QM.SamList[rowIndex].D_sj = new DateTime?(DateTime.Now);
                                                    if (this.lblValue1.InvokeRequired)
                                                        this.lblValue1.Invoke((Delegate)new DNAFrm.Del_BindData(this.LoadStandardData), (object)0);
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
                                                    this.slotno = "";
                                                    this.calormea = 0;
                                                    this.receive = "";
                                                    this.meacnt = 0;
                                                    this.ComSta = ComStatus.END;
                                                    if (this.smfrm.btnMeasure.InvokeRequired)
                                                        this.smfrm.btnMeasure.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)true);
                                                    else
                                                        this.SetMealable(true);
                                                    this.SubScan = false;
                                                }
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                    List<string> list1 = ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).ToList<string>();
                                    if (this.QM.QPar.BackWL.Length > 0)
                                        list1.Add(this.QM.QPar.BackWL);
                                    if (text.Contains("END"))
                                    {
                                        int vv = this.meacnt * 100 / list1.Count<string>();
                                        if (this.progressBar1.InvokeRequired)
                                            this.progressBar1.Invoke((Delegate)new DNAFrm.Del_SetProgressbar(this.SetProgressbarValue), (object)vv);
                                        else
                                            this.SetProgressbarValue(vv);
                                        if (this.meacnt < list1.Count<string>())
                                        {
                                            this.ComSta = ComStatus.END;
                                            this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(list1[this.meacnt]) * 10M).ToString("f0") + "\r\n");
                                            this.ComSta = ComStatus.MEASURE;
                                            CommonFun.WriteSendLine("DNA, Измерьте образец для тестирования, measure 1 2 " + (Convert.ToDecimal(list1[this.meacnt]) * 10M).ToString("f0"));
                                            ++this.meacnt;
                                        }
                                        else
                                        {
                                            this.receive = this.receive.Replace("*DAT", "&");
                                            string[] strArray1 = this.receive.Split('&');
                                            List<QuaMeaData> source = new List<QuaMeaData>();
                                            for (int index = 0; index < ((IEnumerable<string>)strArray1).Count<string>(); ++index)
                                            {
                                                int num2 = strArray1[index].IndexOf("END");
                                                if (num2 > 0)
                                                {
                                                    this.receive = strArray1[index].Substring(1, num2 - 1);
                                                    string[] strArray2 = this.receive.Split(' ');
                                                    QuaMeaData quaMeaData = new QuaMeaData();
                                                    quaMeaData.Value = Convert.ToDouble(strArray2[0]) > 0.01 ? (2.0 - Math.Log10(Convert.ToDouble(strArray2[0]))).ToString(this.absacc) : 4.ToString(this.absacc);
                                                    if (this.QM.QPar.Length != "10" && this.QM.QPar.EConvert)
                                                        quaMeaData.Value = (Convert.ToDouble(quaMeaData.Value) * (Convert.ToDouble(10) / Convert.ToDouble(this.QM.QPar.Length))).ToString(this.absacc);
                                                    quaMeaData.WL = (int)Convert.ToInt16(strArray2[2]);
                                                    quaMeaData.Slot = (int)Convert.ToInt16(strArray2[1]);
                                                    source.Add(quaMeaData);
                                                }
                                            }
                                            if (this.QM.DNAMeaList == null)
                                                this.QM.DNAMeaList = new List<DNAMeasureData>();
                                            int esStatus1 = this.QM.ESStatus;
                                            if (this.QM.ESStatus > 0)
                                            {
                                                this.QM.DNAMeaList = new List<DNAMeasureData>();
                                                this.QM.ESStatus = 0;
                                                this.QM.C_name = "";
                                                this.QM.D_time = new DateTime?();
                                            }
                                            else
                                            {
                                                DateTime? nullable1 = this.QM.D_time;
                                                if (nullable1.HasValue && this.QM.DNAMeaList.Count > 0)
                                                {
                                                    int esStatus2 = this.QM.ESStatus;
                                                    if (this.QM.ESStatus > 0)
                                                    {
                                                        this.QM.DNAMeaList = new List<DNAMeasureData>();
                                                        this.QM.ESStatus = 0;
                                                        this.QM.C_name = "";
                                                    }
                                                    else if (CommonFun.GetAppConfig("currentuser") != this.QM.C_operator)
                                                    {
                                                        this.QM.DNAMeaList = new List<DNAMeasureData>();
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
                                            if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                                            {
                                                int wl1 = (int)Convert.ToInt16(Convert.ToDecimal(list1[0]) * 10M);
                                                int wl2 = (int)Convert.ToInt16(Convert.ToDecimal(list1[1]) * 10M);
                                                num1 = 0M;
                                                System.Decimal num2;
                                                try
                                                {
                                                    num2 = Convert.ToDecimal(source.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl1)).ToList<QuaMeaData>()[0].Value);
                                                }
                                                catch
                                                {
                                                    num2 = 99999.99M;
                                                }
                                                System.Decimal num3;
                                                try
                                                {
                                                    num3 = Convert.ToDecimal(source.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl2)).ToList<QuaMeaData>()[0].Value);
                                                }
                                                catch
                                                {
                                                    num3 = 99999.99M;
                                                }
                                                System.Decimal num4 = 0M;
                                                if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                                {
                                                    int wlb = (int)Convert.ToInt16(Convert.ToDecimal(list1[2]) * 10M);
                                                    try
                                                    {
                                                        num4 = Convert.ToDecimal(source.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wlb)).ToList<QuaMeaData>()[0].Value);
                                                    }
                                                    catch
                                                    {
                                                        num4 = 99999.99M;
                                                    }
                                                }
                                                string[] strArray2 = this.QM.QPar.R.Split(',');
                                                this.sslive = new DNAMeasureData();
                                                if (this.QM.C_head != null && this.QM.C_head.Length > 0)
                                                    this.sslive.C_bz = this.QM.C_head + "-" + (this.QM.DNAMeaList.Count<DNAMeasureData>() + 1).ToString();
                                                this.sslive.A = new List<System.Decimal>();
                                                this.sslive.A.Add(num2);
                                                this.sslive.A.Add(num3);
                                                if (num2 != 99999.99M && num3 != 99999.99M && num4 != 99999.99M)
                                                {
                                                    this.sslive.DNA = Convert.ToDecimal(strArray2[0]) * (num2 - num4) - Convert.ToDecimal(strArray2[1]) * (num3 - num4);
                                                    this.sslive.Protein = Convert.ToDecimal(strArray2[2]) * (num3 - num4) - Convert.ToDecimal(strArray2[3]) * (num2 - num4);
                                                    this.sslive.Ratio = !(num3 - num4 != 0M) ? 0M : (num2 - num4) / (num3 - num4);
                                                }
                                                else
                                                {
                                                    this.sslive.DNA = 99999.99M;
                                                    this.sslive.Protein = 99999.99M;
                                                    this.sslive.Ratio = 99999.99M;
                                                }
                                                this.sslive.D_sj = DateTime.Now;
                                                this.QM.DNAMeaList.Add(this.sslive);
                                            }
                                            else
                                            {
                                                int wl11 = (int)Convert.ToInt16(Convert.ToDecimal(this.QM.QPar.WL) * 10M);
                                                System.Decimal num2;
                                                try
                                                {
                                                    num2 = Convert.ToDecimal(source.Where<QuaMeaData>((Func<QuaMeaData, bool>)(s => s.WL == wl11)).ToList<QuaMeaData>()[0].Value);
                                                }
                                                catch
                                                {
                                                    num2 = 99999.99M;
                                                }
                                                this.sslive = new DNAMeasureData();
                                                if (this.QM.C_head != null && this.QM.C_head.Length > 0)
                                                    this.sslive.C_bz = this.QM.C_head + "-" + (this.QM.DNAMeaList.Count<DNAMeasureData>() + 1).ToString();
                                                this.sslive.XGD = num2;
                                                if (num2 != 99999.99M)
                                                {
                                                    this.sslive.ND = this.QM.K13 * this.sslive.XGD * this.sslive.XGD * this.sslive.XGD + this.QM.K12 * this.sslive.XGD * this.sslive.XGD + this.QM.K11 * this.sslive.XGD + this.QM.K10;
                                                    this.sslive.ND = Convert.ToDecimal(this.sslive.ND.ToString(this.conacc));
                                                }
                                                else
                                                    this.sslive.ND = 99999.99M;
                                                this.sslive.D_sj = DateTime.Now;
                                                this.QM.DNAMeaList.Add(this.sslive);
                                            }
                                            this.currindex = this.QM.DNAMeaList.Count - 1;
                                            if (this.lblValue1.InvokeRequired)
                                                this.lblValue1.Invoke((Delegate)new DNAFrm.Del_BindData(this.LoadMeaData), (object)(this.QM.DNAMeaList.Count - 1));
                                            else
                                                this.LoadMeaData(this.QM.DNAMeaList.Count - 1);
                                            if (this.mcnt < this.QM.QPar.MCnt)
                                            {
                                                this.meacnt = 0;
                                                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(list1[this.meacnt]) * 10M).ToString("f0") + "\r\n");
                                                this.ComSta = ComStatus.MEASURE;
                                                CommonFun.WriteSendLine("DNA, Измерьте образец для тестирования, measure 1 2 " + (Convert.ToDecimal(list1[this.meacnt]) * 10M).ToString("f0"));
                                                ++this.meacnt;
                                                ++this.mcnt;
                                            }
                                            else
                                            {
                                                int num2;
                                                if (this.slotno.Length > 0)
                                                    num2 = this.currslotno >= ((IEnumerable<string>)this.slotno.Split(',')).Count<string>() ? 1 : 0;
                                                else
                                                    num2 = 1;
                                                if (num2 == 0)
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
                                                    if (this.btnScan.InvokeRequired)
                                                        this.btnScan.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)false);
                                                    else
                                                        this.SetMealable(false);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("errorstopmeasure") + ex.ToString(), "Error");
                                    CommonFun.WriteSendLine("DNA, Погрешность измерения, " + ex.ToString());
                                    this.ComSta = ComStatus.END;
                                    if (this.SubScan)
                                    {
                                        if (this.smfrm.btnMeasure.InvokeRequired)
                                            this.smfrm.btnMeasure.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)true);
                                        else
                                            this.SetMealable(true);
                                    }
                                    else if (this.btnScan.InvokeRequired)
                                        this.btnScan.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)false);
                                    else
                                        this.SetMealable(false);
                                    this.currslotno = 0;
                                    this.slotno = "";
                                    this.calormea = 0;
                                    this.receive = "";
                                    this.mcnt = 0;
                                    this.meacnt = 0;
                                    this.SubScan = false;
                                    break;
                                }
                            case ComStatus.CALBGND:
                                CommonFun.WriteLine(text);
                                try
                                {
                                    if (text.Contains("END"))
                                    {
                                        if (this.SubScan)
                                        {
                                            int num1 = 0;
                                            if (this.smfrm.QM.QPar.BackWL.Length > 0)
                                                ++num1;
                                            int num2 = num1 + ((IEnumerable<string>)this.smfrm.QM.QPar.WL.Split(',')).Count<string>();
                                            ++this.endcnt;
                                            if (this.endcnt == num2)
                                            {
                                                this.ComSta = ComStatus.END;
                                                if (this.smfrm.btnXL.InvokeRequired)
                                                    this.smfrm.btnXL.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)true);
                                                else
                                                    this.Setblanklable(true);
                                                this.endcnt = 0;
                                                this.SubScan = false;
                                                CommonFun.showbox(CommonFun.GetLanText("blankfinish"), "Information");
                                            }
                                        }
                                        else
                                        {
                                            int num1 = 0;
                                            if (this.QM.QPar.BackWL.Length > 0)
                                                ++num1;
                                            int num2 = num1 + ((IEnumerable<string>)this.QM.QPar.WL.Split(',')).Count<string>();
                                            ++this.endcnt;
                                            int vv = this.endcnt * 100 / num2;
                                            if (this.progressBar1.InvokeRequired)
                                                this.progressBar1.Invoke((Delegate)new DNAFrm.Del_SetProgressbar(this.SetProgressbarValue), (object)vv);
                                            else
                                                this.SetProgressbarValue(vv);
                                            if (this.endcnt == num2)
                                            {
                                                this.ComSta = ComStatus.END;
                                                if (this.btnBlank.InvokeRequired)
                                                    this.btnBlank.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)false);
                                                else
                                                    this.Setblanklable(false);
                                                this.endcnt = 0;
                                                CommonFun.showbox(CommonFun.GetLanText("blankfinish"), "Information");
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("errorstopblank") + ex.ToString(), "Error");
                                    CommonFun.WriteSendLine("DNA, Ошибка обнуления");
                                    this.ComSta = ComStatus.END;
                                    if (this.SubScan)
                                    {
                                        if (this.smfrm.btnXL.InvokeRequired)
                                            this.smfrm.btnXL.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)true);
                                        else
                                            this.Setblanklable(true);
                                    }
                                    else if (this.btnBlank.InvokeRequired)
                                        this.btnBlank.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)false);
                                    else
                                        this.Setblanklable(false);
                                    this.receive = "";
                                    this.SubScan = false;
                                    this.endcnt = 0;
                                    this.currslotno = 0;
                                    this.slotno = "";
                                    this.calormea = 0;
                                    break;
                                }
                            case ComStatus.BD_RATIO_FLUSH:
                                if (text.Contains("RCVD"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (this.btnBlank.InvokeRequired)
                                        this.btnBlank.Invoke((Delegate)new DNAFrm.Del_setstate(this.setstate), (object)true);
                                    else
                                        this.setstate(true);
                                    if (this.btnBlank.InvokeRequired)
                                        this.btnBlank.Invoke((Delegate)new DNAFrm.Del_starttt(this.Starttt), (object)false);
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

        private void LoadMeaData(int index)
        {
            if (index < 0)
                return;
            if (this.QM.DNAMeaList == null || this.QM.DNAMeaList.Count == 0)
            {
                this.lblNo.Text = "0001";
            }
            else
            {
                this.lblNo.Text = (index + 1).ToString("D4") + "/" + this.QM.DNAMeaList.Count.ToString("D4");
                if (index > this.QM.DNAMeaList.Count)
                    index = this.QM.DNAMeaList.Count<DNAMeasureData>();
                this.lbltimeV.Text = this.QM.DNAMeaList[index].D_sj.ToString();
                this.lblremarkV.Text = this.QM.DNAMeaList[index].C_bz;
                if (this.QM.QPar.MeasureMethodName.Contains("ДНК"))
                {
                    this.lblValue1.Visible = true;
                    this.lblValue2.Visible = true;
                    this.lblUnit1.Visible = true;
                    this.lblUnit2.Visible = true;
                    System.Decimal num;
                    if (this.QM.DNAMeaList[index].DNA != 99999.99M)
                    {
                        Label lblValue1 = this.lblValue1;
                        num = this.QM.DNAMeaList[index].DNA;
                        string str = num.ToString(this.conacc);
                        lblValue1.Text = str;
                    }
                    else
                        this.lblValue1.Text = "---";
                    if (this.QM.DNAMeaList[index].Protein != 99999.99M)
                    {
                        Label lblValue2 = this.lblValue2;
                        num = this.QM.DNAMeaList[index].Protein;
                        string str = num.ToString(this.conacc);
                        lblValue2.Text = str;
                    }
                    else
                        this.lblValue2.Text = "---";
                    this.lblA1.Text = "A1\r\n" + this.QM.QPar.WL.Split(',')[0];
                    this.lblA2.Text = "A2\r\n" + this.QM.QPar.WL.Split(',')[1];
                    if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                        this.lblA3.Text = "Aref\r\n" + this.QM.QPar.BackWL;
                    else
                        this.lblA3.Text = "Aref";
                    this.lblA4.Text = "Ratio";
                    if (this.QM.DNAMeaList[index].A[0] != 99999.99M)
                    {
                        Label lblA1V = this.lblA1V;
                        num = this.QM.DNAMeaList[index].A[0];
                        string str = num.ToString(this.absacc);
                        lblA1V.Text = str;
                    }
                    else
                        this.lblA1V.Text = "---";
                    if (this.QM.DNAMeaList[index].A[1] != 99999.99M)
                    {
                        Label lblA2V = this.lblA2V;
                        num = this.QM.DNAMeaList[index].A[1];
                        string str = num.ToString(this.absacc);
                        lblA2V.Text = str;
                    }
                    else
                        this.lblA2V.Text = "---";
                    if (this.QM.QPar.BackWL.Length > 0)
                    {
                        Label lblA3V = this.lblA3V;
                        num = this.QM.DNAMeaList[index].ABack;
                        string str = num.ToString(this.absacc);
                        lblA3V.Text = str;
                    }
                    else
                        this.lblA3V.Text = " ---";
                    Label lblA4V = this.lblA4V;
                    num = this.QM.DNAMeaList[index].Ratio;
                    string str1 = num.ToString(this.conacc);
                    lblA4V.Text = str1;
                }
                else
                {
                    this.lblValue1.Visible = true;
                    this.lblValue2.Visible = false;
                    this.lblUnit1.Visible = true;
                    this.lblUnit2.Visible = false;
                    System.Decimal num;
                    if (this.QM.DNAMeaList[index].ND != -1M)
                    {
                        Label lblValue1 = this.lblValue1;
                        num = this.QM.DNAMeaList[index].ND;
                        string str = num.ToString();
                        lblValue1.Text = str;
                    }
                    else
                        this.lblValue1.Text = "----";
                    this.lblA1.Text = "A1\r\n" + this.QM.QPar.WL;
                    Label lblA1V = this.lblA1V;
                    num = this.QM.DNAMeaList[index].XGD;
                    string str1 = num.ToString(this.absacc) ?? "";
                    lblA1V.Text = str1;
                    this.lblA2.Text = "";
                    this.lblA3.Text = "";
                    this.lblA4.Text = "";
                    this.lblA2V.Text = "";
                    this.lblA3V.Text = "";
                    this.lblA4V.Text = "";
                }
            }
        }

        private void LoadStandardData(int index)
        {
            int num1 = 0;
            if (this.smfrm.dataGridView1.SelectedCells != null && this.smfrm.dataGridView1.SelectedCells.Count > 0)
                num1 = this.smfrm.dataGridView1.SelectedCells[0].RowIndex;
            this.smfrm.dataGridView1.Rows.Clear();
            for (int index1 = 0; index1 < this.smfrm.QM.SamList.Count; ++index1)
            {
                this.smfrm.dataGridView1.Rows.Add();
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
                if (this.smfrm.QM.SamList[index1].IsExclude)
                {
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Value = (object)Resources.UI_DB_Check_Checked;
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Tag = (object)"on";
                }
                else
                {
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Value = (object)Resources.UI_DB_Check_Unchecked;
                    this.smfrm.dataGridView1.Rows[index1].Cells["ColPC"].Tag = (object)"off";
                }
                this.smfrm.dataGridView1.Rows[index1].Tag = (object)this.smfrm.QM.SamList[index1];
            }
            if (this.smfrm.QM.SamList.Count < this.smfrm.dgvcnt)
                this.smfrm.dataGridView1.Rows.Add(this.smfrm.dgvcnt - this.smfrm.QM.SamList.Count);
            if (num1 + 1 > this.smfrm.dataGridView1.Rows.Count)
                return;
            this.smfrm.dataGridView1.Rows[num1 + 1].Selected = true;
        }

        private void Setblanklable(bool subscan)
        {
            if (subscan)
            {
                this.smfrm.progressBar1.Value = 100;
                this.smfrm.progressBar1.Visible = false;
                this.smfrm.btnXL.Text = CommonFun.GetLanText("blanking");
                this.setState();
            }
            else
            {
                this.btnBlank.Text = CommonFun.GetLanText("blanking");
                this.panel4.Visible = false;
                this.progressBar1.Value = 10;
                this.setState();
            }
        }
        private void SetMealable(bool subscan)
        {
            if (subscan)
            {
                this.smfrm.progressBar1.Value = 100;
                this.smfrm.progressBar1.Visible = false;
                this.smfrm.btnMeasure.Text = CommonFun.GetLanText("measure");
                this.setState();
            }
            else
            {
                this.btnScan.Text = CommonFun.GetLanText("measure");
                this.panel4.Visible = false;
                this.progressBar1.Value = 10;
                this.setState();
            }
        }
        private void SetProgressbarValue(int vv)
        {
            if (vv > 100)
                vv = 100;
            this.progressBar1.Value = vv;
        }

        private void DrawCurve(QuaMethod qm, PictureBox pic, bool isout)
        {
            int num1 = 10;
            if (isout)
                num1 = 8;
            float num2 = 0.0f;
            float num3 = 0.0f;
            float num4 = 0.0f;
            float num5 = 0.0f;
            new StringFormat().FormatFlags = StringFormatFlags.DirectionVertical;
            int num6 = 0;
            int num7 = 20;
            float num8;
            float num9;
            if (qm.DNAMeaList == null || qm.DNAMeaList.Count <= 0)
            {
                num8 = 0.0f;
                num9 = 20f;
            }
            else
            {
                if (qm.DNAMeaList.Count > 20)
                {
                    List<int> source = new List<int>();
                    for (int index = 5; index < 1000; ++index)
                    {
                        int num10 = 4 * index - qm.DNAMeaList.Count;
                        source.Add(num10);
                    }
                    int num11 = source.Where<int>((Func<int, bool>)(s => s > 0)).Min();
                    num7 = qm.DNAMeaList.Count<DNAMeasureData>() + num11;
                }
                int num12;
                if (qm.QPar.Limits.Length > 0)
                    num12 = ((IEnumerable<string>)qm.QPar.Limits.Split(',')).Count<string>() <= 1 ? 1 : 0;
                else
                    num12 = 1;
                if (num12 == 0)
                {
                    num4 = (float)Convert.ToDouble(qm.QPar.Limits.Split(',')[0]);
                    num5 = (float)Convert.ToDouble(qm.QPar.Limits.Split(',')[1]);
                }
                if (qm.DNAMeaList != null && qm.DNAMeaList.Count > 0)
                {
                    num3 = (float)qm.DNAMeaList.Select<DNAMeasureData, System.Decimal>((Func<DNAMeasureData, System.Decimal>)(s => s.ND)).Min();
                    num2 = (float)qm.DNAMeaList.Select<DNAMeasureData, System.Decimal>((Func<DNAMeasureData, System.Decimal>)(s => s.ND)).Max();
                    if ((double)num4 != 0.0 && (double)num5 != 0.0)
                    {
                        if ((double)num3 > (double)num5)
                            num3 = num5;
                        if ((double)num2 < (double)num4)
                            num2 = num4;
                    }
                }
                if ((double)num2 == (double)num3)
                {
                    num8 = -10f;
                    num9 = 10f;
                }
                else
                {
                    num8 = num3 - (float)(((double)num2 - (double)num3) / 8.0);
                    num9 = num2 + (float)(((double)num2 - (double)num3) / 8.0);
                }
            }
            Bitmap bitmap = new Bitmap(pic.Width, pic.Height);
            Graphics graphics = Graphics.FromImage((System.Drawing.Image)bitmap);
            if (isout)
            {
                graphics.FillRectangle((Brush)new SolidBrush(Color.White), 0, 0, pic.Width, pic.Height);
            }
            else
            {
                graphics.DrawRectangle(new Pen(Color.Black, 1f), 0, 0, pic.Width - 1, pic.Height - 1);
                graphics.FillRectangle((Brush)new SolidBrush(Color.White), 1, 1, pic.Width - 2, pic.Height - 2);
            }
            SizeF sizeF1;
            SizeF sizeF2;
            if (qm.DNAMeaList == null || qm.DNAMeaList.Count <= 0)
            {
                sizeF1 = graphics.MeasureString(num7.ToString(), new System.Drawing.Font("Segoe UI", (float)num1));
                sizeF2 = graphics.MeasureString("0.0001", new System.Drawing.Font("Segoe UI", (float)num1));
            }
            else
            {
                sizeF1 = graphics.MeasureString(num7.ToString(), new System.Drawing.Font("Segoe UI", (float)num1));
                sizeF2 = graphics.MeasureString(Convert.ToDouble(qm.DNAMeaList[qm.DNAMeaList.Count - 1].ND).ToString(this.absacc), new System.Drawing.Font("Segoe UI", (float)num1));
            }
            float num13;
            float num14;
            float num15;
            float num16;
            if (isout)
            {
                num13 = (float)((double)sizeF2.Height + (double)sizeF2.Width + 5.0);
                num14 = (float)(this.picOut.Width - 2);
                num15 = (float)((double)this.picOut.Height - (double)sizeF1.Height * 2.0 - 5.0);
                num16 = sizeF1.Height;
            }
            else
            {
                num13 = 20f + sizeF2.Height + sizeF2.Width;
                num14 = (float)pic.Width - sizeF2.Width;
                num15 = (float)((double)pic.Height - (double)sizeF1.Height * 2.0 - 20.0);
                num16 = sizeF1.Height + 20f;
            }
            RectangleF rectangleF = new RectangleF(num13, num16, num14 - num13, num15 - num16);
            graphics.DrawLine(new Pen(Color.Black, 1f), num13, num15, num14, num15);
            graphics.DrawLine(new Pen(Color.Black, 1f), num13, num16, num14, num16);
            graphics.DrawLine(new Pen(Color.Black, 1f), num13, num15, num13, num16);
            graphics.DrawLine(new Pen(Color.Black, 1f), num14, num15, num14, num16);
            float x1 = num13;
            float y1 = num15 + 5f;
            graphics.DrawString(num6.ToString("f0"), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x1, y1));
            SizeF sizeF3 = graphics.MeasureString(num7.ToString("f0"), new System.Drawing.Font("Segoe UI", (float)num1));
            float x2 = num14 - sizeF3.Width;
            graphics.DrawString(num7.ToString("f0"), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x2, y1));
            SizeF sizeF4 = graphics.MeasureString(num8.ToString(this.absacc), new System.Drawing.Font("Segoe UI", (float)num1));
            float x3 = num13 - sizeF4.Width;
            float y2 = num15 - sizeF4.Height / 2f;
            graphics.DrawString(num8.ToString(this.absacc), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x3, y2));
            SizeF sizeF5 = graphics.MeasureString(num9.ToString(this.absacc), new System.Drawing.Font("Segoe UI", (float)num1));
            float x4 = num13 - sizeF5.Width;
            float y3 = num16 - sizeF5.Height / 2f;
            graphics.DrawString(num9.ToString(this.absacc), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x4, y3));
            for (int index = 1; index < 4; ++index)
            {
                Pen pen = new Pen(Color.Black, 1f);
                pen.DashStyle = DashStyle.Dot;
                graphics.DrawLine(pen, num13 + (float)(((double)num14 - (double)num13) * (double)index / 4.0), num15, num13 + (float)(((double)num14 - (double)num13) * (double)index / 4.0), num16);
                graphics.DrawLine(pen, num13, num16 + (float)(((double)num15 - (double)num16) * (double)index / 4.0), num14, num16 + (float)(((double)num15 - (double)num16) * (double)index / 4.0));
            }
            float x5 = num13 + (float)(((double)num14 - (double)num13 - (double)sizeF1.Width) / 2.0);
            float y4 = num15 + 5f;
            float y5 = num16 + (float)(((double)num15 - (double)num16 - (double)sizeF2.Width) / 2.0);
            if (!qm.QPar.MeasureMethodName.Contains("ДНК"))
            {
                float x6 = num13 - graphics.MeasureString(qm.QPar.Unit, new System.Drawing.Font("Segoe UI", (float)num1)).Width;
                graphics.DrawString(qm.QPar.Unit, new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
            }
            graphics.DrawString(CommonFun.GetLanText("number"), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
            if (num7 - num6 == 0 || (double)num9 - (double)num8 == 0.0)
                return;
            float num17 = (num14 - num13) / (float)(num7 - num6);
            float num18 = (float)(((double)num15 - (double)num16) / ((double)num9 - (double)num8));
            if ((double)num4 != 0.0 && (double)num5 != 0.0)
            {
                graphics.DrawLine(new Pen(Color.Red, 1f), num13, num15 - (num4 - num8) * num18, num14, num15 - (num4 - num8) * num18);
                graphics.DrawLine(new Pen(Color.Orange, 1f), num13, num15 - (num5 - num8) * num18, num14, num15 - (num5 - num8) * num18);
            }
            if (qm.DNAMeaList != null && qm.DNAMeaList.Count > 0)
            {
                if (qm.QPar.MeasureMethodName.Contains("ДНК"))
                {
                    for (int index = 0; index < qm.DNAMeaList.Count; ++index)
                    {
                        double num10 = (double)num13 + (double)(index + 1 - num6) * (double)num17;
                        double num11 = Convert.ToDouble(qm.DNAMeaList[index].Ratio) >= (double)num8 ? (Convert.ToDouble(qm.DNAMeaList[index].Ratio) <= (double)num9 ? (double)num15 - (Convert.ToDouble(qm.DNAMeaList[index].Ratio) - (double)num8) * (double)num18 : (double)num16) : (double)num15;
                        if ((double)num4 != 0.0 && (double)num5 != 0.0)
                        {
                            if ((double)(float)qm.DNAMeaList[index].Ratio < (double)num5)
                                graphics.DrawEllipse(new Pen(Color.Olive, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                            else if ((double)(float)qm.DNAMeaList[index].Ratio > (double)num4)
                                graphics.DrawEllipse(new Pen(Color.Red, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                            else
                                graphics.DrawEllipse(new Pen(Color.Black, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                        }
                        else
                            graphics.DrawEllipse(new Pen(Color.Black, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                    }
                }
                else
                {
                    for (int index = 0; index < qm.DNAMeaList.Count; ++index)
                    {
                        double num10 = (double)num13 + (double)(index + 1 - num6) * (double)num17;
                        double num11 = Convert.ToDouble(qm.DNAMeaList[index].ND) >= (double)num8 ? (Convert.ToDouble(qm.DNAMeaList[index].ND) <= (double)num9 ? (double)num15 - (Convert.ToDouble(qm.DNAMeaList[index].ND) - (double)num8) * (double)num18 : (double)num16) : (double)num15;
                        if ((double)num4 != 0.0 && (double)num5 != 0.0)
                        {
                            if ((double)(float)qm.DNAMeaList[index].ND < (double)num5)
                                graphics.DrawEllipse(new Pen(Color.Olive, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                            else if ((double)(float)qm.DNAMeaList[index].ND > (double)num4)
                                graphics.DrawEllipse(new Pen(Color.Red, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                            else
                                graphics.DrawEllipse(new Pen(Color.Black, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                        }
                        else
                            graphics.DrawEllipse(new Pen(Color.Black, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                    }
                }
            }
            pic.Image = (System.Drawing.Image)bitmap;
        }

        private void DrawStandardLine(QuaMethod qm)
        {
            this.picOut.Height = 240;
            this.picOut.Width = 320;
            string str;
            string format1;
            string format2;
            if (qm.QPar.Equation == "C=f(Abs)")
            {
                str = "XGD";
                format1 = this.absacc;
                format2 = this.conacc;
            }
            else
            {
                str = "ND";
                format1 = this.conacc;
                format2 = this.absacc;
            }
            int num1 = 7;
            float num2 = 0.0f;
            float num3 = 0.0f;
            List<Sample> source = new List<Sample>();
            float num4;
            float num5;
            float num6;
            float num7;
            if (qm.SamList != null && qm.SamList.Count > 0)
            {
                System.Decimal? nullable1;
                System.Decimal? nullable2;
                float num8;
                float num9;
                if (str == "ND")
                {
                    for (int index = 0; index < qm.SamList.Count; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.ND = qm.SamList[index].ND;
                        Sample sample2 = sample1;
                        System.Decimal k3 = qm.K3;
                        nullable1 = sample1.ND;
                        System.Decimal? nullable3;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable3 = nullable2;
                        }
                        else
                            nullable3 = new System.Decimal?(k3 * nullable1.GetValueOrDefault());
                        nullable1 = nullable3;
                        nullable2 = sample1.ND;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable2 = sample1.ND;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k2 = qm.K2;
                        nullable2 = sample1.ND;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k2 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal? nd = sample1.ND;
                        nullable2 = nullable2.HasValue & nd.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * nd.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k1 = qm.K1;
                        nullable2 = sample1.ND;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k1 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k0 = qm.K0;
                        System.Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new System.Decimal?(nullable1.GetValueOrDefault() + k0);
                        sample2.XGD = nullable4;
                        source.Add(sample1);
                    }
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Min();
                    num8 = num3 = (float)nullable1.Value;
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Max();
                    num9 = num2 = (float)nullable1.Value;
                }
                else
                {
                    for (int index = 0; index < qm.SamList.Count; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.XGD = qm.SamList[index].XGD;
                        Sample sample2 = sample1;
                        System.Decimal k13 = qm.K13;
                        nullable1 = sample1.XGD;
                        System.Decimal? nullable3;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable3 = nullable2;
                        }
                        else
                            nullable3 = new System.Decimal?(k13 * nullable1.GetValueOrDefault());
                        nullable1 = nullable3;
                        nullable2 = sample1.XGD;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable2 = sample1.XGD;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k12 = qm.K12;
                        nullable2 = sample1.XGD;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k12 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal? xgd = sample1.XGD;
                        nullable2 = nullable2.HasValue & xgd.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * xgd.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k11 = qm.K11;
                        nullable2 = sample1.XGD;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k11 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k10 = qm.K10;
                        System.Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new System.Decimal?(nullable1.GetValueOrDefault() + k10);
                        sample2.ND = nullable4;
                        source.Add(sample1);
                    }
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Min();
                    num8 = num3 = (float)nullable1.Value;
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Max();
                    num9 = num2 = (float)nullable1.Value;
                }
                num4 = (double)num8 >= 0.0 ? num8 * 0.8f : num8 * 1.2f;
                num5 = (double)num9 >= 0.0 ? num9 * 1.2f : num9 * 0.8f;
                if (str == "ND")
                {
                    Sample sample1 = new Sample();
                    sample1.ND = new System.Decimal?(Convert.ToDecimal(num4));
                    Sample sample2 = sample1;
                    System.Decimal k3_1 = qm.K3;
                    nullable1 = sample1.ND;
                    System.Decimal? nullable3;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable3 = nullable2;
                    }
                    else
                        nullable3 = new System.Decimal?(k3_1 * nullable1.GetValueOrDefault());
                    nullable1 = nullable3;
                    nullable2 = sample1.ND;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable2 = sample1.ND;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k2_1 = qm.K2;
                    nullable2 = sample1.ND;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k2_1 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal? nd1 = sample1.ND;
                    nullable2 = nullable2.HasValue & nd1.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * nd1.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k1_1 = qm.K1;
                    nullable2 = sample1.ND;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k1_1 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k0 = qm.K0;
                    System.Decimal? nullable4;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable4 = nullable2;
                    }
                    else
                        nullable4 = new System.Decimal?(nullable1.GetValueOrDefault() + k0);
                    sample2.XGD = nullable4;
                    source.Add(sample1);
                    Sample sample3 = new Sample();
                    sample3.ND = new System.Decimal?(Convert.ToDecimal(num5));
                    Sample sample4 = sample3;
                    System.Decimal k3_2 = qm.K3;
                    nullable1 = sample3.ND;
                    System.Decimal? nullable5;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable5 = nullable2;
                    }
                    else
                        nullable5 = new System.Decimal?(k3_2 * nullable1.GetValueOrDefault());
                    nullable1 = nullable5;
                    nullable2 = sample3.ND;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable2 = sample3.ND;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k2_2 = qm.K2;
                    nullable2 = sample3.ND;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k2_2 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal? nd2 = sample3.ND;
                    nullable2 = nullable2.HasValue & nd2.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * nd2.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k1_2 = qm.K1;
                    nullable2 = sample3.ND;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k1_2 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k10 = qm.K10;
                    System.Decimal? nullable6;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable6 = nullable2;
                    }
                    else
                        nullable6 = new System.Decimal?(nullable1.GetValueOrDefault() + k10);
                    sample4.XGD = nullable6;
                    source.Add(sample3);
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Min();
                    float num10 = (float)nullable1.Value;
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Max();
                    float num11 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Min();
                    num6 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Max();
                    num7 = (float)nullable1.Value;
                    if ((double)num10 < (double)num6)
                        num6 = num10;
                    if ((double)num11 > (double)num7)
                        num7 = num11;
                }
                else
                {
                    Sample sample1 = new Sample();
                    sample1.XGD = new System.Decimal?(Convert.ToDecimal(num4));
                    Sample sample2 = sample1;
                    System.Decimal k13_1 = qm.K13;
                    nullable1 = sample1.XGD;
                    System.Decimal? nullable3;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable3 = nullable2;
                    }
                    else
                        nullable3 = new System.Decimal?(k13_1 * nullable1.GetValueOrDefault());
                    nullable1 = nullable3;
                    nullable2 = sample1.XGD;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable2 = sample1.XGD;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k12_1 = qm.K12;
                    nullable2 = sample1.XGD;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k12_1 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal? xgd1 = sample1.XGD;
                    nullable2 = nullable2.HasValue & xgd1.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * xgd1.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k11_1 = qm.K11;
                    nullable2 = sample1.XGD;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k11_1 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k10_1 = qm.K10;
                    System.Decimal? nullable4;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable4 = nullable2;
                    }
                    else
                        nullable4 = new System.Decimal?(nullable1.GetValueOrDefault() + k10_1);
                    sample2.ND = nullable4;
                    source.Add(sample1);
                    Sample sample3 = new Sample();
                    sample3.XGD = new System.Decimal?(Convert.ToDecimal(num5));
                    Sample sample4 = sample3;
                    System.Decimal k13_2 = qm.K13;
                    nullable1 = sample3.XGD;
                    System.Decimal? nullable5;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable5 = nullable2;
                    }
                    else
                        nullable5 = new System.Decimal?(k13_2 * nullable1.GetValueOrDefault());
                    nullable1 = nullable5;
                    nullable2 = sample3.XGD;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable2 = sample3.XGD;
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k12_2 = qm.K12;
                    nullable2 = sample3.XGD;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k12_2 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal? xgd2 = sample3.XGD;
                    nullable2 = nullable2.HasValue & xgd2.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * xgd2.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k11_2 = qm.K11;
                    nullable2 = sample3.XGD;
                    nullable2 = nullable2.HasValue ? new System.Decimal?(k11_2 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                    nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                    System.Decimal k10_2 = qm.K10;
                    System.Decimal? nullable6;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new System.Decimal?();
                        nullable6 = nullable2;
                    }
                    else
                        nullable6 = new System.Decimal?(nullable1.GetValueOrDefault() + k10_2);
                    sample4.ND = nullable6;
                    source.Add(sample3);
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Min();
                    float num10 = (float)nullable1.Value;
                    nullable1 = qm.SamList.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Max();
                    float num11 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Min();
                    num6 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Max();
                    num7 = (float)nullable1.Value;
                    if ((double)num10 < (double)num6)
                        num6 = num10;
                    if ((double)num11 > (double)num7)
                        num7 = num11;
                }
            }
            else
            {
                num4 = 0.0f;
                num5 = 100f;
                num6 = 0.0f;
                num7 = 20f;
                if (str == "ND")
                {
                    System.Decimal? nullable1;
                    for (int index = 0; index < 100; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.ND = new System.Decimal?(Convert.ToDecimal(index));
                        Sample sample2 = sample1;
                        System.Decimal k3 = qm.K3;
                        nullable1 = sample1.ND;
                        System.Decimal? nullable2;
                        System.Decimal? nullable3;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable3 = nullable2;
                        }
                        else
                            nullable3 = new System.Decimal?(k3 * nullable1.GetValueOrDefault());
                        nullable1 = nullable3;
                        nullable2 = sample1.ND;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable2 = sample1.ND;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k2 = qm.K2;
                        nullable2 = sample1.ND;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k2 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal? nd = sample1.ND;
                        nullable2 = nullable2.HasValue & nd.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * nd.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k1 = qm.K1;
                        nullable2 = sample1.ND;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k1 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k0 = qm.K0;
                        System.Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new System.Decimal?(nullable1.GetValueOrDefault() + k0);
                        sample2.XGD = nullable4;
                        source.Add(sample1);
                    }
                    num3 = 0.0f;
                    num2 = 100f;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Min();
                    float num8 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.XGD)).Max();
                    float num9 = (float)nullable1.Value;
                    if ((double)num8 < (double)num6)
                        num6 = num8;
                    if ((double)num9 > (double)num7)
                        num7 = num9;
                }
                else
                {
                    System.Decimal? nullable1;
                    for (int index = 0; index < 100; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.XGD = new System.Decimal?(Convert.ToDecimal(index));
                        Sample sample2 = sample1;
                        System.Decimal k3 = qm.K3;
                        nullable1 = sample1.XGD;
                        System.Decimal? nullable2;
                        System.Decimal? nullable3;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable3 = nullable2;
                        }
                        else
                            nullable3 = new System.Decimal?(k3 * nullable1.GetValueOrDefault());
                        nullable1 = nullable3;
                        nullable2 = sample1.XGD;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable2 = sample1.XGD;
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k2 = qm.K2;
                        nullable2 = sample1.XGD;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k2 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal? xgd = sample1.XGD;
                        nullable2 = nullable2.HasValue & xgd.HasValue ? new System.Decimal?(nullable2.GetValueOrDefault() * xgd.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k1 = qm.K1;
                        nullable2 = sample1.XGD;
                        nullable2 = nullable2.HasValue ? new System.Decimal?(k1 * nullable2.GetValueOrDefault()) : new System.Decimal?();
                        nullable1 = nullable1.HasValue & nullable2.HasValue ? new System.Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault()) : new System.Decimal?();
                        System.Decimal k0 = qm.K0;
                        System.Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new System.Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new System.Decimal?(nullable1.GetValueOrDefault() + k0);
                        sample2.ND = nullable4;
                        source.Add(sample1);
                    }
                    num3 = 0.0f;
                    num2 = 100f;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Min();
                    float num8 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, System.Decimal?>((Func<Sample, System.Decimal?>)(s => s.ND)).Max();
                    float num9 = (float)nullable1.Value;
                    if ((double)num8 < (double)num6)
                        num6 = num8;
                    if ((double)num9 > (double)num7)
                        num7 = num9;
                }
            }
            Bitmap bitmap = new Bitmap(this.picOut.Width, this.picOut.Height);
            Graphics graphics1 = Graphics.FromImage((System.Drawing.Image)bitmap);
            graphics1.FillRectangle((Brush)new SolidBrush(Color.White), 0, 0, this.picOut.Width, this.picOut.Height);
            SizeF sizeF1;
            SizeF sizeF2;
            if (str == "ND")
            {
                Graphics graphics2 = graphics1;
                double num8 = Convert.ToDouble((object)source[source.Count - 1].ND);
                string text1 = num8.ToString(format1);
                System.Drawing.Font font1 = new System.Drawing.Font("Segoe UI", (float)num1);
                sizeF1 = graphics2.MeasureString(text1, font1);
                Graphics graphics3 = graphics1;
                num8 = Convert.ToDouble((object)source[source.Count - 1].XGD);
                string text2 = num8.ToString(format2);
                System.Drawing.Font font2 = new System.Drawing.Font("Segoe UI", (float)num1);
                sizeF2 = graphics3.MeasureString(text2, font2);
            }
            else
            {
                Graphics graphics2 = graphics1;
                double num8 = Convert.ToDouble((object)source[source.Count - 1].XGD);
                string text1 = num8.ToString(format1);
                System.Drawing.Font font1 = new System.Drawing.Font("Segoe UI", (float)num1);
                sizeF1 = graphics2.MeasureString(text1, font1);
                Graphics graphics3 = graphics1;
                num8 = Convert.ToDouble((object)source[source.Count - 1].ND);
                string text2 = num8.ToString(format2);
                System.Drawing.Font font2 = new System.Drawing.Font("Segoe UI", (float)num1);
                sizeF2 = graphics3.MeasureString(text2, font2);
            }
            float num12 = 5f + sizeF2.Height + sizeF2.Width;
            float num13 = (float)(this.picOut.Width - 2);
            float num14 = (float)((double)this.picOut.Height - (double)sizeF1.Height * 2.0 - 5.0);
            float height = sizeF1.Height;
            RectangleF rectangleF = new RectangleF(num12, height, num13 - num12, num14 - height);
            graphics1.DrawLine(new Pen(Color.Black, 1f), num12, num14, num13, num14);
            graphics1.DrawLine(new Pen(Color.Black, 1f), num12, height, num13, height);
            graphics1.DrawLine(new Pen(Color.Black, 1f), num12, num14, num12, height);
            graphics1.DrawLine(new Pen(Color.Black, 1f), num13, num14, num13, height);
            float x1 = num12;
            float y1 = num14 + 5f;
            graphics1.DrawString(num4.ToString(format1), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x1, y1));
            SizeF sizeF3 = graphics1.MeasureString(num5.ToString(format1), new System.Drawing.Font("Segoe UI", (float)num1));
            float x2 = num13 - sizeF3.Width;
            graphics1.DrawString(num5.ToString(format1), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x2, y1));
            SizeF sizeF4 = graphics1.MeasureString(num6.ToString(format2), new System.Drawing.Font("Segoe UI", (float)num1));
            float x3 = num12 - sizeF4.Width;
            float y2 = num14 - sizeF4.Height / 2f;
            graphics1.DrawString(num6.ToString(format2), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x3, y2));
            SizeF sizeF5 = graphics1.MeasureString(num7.ToString(format2), new System.Drawing.Font("Segoe UI", (float)num1));
            float x4 = num12 - sizeF5.Width;
            float y3 = height - sizeF5.Height / 2f;
            graphics1.DrawString(num7.ToString(format2), new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x4, y3));
            for (int index = 1; index < 4; ++index)
            {
                Pen pen = new Pen(Color.Black, 1f);
                pen.DashStyle = DashStyle.Dot;
                graphics1.DrawLine(pen, num12 + (float)(((double)num13 - (double)num12) * (double)index / 4.0), num14, num12 + (float)(((double)num13 - (double)num12) * (double)index / 4.0), height);
                graphics1.DrawLine(pen, num12, height + (float)(((double)num14 - (double)height) * (double)index / 4.0), num13, height + (float)(((double)num14 - (double)height) * (double)index / 4.0));
            }
            float x5 = num12 + (float)(((double)num13 - (double)num12 - (double)sizeF1.Width) / 2.0);
            float y4 = num14 + 5f;
            float y5 = height + (float)(((double)num14 - (double)height - (double)sizeF2.Width) / 2.0);
            if (str == "ND")
            {
                float x6 = num12 - graphics1.MeasureString("Abs", new System.Drawing.Font("Segoe UI", (float)num1)).Width;
                graphics1.DrawString(qm.QPar.Unit, new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
                graphics1.DrawString("Abs", new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
            }
            else
            {
                float x6 = num12 - graphics1.MeasureString(qm.QPar.Unit, new System.Drawing.Font("Segoe UI", (float)num1)).Width;
                graphics1.DrawString(qm.QPar.Unit, new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
                graphics1.DrawString("Abs", new System.Drawing.Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
            }
            if ((double)num5 - (double)num4 == 0.0 || (double)num7 - (double)num6 == 0.0)
                return;
            double num15 = ((double)num13 - (double)num12) / ((double)num5 - (double)num4);
            double num16 = ((double)num14 - (double)height) / ((double)num7 - (double)num6);
            if (str == "ND")
            {
                double num8 = (double)num12 + (Convert.ToDouble((object)source[source.Count<Sample>() - 1].ND) - (double)num4) * num15;
                double num9 = (double)num14 - (Convert.ToDouble((object)source[source.Count<Sample>() - 1].XGD) - (double)num6) * num16;
                for (int index = source.Count<Sample>() - 2; index >= 0; --index)
                {
                    double num10 = (double)num12 + (Convert.ToDouble((object)source[index].ND) - (double)num4) * num15;
                    double num11 = Convert.ToDouble((object)source[index].XGD) >= (double)num6 ? (Convert.ToDouble((object)source[index].XGD) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)source[index].XGD) - (double)num6) * num16 : (double)height) : (double)num14;
                    graphics1.DrawLine(new Pen(Color.Red, 1f), (float)num8, (float)num9, (float)num10, (float)num11);
                    num8 = num10;
                    num9 = num11;
                }
                if (qm.SamList != null && qm.SamList.Count > 0)
                {
                    for (int index = 0; index < qm.SamList.Count; ++index)
                    {
                        double num10 = (double)num12 + (Convert.ToDouble((object)qm.SamList[index].ND) - (double)num4) * num15;
                        double num11 = Convert.ToDouble((object)qm.SamList[index].XGD) >= (double)num6 ? (Convert.ToDouble((object)qm.SamList[index].XGD) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)qm.SamList[index].XGD) - (double)num6) * num16 : (double)height) : (double)num14;
                        graphics1.DrawEllipse(new Pen(Color.Blue, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                    }
                }
            }
            else
            {
                double num8 = (double)num12 + (Convert.ToDouble((object)source[source.Count<Sample>() - 1].XGD) - (double)num4) * num15;
                double num9 = (double)num14 - (Convert.ToDouble((object)source[source.Count<Sample>() - 1].ND) - (double)num6) * num16;
                for (int index = source.Count<Sample>() - 2; index >= 0; --index)
                {
                    double num10 = (double)num12 + (Convert.ToDouble((object)source[index].XGD) - (double)num4) * num15;
                    double num11 = Convert.ToDouble((object)source[index].ND) >= (double)num6 ? (Convert.ToDouble((object)source[index].ND) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)source[index].ND) - (double)num6) * num16 : (double)height) : (double)num14;
                    graphics1.DrawLine(new Pen(Color.Red, 1f), (float)num8, (float)num9, (float)num10, (float)num11);
                    num8 = num10;
                    num9 = num11;
                }
                if (qm.SamList != null && qm.SamList.Count > 0)
                {
                    for (int index = 0; index < qm.SamList.Count; ++index)
                    {
                        double num10 = (double)num12 + (Convert.ToDouble((object)qm.SamList[index].XGD) - (double)num4) * num15;
                        double num11 = Convert.ToDouble((object)qm.SamList[index].ND) >= (double)num6 ? (Convert.ToDouble((object)qm.SamList[index].ND) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)qm.SamList[index].ND) - (double)num6) * num16 : (double)height) : (double)num14;
                        graphics1.DrawEllipse(new Pen(Color.Blue, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                    }
                }
            }
            this.picOut.Image = (System.Drawing.Image)bitmap;
        }

        private void SendBlankCmd(QuaMethod qm)
        {
            List<string> list = ((IEnumerable<string>)qm.QPar.WL.Split(',')).ToList<string>();
            if (qm.QPar.BackWL.Length > 0)
                list.Add(qm.QPar.BackWL);
            for (int index = 0; index < list.Count<string>(); ++index)
            {
                if (Convert.ToDecimal(list[index]) < 190M || Convert.ToDecimal(list[index]) > 1100M)
                {
                    CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    if (this.SubScan)
                    {
                        if (this.smfrm.btnXL.InvokeRequired)
                        {
                            this.smfrm.btnXL.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)true);
                            return;
                        }
                        this.Setblanklable(true);
                        return;
                    }
                    if (this.btnBlank.InvokeRequired)
                    {
                        this.btnBlank.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)false);
                        return;
                    }
                    this.Setblanklable(false);
                    return;
                }
            }
            if (qm.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || qm.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
            {
                try
                {
                    string[] strArray = qm.QPar.R.Split(',');
                    Convert.ToDecimal(strArray[0]);
                    Convert.ToDecimal(strArray[1]);
                    Convert.ToDecimal(strArray[2]);
                    Convert.ToDecimal(strArray[3]);
                }
                catch
                {
                    CommonFun.showbox(CommonFun.GetLanText("methoderror"), "Error");
                    if (this.SubScan)
                    {
                        if (this.smfrm.btnXL.InvokeRequired)
                        {
                            this.smfrm.btnXL.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)true);
                            return;
                        }
                        this.Setblanklable(true);
                        return;
                    }
                    if (this.btnBlank.InvokeRequired)
                    {
                        this.btnBlank.Invoke((Delegate)new DNAFrm.Del_SetBlankLable(this.Setblanklable), (object)false);
                        return;
                    }
                    this.Setblanklable(false);
                    return;
                }
            }
            for (int index = 0; index < list.Count<string>(); ++index)
            {
                this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(list[index]) * 10M).ToString("f0") + "\r\n");
                CommonFun.WriteSendLine("DNA，calbgnd 1 1 " + (Convert.ToDecimal(list[index]) * 10M).ToString("f0"));
            }
            this.ComSta = ComStatus.CALBGND;
            if (this.btnBack.InvokeRequired)
                this.btnBack.Invoke((Delegate)new DNAFrm.Del_SetState(this.SetState));
            else
                this.SetState();
        }

        private void SendScanCmd(QuaMethod qm)
        {
            List<string> list = ((IEnumerable<string>)qm.QPar.WL.Split(',')).ToList<string>();
            if (qm.QPar.BackWL.Length > 0)
                list.Add(qm.QPar.BackWL);
            for (int index = 0; index < list.Count<string>(); ++index)
            {
                if (Convert.ToDecimal(list[index]) < 190M || Convert.ToDecimal(list[index]) > 1100M)
                {
                    CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    if (this.SubScan)
                    {
                        if (this.smfrm.btnMeasure.InvokeRequired)
                        {
                            this.smfrm.btnMeasure.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)true);
                            return;
                        }
                        this.SetMealable(true);
                        return;
                    }
                    if (this.btnScan.InvokeRequired)
                    {
                        this.btnScan.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)false);
                        return;
                    }
                    this.SetMealable(false);
                    return;
                }
            }
            if (qm.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || qm.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
            {
                try
                {
                    string[] strArray = qm.QPar.R.Split(',');
                    Convert.ToDecimal(strArray[0]);
                    Convert.ToDecimal(strArray[1]);
                    Convert.ToDecimal(strArray[2]);
                    Convert.ToDecimal(strArray[3]);
                }
                catch
                {
                    CommonFun.showbox(CommonFun.GetLanText("methoderror"), "Error");
                    if (this.SubScan)
                    {
                        if (this.smfrm.btnMeasure.InvokeRequired)
                        {
                            this.smfrm.btnMeasure.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)true);
                            return;
                        }
                        this.SetMealable(true);
                        return;
                    }
                    if (this.btnScan.InvokeRequired)
                    {
                        this.btnScan.Invoke((Delegate)new DNAFrm.Del_SetMeasureLable(this.SetMealable), (object)false);
                        return;
                    }
                    this.SetMealable(false);
                    return;
                }
            }
            this.ComSta = ComStatus.MEASURE;
            SerialPort sp = this.sp;
            System.Decimal num = Convert.ToDecimal(list[0]) * 10M;
            string text = "measure 1 2 " + num.ToString("f0") + "\r\n";
            sp.WriteLine(text);
            if (this.btnBack.InvokeRequired)
                this.btnBack.Invoke((Delegate)new DNAFrm.Del_SetState(this.SetState));
            else
                this.SetState();
            num = Convert.ToDecimal(list[0]) * 10M;
            CommonFun.WriteSendLine("DNA，measure 1 2 " + num.ToString("f0"));
            ++this.meacnt;
            ++this.mcnt;
        }

        private void SetState()
        {
            this.setState();
            if (this.SubScan)
            {
                this.smfrm.progressBar1.Visible = true;
                this.smfrm.progressBar1.Value = 10;
            }
            else
            {
                this.panel4.Visible = true;
                if (this.ComSta == ComStatus.CALBGND)
                    this.lblProsess.Text = CommonFun.GetLanText("inblanking");
                else
                    this.lblProsess.Text = CommonFun.GetLanText("measureing");
            }
        }
        private void pibList_Click(object sender, EventArgs e)
        {
            if (this.QM.DNAMeaList == null || this.QM.DNAMeaList.Count == 0)
            {
                CommonFun.showbox(CommonFun.GetLanText("nodata"), "Warning");
            }
            else
            {
                using (MulDetailDataFrm frm = new MulDetailDataFrm())
                {
                    frm.dataGridView3.Visible = false;
                    frm.dataGridView1.Visible = false;
                    frm.dataGridView2.Visible = false;
                    frm.lblTitle.Text = CommonFun.GetLanText("dnadatalist");
                    if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                    {
                        frm.btnDelete.Visible = false;
                        frm.dataGridView2.Columns[5].Visible = false;
                    }
                    frm.Load += (EventHandler)((param0, param1) => frm.DnaListBind(this.QM));
                    frm.dataGridView4.CellClick += (DataGridViewCellEventHandler)((senders, es) =>
                    {
                        if (es.ColumnIndex != 6)
                            return;
                        using (KeyBoard frm1 = new KeyBoard("", ""))
                        {
                            if (this.QM.C_head != null && this.QM.C_head.Length > 0)
                                frm1.txtValue.Text = this.QM.C_head;
                            frm1.btnOK.Click += ((param0, param1) =>
                            {
                                if (new DRMessageBoxFrm(CommonFun.GetLanText("sampleheadconfirm"), "Warning").ShowDialog() == DialogResult.Yes)
                                {
                                    this.QM.C_head = frm1.txtValue.Text;
                                    for (int index = 0; index < this.QM.DNAMeaList.Count; ++index)
                                        this.QM.DNAMeaList[index].C_bz = this.QM.C_head + "-" + (index + 1).ToString();
                                }
                                else
                                    this.QM.DNAMeaList[es.RowIndex].C_bz = frm1.txtValue.Text;
                                frm1.Close();
                                frm.DnaListBind(this.QM);
                            });
                            frm1.ShowDialog();
                        }
                    });
                    frm.btnDelete.Click += (EventHandler)((param0, param1) =>
                    {
                        if (new DRMessageBoxFrm(CommonFun.GetLanText("deleteconfirm"), "Warning").ShowDialog() != DialogResult.Yes)
                            return;
                        bool flag = false;
                        for (int index = 0; index < frm.dataGridView2.Rows.Count; ++index)
                        {
                            if (frm.dataGridView2.Rows[index].Tag != null && frm.dataGridView2.Rows[index].Cells["ColOP1"].Tag.ToString() == "on")
                            {
                                this.QM.DNAMeaList.Remove((DNAMeasureData)frm.dataGridView2.Rows[index].Tag);
                                flag = true;
                            }
                        }
                        if (flag)
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("logdelData"), false);
                        frm.DnaListBind(this.QM);
                        if (flag)
                        {
                            this.LoadMeaData(0);
                            this.currindex = this.QM.DNAMeaList.Count <= 0 ? -1 : 0;
                        }
                    });
                    int num1 = (int)frm.ShowDialog();
                }
            }
        }

        private void lblNo_Click(object sender, EventArgs e)
        {
            if (this.QM.DNAMeaList == null || this.QM.DNAMeaList.Count == 0)
                return;
            using (InputDataFrm frm1 = new InputDataFrm())
            {
                frm1.txtValue.KeyDown += ((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        int int32 = Convert.ToInt32(frm1.txtValue.Text);
                        if (int32 <= 0 || int32 > this.QM.DNAMeaList.Count)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                        }
                        else
                        {
                            this.currindex = int32 - 1;
                            frm1.Close();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm1.btnOK.Click += ((param0, param1) =>
                {
                    try
                    {
                        int int32 = Convert.ToInt32(frm1.txtValue.Text);
                        if (int32 <= 0 || int32 > this.QM.DNAMeaList.Count)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                        }
                        else
                        {
                            this.currindex = int32 - 1;
                            frm1.Close();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm1.ShowDialog();
            }
            this.LoadMeaData(this.currindex);
        }

        private void lblremarkV_Click(object sender, EventArgs e)
        {
            using (KeyBoard frm1 = new KeyBoard("", lblremarkV.Text))
            {
                frm1.btnOK.Click += ((param0, param1) =>
                {
                    try
                    {

                        lblremarkV.Text = frm1.txtValue.Text;
                        this.QM.DNAMeaList[this.currindex].C_bz = this.lblremarkV.Text;
                        frm1.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm1.ShowDialog();
            }
        }

        private void lblValue_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.sourcex = e.X;
            this.sourcey = e.Y;
        }
        private void lblValue_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int num1 = e.X - this.sourcex;
            int num2 = e.Y - this.sourcey;
            if (num1 > 20 && this.currindex >= 0 && this.QM.DNAMeaList != null)
            {
                if (this.currindex <= 0)
                    this.currindex = this.QM.DNAMeaList.Count - 1;
                else
                    --this.currindex;
                this.LoadMeaData(this.currindex);
            }
            if (num1 >= -20 || this.currindex < 0 || this.QM.DNAMeaList == null)
                return;
            if (this.currindex >= this.QM.DNAMeaList.Count - 1)
                this.currindex = 0;
            else
                ++this.currindex;
            this.LoadMeaData(this.currindex);
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            CommonFun.WriteSendLine("DNA, Нажмите, чтобы остановить");
            this.ComSta = ComStatus.END;
            this.btnBlank.Text = CommonFun.GetLanText("blanking");
            this.panel4.Visible = false;
            this.progressBar1.Value = 10;
            this.btnScan.Text = CommonFun.GetLanText("measure");
            this.slotno = "";
            this.currslotno = 0;
            this.calormea = 0;
            this.setState();
            this.meacnt = 0;
        }

        private delegate void Del_setstate(bool status);

        private delegate void Del_starttt(bool status);

        private delegate void Del_BindData(int cux);

        private delegate void Del_SetBlankLable(bool subscan);

        private delegate void Del_SetMeasureLable(bool subscan);

        private delegate void Del_SetWl(string wl);

        private delegate void Del_SetProgressbar(int value);

        private delegate void Del_SetState();

        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".dna";

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.QM.DNAMeaList == null || this.QM.DNAMeaList.Count == 0)
            {
                CommonFun.showbox(CommonFun.GetLanText("nodata"), "Warning");
            }
            else
            {
                using (SaveFrm save = new SaveFrm(extension, "ДНК анализ"))
                {
                    save.btnOK.PreviewMouseLeftButtonUp += ((param0_1, param1_1) =>
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

            XmlNode MeasureMethodName = xd.CreateElement("MeasureMethodName"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.MeasureMethodName; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("WL"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.WL; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("BackWL"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.BackWL; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("R"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.R; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("MCnt"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.MCnt.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("Length"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.Length; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("Unit"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.Unit; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("Threshold"); //Настройки измерения
            MeasureMethodName.InnerText = this.QM.QPar.Limits; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
            {

            }
            else
            {
                MeasureMethodName = xd.CreateElement("SamCnt"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.QPar.SamCnt.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("FittingDM"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.QPar.FittingDM.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит             

                MeasureMethodName = xd.CreateElement("Fitting"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.QPar.Fitting; // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("Equation"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.QPar.Equation; // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("CabMethodDM"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.QPar.CabMethodDM; // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("CabMethod"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.QPar.CabMethod; // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("ZeroB"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.QPar.ZeroB.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K0"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K0.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K1"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K1.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K2"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K2.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K3"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K3.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("AFCS"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.AFCS.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K10"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K10.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K11"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K11.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K12"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K12.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("K13"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.K13.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

                MeasureMethodName = xd.CreateElement("CFCS"); //Настройки измерения
                MeasureMethodName.InnerText = this.QM.CFCS.ToString(); // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит               

            }

            xd.DocumentElement.AppendChild(Settings);

            XmlNode IzmerenieParametr = xd.CreateElement("IzmerenieParametr");

            /*  for (int i = 0; i < this.QM.DNAMeaList.Count; i++)
              {
                  string xmlValue = "";
                  xmlValue = xmlValue + (object)this.QM.DNAMeaList[i] + ";";

                  MeasureMethodName = xd.CreateElement("SamList"); //Настройки измерения
                  MeasureMethodName.InnerText = xmlValue; // и значение
                  IzmerenieParametr.AppendChild(MeasureMethodName); // и указываем кому принадлежит 
              }
              */
            string xmlValue = "";
            for (int index = 0; index < this.QM.DNAMeaList.Count; index++)
            {
                if (this.QM.SamList == null)
                {
                    xmlValue = xmlValue + this.QM.DNAMeaList[index].A[0] + "," + this.QM.DNAMeaList[index].A[1] + "," + this.QM.DNAMeaList[index].ABack + "," + this.QM.DNAMeaList[index].C_bz + "," + this.QM.DNAMeaList[index].DNA + "," + this.QM.DNAMeaList[index].D_sj + "," + this.QM.DNAMeaList[index].ND + "," + this.QM.DNAMeaList[index].Protein + "," + this.QM.DNAMeaList[index].Ratio + "," + this.QM.DNAMeaList[index].XGD;

                    xmlValue = xmlValue + ";";
                }
                else
                {
                    xmlValue = xmlValue + this.QM.DNAMeaList[index].ABack + "," + this.QM.DNAMeaList[index].C_bz + "," + this.QM.DNAMeaList[index].DNA + "," + this.QM.DNAMeaList[index].D_sj + "," + this.QM.DNAMeaList[index].ND + "," + this.QM.DNAMeaList[index].Protein + "," + this.QM.DNAMeaList[index].Ratio + "," + this.QM.DNAMeaList[index].XGD;

                    xmlValue = xmlValue + ";";
                }


            }
            MeasureMethodName = xd.CreateElement("DNAMeaList"); //Настройки измерения
            MeasureMethodName.InnerText = xmlValue; // и значение
            IzmerenieParametr.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            string avalue = "";
            xmlValue = "";
            for (int index = 0; index < this.QM.SamList.Count; index++)
            {
                if (this.QM.SamList[index].Avalue == null)
                {
                    xmlValue = xmlValue + this.QM.SamList[index].C_bz + "," + this.QM.SamList[index].D_sj + "," + this.QM.SamList[index].IsExclude + "," + this.QM.SamList[index].ND + "," + this.QM.SamList[index].XGD;

                    xmlValue = xmlValue + ";";
                }
                else
                {
                    for (int i = 0; i < this.QM.SamList[index].Avalue.Count(); i++)
                    {
                        avalue = avalue + this.QM.SamList[index].Avalue[i];
                        if (i != this.QM.SamList[index].Avalue.Count() - 1)
                        {
                            avalue = avalue + ",";
                        }
                        else
                        {
                            avalue = avalue + ";";
                        }
                    }

                    xmlValue = xmlValue + this.QM.SamList[index].C_bz + "," + this.QM.SamList[index].D_sj + "," + this.QM.SamList[index].IsExclude + "," + this.QM.SamList[index].ND + "," + this.QM.SamList[index].XGD;

                    xmlValue = xmlValue + ";";
                }

            }

            MeasureMethodName = xd.CreateElement("SamList"); //Настройки измерения
            MeasureMethodName.InnerText = xmlValue; // и значение
            IzmerenieParametr.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            if (avalue != "")
            {
                MeasureMethodName = xd.CreateElement("Avalue"); //Настройки измерения
                MeasureMethodName.InnerText = avalue; // и значение
                IzmerenieParametr.AppendChild(MeasureMethodName); // и указываем кому принадлежит 
            }




            xd.DocumentElement.AppendChild(IzmerenieParametr);
            fs.Close();         // Закрываем поток  
            xd.Save(filepath); // Сохраняем файл  
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            Open_File();
        }

        bool shifrTrueFalse = false;

        public List<decimal> A { get; private set; }

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
                  //  xDoc.Load(filepath);

                    XDocument xdoc = XDocument.Load(pathTemp + "/" + openFrm.open_name);
                   // XDocument xdoc = XDocument.Load(filepath);
                    XmlNodeList nodes = xDoc.ChildNodes;

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
                                        if ("MeasureMethodName".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.MeasureMethodName = k.FirstChild.Value;
                                        }
                                        if ("WL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.WL = k.FirstChild.Value;

                                        }
                                        if ("BackWL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.BackWL = k.FirstChild.Value;
                                        }
                                        if ("R".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.R = k.FirstChild.Value;
                                        }
                                        if ("MCnt".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.MCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("Length".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.Length = k.FirstChild.Value;
                                        }
                                        if ("Unit".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.Unit = k.FirstChild.Value;
                                        }
                                        if ("Threshold".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.Limits = k.FirstChild.Value;
                                        }
                                        if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1") || this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                                        {

                                        }
                                        else
                                        {
                                            if ("SamCnt".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.QPar.SamCnt = Convert.ToInt32(k.FirstChild.Value);
                                            }
                                            if ("FittingDM".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.QPar.FittingDM = k.FirstChild.Value;
                                            }
                                            if ("Fitting".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.QPar.Fitting = k.FirstChild.Value;
                                            }
                                            if ("Equation".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.QPar.Equation = k.FirstChild.Value;
                                            }
                                            if ("CabMethodDM".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.QPar.CabMethodDM = k.FirstChild.Value;
                                            }
                                            if ("CabMethod".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.QPar.CabMethod = k.FirstChild.Value;
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
                                            if ("AFCS".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.AFCS = k.FirstChild.Value;
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
                                            if ("CFCS".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QM.CFCS = k.FirstChild.Value;
                                            }
                                        }

                                    }
                                }
                                if ("IzmerenieParametr".Equals(d.Name))
                                {
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {

                                        if ("SamList".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            string aa = k.FirstChild.Value;

                                            //this.lblNo.Text = (index + 1).ToString("D4") + "/" + this.QM.DNAMeaList.Count.ToString("D4");
                                            this.QM.SamList = new List<Sample>();

                                            for (int i = 0; i < k.FirstChild.Value.Split(';').Count() - 1; i++)
                                            {
                                                Sample sample = new Sample();

                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[0] != "")
                                                {
                                                    //decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[3], out res);
                                                    sample.C_bz = k.FirstChild.Value.Split(';')[i].Split(',')[0];
                                                }
                                                else
                                                {
                                                    sample.C_bz = null;
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[1] != "")
                                                {
                                                    sample.D_sj = DateTime.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[1]);
                                                }
                                                else
                                                {
                                                    sample.D_sj = null;
                                                }
                                                sample.IsExclude = Convert.ToBoolean(k.FirstChild.Value.Split(';')[i].Split(',')[2]);
                                                sample.ND = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[3]);
                                                sample.XGD = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[4]);

                                                this.QM.SamList.Add(sample);
                                            }
                                        }
                                        if ("Avalue".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            string aa = k.FirstChild.Value;

                                            //this.lblNo.Text = (index + 1).ToString("D4") + "/" + this.QM.DNAMeaList.Count.ToString("D4");                                          

                                            for (int i = 0; i < k.FirstChild.Value.Split(';').Count() - 1; i++)
                                            {
                                                this.QM.SamList[i].Avalue = new decimal[k.FirstChild.Value.Split(';')[i].Split(',').Count()];
                                                for (int j = 0; j < k.FirstChild.Value.Split(';')[i].Split(',').Count() - 1; j++)
                                                {
                                                    if (k.FirstChild.Value.Split(';')[i].Split(',')[j] != "")
                                                    {
                                                        decimal res;
                                                        decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[j], out res);
                                                        this.QM.SamList[i].Avalue[j] = res;
                                                    }
                                                }

                                            }

                                        }


                                        if ("DNAMeaList".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            string aa = k.FirstChild.Value;

                                            this.QM.DNAMeaList = new List<DNAMeasureData>();

                                            for (int i = 0; i < k.FirstChild.Value.Split(';').Count() - 1; i++)
                                            {

                                                if (this.QM.DNAMeaList == null || this.QM.DNAMeaList.Count == 0)
                                                {
                                                    this.lblNo.Text = "0001";
                                                }
                                                else
                                                {
                                                    this.lblNo.Text = (i + 1).ToString("D4") + "/" + (this.QM.DNAMeaList.Count + 1).ToString("D4");
                                                }


                                                this.sslive = new DNAMeasureData();

                                                this.sslive.A = new List<System.Decimal>();

                                                decimal res;
                                                if (this.QM.SamList == null)
                                                {
                                                    decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[0], out res);
                                                    this.sslive.A.Add(res);

                                                    decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[1], out res);
                                                    this.sslive.A.Add(res);

                                                    decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[2], out res);
                                                    this.sslive.ABack = res;

                                                    if (k.FirstChild.Value.Split(';')[i].Split(',')[3] != "")
                                                    {
                                                        this.sslive.C_bz = k.FirstChild.Value.Split(';')[i].Split(',')[3];
                                                    }
                                                    else
                                                    {
                                                        this.sslive.C_bz = null;
                                                    }


                                                    decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[4], out res);
                                                    this.sslive.DNA = res;

                                                    this.sslive.D_sj = DateTime.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[5]);

                                                    this.sslive.ND = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[6]);
                                                    this.sslive.Protein = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[7]);
                                                    this.sslive.Ratio = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[8]);
                                                    this.sslive.XGD = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[9]);
                                                }
                                                else
                                                {
                                                    //decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[0], out res);
                                                    //  this.sslive.A.Add(res);

                                                    decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[0], out res);
                                                    this.sslive.ABack = res;

                                                    if (k.FirstChild.Value.Split(';')[i].Split(',')[1] != "")
                                                    {
                                                        this.sslive.C_bz = k.FirstChild.Value.Split(';')[i].Split(',')[1];
                                                    }
                                                    else
                                                    {
                                                        this.sslive.C_bz = null;
                                                    }


                                                    decimal.TryParse(k.FirstChild.Value.Split(';')[i].Split(',')[2], out res);
                                                    this.sslive.DNA = res;

                                                    this.sslive.D_sj = DateTime.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[3]);

                                                    this.sslive.ND = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[4]);
                                                    this.sslive.Protein = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[5]);
                                                    this.sslive.Ratio = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[6]);
                                                    this.sslive.XGD = Convert.ToDecimal(k.FirstChild.Value.Split(';')[i].Split(',')[7]);
                                                }

                                                this.QM.DNAMeaList.Add(this.sslive);

                                                this.lbltimeV.Text = this.QM.DNAMeaList[i].D_sj.ToString();
                                                this.lblremarkV.Text = this.QM.DNAMeaList[i].C_bz;

                                                if (this.QM.QPar.MeasureMethodName.Contains("ДНК"))
                                                {
                                                    this.lblValue1.Visible = true;
                                                    this.lblValue2.Visible = true;
                                                    this.lblUnit1.Visible = true;
                                                    this.lblUnit2.Visible = true;
                                                    System.Decimal num;
                                                    if (this.QM.DNAMeaList[i].DNA != 99999.99M)
                                                    {
                                                        Label lblValue1 = this.lblValue1;
                                                        num = this.QM.DNAMeaList[i].DNA;
                                                        string str = num.ToString(this.conacc);
                                                        lblValue1.Text = str;
                                                    }
                                                    else
                                                        this.lblValue1.Text = "---";
                                                    if (this.QM.DNAMeaList[i].Protein != 99999.99M)
                                                    {
                                                        Label lblValue2 = this.lblValue2;
                                                        num = this.QM.DNAMeaList[i].Protein;
                                                        string str = num.ToString(this.conacc);
                                                        lblValue2.Text = str;
                                                    }
                                                    else
                                                        this.lblValue2.Text = "---";
                                                    this.lblA1.Text = "A1\r\n" + this.QM.QPar.WL.Split(',')[0];
                                                    this.lblA2.Text = "A2\r\n" + this.QM.QPar.WL.Split(',')[1];

                                                    if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                                        this.lblA3.Text = "Aref\r\n" + this.QM.QPar.BackWL;
                                                    else
                                                        this.lblA3.Text = "Aref";
                                                    this.lblA4.Text = "Ratio";
                                                    if (this.QM.DNAMeaList[i].A[0] != 99999.99M)
                                                    {
                                                        Label lblA1V = this.lblA1V;
                                                        num = this.QM.DNAMeaList[i].A[0];
                                                        string str = num.ToString(this.absacc);
                                                        lblA1V.Text = str;
                                                    }
                                                    else
                                                        this.lblA1V.Text = "---";
                                                    if (this.QM.DNAMeaList[i].A[1] != 99999.99M)
                                                    {
                                                        Label lblA2V = this.lblA2V;
                                                        num = this.QM.DNAMeaList[i].A[1];
                                                        string str = num.ToString(this.absacc);
                                                        lblA2V.Text = str;
                                                    }
                                                    else
                                                        this.lblA2V.Text = "---";
                                                    if (this.QM.QPar.BackWL.Length > 0)
                                                    {
                                                        Label lblA3V = this.lblA3V;
                                                        num = this.QM.DNAMeaList[i].ABack;
                                                        string str = num.ToString(this.absacc);
                                                        lblA3V.Text = str;
                                                    }
                                                    else
                                                        this.lblA3V.Text = " ---";
                                                    Label lblA4V = this.lblA4V;
                                                    num = this.QM.DNAMeaList[i].Ratio;
                                                    string str1 = num.ToString(this.conacc);
                                                    lblA4V.Text = str1;
                                                }
                                                else
                                                {
                                                    this.lblValue1.Visible = true;
                                                    this.lblValue2.Visible = false;
                                                    this.lblUnit1.Visible = true;
                                                    this.lblUnit2.Visible = false;
                                                    System.Decimal num;
                                                    if (this.QM.DNAMeaList[i].ND != -1M)
                                                    {
                                                        Label lblValue1 = this.lblValue1;
                                                        num = this.QM.DNAMeaList[i].ND;
                                                        string str = num.ToString();
                                                        lblValue1.Text = str;
                                                    }
                                                    else
                                                        this.lblValue1.Text = "----";
                                                    this.lblA1.Text = "A1\r\n" + this.QM.QPar.WL;
                                                    Label lblA1V = this.lblA1V;
                                                    num = this.QM.DNAMeaList[i].XGD;
                                                    string str1 = num.ToString(this.absacc) ?? "";
                                                    lblA1V.Text = str1;
                                                    this.lblA2.Text = "";
                                                    this.lblA3.Text = "";
                                                    this.lblA4.Text = "";
                                                    this.lblA2V.Text = "";
                                                    this.lblA3V.Text = "";
                                                    this.lblA4V.Text = "";
                                                }

                                                if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna1"))
                                                {
                                                    if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                                        this.lblfor.Text = "C(DNA)=62.9*(A1-Aref)-36.0*(A2-Aref); C(Protein)=1552*(A2-Aref)-757.3*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                                                    else
                                                        this.lblfor.Text = "C(DNA)=62.9*A1-36.0*A2; C(Protein)=1552*A2-757.3*A2; Ratio=A1/A2";
                                                }
                                                else if (this.QM.QPar.MeasureMethodName == CommonFun.GetLanText("dna2"))
                                                {
                                                    if (this.QM.QPar.BackWL != null && this.QM.QPar.BackWL.Length > 0)
                                                        this.lblfor.Text = "C(DNA)=49.1*(A1-Aref)-3.48*(A2-Aref); C(Protein)=183*(A2-Aref)-75.8*(A2-Aref); Ratio=(A1-Aref)/(A2-Aref)";
                                                    else
                                                        this.lblfor.Text = "C(DNA)=49.1*A1-3.48*A2; C(Protein)=183*A2-75.8*A2; Ratio=A1/A2";
                                                }
                                                else if (this.QM.QPar.Equation == "Abs=f(C)")
                                                    this.lblfor.Text = this.QM.AFCS;
                                                else
                                                    this.lblfor.Text = this.QM.CFCS;
                                                this.lblUnit1.Text = this.QM.QPar.Unit;
                                                this.lblUnit2.Text = this.QM.QPar.Unit;



                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}




