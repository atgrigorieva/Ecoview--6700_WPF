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
    /// Логика взаимодействия для SetAccuracy.xaml
    /// </summary>
    public partial class SetAccuracy : Window, IDisposable
    {
        public SetAccuracy()
        {
            InitializeComponent();
        }

        private void Abs_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using(ModidyDisplayAccuracy frm = new ModidyDisplayAccuracy())
            {
                frm.Loaded += (RoutedEventHandler)((param0, param1) =>
                {
                    frm.acurracy = Abs.Content.ToString();
                });
                frm.Closed += (EventHandler)((param0, param1) =>
                {
                    Abs.Content = frm.acurracy;
                });
                
                int num = Convert.ToInt32(frm.ShowDialog());
            }

        }

        private void LblT_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (ModidyDisplayAccuracy frm = new ModidyDisplayAccuracy())
            {
                frm.Loaded += (RoutedEventHandler)((param0, param1) =>
                {
                    frm.acurracy = T.Content.ToString();
                });

                frm.Closed += (EventHandler)((param0, param1) =>
                {
                    T.Content = frm.acurracy;
                });
                int num = Convert.ToInt32(frm.ShowDialog());
            }
        }

        private void LblConc_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (ModidyDisplayAccuracy frm = new ModidyDisplayAccuracy())
            {
                frm.Loaded += (RoutedEventHandler)((param0, param1) =>
                {
                    frm.acurracy = Conc.Content.ToString();
                });

                frm.Closed += (EventHandler)((param0, param1) =>
                {
                    Conc.Content = frm.acurracy;
                });
                int num = Convert.ToInt32(frm.ShowDialog());
            }
        }

        public void Dispose()
        {
            
            this.Close();
        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           // this.Close();
          //  
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
