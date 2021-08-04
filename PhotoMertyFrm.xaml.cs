using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Xps;
using Binding = System.Windows.Data.Binding;
using CheckBox = System.Windows.Controls.CheckBox;
using DataGrid = System.Windows.Controls.DataGrid;
using DataGridCell = System.Windows.Controls.DataGridCell;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using TextBox = System.Windows.Controls.TextBox;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing.Printing;
using PrintDialog = System.Windows.Controls.PrintDialog;
using System.Windows.Xps.Packaging;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для PhotoMertyFrm.xaml
    /// </summary>
    public partial class PhotoMertyFrm : Window
    {
        private SerialPort sp = new SerialPort();
        private MulMethod MM;
        private ComStatus ComSta;
        private System.Timers.Timer tdwait = new System.Timers.Timer(15000.0);
        private Thread tdstart;
        private bool runtag = true;
        private Task td;
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken ct;
        private bool automode = false;
        private Queue myque = new Queue();
        private string currslot = "";
        private int calormea = 1;
        private string receive = "";
        private string cmdque = "";

        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");

        private int repeatcnt = 0;

        public bool nonPort; //Порт включен(выключен)
        public string portsName; //Имя порта

       // public ObservableCollection<MyTable> MySource { get; set; }

        bool print_in_tabel = false;

        DispatcherTimer timer1 = new DispatcherTimer();
        public PhotoMertyFrm()
        {
            try
            {
                InitializeComponent();
                /*var cc = meisureGrid.CurrentCell;
                foreach (var col in meisureGrid.Columns)
                {
                    meisureGrid.CurrentCell = new DataGridCellInfo(meisureGrid.CurrentItem, col);
                    meisureGrid.CancelEdit();
                }*/
                
                timer1.Interval = TimeSpan.FromMilliseconds(1000);
                timer1.Tick += new EventHandler(this.timer1_Tick);

                StartProgram();
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        public void StartProgram()
        {
            try
            {
                if (this.MM == null)
                {
                    this.MM = new MulMethod();
                    try
                    {
                        this.MM.C_mode = CommonFun.getXmlValue("PhoMethod", "C_mode");
                        this.MM.WL = CommonFun.getXmlValue("PhoMethod", "WL");
                        if (this.MM.WL == "")
                            this.MM.WL = "546.0";
                        this.MM.R = CommonFun.getXmlValue("PhoMethod", "R");
                    }
                    catch (Exception ex)
                    {
                        this.MM.C_mode = "Abs";
                        this.MM.WL = "546.0";
                        this.MM.R = "1";
                    }
                    this.lblWL.Content = Convert.ToDecimal(this.MM.WL, CultureInfo.InvariantCulture).ToString("f1") + " нм";
                   /* if (this.MM.C_mode == "Abs")
                    {
                        this.lblmode.Content = "Abs";
                        this.lblUnit.Content = "Abs";
                    }
                    else
                    {
                        this.lblmode.Content = "%T";
                        this.lblUnit.Content = "%T";
                    }*/
                }
                this.lblWL.Content = Convert.ToDecimal(this.MM.WL, CultureInfo.InvariantCulture).ToString("f1") + " нм";
                this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
                this.tdstart.Start();
                //this.setstate(false);
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }


            // CommonFun.showbox(ports.Count<string>().ToString());




        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string errormsg = "";
            if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
            {
                CommonFun.showbox(errormsg, "Error");
                this.timer1.Stop();
            }
            else if (!this.sp.IsOpen)
            {
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                this.timer1.Stop();
            }
            else if (this.ComSta != ComStatus.END)
            {
                CommonFun.WriteSendLine("timer1_tick,ComSta != ComStatus.END,return");
            }
            else
            {
                string wl = this.MM.WL;
                this.ComSta = ComStatus.MEASURE;
                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                CommonFun.WriteSendLine("timer，measure 1 2 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
            }
        }

        private void tdstart_Elapsed()
        {

            this.cancelTokenSource = new CancellationTokenSource();
            this.ct = this.cancelTokenSource.Token;
            this.td = new Task(new Action(this.DealRecData), this.ct);
            td.Start();

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
               // CommonFun.WriteSendLine("timer1_tick,ComSta != ComStatus.END,return");
                /*if (!this.automode)
                {
                    this.ComSta = ComStatus.BD_RATIO_FLUSH;
                    this.sp.WriteLine("BD_RATIO_FLUSH \r\n");
                    CommonFun.WriteSendLine("BD_RATIO_FLUSH");
                }
                else*/
                this.ComSta = ComStatus.END;

                //this.timer1.Interval = (int)Convert.ToInt16(Convert.ToDecimal(CommonFun.GetAppConfig("PhotoMetryInterval")) * 1000M);

                this.timer1.Interval = TimeSpan.FromMilliseconds(Convert.ToInt16(Convert.ToDecimal(CommonFun.GetAppConfig("PhotoMetryInterval")) * 1000M));

                this.tdwait.Elapsed += new ElapsedEventHandler(this.tdwait_Elapsed);
                if (!this.btnBlank.Dispatcher.CheckAccess())
                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_BlanckPreviewMouseDown(this.BlankPreviewMouseDown));
                else
                    this.Zero_PreviewMouseDown((object)null, (RoutedEventArgs)null);
            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.Message.ToString() + "tdstart_Elapsed", "Error");

            }

            //if (this.btnBlank.Dispatcher.CheckAccess())
            //    this.btnBlank.Dispatcher.Invoke((Delegate) new Del_setstate(this.setstate), (object)true);
          // else
             //   this.setstate(true);



        }

        private void tdwait_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.ComSta != ComStatus.END)
            {
                if (this.ComSta != ComStatus.CALBGND)
                    return;
                if (this.repeatcnt < 3)
                {
                    if (!this.sp.IsOpen)
                    {
                        CommonFun.GetLanText("opencom Warning");
                    }
                    else
                    {
                        this.ComSta = ComStatus.END;
                        CommonFun.WriteSendLine("tdwait,");
                        string wl = this.MM.WL;
                        this.ComSta = ComStatus.CALBGND;
                        this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                        CommonFun.WriteSendLine("tdwait,calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
                        ++this.repeatcnt;
                    }
                }
                else
                {
                    CommonFun.showbox(CommonFun.GetLanText("noresponse"), "Error");
                    this.ComSta = ComStatus.END;
                    if (!this.btnBlank.Dispatcher.CheckAccess())
                        this.btnBlank.Dispatcher.Invoke((Delegate) new Del_SetBlankLable(this.Setblanklable), (object)false);
                    else
                        this.Setblanklable(false);
                }
            }
            else
            {
                this.tdwait.Stop();
                this.repeatcnt = 0;
            }
        }

        private void Setblanklable(bool value)
        {
            //this.btnBlank.Content = CommonFun.GetLanText( "Обнуление");
            //this.btnBlank.Enabled = true;
           // lblProgress.Content = CommonFun.GetLanText("blanking");
            this.progressBar1.Value = 000;
            //this.panel4.Visible = false;
            this.progressBar1.Value = 0;
            this.tdwait.Stop();
            this.repeatcnt = 0;
            if (value)
            {
                this.timer1.Start();
                this.timer1_Tick((object)null, (EventArgs)null);
            }
            else
                this.timer1.Stop();
        }

        private void BlankPreviewMouseDown() => this.Zero_PreviewMouseDown((object)null, (RoutedEventArgs)null);

        private void Zero_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

            string errormsg = "";
            if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
            {
                CommonFun.showbox(errormsg, "Error");
            }
            else
            {
                this.timer1.Stop();
                if (!this.sp.IsOpen)
                    CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                if (this.ComSta != ComStatus.END)
                {
                    this.cmdque = "calbgnd 1 1 " + (Convert.ToDecimal(this.MM.WL) * 10M).ToString("f0") + "\r\n";
                    //this.btnBlank.Text = CommonFun.GetLanText(this.lanvalue, "inblanking");
                    //this.panel4.Visible = true;
                    this.progressBar1.Value = 30;
                }
                else
                {
                    //this.btnBlank.Text = CommonFun.GetLanText(this.lanvalue, "inblanking");
                    // this.panel4.Visible = true;
                    this.progressBar1.Value = 30;
                    // this.btnBlank.Enabled = false;
                    if (CommonFun.GetAppConfig("EightSlot") == "true")
                    {
                        this.calormea = 1;
                        if (this.currslot.Length > 0)
                        {
                            string wl = this.MM.WL;
                            this.ComSta = ComStatus.CALBGND;
                            this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                           // CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
                        }
                        else
                        {
                            this.ComSta = ComStatus.SYSTATE;
                            this.sp.WriteLine("systate \r\n");
                       //     CommonFun.WriteSendLine("systate");
                        }
                    }
                    else
                    {
                        string wl = this.MM.WL;
                        this.ComSta = ComStatus.CALBGND;
                        this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                //        CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
                    }
                    this.tdwait.Start();
                }
            }
            btnBlank.Background = null;
        }

        private void StopTimer() => this.timer1.Stop();

        private void StartTimer() => this.timer1.Start();

        private void DealRecData()
        {

            while (this.runtag)
            {
                if (this.ct.IsCancellationRequested)
                {
                    try
                    {
                        this.ct.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException ex)
                    {
                        break;
                    }
                }
                else if (this.myque.Count > 0)
                {
                    string text = this.myque.Dequeue().ToString();
                    try
                    {
                        switch (this.ComSta)
                        {
                            case ComStatus.SYSTATE:
                                CommonFun.WriteLine(text);
                                string[] strArray1 = text.Split(' ');
                                if (((IEnumerable<string>)strArray1).Count<string>() > 4)
                                {
                                    this.currslot = strArray1[4];
                                    //if (!this.button1.Dispatcher.CheckAccess())
                                   ///     this.button1.Dispatcher.Invoke((Delegate)new Del_ShowSlot(this.ShowSlot));
                                    //this.ShowSlot();
                                    this.ComSta = ComStatus.SETCHP;
                                    this.sp.WriteLine("SETCHP " + this.currslot + "\r\n");
                                    CommonFun.WriteSendLine("SETCHP " + this.currslot);
                                    break;
                                }
                                break;
                            case ComStatus.SETCHP:
                                CommonFun.WriteLine(text);
                                try
                                {
                                    if (text.Contains("*A#"))
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (this.calormea == 1)
                                        {
                                            string wl = this.MM.WL;
                                            this.ComSta = ComStatus.CALBGND;
                                            this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                                            CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
                                        }
                                        else if (!this.btnBlank.Dispatcher.CheckAccess())
                                            this.btnBlank.Dispatcher.Invoke((Delegate)new Del_TimerStart(this.StartTimer));
                                        else
                                            this.timer1.Start();
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.GetLanText("errorretry" + ex.ToString());
                                    this.ComSta = ComStatus.END;
                                    this.calormea = 0;
                                    break;
                                }
                            case ComStatus.MEASURE:
                                CommonFun.WriteLine(text);
                                this.receive += text;
                                try
                                {
                                    if (text.Contains("END"))
                                    {
                                        if (!this.receive.Contains("*CALDAT"))
                                        {
                                            int startIndex = this.receive.IndexOf("*DAT") + 5;
                                            int num1 = this.receive.IndexOf("END");
                                            if (startIndex < this.receive.Length && num1 > 0 && num1 > startIndex)
                                            {
                                                this.receive = this.receive.Substring(startIndex, num1 - startIndex);
                                                string[] strArray2 = this.receive.Split(' ');
                                                Decimal num2 = Convert.ToDecimal(this.MM.R);
                                                Decimal a1;
                                                Decimal a2;
                                                if (Convert.ToDecimal(strArray2[0]) <= 0M)
                                                {
                                                    if (this.MM.C_mode == "Abs")
                                                    {
                                                        a1 = 20M;
                                                        //a2 = num2 * a1;

                                                        a1 = num2 * a1;
                                                        a2 = Convert.ToDecimal(strArray2[0]);
                                                    }
                                                    else if (Convert.ToDouble(strArray2[0]) < -0.05)
                                                    {
                                                        a1 = 99999.99M;
                                                        a2 = 99999.99M;
                                                    }
                                                    else
                                                    {
                                                        //a1 = Convert.ToDecimal(strArray2[0]);
                                                        // a2 = a1;

                                                        a1 = 20M;
                                                        a1 = num2 * a1;
                                                        a2 = Convert.ToDecimal(strArray2[0]);
                                                    }
                                                }
                                                else if (this.MM.C_mode == "Abs")
                                                {
                                                    a1 = Convert.ToDecimal(2.0 - Math.Log10(Convert.ToDouble(strArray2[0])));
                                                    //a2 = num2 * a1;

                                                    // a1 = 20M;
                                                    a1 = num2 * a1;
                                                    a2 = Convert.ToDecimal(strArray2[0]);
                                                }
                                                else
                                                {
                                                    //a1 = Convert.ToDecimal(strArray2[0]);
                                                    //a2 = a1;

                                                    a1 = Convert.ToDecimal(2.0 - Math.Log10(Convert.ToDouble(strArray2[0])));
                                                    a1 = num2 * a1;
                                                    a2 = Convert.ToDecimal(strArray2[0]);
                                                }
                                                if (!this.lblValueT.Dispatcher.CheckAccess() && !this.lblValueA.Dispatcher.CheckAccess())
                                                {
                                                    this.lblValueT.Dispatcher.Invoke((Delegate)new Del_ShowValue(this.ShowValue), (object)a1, (object)a2, (object)true);
                                                    this.lblValueA.Dispatcher.Invoke((Delegate)new Del_ShowValue(this.ShowValue), (object)a1, (object)a2, (object)true);
                                                }
                                                else
                                                    this.ShowValue(a1, a2, true);
                                            }
                                            this.receive = "";
                                            this.ComSta = ComStatus.END;
                                            if (this.cmdque.Length > 0)
                                            {
                                                if (!this.btnBlank.Dispatcher.CheckAccess())
                                                    this.btnBlank.Dispatcher.Invoke((Delegate)new Del_ExcuteQueCmd(this.Excutecmdque));
                                                else
                                                    this.Excutecmdque();
                                                this.cmdque = "";
                                            }
                                        }
                                        else
                                            this.receive = "";
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.GetLanText("errorstopmeasure" + "," + ex.ToString());
                                    CommonFun.WriteSendLine("error，");
                                    if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Del_TimerStop(this.StopTimer));
                                    else
                                        this.timer1.Stop();
                                    this.ComSta = ComStatus.END;
                                    this.receive = "";
                                    break;
                                }
                            case ComStatus.CALBGND:
                                CommonFun.WriteLine(text);
                                try
                                {
                                    if (text.Contains("END"))
                                    {
                                        this.ComSta = ComStatus.END;
                                        if (!this.btnBlank.Dispatcher.CheckAccess())
                                            this.btnBlank.Dispatcher.Invoke((Delegate) new Del_SetBlankLable(this.Setblanklable), (object)true);
                                        else
                                            this.Setblanklable(true);
                                        break;
                                    }
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    CommonFun.GetLanText("errorstopblank" + "," + ex.ToString());
                                    CommonFun.WriteSendLine("error，");
                                    if (!this.btnBlank.Dispatcher.CheckAccess())
                                        this.btnBlank.Dispatcher.Invoke((Delegate) new Del_SetBlankLable(this.Setblanklable), (object)false);
                                    else
                                        this.Setblanklable(false);
                                    this.receive = "";
                                    this.ComSta = ComStatus.END;
                                    break;
                                }
                            case ComStatus.BD_RATIO_FLUSH:
                                this.ComSta = ComStatus.END;
                                if (!this.btnBlank.Dispatcher.CheckAccess())
                                {
                                   // this.btnBlank.Dispatcher.Invoke((Delegate)new Photometry.Del_setstate(this.setstate), (object)true);
                                    break;
                                }
                                //this.setstate(true);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonFun.showbox(ex.Message.ToString(), "Error");

                    }
                }
            }
        }

        private void ShowValue(Decimal a1, Decimal a2, bool value)
        {
            if (value)
            {
                if (a1 == 99999.99M)
                {
                    this.lblValueA.Content = "---";
                    this.lblValueT.Content = "---";
                    //this.lblAvalue.Content = "----";
                    // this.lblKvalue.Text = this.MM.R;
                }
                this.lblValueA.Content = a1.ToString(this.absacc);
                
                    //this.lblAvalue.Text = a1.ToString(this.absacc) + " A";
                    //  this.lblKvalue.Text = this.MM.R;
               
                this.lblValueT.Content = a2.ToString(this.tacc);

                // this.lblAvalue.Text = a1.ToString(this.tacc) + " %T";
                if (print_in_tabel == true)
                {
                    MyTableList.Add(new MyTable("Образец " + (meisureGrid.Items.Count + 1), lblWL.Content.ToString(), a1.ToString(this.absacc),a2.ToString(this.tacc), DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("ru-RU")), false));
                    meisureGrid.ItemsSource = _itemList;
                    //meisureGrid.Items.Add(new { Number = meisureGrid.Items.Count + 1, Name = "Образец 1", Abs = a1.ToString(this.absacc), TProcent = a2.ToString(this.tacc) });
                    print_in_tabel = false;
                }
            }
            else
                this.timer1.Stop();
            if (this.automode)
                return;
            this.progressBar1.Value = 000;
            //this.panel4.Visible = false;
            this.progressBar1.Value = 0;
        }

        private void Excutecmdque()
        {
            if (CommonFun.GetAppConfig("EightSlot") == "true")
            {
                this.calormea = 1;
                if (this.currslot.Length > 0)
                {
                    string wl = this.MM.WL;
                    this.ComSta = ComStatus.CALBGND;
                    this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                    CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
                }
                else
                {
                    this.ComSta = ComStatus.SYSTATE;
                    this.sp.WriteLine("systate \r\n");
                    CommonFun.WriteSendLine("systate");
                }
            }
            else
            {
                string wl = this.MM.WL;
                this.ComSta = ComStatus.CALBGND;
                this.sp.WriteLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                CommonFun.WriteSendLine("calbgnd 1 1 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
            }
            this.tdwait.Start();
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
                            CommonFun.WriteReceiveMsg(this.sp.ReadLine());
                            this.ComSta = ComStatus.END;
                            break;
                        case ComStatus.SYSTATE:
                        case ComStatus.SETCHP:
                        case ComStatus.MEASURE:
                        case ComStatus.CALBGND:
                        case ComStatus.SCANBASE:
                        case ComStatus.BD_RATIO_FLUSH:
                            string text = this.sp.ReadLine();
                            this.myque.Enqueue((object)text);
                            CommonFun.WriteReceiveMsg(text);
                            break;
                            //  else
                            // CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void MeisureEnergy_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            btnBlank.Background = new SolidColorBrush(Color.FromArgb(100, 221, 221, 221));
        }

        private void Home_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            try
            {
                this.timer1.Stop();
                this.ComSta = ComStatus.END;
                this.runtag = false;
                if (this.cancelTokenSource != null)
                {
                    CommonFun.WriteLine("this.cancelTokenSource.Cancel();");
                    this.cancelTokenSource.Cancel();
                    /*try
                    {
                        this.td.Wait(2000);
                    }
                    catch (AggregateException ex)
                    {
                        foreach (Exception innerException in ex.InnerExceptions)
                            Console.WriteLine(ex.Message + " " + innerException.Message);
                        CommonFun.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        try
                        {
                            this.cancelTokenSource.Dispose();
                        }
                        catch (Exception ex)
                        {
                            CommonFun.WriteLine(ex.ToString());
                        }
                    }*/
               //     CommonFun.WriteLine("this.td.Wait(2000);");
                    this.td.Wait(2000);
                    CommonFun.WriteLine("this.cancelTokenSource.Dispose();");
                    this.cancelTokenSource.Dispose();
                }
            }
            catch (Exception ex)
            {
             //   CommonFun.WriteLine("Исключение при диспозе");
                CommonFun.WriteLine(ex.ToString());
            }
         //   CommonFun.WriteLine("Запись в хмл файл");
            try
            {
                CommonFun.setXmlValue("PhoMethod", "C_mode", this.MM.C_mode);
                CommonFun.setXmlValue("PhoMethod", "WL", this.MM.WL);
                CommonFun.setXmlValue("PhoMethod", "R", this.MM.R);
                //  CommonFun.setXmlValue("PhoMethod", "AutoMode", this.automode.ToString());
            }
            catch
            {
                try
                {
                    CommonFun.setXmlValue("PhoMethod", "C_mode", "Abs");
                    CommonFun.setXmlValue("PhoMethod", "WL", "546.0");
                    CommonFun.setXmlValue("PhoMethod", "R", "1");
                    //  CommonFun.setXmlValue("PhoMethod", "AutoMode", "False");
                }
                catch (Exception ex)
                {
                 //   CommonFun.WriteLine("Ошибка при сохранении значений сеанса фотометрии");
                    CommonFun.WriteLine(ex.ToString());
                }
            }
       //     CommonFun.WriteLine("Начала завершения фотометрического режима");
            try
            {
                if (this.sp.IsOpen)
                {
                    CommonFun.WriteSendLine("Завершение фотометрического режима");
                    this.sp.Close();
                }

                if (this.tdstart != null)
                {
           //         CommonFun.WriteLine("Начала завершения фотометрического режима");
                    this.tdstart.Abort();
                }

           //     CommonFun.WriteLine("Скрываем окно");
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
            catch (Exception ex)
            {
  //              CommonFun.WriteLine("Ошибка при завершении сеанса");
                CommonFun.WriteLine(ex.ToString());
            }
            btnBlank.Background = null;
        }

        private delegate void Del_setstate(bool status);

        private delegate void Del_BlanckPreviewMouseDown();

        private delegate void Del_ShowValue(Decimal a1, Decimal a2, bool value);

        private delegate void Del_TimerStop();

        private delegate void Del_SetBlankLable(bool value);

        private delegate void Del_TimerStart();

        private delegate void Del_ShowSlot();

        private delegate void Del_ExcuteQueCmd();

        private void Set_Wl_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.timer1.Stop();
            this.lblValueT.Content = "-------";
            this.lblValueA.Content = "-------";
            //this.lblAvalue.Text = "-------";
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
                            this.MM.WL = frm.txtValue.Text;
                            this.lblWL.Content = Convert.ToDecimal(this.MM.WL).ToString("f1") + " нм";
                            this.ComSta = ComStatus.END;
                            //if (this.automode)
                            //     this.Zero_PreviewMouseDown((object)null, (RoutedEventArgs)null);
                            frm.Close(); frm.Dispose();
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
                                this.MM.WL = frm.txtValue.Text;
                                this.lblWL.Content = Convert.ToDecimal(this.MM.WL).ToString("f1") + " нм";
                                this.ComSta = ComStatus.END;
                                if (this.automode)
                                    this.Zero_PreviewMouseDown((object)null, (RoutedEventArgs)null);
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

            this.timer1.Start();
            this.Zero_PreviewMouseDown((object)null, (RoutedEventArgs)null);
        }

        /*public struct MyTable
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public string WL { get; set; }
            public string Abs { get; set; }
            public string TProcent { get; set; }
            public string DatatTime { get; set; }
            public bool CheckRow { get; set; }
        }*/

        private void Meisure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

            /*string errormsg = "";
            if (CommonFun.GetAppConfig("GLPEnabled") == "true" && !DongleMgr.VerifyDongle(out errormsg, "73F376F6", "1D18D2074B2F1020"))
            {
                CommonFun.showbox(errormsg, "Error");
                this.timer1.Stop();
            }
            else if (!this.sp.IsOpen)
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
            if (this.ComSta != ComStatus.END)
            {
                this.cmdque = "measure 1 2 " + (Convert.ToDecimal(this.MM.WL) * 10M).ToString("f0") + "\r\n";
                // this.panel4.Visible = true;
                this.progressBar1.Value = 30;
            }
            else
            {
                //this.panel4.Visible = true;
                this.progressBar1.Value = 30;
                string wl = this.MM.WL;
                this.ComSta = ComStatus.MEASURE;
                this.sp.WriteLine("measure 1 2 " + (Convert.ToDecimal(wl) * 10M).ToString("f0") + "\r\n");
                CommonFun.WriteSendLine("measure 1 2 " + (Convert.ToDecimal(wl) * 10M).ToString("f0"));
            }

            // List newItem = new List<>*/

            // print_in_tabel = true;
            if (!this.sp.IsOpen)
            {
                init_item_list();
                meisureGrid.ItemsSource = _itemList;
            }
            else
            {
                print_in_tabel = true;
            }
                //meisureGrid.Items.Add(new { Number = meisureGrid.Items.Count + 1, Name = "Образец 1", Abs = "100", TProcent = "100" });

        }

        private readonly ObservableCollection<MyTable> _itemList = new ObservableCollection<MyTable>();
        public ObservableCollection<MyTable> MyTableList { get { return _itemList; } }
        //public ObservableCollection<MyTable> ItemList = new ObservableCollection<MyTable>();


        public void init_item_list()
        {
            MyTableList.Add(new MyTable("Образец " + (meisureGrid.Items.Count + 1), "500.7", "100", "100", DateTime.Now.ToString(), false));
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           foreach(var item in MyTableList)
            {
                item.BooleanFlag = true;
                meisureGrid.Items.Refresh();
                
            }
        }

        private void UnheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyTableList)
            {
                item.BooleanFlag = false;
                meisureGrid.Items.Refresh();
            }
        }

        private void DeleteRow_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           
            foreach (var x in MyTableList.ToList())
            {
                if (x.BooleanFlag)
                {
                    MyTableList.Remove(x);
                }
            }
            meisureGrid.Items.Refresh();

        }
        string filepath;
        string pathTemp = Path.GetTempPath();
        string extension = ".frm";
        private void Save_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.timer1.Stop();
            if (MyTableList.Count == 0)
            {
                CommonFun.showbox("Измерений не было, сохранять нечего", "Info");
            }
            else
            {
                using (SaveFrm save = new SaveFrm(extension, "Фотометрический режим"))
                {
                    save.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
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
            this.timer1.Start();

            /*if(MyTableList.Count == 0)
            {
                CommonFun.showbox("Измерений не было, сохранять нечего");
            }
            else
            {
                CreateXMLDocumentIzmerenieScan();
                WriteXmlIzmerenieFR();
                EncriptorFileBase64 encriptorFileBase64 = new EncriptorFileBase64(filepath + "/Fotometrics.frm", pathTemp);
            }*/
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

            XmlNode Izmerenie = xd.CreateElement("Izmerenie");

            XmlNode DateTime1 = xd.CreateElement("DateTime"); // дата создания градуировки
            DateTime1.InnerText = DateTime.Now.ToString(); // и значение
            Izmerenie.AppendChild(DateTime1); // и указываем кому принадлежит

            int countIzmer = meisureGrid.Items.Count - 1;
            XmlNode countIzmer1 = xd.CreateElement("countIzmer1");
            countIzmer1.InnerText = Convert.ToString(countIzmer);
            Izmerenie.AppendChild(countIzmer1);
            xd.DocumentElement.AppendChild(Izmerenie);

            string[] HeaderCells = new string[meisureGrid.Columns.Count - 1];
            string[,] Cells1 = new string[meisureGrid.Items.Count, 6];


            for (int i = 0; i < meisureGrid.Items.Count; i++)
            {
                XmlNode Cells2 = xd.CreateElement("Stroka");

                XmlAttribute attribute1 = xd.CreateAttribute("Nomer");
                attribute1.Value = Convert.ToString(i); // устанавливаем значение атрибута
                Cells2.Attributes.Append(attribute1); // добавляем атрибут
             
                for (int j = 1; j < meisureGrid.Columns.Count - 1; j++)
                {

                    try
                    {
                        TextBlock x = meisureGrid.Columns[j].GetCellContent(meisureGrid.Items[i]) as TextBlock;
                        //Cells1[i, j] = x.Text;
                        if (x != null)
                            Cells1[i, j] = x.Text;
                        else
                            if (MyTableList.ElementAt(i).Name != null)
                                Cells1[i, j] = MyTableList.ElementAt(i).Name;
                            else
                                Cells1[i, j] = "";
                    }
                    catch
                    {
                        Cells1[i, j] = MyTableList.ElementAt(i).Name;
                    }
                    
                    HeaderCells[j] = meisureGrid.Columns[j].Header.ToString();

                    XmlNode HeaderCells1 = xd.CreateElement("Stolbec"); // Столбец
                    if (Cells1[i, j] != "")
                    {
                        HeaderCells1.InnerText = Cells1[i, j]; // и значение
                    }
                    else
                    {
                        HeaderCells1.InnerText = "-";
                    }

                    Cells2.AppendChild(HeaderCells1); // и указываем кому принадлежит
                    XmlAttribute attribute = xd.CreateAttribute("Header");
                    attribute.Value = HeaderCells[j]; // устанавливаем значение атрибута
                    HeaderCells1.Attributes.Append(attribute); // добавляем атрибут          
                }


               xd.DocumentElement.AppendChild(Cells2);
            }
            fs.Close();         // Закрываем поток  
            xd.Save(filepath); // Сохраняем файл  

        }

        private void Open_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Open_File();
        }
        bool shifrTrueFalse = false;
        public void Open_File()
        {
            this.timer1.Stop();
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Сохраненные измерения");

            OpenFrm openFrm = new OpenFrm(Directory.GetCurrentDirectory() + @"\Сохраненные измерения", ".frm");
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
                    MyTableList.Clear();
                    meisureGrid.Items.Refresh();
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(pathTemp + "/" + openFrm.open_name);

                    XDocument xdoc = XDocument.Load(pathTemp + "/" + openFrm.open_name);
                    XmlNodeList nodes = xDoc.ChildNodes;
                    foreach (XElement IzmerenieElement in xdoc.Element("Data_Izmerenie").Elements("NumberIzmer"))
                    {
                        XElement countIzmer1Element = IzmerenieElement.Element("countIzmer1");

                        if (countIzmer1Element != null)
                        {
                            //  RowsCount = countIzmer1Element.Value;
                        }
                    }

                    // Читаем в цикле вложенные значения Stroka
                    foreach (XmlNode n in nodes)
                    {
                        if ("Data_Izmerenie".Equals(n.Name))
                        {
                            for (XmlNode d = n.FirstChild; d != null; d = d.NextSibling)
                            {

                                // Обрабатываем в цикле только Stroka
                                if ("Stroka".Equals(d.Name))
                                {
                                    int stolbec = 0;
                                    //Можно, например, в этом цикле, да и не только..., взять какие-то данные
                                    List<object> value_xml = new List<object>();
                                    for (XmlNode k = d.FirstChild; k != null; k = k.NextSibling)
                                    {
                                        if ("Stolbec".Equals(k.Name) && k.FirstChild != null)
                                        {
                                            value_xml.Add(k.FirstChild.Value);
                                        }
                                    }

                                    MyTableList.Add(new MyTable(value_xml));
                                    meisureGrid.ItemsSource = _itemList;

                                }
                            }
                        }
                    }

                    File.Delete(pathTemp + "/" + openFrm.open_name);
                }
            }
            this.timer1.Start();
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
          this.timer1.Stop();
          if (!this.sp.IsOpen)
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");


            /*KeyBoard keyBoard = new KeyBoard("Ввод названия образца", (sender as TextBox).Text);
            keyBoard.ShowDialog();
             (sender as TextBox).Text = keyBoard.text_string;*/
            using (KeyBoard frm2 = new KeyBoard("", (sender as TextBox).Text))
            {
                // frm2.lbltitle.Text = "reason";
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    (sender as TextBox).Text = frm2.txtValue.Text;

                    frm2.Close();

                });
                Convert.ToInt32(frm2.ShowDialog());
            }
           
            timer1.Start();
            
            
        }

        private void Name_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.timer1.Stop();
            if (!this.sp.IsOpen)
                CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");


            /*KeyBoard keyBoard = new KeyBoard("Ввод названия образца", (sender as TextBox).Text);
            keyBoard.ShowDialog();
             (sender as TextBox).Text = keyBoard.text_string;*/
            using (KeyBoard frm2 = new KeyBoard("", (sender as TextBox).Text))
            {
                // frm2.lbltitle.Text = "reason";
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    (sender as TextBox).Text = frm2.txtValue.Text;

                    frm2.Close();

                });
                Convert.ToInt32(frm2.ShowDialog());
            }

            timer1.Start();
        }

        private void ExportWord_PreviewMouseDown(object sender, EventArgs e)
        {
            this.timer1.Stop();
            if (MyTableList.Count == 0)
            {
                CommonFun.showbox("Измерений не было, сохранять нечего", "Info");
            }
            else
            {
                using (SaveFrm save = new SaveFrm(extension, "Фотометрический режим"))
                {
                    save.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
                    {
                        if (save.Name_file.Text.Length > 0)
                        {
                            filepath = Directory.GetCurrentDirectory() + @"\Сохраненные протоколы";
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            filepath = filepath + @"\" + save.Name_file.Text + ".docx";

                            if (!File.Exists(filepath))
                            {
                                //save_name = filepath;
                                ExportWord_(filepath);
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
            this.timer1.Start();
            //ExportWord_();
        
                
        }
        string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private void ExportWord_(string sfd)
        {
            Word.Application wordApp = null;
            wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document wordDoc = null;

            string pathName = path + "\\template\\Protocol for measurement in Photometric mode.docx";
            wordDoc = wordApp.Documents.Add(pathName);

            Word.Bookmark bkm = wordDoc.Bookmarks["Table"];
            Word.Range rng = bkm.Range;

            Object behiavor = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
            Object autoFitBehiavor = Word.WdAutoFitBehavior.wdAutoFitFixed;

            wordDoc.Tables.Add(rng, meisureGrid.Items.Count, meisureGrid.Columns.Count - 1, ref behiavor, ref autoFitBehiavor);

            for (int i = 0; i < meisureGrid.Columns.Count; i++)
            {
                wordDoc.Tables[1].Cell(1, i + 1).Range.Text = meisureGrid.Columns[i].Header.ToString();
            }

            for (int i = 0; i < meisureGrid.Items.Count; i++)
            {
                for (int j = 0; j < meisureGrid.Columns.Count - 1; j++)
                {

                    try
                    {
                        TextBlock x = meisureGrid.Columns[j].GetCellContent(meisureGrid.Items[i]) as TextBlock;
                        if (x != null)
                            wordDoc.Tables[1].Cell(i + 2, j + 1).Range.Text = x.Text;
                        else
                            if (MyTableList.ElementAt(i).Name != null)
                            wordDoc.Tables[1].Cell(i + 2, j + 1).Range.Text = MyTableList.ElementAt(i).Name;
                        else
                            wordDoc.Tables[1].Cell(i + 2, j + 1).Range.Text = "";
                    }
                    catch
                    {
                        wordDoc.Tables[1].Cell(i + 2, j + 1).Range.Text = MyTableList.ElementAt(i).Name;
                    }
                }
            }

            wordDoc.SaveAs2(sfd);

            wordDoc.Close();
            wordApp.Quit();
            MessageBox.Show("Файл сформирован!");


        }

       
        private void ExportToExcel_(string sfd)
        {
            Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
            exApp.Application.Workbooks.Add(Type.Missing);

            exApp.Columns.ColumnWidth = 20;

            exApp.Cells[1, 1] = CommonFun.GetLanText("type");

            if (MM.InstrumentsType == "P7")
                exApp.Cells[1, 2] = "УФ-6700";
            if (MM.InstrumentsType == "P8")
                exApp.Cells[1, 2] = "УФ-6800";
            if (MM.InstrumentsType == "P9")
                exApp.Cells[1, 2] = "УФ-6900";
            //exApp.Cells[1, 2] = MM.InstrumentsType;
            exApp.Cells[1, 3] = CommonFun.GetLanText("serino");
            exApp.Cells[1, 4] = MM.Serials;

            exApp.Cells[2, 1] = CommonFun.GetLanText("specturmrange") + "(нм)";
            exApp.Cells[2, 2] = CommonFun.GetAppConfig("Spectralbandwidth");
            exApp.Cells[2, 3] = CommonFun.GetLanText("lightswitch") + "(нм)";
            exApp.Cells[2, 4] = CommonFun.GetAppConfig("SwithWL");


            exApp.Cells[3, 1] = CommonFun.GetLanText("operatemode");
            if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                exApp.Cells[3, 2] = "GLP";
            else
                exApp.Cells[3, 2] = CommonFun.GetLanText("common");
            exApp.Cells[3, 3] = CommonFun.GetLanText("operators");
            exApp.Cells[3, 4] = MM.C_Operator;

            //exApp.Cells[4, 1] = CommonFun.GetLanText("measuretime");
            //   exApp.Cells[4, 2] = MM.D_MTime;

            for (int i = 0; i < meisureGrid.Columns.Count - 1; i++)
            {
                if (i == 0)
                {
                    exApp.Cells[5, i + 1] = meisureGrid.Columns[i].Header.ToString();
                    if (i == 0)
                        exApp.Cells[5, i + 1] = "Наименование";
                }
                else
                {
                    if (i > 1)
                    {
                        exApp.Cells[5, i] = meisureGrid.Columns[i].Header.ToString();
                    }
                }
            }

            for (int i = 0; i < meisureGrid.Items.Count; i++)
            {
                for (int j = 0; j < meisureGrid.Columns.Count - 1; j++)
                {
                    if (j == 0)
                    {
                        try
                        {
                            TextBlock x = meisureGrid.Columns[j].GetCellContent(meisureGrid.Items[i]) as TextBlock;
                            if (x != null)
                                exApp.Cells[i + 6, j + 1] = x.Text;
                            else
                                if (MyTableList.ElementAt(i).Name != null)
                                exApp.Cells[i + 6, j + 1] = MyTableList.ElementAt(i).Name;
                            else
                                exApp.Cells[i + 6, j + 1] = "";
                        }
                        catch
                        {
                            exApp.Cells[i + 6, j + 1] = MyTableList.ElementAt(i).Name;
                        }
                        Microsoft.Office.Interop.Excel.XlBordersIndex BorderIndex;

                        /*BorderIndex = Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].ColorIndex = 0;

                        BorderIndex = Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].ColorIndex = 0;

                        BorderIndex = Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].ColorIndex = 0;

                        BorderIndex = Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        exApp.Range[i + 6, j + 1].Borders[BorderIndex].ColorIndex = 0;*/

                        //exApp.get_Range(i + 6, j + 1).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        //  exApp.get_Range(i + 6, j + 1).Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    }
                    else
                    {
                        if (j > 1)
                        {
                            try
                            {
                                TextBlock x = meisureGrid.Columns[j].GetCellContent(meisureGrid.Items[i]) as TextBlock;
                                if (x != null)
                                    exApp.Cells[i + 6, j] = x.Text;
                                else
                                    if (MyTableList.ElementAt(i).Name != null)
                                    exApp.Cells[i + 6, j] = MyTableList.ElementAt(i).Name;
                                else
                                    exApp.Cells[i + 6, j] = "";
                            }
                            catch
                            {
                                exApp.Cells[i + 6, j] = MyTableList.ElementAt(i).Name;
                            }
                        }
                    }
                }
            }

            /* exApp.ActiveWorkbook.SaveCopyAs(sfd);
             exApp.ActiveWorkbook.Saved = true;
             // exApp.Visible = true;
             Microsoft.Office.Interop.Excel.Workbook excelWorkbook;
             excelWorkbook = exApp.Workbooks.Open(sfd);
             var exportSuccessful = true;
             try
             {
                 // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
                 excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, sfd.Replace("xlsx", "pdf"));
             }
             catch (System.Exception ex)
             {
                 // Mark the export as failed for the return value...
                 exportSuccessful = false;

                 // Do something with any exceptions here, if you wish...
                 // MessageBox.Show...        
             }
             finally
             {
                 // Close the workbook, quit the Excel, and clean up regardless of the results...
                 excelWorkbook.Close();
                 exApp.Quit();

                 exApp = null;
                 excelWorkbook = null;
             }*/
            exApp.ActiveWorkbook.SaveCopyAs(sfd);
            exApp.ActiveWorkbook.Saved = true;
            exApp.Visible = false;
            exApp.Quit();
        }

        private void BtnToExcel_PreviewMouseDown(object sender, EventArgs e)
        {
            this.timer1.Stop();
            RegistryKey hkcr = Registry.ClassesRoot;
            RegistryKey excelKey = hkcr.OpenSubKey("Excel.Application");
            bool excelInstalled = excelKey == null ? false : true;
            if (excelInstalled == true)
            {
                if (MyTableList.Count == 0)
                {
                    CommonFun.showbox("Измерений не было, сохранять нечего", "Info");
                }
                else
                {
                    using (SaveFrm save = new SaveFrm(extension, "Фотометрический режим"))
                    {
                        save.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
                        {
                            if (save.Name_file.Text.Length > 0)
                            {
                                filepath = Directory.GetCurrentDirectory() + @"\Сохраненные протоколы";
                                if (!Directory.Exists(filepath))
                                {
                                    Directory.CreateDirectory(filepath);
                                }
                                filepath = filepath + @"\" + save.Name_file.Text + ".xlsx";

                                if (!File.Exists(filepath))
                                {
                                    //save_name = filepath;
                                    ExportToExcel_(filepath);
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
            this.timer1.Start();
        }

        private void BtnToPdf_PreviewMouseDown(object sender, EventArgs e)
        {
            ///ExportPDF();
        }

        private void BtnToPrint_PreviewMouseDown(object sender, EventArgs e)
        {
            this.timer1.Stop();
            if (MyTableList.Count == 0)
            {
                CommonFun.showbox("Измерений не было, печатать нечего", "Info");
            }
            else
            {
                string str = "";
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    str += PrinterSettings.InstalledPrinters[i] + "\n";
                }
                if (str != "")
                {
                    /*PrintPreviewDialogSelectPrinter printPreviewDialogSelectPrinter = new PrintPreviewDialogSelectPrinter();
                    printPreviewDialogSelectPrinter.Document = IzmerenieFRprintTable1;
                    printPreviewDialogSelectPrinter.ShowDialog();*/
                    PrintDialog pDialog = new PrintDialog();
                    pDialog.PageRangeSelection = PageRangeSelection.AllPages;
                    pDialog.UserPageRangeEnabled = true;

                    // Display the dialog. This returns true if the user presses the Print button.
                    Nullable<Boolean> print = pDialog.ShowDialog();
                    if (print == true)
                    {
                        XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory() + @"\Сохраненные протоколы\FixedDocumentSequence.xps", FileAccess.ReadWrite);
                        FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
                        pDialog.PrintDocument(fixedDocSeq.DocumentPaginator, "Test print job");
                    }
                }
            }
        }
    }
   /* private void IzmerenieFRprintTable1(object sender, PrintPageEventArgs e)
    {
        e.Graphics.DrawString("Протокол выполнения измерений\n       в Фотометрическом режиме\n\n", new System.Drawing.Font("Times New Roman", 20, FontStyle.Bold), Brushes.Black, 180, 50);
    }
    */


    public class MyTable
    {
        public MyTable() { }
        public MyTable(List<object> array)
        {
          //  this.Number = Convert.ToInt32(array[0]);
            this.Name = Convert.ToString(array[0]);
            this.WL = Convert.ToString(array[1]);
            this.Abs = Convert.ToString(array[2]);
            this.TProcent = Convert.ToString(array[3]);
            this.DateTime_ = Convert.ToString(array[4]);
            this.BooleanFlag = false;
        }
        public MyTable(string Name, string WL, string Abs, string TProcent, string DateTime_, bool BooleanFlag)
        {
          //  this.Number = Number;
            this.Name = Name;
            this.WL = WL;
            this.Abs = Abs;
            this.TProcent = TProcent;
            this.DateTime_ = DateTime_;
            this.BooleanFlag = BooleanFlag;
        }
        //private int _value_number;
      //  public int Number { get { return _value_number; } set { _value_number = value; } }
        private string _value_name;
        public string Name { get { return _value_name; } set { _value_name = value; } }
        private string _value_wl;
        public string WL { get { return _value_wl; } set { _value_wl = value; } }
        private string _value_abs;
        public string Abs { get { return _value_abs; } set { _value_abs = value; } }
        private string _value_tprocent;
        public string TProcent { get { return _value_tprocent; } set { _value_tprocent = value; } }
        private string _value_datetime;
        public string DateTime_ { get { return _value_datetime; } set { _value_datetime = value; } }
        private bool _value_checkrow;
        public bool BooleanFlag { get { return _value_checkrow; } set { _value_checkrow = value; } }
    }


}
