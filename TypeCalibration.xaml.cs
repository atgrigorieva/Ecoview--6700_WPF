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
    /// Логика взаимодействия для TypeCalibration.xaml
    /// </summary>
    public partial class TypeCalibration : Window, IDisposable
    {
        public int type_calibration = -1;
        public TypeCalibration()
        {
            InitializeComponent();
        }
        public void Dispose() { }
        private void Label_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            type_calibration = 0;
            this.Close();
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            type_calibration = 1;
            this.Close();
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            type_calibration = 2;
            this.Close();
        }



        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }

        private void Lblwlcorrection_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Lblbaselinecorrection_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Lbldcurrentcorrection_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }
    }
}
