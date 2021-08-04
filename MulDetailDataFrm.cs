using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UVStudio.Properties;

namespace UVStudio
{
    public partial class MulDetailDataFrm : Form
    {
        public string lanvalue = "";
        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");
        private string conacc = CommonFun.GetAcc("ceAccuracy");
        private int dgvcnt = 0;
        private int acnt = 0;

        private DataGridViewTextBoxColumn ColNO;
        private DataGridViewTextBoxColumn ColND;
        private DataGridViewTextBoxColumn ColXGD;
        private DataGridViewTextBoxColumn ColTime;
        private DataGridViewTextBoxColumn ColBZ;
        private DataGridViewImageColumn ColOP;
        private DataGridViewTextBoxColumn ColNo1;
        private DataGridViewTextBoxColumn ColXGD1;
        private DataGridViewTextBoxColumn ColND1;
        private DataGridViewTextBoxColumn ColTime1;
        private DataGridViewTextBoxColumn ColBZ1;
        private DataGridViewImageColumn ColOP1;
        private DataGridViewTextBoxColumn ColNo2;
        private DataGridViewTextBoxColumn ColND2;
        private DataGridViewTextBoxColumn ColND3;
        private DataGridViewTextBoxColumn ColXGD2;
        private DataGridViewTextBoxColumn ColXGD3;
        private DataGridViewTextBoxColumn ColTime2;
        private DataGridViewTextBoxColumn ColBZ2;
        private DataGridViewImageColumn ColOP2;
        private DataGridViewTextBoxColumn ColNo3;
        private DataGridViewTextBoxColumn ColDNA;
        private DataGridViewTextBoxColumn ColDBZ;
        private DataGridViewTextBoxColumn ColBL;
        private DataGridViewTextBoxColumn ColData;
        private DataGridViewTextBoxColumn ColTime3;
        private DataGridViewTextBoxColumn ColBZ3;
        private DataGridViewImageColumn ColOP3;

