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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для Operation.xaml
    /// </summary>
    public partial class Operation : Window, IDisposable
    {
        private SerialPort sp = new SerialPort();
        private Thread tdstart;
        private Queue myque = new Queue();
        public Operation()
        {
            try
            {
                InitializeComponent();

              //  this.tdstart = new Thread(new ThreadStart(this.tdstart_Elapsed));
              //  this.tdstart.Start();
               
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }

        public void Dispose()
        {
          //  if (sp.IsOpen)
           // {
          //      sp.Close();
         //   }
           // this.Close();
        }

        private void tdstart_Elapsed()
        {
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
            }
            catch (Exception ex)
            {
                CommonFun.WriteLine(ex.ToString());

            }
        }




        private void BackToWindows_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           /* if (sp.IsOpen)
            {
                CommonFun.WriteSendLine("close,");
                sp.WriteLine("DISCONN \r\n");
                CommonFun.WriteSendLine("DISCONN");
                sp.Close();
            }*/

            //Application.Current.Shutdown();
           // this.Close();
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //try
          //  {
         //       if (this.sp.IsOpen)
          //          this.myque.Enqueue((object)this.sp.ReadLine());
                //  else
                // CommonFun.showbox(CommonFun.GetLanText("opencom"), "Warning");
         //   }
          //  catch
          //  {
           // }
        }
    }
}
