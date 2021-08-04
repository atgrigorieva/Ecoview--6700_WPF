using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application = System.Windows.Application;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для MenuProgram.xaml
    /// </summary>
    public partial class MenuProgram : Window
    {
        private SerialPort sp = new SerialPort();
        public MenuProgram()
        {
            InitializeComponent();
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1),
                IsEnabled = true
            };
            timer.Tick += (o, e) => { time_now.Content = DateTime.Now.ToString("HH:mm"); date_now.Content = DateTime.Now.ToString("dd.MM.yyyy"); };
            timer.Start();

            lblName.Content = CommonFun.GetAppConfig("currentuser");

            if (CommonFun.GetAppConfig("currentuser") == "Admin")
                UserManagerButton.IsEnabled = true;

        }

        private void OperationTask_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           // Operation operationProgram = new Operation();
            //operationProgram.Show();
            using (Operation frm = new Operation())
            {
                frm.BackToWindows.PreviewMouseDown += ((param0, param1) =>
                {
                    SerialPort serialPort = new SerialPort();
                    try
                    {
                        serialPort.BaudRate = 38400;
                        serialPort.PortName = "COM2";
                        serialPort.DataBits = 8;
                        serialPort.StopBits = StopBits.One;
                        serialPort.Parity = Parity.None;
                        serialPort.ReadTimeout = -1;
                        serialPort.RtsEnable = true;
                        serialPort.Handshake = Handshake.None;
                        //sp.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceived);
                        serialPort.Open();
                    }
                    catch(Exception ex)
                    {
                        if (serialPort.IsOpen)
                            serialPort.Close();
                    }
                    if (serialPort.IsOpen)
                    {
                        CommonFun.WriteSendLine("close,");
                        serialPort.WriteLine("DISCONN \r\n");
                        CommonFun.WriteSendLine("DISCONN");
                        serialPort.Close();
                    }
                    frm.Close(); frm.Dispose();
                    Application.Current.Shutdown();
                });

                int num = Convert.ToInt32(frm.ShowDialog());
            }
        }

        private void SystemButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            SystemTools systemTools = new SystemTools();
            systemTools.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void PhotomentryButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            //Photometry photometry = new Photometry();
            //photometry.Show();
            //Hide();
            PhotoMertyFrm photometry = new PhotoMertyFrm();
            photometry.Show();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();

        }

        private void MultiWavelengthButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            MulWLFrm mlLWL = new MulWLFrm();
            mlLWL.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();

        }

        private void KineticsButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Kinetics kinetics = new Kinetics();
            kinetics.ShowDialog();
            this.Close();
            //Window parentWindow = Window.GetWindow(this);
           // parentWindow.Close();
        }

        private void TimeScanButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            TimeScanFrm timeScan = new TimeScanFrm();
            timeScan.ShowDialog();
            this.Close();
        }

        private void QuantitationButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            Quantion quantion = new Quantion();
            quantion.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void DualComponentButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            DualComponentAnalysis dualComponent = new DualComponentAnalysis();
            dualComponent.ShowDialog();

            this.Close();
            //Window parentWindow = Window.GetWindow(this);
            // parentWindow.Close();
        }

        private void DNAProteinButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            DNAFrm dNAProtein = new DNAFrm();
            dNAProtein.ShowDialog();
           // Window parentWindow = Window.GetWindow(this);
        //    parentWindow.Close();
        }

        private void SpectrumButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            SpectrumScan spectrumScan = new SpectrumScan();
            spectrumScan.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void UserManagerButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            UserManager userManager = new UserManager();
            userManager.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void PerfomanceVerificationsButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            PerformanceVerification perfomanceVerification = new PerformanceVerification();
            perfomanceVerification.Show();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void FileManagerButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            FileManager fileManager = new FileManager();
            fileManager.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void CustomMethodButton_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            CustomMethod customMethod = new CustomMethod();
            customMethod.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
            if (sp.IsOpen)
            {
                sp.WriteLine("DISCONN \r\n");
                CommonFun.WriteSendLine("DISCONN");
                sp.Close();
            }
        }
    }
}