        public MulDetailDataFrm()
        {
            InitializeComponent();

            DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle12 = new DataGridViewCellStyle();

            this.ColNO = new DataGridViewTextBoxColumn();
            this.ColND = new DataGridViewTextBoxColumn();
            this.ColXGD = new DataGridViewTextBoxColumn();
            this.ColTime = new DataGridViewTextBoxColumn();
            this.ColBZ = new DataGridViewTextBoxColumn();
            this.ColOP = new DataGridViewImageColumn();

            this.ColNo1 = new DataGridViewTextBoxColumn();
            this.ColXGD1 = new DataGridViewTextBoxColumn();
            this.ColND1 = new DataGridViewTextBoxColumn();
            this.ColTime1 = new DataGridViewTextBoxColumn();
            this.ColBZ1 = new DataGridViewTextBoxColumn();
            this.ColOP1 = new DataGridViewImageColumn();

            this.ColNo2 = new DataGridViewTextBoxColumn();
            this.ColND2 = new DataGridViewTextBoxColumn();
            this.ColND3 = new DataGridViewTextBoxColumn();
            this.ColXGD2 = new DataGridViewTextBoxColumn();
            this.ColXGD3 = new DataGridViewTextBoxColumn();
            this.ColTime2 = new DataGridViewTextBoxColumn();
            this.ColBZ2 = new DataGridViewTextBoxColumn();
            this.ColOP2 = new DataGridViewImageColumn();

            this.ColNo3 = new DataGridViewTextBoxColumn();
            this.ColDNA = new DataGridViewTextBoxColumn();
            this.ColDBZ = new DataGridViewTextBoxColumn();
            this.ColBL = new DataGridViewTextBoxColumn();
            this.ColData = new DataGridViewTextBoxColumn();
            this.ColTime3 = new DataGridViewTextBoxColumn();
            this.ColBZ3 = new DataGridViewTextBoxColumn();
            this.ColOP3 = new DataGridViewImageColumn();

            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;

            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.dataGridView1.Columns.AddRange((DataGridViewColumn)this.ColNO, (DataGridViewColumn)this.ColND, (DataGridViewColumn)this.ColXGD, (DataGridViewColumn)this.ColTime, (DataGridViewColumn)this.ColBZ, (DataGridViewColumn)this.ColOP);
            gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle1.BackColor = SystemColors.Window;
            gridViewCellStyle1.Font = new Font("Segoe UI", 16F);
            gridViewCellStyle1.ForeColor = SystemColors.ControlText;
            gridViewCellStyle1.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
            gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = gridViewCellStyle1;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            gridViewCellStyle2.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView1.RowsDefaultCellStyle = gridViewCellStyle2;
            this.dataGridView1.RowTemplate.Height = 45;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.ColNO.FillWeight = 50.76142f;
            //ComponentResourceManager.ApplyResources((object)this.ColNO, "ColNO");
            this.ColNO.Name = "ColNO";
            this.ColNO.ReadOnly = true;
            this.ColNO.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColND.FillWeight = 112.3096f;
            //ComponentResourceManager.ApplyResources((object)this.ColND, "ColND");
            this.ColND.Name = "ColND";
            this.ColND.ReadOnly = true;
            this.ColND.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridViewCellStyle3.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColXGD.DefaultCellStyle = gridViewCellStyle3;
            this.ColXGD.FillWeight = 112.3096f;
            //ComponentResourceManager.ApplyResources((object)this.ColXGD, "ColXGD");
            this.ColXGD.Name = "ColXGD";
            this.ColXGD.ReadOnly = true;
            this.ColXGD.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColTime.FillWeight = 112.3096f;
            //ComponentResourceManager.ApplyResources((object)this.ColTime, "ColTime");
            this.ColTime.Name = "ColTime";
            this.ColTime.ReadOnly = true;
            this.ColTime.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColBZ.FillWeight = 112.3096f;
            //ComponentResourceManager.ApplyResources((object)this.ColBZ, "ColBZ");
            this.ColBZ.Name = "ColBZ";
            this.ColBZ.ReadOnly = true;
            this.ColBZ.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColOP, "ColOP");
            this.ColOP.Name = "ColOP";
            this.ColOP.ReadOnly = true;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            //ComponentResourceManager.ApplyResources((object)this.dataGridView2, "dataGridView2");
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.dataGridView2.Columns.AddRange((DataGridViewColumn)this.ColNo1, (DataGridViewColumn)this.ColXGD1, (DataGridViewColumn)this.ColND1, (DataGridViewColumn)this.ColTime1, (DataGridViewColumn)this.ColBZ1, (DataGridViewColumn)this.ColOP1);
            gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle4.BackColor = SystemColors.Window;
            gridViewCellStyle4.Font = new Font("Segoe UI", 16F);
            gridViewCellStyle4.ForeColor = SystemColors.ControlText;
            gridViewCellStyle4.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
            gridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = gridViewCellStyle4;
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            gridViewCellStyle5.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView2.RowsDefaultCellStyle = gridViewCellStyle5;
            this.dataGridView2.RowTemplate.Height = 45;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.CellClick += new DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            //ComponentResourceManager.ApplyResources((object)this.ColNo1, "ColNo1");
            this.ColNo1.Name = "ColNo1";
            this.ColNo1.ReadOnly = true;
            this.ColNo1.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridViewCellStyle6.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColXGD1.DefaultCellStyle = gridViewCellStyle6;
            //ComponentResourceManager.ApplyResources((object)this.ColXGD1, "ColXGD1");
            this.ColXGD1.Name = "ColXGD1";
            this.ColXGD1.ReadOnly = true;
            this.ColXGD1.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColND1, "ColND1");
            this.ColND1.Name = "ColND1";
            this.ColND1.ReadOnly = true;
            this.ColND1.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColTime1, "ColTime1");
            this.ColTime1.Name = "ColTime1";
            this.ColTime1.ReadOnly = true;
            this.ColTime1.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColBZ1, "ColBZ1");
            this.ColBZ1.Name = "ColBZ1";
            this.ColBZ1.ReadOnly = true;
            this.ColBZ1.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColOP1, "ColOP1");
            this.ColOP1.Name = "ColOP1";
            this.ColOP1.ReadOnly = true;
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            //ComponentResourceManager.ApplyResources((object)this.dataGridView3, "dataGridView3");
            this.dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView3.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.dataGridView3.Columns.AddRange((DataGridViewColumn)this.ColNo2, (DataGridViewColumn)this.ColND2, (DataGridViewColumn)this.ColND3, (DataGridViewColumn)this.ColXGD2, (DataGridViewColumn)this.ColXGD3, (DataGridViewColumn)this.ColTime2, (DataGridViewColumn)this.ColBZ2, (DataGridViewColumn)this.ColOP2);
            gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle7.BackColor = SystemColors.Window;
            gridViewCellStyle7.Font = new Font("Segoe UI", 16F);
            gridViewCellStyle7.ForeColor = SystemColors.ControlText;
            gridViewCellStyle7.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
            gridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            this.dataGridView3.DefaultCellStyle = gridViewCellStyle7;
            this.dataGridView3.MultiSelect = false;
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.RowHeadersVisible = false;
            gridViewCellStyle8.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView3.RowsDefaultCellStyle = gridViewCellStyle8;
            this.dataGridView3.RowTemplate.Height = 45;
            this.dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.CellClick += new DataGridViewCellEventHandler(this.dataGridView3_CellClick);
            //ComponentResourceManager.ApplyResources((object)this.ColNo2, "ColNo2");
            this.ColNo2.Name = "ColNo2";
            this.ColNo2.ReadOnly = true;
            this.ColNo2.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColND2, "ColND2");
            this.ColND2.Name = "ColND2";
            this.ColND2.ReadOnly = true;
            this.ColND2.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColND3, "ColND3");
            this.ColND3.Name = "ColND3";
            this.ColND3.ReadOnly = true;
            this.ColND3.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridViewCellStyle9.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColXGD2.DefaultCellStyle = gridViewCellStyle9;
            //ComponentResourceManager.ApplyResources((object)this.ColXGD2, "ColXGD2");
            this.ColXGD2.Name = "ColXGD2";
            this.ColXGD2.ReadOnly = true;
            this.ColXGD2.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColXGD3, "ColXGD3");
            this.ColXGD3.Name = "ColXGD3";
            this.ColXGD3.ReadOnly = true;
            this.ColXGD3.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColTime2, "ColTime2");
            this.ColTime2.Name = "ColTime2";
            this.ColTime2.ReadOnly = true;
            this.ColTime2.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColBZ2, "ColBZ2");
            this.ColBZ2.Name = "ColBZ2";
            this.ColBZ2.ReadOnly = true;
            this.ColBZ2.SortMode = DataGridViewColumnSortMode.NotSortable;
            //ComponentResourceManager.ApplyResources((object)this.ColOP2, "ColOP2");
            this.ColOP2.Name = "ColOP2";
            this.ColOP2.ReadOnly = true;
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.AllowUserToDeleteRows = false;
            //ComponentResourceManager.ApplyResources((object)this.dataGridView4, "dataGridView4");
            this.dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView4.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.dataGridView4.Columns.AddRange((DataGridViewColumn)this.ColNo3, (DataGridViewColumn)this.ColDNA, (DataGridViewColumn)this.ColDBZ, (DataGridViewColumn)this.ColBL, (DataGridViewColumn)this.ColData, (DataGridViewColumn)this.ColTime3, (DataGridViewColumn)this.ColBZ3, (DataGridViewColumn)this.ColOP3);
            gridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle10.BackColor = SystemColors.Window;
            gridViewCellStyle10.Font = new Font("Segoe UI", 16F);
            gridViewCellStyle10.ForeColor = SystemColors.ControlText;
            gridViewCellStyle10.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
            gridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            this.dataGridView4.DefaultCellStyle = gridViewCellStyle10;
            this.dataGridView4.MultiSelect = false;
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowHeadersVisible = false;
            gridViewCellStyle11.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView4.RowsDefaultCellStyle = gridViewCellStyle11;
            this.dataGridView4.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView4.RowTemplate.Height = 50;
            this.dataGridView4.RowTemplate.Resizable = DataGridViewTriState.True;
            this.dataGridView4.CellClick += new DataGridViewCellEventHandler(this.dataGridView4_CellClick);
            this.ColNo3.FillWeight = 21.27823f;
            //ComponentResourceManager.ApplyResources((object)this.ColNo3, "ColNo3");
            this.ColNo3.Name = "ColNo3";
            this.ColNo3.SortMode = DataGridViewColumnSortMode.NotSortable;
            gridViewCellStyle12.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColDNA.DefaultCellStyle = gridViewCellStyle12;
            this.ColDNA.FillWeight = 31.91734f;
            //ComponentResourceManager.ApplyResources((object)this.ColDNA, "ColDNA");
            this.ColDNA.Name = "ColDNA";
            this.ColDNA.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColDBZ.FillWeight = 31.91734f;
            //ComponentResourceManager.ApplyResources((object)this.ColDBZ, "ColDBZ");
            this.ColDBZ.Name = "ColDBZ";
            this.ColDBZ.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColBL.FillWeight = 31.91734f;
            //ComponentResourceManager.ApplyResources((object)this.ColBL, "ColBL");
            this.ColBL.Name = "ColBL";
            this.ColBL.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColData.FillWeight = 95.75202f;
            //ComponentResourceManager.ApplyResources((object)this.ColData, "ColData");
            this.ColData.Name = "ColData";
            this.ColData.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColTime3.FillWeight = 38.98477f;
            //ComponentResourceManager.ApplyResources((object)this.ColTime3, "ColTime3");
            this.ColTime3.Name = "ColTime3";
            this.ColTime3.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColBZ3.FillWeight = 42.55645f;
            //ComponentResourceManager.ApplyResources((object)this.ColBZ3, "ColBZ3");
            this.ColBZ3.Name = "ColBZ3";
            this.ColBZ3.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColOP3.FillWeight = 25.67652f;
            //ComponentResourceManager.ApplyResources((object)this.ColOP3, "ColOP3");
            this.ColOP3.Name = "ColOP3";
            this.ColOP3.Resizable = DataGridViewTriState.True;
            ColBL.HeaderText = "Соотношение";
            ColBZ.HeaderText = "Имя образца";
            ColBZ1.HeaderText = "Имя образца";
            ColBZ2.HeaderText = "Имя образца";
            ColBZ3.HeaderText = "Имя образца";
            ColData.HeaderText = "Подробные данные";
            ColDBZ.HeaderText = "Белок";
            ColDNA.HeaderText = "DNA";
            ColND.HeaderText = "Результат расчета (Abs)";
            ColND1.HeaderText = "Конц";
            ColND2.HeaderText = "Конц 1";
            ColND3.HeaderText = "Конц 2";
            ColNO.HeaderText = "Серийный номер";
            ColNo1.HeaderText = "Серийный номер 1";
            ColNo2.HeaderText = "Серийный номер 2";
            ColNo3.HeaderText = "Серийный номер 3";
            ColOP.HeaderText = "Действующий";
            ColOP1.HeaderText = "Действующий";
            ColOP2.HeaderText = "Действующий";
            ColOP3.HeaderText = "Действующий";
            ColTime.HeaderText = "Дата и время";
            ColTime1.HeaderText = "Дата и время";
            ColTime2.HeaderText = "Дата и время";
            ColTime3.HeaderText = "Дата и время";
            ColXGD.HeaderText = "Abs";
            ColXGD1.HeaderText = "Abs";
            ColXGD2.HeaderText = "Abs";
            ColXGD3.HeaderText = "Abs";
            
        }

        private void MulDetailDataFrm_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView1.Columns[5].DefaultCellStyle.NullValue = (object)null;
            this.dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView2.Columns[5].DefaultCellStyle.NullValue = (object)null;
            this.dataGridView3.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView3.Columns[7].DefaultCellStyle.NullValue = (object)null;
            this.dataGridView4.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView4.Columns[7].DefaultCellStyle.NullValue = (object)null;
            this.dataGridView1.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dataGridView2.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dataGridView3.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dataGridView3.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dataGridView4.RowsDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dataGridView4.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            this.dgvcnt = this.dataGridView1.Height / 40 - 1;

            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView2.EnableHeadersVisualStyles = false;
            this.dataGridView3.EnableHeadersVisualStyles = false;
            this.dataGridView4.EnableHeadersVisualStyles = false;

        }

        private void btnBack_Click(object sender, EventArgs e) => this.Close();

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = this.dataGridView1.CurrentCell.RowIndex;
            this.dataGridView1.Rows[rowIndex].Height = this.acnt > 3 ? this.acnt * 25 : 60;
            this.dataGridView1.Rows[rowIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            for (int index = 0; index < this.dataGridView1.Rows.Count; ++index)
            {
                if (index != rowIndex)
                {
                    this.dataGridView1.Rows[index].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    this.dataGridView1.Rows[index].Height = 40;
                }
            }
            if (this.dataGridView1.Rows[rowIndex].Tag == null || e.ColumnIndex != 5)
                return;
            if (this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag.ToString() == "off")
            {
                this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Value = (object)Resources.UI_DB_Check_Checked;
                this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag = (object)"on";
            }
            else
            {
                this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Value = (object)Resources.UI_DB_Check_Unchecked;
                this.dataGridView1.Rows[rowIndex].Cells["ColOP"].Tag = (object)"off";
            }
        }

        public void ListBind(MulMethod MM)
        {
            if (MM.MeasreList == null)
                return;
            string[] strArray = MM.WL.Split(',');
            this.acnt = ((IEnumerable<string>)strArray).Count<string>();
            this.dataGridView1.Rows.Clear();
            for (int index1 = 0; index1 < MM.MeasreList.Count; ++index1)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index1].Cells["ColNO"].Value = (object)(index1 + 1).ToString("D4");
                string str = "";
                for (int index2 = 0; index2 < ((IEnumerable<string>)strArray).Count<string>(); ++index2)
                {
                    if (MM.C_mode == CommonFun.GetLanText("Abs"))
                        str = str + "  A(" + strArray[index2] + ")=" + MM.MeasreList[index1].Avalue[index2].ToString(this.absacc) + Environment.NewLine;
                    else
                        str = str + "  T(" + strArray[index2] + ")=" + MM.MeasreList[index1].Avalue[index2].ToString(this.tacc) + Environment.NewLine;
                }
                this.dataGridView1.Rows[index1].Cells["ColXGD"].Value = (object)str;
                this.dataGridView1.Rows[index1].Cells["ColND"].Value = (double)MM.MeasreList[index1].SJJG != -1.0 ? (object)MM.MeasreList[index1].SJJG.ToString(this.absacc) : (object)"----";
                this.dataGridView1.Rows[index1].Cells["ColBZ"].Value = (object)MM.MeasreList[index1].C_bz;
                this.dataGridView1.Rows[index1].Cells["ColTime"].Value = (object)MM.MeasreList[index1].D_MTime;
                this.dataGridView1.Rows[index1].Cells["ColOP"].Value = (object)Resources.UI_DB_Check_Unchecked;
                this.dataGridView1.Rows[index1].Cells["ColOP"].Tag = (object)"off";
                this.dataGridView1.Rows[index1].Tag = (object)MM.MeasreList[index1];
            }
            if (MM.MeasreList.Count >= this.dgvcnt)
                return;
            this.dataGridView1.Rows.Add(this.dgvcnt - MM.MeasreList.Count);
        }

        public void QuaListBind(QuaMethod QM)
        {
            if (QM.MeasreList == null)
                return;
            this.dataGridView2.Rows.Clear();
            for (int index = 0; index < QM.MeasreList.Count; ++index)
            {
                this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[index].Cells["ColNO1"].Value = (object)(index + 1).ToString();
                DataGridViewCell cell1 = this.dataGridView2.Rows[index].Cells["ColXGD1"];
                Decimal num = QM.MeasreList[index].XGD.Value;
                string str1 = num.ToString(this.absacc);
                cell1.Value = (object)str1;
                DataGridViewCell cell2 = this.dataGridView2.Rows[index].Cells["ColND1"];
                num = QM.MeasreList[index].ND.Value;
                string str2 = num.ToString(this.conacc);
                cell2.Value = (object)str2;
                this.dataGridView2.Rows[index].Cells["ColBZ1"].Value = (object)QM.MeasreList[index].C_bz;
                this.dataGridView2.Rows[index].Cells["ColTime1"].Value = (object)QM.MeasreList[index].D_sj;
                this.dataGridView2.Rows[index].Cells["ColOP1"].Value = (object)Resources.UI_DB_Check_Unchecked;
                this.dataGridView2.Rows[index].Cells["ColOP1"].Tag = (object)"off";
                this.dataGridView2.Rows[index].Tag = (object)QM.MeasreList[index];
            }
            if (QM.MeasreList.Count >= this.dgvcnt)
                return;
            this.dataGridView2.Rows.Add(this.dgvcnt - QM.MeasreList.Count);
        }

        public void DualListBind(DualComMethod DM)
        {
            if (DM.MeasreList == null)
                return;
            this.dataGridView3.Rows.Clear();
            for (int index = 0; index < DM.MeasreList.Count; ++index)
            {
                this.dataGridView3.Rows.Add();
                this.dataGridView3.Rows[index].Cells["ColNO2"].Value = (object)(index + 1).ToString();
                DataGridViewCell cell1 = this.dataGridView3.Rows[index].Cells["ColXGD2"];
                float num = DM.MeasreList[index].XGD1.Value;
                string str1 = num.ToString(this.absacc);
                cell1.Value = (object)str1;
                DataGridViewCell cell2 = this.dataGridView3.Rows[index].Cells["ColXGD3"];
                num = DM.MeasreList[index].XGD2.Value;
                string str2 = num.ToString(this.absacc);
                cell2.Value = (object)str2;
                DataGridViewCell cell3 = this.dataGridView3.Rows[index].Cells["ColND2"];
                num = DM.MeasreList[index].ND1.Value;
                string str3 = num.ToString(this.conacc);
                cell3.Value = (object)str3;
                DataGridViewCell cell4 = this.dataGridView3.Rows[index].Cells["ColND3"];
                num = DM.MeasreList[index].ND2.Value;
                string str4 = num.ToString(this.conacc);
                cell4.Value = (object)str4;
                this.dataGridView3.Rows[index].Cells["ColBZ2"].Value = (object)DM.MeasreList[index].C_bz;
                this.dataGridView3.Rows[index].Cells["ColTime2"].Value = (object)DM.MeasreList[index].D_sj;
                this.dataGridView3.Rows[index].Cells["ColOP2"].Value = (object)Resources.UI_DB_Check_Unchecked;
                this.dataGridView3.Rows[index].Cells["ColOP2"].Tag = (object)"off";
                this.dataGridView3.Rows[index].Tag = (object)DM.MeasreList[index];
            }
            if (DM.MeasreList.Count >= this.dgvcnt)
                return;
            //this.dataGridView3.Rows.Add(this.dgvcnt - DM.MeasreList.Count);
        }

        public void DnaListBind(QuaMethod QM)
        {
            if (QM.DNAMeaList == null)
                return;
            if (QM.QPar.MeasureMethodName.Contains("DNA"))
            {
                this.dataGridView2.Visible = false;
                this.dataGridView4.Visible = true;
                this.dataGridView4.Rows.Clear();
                for (int index = 0; index < QM.DNAMeaList.Count; ++index)
                {
                    this.dataGridView4.Rows.Add();
                    this.dataGridView4.Rows[index].Cells["ColNO3"].Value = (object)(index + 1).ToString();
                    this.dataGridView4.Rows[index].Cells["ColDNA"].Value = !(QM.DNAMeaList[index].DNA != 99999.99M) ? (object)"---" : (object)QM.DNAMeaList[index].DNA.ToString(this.conacc);
                    this.dataGridView4.Rows[index].Cells["ColDBZ"].Value = !(QM.DNAMeaList[index].Protein != 99999.99M) ? (object)"---" : (object)QM.DNAMeaList[index].Protein.ToString(this.conacc);
                    this.dataGridView4.Rows[index].Cells["ColBL"].Value = !(QM.DNAMeaList[index].Ratio != 99999.99M) ? (object)"---" : (object)QM.DNAMeaList[index].Ratio.ToString(this.conacc);
                    string str1 = "";
                    string str2;
                    if (QM.DNAMeaList[index].A[0] != 99999.99M)
                        str2 = str1 + "A1(" + QM.QPar.WL.Split(',')[0] + "nm)=" + QM.DNAMeaList[index].A[0].ToString(this.absacc);
                    else
                        str2 = str1 + "A1(---)";
                    string str3;
                    if (QM.DNAMeaList[index].A[1] != 99999.99M)
                        str3 = str2 + "   A2(" + QM.QPar.WL.Split(',')[1] + "nm)=" + QM.DNAMeaList[index].A[1].ToString(this.absacc);
                    else
                        str3 = str2 + "   A2(---)";
                    string str4;
                    if (QM.QPar.BackWL.Length > 0)
                        str4 = str3 + "   Aref(" + QM.QPar.BackWL + "nm)=" + QM.DNAMeaList[index].ABack.ToString(this.absacc);
                    else
                        str4 = str3 + "   ---";
                    this.dataGridView4.Rows[index].Cells["ColData"].Value = (object)str4;
                    this.dataGridView4.Rows[index].Cells["ColTime3"].Value = (object)QM.DNAMeaList[index].D_sj;
                    this.dataGridView4.Rows[index].Cells["ColBZ3"].Value = (object)QM.DNAMeaList[index].C_bz;
                    this.dataGridView4.Rows[index].Cells["ColOP3"].Value = (object)Resources.UI_DB_Check_Unchecked;
                    this.dataGridView4.Rows[index].Cells["ColOP3"].Tag = (object)"off";
                    this.dataGridView4.Rows[index].Tag = (object)QM.DNAMeaList[index];
                }
                if (QM.DNAMeaList.Count >= this.dgvcnt)
                    return;
                this.dataGridView4.Rows.Add(this.dgvcnt - QM.DNAMeaList.Count);
            }
            else
            {
                this.dataGridView2.Visible = true;
                this.dataGridView4.Visible = false;
                this.dataGridView2.Rows.Clear();
                for (int index = 0; index < QM.DNAMeaList.Count; ++index)
                {
                    this.dataGridView2.Rows.Add();
                    this.dataGridView2.Rows[index].Cells["ColNO1"].Value = (object)(index + 1).ToString();
                    this.dataGridView2.Rows[index].Cells["ColXGD1"].Value = !(QM.DNAMeaList[index].XGD != 99999.99M) ? (object)"---" : (object)QM.DNAMeaList[index].XGD.ToString(this.absacc);
                    this.dataGridView2.Rows[index].Cells["ColND1"].Value = !(QM.DNAMeaList[index].ND != 99999.99M) ? (object)"---" : (object)QM.DNAMeaList[index].ND.ToString(this.conacc);
                    this.dataGridView2.Rows[index].Cells["ColBZ1"].Value = (object)QM.DNAMeaList[index].C_bz;
                    this.dataGridView2.Rows[index].Cells["ColTime1"].Value = (object)QM.DNAMeaList[index].D_sj;
                    this.dataGridView2.Rows[index].Cells["ColOP1"].Value = (object)Resources.UI_DB_Check_Unchecked;
                    this.dataGridView2.Rows[index].Cells["ColOP1"].Tag = (object)"off";
                    this.dataGridView2.Rows[index].Tag = (object)QM.DNAMeaList[index];
                }
                if (QM.DNAMeaList.Count < this.dgvcnt)
                    this.dataGridView2.Rows.Add(this.dgvcnt - QM.DNAMeaList.Count);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = this.dataGridView2.CurrentCell.RowIndex;
            this.dataGridView2.Rows[rowIndex].Height = 60;
            for (int index = 0; index < this.dataGridView2.Rows.Count; ++index)
            {
                if (index != rowIndex)
                    this.dataGridView2.Rows[index].Height = 40;
            }
            if (this.dataGridView2.Rows[rowIndex].Tag == null || e.ColumnIndex != 5)
                return;
            if (this.dataGridView2.Rows[rowIndex].Cells["ColOP1"].Tag.ToString() == "off")
            {
                this.dataGridView2.Rows[rowIndex].Cells["ColOP1"].Value = (object)Resources.UI_DB_Check_Checked;
                this.dataGridView2.Rows[rowIndex].Cells["ColOP1"].Tag = (object)"on";
            }
            else
            {
                this.dataGridView2.Rows[rowIndex].Cells["ColOP1"].Value = (object)Resources.UI_DB_Check_Unchecked;
                this.dataGridView2.Rows[rowIndex].Cells["ColOP1"].Tag = (object)"off";
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = this.dataGridView3.CurrentCell.RowIndex;
            this.dataGridView3.Rows[rowIndex].Height = 60;
            for (int index = 0; index < this.dataGridView3.Rows.Count; ++index)
            {
                if (index != rowIndex)
                    this.dataGridView3.Rows[index].Height = 40;
            }
            if (this.dataGridView3.Rows[rowIndex].Tag == null || e.ColumnIndex != 7)
                return;
            if (this.dataGridView3.Rows[rowIndex].Cells["ColOP2"].Tag.ToString() == "off")
            {
                this.dataGridView3.Rows[rowIndex].Cells["ColOP2"].Value = (object)Resources.UI_DB_Check_Checked;
                this.dataGridView3.Rows[rowIndex].Cells["ColOP2"].Tag = (object)"on";
            }
            else
            {
                this.dataGridView3.Rows[rowIndex].Cells["ColOP2"].Value = (object)Resources.UI_DB_Check_Unchecked;
                this.dataGridView3.Rows[rowIndex].Cells["ColOP2"].Tag = (object)"off";
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = this.dataGridView4.CurrentCell.RowIndex;
            this.dataGridView4.Rows[rowIndex].Height = 60;
            for (int index = 0; index < this.dataGridView4.Rows.Count; ++index)
            {
                if (index != rowIndex)
                    this.dataGridView4.Rows[index].Height = 40;
            }
            if (this.dataGridView4.Rows[rowIndex].Tag == null || e.ColumnIndex != 7)
                return;
            if (this.dataGridView4.Rows[rowIndex].Cells["ColOP3"].Tag.ToString() == "off")
            {
                this.dataGridView4.Rows[rowIndex].Cells["ColOP3"].Value = (object)Resources.UI_DB_Check_Checked;
                this.dataGridView4.Rows[rowIndex].Cells["ColOP3"].Tag = (object)"on";
            }
            else
            {
                this.dataGridView4.Rows[rowIndex].Cells["ColOP3"].Value = (object)Resources.UI_DB_Check_Unchecked;
                this.dataGridView4.Rows[rowIndex].Cells["ColOP3"].Tag = (object)"off";
            }
        }
    }
}
