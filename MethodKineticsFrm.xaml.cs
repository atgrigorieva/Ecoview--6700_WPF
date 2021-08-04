using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для MethodKineticsFrm.xaml
    /// </summary>
    public partial class MethodKineticsFrm : Window, IDisposable
    {
        public TimeMethod mpar = (TimeMethod)null;
        public MethodKineticsFrm(TimeMethod smpar)
        {
            InitializeComponent();
            this.pibbackwl.Tag = "off";
            this.pibEConvert.Tag = "off";
            this.pibAutoXY.Tag = "off";
            mpar = smpar;
            // mpar = new TimeMethod();
        }



        private void LblTotal_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void Interval_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void LblDeleytimeDifferentialTime_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void Coefficient_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void LblModeV_PreviewMouseDown(object sender, EventArgs e)
        {
            KineticsMode kineticsMode = new KineticsMode();
            kineticsMode.ShowDialog();
            if (kineticsMode.kinetics_mode != null && kineticsMode.kinetics_mode != "")
            {
                lblModeV.Content = kineticsMode.kinetics_mode.ToString() + " >";
                lblModeV.Tag = kineticsMode.kinetics_mode.ToString() + " >";
                if (kineticsMode.kinetics_mode.ToString() == CommonFun.GetLanText("kinetics"))
                {
                    this.lblBackwl.IsEnabled = true;
                    this.pibbackwl.IsEnabled = true;
                    this.lblR.Content = CommonFun.GetLanText("r");
                    this.lblRV.Content = this.mpar.Rate;
                    this.lbldlx.Content = CommonFun.GetLanText("delaytime") + "/" + CommonFun.GetLanText("Differentialtime") + "(сек)";
                    this.lbldlxv.Content = this.mpar.DelayTime;
                }
                else
                {
                    this.lblBackwl.IsEnabled = false;
                    this.pibbackwl.IsEnabled = false;
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                    bi3.EndInit();
                    this.pibbackwl.Source = bi3;
                    this.pibbackwl.Tag = (object)"off";
                    this.lblR.Content = CommonFun.GetLanText("linearpj") + "(%)";
                    this.lblRV.Content = this.mpar.Criterion + " >";
                    this.lbldlx.Content = CommonFun.GetLanText("Differentialtime") + "(сек)";
                    this.lbldlxv.Content = this.mpar.DiffInterval + " >";
                }
            }
        }

        private void LblWLV_PreviewMouseDown(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) =>
                {
                    frm.Activate();
                });
                frm.lbltitle.Text = CommonFun.GetLanText("wl");
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num > 1100M || num < 190M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            frm.wl = frm.txtValue.Text.ToString();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.Closing += ((param0, param1) =>
                {
                    try
                    {
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num > 1100M || num < 190M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            //   frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl);
                                if (num <= 1100M && num >= 190M)
                                {
                                    //   start_wl = num.ToString("f1");
                                    lblWLV.Content = num.ToString("f1") + " >";
                                }
                                else
                                {

                                }
                            }
                            //   frm.Close(); frm.Dispose();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
                frm.btnCancel.PreviewMouseDown += ((param0, param1) =>
                {
                    //if (!this.automode)
                    //    return;
                    frm.wl = null;
                    frm.Close(); frm.Dispose();
                    frm.Dispose();

                });
                object num1 = frm.ShowDialog();
            }
        }

        private void Pibbackwl_PreviewMouseDown(object sender, EventArgs e)
        {
            if (this.pibbackwl.Tag == (object)"on")
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                bi3.EndInit();
                this.pibbackwl.Source = bi3;
                this.pibbackwl.Tag = (object)"off";
                this.lblbackwlV.IsEnabled = false;
                this.lblBackwl.IsEnabled = false;
            }
            else
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                bi3.EndInit();
                this.pibbackwl.Source = bi3;
                this.pibbackwl.Tag = (object)"on";
                this.lblbackwlV.IsEnabled = true;
                this.lblBackwl.IsEnabled = true;
            }
        }
        private void LblTimeV_PreviewMouseDown(object sender, EventArgs e)
        {
            using (TimeIntervalFrm frm = new TimeIntervalFrm())
            {
                frm.lblTitle.Text = CommonFun.GetLanText("interval");
                frm.lblValue.Content = this.lblIntervalV.Content;
                frm.pibsec.Tag = (object)"on";
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.pibsec.Tag == (object)"on")
                            this.lblTimeV.Content = frm.lblValue.Content.ToString().Replace(" >", "") + " >";
                        else if (frm.pibmin.Tag == (object)"on")
                            this.lblTimeV.Content = (Convert.ToInt32(frm.lblValue.Content.ToString().Replace(" >", "")) * 60).ToString() + " >";
                        else if (frm.pibhour.Tag == (object)"on")
                            this.lblTimeV.Content = (Convert.ToInt32(frm.lblValue.Content.ToString().Replace(" >", "")) * 3600).ToString() + " >";
                        if (Convert.ToInt32(this.lblTimeV.Content.ToString().Replace(" >", "")) > 43200)
                            CommonFun.showbox(CommonFun.GetLanText("miniinterval"), "Warning");
                        frm.Close();
                    }
                    catch
                    {
                    }
                });
                frm.btnCancel.PreviewMouseDown += ((param0, param1) => frm.Close());
                frm.ShowDialog();
            }
        }

        private void LblIntervalV_PreviewMouseDown(object sender, EventArgs e)
        {
            using (TimeIntervalFrm frm = new TimeIntervalFrm())
            {
                frm.lblTitle.Text = CommonFun.GetLanText("interval");
                frm.lblValue.Content = this.lblIntervalV.Content;
                frm.pibsec.Tag = (object)"on";
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.pibsec.Tag == (object)"on")
                            this.lblIntervalV.Content = frm.lblValue.Content.ToString().Replace(" >", "") + " >";
                        else if (frm.pibmin.Tag == (object)"on")
                            this.lblIntervalV.Content = (Convert.ToInt32(frm.lblValue.Content.ToString().Replace(" >", "")) * 60).ToString() + " >";
                        else if (frm.pibhour.Tag == (object)"on")
                            this.lblIntervalV.Content = (Convert.ToInt32(frm.lblValue.Content.ToString().Replace(" >", "")) * 3600).ToString() + " >";
                        if (this.pibbackwl.Tag == (object)"on")
                        {
                            string text1 = this.lblWLV.Content.ToString().Replace(" >", "");
                            string text2 = this.lblbackwlV.Content.ToString().Replace(" >", "");
                            Decimal num1 = Math.Abs(Convert.ToDecimal(text1) - Convert.ToDecimal(text2));
                            int result = 0;
                            int num2 = Math.DivRem(Convert.ToInt32(num1), 10, out result);
                            if (Convert.ToDecimal(this.lblIntervalV.Content.ToString().Replace(" >", "")) < (Decimal)num2)
                            {
                                CommonFun.showbox(CommonFun.GetLanText("recommand"), "Warning");
                                return;
                            }
                        }
                        else if (this.lblTimeV.Content.ToString().Replace(" >", "") != "")
                        {
                            int int32 = Convert.ToInt32(this.lblTimeV.Content.ToString().Replace(" >", ""));
                            if (int32 > 43200)
                            {
                                int num = int32 / 43200;
                                if (Convert.ToInt32(this.lblIntervalV.Content.ToString().Replace(" >", "")) < num + 1)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText("miniinterval"), "Warning");
                                    return;
                                }
                            }
                        }
                        frm.Close();
                    }
                    catch
                    {
                    }
                });
                frm.btnCancel.PreviewMouseDown += ((param0, param1) => frm.Close());
                frm.ShowDialog();
            }
        }

        private void Lbldlxv_PreviewMouseDown(object sender, EventArgs e)
        {
            if (this.lblModeV.Content.ToString().Replace(" >", "") == CommonFun.GetLanText("kinetics"))
            {
                using (DelaytimeDifferential frm = new DelaytimeDifferential())
                {
                    frm.pibsec.Tag = (object)"on";
                    frm.lblTitle.Text = CommonFun.GetLanText("delaytime") + "/" + CommonFun.GetLanText("Differentialtime");

                    if (this.mpar.DelayTime != null)
                    {
                        frm.lblValue1.Content = this.mpar.DelayTime.Split('/')[0] + " >";
                        frm.lblValue2.Content = this.mpar.DelayTime.Split('/')[1] + " >";
                    }
                    frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                    {
                        try
                        {
                            if (frm.lblValue1.Content.ToString().Replace(" >", "").Length < 0 || frm.lblValue2.Content.ToString().Replace(" >", "").Length < 0)
                            {
                                CommonFun.showbox(CommonFun.GetLanText("maxv1"), "Error");
                            }
                            else
                            {
                                string str1 = "";
                                string str2 = "";
                                if (frm.pibsec.Tag == (object)"on")
                                {
                                    str1 = frm.lblValue1.Content.ToString().Replace(" >", "");
                                    str2 = frm.lblValue2.Content.ToString().Replace(" >", "");
                                }
                                else if (frm.pibmin.Tag == (object)"on")
                                {
                                    int num = Convert.ToInt32(frm.lblValue1.Content.ToString().Replace(" >", "")) * 60;
                                    str1 = num.ToString();
                                    num = Convert.ToInt32(frm.lblValue2.Content.ToString().Replace(" >", "")) * 60;
                                    str2 = num.ToString();
                                }
                                else if (frm.pibhour.Tag == (object)"on")
                                {
                                    int num = Convert.ToInt32(frm.lblValue1.Content.ToString().Replace(" >", "")) * 3600;
                                    str1 = num.ToString();
                                    num = Convert.ToInt32(frm.lblValue2.Content.ToString().Replace(" >", "")) * 3600;
                                    str2 = num.ToString();
                                }
                                this.lbldlxv.Content = str1 + "/" + str2 + " >";
                                frm.Close();
                            }
                        }
                        catch
                        {
                        }
                    });
                    frm.ShowDialog();
                }
            }
            else
            {
                using (DelaytimeDifferential frm = new DelaytimeDifferential())
                {
                    frm.lblTitle.Text = CommonFun.GetLanText("delaytime") + "/" + CommonFun.GetLanText("Differentialtime");
                    frm.pibsec.Tag = (object)"on";
                    frm.lblValue1.Content = this.lbldlxv.Content.ToString().Replace(" >", "");
                    frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                    {
                        try
                        {
                            if (frm.pibsec.Tag == (object)"on")
                                this.lblTimeV.Content = frm.lblValue1.Content.ToString().Replace(" >", "") + " >";
                            else if (frm.pibmin.Tag == (object)"on")
                                this.lblTimeV.Content = (Convert.ToInt32(frm.lblValue1.Content.ToString().Replace(" >", "")) * 60).ToString() + " >";
                            else if (frm.pibhour.Tag == (object)"on")
                                this.lblTimeV.Content = (Convert.ToInt32(frm.lblValue1.Content.ToString().Replace(" >", "")) * 3600).ToString() + " >";
                            if (Convert.ToInt32(this.lblTimeV.Content.ToString().Replace(" >", "")) > 43200)
                                CommonFun.showbox(CommonFun.GetLanText("miniinterval"), "Warning");
                            frm.Close();
                        }
                        catch
                        {
                        }
                    });
                    frm.ShowDialog();
                }
            }
        }

        private void LblXmax_PreviewMouseDown(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) =>
                {
                    frm.Activate();
                });
                frm.lbltitle.Text = CommonFun.GetLanText("wl");
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num > 1100M || num < 190M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            frm.wl = frm.txtValue.Text.ToString();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.Closing += ((param0, param1) =>
                {
                    try
                    {
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num > 1100M || num < 190M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            //   frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl);
                                if (num <= 1100M && num >= 190M)
                                {
                                    //   start_wl = num.ToString("f1");
                                    this.lblbackwlV.Content = num.ToString("f1") + " >";
                                }
                                else
                                {

                                }
                            }
                            //   frm.Close(); frm.Dispose();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
                frm.btnCancel.PreviewMouseDown += ((param0, param1) =>
                {
                    //if (!this.automode)
                    //    return;
                    frm.wl = null;
                    frm.Close(); frm.Dispose();
                    frm.Dispose();

                });
                object num1 = frm.ShowDialog();
            }
        }

        private void LblXMin_PreviewMouseDown(object sender, EventArgs e)
        {
            if (this.lblModeV.Content.ToString().Replace(" >", "") == CommonFun.GetLanText("kinetics"))
            {
                using (InputRKineticsFrm frm = new InputRKineticsFrm())
                {
                    if (this.lblRV.Content.ToString().Replace(" >", "") != "")
                    {
                        string[] strArray = this.lblRV.Content.ToString().Replace(" >", "").Split(',');
                        if (strArray[0] != null)
                            frm.lblR1V.Content = strArray[0];
                        if (strArray[1] != null)
                            frm.lblR2V.Content = strArray[1];
                        if (strArray[2] != null)
                            frm.lblR3V.Content = strArray[2];
                        if (strArray[3] != null)
                            frm.lblR4V.Content = strArray[3];
                    }
                    frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                    {
                        if (frm.lblR1V.Content.ToString().Replace(" >", "") == "" || frm.lblR2V.Content.ToString().Replace(" >", "") == "" || frm.lblR3V.Content.ToString().Replace(" >", "") == "" || frm.lblR4V.Content.ToString().Replace(" >", "") == "")
                        {
                            CommonFun.showbox(CommonFun.GetLanText("maxv1"), "Error");
                        }
                        else
                        {
                            this.lblRV.Content = frm.lblR1V.Content.ToString().Replace(" >", "") + "," + frm.lblR2V.Content.ToString().Replace(" >", "") + "," + frm.lblR3V.Content.ToString().Replace(" >", "") + "," + frm.lblR4V.Content.ToString().Replace(" >", "") + " >";
                            frm.Close();
                        }
                    });
                    frm.ShowDialog();
                }
            }
            else
            {
                using (InputDataFrm frm = new InputDataFrm())
                {
                    frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                    {
                        if (es.Key != Key.Return)
                            return;
                        try
                        {
                            if (frm.txtValue.Text.IndexOf('.') == 0)
                                frm.txtValue.Text = "0" + frm.txtValue.Text;
                            Convert.ToDecimal(frm.txtValue.Text);
                            this.lblRV.Content = frm.txtValue.Text + " >";
                            frm.Close();
                        }
                        catch
                        {
                            CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                        }
                    });
                    frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                    {
                        try
                        {
                            if (frm.txtValue.Text.IndexOf('.') == 0)
                                frm.txtValue.Text = "0" + frm.txtValue.Text;
                            Convert.ToDecimal(frm.txtValue.Text);
                            this.lblRV.Content = frm.txtValue.Text + " >";
                            frm.Close();
                        }
                        catch
                        {
                            CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                        }
                    });
                    frm.ShowDialog();
                }
            }
        }

        private void LblLengthV_PreviewMouseDown(object sender, EventArgs e)
        {
            OpticalPath wLOpticalPath = new OpticalPath(lblLengthV.Content.ToString().Replace(" >", ""));
            wLOpticalPath.ShowDialog();
            if (lblLengthV.Content.ToString().Replace(" >", "") != wLOpticalPath.optical_path)
            {
                lblLengthV.Content = wLOpticalPath.optical_path.ToString() + " >";

            }
        }

        private void PibEConvert_PreviewMouseDown(object sender, EventArgs e)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            if (this.pibEConvert.Tag == (object)"on")
            {
                bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                bi3.EndInit();
                this.pibEConvert.Source = bi3;
                this.pibEConvert.Tag = (object)"off";
            }
            else
            {
                bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                bi3.EndInit();
                this.pibEConvert.Source = bi3;
                this.pibEConvert.Tag = (object)"on";
            }
        }

        private void PibAutoXY_PreviewMouseDown(object sender, EventArgs e)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            if (this.pibAutoXY.Tag == (object)"on")
            {
                bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                bi3.EndInit();
                this.pibAutoXY.Source = bi3;
                this.pibAutoXY.Tag = (object)"off";
                this.lblYMax.IsEnabled = true;
                this.lblYMaxV.IsEnabled = true;
                this.lblYMin.IsEnabled = true;
                this.lblYMinV.IsEnabled = true;
                this.lblYMinV.Content = "-4" + " >";
                this.lblYMaxV.Content = "4" + " >";
            }
            else
            {
                bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                bi3.EndInit();
                this.pibAutoXY.Source = bi3;
                this.pibAutoXY.Tag = (object)"on";
                this.lblYMax.IsEnabled = false;
                this.lblYMaxV.IsEnabled = false;
                this.lblYMin.IsEnabled = false;
                this.lblYMinV.IsEnabled = false;
            }
        }

        private void YMax_PreviewMouseDown(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num != 0M && Math.Abs(num) < 0.00001M)
                            frm.txtValue.Text = "0.00001";
                        this.lblYMaxV.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num != 0M && Math.Abs(num) < 0.00001M)
                            frm.txtValue.Text = "0.00001";
                        this.lblYMaxV.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("minv1"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void YMin_PreviewMouseDown(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num != 0M && Math.Abs(num) < 0.00001M)
                            frm.txtValue.Text = "-0.00001";
                        this.lblYMinV.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text);
                        if (num != 0M && Math.Abs(num) < 0.00001M)
                            frm.txtValue.Text = "-0.00001";
                        this.lblYMinV.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("minv1"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void Finish_PreviewMouseDown(object sender, EventArgs e)
        {

        }
        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".mkin";
        private void Save_PreviewMouseDown(object sender, EventArgs e)
        {
            using (SaveFrm save = new SaveFrm(extension, "Настройки кинетического режима"))
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
            xtw.WriteStartElement("Data_Settings");
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
            C_modeDM.InnerText = lblModeV.Content.ToString().Replace(" >", "");// и значение
            Settings.AppendChild(C_modeDM); // и указываем кому принадлежит 

            XmlNode Interval = xd.CreateElement("Interval"); // дата создания настройки
            Interval.InnerText = lblIntervalV.Content.ToString().Replace(" >", "");// и значение
            Settings.AppendChild(Interval); // и указываем кому принадлежит

            XmlNode Length = xd.CreateElement("Length"); // дата создания настройки
            Length.InnerText = lblLengthV.Content.ToString().Replace(" >", "");// и значение
            Settings.AppendChild(Length); // и указываем кому принадлежит

            XmlNode Time = xd.CreateElement("Time"); // дата создания настройки
            Time.InnerText = lblTimeV.Content.ToString().Replace(" >", "");// и значение
            Settings.AppendChild(Time); // и указываем кому принадлежит

            XmlNode WL = xd.CreateElement("WL"); // дата создания настройки
            WL.InnerText = lblWLV.Content.ToString().Replace(" >", "");// и значение
            Settings.AppendChild(WL); // и указываем кому принадлежит

            XmlNode BackWL = xd.CreateElement("BackWL"); // дата создания настройки
            BackWL.InnerText = !(pibbackwl.Tag.ToString().ToString().Replace(" >", "") == "on") ? "" : lblbackwlV.Content.ToString().Replace(" >", "");// и значение
            Settings.AppendChild(BackWL); // и указываем кому принадлежит

            if (lblModeV.Content.ToString().Replace(" >", "") == CommonFun.GetLanText("kinetics"))
            {
                XmlNode DelayTime = xd.CreateElement("DelayTime"); // дата создания настройки
                DelayTime.InnerText = lbldlxv.Content.ToString().Replace(" >", "");// и значение
                Settings.AppendChild(DelayTime); // и указываем кому принадлежит

                XmlNode Rate = xd.CreateElement("Rate"); // дата создания настройки
                Rate.InnerText = lblRV.Content.ToString().Replace(" >", "");// и значение
                Settings.AppendChild(Rate); // и указываем кому принадлежит

            }
            else
            {
                XmlNode DiffInterval = xd.CreateElement("DelayTime"); // дата создания настройки
                DiffInterval.InnerText = lbldlxv.Content.ToString().Replace(" >", "");// и значение
                Settings.AppendChild(DiffInterval); // и указываем кому принадлежит

                XmlNode Criterion = xd.CreateElement("Criterion"); // дата создания настройки
                Criterion.InnerText = lblRV.Content.ToString().Replace(" >", "");// и значение
                Settings.AppendChild(Criterion); // и указываем кому принадлежит
            }

            XmlNode EConvert = xd.CreateElement("EConvert"); // дата создания настройки
            EConvert.InnerText = (!(pibEConvert.Tag == (object)"off")).ToString();// и значение
            Settings.AppendChild(EConvert); // и указываем кому принадлежит            


            XmlNode AutoXY = xd.CreateElement("AutoXY"); // дата создания настройки
            if (pibAutoXY.Tag == (object)"on")
                AutoXY.InnerText = true.ToString();// и значение
            else
                AutoXY.InnerText = false.ToString();// и значение
            Settings.AppendChild(AutoXY); // и указываем кому принадлежит

            XmlNode yMax = xd.CreateElement("yMax"); // дата создания настройки
            yMax.InnerText = lblYMaxV.Content.ToString().ToString().Replace(" >", "");// и значение
            Settings.AppendChild(yMax); // и указываем кому принадлежит

            XmlNode yMin = xd.CreateElement("yMin"); // дата создания настройки
            yMin.InnerText = lblYMinV.Content.ToString().ToString().Replace(" >", "");// и значение
            Settings.AppendChild(yMin); // и указываем кому принадлежит

            xd.DocumentElement.AppendChild(Settings);

            fs.Close();         // Закрываем поток  
            xd.Save(filepath); // Сохраняем файл  

        }

        private void Open_method_PreviewMouseDown(object sender, EventArgs e)
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

                    System.Xml.XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(pathTemp + "/" + openFrm.open_name);

                    XDocument xdoc = XDocument.Load(pathTemp + "/" + openFrm.open_name);
                    XmlNodeList nodes = xDoc.ChildNodes;

                    foreach (XmlNode n in nodes)
                    {
                        if ("Data_Settings".Equals(n.Name))
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
                                            lblModeV.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("Interval".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblIntervalV.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("Length".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblLengthV.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("Time".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblTimeV.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("WL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblWLV.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("BackWL".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            if (k.FirstChild.Value != "True")
                                            {
                                                BitmapImage bi3 = new BitmapImage();
                                                bi3.BeginInit();
                                                bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                                                bi3.EndInit();
                                                this.pibbackwl.Source = bi3;
                                                this.pibbackwl.Tag = (object)"off";
                                                this.lblbackwlV.IsEnabled = false;
                                                this.lblBackwl.IsEnabled = false;
                                            }
                                            else
                                            {
                                                BitmapImage bi3 = new BitmapImage();
                                                bi3.BeginInit();
                                                bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                                                bi3.EndInit();
                                                this.pibbackwl.Source = bi3;
                                                this.pibbackwl.Tag = (object)"on";
                                                this.lblbackwlV.IsEnabled = true;
                                                this.lblBackwl.IsEnabled = true;
                                                lblbackwlV.Content = k.FirstChild.Value + " >";
                                            }
                                        }
                                        if ("DelayTime".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lbldlxv.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("Rate".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblRV.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("DiffInterval".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lbldlxv.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("Criterion".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblRV.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("EConvert".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            BitmapImage bi3 = new BitmapImage();
                                            bi3.BeginInit();
                                            if (k.FirstChild.Value != "False")
                                            {
                                                bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                                                bi3.EndInit();
                                                this.pibEConvert.Source = bi3;
                                                this.pibEConvert.Tag = (object)"on";
                                            }
                                            else
                                            {
                                                bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                                                bi3.EndInit();
                                                this.pibEConvert.Source = bi3;
                                                this.pibEConvert.Tag = (object)"off";
                                            }
                                        }
                                        if ("AutoXY".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            BitmapImage bi3 = new BitmapImage();
                                            bi3.BeginInit();
                                            if (k.FirstChild.Value != "True")
                                            {
                                                bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                                                bi3.EndInit();
                                                this.pibAutoXY.Source = bi3;
                                                this.pibAutoXY.Tag = (object)"off";
                                                this.lblYMax.IsEnabled = true;
                                                this.lblYMaxV.IsEnabled = true;
                                                this.lblYMin.IsEnabled = true;
                                                this.lblYMinV.IsEnabled = true;
                                                this.lblYMinV.Content = "-4" + " >";
                                                this.lblYMaxV.Content = "4" + " >";
                                            }
                                            else
                                            {
                                                bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                                                bi3.EndInit();
                                                this.pibAutoXY.Source = bi3;
                                                this.pibAutoXY.Tag = (object)"on";
                                                this.lblYMax.IsEnabled = false;
                                                this.lblYMaxV.IsEnabled = false;
                                                this.lblYMin.IsEnabled = false;
                                                this.lblYMinV.IsEnabled = false;
                                            }
                                        }
                                        if ("yMax".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblYMaxV.Content = k.FirstChild.Value + " >";
                                        }

                                        if ("yMin".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            lblYMinV.Content = k.FirstChild.Value + " >";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Save.IsEnabled = true;
                    Finish.IsEnabled = true;
                    LeftSettings.IsEnabled = true;
                    RightSettings.IsEnabled = true;
                }
            }
        }


        private void New_method_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void CloseSettings_PreviewMouseDown(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.pibEConvert.Tag = (object)"off";


        }

        public void Dispose()
        {
            
        }
    }
}
