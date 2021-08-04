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
    /// Логика взаимодействия для SettingsMulWlFrm.xaml
    /// </summary>
    public partial class SettingsMulWlFrm : Window, IDisposable
    {
        public string[] wl_mass;
        public string count_wl;
        public string count_measure;
        public string optical_path;
        public string photometric_mode;

        /*public SettingsMulWlFrm(string[] wl_mass, string photometric_mode, int count_wl = 3, int count_measure = 3, int optical_path = 10)
        {
            InitializeComponent();

            this.count_wl = count_wl;
            this.wl_mass = wl_mass;
            if(wl_mass != null)
            {
                WlData();
                Save.IsEnabled = true;
                Finish.IsEnabled = true;
                LeftSettings.IsEnabled = true;
                RightSettings.IsEnabled = true;
            }
            else
            {
                this.wl_mass = new string[count_wl];
                this.wl_mass[0] = wl_1.Content.ToString();
                this.wl_mass[1] = wl_2.Content.ToString();
                this.wl_mass[2] = wl_3.Content.ToString();
            }
            //wl_mass[0] = wl_1.Content.ToString();
            // wl_mass[1] = wl_2.Content.ToString();
            //   wl_mass[2] = wl_3.Content.ToString();

            this.count_measure = count_measure;
            this.optical_path = optical_path;
            if (photometric_mode != null)
            {
                this.photometric_mode = photometric_mode;
                PhotometryMode.Content = this.photometric_mode + " >";
            }
            else
            {
                this.photometric_mode = "Абсорбция (Abs)";
                PhotometryMode.Content = "Абсорбция (Abs) > ";
            }               

            
            CountWL.Content = this.count_wl + " >";
            CountMeasure.Content = this.count_measure + " >";
            OpticalPath.Content = this.optical_path + " >";

            EnabelWL();

        }*/
        public SettingsMulWlFrm()
        {
            InitializeComponent();

            this.count_wl = "3";
            //this.wl_mass = null;
            this.wl_mass = new string[Convert.ToInt32(count_wl)];
            this.wl_mass[0] = wl_1.Content.ToString();
            this.wl_mass[1] = wl_2.Content.ToString();
            this.wl_mass[2] = wl_3.Content.ToString();

            //wl_mass[0] = wl_1.Content.ToString();
            // wl_mass[1] = wl_2.Content.ToString();
            //   wl_mass[2] = wl_3.Content.ToString();

            this.count_measure = "3";
            this.optical_path = "10";
            this.photometric_mode = "Абсорбция (Abs)";
            PhotometryMode.Content = "Абсорбция (Abs) > ";


            CountWL.Content = this.count_wl + " >";
            CountMeasure.Content = this.count_measure + " >";
            OpticalPath.Content = this.optical_path + " >";

            EnabelWL();

        }
        public bool ValueWL()
        {
            bool error = true;
            for (int i = 1; i <= Convert.ToInt32(count_wl); i++)
            {
                switch (i)
                {
                    case 1:
                        if (wl_1.Content.ToString() != "----")
                            this.wl_mass[0] = wl_1.Content.ToString();
                        else
                            error = false;
                        break;
                    case 2:
                        if (wl_2.Content.ToString() != "----")
                            this.wl_mass[1] = wl_2.Content.ToString();
                        else
                            error = false;
                        break;
                    case 3:
                        if (wl_3.Content.ToString() != "----")
                            this.wl_mass[2] = wl_3.Content.ToString();
                        else
                            error = false;
                        break;
                    case 4:
                        if (wl_4.Content.ToString() != "----")
                            this.wl_mass[3] = wl_4.Content.ToString();
                        else
                            error = false;
                        break;
                    case 5:
                        if (wl_5.Content.ToString() != "----")
                            this.wl_mass[4] = wl_5.Content.ToString();
                        else
                            error = false;
                        break;
                    case 6:
                        if (wl_6.Content.ToString() != "----")
                            this.wl_mass[5] = wl_6.Content.ToString();
                        else
                            error = false;
                        break;
                    case 7:
                        if (wl_7.Content.ToString() != "----")
                            this.wl_mass[6] = wl_7.Content.ToString();
                        else
                            error = false;
                        break;
                    case 8:
                        if (wl_8.Content.ToString() != "----")
                            this.wl_mass[7] = wl_8.Content.ToString();
                        else
                            error = false;
                        break;
                    case 9:
                        if (wl_9.Content.ToString() != "----")
                            this.wl_mass[8] = wl_9.Content.ToString();
                        else
                            error = false;
                        break;
                    case 10:
                        if (wl_10.Content.ToString() != "----")
                            this.wl_mass[9] = wl_10.Content.ToString();
                        else
                            error = false;
                        break;
                    case 11:
                        if (wl_4.Content.ToString() != "----")
                            this.wl_mass[11] = wl_11.Content.ToString();
                        else
                            error = false;
                        break;
                    case 12:
                        if (wl_12.Content.ToString() != "----")
                            this.wl_mass[11] = wl_12.Content.ToString();
                        else
                            error = false;
                        break;
                    case 13:
                        if (wl_13.Content.ToString() != "----")
                            this.wl_mass[12] = wl_13.Content.ToString();
                        else
                            error = false;
                        break;
                    case 14:
                        if (wl_14.Content.ToString() != "----")
                            this.wl_mass[13] = wl_14.Content.ToString();
                        else
                            error = false;
                        break;
                    case 15:
                        if (wl_15.Content.ToString() != "----")
                            this.wl_mass[14] = wl_15.Content.ToString();
                        else
                            error = false;
                        break;
                    case 16:
                        if (wl_16.Content.ToString() != "----")
                            this.wl_mass[15] = wl_16.Content.ToString();
                        else
                            error = false;
                        break;
                    case 17:
                        if (wl_17.Content.ToString() != "----")
                            this.wl_mass[16] = wl_17.Content.ToString();
                        else
                            error = false;
                        break;
                    case 18:
                        if (wl_18.Content.ToString() != "----")
                            this.wl_mass[17] = wl_18.Content.ToString();
                        else
                            error = false;
                        break;
                    case 19:
                        if (wl_19.Content.ToString() != "----")
                            this.wl_mass[18] = wl_19.Content.ToString();
                        else
                            error = false;
                        break;
                    case 20:
                        if (wl_20.Content.ToString() != "----")
                            this.wl_mass[19] = wl_20.Content.ToString();
                        else
                            error = false;
                        break;
                }
                if (error == false)
                    return false;
            }
            return true;
        }

        public void Dispose()
        {
            this.Close();
        }

        private void WlData()
        {
            for (int i = 1; i <= Convert.ToInt32(count_wl); i++) {
                switch (i)
                {
                    case 1:
                        wl_1.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 2:
                        wl_2.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 3:
                        wl_3.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 4:
                        wl_4.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 5:
                        wl_5.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 6:
                        wl_6.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 7:
                        wl_7.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 8:
                        wl_8.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 9:
                        wl_9.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 10:
                        wl_10.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 11:
                        wl_11.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 12:
                        wl_12.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 13:
                        wl_13.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 14:
                        wl_14.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 15:
                        wl_15.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 16:
                        wl_16.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 17:
                        wl_17.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 18:
                        wl_18.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 19:
                        wl_19.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                    case 20:
                        wl_20.Content = (Convert.ToDecimal(wl_mass[i - 1]) / 10M).ToString("f1");
                        break;
                }
            }
        }

        private void EnabelWL()
        {
            Lambda1.IsEnabled = false;
            Lambda2.IsEnabled = false;
            Lambda3.IsEnabled = false;
            Lambda4.IsEnabled = false;
            Lambda5.IsEnabled = false;
            Lambda6.IsEnabled = false;
            Lambda7.IsEnabled = false;
            Lambda8.IsEnabled = false;
            Lambda9.IsEnabled = false;
            Lambda10.IsEnabled = false;
            Lambda11.IsEnabled = false;
            Lambda12.IsEnabled = false;
            Lambda13.IsEnabled = false;
            Lambda14.IsEnabled = false;
            Lambda15.IsEnabled = false;
            Lambda16.IsEnabled = false;
            Lambda16.IsEnabled = false;
            Lambda17.IsEnabled = false;
            Lambda18.IsEnabled = false;
            Lambda19.IsEnabled = false;
            Lambda20.IsEnabled = false;

            switch (Convert.ToInt32(count_wl))
            {
                case 1:
                    Lambda1.IsEnabled = true;
                    break;
                case 2:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    break;
                case 3:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    break;
                case 4:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    break;
                case 5:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    break;
                case 6:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    break;
                case 7:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    break;
                case 8:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    break;
                case 9:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    break;
                case 10:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    break;
                case 11:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    break;
                case 12:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    break;
                case 13:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    break;
                case 14:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    Lambda14.IsEnabled = true;
                    break;
                case 15:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    Lambda14.IsEnabled = true;
                    Lambda15.IsEnabled = true;
                    break;
                case 16:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    Lambda14.IsEnabled = true;
                    Lambda15.IsEnabled = true;
                    Lambda16.IsEnabled = true;
                    break;
                case 17:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    Lambda14.IsEnabled = true;
                    Lambda15.IsEnabled = true;
                    Lambda16.IsEnabled = true;
                    Lambda17.IsEnabled = true;
                    break;
                case 18:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    Lambda14.IsEnabled = true;
                    Lambda15.IsEnabled = true;
                    Lambda16.IsEnabled = true;
                    Lambda17.IsEnabled = true;
                    Lambda18.IsEnabled = true;
                    break;
                case 19:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    Lambda14.IsEnabled = true;
                    Lambda15.IsEnabled = true;
                    Lambda16.IsEnabled = true;
                    Lambda16.IsEnabled = true;
                    Lambda17.IsEnabled = true;
                    Lambda18.IsEnabled = true;
                    Lambda19.IsEnabled = true;
                    break;
                case 20:
                    Lambda1.IsEnabled = true;
                    Lambda2.IsEnabled = true;
                    Lambda3.IsEnabled = true;
                    Lambda4.IsEnabled = true;
                    Lambda5.IsEnabled = true;
                    Lambda6.IsEnabled = true;
                    Lambda7.IsEnabled = true;
                    Lambda8.IsEnabled = true;
                    Lambda9.IsEnabled = true;
                    Lambda10.IsEnabled = true;
                    Lambda11.IsEnabled = true;
                    Lambda12.IsEnabled = true;
                    Lambda13.IsEnabled = true;
                    Lambda14.IsEnabled = true;
                    Lambda15.IsEnabled = true;
                    Lambda16.IsEnabled = true;
                    Lambda16.IsEnabled = true;
                    Lambda17.IsEnabled = true;
                    Lambda18.IsEnabled = true;
                    Lambda19.IsEnabled = true;
                    Lambda20.IsEnabled = true;
                    break;


            }
        }

        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".smw";

        private void Save_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            //Save.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(221, 221, 221, 100));
            using (SaveFrm save = new SaveFrm(extension, "Настройки мультиволнового режима"))
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

                XmlNode count_wl_xml = xd.CreateElement("count_wl");
                count_wl_xml.InnerText = Convert.ToString(count_wl);
                Settings.AppendChild(count_wl_xml);
                //  xd.DocumentElement.AppendChild(Settings);

                XmlNode count_measure_xml = xd.CreateElement("count_measure");
                count_measure_xml.InnerText = Convert.ToString(count_measure);
                Settings.AppendChild(count_measure_xml);
                // xd.DocumentElement.AppendChild(count_measure_xml);

                XmlNode optical_path_xml = xd.CreateElement("optical_path");
                optical_path_xml.InnerText = Convert.ToString(optical_path);
                Settings.AppendChild(optical_path_xml);
                xd.DocumentElement.AppendChild(Settings);

                XmlNode SettingsWL = xd.CreateElement("SettingsWL");


                for (int i = 0; i <= wl_mass.Count() - 1; i++)
                {
                    XmlNode lambda = xd.CreateElement("lambda");

                    XmlAttribute attribute1 = xd.CreateAttribute("Nomer");
                    attribute1.Value = i.ToString(); // устанавливаем значение атрибута
                    lambda.Attributes.Append(attribute1); // добавляем атрибут

                    XmlAttribute wl_xml = xd.CreateAttribute("wl");
                    wl_xml.Value = Convert.ToString(wl_mass[i]);
                    lambda.Attributes.Append(wl_xml); // и указываем кому принадлежит

                    SettingsWL.AppendChild(lambda);
                }

                xd.DocumentElement.AppendChild(SettingsWL);

                fs.Close();         // Закрываем поток  
                xd.Save(filepath); // Сохраняем файл  
            }
        }

        private void New_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Save.IsEnabled = true;
            Finish.IsEnabled = true;
            LeftSettings.IsEnabled = true;
            RightSettings.IsEnabled = true;
        //    New_method.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(221, 221, 221, 100));

        }

        private void Open_method_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Open_File();
       //     Open_method.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(221, 221, 221, 100));
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
                                        if ("count_wl".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            count_wl = k.FirstChild.Value;
                                            CountWL.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("count_measure".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            count_measure = k.FirstChild.Value;
                                            CountMeasure.Content = k.FirstChild.Value + " >";
                                        }
                                        if ("optical_path".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            optical_path = k.FirstChild.Value;
                                            OpticalPath.Content = k.FirstChild.Value + " >";
                                        }
                                    }
                                }
                                wl_mass = new string[Convert.ToInt32(count_wl)];
                                if ("SettingsWL".Equals(d.Name))
                                {
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("lambda".Equals(k.Name))
                                        {
                                            if (k.Attributes.Count > 0)
                                            {
                                                XmlNode attr = k.Attributes.GetNamedItem("Nomer");
                                                if (attr != null)
                                                {
                                                    XmlNode attr1 = k.Attributes.GetNamedItem("wl");
                                                    switch (Convert.ToInt32(attr.Value))
                                                    {
                                                        case 0:
                                                            wl_1.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 1:
                                                            wl_2.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 2:
                                                            wl_3.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 3:
                                                            wl_4.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 4:
                                                            wl_5.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 5:
                                                            wl_6.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 6:
                                                            wl_7.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 7:
                                                            wl_8.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 8:
                                                            wl_9.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 9:
                                                            wl_10.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 10:
                                                            wl_11.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 11:
                                                            wl_12.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 12:
                                                            wl_13.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 13:
                                                            wl_14.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 14:
                                                            wl_15.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 15:
                                                            wl_16.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 16:
                                                            wl_17.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 17:
                                                            wl_18.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 18:
                                                            wl_19.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
                                                        case 19:
                                                            wl_20.Content = attr1.Value;
                                                            wl_mass[Convert.ToInt32(attr.Value)] = attr1.Value;
                                                            break;
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
            Save.IsEnabled = true;
            Finish.IsEnabled = true;
            LeftSettings.IsEnabled = true;
            RightSettings.IsEnabled = true;
            EnabelWL();
        }
    

        private void Finish_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            /*bool error = ValueWL();
            if (error == true)
                this.Close();*/
           // Finish.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(221, 221, 221, 100));
        }

     
    
        private void CountWL_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            WLCount wLCount = new WLCount(count_wl);
            wLCount.ShowDialog();
            if (count_wl != wLCount.count_wl)
            {
                CountWL.Content = wLCount.count_wl.ToString() + " >";
                wl_mass = new string[Convert.ToInt32(wLCount.count_wl)];
                count_wl = wLCount.count_wl;
            }
            EnabelWL();

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



        private void Label_PreviewMouseDown(object sender, RoutedEventArgs e)
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
                                    wl_mass[0] = num.ToString("f1").Replace(',', '.');
                                    wl_1.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
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
                            //frm.wl = frm.txtValue.Text.ToString();

                            if (frm.wl != null)
                            {
                                num = Convert.ToDecimal(frm.wl.ToString(), new CultureInfo("en-US"));
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

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
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

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
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
                            //frm.wl = frm.txtValue.Text.ToString();
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

        private void Label_PreviewMouseDown_4(object sender, RoutedEventArgs e)
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

        private void Label_PreviewMouseDown_5(object sender, RoutedEventArgs e)
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

        private void Label_PreviewMouseDown_6(object sender, RoutedEventArgs e)
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
                                    wl_mass[6] = num.ToString("f1").Replace(',', '.');
                                    wl_7.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_7(object sender, RoutedEventArgs e)
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
                                    wl_mass[7] = num.ToString("f1").Replace(',', '.');
                                    wl_8.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_8(object sender, RoutedEventArgs e)
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
                                    wl_mass[8] = num.ToString("f1").Replace(',', '.');
                                    wl_9.Content = num.ToString("f1");
                                }
                                else
                                {

                                }
                            }
                      //      frm.Close(); frm.Dispose();
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

        private void Label_PreviewMouseDown_9(object sender, RoutedEventArgs e)
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
                                    wl_mass[9] = num.ToString("f1").Replace(',', '.');
                                    wl_10.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_10(object sender, RoutedEventArgs e)
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
                                    wl_mass[10] = num.ToString("f1").Replace(',', '.');
                                    wl_11.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_11(object sender, RoutedEventArgs e)
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
                                    wl_mass[11] = num.ToString("f1").Replace(',', '.');
                                    wl_12.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_12(object sender, RoutedEventArgs e)
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
                            //frm.wl = frm.txtValue.Text.ToString();
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

        private void Label_PreviewMouseDown_13(object sender, RoutedEventArgs e)
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
                                    wl_mass[13] = num.ToString("f1").Replace(',', '.');
                                    wl_14.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_14(object sender, RoutedEventArgs e)
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

        private void Label_PreviewMouseDown_15(object sender, RoutedEventArgs e)
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
                                    wl_mass[15] = num.ToString("f1").Replace(',', '.');
                                    wl_16.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_16(object sender, RoutedEventArgs e)
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
                                    wl_mass[16] = num.ToString("f1").Replace(',', '.');
                                    wl_17.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_17(object sender, RoutedEventArgs e)
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
                                    wl_mass[17] = num.ToString("f1").Replace(',', '.');
                                    wl_18.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_18(object sender, RoutedEventArgs e)
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
                                    wl_mass[18] = num.ToString("f1").Replace(',', '.');
                                    wl_19.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_19(object sender, RoutedEventArgs e)
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
                                    wl_mass[19] = num.ToString("f1").Replace(',', '.');
                                    wl_20.Content = num.ToString("f1");
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

        private void Label_PreviewMouseDown_20(object sender, RoutedEventArgs e)
        {
            CountMeasure wLCountMeasure = new CountMeasure(count_measure);
            wLCountMeasure.ShowDialog();
            if (count_measure != wLCountMeasure.count_measure)
            {
                CountMeasure.Content = wLCountMeasure.count_measure.ToString() + " >";
                count_measure = wLCountMeasure.count_measure;
            }
        }


        private void Label_PreviewMouseDown_21(object sender, RoutedEventArgs e)
        {
            OpticalPath wLOpticalPath = new OpticalPath(optical_path);
            wLOpticalPath.ShowDialog();
            if (optical_path != wLOpticalPath.optical_path)
            {
                OpticalPath.Content = wLOpticalPath.optical_path.ToString() + " >";
                optical_path = wLOpticalPath.optical_path;
            }
        }

        public bool close = false;
        public void CloseSettings_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           /* bool error = ValueWL();
            if (error == true)
            {
                close = true;*/
               // this.Close();
            //}
        }
    }
}
