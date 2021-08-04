using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Windows.Threading;
using Binding = System.Windows.Data.Binding;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Drawing;
using Brush = System.Drawing.Brush;
using SystemColors = System.Drawing.SystemColors;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Point = System.Drawing.Point;
using Control = System.Windows.Forms.Control;
using System.ComponentModel;
using Label = System.Windows.Forms.Label;
using FontStyle = System.Drawing.FontStyle;

using Image = System.Drawing.Image;
using DashStyle = System.Drawing.Drawing2D.DashStyle;
using MessageBox = System.Windows.Forms.MessageBox;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;
using TextBox = System.Windows.Controls.TextBox;
using System.IO;
using System.Collections.ObjectModel;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для OpenFrm.xaml
    /// </summary>
    public partial class OpenFrm : Window
    {
        public string open_name;
        public string path;
        public string extension;

        DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
        private DataGridViewTextBoxColumn ColName;
        private DataGridViewTextBoxColumn ColDate;

        public OpenFrm(string path, string extension)
        {
            InitializeComponent();

            DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
            this.ColName = new DataGridViewTextBoxColumn();
            this.ColDate = new DataGridViewTextBoxColumn();

            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      //      gridViewCellStyle1.BackColor = SystemColors.Control;
            gridViewCellStyle1.Font = new Font("Segoe UI", 16F);
        //    gridViewCellStyle1.ForeColor = SystemColors.WindowText;
          //  gridViewCellStyle1.SelectionBackColor = 0;
            gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 50;

            gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle2.BackColor = SystemColors.Window;
            gridViewCellStyle2.Font = new Font("Segoe UI", 16F);
            gridViewCellStyle2.ForeColor = SystemColors.ControlText;
            gridViewCellStyle2.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
            gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = gridViewCellStyle2;

            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";

            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.ColName.Name = "ColName";
            this.ColDate.Name = "ColDate";

            this.dataGridView1.Columns.AddRange( (DataGridViewColumn)this.ColName, (DataGridViewColumn)this.ColDate);

            this.dataGridView1.Columns[0].ReadOnly = true;
            this.dataGridView1.Columns[1].ReadOnly = true;

            this.dataGridView1.Columns[0].HeaderText = "Наименование";
            this.dataGridView1.Columns[1].HeaderText = "Дата изменения";

            this.dataGridView1.Visible = true;
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView1.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular);


            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);


            this.extension = extension;
            this.path = path;

            DirectoryInfo info = new DirectoryInfo(path);
            string pattern = "*" + this.extension;
            FileInfo[] files = info.GetFiles(pattern).OrderBy(p => p.CreationTime).ToArray();

            foreach (FileInfo f in files)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = f.Name.ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = f.CreationTime.ToString("dd.MM.yyyy");
            }
               // MyTableList.Add(new MyTableFiles(f.Name, f.CreationTime.ToString("dd.MM.yyyy"), false));

          ///  meisureGrid.ItemsSource = MyTableList;
            FileName.Content = null;
           // meisureGrid.Focus();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FileName.Content = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
        }

        private readonly ObservableCollection<MyTableFiles> _itemList = new ObservableCollection<MyTableFiles>();
        public ObservableCollection<MyTableFiles> MyTableList { get { return _itemList; } }

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
            if (FileName.Content != null)
            {
                /*string message = "Внимание!\nДанная операция сотрет ваши данные измерения. Продолжить?";
                string caption = "Открыть документ";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    open_name = FileName.Content.ToString();
                    
                }
                else
                {
                    this.Close();
                    
                }*/

                using(MessageBoxOpen messageBoxOpen = new MessageBoxOpen())
                {
                    messageBoxOpen.btnCancel.PreviewMouseDown += ((param0_1, param1_1) => 
                    {
                        messageBoxOpen.Close();
                        this.Close();
                        
                    });
                    messageBoxOpen.btnOK.PreviewMouseDown += ((param0_1, param1_1) =>
                    {
                        open_name = FileName.Content.ToString();
                        messageBoxOpen.Close();
                        this.Close();
                        
                    });

                    messageBoxOpen.ShowDialog();
                }
            }
            else
            {
                CommonFun.showbox("Вы не выбрали файл!", "Warning");
            }
        }
       

        private void meisureGrid_StylusUp(object sender, StylusEventArgs e)
        {
            //FileName.Content = ((System.Windows.Controls.TextBlock)e.OriginalSource).Text;
            FileName.Content = (sender as TextBox).Text;
        }

       /* private void meisureGrid_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
           
            var cellinfo = meisureGrid.SelectedCells[0];
            FileName.Content = (cellinfo.Column.GetCellContent(cellinfo.Item) as TextBlock).Text;
        }

        private object meisureGrid_PreviewMouseDown_()
        {
            return meisureGrid.SelectedItem;
        }

        private void meisureGrid_StylusUp_1(object sender, StylusEventArgs e)
        {
            if (meisureGrid.SelectedItem != null)
            {
                FileName.Content = (meisureGrid.Columns[0].GetCellContent(meisureGrid.SelectedItem) as TextBlock).Text;
            }
        }

        private void MeisureGrid_PreviewMouseDown(object sender, EventArgs e)
        {
            FileName.Content = (meisureGrid.Columns[0].GetCellContent(meisureGrid.SelectedItem) as TextBlock).Text;
        }

        private void MeisureGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (meisureGrid.SelectedItem != null)
            {
                FileName.Content = ((UVStudio.MyTableFiles)meisureGrid.SelectedItem).Name;
            }
        }

        private void meisureGrid_PreviewMouseDown(object sender, EventArgs e)
        {
            if (meisureGrid.SelectedItem != null)
            {
                FileName.Content = ((UVStudio.MyTableFiles)meisureGrid.SelectedItem).Name;
            }
        }*/
    }

    public class MyTableFiles
    {
        public MyTableFiles() { }
        public MyTableFiles(List<object> array)
        {
            this.Name = Convert.ToString(array[0]);
            this.DateTime_ = Convert.ToString(array[1]);
            this.BooleanFlag = false;
        }
        public MyTableFiles(string Name, string DateTime_, bool BooleanFlag)
        {
            this.Name = Name;
            this.DateTime_ = DateTime_;
            this.BooleanFlag = BooleanFlag;
        }

        private string _value_name;
        public string Name { get { return _value_name; } set { _value_name = value; } }       
        private string _value_datetime;
        public string DateTime_ { get { return _value_datetime; } set { _value_datetime = value; } }
        private bool _value_checkrow;
        public bool BooleanFlag { get { return _value_checkrow; } set { _value_checkrow = value; } }
    }

}
