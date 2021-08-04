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
    /// Логика взаимодействия для MessageBoxOpen.xaml
    /// </summary>
    public partial class MessageBoxOpen : Window, IDisposable
    {
        public MessageBoxOpen()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            
        }

        private void BtnOK_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }
    }
}
