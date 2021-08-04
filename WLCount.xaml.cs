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
    /// Логика взаимодействия для WLCount.xaml
    /// </summary>
    public partial class WLCount : Window
    {
        public string count_wl;
        public WLCount(string wl_mass)
        {
            InitializeComponent();
            this.count_wl = wl_mass;
        }
        public void Dispose()
        {
            this.Close();
        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }

        private void Label_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_4(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_5(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_6(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_7(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_8(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_9(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_10(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_11(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_12(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_13(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_14(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_15(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_16(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_17(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_18(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_19(object sender, RoutedEventArgs e)
        {
            count_wl = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }
    }
}
