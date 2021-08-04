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
    /// Логика взаимодействия для AddGroup.xaml
    /// </summary>
    public partial class AddGroup : Window, IDisposable
    {
        public AddGroup()
        {
            InitializeComponent();
        }

        private void TextName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextName_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            using (KeyBoard frm2 = new KeyBoard("", ""))
            {
                frm2.Loaded += (RoutedEventHandler)((param2_1, param2_2) => {
                    frm2.Activate();
                });
                frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    this.TextName.Text = frm2.txtValue.Text;

                    frm2.Close();

                });
                int num = Convert.ToInt32(frm2.ShowDialog());
            }
        }

        public void Dispose()
        {
            
        }
        string dataSource = "programdb.db;Version=3;";
        private void BtnCreateNewGroup_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TextName.Text != "")
            {
                List<string> GroupUnic = new List<string>();
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = "Data Source=" + dataSource;
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        
                        /*IDataReader dataReaderGroup = SQLiteHelper.ExecuteReader(new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")), "select distinct C_Groups from GroupRights where C_groups = '@group'", new object[1]
                            {this.TextName.Text});*/
                        System.Data.IDataReader dataReader_ = SQLiteHelper.ExecuteReader(cmd, "select distinct C_groups from GroupRights where C_groups = @group", new object[1] { TextName.Text.ToString() });

                        while (dataReader_.Read())
                        {
                            GroupUnic.Add(dataReader_[0].ToString());
                           
                        }
                        dataReader_.Close();
                    }
                }
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = "Data Source=" + dataSource;
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {

                        if (GroupUnic.Count == 0)
                        {
                            connection.Close();
                            connection.Open();

                            /*cmd.CommandText = "update Users set C_fullname = :fullname, C_pwd = :pass, C_groups = :groups where C_name = :name";
                            cmd.Parameters.Add("fullname", System.Data.DbType.String).Value = this.TextFullName.Text;
                            cmd.Parameters.Add("pass", System.Data.DbType.String).Value = this.TextPassword.Text;
                            cmd.Parameters.Add("groups", System.Data.DbType.String).Value = this.BoxSelectGroup.Text;
                            cmd.Parameters.Add("name", System.Data.DbType.String).Value = ListData[ListUsers.SelectedIndex].Name.ToString();
                            cmd.ExecuteNonQuery();*/
                            //  rewrite = true;
                            if (CheckCreateRights.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups = '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, CheckCreateRights.Content.ToString() });

                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, CheckCreateRights.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }
                                dataReader.Close();

                            }
                           
                            if (SettingRights.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups =  '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, SettingRights.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, SettingRights.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }
                                dataReader.Close();

                            }
                            
                            if (ModificationSetting.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups =  '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, ModificationSetting.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, ModificationSetting.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }
                                dataReader.Close();

                            }
                            
                            if (CreateGradAndMethodts.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups =  '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, CreateGradAndMethodts.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, CreateGradAndMethodts.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }
                                dataReader.Close();

                            }
                           
                            if (ModificationGradAndMethodts.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups = '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, ModificationGradAndMethodts.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, ModificationGradAndMethodts.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }
                                dataReader.Close();

                            }
                           
                            if (Blank.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups =  '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, Blank.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, Blank.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }
                                dataReader.Close();

                            }
                        
                            if (ViewFiles.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups = '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, ViewFiles.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, ViewFiles.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }

                                dataReader.Close();
                            }
                           
                            if (InsertResults.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups = '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, InsertResults.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, InsertResults.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }

                                dataReader.Close();
                            }
                            

                            if (PrintProtocol.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups = '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, PrintProtocol.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, PrintProtocol.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }

                                dataReader.Close();
                            }
                           
                            if (CreateUsers.IsChecked == true)
                            {
                                System.Data.IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select * from GroupRights where C_groups =  '@group' and C_rights = '@rights'", new object[2] { this.TextName.Text, CreateUsers.Content.ToString() });
                                if (!dataReader.Read())
                                {
                                    using (SQLiteConnection connection_ = new SQLiteConnection("Data Source=" + dataSource))
                                        SQLiteHelper.ExecuteNonQuery(connection_, "insert into GroupRights (C_groups,C_rights) values (@C_groups,@C_rights)", this.TextName.Text, CreateUsers.Content.ToString());
                                    //lblNotUser.Content = "Добавлено!";
                                }

                                dataReader.Close();
                            }
                           

                            lblNotGroup.Content = "Добавлено!";


                        }
                        else
                        {
                            lblNotGroup.Content = "Пользователь с таким логином уже существует!";
                            return;
                        }
                    }
                }
            }

            else
            {
                lblNotGroup.Content = "Логин пользователя не заполнен!";
                return;
            }
            btnCreateNewGroup.Background = null;


        }

        private void BtnCloseCreateNewGroup_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CheckCreateRights_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SettingRights_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ModificationSetting_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CreateGradAndMethodts_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ModificationGradAndMethodts_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Blank_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ViewFiles_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void InsertResults_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PrintProtocol_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CreateUsers_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CreateUsers_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void PrintProtocol_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void InsertResults_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Blank_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
