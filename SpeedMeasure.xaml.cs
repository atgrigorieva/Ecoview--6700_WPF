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
    /// Логика взаимодействия для SpeedMeasure.xaml
    /// </summary>
    public partial class SpeedMeasure : Window
    {
        public string speed_measure;
        public SpeedMeasure(string speed_measure)
        {
            InitializeComponent();
            this.speed_measure = speed_measure;
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
            speed_measure = Hight.Content.ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            speed_measure = Medium.Content.ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            speed_measure = Low.Content.ToString();
            this.Close();
            
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            speed_measure = AccuracyMost.Content.ToString();
            this.Close();
            
        }

        

    }
}


