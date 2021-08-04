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
    /// Логика взаимодействия для CountMeasure.xaml
    /// </summary>
    public partial class CountMeasure : Window
    {
        public string count_measure;
        public CountMeasure(string count_measure)
        {
            InitializeComponent();
            this.count_measure = count_measure;
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
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_10(object sender, RoutedEventArgs e)
        {
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_20(object sender, RoutedEventArgs e)
        {
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_30(object sender, RoutedEventArgs e)
        {
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_50(object sender, RoutedEventArgs e)
        {
            count_measure = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown(object sender, EventArgs e)
        {

        }
    }
}
