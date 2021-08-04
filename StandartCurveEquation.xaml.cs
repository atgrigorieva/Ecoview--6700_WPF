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
    /// Логика взаимодействия для StandartCurveEquation.xaml
    /// </summary>
    public partial class StandartCurveEquation : Window
    {
        public string curveEquation;
        public StandartCurveEquation(string curveEquation)
        {
            InitializeComponent();
            this.curveEquation = curveEquation;
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
            curveEquation = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            curveEquation = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

    }
}
