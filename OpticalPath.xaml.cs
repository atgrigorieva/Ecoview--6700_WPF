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
    /// Логика взаимодействия для OpticalPath.xaml
    /// </summary>
    public partial class OpticalPath : Window
    {
        public string optical_path;
        public OpticalPath(string optical_path)
        {
            InitializeComponent();
            this.optical_path = optical_path;
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
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_10(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_20(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_30(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_50(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }
        private void Label_PreviewMouseDown_100(object sender, RoutedEventArgs e)
        {
            optical_path = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }
    }
}
