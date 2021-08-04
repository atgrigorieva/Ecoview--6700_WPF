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
    /// Логика взаимодействия для StepInterval.xaml
    /// </summary>
    public partial class StepInterval : Window
    {
        public string step_interval;
        public StepInterval(string step_interval)
        {
            InitializeComponent();
            this.step_interval = step_interval;
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
            step_interval = 0.1.ToString("f1");
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            step_interval = 0.2.ToString("f1");
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            step_interval = 0.5.ToString("f1");
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            step_interval = 1.0.ToString("f1");
            this.Close();
            
        }

        private void Label_PreviewMouseDown_4(object sender, RoutedEventArgs e)
        {
            step_interval = 2.0.ToString("f1");
            this.Close();
            
        }

        private void Label_PreviewMouseDown_5(object sender, RoutedEventArgs e)
        {
            step_interval = 5.0.ToString("f1");
            this.Close();
            
        }

    }
}
