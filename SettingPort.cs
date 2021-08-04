using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UVStudio
{
    public partial class SettingPort : Form
    {
      //  bool nonPort;
    //    public string portsName; //Имя порта
        Conection _Conection;
       // public SettingPort(bool nonPort, string portsName)
        public SettingPort(Conection parent)
        {
            InitializeComponent();
            this._Conection = parent;
            //this.nonPort = nonPort;
            //this.portsName = portsName;


            //CO();
            // SW();
            // InitializeTimer();
            string[] ports = SerialPort.GetPortNames();

            try
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    SerialPort newPort = new SerialPort();

                    // настройки порта (Communication interface)
                    newPort.PortName = ports[i];
                    newPort.BaudRate = 9600;
                    newPort.DataBits = 8;
                    newPort.Parity = System.IO.Ports.Parity.None;
                    newPort.StopBits = System.IO.Ports.StopBits.One;
                    // Установка таймаутов чтения/записи (read/write timeouts)
                    newPort.ReadTimeout = -1;
                    //newPort.WriteTimeout = 100;
                    //    newPort.DataReceived += new SerialDataReceivedEventHandler(newPort_DataReceived);
                    //newPort.RtsEnable = false;
                    //newPort.DtrEnable = true;
                    try
                    {
                        newPort.Open();// CommonFun.showbox("ПОРТ ОТКРЫТ " + newPort.PortName);
                        newPort.Write("CONNECT \r\n\t");
                        int byteRecieved = newPort.ReadBufferSize;
                        System.Threading.Thread.Sleep(50);
                        byte[] buffer = new byte[byteRecieved];
                        try
                        {
                            newPort.Read(buffer, 0, byteRecieved);
                            newPort.DiscardInBuffer();
                            newPort.DiscardOutBuffer();
                            newPort.WriteLine("DISCONNECT \r\n\t");
                            newPort.ReadLine();
                            newPort.Close();

                        } // Читаем ответ(если ничего не пришло отваливаемся по ReadTimeout = 500
                        catch (TimeoutException)
                        { /* Девайса нет */

                            newPort.DiscardInBuffer();
                            newPort.DiscardOutBuffer();
                            newPort.Close();
                            ports[i] = null;
                            ports = ports.Where(x => x != null).ToArray();
                            i--;

                        }
                    }
                    catch
                    {
                        ports[i] = null;
                        ports = ports.Where(x => x != null).ToArray();
                        i--;
                    }

                }
               /* string s1 = "";
                StreamReader fs = new StreamReader(@"pribor/openport.port");
                string s = "";


                s = fs.ReadLine();
                s1 = s;
                fs.Close();*/

                selectPort.Items.Clear();
                selectPort.Items.AddRange(ports);
                if (ports.Length != 0)
                {
                    selectPort.SelectedIndex = 0;
                    _Conection.nonPort = true;
                }
                else
                {
                    if (ports.Length != 0)
                    {
                        selectPort.SelectedIndex = 0;
                        _Conection.nonPort = true;
                       
                    }
                    else
                    {
                        CommonFun.showbox("Подсоедините спектрофотометр и попробуйте подключиться снова, serial port monitor!", "Info");
                        _Conection.nonPort = false;
                        Close();
                        // Dispose();
                    }
                }

            }
            catch (Exception e)
            {
               // CommonFun.showbox("Порт занят! Освободите порт!", "Info");
                CommonFun.showbox("Ошибка = " + e.ToString(), "Info");
            }
        }

        private void SettingPort_Load(object sender, EventArgs e)
        {

        }

        private void SettingPort_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_Conection.nonPort == false)
            {
                _Conection.nonPort = false;
                CommonFun.showbox("Порт не выбран!", "Info");
                Close();
            }
            else
            {
                _Conection.nonPort = true;
                Close();
            }
        }

        private void conection_Click(object sender, EventArgs e)
        {
            _Conection.portsName = selectPort.SelectedItem.ToString();

            Close();
        }
    }
}
