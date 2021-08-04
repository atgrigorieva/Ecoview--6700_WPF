using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Color = System.Drawing.Color;
using SystemColors = System.Drawing.SystemColors;

namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для CurrSampleFrm.xaml
    /// </summary>
    public partial class CurrSampleFrm : Window, IDisposable
    {
        private string lanvalue;
        private int dgvcnt = 0;
        private List<SpectraScan> SList = new List<SpectraScan>();
        private int MulShow = 0;
        //public DataGridView dataGridView1;
        private ColorDialog colorDialog1;
        //private DataGridViewTextBoxColumn ColStatus;
        private DataGridViewTextBoxColumn ColName;
        private DataGridViewTextBoxColumn ColColor;
        private DataGridViewImageColumn ColOP;
        private DataGridViewImageColumn ColOP1;
        public CurrSampleFrm(List<SpectraScan> list, int mulshow)
        {
            InitializeComponent();
            this.SList = list;
            this.MulShow = mulshow;


            ///this.ListBind();
        }

        private void CurrSampleFrm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex != -1)
            {
                if (this.dataGridView1.Rows[rowIndex].Tag == null)
                    return;
                /*if (e.ColumnIndex == 2)
                {
                    colorDialog1 = new ColorDialog();
                    colorDialog1.AllowFullOpen = false;
                    // Allows the user to get help. (The default is false.)
                    colorDialog1.ShowHelp = true;
                    if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColColor"].Style = new DataGridViewCellStyle()
                        {
                            BackColor = this.colorDialog1.Color
                        };
                        this.SList[rowIndex].color = this.colorDialog1.Color;
                    }
                    this.dataGridView1.ClearSelection();
                }
                else if (e.ColumnIndex == 3)
                {
                    if (this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag == (object)"off")
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Value = (object)Properties.Resources.UI_DB_Check_Checked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag = (object)"on";
                        this.SList[rowIndex].IsShow = 1;
                    }
                    else
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Value = (object)Properties.Resources.UI_DB_Check_Unchecked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag = (object)"off";
                        this.SList[rowIndex].IsShow = 0;
                    }
                }
                else
                {
                    if (e.ColumnIndex != 4)
                        return;
                    if (this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Tag == (object)"off")
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Value = (object)Properties.Resources.UI_DB_Check_Checked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Tag = (object)"on";
                    }
                    else
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Value = (object)Properties.Resources.UI_DB_Check_Unchecked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Tag = (object)"off";
                    }
                }*/
                if (e.ColumnIndex == 1)
                {
                    colorDialog1 = new ColorDialog();
                    colorDialog1.AllowFullOpen = false;
                    // Allows the user to get help. (The default is false.)
                    colorDialog1.ShowHelp = true;
                    if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColColor"].Style = new DataGridViewCellStyle()
                        {
                            BackColor = this.colorDialog1.Color
                        };
                        this.SList[rowIndex].color = this.colorDialog1.Color;
                    }
                    this.dataGridView1.ClearSelection();
                }
                else if (e.ColumnIndex == 2)
                {
                    if (this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag == (object)"off")
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Value = (object)Properties.Resources.UI_DB_Check_Checked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag = (object)"on";
                        this.SList[rowIndex].IsShow = 1;
                    }
                    else
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Value = (object)Properties.Resources.UI_DB_Check_Unchecked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag = (object)"off";
                        this.SList[rowIndex].IsShow = 0;
                    }
                }
                else
                {
                    if (e.ColumnIndex != 3)
                        return;
                    if (this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Tag == (object)"off")
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Value = (object)Properties.Resources.UI_DB_Check_Checked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Tag = (object)"on";
                    }
                    else
                    {
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Value = (object)Properties.Resources.UI_DB_Check_Unchecked;
                        this.dataGridView1.Rows[rowIndex].Cells["ColOP1"].Tag = (object)"off";
                    }
                }
            }
        }


        public void ListBind()
        {
            this.dataGridView1.Rows.Clear();
            for (int index = 0; index < this.SList.Count<SpectraScan>(); ++index)
            {
                SpectraScan spectraScan = this.SList[index];
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells["ColName"].Value = (object)spectraScan.C_name;
                //this.dataGridView1.Rows[index].Cells["ColStatus"].Value = spectraScan.status ? (object)CommonFun.GetLanText("saved") : (object)CommonFun.GetLanText("unsaved");
                this.dataGridView1.Rows[index].Cells["ColColor"].Style = new DataGridViewCellStyle()
                {
                    BackColor = spectraScan.color
                };
                if (spectraScan.IsShow == 1)
                {
                    this.dataGridView1.Rows[index].Cells["ColOP"].Value = (object)Properties.Resources.UI_DB_Check_Checked;
                    this.dataGridView1.Rows[index].Cells["ColOP"].Tag = (object)"on";
                }
                else
                {
                    this.dataGridView1.Rows[index].Cells["ColOP"].Value = (object)Properties.Resources.UI_DB_Check_Unchecked;
                    this.dataGridView1.Rows[index].Cells["ColOP"].Tag = (object)"off";
                }
                this.dataGridView1.Rows[index].Cells["ColOP1"].Value = (object)Properties.Resources.UI_DB_Check_Unchecked;
                this.dataGridView1.Rows[index].Cells["ColOP1"].Tag = (object)"off";
                this.dataGridView1.Rows[index].Tag = (object)spectraScan;
                // this.dataGridView1.Columns[0].Width = 1;
            }
            if (this.SList.Count >= this.dgvcnt)
                return;

            // CommonFun.showbox("КОличество строк " + dataGridView1.Rows.Count.ToString(), "Количество строк в таблице");
            //    CommonFun.showbox("КОличество столбцов " + dataGridView1.ColumnCount.ToString(), "Количество столбцов в таблице");
            //    this.dataGridView1.Rows.Add(this.dgvcnt - this.SList.Count<SpectraScan>());
        }

        private void BtnDelete_PreviewMouseDown(object sender, EventArgs e)
        {
            List<SpectraScan> spectraScanList = new List<SpectraScan>();
            for (int index = 0; index < this.dataGridView1.Rows.Count; ++index)
            {
                if (this.dataGridView1.Rows[index].Tag != null && this.dataGridView1.Rows[index].Cells["ColOP"].Tag== (object)"on")
                    spectraScanList.Add(this.SList[index]);
            }
            for (int index = 0; index < spectraScanList.Count; ++index)
                this.SList.Remove(spectraScanList[index]);
            if (spectraScanList.Count < 1)
                return;
            this.ListBind();
        }

        private void Top_PreviewMouseDown(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedCells == null || this.dataGridView1.SelectedCells.Count < 1)
                return;
            int rowIndex = this.dataGridView1.SelectedCells[0].RowIndex;
            if (rowIndex == 0)
            {
                CommonFun.showbox("firstline", "Warning");
            }
            else
            {
                if (rowIndex > this.SList.Count)
                    return;
                if (this.btnDelete.Content.ToString() != CommonFun.GetLanText("Singlesample"))
                    this.SList[rowIndex].IsShow = 0;
                SpectraScan spectraScan = this.SList[rowIndex];
                for (int index = rowIndex; index > 0; --index)
                    this.SList[index] = this.SList[index - 1];
                this.SList[0] = spectraScan;
                this.ListBind();
            }
        }

        private void AllCurrent_PreviewMouseDown(object sender, EventArgs e)
        {
            foreach (SpectraScan spectraScan in this.SList)
                spectraScan.IsShow = spectraScan.IsShow != 1 ? 1 : 0;
            this.ListBind();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedCells == null || this.dataGridView1.SelectedCells.Count <= 0)
                return;
            SpectraScan tag = (SpectraScan)this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Tag;
            if (tag != null)
            {
                this.lblXHV.Content = tag.InstrumentsType;
                this.lblXLHV.Content = tag.Serials;
                this.lblCLMSV.Content = tag.C_Mode;
                this.lblQSBCV.Content = tag.C_BeginWL;
                this.lblJSBCV.Content = tag.C_EndWL;
                this.lblJGV.Content = tag.C_StepLen;
                this.lblSDV.Content = tag.C_ScanSpeed;
                this.lblGCV.Content = tag.C_SLength;
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();

            //this.Close();
        }

        private void CloseSettings_PreviewMouseDown(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            List<SpectraScan> spectraScanList = new List<SpectraScan>();
            for (int index = 0; index < this.dataGridView1.Rows.Count; ++index)
            {
                if (this.dataGridView1.Rows[index].Tag != null && this.dataGridView1.Rows[index].Cells["ColOP"].Tag== (object)"on")
                    spectraScanList.Add(this.SList[index]);
            }
            for (int index = 0; index < spectraScanList.Count; ++index)
                this.SList.Remove(spectraScanList[index]);
            if (spectraScanList.Count < 1)
                return;
            this.ListBind();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
                DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
                DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
                DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
                DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
                DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();

                ///this.ColStatus = new DataGridViewTextBoxColumn();
                this.ColName = new DataGridViewTextBoxColumn();
                this.ColColor = new DataGridViewTextBoxColumn();
                this.ColOP = new DataGridViewImageColumn();
                this.ColOP1 = new DataGridViewImageColumn();
                this.colorDialog1 = new ColorDialog();

                this.dataGridView1.AutoGenerateColumns = true;

                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AllowUserToDeleteRows = false;
                this.dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
                gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
                gridViewCellStyle1.BackColor = SystemColors.Control;
                gridViewCellStyle1.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular);
                //gridViewCellStyle1.ForeColor = SystemColors.WindowText;
                //gridViewCellStyle1.SelectionBackColor = Color.White;
                gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
                gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
                this.dataGridView1.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;

                gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
                gridViewCellStyle2.BackColor = SystemColors.Window;
                gridViewCellStyle2.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
                gridViewCellStyle2.ForeColor = SystemColors.ControlText;
                gridViewCellStyle2.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
                gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
                gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
                this.dataGridView1.DefaultCellStyle = gridViewCellStyle2;
                this.dataGridView1.MultiSelect = false;
                this.dataGridView1.Name = "dataGridView1";
                this.dataGridView1.ReadOnly = true;
                this.dataGridView1.RowHeadersVisible = false;
                gridViewCellStyle3.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
                this.dataGridView1.RowsDefaultCellStyle = gridViewCellStyle3;
                this.dataGridView1.RowTemplate.Height = 50;
                this.dataGridView1.ColumnHeadersHeight = 50;
                this.dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
                this.dataGridView1.SelectionChanged += new EventHandler(this.dataGridView1_SelectionChanged);
                // this.ColStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridViewCellStyle4.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
                //this.ColStatus.DefaultCellStyle = gridViewCellStyle4;
                //ComponentResourceManager.ApplyResources((object)this.ColStatus, "ColStatus");
                /* this.ColStatus.Name = "ColStatus";
                 this.ColStatus.HeaderText = "Статус";
                 this.ColStatus.ReadOnly = true;
                 this.ColStatus.SortMode = DataGridViewColumnSortMode.NotSortable;*/
                this.ColName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridViewCellStyle5.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);

                this.ColName.DefaultCellStyle = gridViewCellStyle5;
                // ComponentResourceManager.ApplyResources((object)this.ColName, "ColName");
                this.ColName.Name = "ColName";
                this.ColName.HeaderText = "Имя";
                this.ColName.ReadOnly = true;
                this.ColName.SortMode = DataGridViewColumnSortMode.NotSortable;
                gridViewCellStyle6.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
                this.ColColor.DefaultCellStyle = gridViewCellStyle6;
                //   ComponentResourceManager.ApplyResources((object)this.ColColor, "ColColor");
                this.ColColor.Name = "ColColor";
                this.ColColor.HeaderText = "Цвет";
                this.ColColor.ReadOnly = true;
                this.ColColor.SortMode = DataGridViewColumnSortMode.NotSortable;
                //  ComponentResourceManager.ApplyResources((object)this.ColOP, "ColOP");
                this.ColOP.Name = "ColOP";
                this.ColOP.ReadOnly = true;
                this.ColOP.HeaderText = "Операция";
                this.ColOP.Resizable = DataGridViewTriState.True;
                //  ComponentResourceManager.ApplyResources((object)this.ColOP1, "ColOP1");
                this.ColOP1.Name = "ColOP1";
                this.ColOP1.HeaderText = "Операция";
                this.ColOP1.ReadOnly = true;

                /// this.dataGridView1.Columns.AddRange((DataGridViewColumn)this.ColStatus, (DataGridViewColumn)this.ColName, (DataGridViewColumn)this.ColColor, (DataGridViewColumn)this.ColOP, (DataGridViewColumn)this.ColOP1);
                this.dataGridView1.Columns.AddRange((DataGridViewColumn)this.ColName, (DataGridViewColumn)this.ColColor, (DataGridViewColumn)this.ColOP, (DataGridViewColumn)this.ColOP1);
                // ComponentResourceManager.ApplyResources((object)this.lblInfo, "lblInfo");



                this.colorDialog1.AllowFullOpen = false;
                this.colorDialog1.SolidColorOnly = true;
                if (this.MulShow > 0)
                {
                    this.btnAllShow.IsEnabled = true;
                    // this.dataGridView1.Columns[3].Visible = true;
                    this.dataGridView1.Columns[2].Visible = true;
                }
                else
                {
                    this.btnAllShow.IsEnabled = false;
                    //  this.dataGridView1.Columns[3].Visible = false;
                    this.dataGridView1.Columns[2].Visible = false;
                }
                this.lblXHV.Content = CommonFun.GetAppConfig("modelnumber");
                this.lblCZZV.Content = CommonFun.GetAppConfig("currentuser");
                if (CommonFun.GetAppConfig("GLPEnabled") == "true")
                {
                    this.lblCZMSV.Content = "GLP";
                    this.btnDelete.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.lblCZMSV.Content = "Common";
                    this.btnDelete.Visibility = Visibility.Visible;
                }
                this.lblGPDKV.Content = CommonFun.GetAppConfig("Spectralbandwidth");
                if (this.SList.Count > 0)
                {
                    this.lblCLMSV.Content = this.SList[0].C_Mode;
                    this.lblQSBCV.Content = this.SList[0].C_BeginWL;
                    this.lblJSBCV.Content = this.SList[0].C_EndWL;
                    this.lblJGV.Content = this.SList[0].C_StepLen;
                    this.lblSDV.Content = this.SList[0].C_ScanSpeed;
                    this.lblGCV.Content = this.SList[0].C_SLength;
                }
                //this.dataGridView1.Columns[3].DefaultCellStyle.NullValue = (object)null;
                //this.dataGridView1.Columns[4].DefaultCellStyle.NullValue = (object)null;
                this.dataGridView1.Columns[2].DefaultCellStyle.NullValue = (object)null;
                this.dataGridView1.Columns[3].DefaultCellStyle.NullValue = (object)null;
                this.dataGridView1.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular);
                this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular);
                this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
                this.dataGridView1.AllowUserToAddRows = false;

                this.dataGridView1.EnableHeadersVisualStyles = false;

                this.dataGridView1.Refresh();
                // this.dgvcnt = this.dataGridView1.Height / 40 - 1;
                this.ListBind();



            }
            catch (Exception ed)
            {
                CommonFun.showbox(ed.ToString(), "Ошибка");
            }
        }
    }
}
