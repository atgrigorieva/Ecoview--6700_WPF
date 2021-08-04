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
    /// Логика взаимодействия для MeasureMethod.xaml
    /// </summary>
    public partial class MeasureMethod : Window
    {
        public string measure_method;
        public MeasureMethod(string measure_method)
        {
            InitializeComponent();
            this.measure_method = measure_method;
        }

        public void Dispose()
        {
            this.Close();
        }

        private void Label_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            //optical_path = 1;
            measure_method = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            //   optical_path = 2;
            measure_method = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            //  optical_path = 3;
            measure_method = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            //  optical_path = 5;
            measure_method = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }
        private void btnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }

       
    }
}
