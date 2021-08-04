using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для UserManager.xaml
    /// </summary>
    public partial class UserManager : Window
    {
        public List<ListDataUser> ListData;
        public List<ComboDataGroups> ListDataGroups;
        public UserManager()
        {
            InitializeComponent();           

            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {
                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from Users", new object[0]);
                ListData = new List<ListDataUser>();
                while (dataReader.Read())
                {
                   

                    ListData.Add(new ListDataUser { Name = dataReader[0].ToString(), FullName = dataReader[1].ToString(), Password = dataReader[2].ToString(), Groups = dataReader[3].ToString() });
                }
                ListUsers.ItemsSource = ListData;

             
            }
            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {

                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from Groups", new object[0]);
                ListDataGroups = new List<ComboDataGroups>();
                while (dataReader.Read())
                {
                    ListDataGroups.Add(new ComboDataGroups { Name = dataReader[0].ToString() });
                }
                BoxSelectGroup.ItemsSource = ListDataGroups;
            }
        }

        private void btnBack_PreviewMouseDown(object sender, EventArgs e)
        {
            Hide();
            //      CommonFun.WriteLine("Получаем меню");
            MenuProgram menuProgram = new MenuProgram();
            //     CommonFun.WriteLine("Выводим меню");
            menuProgram.Show();
            //    CommonFun.WriteLine("Получаем родительское окно");
            Window parentWindow = Window.GetWindow(this);
            //     CommonFun.WriteLine("Закрываем родительское окно");
            parentWindow.Close();

        }

        private void BtnCreateNewUser_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            using (AddNewUser addUser = new AddNewUser())
            {
                addUser.btnCloseCreateNewUser.PreviewMouseDown += ((param0_1, param1_1) =>
                {
                    ListData.Clear();
                    using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
                    {
                        System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from Users", new object[0]);
                        ListData = new List<ListDataUser>();
                        while (dataReader.Read())
                        {


                            ListData.Add(new ListDataUser { Name = dataReader[0].ToString(), FullName = dataReader[1].ToString(), Password = dataReader[2].ToString(), Groups = dataReader[3].ToString() });
                        }
                        ListUsers.ItemsSource = ListData;
                    }
                    addUser.Close();
                });
                addUser.ShowDialog();
            }
            btnCreateNewUser.Background = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ListUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CommonFun.showbox(ListUsers.SelectedIndex.ToString(), "Info");
            //  CommonFun.showbox(ListData[ListUsers.SelectedIndex].Name.ToString(), "Info");
            if (ListUsers.SelectedIndex != - 1)
            {
                this.TextPassword.Text = (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.Password).ToList()[0].ToString();
                this.BoxSelectGroup.Text = (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.Groups).ToList()[0].ToString();
                this.TextFullName.Text = (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.FullName).ToList()[0].ToString();

                btnReview.IsEnabled = true;

                switch (ListData[ListUsers.SelectedIndex].Name.ToString())
                {
                    case ("Admin"):
                        btnDeleteUser.IsEnabled = false;
                        break;
                    case ("Guest"):
                        btnDeleteUser.IsEnabled = false;
                        break;
                    case ("OP"):
                        btnDeleteUser.IsEnabled = false;
                        break;
                    case ("RE"):
                        btnDeleteUser.IsEnabled = false;
                        break;
                    default:
                        btnDeleteUser.IsEnabled = true;
                        break;
                }
            }
          
        }

        private void BtnReview_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            btnReview.Visibility = Visibility.Hidden;
          //  btnSaveReview.Visibility = Visibility.Visible;
            btnDefaultSet.Visibility = Visibility.Visible;
            btnDeleteUser.Visibility = Visibility.Hidden;
            StackParam.IsEnabled = true;

            if (ListUsers.SelectedIndex != -1)
            {

                switch (ListData[ListUsers.SelectedIndex].Name.ToString())
                {
                    case ("Admin"):
                        BoxSelectGroup.IsEnabled = false;
                        break;
                    case ("Guest"):
                        BoxSelectGroup.IsEnabled = false;
                        break;
                    case ("OP"):
                        BoxSelectGroup.IsEnabled = false;
                        break;
                    case ("RE"):
                        BoxSelectGroup.IsEnabled = false;
                        break;
                    default:
                        BoxSelectGroup.IsEnabled = true;
                        break;
                }
            }
            btnReview.Background = null;
        }
       // bool rewrite = false;
        private void BtnSaveReview_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

            using (SQLiteConnection connection = new SQLiteConnection())
            {
                connection.ConnectionString = "Data Source=" + dataSource;
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {

                    cmd.CommandText = "update Users set C_fullname = :fullname, C_pwd = :pass, C_groups = :groups where C_name = :name";
                    cmd.Parameters.Add("fullname", System.Data.DbType.String).Value = this.TextFullName.Text;
                    cmd.Parameters.Add("pass", System.Data.DbType.String).Value = this.TextPassword.Text;
                    cmd.Parameters.Add("groups", System.Data.DbType.String).Value = this.BoxSelectGroup.Text;
                    cmd.Parameters.Add("name", System.Data.DbType.String).Value = ListData[ListUsers.SelectedIndex].Name.ToString();
                    cmd.ExecuteNonQuery();
                  //  rewrite = true;
                }
            }
            ListData.Clear();
            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {
                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from Users", new object[0]);
                ListData = new List<ListDataUser>();
                while (dataReader.Read())
                {


                    ListData.Add(new ListDataUser { Name = dataReader[0].ToString(), FullName = dataReader[1].ToString(), Password = dataReader[2].ToString(), Groups = dataReader[3].ToString() });
                }
                ListUsers.ItemsSource = ListData;
            }
            btnReview.Visibility = Visibility.Visible;
            btnSaveReview.Visibility = Visibility.Hidden;
            btnDefaultSet.Visibility = Visibility.Hidden;
            btnDeleteUser.Visibility = Visibility.Visible;
            StackParam.IsEnabled = false;
            btnSaveReview.Background = null;
        }

        private void BtnDefaultSet_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            this.TextPassword.Text = (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.Password).ToList()[0].ToString();
            this.BoxSelectGroup.Text = (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.Groups).ToList()[0].ToString();
            this.TextFullName.Text = (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.FullName).ToList()[0].ToString();

            btnReview.Visibility = Visibility.Visible;
            btnSaveReview.Visibility = Visibility.Hidden;
            btnDefaultSet.Visibility = Visibility.Hidden;
            btnDeleteUser.Visibility = Visibility.Visible;
            StackParam.IsEnabled = false;
            btnDefaultSet.Background = null;
        }

        private void TextPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListUsers.SelectedIndex != -1)
            {
                if (this.TextPassword.Text != (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.Password).ToList()[0].ToString())
                    btnSaveReview.Visibility = Visibility.Visible;
                else
                    btnSaveReview.Visibility = Visibility.Hidden;
            }

        }

        private void TextGroup_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListUsers.SelectedIndex != -1)
            {
                if (this.BoxSelectGroup.Text != (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.Groups).ToList()[0].ToString())
                    btnSaveReview.Visibility = Visibility.Visible;
                else
                    btnSaveReview.Visibility = Visibility.Hidden;
            }
        }

        private void TextFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListUsers.SelectedIndex != - 1) {
                if (this.TextFullName.Text != (from user in this.ListData where user.Name == ListData[ListUsers.SelectedIndex].Name.ToString() select user.FullName).ToList()[0].ToString())
                    btnSaveReview.Visibility = Visibility.Visible;
                else
                    btnSaveReview.Visibility = Visibility.Hidden;
            }
        }
        string dataSource = "programdb;Version=3;";
        private void BtnDeleteUser_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

            using (SQLiteConnection connection = new SQLiteConnection())
            {
                connection.ConnectionString = "Data Source=" + dataSource;
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {

                    cmd.CommandText = "delete from Users where C_name = :name";
                    cmd.Parameters.Add("name", System.Data.DbType.String).Value = ListData[ListUsers.SelectedIndex].Name.ToString();
                    cmd.ExecuteNonQuery();
             //       rewrite = true;
                }
            }
            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {
                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from Users", new object[0]);
                ListData = new List<ListDataUser>();
                while (dataReader.Read())
                {


                    ListData.Add(new ListDataUser { Name = dataReader[0].ToString(), FullName = dataReader[1].ToString(), Password = dataReader[2].ToString(), Groups = dataReader[3].ToString() });
                }
                ListUsers.ItemsSource = ListData;
            }

            this.TextPassword.Text = "";
            this.BoxSelectGroup.Text = "";
            this.TextFullName.Text = "";

            btnReview.Visibility = Visibility.Visible;
            btnSaveReview.Visibility = Visibility.Hidden;
            btnDefaultSet.Visibility = Visibility.Hidden;
            btnDeleteUser.Visibility = Visibility.Visible;
            StackParam.IsEnabled = false;
            btnDeleteUser.Background = null;
        }

        private void BtnReviewGroups_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            using (Groups groups = new Groups())
            {
                groups.ShowDialog();
            }
        }
    }

    public class ListDataUser
    {

        public string Name { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Groups { get; set; }
        //public string Groups { get; set; }
        
        public ListDataUser()
        {

        }
    }
    public class ComboDataGroups
    {
        public string Name { get; set; }
        public ComboDataGroups()
        {

        }
    }
}
