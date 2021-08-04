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
    /// Логика взаимодействия для ModidyDisplayAccuracy.xaml
    /// </summary>
    public partial class ModidyDisplayAccuracy : Window, IDisposable
    {
        public string acurracy = "";
        public ModidyDisplayAccuracy()
        {
            InitializeComponent();
        }

        private void Label_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            acurracy = "0";
            this.Close();
        }

        private void Label_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            acurracy = "0.0";
            this.Close();
        }

        private void Label_PreviewMouseDown_2(object sender, RoutedEventArgs e)
        {
            acurracy = "0.00";
            this.Close();
        }

        private void Label_PreviewMouseDown_3(object sender, RoutedEventArgs e)
        {
            acurracy = "0.000";
            this.Close();
        }

        private void Label_PreviewMouseDown_4(object sender, RoutedEventArgs e)
        {
            acurracy = "0.0000";
            this.Close();
        }

        private void Label_PreviewMouseDown_5(object sender, RoutedEventArgs e)
        {
            acurracy = "0.00000";
            this.Close();
        }

        private void Label_PreviewMouseDown_6(object sender, RoutedEventArgs e)
        {
            acurracy = "0.000000";
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
    }
}
