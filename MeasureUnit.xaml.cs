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
    /// Логика взаимодействия для MeasureUnit.xaml
    /// </summary>
    public partial class MeasureUnit : Window
    {
        public string measureunit;
        public MeasureUnit(string measureunit)
        {
            InitializeComponent();
            this.measureunit = measureunit;
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
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_4(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            

        }

        private void Label_PreviewMouseDown_5(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_6(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_7(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_8(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_9(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_10(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_11(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_12(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_13(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_14(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_15(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_16(object sender, RoutedEventArgs e)
        {
            measureunit = (((System.Windows.Controls.ContentControl)sender).Content).ToString();
            this.Close();
            
        }

    }
}
