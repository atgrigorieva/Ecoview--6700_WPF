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
    /// Логика взаимодействия для DNAMode.xaml
    /// </summary>
    public partial class DNAMode : Window
    {
        public string dnaMode = "";
        public DNAMode()
        {
            InitializeComponent();
        }

        private void Dna1_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            dnaMode = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            this.Close();
        }

        private void Dna2_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            dnaMode = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            this.Close();
        }

        private void Lowery_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            dnaMode = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            this.Close();
        }

        private void Uv_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            dnaMode = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            this.Close();
        }

        private void Bca_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            dnaMode = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            this.Close();
        }

        private void Cbb_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            dnaMode = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            this.Close();
        }

        private void Biuret_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            dnaMode = ((System.Windows.FrameworkElement)sender).Tag.ToString();
            this.Close();
        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
