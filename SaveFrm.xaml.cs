using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для SaveFrm.xaml
    /// </summary>
    public partial class SaveFrm : Window, IDisposable
    {
        public string save_name;
        public string extension;
        public SaveFrm(string extension, string name)
        {
            InitializeComponent();
            this.extension = extension;
            Name_file.Text = name;
        }

        public void Dispose()
        {
            
            this.Close();
        }
        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
           /* string filepath = Directory.GetCurrentDirectory() + @"\Сохраненные измерения\";
            if (!File.Exists(filepath + Name_file.Text + extension))
            {
                save_name = Name_file.Text;
                
            }
            else
            {
                CommonFun.showbox("Файл с таким именем уже существует.", "Info");
            }*/
        }

        private void Name_file_GotFocus(object sender, RoutedEventArgs e)
        {
            /*KeyBoard keyBoard = new KeyBoard("Ввод имени файла", Name_file.Text);
            keyBoard.ShowDialog();
            (sender as TextBox).Text = keyBoard.text_string;*/
            using (KeyBoard frm2 = new KeyBoard("", ""))
            {
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                // frm2.lbltitle.Text = "reason";
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    (sender as TextBox).Text = frm2.txtValue.Text;
                    // CommonFun.Set("DDTime", "0");
                    //   CommonFun.InsertLog("System", "Reset deuterium lamp time" + "," + "Reason" + ":" + reason, true);
                    frm2.Close();

                });
                Convert.ToInt32(frm2.ShowDialog());
            }
        }

        private void Name_file_GotFocus_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnOK_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }
    }
}
