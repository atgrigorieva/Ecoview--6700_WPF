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
    /// Логика взаимодействия для CalibrationMethod.xaml
    /// </summary>
    public partial class CalibrationMethod : Window
    {
        public string calibration_method;
        public string calibration_method_tag;
        public CalibrationMethod(string calibration_method)
        {
            InitializeComponent();
            this.calibration_method = calibration_method;
        }

        private void Label_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           
            calibration_method = ((System.Windows.Controls.ContentControl)sender).Content.ToString();
            calibration_method_tag = ((System.Windows.Controls.ContentControl)sender).Tag.ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            calibration_method = ((System.Windows.Controls.ContentControl)sender).Content.ToString();
            calibration_method_tag = ((System.Windows.Controls.ContentControl)sender).Tag.ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            calibration_method = ((System.Windows.Controls.ContentControl)sender).Content.ToString();
            calibration_method_tag = ((System.Windows.Controls.ContentControl)sender).Tag.ToString();
            this.Close();
            
        }

        public void Dispose()
        {
            
            this.Close();
        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {

        }

        private void Label_PreviewMouseDown(object sender, EventArgs e)
        {

        }
    }
}
