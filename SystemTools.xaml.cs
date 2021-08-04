using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SQLite;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для SystemTools.xaml
    /// </summary>
    public partial class SystemTools : Window, IDisposable
    {
        public SerialPort sp = new SerialPort();
        private ComStatus ComSta;
        public Thread dealth;
        private bool runptag = true;
        private int DDS = 0;
        private int WDS = 0;
        public List<string> rightlist = new List<string>();
        private Thread tdstart;
        private int syscaltype = 0;
        private bool restore = false;
        private Queue myque = new Queue();
        private int cnt = 0;
        private int tickcnt = 0;
        private System.Timers.Timer tdwait = new System.Timers.Timer(5000.0);
        DispatcherTimer timer1 = new DispatcherTimer();

        public void Dispose()
        {
            
        }

        public SystemTools()
        {
            InitializeComponent();
            this.panelleft.IsEnabled = true;
            timer1.Interval = TimeSpan.FromMilliseconds(1000);
            timer1.Tick += new EventHandler(this.timer1_Tick);
            this.ShowPara();
            this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
            this.tdstart.Start();
            this.setstate(false);
            if (!(CommonFun.GetAppConfig("currentconnect") == "-1"))
                return;
            this.btnBack.IsEnabled = true;
        }

        private void setstate(bool status)
        {
            this.btnBack.IsEnabled = status;
           // this.btnEngineer.Enabled = status;
            this.lblXTJZ.IsEnabled = status;
            this.lblxtjzc.IsEnabled = status;
            this.lblBrandWidth.IsEnabled = status;
            this.lblGPDK.IsEnabled = status;
            this.lblLightSource.IsEnabled = status;
            this.lblGY.IsEnabled = status;
            this.btnDefaultSet.IsEnabled = status;
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
            {
                if (this.rightlist.Contains("rightsysbandwidth") && status)
                {
                    this.lblBrandWidth.IsEnabled = true;
                    this.lblGPDK.IsEnabled = true;
                }
                else
                {
                    this.lblBrandWidth.IsEnabled = false;
                    this.lblGPDK.IsEnabled = false;
                }
                if (this.rightlist.Contains("rightsyslight") && status)
                {
                    this.lblLightSource.IsEnabled = true;
                    this.lblGY.IsEnabled = true;
                }
                else
                {
                    this.lblLightSource.IsEnabled = false;
                    this.lblGY.IsEnabled = false;
                }
                if (this.rightlist.Contains("rightsyscalibration") && status)
                {
                    this.lblXTJZ.IsEnabled = true;
                    this.lblxtjzc.IsEnabled = true;
                }
                else
                {
                    this.lblXTJZ.IsEnabled = false;
                    this.lblxtjzc.IsEnabled = false;
                }
                if (this.rightlist.Contains("rightsyssetaccuracy") && status)
                {
                    this.lblJD.IsEnabled = true;
                    this.lblAccurcy.IsEnabled = true;
                }
                else
                {
                    this.lblJD.IsEnabled = false;
                    this.lblAccurcy.IsEnabled = false;
                }
                if (this.rightlist.Contains("rightsyslanguage") && status)
                {
                    //this.lblLanV.Enabled = true;
                   // this.lblLanguage.Enabled = true;
                }
                else
                {
                   // this.lblLanV.Enabled = false;
                    //this.lblLanguage.Enabled = false;
                }
                if (this.rightlist.Contains("rightsysprinter") && status)
                {
                    //this.label15.Enabled = true;
                  //  this.lblPrinters.Enabled = true;
                }
                else
                {
                    //this.label15.Enabled = false;
                  //  this.lblPrinters.Enabled = false;
                }
                if (this.rightlist.Contains("rightsysreset") && status)
                    this.btnDefaultSet.IsEnabled = true;
                else
                    this.btnDefaultSet.IsEnabled = false;
                if (this.rightlist.Contains("rightsysselfcheck") && status)
                {
                    this.lblKJZJ.IsEnabled = true;
                   // this.lblkjzjc.IsEnabled = true;
                }
                else
                {
                    this.lblKJZJ.IsEnabled = false;
               //     this.lblkjzjc.IsEnabled = false;
                }
                //if (this.rightlist.Contains("rightsysengineer") && status)
                 //   this.btnEngineer.Enabled = true;
              //  else
                 //   this.btnEngineer.Enabled = false;
            }
            if (!(CommonFun.GetAppConfig("tunablebandwidth") == "false"))
                return;
            this.lblBrandWidth.IsEnabled = false;
            this.lblGPDK.IsEnabled = false;
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
                this.ComSta = ComStatus.END;
            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.Message.ToString(), "Error");
            }
            if (!this.btnBack.Dispatcher.CheckAccess())
                this.btnBack.Dispatcher.Invoke((Delegate)new Del_setstate(this.setstate), (object)true);
            else
                this.setstate(true);
        }
        private void btnBack_PreviewMouseDown(object sender, EventArgs e)
        {
            if (this.sp.IsOpen)
            {
                CommonFun.WriteSendLine("Выход в меню");
                this.sp.Close();
            }
            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();
            this.timer1.Stop();
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
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (this.sp.IsOpen)
                {
                    switch (this.ComSta)
                    {
                        case ComStatus.Connect:
                        case ComStatus.SYSTATE:
                        case ComStatus.RECHDCUR:
                        case ComStatus.SETSWL:
                        case ComStatus.SETCHP:
                        case ComStatus.SETSLIT:
                        case ComStatus.SETLAMP:
                        case ComStatus.SCANBASE:
                        case ComStatus.WLCALIB:
                        case ComStatus.RESETCHP:
                        case ComStatus.SET_FIVEEIGHTFRAME:
                            this.myque.Enqueue((object)this.sp.ReadLine());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine("sp_DataReceived error" + ex.ToString() + "," + (object)this.ComSta + "," + this.sp.IsOpen.ToString());
                this.ComSta = ComStatus.END;
                this.SetState();
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
                            case ComStatus.RECHDCUR:
                                CommonFun.WriteLine(text);
                                if (text.Contains("*A#"))
                                {
                                    ++this.cnt;
                                    if (this.cnt == 2)
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (!this.progressBar1.Dispatcher.CheckAccess())
                                            this.progressBar1.Dispatcher.Invoke((Delegate)new Del_finishscanbase(this.scanbasefinish));
                                        else
                                            this.scanbasefinish();
                                        this.cnt = 0;
                                        CommonFun.showbox("Коррекция темнового тока завершена", "Information");
                                    }
                                    break;
                                }
                                break;
                            case ComStatus.SETSWL:
                                CommonFun.WriteLine(text);
                                if (text.Contains("*A#"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (!this.progressBar1.Dispatcher.CheckAccess())
                                        this.progressBar1.Dispatcher.Invoke((Delegate)new Del_finishscanbase(this.scanbasefinish));
                                    else
                                        this.scanbasefinish();
                                    this.restore = false;
                                    CommonFun.showbox("Завершено", "Information");
                                    break;
                                }
                                break;
                            case ComStatus.SETCHP:
                                CommonFun.WriteLine(text);
                                if (text.Contains("*A#"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (!this.progressBar1.Dispatcher.CheckAccess())
                                        this.progressBar1.Dispatcher.Invoke((Delegate)new Del_StartSystemCalibration(this.StartSysCal), (object)this.syscaltype);
                                    else
                                        this.StartSysCal(this.syscaltype);
                                    break;
                                }
                                break;
                            case ComStatus.SETSLIT:
                                CommonFun.WriteLine(text);
                                if (text.Contains("*A#"))
                                {
                                    if (this.restore)
                                    {
                                        this.ComSta = ComStatus.SETSWL;
                                        this.sp.WriteLine("setswl 3400\r\n");
                                        CommonFun.WriteSendLine("setswl 3400");
                                    }
                                    else
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (!this.progressBar1.Dispatcher.CheckAccess())
                                            this.progressBar1.Dispatcher.Invoke((Delegate)new Del_finishscanbase(this.scanbasefinish));
                                        else
                                            this.scanbasefinish();
                                        CommonFun.showbox("Завершено", "Information");
                                    }
                                    break;
                                }
                                break;
                            case ComStatus.SETLAMP:
                                CommonFun.WriteLine(text);
                                try
                                {
                                    if (text.Contains("*A#"))
                                    {
                                        this.ComSta = ComStatus.END;
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    this.ComSta = ComStatus.END;
                                    CommonFun.showbox("errorretry" + ex.ToString(), "Error");
                                    break;
                                }
                            case ComStatus.SCANBASE:
                                CommonFun.WriteLine(text);
                                if (text.Contains("END"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (!this.progressBar1.Dispatcher.CheckAccess())
                                        this.progressBar1.Dispatcher.Invoke((Delegate)new Del_finishscanbase(this.scanbasefinish));
                                    else
                                        this.scanbasefinish();
                                    CommonFun.showbox("Коррекция базовой линии завершена", "Information");
                                    break;
                                }
                                break;
                            case ComStatus.WLCALIB:
                                CommonFun.WriteLine(text);
                                if (text.Contains("*A# 6 1"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (!this.progressBar1.Dispatcher.CheckAccess())
                                        this.progressBar1.Dispatcher.Invoke((Delegate)new Del_finishscanbase(this.scanbasefinish));
                                    else
                                        this.scanbasefinish();
                                    CommonFun.showbox("Коррекция длины волны завершена", "Information");
                                    break;
                                }
                                if (text.Contains("*A# 6 -1"))
                                {
                                    this.ComSta = ComStatus.END;
                                    if (!this.progressBar1.Dispatcher.CheckAccess())
                                        this.progressBar1.Dispatcher.Invoke((Delegate)new Del_finishscanbase(this.scanbasefinish));
                                    else
                                        this.scanbasefinish();
                                    CommonFun.showbox("Коррекция длины волны завершена с ошибкой", "Information");
                                    break;
                                }
                                break;
                            case ComStatus.RESETCHP:
                                CommonFun.WriteLine(text);
                                if (text.Contains("*A#"))
                                {
                                    this.ComSta = ComStatus.END;
                                    CommonFun.showbox("Завершено", "Information");
                                    break;
                                }
                                break;
                            case ComStatus.SET_FIVEEIGHTFRAME:
                                CommonFun.WriteLine(text);
                                if (text.Contains("*A#"))
                                {
                                    this.ComSta = ComStatus.RESETCHP;
                                    this.sp.WriteLine("selftest 5\r\n");
                                    CommonFun.WriteSendLine("selftest 5");
                                    break;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.cnt = 0;
                        this.ComSta = ComStatus.END;
                        this.SetState();
                        CommonFun.showbox(ex.Message.ToString(), "Error");
                    }
                }
            }
        }

        private void StartSysCal(int k)
        {
            switch (k)
            {
                case 0:
                    this.sp.WriteLine("SELFTEST 6\r\n");
                    CommonFun.WriteSendLine("SELFTEST 6");
                    this.ComSta = ComStatus.WLCALIB;
                    this.SetState();
                    this.progressBar1.Value = 0;
                   // this.panel4.Visible = true;
                   // this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "inwlcorrection");
                    this.timer1.IsEnabled = true;
                    CommonFun.InsertLog("System", "Wavelentgth Correction", false);
                    break;
                case 1:
                    this.sp.WriteLine("scanbase\r\n");
                    CommonFun.WriteSendLine("scanbase");
                    this.ComSta = ComStatus.SCANBASE;
                    this.SetState();
                    this.progressBar1.Value = 0;
                    this.timer1.IsEnabled = true;
                //    this.panel4.Visible = true;
                ///    this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "inscanbase");
                    CommonFun.InsertLog("System", "System baseline correction", false);
                    break;
                case 2:
                    this.sp.WriteLine("rechdcur\r\n");
                    CommonFun.WriteSendLine("rechdcur");
                    this.SetState();
                    this.ComSta = ComStatus.RECHDCUR;
                    this.progressBar1.Value = 0;
                   // this.panel4.Visible = true;
                    this.timer1.IsEnabled = true;
                  //  this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "indcurrentcorrection");
                    CommonFun.InsertLog("System", "Dark current correction", false);
                    break;
            }
        }
        private void scanbasefinish()
        {
            this.timer1.Stop();
           this.timer1.IsEnabled = false;
            this.progressBar1.Value = 000;
            
          //  this.panel4.Visible = false;
            this.SetState();
        }
        private void setprobar(int value)
        {
            if (this.progressBar1.Value + value >= 100)
                this.progressBar1.Value = 000;
            else
                this.progressBar1.Value += value;
        }

        private void SetState()
        {
            if (this.ComSta == ComStatus.END)
            {
               // this.btnEngineer.Enabled = true;
              //  this.btnswitchconncet.Enabled = true;
                this.btnDefaultSet.IsEnabled = true;
                this.btnBack.IsEnabled = true;
                this.panelleft.IsEnabled = true;
                if (!(CommonFun.GetAppConfig("tunablebandwidth") == "false"))
                    return;
                this.lblBrandWidth.IsEnabled = false;
                this.lblGPDK.IsEnabled = false;
                this.progressBar1.Value = 0;
            }
            else
            {
               // this.btnEngineer.Enabled = false;
              //  this.btnswitchconncet.Enabled = false;
                this.btnDefaultSet.IsEnabled = false;
                this.btnBack.IsEnabled = false;
               this.panelleft.IsEnabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ++this.tickcnt;
            switch (this.ComSta)
            {
                case ComStatus.RECHDCUR:
                    if (!this.progressBar1.Dispatcher.CheckAccess())
                    {
                        this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetProcessbar(this.setprobar), (object)5);
                        break;
                    }
                    this.setprobar(5);
                    break;
                case ComStatus.SCANBASE:
                    if (this.tickcnt % 10 == 0)
                    {
                        if (!this.progressBar1.Dispatcher.CheckAccess())
                        {
                            this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetProcessbar(this.setprobar), (object)2);
                            break;
                        }
                        this.setprobar(2);
                        break;
                    }
                    break;
                case ComStatus.WLCALIB:
                    if (!this.progressBar1.Dispatcher.CheckAccess())
                    {
                        this.progressBar1.Dispatcher.Invoke((Delegate)new Del_SetProcessbar(this.setprobar), (object)2);
                        break;
                    }
                    this.setprobar(2);
                    break;
            }
            if (this.tickcnt != 1800 || this.ComSta != ComStatus.SCANBASE && this.ComSta != ComStatus.WLCALIB)
                return;
            this.ComSta = ComStatus.END;
            if (!this.progressBar1.Dispatcher.CheckAccess())
                this.progressBar1.Dispatcher.Invoke((Delegate)new Del_finishscanbase(this.scanbasefinish));
            else
                this.scanbasefinish();
            CommonFun.showbox("error, please retry", "Error");
        }

        private void Home_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }

        private void BtnStandartSettings_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBack_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Zero_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Bandwidth_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Calibration_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            //TypeCalibration typeCalibration_ = new TypeCalibration();
            //typeCalibration_.ShowDialog();
            using (TypeCalibration frm = new TypeCalibration())
            {
                frm.Closed += (EventHandler)((param0_2, param1_2) =>
                {
                    switch (frm.type_calibration)
                    {
                        case 0:
                            this.sp.WriteLine("SELFTEST 6\r\n");
                            CommonFun.WriteSendLine("SELFTEST 6");
                            this.ComSta = ComStatus.WLCALIB;
                            this.SetState();
                            this.progressBar1.Value = 0;
                            //    this.panel4.Visible = true;
                            //   this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "inwlcorrection");
                            this.timer1.IsEnabled = true;
                            CommonFun.InsertLog("System", "Wavelentgth Correction", false);
                            break;
                        case 1:
                            this.sp.WriteLine("scanbase\r\n");
                            CommonFun.WriteSendLine("scanbase");
                            this.ComSta = ComStatus.SCANBASE;
                            this.SetState();
                            this.progressBar1.Value = 0;
                            //    this.panel4.Visible = true;
                            //   this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "inwlcorrection");
                            this.timer1.IsEnabled = true;
                            CommonFun.InsertLog("System", "System baseline correction", false);
                            break;
                        case 2:
                            this.sp.WriteLine("rechdcur\r\n");
                            CommonFun.WriteSendLine("rechdcur");
                            this.SetState();
                            this.ComSta = ComStatus.RECHDCUR;
                            this.progressBar1.Value = 0;
                            //    this.panel4.Visible = true;
                            //   this.lblProsess.Text = CommonFun.GetLanText(this.lanvalue, "inwlcorrection");
                            this.timer1.IsEnabled = true;
                            CommonFun.InsertLog("System", "Dark current correction", false);
                            break;
                    }
                });
                

                int num2 = Convert.ToInt32(frm.ShowDialog());

            }
            
        }

        private void LightSource_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.WDS = Convert.ToInt32(CommonFun.GetAppConfig("WDStatus"));
            this.DDS = Convert.ToInt32(CommonFun.GetAppConfig("DDStatus"));
            using (LightSource frm = new LightSource())
            {
               
                if (this.WDS == 1)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                    bi3.EndInit();
                    frm.pibWD.Source = bi3;
                    //frm.pibWD.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                    frm.pibWD.Tag = (object)"on";
                }
                else
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                    bi3.EndInit();
                    frm.pibWD.Source = bi3;
                    //frm.pibWD.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                    frm.pibWD.Tag = (object)"off";
                }
                if (this.DDS == 1)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                    bi3.EndInit();
                    frm.pibDD.Source = bi3;
                  //  frm.pibDD.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                    frm.pibDD.Tag = (object)"on";
                }
                else
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                    bi3.EndInit();
                    frm.pibDD.Source = bi3;
                    //frm.pibDD.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                    frm.pibDD.Tag = (object)"off";
                }
                frm.lblDDH.Content = CommonFun.GetAppConfig("DDTime") + "H";
                frm.lblWDH.Content = CommonFun.GetAppConfig("WDTime") + "H";
                frm.lblvalue.Content = CommonFun.GetAppConfig("SwithWL");

                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    if (!this.sp.IsOpen)
                        CommonFun.showbox("opencom", "Warning");
                    else if (this.ComSta != ComStatus.END)
                    {
                        CommonFun.showbox("Last command is not finished!", "Warning");
                    }
                    else
                    {
                        Decimal num1 = Convert.ToDecimal(frm.lblvalue.Content);
                        if (num1 < 325M || num1 > 355M)
                        {
                            CommonFun.showbox("errordata", "Error");
                        }
                        else
                        {
                            this.sp.WriteLine("setswl " + Convert.ToInt32(num1 * 10M).ToString() + "\r\n");
                            CommonFun.WriteSendLine("setswl " + Convert.ToInt32(num1 * 10M).ToString());
                            CommonFun.Set("SwithWL", frm.lblvalue.Content.ToString());
                            CommonFun.InsertLog("System", "Modify the light source switch point", true);
                            string str1;
                            string str2;
                            if (frm.pibDD.Tag == (object)"off")
                            {
                                int num2 = 0;
                                if (num2 != this.DDS)
                                {
                                    this.DDS = num2;
                                    this.sp.WriteLine("setlamp 1 0\r\n");
                                    this.ComSta = ComStatus.SETLAMP;
                                    CommonFun.WriteSendLine("setlamp 1 0");
                                    CommonFun.Set("DDStatus", "0");
                                    str1 = "";
                                    SQLiteHelper.ExecuteNonQuery("Data Source=programdb.db;Version=3;", "insert into LampOperation (D_time,C_oper,C_lamp,C_user) values (@D_time,@C_oper,@C_lamp,@C_user)", (object)DateTime.Now, (object)"Close", (object)"DD", (object)CommonFun.GetAppConfig("currentuser"));
                                    DateTime? nullable = new DateTime?();
                                    using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
                                    {
                                        IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from  LampOperation  where C_lamp=@C_lamp and C_oper=@C_oper order by D_time desc  LIMIT  0, 1", new object[2]
                                        {
                      (object) "DD",
                      (object) "Open"
                                        });
                                        while (dataReader.Read())
                                            nullable = dataReader.IsDBNull(dataReader.GetOrdinal("D_time")) ? new DateTime?() : (DateTime?)dataReader["D_time"];
                                    }
                                    if (nullable.HasValue)
                                    {
                                        TimeSpan timeSpan = DateTime.Now.Subtract(nullable.Value);
                                        if (timeSpan.Hours >= 0)
                                        {
                                            int int32_1 = Convert.ToInt32(CommonFun.GetAppConfig("DDTime"));
                                            int int32_2 = Convert.ToInt32(CommonFun.GetAppConfig("DDTTime"));
                                            int num3 = int32_1 + timeSpan.Hours;
                                            CommonFun.Set("DDTime", num3.ToString());
                                            num3 = int32_2 + timeSpan.Hours;
                                            CommonFun.Set("DDTTime", num3.ToString());
                                        }
                                    }
                                    CommonFun.InsertLog("System", "Close the deuterium lamp", false);
                                }
                            }
                            else if (1 == this.DDS)
                            {
                                this.sp.WriteLine("setlamp 1 1\r\n");
                                this.ComSta = ComStatus.SETLAMP;
                                CommonFun.WriteSendLine("setlamp 1 1");
                                CommonFun.Set("DDStatus", "1");
                                str2 = "";
                                SQLiteHelper.ExecuteNonQuery("Data Source=programdb.db;Version=3;", "insert into LampOperation (D_time,C_oper,C_lamp,C_user) values (@D_time,@C_oper,@C_lamp,@C_user)", (object)DateTime.Now, (object)"Open", (object)"DD", (object)CommonFun.GetAppConfig("currentuser"));
                                CommonFun.InsertLog("System", "Open the deuterium lamp", false);
                            }
                            if (frm.pibWD.Tag == (object)"off")
                            {
                                if (0 != this.WDS)
                                {
                                    this.sp.WriteLine("setlamp 2 0\r\n");
                                    this.ComSta = ComStatus.SETLAMP;
                                    CommonFun.WriteSendLine("setlamp 2 0");
                                    CommonFun.Set("WDStatus", "0");
                                    str1 = "";
                                    SQLiteHelper.ExecuteNonQuery("Data Source=programdb.db;Version=3;", "insert into LampOperation (D_time,C_oper,C_lamp,C_user) values (@D_time,@C_oper,@C_lamp,@C_user)", (object)DateTime.Now, (object)"Close", (object)"WD", (object)CommonFun.GetAppConfig("currentuser"));
                                    DateTime? nullable = new DateTime?();
                                    using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
                                    {
                                        IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from  LampOperation  where C_lamp=@C_lamp and C_oper=@C_oper order by D_time desc  LIMIT  0, 1", new object[2]
                                        {
                      (object) "WD",
                      (object) "Open"
                                        });
                                        while (dataReader.Read())
                                            nullable = dataReader.IsDBNull(dataReader.GetOrdinal("D_time")) ? new DateTime?() : (DateTime?)dataReader["D_time"];
                                    }
                                    if (nullable.HasValue)
                                    {
                                        TimeSpan timeSpan = DateTime.Now.Subtract(nullable.Value);
                                        if (timeSpan.Hours >= 0)
                                        {
                                            CommonFun.Set("WDTime", (Convert.ToInt32(CommonFun.GetAppConfig("WDTime")) + timeSpan.Hours).ToString());
                                            CommonFun.Set("WDTTime", (Convert.ToInt32(CommonFun.GetAppConfig("WDTTime")) + timeSpan.Hours).ToString());
                                        }
                                    }
                                    CommonFun.InsertLog("System", "Close the tungsten lamp", false);
                                }
                            }
                            else if (1 == this.WDS)
                            {
                                this.sp.WriteLine("setlamp 2 1\r\n");
                                this.ComSta = ComStatus.SETLAMP;
                                CommonFun.WriteSendLine("setlamp 2 1");
                                CommonFun.Set("WDStatus", "1");
                                str2 = "";
                                SQLiteHelper.ExecuteNonQuery("Data Source=programdb.db;Version=3;", "insert into LampOperation (D_time,C_oper,C_lamp,C_user) values (@D_time,@C_oper,@C_lamp,@C_user)", (object)DateTime.Now, (object)"Open", (object)"WD", (object)CommonFun.GetAppConfig("currentuser"));
                                CommonFun.InsertLog("System", "Open the tungsten lamp", false);
                            }
                            frm.lblDDH.Content = CommonFun.GetAppConfig("DDTime") + "ч";
                            frm.lblWDH.Content = CommonFun.GetAppConfig("WDTime") + "ч";
                            frm.Close(); frm.Dispose();
                            this.lblGY.Content = CommonFun.GetAppConfig("DDTime") + "ч/" + CommonFun.GetAppConfig("WDTime") + "ч  >";
                        }
                    }
                });
                int num = Convert.ToInt32(frm.ShowDialog());
            }
        }

        private void Accuracy_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using(SetAccuracy frm1 = new SetAccuracy())
            {
                frm1.Conc.Content = CommonFun.GetAppConfig("ceAccuracy");
                frm1.Abs.Content = CommonFun.GetAppConfig("absAccuracy");
                frm1.T.Content = CommonFun.GetAppConfig("tAccuracy");
                frm1.btnOK.PreviewMouseDown += ((param2, param3) =>
                {
                    CommonFun.Set("absAccuracy", frm1.Abs.Content.ToString());
                    CommonFun.Set("tAccuracy", frm1.T.Content.ToString());
                    CommonFun.Set("ceAccuracy", frm1.Conc.Content.ToString());
                    this.lblJD.Content = CommonFun.GetAppConfig("absAccuracy") + "," + CommonFun.GetAppConfig("tAccuracy") + "%," + CommonFun.GetAppConfig("ceAccuracy") + "  >";
                    frm1.Close(); frm1.Dispose();
                    CommonFun.InsertLog("System", "Modify display accuracy", false);
                });
                frm1.btnCancel.PreviewMouseDown += ((param0_2, param1_3) =>
                {
                    frm1.Close(); frm1.Dispose();
                });
                int num = Convert.ToInt32(frm1.ShowDialog());
            }
        }

        private void Printer_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void GeneralOptions_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (GeneralOptions frm = new GeneralOptions())
            {
                frm.lblwarmuptime.Content = CommonFun.GetAppConfig("Preheatingtime");
                if (CommonFun.GetAppConfig("AutoCalSY") == "true")
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("img/UI_DB_Switcher_On.png", UriKind.Relative);
                    bi3.EndInit();
                    frm.pibWD.Source = bi3;
                    //frm.pibWD.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                    frm.pibWD.Tag = (object)"on";
                }
                else
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("img/UI_DB_Switcher_Off.png", UriKind.Relative);
                    bi3.EndInit();
                    frm.pibWD.Source = bi3;
                    //frm.pibWD.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                    frm.pibWD.Tag = (object)"off";
                }
                if (CommonFun.GetAppConfig("EightSlot") == "true")
                {
                   // frm.lblypj.Enabled = true;
                  //  frm.lblypjv.Enabled = true;
                }
                else
                {
                  //  frm.lblypj.Enabled = false;
                   // frm.lblypjv.Enabled = false;
                }
                /*switch (CommonFun.GetAppConfig("SlotType"))
                {
                    case "0":
                        frm.lblypjv.Text = CommonFun.GetLanText(this.lanvalue, "fiveslot");
                        break;
                    case "1":
                        frm.lblypjv.Text = CommonFun.GetLanText(this.lanvalue, "eightslot");
                        break;
                }*/
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        int int32 = Convert.ToInt32(frm.lblwarmuptime.Content);
                        if (int32 < 15 || int32 > 120)
                        {
                            CommonFun.showbox("errordata", "Error");
                        }
                        else
                        {
                            CommonFun.Set("Preheatingtime", frm.lblwarmuptime.Content.ToString());
                            if (frm.pibWD.Tag== (object)"on")
                                CommonFun.Set("AutoCalSY", "true");
                            else
                                CommonFun.Set("AutoCalSY", "false");
                            /*if (frm.lblypj.Enabled && frm.lblypjv.Tag != null)
                            {
                                CommonFun.Set("SlotType", frm.lblypjv.Tag.ToString());
                                if (!this.sp.IsOpen)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText(this.lanvalue, "opencom"), "Warning");
                                    return;
                                }
                                if (this.ComSta != ComStatus.END)
                                {
                                    CommonFun.showbox(CommonFun.GetLanText(this.lanvalue, "waitforcmd"), "Warning");
                                    return;
                                }
                                this.ComSta = ComStatus.SET_FIVEEIGHTFRAME;
                                this.sp.WriteLine("SET_FIVEEIGHTFRAME " + frm.lblypjv.Tag.ToString() + "\r\n");
                                CommonFun.WriteSendLine("SET_FIVEEIGHTFRAME " + frm.lblypjv.Tag.ToString());
                            }*/
                            frm.Close(); frm.Dispose();
                            CommonFun.InsertLog("System", "Set the boot self check parameters", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonFun.showbox("errordata: " + ex.ToString(), "Error");
                    }
                });

                frm.btnCancel.PreviewMouseDown += ((param0_1, param1_1) => 
                {
                    frm.Close(); frm.Dispose();
                });
                int num = Convert.ToInt32(frm.ShowDialog());
            }
        }

        private void ID_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void SerialNumber_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void HardwareVersion_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void FirmwareVersion_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void SoftwareVersion_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }
        private void ShowPara()
        {
            //this.lbltype.Content = CommonFun.GetAppConfig("modelnumber");

            if (CommonFun.GetAppConfig("modelnumber") == "P7")
                this.lbltype.Content = "УФ-6700";
            if (CommonFun.GetAppConfig("modelnumber") == "P8")
                this.lbltype.Content = "УФ-6800";
            if (CommonFun.GetAppConfig("modelnumber") == "P9")
                this.lbltype.Content = "УФ-6900";
            this.lblserialno.Content = CommonFun.GetAppConfig("serialno");
            string[] strArray = CommonFun.GetAppConfig("version").Split(',');
            this.lblID.Content = strArray[0];
            if (((IEnumerable<string>)strArray).Count<string>() > 1)
                this.lblhv.Content = strArray[1];
            if (((IEnumerable<string>)strArray).Count<string>() > 2)
                this.lblfirmwareV.Content = strArray[2];
            this.lblGPDK.Content = CommonFun.GetAppConfig("Spectralbandwidth") + "(нм)  >";
            if (strArray[0].Contains("U"))
                this.lblGY.Content = CommonFun.GetAppConfig("DDTime") + "ч/" + CommonFun.GetAppConfig("WDTime") + "ч  >";
            if (strArray[0].Contains("X"))
            {
                this.lblGY.IsEnabled = false;
                this.lblLightSource.IsEnabled = false;
                this.lblGY.Content = "";
                //this.label24.Text = CommonFun.GetLanText(this.lanvalue, "XD");
            }
            this.lblJD.Content = CommonFun.GetAppConfig("absAccuracy") + "," + CommonFun.GetAppConfig("tAccuracy") + "%," + CommonFun.GetAppConfig("ceAccuracy") + "  >";
        }

        private delegate void Del_setstate(bool status);

        private delegate void Del_finishscanbase();

        private delegate void Del_SetProcessbar(int value);

        private delegate void Del_StartSystemCalibration(int type);
    }
}
