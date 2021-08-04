using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SWF = System.Windows.Forms;

namespace Ecoview_Normal
{
    class SettingWL
    {
        //string wavelength1;
        // SerialPort newPort;
        // TextBox GWNew;
        Conection _Conection;
        public SettingWL(Conection parent)
        {
            this._Conection = parent;
          //  this.wavelength1 = wavelength;
          //  this.newPort = newPort1;
            int byteRecieved = _Conection.newPort.ReadBufferSize;
            
            Thread.Sleep(500);
            byte[] buffer = new byte[byteRecieved];
            _Conection.newPort.Read(buffer, 0, byteRecieved);
            
            string GW1 = "";

            for (int i = 0; i <= 50; i++)
            {
                GW1 = GW1 + Convert.ToChar(buffer[i]);
            }
            var GWarr = GW1.Split("\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);



            _Conection.GW1_2 = GWarr[2];
            _Conection.GWNew.Text = _Conection.GW1_2;
            _Conection.versionPribor = GWarr[1];
            if (_Conection.wavelength1 == Convert.ToString(0) || _Conection.wavelength1 == "")
            {
                _Conection.wavelength1 = _Conection.GW1_2;
            }
            else
            {
                bool dlinavoln = true;

                if (_Conection.versionPribor.Contains("V"))
                {
                    if (Convert.ToDouble(_Conection.wavelength1.Replace(".", ",")) < 315)
                    {
                        MessageBox.Show("Установленая длина волны выходит за пределы диапазона спектрофотометра, измените настройки градуировки!");
                        dlinavoln = false;
                    }
                    if (Convert.ToDouble(_Conection.wavelength1.Replace(".", ",")) > 1050)
                    {
                        MessageBox.Show("Установленая длина волны выходит за пределы диапазона спектрофотометра, измените настройки градуировки!");
                        dlinavoln = false;
                    }
                }
                else
                {
                    if (_Conection.versionPribor.Contains("U") && _Conection.versionPribor.Contains("2"))
                    {
                        if (Convert.ToDouble(_Conection.wavelength1.Replace(".", ",")) < 190)
                        {
                            MessageBox.Show("Установленая длина волны выходит за пределы диапазона спектрофотометра, измените настройки градуировки!");
                            dlinavoln = false;
                        }
                        if (Convert.ToDouble(_Conection.wavelength1.Replace(".", ",")) > 1050)
                        {
                            MessageBox.Show("Установленая длина волны выходит за пределы диапазона спектрофотометра, измените настройки градуировки!");
                            dlinavoln = false;
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(_Conection.wavelength1.Replace(".", ",")) < 200)
                        {
                            MessageBox.Show("Установленая длина волны выходит за пределы диапазона спектрофотометра, измените настройки градуировки!");
                            dlinavoln = false;
                        }
                        if (Convert.ToDouble(_Conection.wavelength1.Replace(".", ",")) > 1050)
                        {
                            MessageBox.Show("Установленая длина волны выходит за пределы диапазона спектрофотометра, измените настройки градуировки!");
                            dlinavoln = false;
                        }
                    }
                }

                if (dlinavoln == true)
                {
                    SW();
                }
            }
        }
        public void SW()
        {
            double wevelenght1_double = Convert.ToDouble(_Conection.wavelength1.Replace(".", ","));

            LogoForm2 logoform2 = new LogoForm2();
            _Conection.newPort.Write("SW " + wevelenght1_double.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) + "\r");


            string indata = _Conection.newPort.ReadExisting();

            bool indata_bool = true;
            while (indata_bool == true)
            {
                if (indata.Contains(">"))
                {

                    indata_bool = false;

                }

                else {
                    indata = _Conection.newPort.ReadExisting();
                }
            }


            SWF.Application.OpenForms["LogoFrm2"].Close();
            _Conection.GWNew.Text = string.Format("{0:0.00}", _Conection.wavelength1);
        }
        
    }
}
