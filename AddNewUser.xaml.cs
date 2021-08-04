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
    /// Логика взаимодействия для AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Window, IDisposable
    {
        public List<ComboDataGroup> ListData;
        public AddNewUser()
        {
            InitializeComponent();

            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {
                IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select distinct C_Groups from GroupRights", new object[0]);
                this.ListData = new List<ComboDataGroup>();
                while (dataReader.Read())
                {
                    this.ListData.Add(new ComboDataGroup { Name = dataReader[0].ToString()});
                }
                this.BoxSelectGroup.ItemsSource = this.ListData;

            }
        }

        private void BtnCloseCreateNewUser_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           // btnCloseCreateNewUser.Background = null;
        }

        private void BtnCreateNewUser_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if (TextLogin.Text != "") {
                List<string> UserUnic = new List<string>();
                IDataReader dataReader = SQLiteHelper.ExecuteReader(new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")), "select C_name from Users where C_name =@login", new object[1]
                    {
                    this.TextLogin.Text
                    });
                while (dataReader.Read())
                {
                    UserUnic.Add(dataReader["C_name"].ToString());
                }
                if (UserUnic.Count == 0)
                {
                    if (TextPassword.Text == "")
                    {
                        lblNotUser.Content = "Пароль не может быть пустым!";

                    }
                    else
                    {
                        if (BoxSelectGroup.Text == "")
                        {
                            lblNotUser.Content = "Группа пользователя не была выбрана!";
                            return;
                        }
                        else
                        {
                            SQLiteHelper.ExecuteNonQuery("Data Source=programdb.db;Version=3;", "insert into Users (C_name,C_fullname,C_pwd,C_groups,D_createtime) values (@C_name,@C_fullname,@C_pwd,@C_groups,@C_time)", TextLogin.Text.ToString(), TextFullName.Text.ToString(), TextPassword.Text.ToString(), BoxSelectGroup.Text.ToString(), (object)DateTime.Now);
                            lblNotUser.Content = "Добавлено!";
                        }
                    }
                }
                else
                {
                    lblNotUser.Content = "Пользователь с таким логином уже существует!";
                    return;
                }
            }
            else
            {
                lblNotUser.Content = "Логин пользователя не заполнен!";
                return;
            }
            btnCreateNewUser.Background = null;
        }
        

        private void TextLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TextFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TextPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TextLogin_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (KeyBoard frm2 = new KeyBoard("", ""))
            {
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    this.TextLogin.Text = frm2.txtValue.Text;

                    frm2.Close();

                });
                int num = Convert.ToInt32(frm2.ShowDialog());
            }
        }

        private void TextFullName_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (KeyBoard frm2 = new KeyBoard("", ""))
            {
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    this.TextFullName.Text = frm2.txtValue.Text;

                    frm2.Close();

                });
                int num = Convert.ToInt32(frm2.ShowDialog());
            }
        }

        private void TextPassword_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (KeyBoard frm2 = new KeyBoard("", ""))
            {
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    this.TextPassword.Text = frm2.txtValue.Text;

                    frm2.Close();

                });
                int num = Convert.ToInt32(frm2.ShowDialog());
            }
        }

        public void Dispose()
        {
            
        }

     
    }

    public class ComboDataGroup
    {
        public string Name { get; set; }
        public ComboDataGroup()
        {

        }
    }
}
