using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
       private SerialPort sp = new SerialPort();
        private string lanvalue = "";
        public bool nonPort; //Порт включен(выключен)
        public SerialPort newPort; //SerialPort
        public string portsName; //Имя порта

        private int step = 0;
        private int repeatcnt = 0;
        bool runptag = true;
        private int retry = 0;
        private Thread dealth;
        private System.Timers.Timer tdwait = new System.Timers.Timer(5000.0);
        private Thread tdstart;
        private Queue myque = new Queue();

        private int timecnt = 0;
        private int totalcnt = 0;

        private List<int> errorlist = new List<int>();
        private List<int> nerrorlist = new List<int>();
        private int retestcnt = 0;

        DispatcherTimer timer1 = new DispatcherTimer();

        string quest = "";
        //  private Queue myque = new Queue();
        CultureInfo culture;
               
        public void Dispose()
        {
            
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                // this.lanvalue = CommonFun.GetAppConfig("Language");
                // Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(this.lanvalue);
                // this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
                //Console.OutputEncoding = Encoding.UTF8;
                // Change current culture

                if (Thread.CurrentThread.CurrentCulture.Name == "ru-RU")
                    culture = CultureInfo.CreateSpecificCulture("en-US");
                else
                    culture = CultureInfo.CreateSpecificCulture("en-En");

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                timer1.Interval = TimeSpan.FromMilliseconds(1000);
                timer1.Tick += new EventHandler(this.timer1_Tick);

                Dispatcher.Invoke(() => this.btnRetry.IsEnabled = false);
                Dispatcher.Invoke(() => this.btnRetry.Visibility = Visibility.Collapsed);
                Dispatcher.Invoke(() => this.btnPass.IsEnabled = false);
                Dispatcher.Invoke(() => this.btnPass.Visibility = Visibility.Collapsed);
             

                /*Hide();
                SpectrumScan mertyFrm = new SpectrumScan();
                mertyFrm.ShowDialog();
                Window parentWindow = Window.GetWindow(this);
                parentWindow.Close();*/
                using (SelectUser selectUser = new SelectUser())
                {
                    selectUser.Loaded += (RoutedEventHandler)((param0_1, param1_1) =>
                    {
                        List<string[]> nullabel = new List<string[]>();

                        using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
                        {
                            IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from Users", new object[0]);
                            selectUser.ListData = new List<ComboDataUser>();
                            while (dataReader.Read())
                            {
                                nullabel.Add(new string[] { dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString() });

                                selectUser.ListData.Add(new ComboDataUser { Name = dataReader[0].ToString(), Password = dataReader[2].ToString() });
                            }
                            selectUser.BoxSelectUser.ItemsSource = selectUser.ListData;
                            //  BoxSelectUser.DisplayMemberPath = "Value";
                            //    BoxSelectUser.SelectedValuePath = "Id";

                            selectUser.BoxSelectUser.SelectedValue = "2";
                        }

                    });
                   
                    selectUser.btnCancel.PreviewMouseDown += ((param0_1, param1_1) =>
                    {
                        selectUser.Close();
                        this.Close();
                    });

                    selectUser.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
                    {
                        if (selectUser.TextPasswordUser.Text.ToString() == (from user in selectUser.ListData where user.Name == selectUser.BoxSelectUser.Text.ToString() select user.Password).ToList()[0].ToString())
                        {
                            CommonFun.Set("currentuser", selectUser.BoxSelectUser.Text.ToString());
                            selectUser.Close();

                            StartProgram();
                            /*Hide();
                            PhotoMertyFrm mertyFrm = new PhotoMertyFrm();
                            mertyFrm.ShowDialog();
                            Window parentWindow = Window.GetWindow(this);
                            parentWindow.Close();*/
                        }
                        else
                        {
                            CommonFun.showbox("Не верный пароль!", "Info");

                        }
                    });

                    selectUser.ShowDialog();
                }
               
                //this.btnPass.IsEnabled = true;
                //this.btnPass.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        public void StartProgram()
        {
            try { 
                tdwait.Elapsed += new ElapsedEventHandler(this.tdwait_Elapsed);
                string[] ports = SerialPort.GetPortNames();
              
                tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
                tdstart.Start();

             
            }
            catch (Exception ex)
            {
               // StatusSystem.Text += ex.ToString() + "\r\n";
            }
            // CommonFun.showbox(ports.Count<string>().ToString());

        }

        private void tdstart_Elapsed()
        {
            try
            {
                repeatcnt = 0;
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
                sp.WriteLine("CONNECT \r\n");

                this.step = 1;
                tdwait.Start();
                dealth = new Thread(new ThreadStart(this.DealRecData));
                dealth.Start();
            }
            catch
            {
                Dispatcher.Invoke(() => this.btnPass.IsEnabled = true);
                Dispatcher.Invoke(() => btnPass.Visibility = Visibility.Visible);
            }
            
        }

        private void tdwait_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.step == 1)
            {
                if (this.myque.Count > 0)
                {
                    string text = this.myque.Dequeue().ToString();
                    if (text.Contains("*A#"))
                    {
                        repeatcnt = 0;
                        /*if (sp.IsOpen)
                        {
                            sp.WriteLine("DISCONN \r\n");
                            //CommonFun.WriteSendLine("DISCONN");
                            sp.Close();
                        }
                        //++step;*/
                        tdwait.Stop();

                    }
                }
                else
                {
                    if (this.repeatcnt < 3)
                    {
                        sp.WriteLine("CONNECT \r\n");
                        Thread.Sleep(50);
                        repeatcnt += 1;

                    }
                    else
                    {
                        this.tdwait.Stop();
                        this.repeatcnt = 0;
                        if (this.sp.IsOpen)
                            this.sp.Close();
                       
                            this.runptag = false;
                            if (this.dealth != null)
                                this.dealth.Abort();
                    }
                }


            }
            else
            {
                this.tdwait.Stop();
                this.repeatcnt = 0;
            }


        }

        private void DealRecData()
        {
            try
            {
                while (this.runptag)
                {
                    if (this.myque.Count > 0)
                    {
                        string text = this.myque.Dequeue().ToString();
                        //StatusSystem.Text += text.ToString() + "\r\n";
                        if (retry == 1)
                        {
                            if (text.Contains("*A# 2 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lbldqhdj.Dispatcher.CheckAccess())
                                    this.lbldqhdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)3, (object)true);
                                else
                                    this.SetStatus(3, true);
                            }
                            else if (text.Contains("*A# 2 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(3);
                                if (!this.lbldqhdj.Dispatcher.CheckAccess())
                                    this.lbldqhdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)3, (object)false);
                                else
                                    this.SetStatus(3, false);
                            }
                            else if (text.Contains("*A# 3 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lbllgpdj.Dispatcher.CheckAccess())
                                    this.lbllgpdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)4, (object)true);
                                else
                                    this.SetStatus(4, true);
                            }
                            else if (text.Contains("*A# 3 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(4);
                                if (!this.lbllgpdj.Dispatcher.CheckAccess())
                                    this.lbllgpdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)4, (object)false);
                                else
                                    this.SetStatus(4, false);
                            }
                            else if (text.Contains("*A# 4 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lblxfdj.Dispatcher.CheckAccess())
                                    this.lblxfdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)5, (object)true);
                                else
                                    this.SetStatus(5, true);
                            }
                            else if (text.Contains("*A# 4 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(5);
                                if (!this.lblxfdj.Dispatcher.CheckAccess())
                                {
                                    Del_SetStatus delSetStatus = new Del_SetStatus(this.SetStatus);
                                    object[] objArray = new object[2]
                                    {
                                        (object) 5,
                                        (object) false
                                    };
                                    this.lblxfdj.Dispatcher.Invoke((Delegate) delSetStatus, (object)false);
                                }
                                else
                                    this.SetStatus(5, false);
                            }
                            else if (text.Contains("*A# 5 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lblypc.Dispatcher.CheckAccess())
                                    this.lblypc.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)7, (object)true);
                                else
                                    this.SetStatus(7, true);
                            }
                            else if (text.Contains("*A# 5 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(7);
                                if (!this.lblypc.Dispatcher.CheckAccess())
                                    this.lblypc.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)7, (object)false);
                                else
                                    this.SetStatus(7, false);
                            }
                            else if (text.Contains("*A# 1 1"))
                            {
                                ++this.retestcnt;
                                if (this.lbllgpdj.Dispatcher.CheckAccess())
                                    this.lbllgpdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)8, (object)true);
                                else
                                    this.SetStatus(8, true);
                            }
                            else if (text.Contains("*A# 1 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(8);
                                if (!this.lbllgpdj.Dispatcher.CheckAccess())
                                    this.lbllgpdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)8, (object)false);
                                else
                                    this.SetStatus(8, false);
                            }
                            else if (text.Contains("*A# 8 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lbladl.Dispatcher.CheckAccess())
                                    this.lbladl.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)9, (object)true);
                                else
                                    this.SetStatus(9, true);
                            }
                            else if (text.Contains("*A# 8 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(8);
                                if (!this.lbladl.Dispatcher.CheckAccess())
                                    this.lbladl.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)10, (object)false);
                                else
                                    this.SetStatus(10, false);
                            }
                            else if (text.Contains("*A# 6 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lblbcjz.Dispatcher.CheckAccess())
                                    this.lblbcjz.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)10, (object)true);
                                else
                                    this.SetStatus(10, true);
                            }
                            else if (text.Contains("*A# 6 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(10);
                                if (!this.lblbcjz.Dispatcher.CheckAccess())
                                    this.lblbcjz.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)10, (object)false);
                                else
                                    this.SetStatus(10, false);
                            }
                            else if (text.Contains("*A# 9 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lblpower.Dispatcher.CheckAccess())
                                    this.lblpower.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)11, (object)true);
                                else
                                    this.SetStatus(11, true);
                            }
                            else if (text.Contains("*A# 9 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(11);
                                if (!this.lblpower.Dispatcher.CheckAccess())
                                    this.lblpower.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)11, (object)false);
                                else
                                    this.SetStatus(11, false);
                            }
                            else if (text.Contains("*A# 7 1"))
                            {
                                ++this.retestcnt;
                                if (!this.lblsysbase.Dispatcher.CheckAccess())
                                    this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)12, (object)true);
                                else
                                    this.SetStatus(12, true);
                            }
                            else if (text.Contains("*A# 7 -1"))
                            {
                                ++this.retestcnt;
                                this.nerrorlist.Add(12);
                                if (!this.lblsysbase.Dispatcher.CheckAccess())
                                    this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)12, (object)false);
                                else
                                    this.SetStatus(12, false);
                            }
                            if (this.retestcnt == this.errorlist.Count)
                            {
                                this.retry = 0;
                                this.retestcnt = 0;
                                this.errorlist = this.nerrorlist;
                                this.nerrorlist = new List<int>();
                            }
                        }
                        else
                        {
                            switch (this.step)
                            {

                                case 1:
                                    if (!this.lblboot.Dispatcher.CheckAccess())
                                        this.lblboot.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                    else
                                        this.SetStatus(this.step, true);
                                    this.sp.WriteLine("GET_VERSION \r\n");
                                    CommonFun.WriteSendLine("GET_VERSION");
                                    ++this.step;
                                    break;
                                case 2:
                                    string[] strArray = text.Split(' ');
                                    bool sta = strArray[1].Contains("65");
                                    if (!this.lblboot.Dispatcher.CheckAccess())
                                        this.lblboot.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)sta);
                                    else
                                        this.SetStatus(this.step, sta);
                                    if (!this.lblboot.Dispatcher.CheckAccess())
                                        this.lblboot.Dispatcher.Invoke((Delegate) new Del_SetBandWidth(this.SetBandWidth), (object)text);
                                    else
                                        this.SetBandWidth(strArray[1]);
                                    this.sp.WriteLine("SELFTEST 2\r\n");
                                    CommonFun.WriteSendLine("SELFTEST 2");
                                    ++this.step;
                                    break;

                                case 3:
                                    if (text.Contains("*A# 2 1"))
                                    {
                                        if (!this.lbldqhdj.Dispatcher.CheckAccess())
                                            this.lbldqhdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        this.sp.WriteLine("SELFTEST 3\r\n");
                                        CommonFun.WriteSendLine("SELFTEST 3");
                                        ++this.step;
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 2\r\n");
                                        CommonFun.WriteSendLine("Unknown command, SELFTEST 2 again");
                                        this.step = 3;
                                        break;
                                    }
                                    if (text.Contains("*A# 2 -1"))
                                    {
                                        if (!this.lbldqhdj.Dispatcher.CheckAccess())
                                            this.lbldqhdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        this.sp.WriteLine("SELFTEST 3\r\n");
                                        CommonFun.WriteSendLine("last step fail,SELFTEST 3");
                                        ++this.step;
                                        break;
                                    }
                                    break;

                                case 4:
                                    if (text.Contains("*A# 3 1"))
                                    {
                                        if (!this.lbllgpdj.Dispatcher.CheckAccess())
                                            this.lbllgpdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        if (Dispatcher.Invoke(() => !this.lblxfdj.IsEnabled))
                                        {
                                            ++this.step;
                                            if (this.lblxfdj.Dispatcher.CheckAccess())
                                                this.lblxfdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                            else
                                                this.SetStatus(this.step, true);
                                            this.sp.WriteLine("detcuvch \r\n");
                                            CommonFun.WriteSendLine("last step ok,detcuvch");
                                        }
                                        else
                                        {
                                            this.sp.WriteLine("SELFTEST 4\r\n");
                                            CommonFun.WriteSendLine("last step ok,SELFTEST 4");
                                        }
                                        ++this.step;
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 3\r\n");
                                        CommonFun.WriteSendLine("Unknown command,SELFTEST 3 again");
                                        this.step = 4;
                                        break;
                                    }
                                    if (text.Contains("*A# 3 -1"))
                                    {
                                        if (!this.lbllgpdj.Dispatcher.CheckAccess())
                                            this.lbllgpdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        if (Dispatcher.Invoke(() => !this.lblxfdj.IsEnabled))
                                        {
                                            ++this.step;
                                            if (!this.lblxfdj.Dispatcher.CheckAccess())
                                                this.lblxfdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                            else
                                                this.SetStatus(this.step, true);
                                            this.sp.WriteLine("detcuvch \r\n");
                                            CommonFun.WriteSendLine("last step ok,detcuvch");
                                        }
                                        else
                                        {
                                            this.sp.WriteLine("SELFTEST 4\r\n");
                                            CommonFun.WriteSendLine("last step ok,SELFTEST 4");
                                        }
                                        ++this.step;
                                        break;
                                    }
                                    break;

                                case 5:
                                    if (text.Contains("*A# 4 1"))
                                    {
                                        if (!this.lblxfdj.Dispatcher.CheckAccess())
                                            this.lblxfdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        this.sp.WriteLine("detcuvch \r\n");
                                        CommonFun.WriteSendLine("last step ok,detcuvch");
                                        ++this.step;
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 4\r\n");
                                        CommonFun.WriteSendLine("Unknown command,SELFTEST 4 again");
                                        this.step = 5;
                                        break;
                                    }
                                    if (text.Contains("*A# 4 -1"))
                                    {
                                        if (!this.lblxfdj.Dispatcher.CheckAccess())
                                            this.lblxfdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        this.sp.WriteLine("detcuvch \r\n");
                                        CommonFun.WriteSendLine("last step ok,detcuvch");
                                        ++this.step;
                                        break;
                                    }
                                    break;
                                case 6:
                                    if (text.Contains("*A# 2"))
                                    {
                                        if (!this.lblypc.Dispatcher.CheckAccess())
                                        {
                                            if (!this.lblypc.Dispatcher.CheckAccess())
                                                this.lblypc.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                            else
                                                this.SetStatus(this.step, true);
                                            this.sp.WriteLine("SELFTEST 5\r\n");
                                            CommonFun.WriteSendLine("last step fail,SELFTEST 5");
                                            ++this.step;
                                            break;
                                        }
                                        break;
                                    }
                                    if (!this.lblypc.Dispatcher.CheckAccess())
                                        this.lblypc.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                    else
                                        this.SetStatus(this.step, false);
                                    ++this.step;
                                    if (!this.lblypc.Dispatcher.CheckAccess())
                                        this.lblypc.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                    else
                                        this.SetStatus(this.step, true);
                                    this.sp.WriteLine("SELFTEST 1\r\n");
                                    CommonFun.WriteSendLine("last step fail,SELFTEST 1");
                                    ++this.step;
                                    break;
                                case 7:
                                    if (text.Contains("*A# 5 1"))
                                    {
                                        if (!this.lblypc.Dispatcher.CheckAccess())
                                            this.lblypc.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        this.sp.WriteLine("SELFTEST 1\r\n");
                                        CommonFun.WriteSendLine("SELFTEST 1");
                                        ++this.step;
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 5\r\n");
                                        CommonFun.WriteSendLine("Unknown command,SELFTEST 5 again");
                                        this.step = 7;
                                        break;
                                    }
                                    if (text.Contains("*A# 5 -1"))
                                    {
                                        if (!this.lblypc.Dispatcher.CheckAccess())
                                            this.lblypc.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        this.sp.WriteLine("SELFTEST 1\r\n");
                                        CommonFun.WriteSendLine("last step fail,SELFTEST 1");
                                        ++this.step;
                                        break;
                                    }
                                    break;
                                case 8:
                                    if (text.Contains("*A# 1 1"))
                                    {
                                        if (!this.lblbcdj.Dispatcher.CheckAccess())
                                            this.lblbcdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        this.sp.WriteLine("SELFTEST 8\r\n");
                                        CommonFun.WriteSendLine("last step ok,SELFTEST 8");
                                        ++this.step;
                                        if (!this.lbladl.Dispatcher.CheckAccess())
                                        {
                                            this.lblbcdj.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                            break;
                                        }
                                        this.SetChecking(this.step);
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 1\r\n");
                                        CommonFun.WriteSendLine("Unknown command,SELFTEST 1 again");
                                        this.step = 8;
                                        break;
                                    }
                                    if (text.Contains("*A# 1 -1"))
                                    {
                                        if (!this.lblbcdj.Dispatcher.CheckAccess())
                                            this.lblbcdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        this.sp.WriteLine("SELFTEST 8\r\n");
                                        CommonFun.WriteSendLine("last step fail,SELFTEST 8");
                                        ++this.step;
                                        if (this.lbladl.Dispatcher.CheckAccess())
                                            this.lblbcdj.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                        else
                                            this.SetChecking(this.step);
                                        break;
                                    }
                                    break;
                                case 9:
                                    if (text.Contains("*A# 8 1"))
                                    {
                                        if (!this.lbladl.Dispatcher.CheckAccess())
                                            this.lblbcdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        this.sp.WriteLine("SELFTEST 6\r\n");
                                        CommonFun.WriteSendLine("last step ok,SELFTEST 6");
                                        ++this.step;
                                        if (this.lblbcjz.Dispatcher.CheckAccess())
                                        {
                                            this.lblbcjz.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                            break;
                                        }
                                        this.SetChecking(this.step);
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 8\r\n");
                                        CommonFun.WriteSendLine("Unknown command,SELFTEST 8 again");
                                        this.step = 9;
                                        break;
                                    }
                                    if (text.Contains("*A# 8 -1"))
                                    {
                                        if (!this.lbladl.Dispatcher.CheckAccess())
                                            this.lblbcdj.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        this.sp.WriteLine("SELFTEST 6\r\n");
                                        CommonFun.WriteSendLine("last step fail,SELFTEST 6");
                                        ++this.step;
                                        if (!this.lblbcjz.Dispatcher.CheckAccess())
                                            this.lblbcjz.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                        else
                                            this.SetChecking(this.step);
                                        break;
                                    }
                                    break;
                                case 10:
                                    if (text.Contains("*A# 6 1"))
                                    {
                                        if (!this.lblbcjz.Dispatcher.CheckAccess())
                                            this.lblbcjz.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        this.sp.WriteLine("SELFTEST 9\r\n");
                                        CommonFun.WriteSendLine("last step ok,SELFTEST 9");
                                        ++this.step;
                                        if (!this.lblpower.Dispatcher.CheckAccess())
                                        {
                                            this.lblpower.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                            break;
                                        }
                                        this.SetChecking(this.step);
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 6\r\n");
                                        CommonFun.WriteSendLine("Unknown command,SELFTEST 6 again");
                                        this.step = 10;
                                        break;
                                    }
                                    if (text.Contains("*A# 6 -1") || text.Contains("*A# 6 0"))
                                    {
                                        if (!this.lblbcjz.Dispatcher.CheckAccess())
                                            this.lblbcjz.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        this.sp.WriteLine("SELFTEST 9\r\n");
                                        CommonFun.WriteSendLine("last step fail,SELFTEST 9");
                                        ++this.step;
                                        if (!this.lblpower.Dispatcher.CheckAccess())
                                            this.lblpower.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                        else
                                            this.SetChecking(this.step);
                                        break;
                                    }
                                    break;
                                case 11:
                                    if (text.Contains("*A# 9 1"))
                                    {
                                        if (!this.lblpower.Dispatcher.CheckAccess())
                                            this.lblpower.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        ++this.step;
                                        if (!this.btnPass.Dispatcher.CheckAccess())
                                            this.btnPass.Dispatcher.Invoke((Delegate) new Del_PassVisiable(this.Passvisi));
                                        else {
                                            Dispatcher.Invoke(() => this.btnPass.IsEnabled = true);
                                            Dispatcher.Invoke(() => this.btnPass.Visibility = Visibility.Visible);
                                            }
                                        if (CommonFun.GetAppConfig("AutoCalSY") == "true")
                                        {
                                            this.sp.WriteLine("SELFTEST 7\r\n");
                                            CommonFun.WriteSendLine("last step ok,autocalsy,SELFTEST 7");
                                            if (!this.lblsysbase.Dispatcher.CheckAccess())
                                            {
                                                this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                                break;
                                            }
                                            this.SetChecking(this.step);
                                            break;
                                        }
                                        if (!this.lblsysbase.Dispatcher.CheckAccess())
                                            this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        ++this.step;
                                        if (!this.lblwarmup.Dispatcher.CheckAccess())
                                            this.lblwarmup.Dispatcher.Invoke((Delegate) new Del_Starttimer(this.Starttimer));
                                        else
                                            this.Starttimer();
                                        if (!this.lblwarmup.Dispatcher.CheckAccess())
                                            this.lblwarmup.Dispatcher.Invoke((Delegate)new Del_SetChecking(this.SetChecking), (object)this.step);
                                        else
                                            this.SetChecking(this.step);
                                        break;
                                    }
                                    if (text.Contains("Unknown command"))
                                    {
                                        this.sp.WriteLine("SELFTEST 9\r\n");
                                        CommonFun.WriteSendLine("Unknown command,SELFTEST9 again");
                                        this.step = 11;
                                        break;
                                    }
                                    if (text.Contains("*A# 9 -1"))
                                    {
                                        if (!this.lblpower.Dispatcher.CheckAccess())
                                            this.lblpower.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        ++this.step;
                                        if (!this.btnPass.Dispatcher.CheckAccess())
                                            this.btnPass.Dispatcher.Invoke((Delegate) new Del_PassVisiable(this.Passvisi));
                                        else
                                        {
                                            Dispatcher.Invoke(() => this.btnPass.IsEnabled = true);
                                            Dispatcher.Invoke(() => this.btnPass.Visibility = Visibility.Visible);
                                        }
                                            if (CommonFun.GetAppConfig("AutoCalSY") == "true")
                                        {
                                            this.sp.WriteLine("SELFTEST 7\r\n");
                                            CommonFun.WriteSendLine("last step fail,autocalsy,SELFTEST 7");
                                            if (!this.lblsysbase.Dispatcher.CheckAccess())
                                                this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                            else
                                                this.SetChecking(this.step);
                                        }
                                        else
                                        {
                                            if (!this.lblsysbase.Dispatcher.CheckAccess())
                                                this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                            else
                                                this.SetStatus(this.step, true);
                                            ++this.step;
                                            if (!this.lblwarmup.Dispatcher.CheckAccess())
                                                this.lblwarmup.Dispatcher.Invoke((Delegate) new Del_Starttimer(this.Starttimer));
                                            else
                                                this.Starttimer();
                                            if (!this.lblwarmup.Dispatcher.CheckAccess())
                                                this.lblwarmup.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                            else
                                                this.SetChecking(this.step);
                                        }
                                        break;
                                    }
                                    break;
                                case 12:
                                    if (text.Contains("*A# 7 1"))
                                    {
                                        if (!this.lblsysbase.Dispatcher.CheckAccess())
                                            this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)true);
                                        else
                                            this.SetStatus(this.step, true);
                                        if (!this.lblwarmup.Dispatcher.CheckAccess())
                                            this.lblwarmup.Dispatcher.Invoke((Delegate) new Del_Starttimer(this.Starttimer));
                                        else
                                            this.Starttimer();
                                        ++this.step;
                                        if (!this.lblwarmup.Dispatcher.CheckAccess())
                                        {
                                            this.lblwarmup.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                            break;
                                        }
                                        this.SetChecking(this.step);
                                        break;
                                    }
                                    if (text.Contains("*A# 7 -1"))
                                    {
                                        if (!this.lblsysbase.Dispatcher.CheckAccess())
                                            this.lblsysbase.Dispatcher.Invoke((Delegate) new Del_SetStatus(this.SetStatus), (object)this.step, (object)false);
                                        else
                                            this.SetStatus(this.step, false);
                                        if (!this.lblwarmup.Dispatcher.CheckAccess())
                                            this.lblwarmup.Dispatcher.Invoke((Delegate) new Del_Starttimer(this.Starttimer));
                                        else
                                            this.Starttimer();
                                        ++this.step;
                                        if (!this.lblwarmup.Dispatcher.CheckAccess())
                                            this.lblwarmup.Dispatcher.Invoke((Delegate) new Del_SetChecking(this.SetChecking), (object)this.step);
                                        else
                                            this.SetChecking(this.step);
                                        break;
                                    }
                                    break;


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }

        }
        private void SetChecking(int stp)
        {
            switch (stp)
            {
                case 9:
                    lbladl.Dispatcher.Invoke(() =>
                    {
                        this.lbladl.Content = "Проверка";
                    });
                    break;
                case 10:
                    lblbcjz.Dispatcher.Invoke(() =>
                    {
                        this.lblbcjz.Content = "Проверка";
                    });
                    break;
                case 12:
                    lblsysbase.Dispatcher.Invoke(() =>
                    {
                        this.lblsysbase.Content = "Проверка";
                    });
                    break;
                case 13:
                    lblwarmup.Dispatcher.Invoke(() =>
                    {
                        this.lblwarmup.Content = "";
                    });
                    break;
            }
        }

        private void Passvisi()
        {
            if (step >= 12){
                Dispatcher.Invoke(() => this.btnPass.IsEnabled = true);
                Dispatcher.Invoke(() => this.btnPass.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => btnRetry.Visibility = Visibility.Visible);
                if (this.errorlist.Count > 0)
                {
                    Dispatcher.Invoke(() => this.btnRetry.IsEnabled = true);
                    Dispatcher.Invoke(() => this.btnRetry.Visibility = Visibility.Visible);
                    //Dispatcher.Invoke(() => this.btnPass.Visibility = Visibility.Collapsed);
                }
                else
                {
                    Dispatcher.Invoke(() => this.btnRetry.IsEnabled = false);
                    Dispatcher.Invoke(() => this.btnRetry.Visibility = Visibility.Collapsed);
                    Dispatcher.Invoke(() => this.btnPass.Visibility = Visibility.Visible);
                }
            }
        }

        private void Starttimer()
        {
            if (this.totalcnt == 0)
                this.totalcnt = 15 * 60;
            this.timecnt = 0;

            timer1.Tick += timer1_Tick;
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ++this.timecnt;
            int num = this.totalcnt - this.timecnt;
            if (num < 60)
                Dispatcher.Invoke(() =>this.lblwarmup.Content = "Прогрев секунд:" + " " + (object)num);
            else
                Dispatcher.Invoke(() =>this.lblwarmup.Content = "Прогрев минут:" + " " + (object)(num / 60));
            if (this.timecnt != this.totalcnt)
                return;
            this.timer1.Stop();
            Dispatcher.Invoke(() =>
                this.lblwarmup.Content = "Ок");
            Dispatcher.Invoke(() =>
                this.lblwarmup.Foreground = Brushes.Green);
            Dispatcher.Invoke(() => this.progressBar1.Value = 000);
            Dispatcher.Invoke(() => this.progressBar1.Visibility = Visibility.Collapsed);
            
            if (this.errorlist.Count <= 0)
                this.btnPass_PreviewMouseDown((object)null, (EventArgs)null);
        }

        private void SetStatus(int stp, bool sta)
        {
            switch (stp)
            {
                case 1:
                    if (sta)
                    {
                        lblboot.Dispatcher.Invoke(() =>
                        {
                            this.lblboot.Content = "Ок";
                            this.lblboot.Foreground = Brushes.Green;
                        });
                        this.tdwait.Stop();
                        this.repeatcnt = 0;
                        label2.Dispatcher.Invoke(() =>
                        {
                            this.label2.Visibility = Visibility.Visible;
                        });
                        lbldqhdj.Dispatcher.Invoke(() =>
                        {
                            this.lbldqhdj.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        //CommonFun.showbox(CommonFun.GetLanText("nonauthorize"), "Error");
                        lblboot.Dispatcher.Invoke(() =>
                        {
                            this.lblboot.Content = "Ошибка";
                            this.lblboot.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(1);
                        this.tdwait.Stop();
                        this.repeatcnt = 0;
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 8;
                    });
                    break;
                case 2:
                    if (sta)
                    {
                        CommonFun.Set("tunablebandwidth", "true");
                        break;
                    }
                    CommonFun.Set("tunablebandwidth", "false");
                    label4.Dispatcher.Invoke(() =>
                    {
                        this.label4.IsEnabled = false;
                    });
                    lblxfdj.Dispatcher.Invoke(() =>
                    {
                        this.lblxfdj.IsEnabled = false;
                        this.lblxfdj.Content = "Не изменяется";
                    });
                    break;
                case 3:
                    if (sta)
                    {
                        lbldqhdj.Dispatcher.Invoke(() =>
                        {
                            this.lbldqhdj.Content = "Ок";
                            this.lbldqhdj.Foreground = Brushes.Green;
                        });
                        label3.Dispatcher.Invoke(() =>
                        {
                            this.label3.Visibility = Visibility.Visible;
                        });
                        lblbcdj.Dispatcher.Invoke(() =>
                        {
                            this.lblbcdj.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        lbldqhdj.Dispatcher.Invoke(() =>
                        {
                            this.lbldqhdj.Content = "Ошибка";
                            this.lbldqhdj.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(3);
                        label3.Dispatcher.Invoke(() =>
                        {
                            this.label3.Visibility = Visibility.Visible;
                        });
                        lblbcdj.Dispatcher.Invoke(() =>
                        {
                            this.lblbcdj.Visibility = Visibility.Visible;
                        });
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 06;
                    });
                    break;
                case 4:
                    if (sta)
                    {
                        lblbcdj.Dispatcher.Invoke(() =>
                        {
                            this.lblbcdj.Content = "Ок";
                            this.lblbcdj.Foreground = Brushes.Green;
                        });
                        label4.Dispatcher.Invoke(() =>
                        {
                            this.label4.Visibility = Visibility.Visible;
                        });
                        lblxfdj.Dispatcher.Invoke(() =>
                        {
                            this.lblxfdj.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        lblbcdj.Dispatcher.Invoke(() =>
                        {
                            this.lblbcdj.Content = "Ошибка";
                            this.lblbcdj.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(4);
                        label4.Dispatcher.Invoke(() =>
                        {
                            this.label4.Visibility = Visibility.Visible;
                        });
                        lblxfdj.Dispatcher.Invoke(() =>
                        {
                            this.lblxfdj.Visibility = Visibility.Visible;
                        });
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 24;
                    });
                    break;
                case 5:
                    if (sta)
                    {
                        if (Dispatcher.Invoke(() => this.lblxfdj.IsEnabled))
                        {
                            lblxfdj.Dispatcher.Invoke(() =>
                            {
                                this.lblxfdj.Content = "Ок";
                                this.lblxfdj.Foreground = Brushes.Green;
                            });
                        }
                        label5.Dispatcher.Invoke(() =>
                        {
                            this.label5.Visibility = Visibility.Visible;
                        });
                        lblypc.Dispatcher.Invoke(() =>
                        {
                            this.lblypc.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        lblxfdj.Dispatcher.Invoke(() =>
                        {
                            this.lblxfdj.Content = "Ошибка";
                            this.lblxfdj.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(5);
                        label5.Dispatcher.Invoke(() =>
                        {
                            this.label5.Visibility = Visibility.Visible;
                        });
                        lblypc.Dispatcher.Invoke(() =>
                        {
                            this.lblypc.Visibility = Visibility.Visible;
                        });
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 32;
                    });
                    break;
                case 6:
                    if (sta)
                    {
                        CommonFun.Set("EightSlot", "true");
                        break;
                    }
                    CommonFun.Set("EightSlot", "false");
                    label5.Dispatcher.Invoke(() =>
                    {
                        this.label5.IsEnabled = false;
                    });
                    lblypc.Dispatcher.Invoke(() =>
                    {
                        this.lblypc.IsEnabled = false;
                        this.lblypc.Content = "Не установлен";
                    });
                    break;
                case 7:
                    if (sta)
                    {
                        if (Dispatcher.Invoke(() => this.lblypc.IsEnabled))
                        {
                            lblypc.Dispatcher.Invoke(() =>
                            {
                                this.lblypc.Content = "Ок";
                                this.lblypc.Foreground = Brushes.Green;
                            });
                        }
                        label6.Dispatcher.Invoke(() =>
                        {
                            this.label6.Visibility = Visibility.Visible;
                        });
                        lbllgpdj.Dispatcher.Invoke(() =>
                        {
                            this.lbllgpdj.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        lblypc.Dispatcher.Invoke(() =>
                        {
                            this.lblypc.Content = "Ошибка";
                            this.lblypc.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(7);
                        label6.Dispatcher.Invoke(() =>
                        {
                            this.label6.Visibility = Visibility.Visible;
                        });
                        lbllgpdj.Dispatcher.Invoke(() =>
                        {
                            this.lbllgpdj.Visibility = Visibility.Visible;
                        });
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    { 
                        this.progressBar1.Value = 40;
                    });
                    break;
                case 8:
                    if (sta)
                    {
                        lbllgpdj.Dispatcher.Invoke(() =>
                        {
                            this.lbllgpdj.Content = "Ок";
                            this.lbllgpdj.Foreground = Brushes.Green;
                        });
                        label7.Dispatcher.Invoke(() =>
                        {
                            this.label7.Visibility = Visibility.Visible;
                        });
                        lbladl.Dispatcher.Invoke(() =>
                        {
                            this.lbladl.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        lbllgpdj.Dispatcher.Invoke(() =>
                        {
                            this.lbllgpdj.Content = "Ошибка";
                            this.lbllgpdj.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(8);
                        label7.Dispatcher.Invoke(() =>
                        {
                            this.label7.Visibility = Visibility.Visible;
                        });
                        lbladl.Dispatcher.Invoke(() =>
                        {
                            this.lbladl.Visibility = Visibility.Visible;
                        });
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 48;
                    });
                    break;
                case 9:
                    if (sta)
                    {
                        lbladl.Dispatcher.Invoke(() =>
                        {
                            this.lbladl.Content = "Ок";
                            this.lbladl.Foreground = Brushes.Green;
                        });
                        label8.Dispatcher.Invoke(() =>
                        {
                            this.label8.Visibility = Visibility.Visible;
                        });
                        lblbcjz.Dispatcher.Invoke(() =>
                        {
                            this.lblbcjz.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        lbladl.Dispatcher.Invoke(() =>
                        {
                            this.lbladl.Content = "Ошибка";
                            this.lbladl.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(9);
                        label8.Dispatcher.Invoke(() =>
                        {
                            this.label8.Visibility = Visibility.Visible;
                        });
                        lblbcjz.Dispatcher.Invoke(() =>
                        {
                            this.lblbcjz.Visibility = Visibility.Visible;
                        });
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 56;
                    });
                    break;
                case 10:
                    if (sta)
                    {
                        lblbcjz.Dispatcher.Invoke(() =>
                        {
                            this.lblbcjz.Content = "Ок";
                            this.lblbcjz.Foreground = Brushes.Green;
                        });
                        label9.Dispatcher.Invoke(() =>
                        {
                            this.label9.Visibility = Visibility.Visible;
                        });
                        lblpower.Dispatcher.Invoke(() =>
                        {
                            this.lblpower.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        lblbcjz.Dispatcher.Invoke(() =>
                        {
                            this.lblbcjz.Content = "Ошибка";
                            this.lblbcjz.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(10);
                        label9.Dispatcher.Invoke(() =>
                        {
                            this.label9.Visibility = Visibility.Visible;
                        });
                        lblpower.Dispatcher.Invoke(() =>
                        {
                            this.lblpower.Visibility = Visibility.Visible;
                        });
                    }
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 66;
                    });
                    break;
                case 11:
                    if (sta)
                    {
                        lblpower.Dispatcher.Invoke(() =>
                        {
                            this.lblpower.Content = "Ок";
                            this.lblpower.Foreground = Brushes.Green;
                        });
                    }
                    else
                    {
                        lblpower.Dispatcher.Invoke(() =>
                        {
                            this.lblpower.Content = "Ошибка";
                            this.lblpower.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(11);
                    }
                    label10.Dispatcher.Invoke(() =>
                    {
                        this.label10.Visibility = Visibility.Visible;
                    });
                    lblsysbase.Dispatcher.Invoke(() =>
                    {
                        this.lblsysbase.Visibility = Visibility.Visible;
                    });
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 74;
                    });
                    break;
                case 12:
                    if (sta)
                    {
                        lblsysbase.Dispatcher.Invoke(() =>
                        {
                            this.lblsysbase.Content = "Ок";
                            this.lblsysbase.Foreground = Brushes.Green;
                        });
                    }
                    else
                    {
                        lblsysbase.Dispatcher.Invoke(() =>
                        {
                            this.lblsysbase.Content = "Ошибка";
                            this.lblsysbase.Foreground = Brushes.Red;
                        });
                        this.errorlist.Add(12);
                    }
                    label11.Dispatcher.Invoke(() =>
                    {
                        this.label11.Visibility = Visibility.Visible;
                    });
                    lblwarmup.Dispatcher.Invoke(() =>
                    {
                        this.lblwarmup.Visibility = Visibility.Visible;
                    });
                    progressBar1.Dispatcher.Invoke(() =>
                    {
                        this.progressBar1.Value = 90;
                    });
                    break;
                case 13:
                    if (sta)
                    {
                        lblwarmup.Dispatcher.Invoke(() =>
                        {
                            this.lblwarmup.Content = "Ок";
                        });
                        break;
                    }
                    lblwarmup.Dispatcher.Invoke(() =>
                    {
                        this.lblwarmup.Content = "Ошибка";
                    });
                    break;
            }
        }



        private void btnPass_PreviewMouseDown(object sender, EventArgs e)
        {
            /*Hide();
            MenuProgram menuProgram = new MenuProgram();
            menuProgram.Show();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();*/
            btnPass.Background = null;
       
            this.timer1.Stop();
            if (this.step == 10 && CommonFun.GetAppConfig("AutoCalSY") == "true")
                CommonFun.WriteSendLine("pass btuton,");

            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();

            if (sp.IsOpen)
            {
                //sp.WriteLine("DISCONN \r\n");
                CommonFun.WriteSendLine("gueset,login,");
                sp.Close();
            }

            Hide();
            MenuProgram menuProgram = new MenuProgram();
            menuProgram.Show();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (this.sp.IsOpen)
                    this.myque.Enqueue((object)this.sp.ReadLine());
              //  else
               // CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
            }
            catch
            {
            }
        }

        private void SetBandWidth(string msg)
        {
            string[] strArray = msg.Split(' ');
            if (msg.Contains("U61T"))
            {
                CommonFun.Set("Spectralbandwidth", "1.8");
                CommonFun.Set("modelnumber", "P7");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "U61T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "U61T");
            }
            else if (msg.Contains("U63T"))
            {
                CommonFun.Set("Spectralbandwidth", "1");
                CommonFun.Set("modelnumber", "P8");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "U63T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "U63T");
            }
            else if (msg.Contains("U32T"))
            {
                CommonFun.Set("Spectralbandwidth", "1.8");
                CommonFun.Set("modelnumber", "P5");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "U32T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "U32T");
            }
            else if (msg.Contains("U33T"))
            {
                CommonFun.Set("Spectralbandwidth", "1");
                CommonFun.Set("modelnumber", "P6");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "U33T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "U33T");
            }
            else if (msg.Contains("U65T"))
            {
                CommonFun.Set("modelnumber", "P9");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "U65T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "U65T");
            }
            else if (msg.Contains("X61T"))
            {
                CommonFun.Set("Spectralbandwidth", "1.8");
                CommonFun.Set("modelnumber", "M7");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "X61T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "X61T");
            }
            else if (msg.Contains("X63T"))
            {
                CommonFun.Set("Spectralbandwidth", "1");
                CommonFun.Set("modelnumber", "M8");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "X63T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "X63T");
            }
            else if (msg.Contains("X32T"))
            {
                CommonFun.Set("Spectralbandwidth", "1.8");
                CommonFun.Set("modelnumber", "M5");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "X32T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "X32T");
            }
            else if (msg.Contains("X33T"))
            {
                CommonFun.Set("Spectralbandwidth", "1");
                CommonFun.Set("modelnumber", "M6");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "X33T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "X33T");
            }
            else if (msg.Contains("X65T"))
            {
                CommonFun.Set("modelnumber", "M9");
                if (((IEnumerable<string>)strArray).Count<string>() == 4)
                    CommonFun.Set("version", "X65T," + strArray[2] + "," + strArray[3]);
                else
                    CommonFun.Set("version", "X65T");
            }
            else
                CommonFun.Set("version", msg.Replace("/n", "").Replace("/r", "").Replace("*A# ", ""));
        }

        private delegate void Del_setconfig(string res);

        private delegate void Del_SetStatus(int stp, bool sta);

        private delegate void Del_PassVisiable();

        private delegate void Del_SetChecking(int stp);

        private delegate void Del_Starttimer();

        private delegate void Del_WriteLog(string msg);

        private delegate void Del_SetBandWidth(string msg);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.timer1.Stop();
            if (this.step == 10 && CommonFun.GetAppConfig("AutoCalSY") == "true")
                CommonFun.WriteSendLine("pass btuton,");

            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();

            if (sp.IsOpen)
            {
                sp.WriteLine("DISCONN \r\n");
                CommonFun.WriteSendLine("DISCONN");
                sp.Close();
            }
        }

        private void pibpoweroff_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.timer1.Stop();
            
            this.runptag = false;
            if (this.dealth != null)
                this.dealth.Abort();
            if (this.tdstart != null)
                this.tdstart.Abort();

            if (sp.IsOpen)
            {
                CommonFun.WriteSendLine("close,");
                sp.WriteLine("DISCONN \r\n");
                CommonFun.WriteSendLine("DISCONN");
                sp.Close();
            }
            this.Close();
        }

    }
}
