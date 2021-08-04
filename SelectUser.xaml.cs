using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
    /// Логика взаимодействия для SelectUser.xaml
    /// </summary>
    public partial class SelectUser : Window, IDisposable
    {
        public List<string[]> nullabel;
        public List<ComboDataUser> ListData;
        public SelectUser()
        {
            InitializeComponent();
            

        }

        public void Dispose()
        {
            this.Close();
        }

        private void BtnCancel_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOK_PreviewMouseDown(object sender, RoutedEventArgs e)
        {


        }

        string reason = "";
              
        private void TextPasswordUser_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (KeyBoard frm2 = new KeyBoard("", ""))
            {
                // frm2.lbltitle.Text = "reason";
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    reason = frm2.txtValue.Text;
                    this.TextPasswordUser.Text = reason;
                    // CommonFun.Set("DDTime", "0");
                    //   CommonFun.InsertLog("System", "Reset deuterium lamp time" + "," + "Reason" + ":" + reason, true);
                    frm2.Close();

                });
               Convert.ToInt32(frm2.ShowDialog());
            }
  
        }

      

        
    }

    public class ComboDataUser
    {
       
        public string Name { get; set; }
        public string Password { get; set; }
        public ComboDataUser()
        {

        }
    }
}
