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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для DelaytimeDifferential.xaml
    /// </summary>
    public partial class DelaytimeDifferential : Window, IDisposable
    {
        public DelaytimeDifferential()
        {
            InitializeComponent();

            this.pibsec.Tag = (object)"on";
            this.pibmin.Tag = (object)"off";
            this.pibhour.Tag = (object)"off";
        }

        public void Dispose()
        {
            this.Close();
        }

        private void LblValue1_PreviewMouseDown(object sender, EventArgs e)
        {
            using (InputTimerIntervalFrm frm = new InputTimerIntervalFrm())
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
                        this.lblValue1.Content = frm.txtValue.Text + " >";
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
                        this.lblValue1.Content = frm.txtValue.Text + " >";
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

        private void LblValue2_PreviewMouseDown(object sender, EventArgs e)
        {
            using (InputTimerIntervalFrm frm = new InputTimerIntervalFrm())
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
                        this.lblValue2.Content = frm.txtValue.Text + " >";
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
                        this.lblValue2.Content = frm.txtValue.Text + " >";
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
        private void Pibhour_PreviewMouseDown(object sender, EventArgs e)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();


            if (this.pibhour.Tag == (object)"on")
            {
                bi3.UriSource = new Uri("img/UI_DB_Radio_Unselected.png", UriKind.Relative);
                this.pibhour.Source = bi3;
                bi3.EndInit();
                this.pibhour.Tag = (object)"off";
            }
            else
            {
                BitmapImage bi3_ = new BitmapImage();
                bi3_.BeginInit();
                bi3.UriSource = new Uri("img/UI_DB_Radio_Selected.png", UriKind.Relative);
                bi3_.UriSource = new Uri("img/UI_DB_Radio_Unselected.png", UriKind.Relative);
                this.pibhour.Source = bi3;
                this.pibhour.Tag = (object)"on";

                this.pibmin.Source = bi3_;
                this.pibmin.Tag = (object)"off";
                this.pibsec.Source = bi3_;
                this.pibsec.Tag = (object)"off";
                bi3.EndInit();
                bi3_.EndInit();
            }

        }

        private void Pibmin_PreviewMouseDown(object sender, EventArgs e)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();


            if (this.pibmin.Tag == (object)"on")
            {
                bi3.UriSource = new Uri("img/UI_DB_Radio_Unselected.png", UriKind.Relative);
                this.pibmin.Source = bi3;
                bi3.EndInit();
                this.pibmin.Tag = (object)"off";
            }
            else
            {
                BitmapImage bi3_ = new BitmapImage();
                bi3_.BeginInit();
                bi3.UriSource = new Uri("img/UI_DB_Radio_Selected.png", UriKind.Relative);
                bi3_.UriSource = new Uri("img/UI_DB_Radio_Unselected.png", UriKind.Relative);
                this.pibmin.Source = bi3;
                this.pibmin.Tag = (object)"on";

                this.pibhour.Source = bi3_;
                this.pibhour.Tag = (object)"off";
                this.pibsec.Source = bi3_;
                this.pibsec.Tag = (object)"off";
                bi3.EndInit();
                bi3_.EndInit();
            }
        }

        private void Pibsec_PreviewMouseDown(object sender, EventArgs e)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();


            if (this.pibsec.Tag == (object)"on")
            {
                bi3.UriSource = new Uri("img/UI_DB_Radio_Unselected.png", UriKind.Relative);
                this.pibhour.Source = bi3;
                bi3.EndInit();
                this.pibsec.Tag = (object)"off";
            }
            else
            {
                BitmapImage bi3_ = new BitmapImage();
                bi3_.BeginInit();
                bi3.UriSource = new Uri("img/UI_DB_Radio_Selected.png", UriKind.Relative);
                bi3_.UriSource = new Uri("img/UI_DB_Radio_Unselected.png", UriKind.Relative);
                this.pibsec.Source = bi3;
                this.pibsec.Tag = (object)"on";

                this.pibmin.Source = bi3_;
                this.pibmin.Tag = (object)"off";
                this.pibhour.Source = bi3_;
                this.pibhour.Tag = (object)"off";
                bi3.EndInit();
                bi3_.EndInit();
            }
        }

        private void BtnCancel_PreviewMouseDown(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnOK_PreviewMouseDown(object sender, EventArgs e)
        {

        }
    }
}
