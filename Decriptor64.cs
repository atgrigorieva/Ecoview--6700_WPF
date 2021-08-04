using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UVStudio
{
    class Decriptor64
    {
        string filename, pathname;
        bool shifrTrueFalse;
        public Decriptor64(string filename1, string pathname1, ref bool shifrTrueFalse1)
        {
            this.filename = filename1;
            this.pathname = pathname1;
            this.shifrTrueFalse = shifrTrueFalse1;
            FileDecriptor64(filename);
            filename1 = this.filename;
            shifrTrueFalse1 = this.shifrTrueFalse;

        }
        public void FileDecriptor64(string filename)
        {
            //  FileStream fstreamWrite = new FileStream(pathname + "/" + filename, FileMode.OpenOrCreate);
            StreamReader fstreamRead = new StreamReader(@filename);
            string fileread = "";

            while (true)
            {
                // Читаем строку из файла во временную переменную.
                string temp = fstreamRead.ReadLine();

                // Если достигнут конец файла, прерываем считывание.
                if (temp == null) break;

                // Пишем считанную строку в итоговую переменную.
                fileread += temp;
            }
            if (fileread.Contains("xml"))
            {

                shifrTrueFalse = false;
                return;
            }
            else
            {
                //  string input = "SGVsbG8sIFdvcmxk";
                byte[] buffer = Convert.FromBase64String(fileread);
                string text = Encoding.UTF8.GetString(buffer);
                fstreamRead.Close();
                // fstreamWrite.Close();
                filename = filename.Substring(filename.LastIndexOf(@"/") + 1);
                File.WriteAllText(pathname + "/" + filename, text, Encoding.UTF8);
                shifrTrueFalse = true;
                // return shifrTrueFalse;
            }
        }
    }
}
