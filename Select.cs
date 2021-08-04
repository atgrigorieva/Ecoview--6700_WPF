using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ecoview_Normal
{
    public partial class Select : Form
    {
        bool searchKeyNotFound = false;
        public Select()
        {
            InitializeComponent();
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                string versionURL = "https://ecoview.ru/ecoview-version/version-normal";
                if (label1.Text.Substring(16) == wc.DownloadString(versionURL))
                {

                    label1.Text = "";
                    label1.ForeColor = Color.Black;
                }
                else
                {
                    label1.Text = "Внимание! Доступна новая версия " + wc.DownloadString(versionURL);
                    label1.ForeColor = Color.Red;
                }
                label1.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Italic);
                //label1.Location = new Point(204, 115);

            }

            catch
            {
                label1.Text = "";
            }

        }
        bool click = false;
        int selet_rezim;
        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                selet_rezim = 1;
                
            }
            else
            {
                if(radioButton2.Checked == true)
                {
                    selet_rezim = 2;
                }
                else
                {
                    if(radioButton3.Checked == true)
                    {
                        selet_rezim = 3;
                    }
                    else
                    {
                        if(radioButton4.Checked == true)
                        {
                            selet_rezim = 4;
                        }
                        else
                        {
                            if(radioButton5.Checked == true)
                            {
                                selet_rezim = 9;
                            }
                        }
                    }
                }
            }
            if (searchKeyNotFound == false)
            {
                Hide();
                Ecoview f2 = new Ecoview(selet_rezim);
                f2.ShowDialog();
                this.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            click = false;
            Application.Exit();
        }

        private void Select_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (click != true)
            {
                
                System.Windows.Forms.Application.ExitThread();

            }
        }

        private void Select_Load(object sender, EventArgs e)
        {
            // 1. Получаем размер папки

            string pathToDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            //MessageBox.Show(path);
            double catalogSize = 0;
            string fileControl = pathToDirectory + "/Ecoview Normal.exe";
            FileInfo file = new FileInfo(fileControl);

            // Последний размер файла программы 0.0028972625732421875

            catalogSize = (Convert.ToDouble(file.Length) / 1024 / 1024 / 1024);

            /*if (catalogSize != 0.0028972625732421875)
            {
                MessageBox.Show("Программа не может быть запущена! Проверьте папку установки программы!");
                System.Environment.Exit(0);
            }*/


            // 2. Проверка хеш суммы MD5

            string md5Hash = "";

            using (var md5 = MD5.Create())
            {
                byte[] contentBytes = File.ReadAllBytes(fileControl);

                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);


                //Handles empty filePaths case
                md5.TransformFinalBlock(new byte[0], 0, 0);

                md5Hash  = BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();

            }

            // Последний md5 хеш =  "9578ea57c8396c6bd24724ef18ad12e5"
            string md5Hash_old = File.ReadAllText(pathToDirectory + "/key");            
            
            string decode_md5Hash_old = Encoding.UTF8.GetString(Convert.FromBase64String(md5Hash_old));



           if (md5Hash != decode_md5Hash_old)
            {
                    MessageBox.Show("Программа не может быть запущена! Проверьте папку установки программы! Хеш сумма не совпадает!");
                    System.Environment.Exit(0);
            }
        
            
        }

        static double sizeOfFolder(string folder, ref double catalogSize)
        {
            try
            {
                //В переменную catalogSize будем записывать размеры всех файлов, с каждым
                //новым файлом перезаписывая данную переменную
                DirectoryInfo di = new DirectoryInfo(folder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles();
                //В цикле пробегаемся по всем файлам директории di и складываем их размеры
                foreach (FileInfo f in fi)
                {
                    //Записываем размер файла в байтах
                    catalogSize = catalogSize + f.Length;
                }
                //В цикле пробегаемся по всем вложенным директориям директории di 
                foreach (DirectoryInfo df in diA)
                {
                    //рекурсивно вызываем наш метод
                    sizeOfFolder(df.FullName, ref catalogSize);
                }
                //1ГБ = 1024 Байта * 1024 КБайта * 1024 МБайта
                return (catalogSize / 1024 / 1024 / 1024);
            }
            //Начинаем перехватывать ошибки
            //DirectoryNotFoundException - директория не найдена
            catch (DirectoryNotFoundException ex)
            {
               MessageBox.Show("Директория не найдена. Ошибка: " + ex.Message);
                return 0;
            }
            //UnauthorizedAccessException - отсутствует доступ к файлу или папке
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Отсутствует доступ. Ошибка: " + ex.Message);
                return 0;
            }
            //Во всех остальных случаях
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка. Обратитесь к администратору. Ошибка: " + ex.Message);
                return 0;
            }
        }

        public static string CreateDirectoryMd5(string srcPath)
        {
            
            var filePaths = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories).OrderBy(p => p).ToArray();

            using (var md5 = MD5.Create())
            {
                foreach (var filePath in filePaths)
                {
                    // hash path
                    byte[] pathBytes = Encoding.UTF8.GetBytes(filePath);
                    md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                    // hash contents
                    byte[] contentBytes = File.ReadAllBytes(filePath);

                    md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                }

                //Handles empty filePaths case
                md5.TransformFinalBlock(new byte[0], 0, 0);

                return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
            }
        }
    }
}
