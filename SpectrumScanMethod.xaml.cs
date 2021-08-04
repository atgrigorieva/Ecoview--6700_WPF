using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;
using Path = System.IO.Path;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для SpectrumScanMethod.xaml
    /// </summary>
    public partial class SpectrumScanMethod : Window, IDisposable
    {
        public string photometric_mode, speed_measure;
        public string loop_measure;
        public string step_interval, time_interval;
        public string start_wl, cancel_wl;
        public string optical_path;
        public SpectrumScanMethod()
        {
            InitializeComponent();
            this.photometric_mode = "Абсорбция (Abs)";
            PhotometryMode.Content = this.photometric_mode + " >";
            this.loop_measure = "1";
            LoopMeasure.Content = this.loop_measure + " >";
            this.step_interval = "0.1";
            StepMeasure.Content = this.step_interval + " >";
            this.speed_measure = "Быстро";
            SpeedMeasure.Content = this.speed_measure + " >";            
            this.start_wl = "1100.0";
            StartWl.Content = this.start_wl + " >";
            this.cancel_wl = "190.0";
            CancelWL.Content = this.cancel_wl + " >";
            this.optical_path = "10";
            OpticalPath.Content = this.optical_path + " >";
            //this.time_interval = "1";
        }
        public void Dispose()
        {
            
        }
        private void PhotometryMode_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            PhotometricMode photometricMode = new PhotometricMode();
            photometricMode.ShowDialog();
            if (photometricMode.photometry_Mode != null)
            {
                PhotometryMode.Content = photometricMode.photometry_Mode.ToString() + " >";
                photometric_mode = photometricMode.photometry_Mode.ToString();
            }
            else
            {
                photometric_mode = PhotometryMode.Content.ToString().Remove(PhotometryMode.Content.ToString().Length - 2);
            }
        }

        private void New_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Save.IsEnabled = true;
            Finish.IsEnabled = true;
            LeftSettings.IsEnabled = true;
            RightSettings.IsEnabled = true;
            New_method.Background = null;
        }

        private void Open_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Open_File();
            Open_method.Background = null;
        }
        bool shifrTrueFalse = false;
        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".mss";
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
                                        if ("photometric_mode".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            photometric_mode = k.FirstChild.Value;
                                            PhotometryMode.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("speed_measure".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            speed_measure = k.FirstChild.Value;
                                            SpeedMeasure.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("loop_measure".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            loop_measure = k.FirstChild.Value;
                                            LoopMeasure.Content = k.FirstChild.Value + " >";
                                        }

                                        if ("step_interval".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            step_interval = k.FirstChild.Value;
                                            StepMeasure.Content = k.FirstChild.Value + " >";
                                        }

                                        /*if ("time_interval".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            time_interval = k.FirstChild.Value;
                                            LoopIntervalMeasure.Content = k.FirstChild.Value + " >";
                                        }*/

                                        if ("start_wl".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            start_wl = k.FirstChild.Value;
                                            StartWl.Content = k.FirstChild.Value + " >";
                                        }

                                        if ("cancel_wl".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            cancel_wl = k.FirstChild.Value;
                                            CancelWL.Content = k.FirstChild.Value + " >";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Save.IsEnabled = true;
            Finish.IsEnabled = true;
            LeftSettings.IsEnabled = true;
            //RightSettings.IsEnabled = true;
            //EnabelWL();
        }

        private void Save_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (SaveFrm save = new SaveFrm(extension, "Настройки сканирующего режима"))
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
            Save.Background = null;


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
            bool error = ValueWL();
            if (error != false)
            {
                XmlDocument xd = new XmlDocument();
                FileStream fs = new FileStream(filepath, FileMode.Open);
                xd.Load(fs);

                XmlNode Settings = xd.CreateElement("Settings");

                XmlNode DateTime1 = xd.CreateElement("DateTime"); // дата создания градуировки
                DateTime1.InnerText = DateTime.Now.ToString(); // и значение
                Settings.AppendChild(DateTime1); // и указываем кому принадлежит

                XmlNode photometric_mode_xml = xd.CreateElement("photometric_mode");
                photometric_mode_xml.InnerText = Convert.ToString(photometric_mode);
                Settings.AppendChild(photometric_mode_xml);
                // xd.DocumentElement.AppendChild(Settings);

                XmlNode speed_measure_xml = xd.CreateElement("speed_measure");
                speed_measure_xml.InnerText = Convert.ToString(speed_measure);
                Settings.AppendChild(speed_measure_xml);

                XmlNode loop_measure_xml = xd.CreateElement("loop_measure");
                loop_measure_xml.InnerText = Convert.ToString(loop_measure);
                Settings.AppendChild(loop_measure_xml);

                XmlNode step_interval_xml = xd.CreateElement("step_interval");
                step_interval_xml.InnerText = Convert.ToString(step_interval);
                Settings.AppendChild(step_interval_xml);

                /*XmlNode time_interval_xml = xd.CreateElement("time_interval");
                time_interval_xml.InnerText = Convert.ToString(time_interval);
                Settings.AppendChild(time_interval_xml);*/

                XmlNode start_wl_xml = xd.CreateElement("start_wl");
                start_wl_xml.InnerText = Convert.ToString(start_wl);
                Settings.AppendChild(start_wl_xml);

                XmlNode cancel_wl_xml = xd.CreateElement("cancel_wl");
                cancel_wl_xml.InnerText = Convert.ToString(cancel_wl);
                Settings.AppendChild(cancel_wl_xml);

                xd.DocumentElement.AppendChild(Settings);

                fs.Close();         // Закрываем поток  
                xd.Save(filepath); // Сохраняем файл  
        


            }
        }
        private bool ValueWL()
        {
            return true;
        }
        private void Finish_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            bool error = ValueWL();
            if (error == true)
            {
                this.Close();
            }
        }

       
        private void StartWl_PreviewMouseDown(object sender, RoutedEventArgs e)
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
                                num = Convert.ToDecimal(frm.wl);
                                if (num <= 1100M && num >= 190M)
                                {
                                    start_wl = num.ToString("f1");
                                    StartWl.Content = num.ToString("f1") + " >";
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
                    frm.Close(); 

                });
                object num1 = frm.ShowDialog();
            }
        }

        private void CancelWL_PreviewMouseDown(object sender, RoutedEventArgs e)
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
                /*frm.btnOK.PreviewMouseDown += ((param0, param1) =>
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
                                num = Convert.ToDecimal(frm.wl);
                                if (num <= 1100M && num >= 190M)
                                {
                                    cancel_wl = num.ToString("f1");
                                    CancelWL.Content = num.ToString("f1") + " >";
                                }
                                else
                                {

                                }
                            }
                            frm.Close(); frm.Dispose();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });*/
                frm.btnCancel.PreviewMouseDown += ((param0, param1) =>
                {
                    //if (!this.automode)
                    //    return;
                    frm.wl = null;
                    frm.Close(); 

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
                                num = Convert.ToDecimal(frm.wl);
                                if (num <= 1100M && num >= 190M)
                                {
                                    cancel_wl = num.ToString("f1");
                                    CancelWL.Content = num.ToString("f1") + " >";
                                }
                                else
                                {

                                }
                            }
                  //          frm.Close(); frm.Dispose();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
                object num1 = frm.ShowDialog();
            }
        }

        private void StepMeasure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            StepInterval stepInterval1 = new StepInterval(step_interval);
            stepInterval1.ShowDialog();
            if (step_interval != stepInterval1.step_interval)
            {
                string num = stepInterval1.step_interval.ToString();
                StepMeasure.Content = num + " >";
                step_interval = num;
            }
        }

        private void SpeedMeasure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            SpeedMeasure speedMeasure = new SpeedMeasure(speed_measure);
            speedMeasure.ShowDialog();
            if (speed_measure != speedMeasure.speed_measure)
            {
                SpeedMeasure.Content = speedMeasure.speed_measure.ToString() + " >";
                speed_measure = speedMeasure.speed_measure;
            }
        }

        private void LoopMeasure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           /* InputNumberFrm frm = new InputNumberFrm();
            frm.ShowDialog();
            if (frm.loop_measure != null)
            {
                Decimal num = Convert.ToDecimal(frm.loop_measure.ToString(), new CultureInfo("en-US"));
                if (num < 100M && num > 0M)
                {
                    loop_measure = num.ToString();
                    LoopMeasure.Content = num.ToString() + " >";
                }
                else
                {

                }
            }*/
        }

        private void LoopIntervalMeasure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            /*InputTimerIntervalFrm frm = new InputTimerIntervalFrm();
            frm.ShowDialog();
            if (frm.loop_measure != null)
            {
                Decimal num = Convert.ToDecimal(frm.loop_measure.ToString(), new CultureInfo("en-US"));
                if (num <= 3600M && num > 0M)
                {
                    time_interval = num.ToString();
                    
                    LoopIntervalMeasure.Content = num.ToString() + " >";
                }
                else
                {

                }
            }*/
            using (InputTimerIntervalFrm frm = new InputTimerIntervalFrm())
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
                        this.LoopIntervalMeasure.Content = frm.txtValue.Text + " >";
                        time_interval = frm.txtValue.Text;
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
                        this.LoopIntervalMeasure.Content = frm.txtValue.Text + " >";
                        time_interval = frm.txtValue.Text;
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

        public bool close = false;
        private void CloseSettings_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            bool error = ValueWL();
            if (error == true)
            {
                close = true;
                this.Close();
            }
        }

        private void OpticalPath_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            OpticalPath wLOpticalPath = new OpticalPath(optical_path);
            wLOpticalPath.ShowDialog();
            if (optical_path != wLOpticalPath.optical_path)
            {
                OpticalPath.Content = wLOpticalPath.optical_path.ToString() + " >";
                optical_path = wLOpticalPath.optical_path;
            }
        }

        private void SwitchOnOFfAutoAdjust_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void YMax_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void YMin_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }
    }
}
