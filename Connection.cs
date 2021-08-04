using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UVStudio
{
    public class Conection
    {
        MainWindow _Analis;
        public Conection(MainWindow parent)
        {
            this._Analis = parent;
            COnectionPort();


        }
        public bool nonPort;
        public string portsName;
        public SerialPort newPort;
        private System.Timers.Timer tdwait = new System.Timers.Timer(5000.0);
   
        public string versionPribor; //версия прибора
   
        public void COnectionPort()
        {
            // SettingPort _SettingPort = new SettingPort(_Analis.nonPort, _Analis.portsName);
            newPort = new SerialPort();
            SettingPort _SettingPort = new SettingPort(this);
            _Analis.newPort = newPort;
            _Analis.nonPort = nonPort;
            _Analis.portsName = portsName;
            if (_Analis.nonPort == true)
            {
                _SettingPort.ShowDialog();
            }
            else
            {
                _SettingPort.Dispose();
            }
            _Analis.newPort = newPort;
            _Analis.nonPort = nonPort;
            _Analis.portsName = portsName;
            if (_Analis.nonPort == true)
            {
                //_Analis.newPort = new SerialPort();

                try
                {
                    // настройки порта (Communication interface)
                    _Analis.newPort.PortName = portsName;
                    _Analis.newPort.BaudRate = 9600;
                    _Analis.newPort.DataBits = 8;
                    _Analis.newPort.Parity = System.IO.Ports.Parity.None;
                    _Analis.newPort.StopBits = System.IO.Ports.StopBits.One;
                    // Установка таймаутов чтения/записи (read/write timeouts)
                    _Analis.newPort.ReadTimeout = -1;
                    //_Analis.newPort.WriteTimeout = 20000;
                    //    newPort.DataReceived += new SerialDataReceivedEventHandler(newPort_DataReceived);
                    //_Analis.newPort.RtsEnable = false;
                    //_Analis.newPort.DtrEnable = true;
                    _Analis.newPort.Open();// CommonFun.showbox("ПОРТ ОТКРЫТ " + newPort.PortName);


                    //_Analis.newPort.DiscardInBuffer();
                    //_Analis.newPort.DiscardOutBuffer();
                }
                catch (Exception)
                {
                    CommonFun.showbox("Порт не был выбран, connection!", "Info");
                    return;

                }
               



            }


        }
        

    }
}
