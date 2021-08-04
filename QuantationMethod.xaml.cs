using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brush = System.Drawing.Brush;
using SystemColors = System.Drawing.SystemColors;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Point = System.Drawing.Point;
using Control = System.Windows.Forms.Control;
using System.ComponentModel;
using Label = System.Windows.Forms.Label;
using FontStyle = System.Drawing.FontStyle;

using Image = System.Drawing.Image;
using DashStyle = System.Drawing.Drawing2D.DashStyle;
using Path = System.IO.Path;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для QuantationMethod.xaml
    /// </summary>
    public partial class QuantationMethod : Window,IDisposable
    {
        public string[] wl_mass;
        public string step_interval;
        string count_measure;
        string optical_path;
        string curveEquation;
        string fittingMethod;
        string count_standard_sample;
        string measureunit;
        bool zero_intercept;
        string measure_method;
        string calibration_method;
        private QuaMethod QM;
        private List<QuaMeaMethod> mlist = new List<QuaMeaMethod>();
        //public QuantationMethod(string[] wl_mass, string step_interval, string count_measure, string optical_path, string curveEquation, string fittingMethod, string count_standard_sample, string measureunit, bool zero_intercept, string measure_method, string calibration_method)

        public QuantationMethod(QuaMethod qpar, string[] wl_mass, string step_interval, string count_measure, string optical_path, string curveEquation, string fittingMethod, string count_standard_sample, string measureunit, bool zero_intercept, string measure_method, string calibration_method)
        {
            InitializeComponent();
            this.QPar = qpar;
            QM = qpar;
            this.mlist = CommonFun.CommonMethodList(1);
            // QuaMeaMethod quaMeaMethod = new QuaMeaMethod();
            // quaMeaMethod.C_gs = QPar.
            this.wl_mass = wl_mass;
            this.step_interval = step_interval;
            this.count_measure = count_measure;
            this.optical_path = optical_path;
            this.curveEquation = curveEquation;
            this.fittingMethod = fittingMethod;
            this.count_standard_sample = count_standard_sample;
            this.measureunit = measureunit;
            this.zero_intercept = zero_intercept;
            this.measure_method = measure_method;
            this.calibration_method = calibration_method;

            lblMeaMeth.Content = this.measure_method;
            if (wl_mass != null)
            {
                for (int i = 0; i < wl_mass.Count(); i++)
                {
                    if (i != 0)
                    {
                        lblWlV.Content += Convert.ToDecimal(wl_mass[i], new CultureInfo("en-US")).ToString();
                    }
                    else
                    {
                        lblWlV.Content = Convert.ToDecimal(wl_mass[i], new CultureInfo("en-US")).ToString();
                    }
                    if (i != wl_mass.Count() - 1)
                    {
                        lblWlV.Content += ",";
                    }
                }
            }
            else
            {
                lblWlV.Content = null;
            }

            lblCountMea.Content = count_standard_sample;
            lblOpticalPath.Content = optical_path;
            lblequationV.Content = curveEquation;
            lblfittingV.Content = fittingMethod;
            if (zero_intercept == false)
                lblZeroInc.Content = "Нет";
            else
                lblZeroInc.Content = "Да";
            lblCabMethodV.Content = calibration_method;
            lblCountStandSample.Content = count_standard_sample;
            lblMeasureunit.Content = measureunit;
            ///lblThresholdV.Content
            ///
            
 

        }
        public void Dispose() {  }
        public QuaMethod QPar { get; set; }
        private void MeathodQuaFrm_Load(object sender, EventArgs e)
        {           
            this.ShowQm();
        }

        public void ShowQm()
        {
            /*lblMeaMeth.Content = QM.QPar.MeasureMethodName;
            lblWlV.Content = QM.QPar.WL;
            lblOpticalPath.Content = optical_path;
            lblequationV.Content = QM.QPar.Equation;
            lblfittingV.Content = QM.QPar.Fitting;
            lblZeroInc.Content = QM.QPar.ZeroB;
            lblCabMethodV.Content = QM.QPar.CabMethod;
            lblCountStandSample.Content = count_standard_sample;
            lblMeasureunit.Content = QM.QPar.Unit;*/
            if (this.QPar.Page == 1)
            {
                this.lblMeaMeth.Content = this.QPar.QPar.MeasureMethodName;
                //this.lblMeaMeth.Content = (object)this.QPar.QPar.MeasureMethod;
                this.lblCabMethodV.Content = this.QPar.QPar.CabMethod;
             
               // this.lblCabMethodV.Tag = (object)this.QPar.QPar.CabMethodDM;
                this.lblfittingV.Content = this.QPar.QPar.Fitting;
              //  this.lblFittingV.Tag = (object)this.QPar.QPar.FittingDM;
                this.lblequationV.Content = this.QPar.QPar.Equation;
              //  this.lblSamCntV.Text = this.QPar.QPar.SamCnt.ToString();
                this.lblZeroInc.Content = this.QPar.QPar.ZeroB ? true : false;
                this.lblCountMea.Content = this.QPar.QPar.MCnt;
                this.lblCountStandSample.Content = this.QPar.QPar.SamCnt;
                if (this.QPar.QPar.Equation == "C=f(Abs)")
                    this.lblsc.Content = this.QPar.CFCS;
                else
                    this.lblsc.Content = this.QPar.AFCS;
                Decimal? nullable;
                if (this.QPar.R.HasValue)
                {
                    //Label lblsc = this.lblsc;
                    string text = this.lblsc.Content.ToString();
                    nullable = this.QPar.R;
                    string str1 = nullable.Value.ToString("f6");
                    string str2 = text + ",  R=" + str1;
                    this.lblsc.Content = str2;
                }
                int num1 = 1;
                this.lblxgd.Content = "Абс";
                this.lblnd.Content = "Конц";
                this.lblxgd1.Content = "Абс";
                this.lblnd1.Content = "Конц";
                this.lblxgd2.Content = "Абс";
                this.lblnd2.Content = "Конц";
                if (this.QPar.SamList != null && this.QPar.SamList.Count > 0)
                {
                    int? square = this.QPar.QPar.MeasureMethod.Square;
                    if ((square.GetValueOrDefault() != 1 ? 1 : (!square.HasValue ? 1 : 0)) != 0)
                    {
                        //this.label1.Visible = true;
                        this.lblnd.Visibility = Visibility = Visibility.Visible;
                        this.lblxgd.Visibility = Visibility = Visibility.Visible;
                        Decimal num2;
                        Decimal num3;
                        foreach (Sample sam in this.QPar.SamList)
                        {
                            if (num1 <= 7)
                            {
                                if (sam.Avalue != null && ((IEnumerable<Decimal>)sam.Avalue).Count<Decimal>() > 1)
                                {
                                    this.lblxgd.Content += "\r\n";
                                    foreach (Decimal num4 in sam.Avalue)
                                    {
                                        num2 = num4;
                                        this.lblxgd.Content = this.lblxgd.Content + num2.ToString(CommonFun.GetAcc("absAccuracy")) + ",";
                                    }
                                    this.lblxgd.Content = this.lblxgd.Content.ToString().Substring(0, this.lblxgd.Content.ToString().Length - 1);
                               //     Label lblnd = this.lblnd;
                                    string text = this.lblnd.Content.ToString();
                                    nullable = sam.ND;
                                    num3 = nullable.Value;
                                    string str1 = num3.ToString(CommonFun.GetAcc("ceAccuracy"));
                                    string str2 = text + "\r\n" + str1;
                                    lblnd.Content = str2;
                                    ++num1;
                                }
                                else
                                {
                                    //Label lblxgd = this.lblxgd;
                                    string text1 = this.lblxgd.Content.ToString();
                                    nullable = sam.XGD;
                                    num3 = nullable.Value;
                                    string str1 = num3.ToString(CommonFun.GetAcc("absAccuracy"));
                                    string str2 = text1 + "\r\n" + str1;
                                    lblxgd.Content = str2;
                                   // Label lblnd = this.lblnd;
                                    string text2 = this.lblnd.Content.ToString();
                                    nullable = sam.ND;
                                    num3 = nullable.Value;
                                    string str3 = num3.ToString(CommonFun.GetAcc("ceAccuracy"));
                                    string str4 = text2 + "\r\n" + str3;
                                    lblnd.Content = str4;
                                    ++num1;
                                }
                            }
                            else if (num1 <= 14)
                            {
                                if (sam.Avalue != null && ((IEnumerable<Decimal>)sam.Avalue).Count<Decimal>() > 1)
                                {
                                    this.lblxgd1.Content += "\r\n";
                                    foreach (Decimal num4 in sam.Avalue)
                                    {
                                        num2 = num4;
                                        this.lblxgd1.Content = this.lblxgd1.Content + num2.ToString(CommonFun.GetAcc("absAccuracy")) + ",";
                                    }
                                    this.lblxgd1.Content = this.lblxgd1.Content.ToString().Substring(0, this.lblxgd1.Content.ToString().Length - 1);
                                   //Label lblnd1 = this.lblnd1;
                                    string text = this.lblnd1.Content.ToString();
                                    nullable = sam.ND;
                                    num3 = nullable.Value;
                                    string str1 = num3.ToString(CommonFun.GetAcc("ceAccuracy"));
                                    string str2 = text + "\r\n" + str1;
                                    lblnd1.Content = str2;
                                    ++num1;
                                }
                                else
                                {
                                    //Label lblxgd1 = this.lblxgd1;
                                    string text1 = this.lblxgd1.Content.ToString();
                                    nullable = sam.XGD;
                                    num3 = nullable.Value;
                                    string str1 = num3.ToString(CommonFun.GetAcc("absAccuracy"));
                                    string str2 = text1 + "\r\n" + str1;
                                    lblxgd1.Content = str2;
                                   // Label lblnd1 = this.lblnd1;
                                    string text2 = this.lblnd1.Content.ToString();
                                    nullable = sam.ND;
                                    num3 = nullable.Value;
                                    string str3 = num3.ToString(CommonFun.GetAcc("ceAccuracy"));
                                    string str4 = text2 + "\r\n" + str3;
                                    lblnd1.Content = str4;
                                    ++num1;
                                }
                            }
                            else if (num1 <= 21)
                            {
                                if (sam.Avalue != null && ((IEnumerable<Decimal>)sam.Avalue).Count<Decimal>() > 1)
                                {
                                    this.lblxgd2.Content += "\r\n";
                                    foreach (Decimal num4 in sam.Avalue)
                                    {
                                        num2 = num4;
                                        this.lblxgd2.Content = this.lblxgd2.Content + num2.ToString(CommonFun.GetAcc("absAccuracy")) + ",";
                                    }
                                    this.lblxgd2.Content = this.lblxgd2.Content.ToString().Substring(0, this.lblxgd2.Content.ToString().Length - 1);
                                 //   Label lblnd2 = this.lblnd2;
                                    string text = this.lblnd2.Content.ToString();
                                    nullable = sam.ND;
                                    num3 = nullable.Value;
                                    string str1 = num3.ToString(CommonFun.GetAcc("ceAccuracy"));
                                    string str2 = text + "\r\n" + str1;
                                    lblnd2.Content = str2;
                                    ++num1;
                                }
                                else
                                {
                                   // Label lblxgd2 = this.lblxgd2;
                                    string text1 = this.lblxgd2.Content.ToString();
                                    nullable = sam.XGD;
                                    num3 = nullable.Value;
                                    string str1 = num3.ToString(CommonFun.GetAcc("absAccuracy"));
                                    string str2 = text1 + "\r\n" + str1;
                                    lblxgd2.Content = str2;
                                   // Label lblnd2 = this.lblnd2;
                                    string text2 = this.lblnd2.Content.ToString();
                                    nullable = sam.ND;
                                    num3 = nullable.Value;
                                    string str3 = num3.ToString(CommonFun.GetAcc("ceAccuracy"));
                                    string str4 = text2 + "\r\n" + str3;
                                    lblnd2.Content = str4;
                                    ++num1;
                                }
                            }
                        }
                    }
                    else
                    {
                       // this.label1.Visible = false;
                        this.lblnd.Visibility = Visibility.Hidden;
                        this.lblxgd.Visibility = Visibility.Hidden;
                        this.lblnd1.Visibility = Visibility.Hidden;
                        this.lblxgd1.Visibility = Visibility.Hidden;
                        this.lblnd2.Visibility = Visibility.Hidden;
                        this.lblxgd2.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    //this.label1.Visible = false;
                    this.lblnd.Visibility = Visibility.Hidden;
                    this.lblxgd.Visibility = Visibility.Hidden;
                    this.lblnd1.Visibility = Visibility.Hidden;
                    this.lblxgd1.Visibility = Visibility.Hidden;
                    this.lblnd2.Visibility = Visibility.Hidden;
                    this.lblxgd2.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                /*this.lblMeaMeth.Content = CommonFun.GetLanText(this.lanvalue, "biogmethod");
                this.lblMMV.Tag = (object)null;
                if (this.QPar.QPar.MeasureMethodName == "dna1" || this.QPar.QPar.MeasureMethodName == "dna2")
                {
                    this.lblCabMethodV.Content = "";
                    this.lblFittingV.Text = "";
                    this.lblSamCntV.Text = "";
                    this.lblZeroBV.Text = "";
                    this.lblfcv.Text = "";
                    this.lblfcs.Visible = true;
                    this.panel5.Visible = false;
                    this.picSD.Visible = false;
                    this.panel4.BorderStyle = BorderStyle.FixedSingle;
                    this.lblfcs.Text += "\r\n\r\n";
                    string[] strArray = this.QPar.QPar.R.Split(',');
                    if (this.QPar.QPar.BackWL.Length > 0)
                        this.lblfcs.Text = "C(DNA)=(A1-Aref)*" + strArray[0] + "-(A2-Aref)*" + strArray[1] + "\r\n\r\nC(protein)=(A2-Aref)*" + strArray[2] + "-(A1-Aref)*" + strArray[3] + "\r\n\r\nRation=(A1-Aref)/(A2-Aref)\r\n";
                    else
                        this.lblfcs.Text = "C(DNA)=A1*" + strArray[0] + "-A2*" + strArray[1] + "\r\n\r\nC(protein)=A2*" + strArray[2] + "-A1*K4" + strArray[3] + "\r\n\r\nRation=A1/A2\r\n";
                }
                else
                {
                    this.picSD.Visible = true;
                    this.panel5.Visible = true;
                    this.panel4.BorderStyle = BorderStyle.None;
                    this.lblfcs.Visible = false;
                    this.lblCabMethodV.Text = this.QPar.QPar.CabMethod;
                    this.lblCabMethodV.Tag = (object)this.QPar.QPar.CabMethodDM;
                    this.lblFittingV.Text = this.QPar.QPar.Fitting;
                    this.lblFittingV.Tag = (object)this.QPar.QPar.FittingDM;
                    this.lblfcv.Text = this.QPar.QPar.Equation;
                    this.lblSamCntV.Text = this.QPar.QPar.SamCnt.ToString();
                    this.lblZeroBV.Text = this.QPar.QPar.ZeroB ? CommonFun.GetLanText(this.lanvalue, "active") : CommonFun.GetLanText(this.lanvalue, "closed");
                    if (this.QPar.QPar.Equation == "C=f(Abs)")
                        this.lblsc.Text = CommonFun.GetLanText(this.lanvalue, "standardcurveequation") + ": " + this.QPar.CFCS;
                    else
                        this.lblsc.Text = CommonFun.GetLanText(this.lanvalue, "standardcurveequation") + ": " + this.QPar.AFCS;
                    Decimal? nullable;
                    if (this.QPar.R.HasValue)
                    {
                        Label lblsc = this.lblsc;
                        string text = this.lblsc.Text;
                        nullable = this.QPar.R;
                        string str1 = nullable.Value.ToString("f5");
                        string str2 = text + ",  R=" + str1;
                        lblsc.Text = str2;
                    }
                    int num = 1;
                    this.lblxgd.Text = CommonFun.GetLanText(this.lanvalue, "Abs");
                    this.lblnd.Text = CommonFun.GetLanText(this.lanvalue, "conce");
                    this.lblxgd1.Text = CommonFun.GetLanText(this.lanvalue, "Abs");
                    this.lblnd1.Text = CommonFun.GetLanText(this.lanvalue, "conce");
                    this.lblxgd2.Text = CommonFun.GetLanText(this.lanvalue, "Abs");
                    this.lblnd2.Text = CommonFun.GetLanText(this.lanvalue, "conce");
                    if (this.QPar.SamList != null && this.QPar.SamList.Count > 0)
                    {
                        this.label1.Visible = true;
                        this.lblnd.Visible = true;
                        this.lblxgd.Visible = true;
                        foreach (Sample sam in this.QPar.SamList)
                        {
                            if (num <= 7)
                            {
                                Label lblxgd = this.lblxgd;
                                string text1 = this.lblxgd.Text;
                                nullable = sam.XGD;
                                string str1 = nullable.Value.ToString(CommonFun.GetAcc("absAccuracy"));
                                string str2 = text1 + "\r\n" + str1;
                                lblxgd.Text = str2;
                                Label lblnd = this.lblnd;
                                string text2 = this.lblnd.Text;
                                nullable = sam.ND;
                                string str3 = nullable.Value.ToString(CommonFun.GetAcc("ceAccuracy"));
                                string str4 = text2 + "\r\n" + str3;
                                lblnd.Text = str4;
                                ++num;
                            }
                            else if (num <= 14)
                            {
                                Label lblxgd1 = this.lblxgd1;
                                string text1 = this.lblxgd1.Text;
                                nullable = sam.XGD;
                                string str1 = nullable.Value.ToString(CommonFun.GetAcc("absAccuracy"));
                                string str2 = text1 + "\r\n" + str1;
                                lblxgd1.Text = str2;
                                Label lblnd1 = this.lblnd1;
                                string text2 = this.lblnd1.Text;
                                nullable = sam.ND;
                                string str3 = nullable.Value.ToString(CommonFun.GetAcc("ceAccuracy"));
                                string str4 = text2 + "\r\n" + str3;
                                lblnd1.Text = str4;
                                ++num;
                            }
                            else if (num <= 21)
                            {
                                Label lblxgd2 = this.lblxgd2;
                                string text1 = this.lblxgd2.Text;
                                nullable = sam.XGD;
                                string str1 = nullable.Value.ToString(CommonFun.GetAcc("absAccuracy"));
                                string str2 = text1 + "\r\n" + str1;
                                lblxgd2.Text = str2;
                                Label lblnd2 = this.lblnd2;
                                string text2 = this.lblnd2.Text;
                                nullable = sam.ND;
                                string str3 = nullable.Value.ToString(CommonFun.GetAcc("ceAccuracy"));
                                string str4 = text2 + "\r\n" + str3;
                                lblnd2.Text = str4;
                                ++num;
                            }
                        }
                    }
                    else
                    {
                        this.label1.Visible = false;
                        this.lblnd.Visible = false;
                        this.lblxgd.Visible = false;
                        this.lblnd1.Visible = false;
                        this.lblxgd1.Visible = false;
                        this.lblnd2.Visible = false;
                        this.lblxgd2.Visible = false;
                    }
                }*/
            }
           // this.lblgcv.Content = this.QPar.QPar.Length;
            this.lblWlV.Content = this.QPar.QPar.WL;
          //  this.lblMcntV.Content = this.QPar.QPar.MCnt.ToString();
            this.lblMeaMeth.Content = this.QPar.QPar.MeasureMethodName;
            this.lblOpticalPath.Content = this.QPar.QPar.Length;
            //  this.lbleconvertV.Content = this.QPar.QPar.EConvert ? CommonFun.GetLanText(this.lanvalue, "active") : CommonFun.GetLanText(this.lanvalue, "closed");
            /* if (this.QPar.QPar.Limits.Length > 0)
             {
                 if (((IEnumerable<string>)this.QPar.QPar.Limits.Split(',')).Count<string>() > 1)
                     this.lblThresholdV.Text = CommonFun.GetLanText(this.lanvalue, "maxv") + ": " + this.QPar.QPar.Limits.Split(',')[0] + "," + CommonFun.GetLanText(this.lanvalue, "minv") + ": " + this.QPar.QPar.Limits.Split(',')[1];
                 else
                     this.lblThresholdV.Text = this.QPar.QPar.Limits;
             }
             else
                 this.lblThresholdV.Text = "";*/
            this.lblMeasureunit.Content = this.QPar.QPar.Unit.Trim().Replace(">", "");
            this.DrawCurve();
        }

        private void New_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            /*bool newmethod = true;
            // QuantitationNewMethod quantationMethod = new QuantitationNewMethod(wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method,newmethod);
           
         

            QuantitationNewMethod quantationMethod = new QuantitationNewMethod(QM, wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method, newmethod);
             quantationMethod.ShowDialog();

            if(quantationMethod.cancel_ == true)
            {
                lblMeaMeth.Content = QM.QPar.MeasureMethodName;
                lblWlV.Content = QM.QPar.WL;
                lblOpticalPath.Content = optical_path;
                lblequationV.Content = QM.QPar.Equation;
                lblfittingV.Content = QM.QPar.Fitting;
                lblZeroInc.Content = QM.QPar.ZeroB;
                lblCabMethodV.Content = QM.QPar.CabMethod;
                lblCountStandSample.Content = count_standard_sample;
                lblMeasureunit.Content = QM.QPar.Unit;
            }*/
        }

        private void Open_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Open_File();
        }
        bool shifrTrueFalse = false;
        public void Open_File()
        {

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Сохраненные шаблоны");

            OpenFrm openFrm = new OpenFrm(Directory.GetCurrentDirectory() + @"\Сохраненные шаблоны", extension);
            openFrm.ShowDialog();
            if (openFrm.open_name != null)
            {
                filepath = Directory.GetCurrentDirectory() + "/Сохраненные шаблоны/" + openFrm.open_name;
                Decriptor64 decriptorfile = new Decriptor64(filepath, pathTemp, ref shifrTrueFalse);
                if (shifrTrueFalse == false)
                {
                    CommonFun.showbox("Формат файла не поддерживается!", "Info");
                    return;
                }
                else
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(pathTemp + "/" + openFrm.open_name);

                    XDocument xdoc = XDocument.Load(pathTemp + "/" + openFrm.open_name);
                    XmlNodeList nodes = xDoc.ChildNodes;

                    foreach (XmlNode n in nodes)
                    {
                        if ("Data_Izmerenie".Equals(n.Name))
                        {
                            for (XmlNode d = n.FirstChild; d != null; d = d.NextSibling)
                            {
                                if ("IzmerenieParametr".Equals(d.Name))
                                {
                                    //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("MeasureMethodName".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.MeasureMethodName = k.FirstChild.Value;
                                        }
                                        if ("WL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.WL = k.FirstChild.Value;
                                        }
                                        if ("CountMea".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.MCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("CountStandard".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.SamCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("Length".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Length = k.FirstChild.Value;
                                        }
                                        if ("Equation".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Equation = k.FirstChild.Value;
                                        }
                                        if ("Fitting".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Fitting = k.FirstChild.Value;
                                        }
                                        if ("Zero".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.ZeroB = Convert.ToBoolean(k.FirstChild.Value);
                                        }
                                        if ("CabMethod".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.CabMethod = k.FirstChild.Value;
                                        }
                                        if ("Unit".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Unit = k.FirstChild.Value;
                                        }
                                        if ("Square".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.MeasureMethod.Square = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("MeasureMethodName".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.MeasureMethodName = k.FirstChild.Value;
                                        }


                                    }


                                }
                                if ("Izmerenie".Equals(d.Name))
                                {
                                    if (this.QPar.QPar.CabMethod != "Ввод коэффициентов уравнения")
                                    {
                                        if (this.QPar.SamList == null)
                                            this.QPar.SamList = new List<Sample>();
                                        if (this.QPar.SamList.Count<Sample>() > this.QPar.QPar.SamCnt)
                                            this.QPar.SamList = new List<Sample>();
                                        while (this.QPar.SamList.Count<Sample>() < this.QPar.QPar.SamCnt)
                                            this.QPar.SamList.Add(new Sample()
                                            {
                                                IsExclude = false
                                            });
                                    }
                                    int stolbec = 0;
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {


                                        // Обрабатываем в цикле только Stroka
                                        if ("Stroka".Equals(k.Name))
                                        {

                                            for (XmlNode m = k.FirstChild; m != null; m = m.NextSibling)
                                            {
                                                if ("ND".Equals(m.Name) && m.FirstChild != null)
                                                {
                                                    this.QPar.SamList[stolbec].ND = Convert.ToDecimal(m.FirstChild.Value, new CultureInfo("en-US"));
                                                }
                                                if ("XGD".Equals(m.Name) && m.FirstChild != null)
                                                {
                                                    this.QPar.SamList[stolbec].XGD = Convert.ToDecimal(m.FirstChild.Value, new CultureInfo("en-US"));
                                                }
                                                if ("IsExclude".Equals(m.Name) && m.FirstChild != null)
                                                {
                                                    this.QPar.SamList[stolbec].IsExclude = Convert.ToBoolean(m.FirstChild.Value, new CultureInfo("en-US"));
                                                }
                                                if ("C_bz".Equals(m.Name) && m.FirstChild != null)
                                                {
                                                    this.QPar.SamList[stolbec].C_bz = m.FirstChild.Value;
                                                }
                                                if ("D_sj".Equals(m.Name) && m.FirstChild != null)
                                                {
                                                    this.QPar.SamList[stolbec].D_sj = Convert.ToDateTime(m.FirstChild.Value, new CultureInfo("en-US"));
                                                }
                                                if ("Avalue".Equals(m.Name) && m.FirstChild != null)
                                                {
                                                    for (XmlNode a = m.FirstChild; a != null; a = a.NextSibling)
                                                    {
                                                        if (a.FirstChild != null)
                                                        {
                                                            string[] avalue = new string[a.FirstChild.Value.Split(',').Count()];
                                                            avalue = a.FirstChild.Value.Split(',');
                                                            this.QPar.SamList[stolbec].Avalue = new decimal[avalue.Count()];
                                                            for (int i = 0; i < avalue.Count(); i++)
                                                            {
                                                                this.QPar.SamList[stolbec].Avalue[i] = Convert.ToDecimal(avalue[i], new CultureInfo("en-US"));
                                                            }
                                                        }
                                                    }
                                                }

                                            }


                                        }
                                        stolbec++;

                                        //if (this.QPar.QPar.CabMethod == "Ввод коэффициентов уравнения")
                                    //    {
                                            if ("K0".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K0 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                            if ("K1".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K1 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                            if ("K2".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K2 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                            if ("K3".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K3 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                            if ("K10".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K10 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                            if ("K11".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K11 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                            if ("K12".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K12 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                            if ("K13".Equals(k.Name) && k.FirstChild != null)
                                            {
                                                this.QPar.K13 = Convert.ToDecimal(k.FirstChild.Value, new CultureInfo("en-US"));
                                            }
                                     //   }

                                        if ("CFCS".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.CFCS = k.FirstChild.Value;
                                        }
                                        if ("AFCS".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.AFCS = k.FirstChild.Value;
                                        }

                                        if ("R".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QM.QPar.R = k.FirstChild.Value;
                                        }
                                    }


                                }
                            }
                        }
                    }





                }

                btnOK.IsEnabled = true;
                ShowQm();

            }
        }

        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".mqua";
        private void Save_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (SaveFrm save = new SaveFrm(extension, "Градуировка"))
            {
                save.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
                {
                     if (save.Name_file.Text.Length > 0)
                        {
                            filepath = Directory.GetCurrentDirectory() + @"\Сохраненные шаблоны";
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

            XmlNode DateTime1 = xd.CreateElement("DateTime"); // дата создания градуировки
            DateTime1.InnerText = DateTime.Now.ToString(); // и значение
            IzmerenieParametr.AppendChild(DateTime1); // и указываем кому принадлежит

            XmlNode MeasureMethodName = xd.CreateElement("MeasureMethodName"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.MeasureMethodName; // и значение
            IzmerenieParametr.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            XmlNode WL = xd.CreateElement("WL"); //длина волны
            WL.InnerText = this.QPar.QPar.WL; // и значение
            IzmerenieParametr.AppendChild(WL); // и указываем кому принадлежит 

            XmlNode CountMea = xd.CreateElement("CountMea"); //кол-во измерений
            CountMea.InnerText = this.QPar.QPar.MCnt.ToString(); // и значение
            IzmerenieParametr.AppendChild(CountMea); // и указываем кому принадлежит 

            XmlNode CountStandard = xd.CreateElement("CountStandard"); //количество образцов
            CountStandard.InnerText = this.QPar.QPar.SamCnt.ToString(); // и значение
            IzmerenieParametr.AppendChild(CountStandard); // и указываем кому принадлежит 

            XmlNode Length = xd.CreateElement("Length"); //длина луча
            Length.InnerText = this.QPar.QPar.Length; // и значение
            IzmerenieParametr.AppendChild(Length); // и указываем кому принадлежит  

            XmlNode Equation = xd.CreateElement("Equation"); //уравнение
            Equation.InnerText = this.QPar.QPar.Equation; // и значение
            IzmerenieParametr.AppendChild(Equation); // и указываем кому принадлежит            

            XmlNode Fitting = xd.CreateElement("Fitting"); //формула
            Fitting.InnerText = this.QPar.QPar.Fitting; // и значение
            IzmerenieParametr.AppendChild(Fitting); // и указываем кому принадлежит       

            XmlNode Zero = xd.CreateElement("Zero"); //нулевая точка
            Zero.InnerText = this.QPar.QPar.ZeroB.ToString(); // и значение
            IzmerenieParametr.AppendChild(Zero); // и указываем кому принадлежит    

            XmlNode CabMethod = xd.CreateElement("CabMethod"); //обнуление
            CabMethod.InnerText = this.QPar.QPar.CabMethod; // и значение
            IzmerenieParametr.AppendChild(CabMethod); // и указываем кому принадлежит        

            XmlNode CabMethodMD = xd.CreateElement("CabMethodMD"); //обнуление
            CabMethod.InnerText = this.QPar.QPar.CabMethod; // и значение
            IzmerenieParametr.AppendChild(CabMethod); // и указываем кому принадлежит      

            XmlNode Unit = xd.CreateElement("Unit"); //ед.измерения
            Unit.InnerText = this.QPar.QPar.Unit; // и значение
            IzmerenieParametr.AppendChild(Unit); // и указываем кому принадлежит       

            XmlNode Square = xd.CreateElement("Square"); //ед.измерения
            Square.InnerText = this.QPar.QPar.MeasureMethod.Square.ToString(); // и значение
            IzmerenieParametr.AppendChild(Unit); // и указываем кому принадлежит        

            xd.DocumentElement.AppendChild(IzmerenieParametr);

            XmlNode Izmerenie = xd.CreateElement("Izmerenie");

            if (this.QPar.QPar.CabMethod != "Ввод коэффициентов уравнения")
            {

                // string[] HeaderCells = new string[this.QM.QPar.SamCnt];
                // string[,] Cells1 = new string[this.QM.SamList.Count(), 5];

                for (int i = 0; i < this.QPar.QPar.SamCnt; i++)
                {
                    XmlNode Cells2 = xd.CreateElement("Stroka");

                    XmlNode attribute2 = xd.CreateElement("ND");//Концетрация
                    attribute2.InnerText = Convert.ToString(this.QPar.SamList[i].ND); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute2); // добавляем атрибут

                    XmlNode attribute3 = xd.CreateElement("XGD"); //Абсорбция
                    attribute3.InnerText = Convert.ToString(this.QPar.SamList[i].XGD); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute3); // добавляем атрибут

                    XmlNode attribute4 = xd.CreateElement("IsExclude");
                    attribute4.InnerText = Convert.ToString(this.QPar.SamList[i].IsExclude); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute4); // добавляем атрибут

                    XmlNode attribute5 = xd.CreateElement("C_bz");
                    attribute5.InnerText = Convert.ToString(this.QPar.SamList[i].C_bz); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute5); // добавляем атрибут

                    XmlNode attribute6 = xd.CreateElement("D_sj");
                    attribute6.InnerText = Convert.ToString(this.QPar.SamList[i].D_sj); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute6); // добавляем атрибут

                    XmlNode attribute7 = xd.CreateElement("Avalue");
                    attribute7.InnerText = Convert.ToString(this.QPar.SamList[i].Avalue); // устанавливаем значение атрибута
                    Cells2.AppendChild(attribute7); // добавляем атрибут

 

                    Izmerenie.AppendChild(Cells2);
                }

            }
           /* else
            {*/
                XmlNode K0 = xd.CreateElement("K0"); //длина луча
                K0.InnerText = this.QPar.K0.ToString(); // и значение
                Izmerenie.AppendChild(K0); // и указываем кому принадлежит  

                XmlNode K1 = xd.CreateElement("K1"); //длина луча
                K1.InnerText = this.QPar.K1.ToString(); // и значение
                Izmerenie.AppendChild(K1); // и указываем кому принадлежит  

                XmlNode K2 = xd.CreateElement("K2"); //длина луча
                K2.InnerText = this.QPar.K1.ToString(); // и значение
                Izmerenie.AppendChild(K2); // и указываем кому принадлежит  

                XmlNode K3 = xd.CreateElement("K3"); //длина луча
                K3.InnerText = this.QPar.K3.ToString(); // и значение
                Izmerenie.AppendChild(K3); // и указываем кому принадлежит  

                XmlNode K10 = xd.CreateElement("K10"); //длина луча
                K10.InnerText = this.QPar.K10.ToString(); // и значение
                Izmerenie.AppendChild(K10); // и указываем кому принадлежит  

                XmlNode K12 = xd.CreateElement("K12"); //длина луча
                K12.InnerText = this.QPar.K12.ToString(); // и значение
                Izmerenie.AppendChild(K12); // и указываем кому принадлежит  

                XmlNode K11 = xd.CreateElement("K11"); //длина луча
                K11.InnerText = this.QPar.K11.ToString(); // и значение
                Izmerenie.AppendChild(K11); // и указываем кому принадлежит  

                XmlNode K13 = xd.CreateElement("K13"); //длина луча
                K13.InnerText = this.QPar.K13.ToString(); // и значение
                Izmerenie.AppendChild(K13); // и указываем кому принадлежит        

         //   }

            XmlNode R = xd.CreateElement("R"); //
            if (this.QM.QPar.R != null)
            {
                R.InnerText = this.QM.QPar.R.ToString(); // и значение
            }
            else
            {
                R.InnerText = ""; // и значение
            }
            Izmerenie.AppendChild(R); // и указываем кому принадлежит  



            if (this.QPar.QPar.Equation == "C=f(Abs)")
            {
                XmlNode CFCS = xd.CreateElement("CFCS"); //
                CFCS.InnerText = this.QPar.CFCS.ToString(); // и значение
                Izmerenie.AppendChild(CFCS); // и указываем кому принадлежит  
            }
            else
            {
                XmlNode AFCS = xd.CreateElement("AFCS"); //
                AFCS.InnerText = this.QPar.AFCS.ToString(); // и значение
                Izmerenie.AppendChild(AFCS); // и указываем кому принадлежит  
            }

            xd.DocumentElement.AppendChild(Izmerenie);
            fs.Close();         // Закрываем поток  
            xd.Save(filepath); // Сохраняем файл  
        }
        private void CloseSettings_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
           
        }
        public void DrawCurve()
        {
            string str;
            string acc1;
            string acc2;
            if (this.QPar.QPar.Equation == "C=f(Abs)")
            {
                str = "XGD";
                acc1 = CommonFun.GetAcc("absAccuracy");
                acc2 = CommonFun.GetAcc("ceAccuracy");
            }
            else
            {
                str = "ND";
                acc1 = CommonFun.GetAcc("ceAccuracy");
                acc2 = CommonFun.GetAcc("absAccuracy");
            }
            int num1 = 10;
            float num2 = 0.0f;
            float num3 = 0.0f;
            List<Sample> source = new List<Sample>();
            float num4;
            float num5;
            float num6;
            float num7;
            if (this.QPar.QPar.MeasureMethodName.Contains("DNA"))
            {
                num4 = 0.0f;
                num5 = 100f;
                num6 = 0.0f;
                num7 = 20f;
            }
            else if (this.QPar.SamList != null && this.QPar.SamList.Count > 0)
            {
                Decimal? nullable1;
                Decimal? nullable2;
                Decimal? nullable3;
                float num8;
                float num9;
                if (str == "ND")
                {
                    for (int index = 0; index < this.QPar.SamList.Count; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.ND = this.QPar.SamList[index].ND;
                        Sample sample2 = sample1;
                        Decimal k3 = this.QPar.K3;
                        nullable1 = sample1.ND;
                        Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new Decimal?(k3 * nullable1.GetValueOrDefault());
                        nullable1 = nullable4;
                        nullable2 = sample1.ND;
                        Decimal? nullable5;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable5 = nullable3;
                        }
                        else
                            nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable5;
                        nullable2 = sample1.ND;
                        Decimal? nullable6;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable6 = nullable3;
                        }
                        else
                            nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable6;
                        Decimal k2 = this.QPar.K2;
                        nullable2 = sample1.ND;
                        Decimal? nullable7;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable7 = nullable3;
                        }
                        else
                            nullable7 = new Decimal?(k2 * nullable2.GetValueOrDefault());
                        nullable2 = nullable7;
                        nullable3 = sample1.ND;
                        nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable8;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable8 = nullable3;
                        }
                        else
                            nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable8;
                        Decimal k1 = this.QPar.K1;
                        nullable2 = sample1.ND;
                        Decimal? nullable9;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable9 = nullable3;
                        }
                        else
                            nullable9 = new Decimal?(k1 * nullable2.GetValueOrDefault());
                        nullable2 = nullable9;
                        Decimal? nullable10;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable10 = nullable3;
                        }
                        else
                            nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable10;
                        Decimal k0 = this.QPar.K0;
                        Decimal? nullable11;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable11 = nullable2;
                        }
                        else
                            nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k0);
                        sample2.XGD = nullable11;
                        source.Add(sample1);
                    }
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                    num8 = num3 = (float)nullable1.Value;
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                    num9 = num2 = (float)nullable1.Value;
                }
                else
                {
                    for (int index = 0; index < this.QPar.SamList.Count; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.XGD = this.QPar.SamList[index].XGD;
                        Sample sample2 = sample1;
                        Decimal k13 = this.QPar.K13;
                        nullable1 = sample1.XGD;
                        Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new Decimal?(k13 * nullable1.GetValueOrDefault());
                        nullable1 = nullable4;
                        nullable2 = sample1.XGD;
                        Decimal? nullable5;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable5 = nullable3;
                        }
                        else
                            nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable5;
                        nullable2 = sample1.XGD;
                        Decimal? nullable6;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable6 = nullable3;
                        }
                        else
                            nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable6;
                        Decimal k12 = this.QPar.K12;
                        nullable2 = sample1.XGD;
                        Decimal? nullable7;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable7 = nullable3;
                        }
                        else
                            nullable7 = new Decimal?(k12 * nullable2.GetValueOrDefault());
                        nullable2 = nullable7;
                        nullable3 = sample1.XGD;
                        nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable8;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable8 = nullable3;
                        }
                        else
                            nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable8;
                        Decimal k11 = this.QPar.K11;
                        nullable2 = sample1.XGD;
                        Decimal? nullable9;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable9 = nullable3;
                        }
                        else
                            nullable9 = new Decimal?(k11 * nullable2.GetValueOrDefault());
                        nullable2 = nullable9;
                        Decimal? nullable10;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable10 = nullable3;
                        }
                        else
                            nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable10;
                        Decimal k10 = this.QPar.K10;
                        Decimal? nullable11;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable11 = nullable2;
                        }
                        else
                            nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k10);
                        sample2.ND = nullable11;
                        source.Add(sample1);
                    }
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                    num8 = num3 = (float)nullable1.Value;
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                    num9 = num2 = (float)nullable1.Value;
                }
                num4 = (double)num8 >= 0.0 ? num8 * 0.8f : num8 * 1.2f;
                num5 = (double)num9 >= 0.0 ? num9 * 1.2f : num9 * 0.8f;
                if (str == "ND")
                {
                    Sample sample1 = new Sample();
                    sample1.ND = new Decimal?(Convert.ToDecimal(num4));
                    Sample sample2 = sample1;
                    Decimal k3_1 = this.QPar.K3;
                    nullable1 = sample1.ND;
                    Decimal? nullable4;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable4 = nullable2;
                    }
                    else
                        nullable4 = new Decimal?(k3_1 * nullable1.GetValueOrDefault());
                    nullable1 = nullable4;
                    nullable2 = sample1.ND;
                    Decimal? nullable5;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable5 = nullable3;
                    }
                    else
                        nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable5;
                    nullable2 = sample1.ND;
                    Decimal? nullable6;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable6 = nullable3;
                    }
                    else
                        nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable6;
                    Decimal k2_1 = this.QPar.K2;
                    nullable2 = sample1.ND;
                    Decimal? nullable7;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable7 = nullable3;
                    }
                    else
                        nullable7 = new Decimal?(k2_1 * nullable2.GetValueOrDefault());
                    nullable2 = nullable7;
                    nullable3 = sample1.ND;
                    nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                    Decimal? nullable8;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable8 = nullable3;
                    }
                    else
                        nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable8;
                    Decimal k1_1 = this.QPar.K1;
                    nullable2 = sample1.ND;
                    Decimal? nullable9;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable9 = nullable3;
                    }
                    else
                        nullable9 = new Decimal?(k1_1 * nullable2.GetValueOrDefault());
                    nullable2 = nullable9;
                    Decimal? nullable10;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable10 = nullable3;
                    }
                    else
                        nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable10;
                    Decimal k0_1 = this.QPar.K0;
                    Decimal? nullable11;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable11 = nullable2;
                    }
                    else
                        nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k0_1);
                    sample2.XGD = nullable11;
                    source.Add(sample1);
                    Sample sample3 = new Sample();
                    sample3.ND = new Decimal?(Convert.ToDecimal(num5));
                    Sample sample4 = sample3;
                    Decimal k3_2 = this.QPar.K3;
                    nullable1 = sample3.ND;
                    Decimal? nullable12;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable12 = nullable2;
                    }
                    else
                        nullable12 = new Decimal?(k3_2 * nullable1.GetValueOrDefault());
                    nullable1 = nullable12;
                    nullable2 = sample3.ND;
                    Decimal? nullable13;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable13 = nullable3;
                    }
                    else
                        nullable13 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable13;
                    nullable2 = sample3.ND;
                    Decimal? nullable14;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable14 = nullable3;
                    }
                    else
                        nullable14 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable14;
                    Decimal k2_2 = this.QPar.K2;
                    nullable2 = sample3.ND;
                    Decimal? nullable15;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable15 = nullable3;
                    }
                    else
                        nullable15 = new Decimal?(k2_2 * nullable2.GetValueOrDefault());
                    nullable2 = nullable15;
                    nullable3 = sample3.ND;
                    nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                    Decimal? nullable16;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable16 = nullable3;
                    }
                    else
                        nullable16 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable16;
                    Decimal k1_2 = this.QPar.K1;
                    nullable2 = sample3.ND;
                    Decimal? nullable17;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable17 = nullable3;
                    }
                    else
                        nullable17 = new Decimal?(k1_2 * nullable2.GetValueOrDefault());
                    nullable2 = nullable17;
                    Decimal? nullable18;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable18 = nullable3;
                    }
                    else
                        nullable18 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable18;
                    Decimal k0_2 = this.QPar.K0;
                    Decimal? nullable19;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable19 = nullable2;
                    }
                    else
                        nullable19 = new Decimal?(nullable1.GetValueOrDefault() + k0_2);
                    sample4.XGD = nullable19;
                    source.Add(sample3);
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                    float num10 = (float)nullable1.Value;
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                    float num11 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                    num6 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                    num7 = (float)nullable1.Value;
                    if ((double)num10 < (double)num6)
                        num6 = num10;
                    if ((double)num11 > (double)num7)
                        num7 = num11;
                }
                else
                {
                    Sample sample1 = new Sample();
                    sample1.XGD = new Decimal?(Convert.ToDecimal(num4));
                    Sample sample2 = sample1;
                    Decimal k13_1 = this.QPar.K13;
                    nullable1 = sample1.XGD;
                    Decimal? nullable4;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable4 = nullable2;
                    }
                    else
                        nullable4 = new Decimal?(k13_1 * nullable1.GetValueOrDefault());
                    nullable1 = nullable4;
                    nullable2 = sample1.XGD;
                    Decimal? nullable5;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable5 = nullable3;
                    }
                    else
                        nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable5;
                    nullable2 = sample1.XGD;
                    Decimal? nullable6;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable6 = nullable3;
                    }
                    else
                        nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable6;
                    Decimal k12_1 = this.QPar.K12;
                    nullable2 = sample1.XGD;
                    Decimal? nullable7;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable7 = nullable3;
                    }
                    else
                        nullable7 = new Decimal?(k12_1 * nullable2.GetValueOrDefault());
                    nullable2 = nullable7;
                    nullable3 = sample1.XGD;
                    nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                    Decimal? nullable8;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable8 = nullable3;
                    }
                    else
                        nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable8;
                    Decimal k11_1 = this.QPar.K11;
                    nullable2 = sample1.XGD;
                    Decimal? nullable9;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable9 = nullable3;
                    }
                    else
                        nullable9 = new Decimal?(k11_1 * nullable2.GetValueOrDefault());
                    nullable2 = nullable9;
                    Decimal? nullable10;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable10 = nullable3;
                    }
                    else
                        nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable10;
                    Decimal k10_1 = this.QPar.K10;
                    Decimal? nullable11;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable11 = nullable2;
                    }
                    else
                        nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k10_1);
                    sample2.ND = nullable11;
                    source.Add(sample1);
                    Sample sample3 = new Sample();
                    sample3.XGD = new Decimal?(Convert.ToDecimal(num5));
                    Sample sample4 = sample3;
                    Decimal k13_2 = this.QPar.K13;
                    nullable1 = sample3.XGD;
                    Decimal? nullable12;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable12 = nullable2;
                    }
                    else
                        nullable12 = new Decimal?(k13_2 * nullable1.GetValueOrDefault());
                    nullable1 = nullable12;
                    nullable2 = sample3.XGD;
                    Decimal? nullable13;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable13 = nullable3;
                    }
                    else
                        nullable13 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable13;
                    nullable2 = sample3.XGD;
                    Decimal? nullable14;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable14 = nullable3;
                    }
                    else
                        nullable14 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                    nullable1 = nullable14;
                    Decimal k12_2 = this.QPar.K12;
                    nullable2 = sample3.XGD;
                    Decimal? nullable15;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable15 = nullable3;
                    }
                    else
                        nullable15 = new Decimal?(k12_2 * nullable2.GetValueOrDefault());
                    nullable2 = nullable15;
                    nullable3 = sample3.XGD;
                    nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                    Decimal? nullable16;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable16 = nullable3;
                    }
                    else
                        nullable16 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable16;
                    Decimal k11_2 = this.QPar.K11;
                    nullable2 = sample3.XGD;
                    Decimal? nullable17;
                    if (!nullable2.HasValue)
                    {
                        nullable3 = new Decimal?();
                        nullable17 = nullable3;
                    }
                    else
                        nullable17 = new Decimal?(k11_2 * nullable2.GetValueOrDefault());
                    nullable2 = nullable17;
                    Decimal? nullable18;
                    if (!(nullable1.HasValue & nullable2.HasValue))
                    {
                        nullable3 = new Decimal?();
                        nullable18 = nullable3;
                    }
                    else
                        nullable18 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                    nullable1 = nullable18;
                    Decimal k10_2 = this.QPar.K10;
                    Decimal? nullable19;
                    if (!nullable1.HasValue)
                    {
                        nullable2 = new Decimal?();
                        nullable19 = nullable2;
                    }
                    else
                        nullable19 = new Decimal?(nullable1.GetValueOrDefault() + k10_2);
                    sample4.ND = nullable19;
                    source.Add(sample3);
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                    float num10 = (float)nullable1.Value;
                    nullable1 = this.QPar.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                    float num11 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                    num6 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
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
                    Decimal? nullable1;
                    for (int index = 0; index < 100; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.ND = new Decimal?(Convert.ToDecimal(index));
                        Sample sample2 = sample1;
                        Decimal k3 = this.QPar.K3;
                        nullable1 = sample1.ND;
                        Decimal? nullable2;
                        Decimal? nullable3;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable3 = nullable2;
                        }
                        else
                            nullable3 = new Decimal?(k3 * nullable1.GetValueOrDefault());
                        nullable1 = nullable3;
                        nullable2 = sample1.ND;
                        Decimal? nullable4;
                        Decimal? nullable5;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable5 = nullable4;
                        }
                        else
                            nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable5;
                        nullable2 = sample1.ND;
                        Decimal? nullable6;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable6 = nullable4;
                        }
                        else
                            nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable6;
                        Decimal k2 = this.QPar.K2;
                        nullable2 = sample1.ND;
                        Decimal? nullable7;
                        if (!nullable2.HasValue)
                        {
                            nullable4 = new Decimal?();
                            nullable7 = nullable4;
                        }
                        else
                            nullable7 = new Decimal?(k2 * nullable2.GetValueOrDefault());
                        nullable2 = nullable7;
                        nullable4 = sample1.ND;
                        nullable2 = nullable2.HasValue & nullable4.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable8;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable8 = nullable4;
                        }
                        else
                            nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable8;
                        Decimal k1 = this.QPar.K1;
                        nullable2 = sample1.ND;
                        Decimal? nullable9;
                        if (!nullable2.HasValue)
                        {
                            nullable4 = new Decimal?();
                            nullable9 = nullable4;
                        }
                        else
                            nullable9 = new Decimal?(k1 * nullable2.GetValueOrDefault());
                        nullable2 = nullable9;
                        Decimal? nullable10;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable10 = nullable4;
                        }
                        else
                            nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable10;
                        Decimal k0 = this.QPar.K0;
                        Decimal? nullable11;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable11 = nullable2;
                        }
                        else
                            nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k0);
                        sample2.XGD = nullable11;
                        source.Add(sample1);
                    }
                    num3 = 0.0f;
                    num2 = 100f;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                    float num8 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                    float num9 = (float)nullable1.Value;
                    if ((double)num8 < (double)num6)
                        num6 = num8;
                    if ((double)num9 > (double)num7)
                        num7 = num9;
                }
                else
                {
                    Decimal? nullable1;
                    for (int index = 0; index < 100; ++index)
                    {
                        Sample sample1 = new Sample();
                        sample1.XGD = new Decimal?(Convert.ToDecimal(index));
                        Sample sample2 = sample1;
                        Decimal k13 = this.QPar.K13;
                        nullable1 = sample1.XGD;
                        Decimal? nullable2;
                        Decimal? nullable3;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable3 = nullable2;
                        }
                        else
                            nullable3 = new Decimal?(k13 * nullable1.GetValueOrDefault());
                        nullable1 = nullable3;
                        nullable2 = sample1.XGD;
                        Decimal? nullable4;
                        Decimal? nullable5;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable5 = nullable4;
                        }
                        else
                            nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable5;
                        nullable2 = sample1.XGD;
                        Decimal? nullable6;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable6 = nullable4;
                        }
                        else
                            nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable6;
                        Decimal k12 = this.QPar.K12;
                        nullable2 = sample1.XGD;
                        Decimal? nullable7;
                        if (!nullable2.HasValue)
                        {
                            nullable4 = new Decimal?();
                            nullable7 = nullable4;
                        }
                        else
                            nullable7 = new Decimal?(k12 * nullable2.GetValueOrDefault());
                        nullable2 = nullable7;
                        nullable4 = sample1.XGD;
                        nullable2 = nullable2.HasValue & nullable4.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable8;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable8 = nullable4;
                        }
                        else
                            nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable8;
                        Decimal k11 = this.QPar.K11;
                        nullable2 = sample1.XGD;
                        Decimal? nullable9;
                        if (!nullable2.HasValue)
                        {
                            nullable4 = new Decimal?();
                            nullable9 = nullable4;
                        }
                        else
                            nullable9 = new Decimal?(k11 * nullable2.GetValueOrDefault());
                        nullable2 = nullable9;
                        Decimal? nullable10;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable4 = new Decimal?();
                            nullable10 = nullable4;
                        }
                        else
                            nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable10;
                        Decimal k10 = this.QPar.K10;
                        Decimal? nullable11;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable11 = nullable2;
                        }
                        else
                            nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k10);
                        sample2.ND = nullable11;
                        source.Add(sample1);
                    }
                    num3 = 0.0f;
                    num2 = 100f;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                    float num8 = (float)nullable1.Value;
                    nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                    float num9 = (float)nullable1.Value;
                    if ((double)num8 < (double)num6)
                        num6 = num8;
                    if ((double)num9 > (double)num7)
                        num7 = num9;
                }
            }
            Bitmap bitmap = new Bitmap(this.picSD.Width, this.picSD.Height);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.DrawRectangle(new Pen(Color.Black, 1f), 0, 0, this.picSD.Width - 1, this.picSD.Height - 1);
            graphics.FillRectangle((Brush)new SolidBrush(Color.White), 1, 1, this.picSD.Width - 2, this.picSD.Height - 2);
            SizeF sizeF1;
            SizeF sizeF2;
            if (source.Count > 0)
            {
                if (str == "ND")
                {
                    sizeF1 = graphics.MeasureString(Convert.ToDouble((object)source[source.Count - 1].ND).ToString(acc1), new Font("Segoe UI", (float)num1));
                    sizeF2 = graphics.MeasureString(Convert.ToDouble((object)source[source.Count - 1].XGD).ToString(acc2), new Font("Segoe UI", (float)num1));
                }
                else
                {
                    sizeF1 = graphics.MeasureString(Convert.ToDouble((object)source[source.Count - 1].XGD).ToString(acc1), new Font("Segoe UI", (float)num1));
                    sizeF2 = graphics.MeasureString(Convert.ToDouble((object)source[source.Count - 1].ND).ToString(acc2), new Font("Segoe UI", (float)num1));
                }
            }
            else
            {
                sizeF1 = graphics.MeasureString("100.0", new Font("Segoe UI", (float)num1));
                sizeF2 = graphics.MeasureString("20.00", new Font("Segoe UI", (float)num1));
            }
            float num12 = 20f + sizeF2.Height + sizeF2.Width;
            float num13 = (float)(this.picSD.Width - 20) - sizeF2.Width;
            float num14 = (float)((double)this.picSD.Height - (double)sizeF1.Height * 2.0 - 20.0);
            float num15 = sizeF1.Height + 20f + sizeF1.Height;
            RectangleF rectangleF = new RectangleF(num12, num15, num13 - num12, num14 - num15);
            if (this.QPar.C_mName != null && this.QPar.C_mName.Length > 0)
            {
                SizeF sizeF3 = graphics.MeasureString(this.QPar.C_mName, new Font("Segoe UI", (float)num1));
                graphics.DrawString(this.QPar.C_mName, new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(num12 + (float)(((double)num13 - (double)num12 - (double)sizeF3.Width) / 2.0), num15 - 20f));
            }
            graphics.DrawLine(new Pen(Color.Black, 1f), num12, num14, num13, num14);
            graphics.DrawLine(new Pen(Color.Black, 1f), num12, num15, num13, num15);
            graphics.DrawLine(new Pen(Color.Black, 1f), num12, num14, num12, num15);
            graphics.DrawLine(new Pen(Color.Black, 1f), num13, num14, num13, num15);
            float x1 = num12;
            float y1 = num14 + 5f;
            graphics.DrawString(num4.ToString(acc1), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x1, y1));
            SizeF sizeF4 = graphics.MeasureString(num5.ToString(acc1), new Font("Segoe UI", (float)num1));
            float x2 = num13 - sizeF4.Width;
            graphics.DrawString(num5.ToString(acc1), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x2, y1));
            SizeF sizeF5 = graphics.MeasureString(num6.ToString(acc2), new Font("Segoe UI", (float)num1));
            float x3 = num12 - sizeF5.Width;
            float y2 = num14 - sizeF5.Height / 2f;
            graphics.DrawString(num6.ToString(acc2), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x3, y2));
            SizeF sizeF6 = graphics.MeasureString(num7.ToString(acc2), new Font("Segoe UI", (float)num1));
            float x4 = num12 - sizeF6.Width;
            float y3 = num15 - sizeF6.Height / 2f;
            graphics.DrawString(num7.ToString(acc2), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x4, y3));
            for (int index = 1; index < 4; ++index)
            {
                Pen pen = new Pen(Color.Black, 1f);
                pen.DashStyle = DashStyle.Dot;
                graphics.DrawLine(pen, num12 + (float)(((double)num13 - (double)num12) * (double)index / 4.0), num14, num12 + (float)(((double)num13 - (double)num12) * (double)index / 4.0), num15);
                graphics.DrawLine(pen, num12, num15 + (float)(((double)num14 - (double)num15) * (double)index / 4.0), num13, num15 + (float)(((double)num14 - (double)num15) * (double)index / 4.0));
            }
            float x5 = num12 + (float)(((double)num13 - (double)num12 - (double)sizeF1.Width) / 2.0);
            float y4 = num14 + 5f;
            float y5 = num15 + (float)(((double)num14 - (double)num15 - (double)sizeF2.Width) / 2.0);
            if (str == "ND")
            {
                float x6 = num12 - graphics.MeasureString("Abs", new Font("Segoe UI", (float)num1)).Width;
                graphics.DrawString(this.QPar.QPar.Unit, new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
                graphics.DrawString("Abs", new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
            }
            else
            {
                float x6 = num12 - graphics.MeasureString(this.QPar.QPar.Unit, new Font("Segoe UI", (float)num1)).Width;
                graphics.DrawString(this.QPar.QPar.Unit, new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
                graphics.DrawString("Abs", new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
            }
            if ((double)num5 - (double)num4 == 0.0 || (double)num7 - (double)num6 == 0.0)
                return;
            double num16 = ((double)num13 - (double)num12) / ((double)num5 - (double)num4);
            double num17 = ((double)num14 - (double)num15) / ((double)num7 - (double)num6);
            if (source.Count > 0)
            {
                if (str == "ND")
                {
                    List<Sample> list = source.OrderBy<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).ToList<Sample>();
                    double num8 = (double)num12 + (Convert.ToDouble((object)list[list.Count<Sample>() - 1].ND) - (double)num4) * num16;
                    double num9 = (double)num14 - (Convert.ToDouble((object)list[list.Count<Sample>() - 1].XGD) - (double)num6) * num17;
                    for (int index = list.Count<Sample>() - 2; index >= 0; --index)
                    {
                        double num10 = (double)num12 + (Convert.ToDouble((object)list[index].ND) - (double)num4) * num16;
                        double num11 = Convert.ToDouble((object)list[index].XGD) >= (double)num6 ? (Convert.ToDouble((object)list[index].XGD) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)list[index].XGD) - (double)num6) * num17 : (double)num15) : (double)num14;
                        graphics.DrawLine(new Pen(Color.Red, 1f), (float)num8, (float)num9, (float)num10, (float)num11);
                        num8 = num10;
                        num9 = num11;
                    }
                    if (this.QPar.SamList != null && this.QPar.SamList.Count > 0)
                    {
                        for (int index = 0; index < this.QPar.SamList.Count; ++index)
                        {
                            double num10 = (double)num12 + (Convert.ToDouble((object)this.QPar.SamList[index].ND) - (double)num4) * num16;
                            double num11 = Convert.ToDouble((object)this.QPar.SamList[index].XGD) >= (double)num6 ? (Convert.ToDouble((object)this.QPar.SamList[index].XGD) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)this.QPar.SamList[index].XGD) - (double)num6) * num17 : (double)num15) : (double)num14;
                            graphics.DrawEllipse(new Pen(Color.Blue, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                        }
                    }
                }
                else
                {
                    List<Sample> list = source.OrderBy<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).ToList<Sample>();
                    double num8 = (double)num12 + (Convert.ToDouble((object)list[list.Count<Sample>() - 1].XGD) - (double)num4) * num16;
                    double num9 = (double)num14 - (Convert.ToDouble((object)list[list.Count<Sample>() - 1].ND) - (double)num6) * num17;
                    for (int index = list.Count<Sample>() - 2; index >= 0; --index)
                    {
                        double num10 = (double)num12 + (Convert.ToDouble((object)list[index].XGD) - (double)num4) * num16;
                        double num11 = Convert.ToDouble((object)list[index].ND) >= (double)num6 ? (Convert.ToDouble((object)list[index].ND) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)list[index].ND) - (double)num6) * num17 : (double)num15) : (double)num14;
                        graphics.DrawLine(new Pen(Color.Red, 1f), (float)num8, (float)num9, (float)num10, (float)num11);
                        num8 = num10;
                        num9 = num11;
                    }
                    if (this.QPar.SamList != null && this.QPar.SamList.Count > 0)
                    {
                        for (int index = 0; index < this.QPar.SamList.Count; ++index)
                        {
                            double num10 = (double)num12 + (Convert.ToDouble((object)this.QPar.SamList[index].XGD) - (double)num4) * num16;
                            double num11 = Convert.ToDouble((object)this.QPar.SamList[index].ND) >= (double)num6 ? (Convert.ToDouble((object)this.QPar.SamList[index].ND) <= (double)num7 ? (double)num14 - (Convert.ToDouble((object)this.QPar.SamList[index].ND) - (double)num6) * num17 : (double)num15) : (double)num14;
                            graphics.DrawEllipse(new Pen(Color.Blue, 2f), Convert.ToInt32(num10) - 1, Convert.ToInt32(num11) - 1, 3, 3);
                        }
                    }
                }
            }
            this.picSD.Image = (Image)bitmap;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.QPar.QPar.MeasureMethodName != null)
            {
                this.ShowQm();
                this.Save.IsEnabled = true;
            }
        }
    }
}
