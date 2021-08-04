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

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для QuaInputFrm.xaml
    /// </summary>
    public partial class QuaInputFrm : Window, IDisposable
    {
        public QuaInputFrm()
        {
            InitializeComponent();
        }

        private void Grid_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblMax.Content = frm.txtValue.Text + " >";
                        frm.Close();
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
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblMax.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void Grid_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblMin.Content = frm.txtValue.Text + " >";
                        frm.Close();
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
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblMin.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void Grid_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblv2.Content = frm.txtValue.Text + " >";
                        frm.Close();
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
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblv2.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void Grid_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                frm.txtValue.KeyDown += (KeyEventHandler)((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblv3.Content = frm.txtValue.Text + " >";
                        frm.Close();
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
                        if (frm.txtValue.Text.IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text;
                        Convert.ToDecimal(frm.txtValue.Text);
                        this.lblv3.Content = frm.txtValue.Text + " >";
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void BtnOK_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~QuaInputFrm() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
