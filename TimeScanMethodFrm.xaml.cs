using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для TimeScanMethodFrm.xaml
    /// </summary>
    public partial class TimeScanMethodFrm : Window, IDisposable
    {
        public TimeScanMethodFrm()
        {
            InitializeComponent();
            photometric_mode = lblModeV.Content.ToString().Remove(lblModeV.Content.ToString().Length - 2);
            this.pibEConvert.Tag = "off";
            this.pibAutoXY.Tag = "off";
           
        }
        string photometric_mode;

        private void LblModeV_PreviewMouseDown(object sender, EventArgs e)
        {
            PhotometricMode photometricMode = new PhotometricMode();
            photometricMode.ShowDialog();
            if (photometricMode.photometry_Mode != null)
            {
                lblModeV.Content = photometricMode.photometry_Mode.ToString() + " >";
                photometric_mode = photometricMode.photometry_Mode.ToString();
            }
            else
            {
                photometric_mode = lblModeV.Content.ToString().Remove(lblModeV.Content.ToString().Length - 2);
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
                        if (this.lblTimeV.Content.ToString().Replace(" >", "") != "")
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
        private void LblLengthV_PreviewMouseDown(object sender, EventArgs e)
        {
            OpticalPath wLOpticalPath = new OpticalPath(lblLengthV.Content.ToString().Replace(" >", ""));
            wLOpticalPath.ShowDialog();
            if (lblLengthV.Content.ToString().Replace(" >", "") != wLOpticalPath.optical_path)
            {
                lblLengthV.Content = wLOpticalPath.optical_path.ToString() + " >";

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
        private void CloseSettings_PreviewMouseDown(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.pibEConvert.Tag = (object)"off";


        }

        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".mscntm";
        private void Save_PreviewMouseDown(object sender, EventArgs e)
        {
            using (SaveFrm save = new SaveFrm(extension, "Настройки временного сканирования"))
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
                                            if (k.FirstChild.Value != "False")
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
                                            else
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

        private void New_method_PreviewMouseDown(object sender, EventArgs e)
        {

        }
        private void Finish_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        public void Dispose()
        {
            this.Close();
        }
    }

}
