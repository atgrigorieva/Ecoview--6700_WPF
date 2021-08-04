using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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
using KeyEventHandler = System.Windows.Input.KeyEventHandler;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для Photometry.xaml
    /// </summary>
    public partial class Photometry : Window
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


        DispatcherTimer timer1 = new DispatcherTimer();
        public Photometry()
        {
            try
            {
                InitializeComponent();

                timer1.Interval = TimeSpan.FromMilliseconds(1000);
                timer1.Tick += new EventHandler(this.timer1_Tick);

                StartProgram();
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
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
                    this.lblWL.Content = Convert.ToDecimal(this.MM.WL).ToString("f1") + " nm";
                    if (this.MM.C_mode == "Abs")
                    {
                        this.lblmode.Content = "Abs";
                        this.lblUnit.Content = "Abs";
                    }
                    else
                    {
                        this.lblmode.Content = "%T";
                        this.lblUnit.Content = "%T";
                    }
                }
                this.lblWL.Content = Convert.ToDecimal(this.MM.WL).ToString("f1") + " nm";
                this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
                this.tdstart.Start();
                this.setstate(false);
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }


            // CommonFun.showbox(ports.Count<string>().ToString());




        }

        private void setstate(bool status)
        {
  
            this.button1.IsEnabled = status;
            this.button2.IsEnabled = status;
            this.button3.IsEnabled = status;
            this.button4.IsEnabled = status;
            this.button5.IsEnabled = status;
            this.button6.IsEnabled = status;
            this.button7.IsEnabled = status;
            this.button8.IsEnabled = status;
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

                if (!this.automode)
                {
                    this.ComSta = ComStatus.BD_RATIO_FLUSH;
                    this.sp.WriteLine("BD_RATIO_FLUSH \r\n");
                    CommonFun.WriteSendLine("BD_RATIO_FLUSH");
                }
                else
                    this.ComSta = ComStatus.END;

                //this.timer1.Interval = (int)Convert.ToInt16(Convert.ToDecimal(CommonFun.GetAppConfig("PhotoMetryInterval")) * 1000M);

            }
            catch (Exception ex)
            {
                CommonFun.showbox(ex.Message.ToString(), "Error");

            }


        }

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
                                    if (!this.button1.Dispatcher.CheckAccess())
                                        this.button1.Dispatcher.Invoke((Delegate) new Photometry.Del_ShowSlot(this.ShowSlot));
                                    else
                                        this.ShowSlot();
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
                                            this.btnBlank.Dispatcher.Invoke((Delegate) new Photometry.Del_TimerStart(this.StartTimer));
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
                                                if (!this.lblValue.Dispatcher.CheckAccess())
                                                    this.lblValue.Dispatcher.Invoke((Delegate) new Photometry.Del_ShowValue(this.ShowValue), (object)a1, (object)a2, (object)true);
                                                else
                                                    this.ShowValue(a1, a2, true);
                                            }
                                            this.receive = "";
                                            this.ComSta = ComStatus.END;
                                            if (this.cmdque.Length > 0)
                                            {
                                                if (!this.btnBlank.Dispatcher.CheckAccess())
                                                    this.btnBlank.Dispatcher.Invoke((Delegate)new Photometry.Del_ExcuteQueCmd(this.Excutecmdque));
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
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Photometry.Del_TimerStop(this.StopTimer));
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
                                            this.btnBlank.Dispatcher.Invoke((Delegate)new Photometry.Del_SetBlankLable(this.Setblanklable), (object)true);
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
                                        this.btnBlank.Dispatcher.Invoke((Delegate)new Photometry.Del_SetBlankLable(this.Setblanklable), (object)false);
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
                                    this.btnBlank.Dispatcher.Invoke((Delegate)new Photometry.Del_setstate(this.setstate), (object)true);
                                    break;
                                }
                                this.setstate(true);
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
                    this.lblValue.Content = "---";
                    //this.lblAvalue.Content = "----";
                   // this.lblKvalue.Text = this.MM.R;
                }
                else if (this.MM.C_mode == "Abs")
                {
                    this.lblValue.Content = a1.ToString(this.absacc);
                    this.lblUnit.Content = "Abs";
                    //this.lblAvalue.Text = a1.ToString(this.absacc) + " A";
                  //  this.lblKvalue.Text = this.MM.R;
                }
                else
                {
                    this.lblValue.Content = a2.ToString(this.tacc);
                    this.lblUnit.Content = "%T";
                   // this.lblAvalue.Text = a1.ToString(this.tacc) + " %T";
                }
                meisureGrid.Items.Add(new { Number = meisureGrid.Items.Count + 1, Name = "Образец 1", Abs = a1.ToString(this.absacc), TProcent = a2.ToString(this.tacc) });
            }
            else
                this.timer1.Stop();
            if (this.automode)
                return;
            this.progressBar1.Value = 000;
            //this.panel4.Visible = false;
            this.progressBar1.Value = 0;
        }


        private void StopTimer() => this.timer1.Stop();

        private void StartTimer() => this.timer1.Start();

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
                        this.btnBlank.Dispatcher.Invoke((Delegate) new Photometry.Del_SetBlankLable(this.Setblanklable), (object)false);
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
            //this.btnBlank.Text = CommonFun.GetLanText(this.lanvalue, "blanking");
            //this.btnBlank.Enabled = true;
            this.progressBar1.Value = 000;
            //this.panel4.Visible = false;
            this.progressBar1.Value = 0;
            this.tdwait.Stop();
            this.repeatcnt = 0;
            this.timer1.Stop();
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
                    CommonFun.WriteLine("this.td.Wait(2000);");
                    this.td.Wait(2000);
                    CommonFun.WriteLine("this.cancelTokenSource.Dispose();");
                    this.cancelTokenSource.Dispose();
                }
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine("Исключение при диспозе");
                CommonFun.WriteLine(ex.ToString());
            }
            CommonFun.WriteLine("Запись в хмл файл");
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
                    CommonFun.WriteLine("Ошибка при сохранении значений сеанса фотометрии");
                    CommonFun.WriteLine(ex.ToString());
                }
            }
            CommonFun.WriteLine("Начала завершения фотометрического режима");
            try
            {
                if (this.sp.IsOpen)
                {
                    CommonFun.WriteSendLine("Завершение фотометрического режима");
                    this.sp.Close();
                }
                
                if (this.tdstart != null)
                {
                    CommonFun.WriteLine("Начала завершения фотометрического режима");
                    this.tdstart.Abort();
                }

                CommonFun.WriteLine("Скрываем окно");
                Hide();
                CommonFun.WriteLine("Получаем меню");
                MenuProgram menuProgram = new MenuProgram();
                CommonFun.WriteLine("Выводим меню");
                menuProgram.Show();
                CommonFun.WriteLine("Получаем родительское окно");
                Window parentWindow = Window.GetWindow(this);
                CommonFun.WriteLine("Закрываем родительское окно");
                parentWindow.Close();
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine("Ошибка при завершении сеанса");
                CommonFun.WriteLine(ex.ToString());
            }
            
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
            }

        }

        private void Meisure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

            string errormsg = "";
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

            // List newItem = new List<>
            
        }



        private void Button1_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "1";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 1\r\n");
                CommonFun.WriteSendLine("SETCHP 1");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Button2_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "2";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 2\r\n");
                CommonFun.WriteSendLine("SETCHP 2");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Button3_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "3";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 3\r\n");
                CommonFun.WriteSendLine("SETCHP 3");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Button4_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "4";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 4\r\n");
                CommonFun.WriteSendLine("SETCHP 4");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Button5_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "5";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 5\r\n");
                CommonFun.WriteSendLine("SETCHP 5");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Button6_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "6";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 6\r\n");
                CommonFun.WriteSendLine("SETCHP 6");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Button7_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "7";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 7\r\n");
                CommonFun.WriteSendLine("SETCHP 7");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Button8_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                this.currslot = "8";
                this.timer1.Stop();
                this.calormea = 2;
                this.ComSta = ComStatus.SETCHP;
                this.sp.WriteLine("SETCHP 8\r\n");
                CommonFun.WriteSendLine("SETCHP 8");
                image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                image8.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void ShowSlot()
        {
            try
            {
                switch (this.currslot)
                {
                    case "1":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        break;
                    case "2":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        break;
                    case "3":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        break;
                    case "4":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        break;
                    case "5":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        break;
                    case "6":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        break;
                    case "7":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        break;
                    case "8":
                        image1.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image2.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image3.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image4.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image5.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image6.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image7.Source = new BitmapImage(new Uri("img/ico_Cell_Unselected.png", UriKind.Relative));
                        image8.Source = new BitmapImage(new Uri("img/ico_Cell_Sellected.png", UriKind.Relative));
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        private void Mode_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if (this.lblmode.Content.ToString() == "%T")
            {
                this.MM.C_mode = "Abs";
                this.lblUnit.Content = "Abs";
                this.lblmode.Content = "Abs";
            }
            else
            {
                this.lblmode.Content = "%T";
                this.MM.C_mode = "%T";
                this.lblUnit.Content = "%T";
            }
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
            this.lblValue.Content = "-------";
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
                            this.lblWL.Content = Convert.ToDecimal(this.MM.WL).ToString("f1") + " nm";
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
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
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
                            this.MM.WL = frm.txtValue.Text;
                            this.lblWL.Content = Convert.ToDecimal(this.MM.WL).ToString("f1") + " nm";
                            this.ComSta = ComStatus.END;
                            if (this.automode)
                                this.Zero_PreviewMouseDown((object)null, (RoutedEventArgs)null);
                            frm.Close(); frm.Dispose();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
                frm.btnCancel.PreviewMouseDown += ((param0, param1) =>
                {
                    if (!this.automode)
                        return;
                    this.Zero_PreviewMouseDown((object)null, (RoutedEventArgs)null);
                });
                object num1 = frm.ShowDialog();
            }



        }

        private void Button1_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }
    }
}
