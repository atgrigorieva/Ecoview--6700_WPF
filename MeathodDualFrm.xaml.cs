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
    /// Логика взаимодействия для MeathodDualFrm.xaml
    /// </summary>
    public partial class MeathodDualFrm : Window, IDisposable
    {
        public MeathodDualFrm(DualComMethod qpar)
        {
            InitializeComponent();
            this.QPar = qpar;
        }

        public DualComMethod QPar { get; set; }

        private void BtnNew_PreviewMouseDown(object sender, EventArgs e)
        {
            this.left_settings.IsEnabled = true;
            this.right_settings.IsEnabled = true;
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
                                if ("Settings".Equals(d.Name))
                                {
                                    //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("WL1".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.WL1 = k.FirstChild.Value;
                                        }
                                        if ("WL2".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.WL2 = k.FirstChild.Value;
                                        }
                                        if ("MCnt".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.MCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("EConvert".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.EConvert = Convert.ToBoolean(k.FirstChild.Value);
                                        }
                                        if ("CabMethod".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.CabMethod = k.FirstChild.Value;
                                        }
                                        if ("CabMethodDM".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.CabMethodDM = k.FirstChild.Value;
                                        }
                                        if ("Fitting".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Fitting = k.FirstChild.Value;
                                        }
                                        if ("FittingDM".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.FittingDM = k.FirstChild.Value;
                                        }
                                        if ("SamCnt".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.SamCnt = Convert.ToInt32(k.FirstChild.Value);
                                        }
                                        if ("ZeroB".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.ZeroB = Convert.ToBoolean(k.FirstChild.Value);
                                        }
                                        if ("Length".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Length = k.FirstChild.Value;
                                        }
                                        if ("Unit".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Unit = k.FirstChild.Value;
                                        }
                                        if ("Equation".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.QPar.Equation = k.FirstChild.Value;
                                        }
                                        if ("F1".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.F1 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("F2".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.F2 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("F3".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.F3 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("F4".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.F4 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K10".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K10 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K11".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K11 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K12".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K12 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K13".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K13 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("AFCS1".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.AFCS1 = k.FirstChild.Value;
                                        }
                                        if ("R11".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.R11 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K100".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K100 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K110".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K110 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K120".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K120 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K130".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K130 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("CFCS1".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.CFCS1 = k.FirstChild.Value;
                                        }
                                        if ("R12".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.R12 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K20".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K20 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K21".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K21 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K22".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K22 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K23".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K23 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("AFCS2".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.AFCS2 = k.FirstChild.Value;
                                        }
                                        if ("R21".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.R12 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K200".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K200 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K210".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K210 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K220".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K220 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("K230".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.K230 = float.Parse(k.FirstChild.Value);
                                        }
                                        if ("CFCS2".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.CFCS2 = k.FirstChild.Value;
                                        }
                                        if ("R22".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            this.QPar.R22 = float.Parse(k.FirstChild.Value);
                                        }

                                        if ("SamList".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            //this.QPar.R22 = float.Parse(k.FirstChild.Value);
                                            this.QPar.SamList = new List<DualSample>();
                                            for (int i = 0; i < k.FirstChild.Value.Split(';').Count() - 1; i++)
                                            {
                                                DualSample sample = new DualSample();
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[0] != "")
                                                {
                                                    sample.XGD1 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[0]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[1] != "")
                                                {
                                                    sample.XGD2 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[1]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[2] != "")
                                                {
                                                    sample.ND1 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[2]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[3] != "")
                                                {
                                                    sample.ND2 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[3]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[4] != "")
                                                {
                                                    sample.D_sj = DateTime.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[4]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[5] != "")
                                                {
                                                    sample.C_bz = k.FirstChild.Value.Split(';')[i].Split(',')[5];
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[6] != "")
                                                {
                                                    sample.IsExclude = Convert.ToBoolean(k.FirstChild.Value.Split(';')[i].Split(',')[6]);
                                                }

                                                this.QPar.SamList.Add(sample);
                                            }
                                        }

                                        if ("cbSamList".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            //this.QPar.R22 = float.Parse(k.FirstChild.Value);
                                            this.QPar.SamList = new List<DualSample>();
                                            for (int i = 0; i < k.FirstChild.Value.Split(';').Count() - 1; i++)
                                            {
                                                DualSample sample = new DualSample();
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[0] != "")
                                                {
                                                    sample.XGD1 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[0]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[1] != "")
                                                {
                                                    sample.XGD2 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[1]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[2] != "")
                                                {
                                                    sample.ND1 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[2]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[3] != "")
                                                {
                                                    sample.ND2 = float.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[3]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[4] != "")
                                                {
                                                    sample.D_sj = DateTime.Parse(k.FirstChild.Value.Split(';')[i].Split(',')[4]);
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[5] != "")
                                                {
                                                    sample.C_bz = k.FirstChild.Value.Split(';')[i].Split(',')[5];
                                                }
                                                if (k.FirstChild.Value.Split(';')[i].Split(',')[6] != "")
                                                {
                                                    sample.IsExclude = Convert.ToBoolean(k.FirstChild.Value.Split(';')[i].Split(',')[6]);
                                                }

                                                this.QPar.cbSamList.Add(sample);
                                            }
                                        }



                                    }
                                }
                            }
                        }
                    }

                    ShowQm();
                }
            }
        }

        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".mdual";

        private void Save_PreviewMouseDown(object sender, EventArgs e)
        {
            using (SaveFrm save = new SaveFrm(extension, "Двухкомпонентный анализ"))
            {
                save.btnOK.PreviewMouseLeftButtonUp += ((param0_1, param1_1) =>
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
                            //  EncriptorFileBase64 encriptorFileBase64 = new EncriptorFileBase64(filepath, pathTemp);
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

            XmlNode Settings = xd.CreateElement("Settings");

            XmlNode MeasureMethodName = xd.CreateElement("WL1"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.WL1; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("WL2"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.WL2; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("MCnt"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.MCnt.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("EConvert"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.EConvert.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("CabMethod"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.CabMethod; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("CabMethodDM"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.CabMethodDM; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("Fitting"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.Fitting; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("FittingDM"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.FittingDM; // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("SamCnt"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.SamCnt.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("ZeroB"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.ZeroB.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("Length"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.Length.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("Unit"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.Unit.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("Equation"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.QPar.Equation.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("F1"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.F1.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("F2"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.F2.ToString(); // и значение

            MeasureMethodName = xd.CreateElement("F3"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.F3.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("F4"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.F4.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит             

            MeasureMethodName = xd.CreateElement("K10"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K10.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K11"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K11.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K12"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K12.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K13"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K13.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("AFCS1"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.AFCS1.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("R11"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.R11.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K100"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K100.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K110"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K110.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K120"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K120.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K130"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K130.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит

            MeasureMethodName = xd.CreateElement("CFCS1"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.CFCS1.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("R12"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.R12.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K20"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K20.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K21"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K21.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K22"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K22.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K23"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K23.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("AFCS2"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.AFCS2.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("R21"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.R21.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K200"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K200.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K210"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K210.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K220"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K220.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("K230"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.K230.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("CFCS2"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.CFCS2.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            MeasureMethodName = xd.CreateElement("R22"); //Настройки измерения
            MeasureMethodName.InnerText = this.QPar.R22.ToString(); // и значение
            Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 

            string xmlValue = "";
            if (this.QPar.SamList != null || this.QPar.SamList.Count >= 0)
            {
                for (int index = 0; index < this.QPar.SamList.Count; index++)
                {
                    xmlValue = xmlValue + this.QPar.SamList[index].XGD1 + "," + this.QPar.SamList[index].XGD2 + "," + this.QPar.SamList[index].ND1 + "," + this.QPar.SamList[index].ND2 + "," + this.QPar.SamList[index].D_sj + "," + this.QPar.SamList[index].C_bz + "," + this.QPar.SamList[index].IsExclude;

                    xmlValue = xmlValue + ";";
                }
                MeasureMethodName = xd.CreateElement("SamList"); //Настройки измерения
                MeasureMethodName.InnerText = xmlValue; // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 
            }

            xmlValue = "";
            if (this.QPar.cbSamList != null || this.QPar.cbSamList.Count >= 0)
            {
                for (int index = 0; index < this.QPar.cbSamList.Count; index++)
                {
                    xmlValue = xmlValue + this.QPar.cbSamList[index].XGD1 + "," + this.QPar.cbSamList[index].XGD2 + "," + this.QPar.cbSamList[index].ND1 + "," + this.QPar.cbSamList[index].ND2 + "," + this.QPar.cbSamList[index].D_sj + "," + this.QPar.cbSamList[index].C_bz + "," + this.QPar.cbSamList[index].IsExclude;

                    xmlValue = xmlValue + ";";
                }
                MeasureMethodName = xd.CreateElement("cbSamList"); //Настройки измерения
                MeasureMethodName.InnerText = xmlValue; // и значение
                Settings.AppendChild(MeasureMethodName); // и указываем кому принадлежит 
            }

            xd.DocumentElement.AppendChild(Settings);
            fs.Close();         // Закрываем поток  
            xd.Save(filepath); // Сохраняем файл  
        }

        private void BtnOK_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void BtnBack_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        public void Dispose()
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this.ShowQm();
        }

        public void ShowQm()
        {
            this.lblwl1V.Content = this.QPar.QPar.WL1;
            this.lblWL2V.Content = this.QPar.QPar.WL2;
            this.lblMcntV.Content = this.QPar.QPar.MCnt.ToString();
            this.lbleconvertV.Content = this.QPar.QPar.EConvert ? CommonFun.GetLanText("active") : CommonFun.GetLanText("closed");
            this.lblCabMethodV.Content = this.QPar.QPar.CabMethod;
            this.lblCabMethodV.Tag = (object)this.QPar.QPar.CabMethodDM;
            this.lblFittingV.Content = this.QPar.QPar.Fitting;
            this.lblFittingV.Tag = (object)this.QPar.QPar.FittingDM;
            this.lblSamCntV.Content = this.QPar.QPar.SamCnt.ToString();
            this.lblZeroBV.Content = this.QPar.QPar.ZeroB ? CommonFun.GetLanText("active") : CommonFun.GetLanText("closed");
            this.lblgcv.Content = this.QPar.QPar.Length;
            this.lblunitV.Content = this.QPar.QPar.Unit;
            this.lblfcv.Content = this.QPar.QPar.Equation;

            float num = this.QPar.F1;
            string str1 = "F1: " + num.ToString();
            lblf1.Content = str1;

            num = this.QPar.F2;
            string str2 = "F2: " + num.ToString();
            lblf2.Content = str2;

            num = this.QPar.F3;
            string str3 = "F3: " + num.ToString();
            lblf3.Content = str3;

            num = this.QPar.F4;
            string str4 = "F4: " + num.ToString();
            lblf4.Content = str4;
            if (this.QPar.QPar.Equation == "Abs=f(C)")
            {
                this.lblsc1.Content = CommonFun.GetLanText("standardcurve") + " 1: " + this.QPar.AFCS1 + ", R1=" + (object)this.QPar.R11;
                this.lblsc2.Content = CommonFun.GetLanText("standardcurve") + " 2: " + this.QPar.AFCS2 + ", R1=" + (object)this.QPar.R21;
            }
            else
            {
                this.lblsc1.Content = CommonFun.GetLanText("standardcurve") + " 1: " + this.QPar.CFCS1 + ", R1=" + (object)this.QPar.R21;
                this.lblsc2.Content = CommonFun.GetLanText("standardcurve") + " 2: " + this.QPar.CFCS2 + ", R2=" + (object)this.QPar.R21;
            }
            if (this.QPar.QPar.CabMethod != CommonFun.GetLanText("inputr"))
            {
                this.label5.Visibility = Visibility.Visible;
                this.lblcb1.Visibility = Visibility.Visible;
                this.lblcb2.Visibility = Visibility.Visible;
                this.label6.Visibility = Visibility.Visible;
                this.lblsample1.Visibility = Visibility.Visible;
                this.lblsample2.Visibility = Visibility.Visible;
                this.lblsample3.Visibility = Visibility.Visible;
                this.lblsample4.Visibility = Visibility.Visible;
                if (this.QPar.cbSamList != null && this.QPar.cbSamList.Count > 0)
                {
                    foreach (DualSample cbSam in this.QPar.cbSamList)
                    {
                        Label lblcb1 = this.lblcb1;
                        string text1 = this.lblcb1.Content.ToString();
                        float? nullable = cbSam.XGD1;
                        string str5 = nullable.ToString();
                        string str6 = text1 + "\r\n" + str5;
                        lblcb1.Content = str6;
                        Label lblcb2 = this.lblcb2;
                        string text2 = this.lblcb2.Content.ToString();
                        nullable = cbSam.XGD2;
                        string str7 = nullable.ToString();
                        string str8 = text2 + "\r\n" + str7;
                        lblcb2.Content = str8;
                    }
                }
                if (this.QPar.SamList == null || this.QPar.SamList.Count <= 0)
                    return;
                foreach (DualSample sam in this.QPar.SamList)
                {
                    Label lblsample1 = this.lblsample1;
                    string text1 = this.lblsample1.Content.ToString();
                    float? nullable = sam.XGD1;
                    string str5 = nullable.ToString();
                    string str6 = text1 + "\r\n" + str5;
                    lblsample1.Content = str6;
                    Label lblsample2 = this.lblsample2;
                    string text2 = this.lblsample2.Content.ToString();
                    nullable = sam.XGD2;
                    string str7 = nullable.ToString();
                    string str8 = text2 + "\r\n" + str7;
                    lblsample2.Content = str8;
                    Label lblsample3 = this.lblsample3;
                    string text3 = this.lblsample3.Content.ToString();
                    nullable = sam.ND1;
                    string str9 = nullable.ToString();
                    string str10 = text3 + "\r\n" + str9;
                    lblsample3.Content = str10;
                    Label lblsample4 = this.lblsample4;
                    string text4 = this.lblsample4.Content.ToString();
                    nullable = sam.ND2;
                    string str11 = nullable.ToString();
                    string str12 = text4 + "\r\n" + str11;
                    lblsample4.Content = str12;
                }
            }
            else
            {
                this.label5.Visibility = Visibility.Collapsed;
                this.lblcb1.Visibility = Visibility.Collapsed;
                this.lblcb2.Visibility = Visibility.Collapsed;
                this.label6.Visibility = Visibility.Collapsed;
                this.lblsample1.Visibility = Visibility.Collapsed;
                this.lblsample2.Visibility = Visibility.Collapsed;
                this.lblsample3.Visibility = Visibility.Collapsed;
                this.lblsample4.Visibility = Visibility.Collapsed;
            }
        }
    }
}
