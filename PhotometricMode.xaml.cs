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
    /// Логика взаимодействия для PhotometricMode.xaml
    /// </summary>
    public partial class PhotometricMode : Window
    {
        public string photometry_Mode;
        public PhotometricMode()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            this.Close();
        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }

        private void Grid_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            photometry_Mode = "Абсорбция (Abs)";
            this.Close();
            
        }

        private void Grid_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {
            photometry_Mode = "Коэффициент пропускания (%T)";
            this.Close();
            
        }
    }
}
