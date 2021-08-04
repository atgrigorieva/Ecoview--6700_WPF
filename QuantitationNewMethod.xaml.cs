using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для QuantitationNewMethod.xaml
    /// </summary>
    public partial class QuantitationNewMethod : Window, IDisposable
    {


        //public QuantitationNewMethod(string[] wl_mass, string step_interval, string count_measure, string optical_path, string curveEquation, string fittingMethod, string count_standard_sample, string measureunit, bool zero_intercept, string measure_method, string calibration_method, bool newmethod = false)

        public QuantitationNewMethod(QuaMethod qpar, string[] wl_mass, string step_interval, string count_measure, string optical_path, string curveEquation, string fittingMethod, string count_standard_sample, string measureunit, bool zero_intercept, string measure_method, string calibration_method, bool newmethod = false)
        {
            InitializeComponent();
            this.QPar = qpar;
            QM = qpar;

            if (newmethod == true)
             {

                this.measure_method = lblMeasurment.Content.ToString();
                this.wl_mass = new string[1];
                this.wl_mass[0] = wl_1.Content.ToString().Replace(',', '.');
                
                 if(measure_method == "Границы")
                 {
                     this.step_interval = wl_3.Content.ToString();
                 }
                 else
                 {
                     this.step_interval = "0";
                 }
                 this.count_measure = lblCountMeasure.Content.ToString();
                 this.optical_path = lblOpticalPath.Content.ToString();
                 this.curveEquation = lblStandardEquation.Content.ToString();
                 this.fittingMethod = lblfittingMethod.Content.ToString();
                 this.count_standard_sample = lblCountSample.Content.ToString();
                 this.measureunit = lblmeasureunit.Content.ToString();
                 this.zero_intercept = false;
                 this.calibration_method = lblCabMethodV.Content.ToString();
                 //  step_interval = 1
                 //      count_measure, optical_path, curveEquation, fittingMethod, count_standalblmeasureunit.Contentrd_sample, measureunit, zero_intercept, measure_method,calibration_method
             }
             else
             {
                 //Save.IsEnabled = true;
                 Next.IsEnabled = true;
                 LeftSettings.IsEnabled = true;
                 RightSettings.IsEnabled = true;
                 this.measure_method = measure_method;
                 lblMeasurment.Content = measure_method;
                 this.wl_mass = new string[wl_mass.Count()];
                 this.wl_mass = wl_mass;
                 switch (measure_method)
                 {
                     case ("Одноволновое"):
                         wl_1.Content = wl_mass[0].Replace('.', ',');
                         this.step_interval = "0";
                         Lambda1.IsEnabled = true;
                         Lambda2.IsEnabled = false;
                         Lambda3.IsEnabled = false;
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
                 }

                 this.count_measure = count_measure;
                 lblCountMeasure.Content = count_measure;

                 this.optical_path = optical_path;
                 lblOpticalPath.Content = optical_path;

                 this.curveEquation = curveEquation;
                 lblStandardEquation.Content = curveEquation;

                 this.fittingMethod = fittingMethod;
                 lblfittingMethod.Content = fittingMethod;

                 this.count_standard_sample = count_standard_sample;
                 lblCountSample.Content = count_standard_sample;

                 this.measureunit = measureunit;
                 lblmeasureunit.Content = measureunit;

                 this.zero_intercept = zero_intercept;
                 if (zero_intercept == true)
                 {

                     BitmapImage bi3 = new BitmapImage();
                     bi3.BeginInit();
                     bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                     bi3.EndInit();
                     ZeroIntercept.Source = bi3;

                 }
                 else
                 {
                     BitmapImage bi3 = new BitmapImage();
                     bi3.BeginInit();
                     bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                     bi3.EndInit();
                     ZeroIntercept.Source = bi3;

                 }

                 this.calibration_method = calibration_method;
                 lblCabMethodV.Content = calibration_method;

             }
        }
        private QuaMethod QM;
        public QuaMethod QPar { get; set; }
        public void Dispose() {  }
        private void CloseSettings_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Wl_1_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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

                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.txtValue.Text.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[0] = num.ToString("f1").Replace(',', '.');
                                    wl_1.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                           // frm.Close(); frm.Dispose();
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

        private void Wl_2_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using(InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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

                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.txtValue.Text.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[1] = num.ToString("f1").Replace(',', '.');
                                    wl_2.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                          //  frm.Close(); frm.Dispose();
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
        public string[] wl_mass;
        public string step_interval;
        private void Wl_3_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if (LamdbaLbl.Content.ToString() != "Step")
            {
                using (InputDataFrm frm = new InputDataFrm())
                {
                    frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                              //  frm.wl = frm.txtValue.Text.ToString();
                                if (frm.wl != null)
                                {
                                    num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                    if (num <= 1100M && num >= 190M)
                                    {
                                        wl_mass[2] = num.ToString("f1").Replace(',', '.');
                                        wl_3.Content = num.ToString("f1");
                                    }
                                    else
                                    {

                                    }
                                }
                               // frm.Close(); frm.Dispose();
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
            else
            {
                StepInterval stepInterval1 = new StepInterval(step_interval);
                stepInterval1.ShowDialog();
                if (step_interval != stepInterval1.step_interval)
                {
                    string num = stepInterval1.step_interval.ToString();
                    wl_3.Content = num;
                    step_interval = num;
                }
            }
        }

        private void Wl_4_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                           // frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[3] = num.ToString("f1").Replace(',', '.');
                                    wl_4.Content = num.ToString("f1");
                                }
                                else
                                {
                                    frm.wl = frm.txtValue.Text.ToString();
                                }
                            }
                          //  frm.Close(); frm.Dispose();
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

        private void Wl_5_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                          //  frm.wl = frm.txtValue.Text.ToString();

                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[4] = num.ToString("f1").Replace(',', '.');
                                    wl_5.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                        //    frm.Close(); frm.Dispose();
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

        private void Wl_6_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                          // frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[5] = num.ToString("f1").Replace(',', '.');
                                    wl_6.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                           // frm.Close(); frm.Dispose();
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

        private void Wl_7_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                          //  frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[6] = num.ToString("f1").Replace(',', '.');
                                    wl_7.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                           // frm.Close(); frm.Dispose();
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

        private void Wl_8_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                      //      frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[7] = num.ToString("f1").Replace(',', '.');
                                    wl_8.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                           // frm.Close(); frm.Dispose();
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

        private void Wl_9_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                        //    frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[8] = num.ToString("f1").Replace(',', '.');
                                    wl_9.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                      //     frm.Close(); frm.Dispose();
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

        private void Wl_10_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                       //     frm.wl = frm.txtValue.Text.ToString();

                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[9] = num.ToString("f1").Replace(',', '.');
                                    wl_10.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                        //    frm.Close(); frm.Dispose();
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

        private void Wl_11_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                       //     frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[10] = num.ToString("f1").Replace(',', '.');
                                    wl_11.Content = num.ToString("f1");
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

        private void Wl_12_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                        //    frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[11] = num.ToString("f1").Replace(',', '.');
                                    wl_12.Content = num.ToString("f1");
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

        private void Wl_13_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[12] = num.ToString("f1").Replace(',', '.');
                                    wl_13.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                            //frm.Close(); frm.Dispose();
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

        private void Wl_14_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[13] = num.ToString("f1").Replace(',', '.');
                                    wl_14.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                        //    frm.Close(); frm.Dispose();
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

        private void Wl_15_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                       //     frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[14] = num.ToString("f1").Replace(',', '.');
                                    wl_15.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                           // frm.Close(); frm.Dispose();
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

        private void Wl_16_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                        //    frm.wl = frm.txtValue.Text.ToString();

                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[15] = num.ToString("f1").Replace(',', '.');
                                    wl_16.Content = num.ToString("f1");
                                }
                                else
                                {
                                   // frm.wl = frm.txtValue.Text.ToString();
                                }
                            }
                          //  frm.Close(); frm.Dispose();
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

        private void Wl_17_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[16] = num.ToString("f1").Replace(',', '.');
                                    wl_17.Content = num.ToString("f1");
                                }
                                else
                                {
                                //    frm.wl = frm.txtValue.Text.ToString();
                                }
                            }
                          //  frm.Close(); frm.Dispose();
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

        private void Wl_18_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                       //     frm.wl = frm.txtValue.Text.ToString();

                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[17] = num.ToString("f1").Replace(',', '.');
                                    wl_18.Content = num.ToString("f1");
                                }
                                else
                                {
                                 //   frm.wl = frm.txtValue.Text.ToString();

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

        private void Wl_19_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                        //    frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[18] = num.ToString("f1").Replace(',', '.');
                                    wl_19.Content = num.ToString("f1");
                                }
                                else
                                {
                               //     frm.wl = frm.txtValue.Text.ToString();
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

        private void Wl_20_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
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
                           // frm.wl = frm.txtValue.Text.ToString();
                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
                                if (num <= 1100M && num >= 190M)
                                {
                                    wl_mass[19] = num.ToString("f1").Replace(',', '.');
                                    wl_20.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                            //frm.Close(); frm.Dispose();
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
        public string count_measure;
        private void CountMeasure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            CountMeasure wLCountMeasure = new CountMeasure(count_measure);
            wLCountMeasure.ShowDialog();
            if (count_measure != wLCountMeasure.count_measure)
            {
                lblCountMeasure.Content = wLCountMeasure.count_measure.ToString() + " >";
                count_measure = wLCountMeasure.count_measure;
            }
        }
        public string optical_path;
        private void OpticalPath_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            OpticalPath wLOpticalPath = new OpticalPath(optical_path);
            wLOpticalPath.ShowDialog();
            if (optical_path != wLOpticalPath.optical_path)
            {
                lblOpticalPath.Content = wLOpticalPath.optical_path.ToString() + " >";
                optical_path = wLOpticalPath.optical_path;
            }
        }
        public string curveEquation;
        private void Label_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            StandartCurveEquation standartCurveEquation = new StandartCurveEquation(curveEquation);
            standartCurveEquation.ShowDialog();
            if (curveEquation != standartCurveEquation.curveEquation)
            {
                lblStandardEquation.Content = standartCurveEquation.curveEquation.ToString() + " >";
                curveEquation = standartCurveEquation.curveEquation;
            }
        }
        public string fittingMethod;
        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            FittingMethod fittingMethod_ = new FittingMethod(fittingMethod);
            fittingMethod_.ShowDialog();
            if (fittingMethod != fittingMethod_.fittingMethod)
            {
                lblfittingMethod.Content = fittingMethod_.fittingMethod.ToString() + " >";
                fittingMethod = fittingMethod_.fittingMethod;
            }
        }

        private void Next_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

            /*if (calibration_method != "Ввод коэффициентов уравнения")
            {
                QuantitationNewMethodNext quantationMethod = new QuantitationNewMethodNext(wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method);
                quantationMethod.ShowDialog();
                
            }
            else
            {
                QuantitationNewMethodNextCoefficien quantationMethod = new QuantitationNewMethodNextCoefficien(wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method);
                quantationMethod.ShowDialog();
            }*/
            // QuantitationNewMethodNext quantationMethod = new QuantitationNewMethodNext(wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method);
            // QuantitationNewMethodNext quantationMethod = new QuantitationNewMethodNext(QM, wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method);


            /*switch (measure_method)
            {
                case "Одноволновое":
                    QM.QPar.MeasureMethod.WLCnt = 1;
                    QM.QPar.MeasureMethod.Square = 1;
                    QM.QPar.MeasureMethod.C_name = "Одноволновое";
                    QM.QPar.MeasureMethod.C_DM = "Одноволновое";
                    QM.QPar.MeasureMethod.C_mode = "All";
                    QM.QPar.MeasureMethod.C_gs = "A=A1";
                    QM.QPar.MeasureMethod.RCnt = new int?(0);
                    break;
                case "Двухволновое":
                    QM.QPar.MeasureMethod.WLCnt = 2;
                    QM.QPar.MeasureMethod.Square = 0;
                    QM.QPar.MeasureMethod.C_name = "Двухволновое";
                    QM.QPar.MeasureMethod.C_DM = "Двухволновое";
                    QM.QPar.MeasureMethod.C_mode = "All";
                    QM.QPar.MeasureMethod.C_gs = "A=A1-A2";
                    QM.QPar.MeasureMethod.RCnt = new int?(2);
                    break;
                case "Трехволновое":
                    QM.QPar.MeasureMethod.WLCnt = 3;
                    QM.QPar.MeasureMethod.Square = 0;
                    QM.QPar.MeasureMethod.C_name = "Трехволновое";
                    QM.QPar.MeasureMethod.C_DM = "Трехволновое";
                    QM.QPar.MeasureMethod.C_mode = "All";
                    QM.QPar.MeasureMethod.C_gs = "A=A1-A2-(λ1-λ2)*(A2-A3)/(λ2-λ3)";
                    QM.QPar.MeasureMethod.RCnt = new int?(0);
                    break;
                case "Границы":
                    QM.QPar.MeasureMethod.WLCnt = 2;
                    QM.QPar.MeasureMethod.Square = 1;
                    QM.QPar.MeasureMethod.C_name = "Границы";
                    QM.QPar.MeasureMethod.C_DM = "Границы";
                    QM.QPar.MeasureMethod.C_mode = "All";
                    QM.QPar.MeasureMethod.C_gs = "A=∫f(A)dλ";
                    QM.QPar.MeasureMethod.RCnt = new int?(0);
                    break;
            }

            this.QM.QPar.MeasureMethodName = measure_method.Trim().Replace(" >", "");
            this.QM.QPar.MeasureMethod = CommonFun.GetByName(this.QM.QPar.MeasureMethodName);
            this.QM.QPar.MeasureMethodName = this.QM.QPar.MeasureMethod.C_name;
            this.QM.QPar.CabMethodDM = calibration_method.Trim().Replace(" >", "");
            this.QM.QPar.CabMethod = this.QM.QPar.CabMethodDM;
            this.QM.QPar.EConvert = false;
            this.QM.QPar.Cuvettemath = zero_intercept;
            this.QM.QPar.Equation = curveEquation.Trim().Replace(" >", "");
            this.QM.QPar.FittingDM = fittingMethod.Trim().Replace(" >", "");
            this.QM.QPar.Fitting = fittingMethod.Trim().Replace(" >", "");
            this.QM.QPar.Length = optical_path.Trim().Replace(" >", "");
            this.QM.QPar.Limits = "";
            this.QM.QPar.MCnt = Convert.ToInt16(count_standard_sample.Trim().Replace(" >", ""));
            this.QM.QPar.SamCnt = wl_mass.Count();
            this.QM.QPar.Unit = measureunit.Trim().Replace(">", "");
            this.QM.QPar.WL = string.Join(",", wl_mass);
            this.QM.QPar.ZeroB = zero_intercept;

            this.Hide();
            QuantitationNewMethodNext quantationMethod = new QuantitationNewMethodNext(QM);
            quantationMethod.ShowDialog();            
            if(quantationMethod.PreviewMouseDown_last == true)
            {
                this.Show();
            }
            else
            {
                if(quantationMethod.cancel_ == true)
                {
                    cancel_ = true;
                }
                else
                {

                }
                this.Close();
            }*/
        }

        public bool cancel_;

        private void Open_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Save_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void New_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            //Save.IsEnabled = true;
            Next.IsEnabled = true;
            LeftSettings.IsEnabled = true;
            RightSettings.IsEnabled = true;
        }
        public string count_standard_sample;
        private void LblCountSample_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            
            StandardSampleCount count_standard_sample_ = new StandardSampleCount(count_standard_sample);
            count_standard_sample_.ShowDialog();
            if (count_standard_sample != count_standard_sample_.count_standard_sample.ToString())
            {
                lblCountSample.Content = count_standard_sample_.count_standard_sample.ToString() + " >";
                count_standard_sample = count_standard_sample_.count_standard_sample.ToString();
            }
        }
        public string measureunit;
        private void Lblmeasureunit_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            MeasureUnit measureunit_ = new MeasureUnit(measureunit);
            measureunit_.ShowDialog();
            if (measureunit != measureunit_.measureunit)
            {
                lblmeasureunit.Content = measureunit_.measureunit.ToString() + " >";
                measureunit = measureunit_.measureunit;
            }
        }

        public bool zero_intercept = false;
        private void ZeroIntercept_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if (zero_intercept == false)
            {
                
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                bi3.EndInit();
                ZeroIntercept.Source = bi3;
                zero_intercept = true;
            }
            else
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                bi3.EndInit();
                ZeroIntercept.Source = bi3;
                zero_intercept = false;
            }
        }
        public string measure_method;
        private void LblMeasurment_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            MeasureMethod measure_method_ = new MeasureMethod(measure_method);
            measure_method_.ShowDialog();
            if (measure_method != measure_method_.measure_method)
            {
                lblMeasurment.Content = measure_method_.measure_method.ToString() + " >";
                measure_method = measure_method_.measure_method;
                OpenWl();
            }
        }
        public void setwl(int wlcnt, string wl, bool square)
        {
            switch (wlcnt)
            {
                case 1:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = false;
                    Lambda3.IsEnabled = false;
                    LamdbaLbl.Content = '\u03BB' + "3";
                    if (wl.Length > 0)
                        this.wl_1.Content = wl;
                    break;
                case 2:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = false;
                    LamdbaLbl.Content = '\u03BB' + "3";
                    if (wl.Length <= 0)
                        break;
                    string[] strArray1 = wl.Split(',');
                    this.wl_1.Content = strArray1[0];
                    this.wl_2.Content = strArray1[1];
                    break;
                case 3:
                    if (wl.Length > 0)
                    {
                        string[] strArray2 = wl.Split(',');
                        this.wl_1.Content = strArray2[0];
                        this.wl_2.Content = strArray2[1];
                        this.wl_3.Content = strArray2[2];
                    }
                    if (square)
                    {
                        this.LamdbaLbl.Content = "Step";
                        break;
                    }
                    this.LamdbaLbl.Content = '\u03BB' + "3";
                    break;
            }
        }

        private void OpenWl()
        {
            switch (measure_method)
            {
                case "Одноволновое":
                    wl_mass = new string[1];
                    wl_mass[0] = wl_1.Content.ToString();
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = false;
                    Lambda3.IsEnabled = false;
                    LamdbaLbl.Content = '\u03BB' + "3";
                    wl_3.Content = "----";
                    break;
                case "Двухволновое":
                    wl_mass = new string[2];
                    wl_mass[0] = wl_1.Content.ToString();
                    wl_mass[1] = wl_2.Content.ToString();
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = false;
                    LamdbaLbl.Content = '\u03BB' + "3";
                    wl_3.Content = "----";
                    break;
                case "Трехволновое":
                    wl_mass = new string[3];
                    wl_mass[0] = wl_1.Content.ToString();
                    wl_mass[1] = wl_2.Content.ToString();
                    wl_mass[2] = wl_3.Content.ToString();
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    LamdbaLbl.Content = '\u03BB' + "3";
                    wl_3.Content = "----";
                    break;
                case "Границы":
                    wl_mass = new string[2];
                    wl_mass[0] = wl_1.Content.ToString();
                    wl_mass[1] = wl_2.Content.ToString();
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    LamdbaLbl.Content = "Step";
                    wl_3.Content = "----";
                    break;
            }
        }
        public string calibration_method;
        private void LblCabMethodV_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            CalibrationMethod calibration_method_ = new CalibrationMethod(calibration_method);
            calibration_method_.ShowDialog();
            if (calibration_method != calibration_method_.calibration_method)
            {
                lblCabMethodV.Content = calibration_method_.calibration_method.ToString() + " >";
                calibration_method = calibration_method_.calibration_method;
                if (calibration_method == "Ввод коэффициентов уравнения")
                {
                    Equation.IsEnabled = false;
                    lblCountSample.IsEnabled = false;
                }
                else
                {
                    Equation.IsEnabled = true;
                    lblCountSample.IsEnabled = true;
                }
               // OpenWl();
            }
            
        }
        private void btnBack_PreviewMouseDown(object sender, EventArgs e) => this.Close();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Wl_1_PreviewMouseDown(object sender, EventArgs e)
        {

        }
    }
}
