using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using UVStudio.Properties;

namespace UVStudio
{
    public partial class StandardMeasureFrm : Form
    {
        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");
        private string conacc = CommonFun.GetAcc("ceAccuracy");
        public int dgvcnt = 0;
        private Decimal K0 = 0M;
        private Decimal K1 = 0M;
        private Decimal K2 = 0M;
        private Decimal K3 = 0M;
        private Decimal K10 = 0M;
        private Decimal K11 = 0M;
        private Decimal K12 = 0M;
        private Decimal K13 = 0M;
        private Decimal R = 0M;

        private DataGridViewImageColumn ColPC;
        private DataGridViewTextBoxColumn ColNo;
        private DataGridViewTextBoxColumn ColXGD;
        private DataGridViewTextBoxColumn ColND;
        private DataGridViewTextBoxColumn ColBZ;

        public StandardMeasureFrm()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, EventArgs e) => this.Close();
        public QuaMethod QM { get; set; }

        private void StandardMeasureFrm_Load(object sender, EventArgs e)
        {
          
            this.dgvcnt = this.dataGridView1.Height / 40 - 1;
            this.panel3.Width = (this.Width - 44 - 6) / 2;
            this.dataGridView1.Width = this.panel3.Width;
            this.picCurve.Width = this.panel3.Width;
            this.lblfcs.Width = this.panel3.Width;
            this.lblstcurve.Width = this.panel3.Width - 4;
            this.picCurve.Location = new Point(this.panel3.Width + 33 + 6, this.picCurve.Location.Y);
            this.lblfcs.Location = new Point(this.panel3.Width + 33 + 6, this.lblfcs.Location.Y);
            this.lblstcurve.Location = new Point(this.panel3.Width + 33 + 6 + 2, this.lblstcurve.Location.Y);

            DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle9 = new DataGridViewCellStyle();

            this.ColPC = new DataGridViewImageColumn();
            this.ColNo = new DataGridViewTextBoxColumn();
            this.ColXGD = new DataGridViewTextBoxColumn();
            this.ColND = new DataGridViewTextBoxColumn();
            this.ColBZ = new DataGridViewTextBoxColumn();

            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;

            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle1.BackColor = SystemColors.Control;
            gridViewCellStyle1.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            gridViewCellStyle1.ForeColor = SystemColors.WindowText;
            gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange((DataGridViewColumn)this.ColPC, (DataGridViewColumn)this.ColNo, (DataGridViewColumn)this.ColXGD, (DataGridViewColumn)this.ColND, (DataGridViewColumn)this.ColBZ);
            gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle2.BackColor = SystemColors.Window;
            gridViewCellStyle2.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            gridViewCellStyle2.ForeColor = SystemColors.ControlText;
            gridViewCellStyle2.SelectionBackColor = Color.FromArgb(192, 192, (int)byte.MaxValue);
            gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = gridViewCellStyle2;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle3.BackColor = SystemColors.Control;
            gridViewCellStyle3.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            gridViewCellStyle3.ForeColor = SystemColors.WindowText;
            gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = gridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            gridViewCellStyle4.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView1.RowsDefaultCellStyle = gridViewCellStyle4;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
         //   this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
         //   this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewCellStyle5.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
           // gridViewCellStyle5.NullValue =  ComponentResourceManager.GetObject("dataGridViewCellStyle11.NullValue");
            this.ColPC.DefaultCellStyle = gridViewCellStyle5;
           // ComponentResourceManager.ApplyResources((object)this.ColPC, "ColPC");
            this.ColPC.Name = "ColPC";
            this.ColPC.HeaderText = "Исключить";
            gridViewCellStyle6.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColPC.DefaultCellStyle = gridViewCellStyle6;
           // ComponentResourceManager.ApplyResources((object)this.ColNo, "ColNo");
            this.ColNo.Name = "ColNo";
            this.ColNo.HeaderText = "Номер";
            gridViewCellStyle7.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColXGD.DefaultCellStyle = gridViewCellStyle7;
           // ComponentResourceManager.ApplyResources((object)this.ColXGD, "ColXGD");
            this.ColXGD.Name = "ColXGD";
            this.ColXGD.HeaderText = "Abs";
            gridViewCellStyle8.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColND.DefaultCellStyle = gridViewCellStyle8;
           // ComponentResourceManager.ApplyResources((object)this.ColND, "ColND");
            this.ColND.Name = "ColND";
            this.ColND.HeaderText = "Концетрация";
            gridViewCellStyle9.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ColBZ.DefaultCellStyle = gridViewCellStyle9;
           // ComponentResourceManager.ApplyResources((object)this.ColBZ, "ColBZ");
            this.ColBZ.Name = "ColBZ";
            this.ColBZ.HeaderText = "Примечание";

            this.dataGridView1.Columns[0].DefaultCellStyle.NullValue = (object)null;
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
            this.dataGridView1.RowsDefaultCellStyle.Font = new Font("Segoe UI", 12f, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12f, FontStyle.Regular);

            this.dataGridView1.ColumnHeadersHeight = 50;
        }

        private void lblK0_Click(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                if (this.lblK0V.Text != "")
                    frm.txtValue.Text = this.lblK0V.Text;
                frm.txtValue.KeyDown += ((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK0V.Text = frm.txtValue.Text.ToString();
                   //     frm.Close();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K0 = num;
                            this.QM.K0 = this.K0;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K10 = num;
                            this.QM.K10 = this.K10;
                        }
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K0", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K0", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK0V.Text = frm.txtValue.Text.ToString();
                       // frm.Close();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K0 = num;
                            this.QM.K0 = this.K0;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K10 = num;
                            this.QM.K10 = this.K10;
                        }
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K0", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K0", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void lblK1_Click(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                if (this.lblK1V.Text != "")
                    frm.txtValue.Text = this.lblK1V.Text;
                frm.txtValue.KeyDown += ((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK1V.Text = frm.txtValue.Text.ToString();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K1 = num;
                            this.QM.K1 = this.K1;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K11 = num;
                            this.QM.K11 = this.K11;
                        }
                       // frm.Close();
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K1", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K1", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK1V.Text = frm.txtValue.Text.ToString();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K1 = num;
                            this.QM.K1 = this.K1;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K11 = num;
                            this.QM.K11 = this.K11;
                        }
                      //  frm.Close();
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K1", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K1", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
               frm.ShowDialog();
            }
        }

        private void lblK2_Click(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                if (this.lblK2V.Text != "")
                    frm.txtValue.Text = this.lblK2V.Text;
                frm.txtValue.KeyDown += ((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK2V.Text = frm.txtValue.Text.ToString();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K2 = num;
                            this.QM.K2 = this.K2;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K12 = num;
                            this.QM.K12 = this.K12;
                        }
                        //frm.Close();
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K2", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K2", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK2V.Text = frm.txtValue.Text.ToString();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K2 = num;
                            this.QM.K2 = this.K2;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K12 = num;
                            this.QM.K12 = this.K12;
                        }
                      //  frm.Close();
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K2", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K2", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        private void lblK3_Click(object sender, EventArgs e)
        {
            using (InputDataFrm frm = new InputDataFrm())
            {
                if (this.lblK3V.Text != "")
                    frm.txtValue.Text = this.lblK3V.Text;
                frm.txtValue.KeyDown += ((senders, es) =>
                {
                    if (es.Key != Key.Return)
                        return;
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK3V.Text = frm.txtValue.Text.ToString();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K3 = num;
                            this.QM.K3 = this.K3;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K13 = num;
                            this.QM.K13 = this.K13;
                        }
                    //    frm.Close();
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K3", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K3", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm.btnOK.PreviewMouseDown += ((param0, param1) =>
                {
                    try
                    {
                        if (frm.txtValue.Text.ToString().IndexOf('.') == 0)
                            frm.txtValue.Text = "0" + frm.txtValue.Text.ToString();
                        Decimal num = Convert.ToDecimal(frm.txtValue.Text.ToString());
                        this.lblK3V.Text = frm.txtValue.Text.ToString();
                        if (this.QM.QPar.Equation == "Abs=f(C)")
                        {
                            this.K3 = num;
                            this.QM.K3 = this.K3;
                            this.ConvertFcs();
                        }
                        else
                        {
                            this.K13 = num;
                            this.QM.K13 = this.K13;
                        }
                    //    frm.Close();
                        CommonFun.GenerateFCS(this.QM);
                        if (this.QM.QPar.Equation == "C=f(Abs)")
                            this.lblfcs.Text = this.QM.CFCS;
                        else
                            this.lblfcs.Text = this.QM.AFCS;
                        this.DrawCurve(this.QM);
                        if (this.QM.Page == 1)
                            CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputr") + " K3", false);
                        else
                            CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputr") + " K3", false);
                        frm.Close();
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                    }
                });
                frm.ShowDialog();
            }
        }

        public void GenerateNewSampleData()
        {
            if (this.QM.SamList == null)
                this.QM.SamList = new List<Sample>();
            if (this.QM.SamList.Count<Sample>() > this.QM.QPar.SamCnt)
                this.QM.SamList = new List<Sample>();
            while (this.QM.SamList.Count<Sample>() < this.QM.QPar.SamCnt)
                this.QM.SamList.Add(new Sample()
                {
                    IsExclude = false
                });
            this.BindData();
        }
        private void BindData()
        {
            this.dataGridView1.Rows.Clear();
            for (int index = 0; index < this.QM.SamList.Count; ++index)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells["ColNO"].Value = (object)(index + 1).ToString();
                Decimal? nullable = this.QM.SamList[index].XGD;
                if (nullable.HasValue)
                {
                    DataGridViewCell cell = this.dataGridView1.Rows[index].Cells["ColXGD"];
                    nullable = this.QM.SamList[index].XGD;
                    string str = nullable.Value.ToString(this.absacc);
                    cell.Value = (object)str;
                }
                else
                    this.dataGridView1.Rows[index].Cells["ColXGD"].Value = (object)this.QM.SamList[index].XGD;
                nullable = this.QM.SamList[index].ND;
                if (nullable.HasValue)
                {
                    DataGridViewCell cell = this.dataGridView1.Rows[index].Cells["ColND"];
                    nullable = this.QM.SamList[index].ND;
                    string str = nullable.Value.ToString(this.conacc);
                    cell.Value = (object)str;
                }
                else
                    this.dataGridView1.Rows[index].Cells["ColND"].Value = (object)this.QM.SamList[index].ND;
                this.dataGridView1.Rows[index].Cells["ColBZ"].Value = (object)this.QM.SamList[index].C_bz;
                if (this.QM.SamList[index].IsExclude)
                {
                    this.dataGridView1.Rows[index].Cells["ColPC"].Value = (object)Resources.UI_DB_Check_Checked;
                    this.dataGridView1.Rows[index].Cells["ColPC"].Tag = (object)"on";
                }
                else
                {
                    this.dataGridView1.Rows[index].Cells["ColPC"].Value = (object)Resources.UI_DB_Check_Unchecked;
                    this.dataGridView1.Rows[index].Cells["ColPC"].Tag = (object)"off";
                }
                this.dataGridView1.Rows[index].Tag = (object)this.QM.SamList[index];
            }
            if (this.QM.SamList.Count >= this.dgvcnt)
                return;
            this.dataGridView1.Rows.Add(this.dgvcnt - this.QM.SamList.Count);
        }

        public void DrawCurve(QuaMethod qm)
        {
            string str1;
            string format1;
            string format2;
            if (qm.QPar.Equation == "C=f(Abs)")
            {
                str1 = "XGD";
                format1 = this.absacc;
                format2 = this.conacc;
            }
            else
            {
                str1 = "ND";
                format1 = this.conacc;
                format2 = this.absacc;
            }
            int num1 = 10;
            float num2 = 0.0f;
            float num3 = 0.0f;
            List<Sample> source = new List<Sample>();
            Bitmap bitmap = new Bitmap(this.picCurve.Width, this.picCurve.Height);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.DrawRectangle(new Pen(Color.Black, 1f), 0, 0, this.picCurve.Width - 1, this.picCurve.Height - 1);
            graphics.FillRectangle((Brush)new SolidBrush(Color.White), 1, 1, this.picCurve.Width - 2, this.picCurve.Height - 2);
            Decimal? nullable1;
            SizeF sizeF1;
            SizeF sizeF2;
            if (qm.SamList != null && qm.SamList.Count > 0)
            {
                if (qm.CFCS != null || qm.AFCS != null)
                {
                    nullable1 = qm.SamList[0].ND;
                    int num4;
                    if (!nullable1.HasValue)
                    {
                        nullable1 = qm.SamList[0].XGD;
                        num4 = !nullable1.HasValue ? 1 : 0;
                    }
                    else
                        num4 = 0;
                    if (num4 == 0)
                    {
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                        Decimal num5 = nullable1.Value;
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                        Decimal num6 = nullable1.Value;
                        string format3 = "f2";
                        string format4 = "f4";
                        if (num5 <= 0.1M)
                            format3 = "f4";
                        else if (num5 <= 1M)
                            format3 = "f3";
                        else if (num5 <= 10M)
                            format3 = "f2";
                        else if (num5 <= 100M)
                            format3 = "f1";
                        if (num6 <= 0.1M)
                            format4 = "f4";
                        else if (num6 <= 1M)
                            format4 = "f3";
                        else if (num6 <= 10M)
                            format4 = "f2";
                        else if (num6 <= 100M)
                            format4 = "f1";
                        if (str1 == "ND")
                        {
                            sizeF1 = graphics.MeasureString(num5.ToString(format3), new Font("Segoe UI", (float)num1));
                            sizeF2 = graphics.MeasureString(num6.ToString(format4), new Font("Segoe UI", (float)num1));
                        }
                        else
                        {
                            sizeF1 = graphics.MeasureString(num6.ToString(format3), new Font("Segoe UI", (float)num1));
                            sizeF2 = graphics.MeasureString(num5.ToString(format4), new Font("Segoe UI", (float)num1));
                        }
                    }
                    else if (str1 == "ND")
                    {
                        sizeF1 = graphics.MeasureString("100.0", new Font("Segoe UI", (float)num1));
                        sizeF2 = graphics.MeasureString("20.00", new Font("Segoe UI", (float)num1));
                    }
                    else
                    {
                        sizeF1 = graphics.MeasureString("20.00", new Font("Segoe UI", (float)num1));
                        sizeF2 = graphics.MeasureString("100.0", new Font("Segoe UI", (float)num1));
                    }
                }
                else if (str1 == "ND")
                {
                    sizeF1 = graphics.MeasureString("100.0", new Font("Segoe UI", (float)num1));
                    sizeF2 = graphics.MeasureString("20.00", new Font("Segoe UI", (float)num1));
                }
                else
                {
                    sizeF1 = graphics.MeasureString("20.00", new Font("Segoe UI", (float)num1));
                    sizeF2 = graphics.MeasureString("100.0", new Font("Segoe UI", (float)num1));
                }
            }
            else if (str1 == "ND")
            {
                sizeF1 = graphics.MeasureString("100.0", new Font("Segoe UI", (float)num1));
                sizeF2 = graphics.MeasureString("20.00", new Font("Segoe UI", (float)num1));
            }
            else
            {
                sizeF1 = graphics.MeasureString("20.00", new Font("Segoe UI", (float)num1));
                sizeF2 = graphics.MeasureString("100.0", new Font("Segoe UI", (float)num1));
            }
            float num7 = 20f + sizeF2.Height + sizeF2.Width;
            float num8 = (float)this.picCurve.Width - sizeF2.Width;
            float num9 = (float)((double)this.picCurve.Height - (double)sizeF1.Height * 2.0 - 20.0);
            float num10 = (float)((double)sizeF1.Height + 20.0 + 20.0);
            RectangleF rectangleF = new RectangleF(num7, num10, num8 - num7, num9 - num10);
            if (qm.QPar.Equation == "C=f(Abs)")
            {
                if (qm.CFCS != null && qm.CFCS.Length > 0)
                    this.lblfcs.Text = qm.CFCS;
                nullable1 = qm.R;
                if (nullable1.HasValue)
                {
                    Label lblfcs = this.lblfcs;
                    string text = this.lblfcs.Text;
                    nullable1 = qm.R;
                    string str2 = nullable1.Value.ToString("f5");
                    string str3 = text + "\r\nR=" + str2;
                    lblfcs.Text = str3;
                }
            }
            else
            {
                if (qm.AFCS != null && qm.AFCS.Length > 0)
                    this.lblfcs.Text = qm.AFCS;
                nullable1 = qm.R;
                if (nullable1.HasValue)
                {
                    Label lblfcs = this.lblfcs;
                    string text = this.lblfcs.Text;
                    nullable1 = qm.R;
                    string str2 = nullable1.Value.ToString("f5");
                    string str3 = text + "\r\nR=" + str2;
                    lblfcs.Text = str3;
                }
            }
            graphics.DrawLine(new Pen(Color.Black, 1f), num7, num9, num8, num9);
            graphics.DrawLine(new Pen(Color.Black, 1f), num7, num10, num8, num10);
            graphics.DrawLine(new Pen(Color.Black, 1f), num7, num9, num7, num10);
            graphics.DrawLine(new Pen(Color.Black, 1f), num8, num9, num8, num10);
            float num11;
            float num12;
            float num13;
            float num14;
            if (qm.SamList != null && qm.SamList.Count > 0)
            {
                int num4;
                if (qm.CFCS != null || qm.AFCS != null)
                {
                    nullable1 = qm.SamList[0].ND;
                    if (nullable1.HasValue)
                    {
                        nullable1 = qm.SamList[0].XGD;
                        num4 = !nullable1.HasValue ? 1 : 0;
                        goto label_55;
                    }
                }
                num4 = 1;
            label_55:
                if (num4 == 0)
                {
                    Decimal? nullable2;
                    Decimal? nullable3;
                    float num5;
                    float num6;
                    if (str1 == "ND")
                    {
                        for (int index = 0; index < qm.SamList.Count; ++index)
                        {
                            nullable1 = qm.SamList[index].ND;
                            int num15;
                            if (nullable1.HasValue)
                            {
                                nullable1 = qm.SamList[index].XGD;
                                num15 = !nullable1.HasValue ? 1 : 0;
                            }
                            else
                                num15 = 1;
                            if (num15 == 0)
                            {
                                Sample sample1 = new Sample();
                                sample1.ND = qm.SamList[index].ND;
                                Sample sample2 = sample1;
                                Decimal k3 = qm.K3;
                                nullable1 = sample1.ND;
                                Decimal? nullable4;
                                if (!nullable1.HasValue)
                                {
                                    nullable2 = new Decimal?();
                                    nullable4 = nullable2;
                                }
                                else
                                    nullable4 = new Decimal?(k3 * nullable1.GetValueOrDefault());
                                nullable1 = nullable4;
                                nullable2 = sample1.ND;
                                Decimal? nullable5;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable5 = nullable3;
                                }
                                else
                                    nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                                nullable1 = nullable5;
                                nullable2 = sample1.ND;
                                Decimal? nullable6;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable6 = nullable3;
                                }
                                else
                                    nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                                nullable1 = nullable6;
                                Decimal k2 = qm.K2;
                                nullable2 = sample1.ND;
                                Decimal? nullable7;
                                if (!nullable2.HasValue)
                                {
                                    nullable3 = new Decimal?();
                                    nullable7 = nullable3;
                                }
                                else
                                    nullable7 = new Decimal?(k2 * nullable2.GetValueOrDefault());
                                nullable2 = nullable7;
                                nullable3 = sample1.ND;
                                nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                                Decimal? nullable8;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable8 = nullable3;
                                }
                                else
                                    nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                                nullable1 = nullable8;
                                Decimal k1 = qm.K1;
                                nullable2 = sample1.ND;
                                Decimal? nullable9;
                                if (!nullable2.HasValue)
                                {
                                    nullable3 = new Decimal?();
                                    nullable9 = nullable3;
                                }
                                else
                                    nullable9 = new Decimal?(k1 * nullable2.GetValueOrDefault());
                                nullable2 = nullable9;
                                Decimal? nullable10;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable10 = nullable3;
                                }
                                else
                                    nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                                nullable1 = nullable10;
                                Decimal k0 = qm.K0;
                                Decimal? nullable11;
                                if (!nullable1.HasValue)
                                {
                                    nullable2 = new Decimal?();
                                    nullable11 = nullable2;
                                }
                                else
                                    nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k0);
                                sample2.XGD = nullable11;
                                source.Add(sample1);
                            }
                        }
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                        num5 = num3 = (float)nullable1.Value;
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                        num6 = num2 = (float)nullable1.Value;
                    }
                    else
                    {
                        for (int index = 0; index < qm.SamList.Count; ++index)
                        {
                            nullable1 = qm.SamList[index].ND;
                            int num15;
                            if (nullable1.HasValue)
                            {
                                nullable1 = qm.SamList[index].XGD;
                                num15 = !nullable1.HasValue ? 1 : 0;
                            }
                            else
                                num15 = 1;
                            if (num15 == 0)
                            {
                                Sample sample1 = new Sample();
                                sample1.XGD = qm.SamList[index].XGD;
                                Sample sample2 = sample1;
                                Decimal k13 = qm.K13;
                                nullable1 = sample1.XGD;
                                Decimal? nullable4;
                                if (!nullable1.HasValue)
                                {
                                    nullable2 = new Decimal?();
                                    nullable4 = nullable2;
                                }
                                else
                                    nullable4 = new Decimal?(k13 * nullable1.GetValueOrDefault());
                                nullable1 = nullable4;
                                nullable2 = sample1.XGD;
                                Decimal? nullable5;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable5 = nullable3;
                                }
                                else
                                    nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                                nullable1 = nullable5;
                                nullable2 = sample1.XGD;
                                Decimal? nullable6;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable6 = nullable3;
                                }
                                else
                                    nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                                nullable1 = nullable6;
                                Decimal k12 = qm.K12;
                                nullable2 = sample1.XGD;
                                Decimal? nullable7;
                                if (!nullable2.HasValue)
                                {
                                    nullable3 = new Decimal?();
                                    nullable7 = nullable3;
                                }
                                else
                                    nullable7 = new Decimal?(k12 * nullable2.GetValueOrDefault());
                                nullable2 = nullable7;
                                nullable3 = sample1.XGD;
                                nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                                Decimal? nullable8;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable8 = nullable3;
                                }
                                else
                                    nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                                nullable1 = nullable8;
                                Decimal k11 = qm.K11;
                                nullable2 = sample1.XGD;
                                Decimal? nullable9;
                                if (!nullable2.HasValue)
                                {
                                    nullable3 = new Decimal?();
                                    nullable9 = nullable3;
                                }
                                else
                                    nullable9 = new Decimal?(k11 * nullable2.GetValueOrDefault());
                                nullable2 = nullable9;
                                Decimal? nullable10;
                                if (!(nullable1.HasValue & nullable2.HasValue))
                                {
                                    nullable3 = new Decimal?();
                                    nullable10 = nullable3;
                                }
                                else
                                    nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                                nullable1 = nullable10;
                                Decimal k10 = qm.K10;
                                Decimal? nullable11;
                                if (!nullable1.HasValue)
                                {
                                    nullable2 = new Decimal?();
                                    nullable11 = nullable2;
                                }
                                else
                                    nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k10);
                                sample2.ND = nullable11;
                                source.Add(sample1);
                            }
                        }
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                        num5 = num3 = (float)nullable1.Value;
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                        num6 = num2 = (float)nullable1.Value;
                    }
                    num11 = (double)num5 >= 0.0 ? num5 * 0.8f : num5 * 1.2f;
                    num12 = (double)num6 >= 0.0 ? num6 * 1.2f : num6 * 0.8f;
                    if (str1 == "ND")
                    {
                        Sample sample1 = new Sample();
                        sample1.ND = new Decimal?(Convert.ToDecimal(num11));
                        Sample sample2 = sample1;
                        Decimal k3_1 = qm.K3;
                        nullable1 = sample1.ND;
                        Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new Decimal?(k3_1 * nullable1.GetValueOrDefault());
                        nullable1 = nullable4;
                        nullable2 = sample1.ND;
                        Decimal? nullable5;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable5 = nullable3;
                        }
                        else
                            nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable5;
                        nullable2 = sample1.ND;
                        Decimal? nullable6;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable6 = nullable3;
                        }
                        else
                            nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable6;
                        Decimal k2_1 = qm.K2;
                        nullable2 = sample1.ND;
                        Decimal? nullable7;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable7 = nullable3;
                        }
                        else
                            nullable7 = new Decimal?(k2_1 * nullable2.GetValueOrDefault());
                        nullable2 = nullable7;
                        nullable3 = sample1.ND;
                        nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable8;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable8 = nullable3;
                        }
                        else
                            nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable8;
                        Decimal k1_1 = qm.K1;
                        nullable2 = sample1.ND;
                        Decimal? nullable9;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable9 = nullable3;
                        }
                        else
                            nullable9 = new Decimal?(k1_1 * nullable2.GetValueOrDefault());
                        nullable2 = nullable9;
                        Decimal? nullable10;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable10 = nullable3;
                        }
                        else
                            nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable10;
                        Decimal k0_1 = qm.K0;
                        Decimal? nullable11;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable11 = nullable2;
                        }
                        else
                            nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k0_1);
                        sample2.XGD = nullable11;
                        source.Add(sample1);
                        Sample sample3 = new Sample();
                        sample3.ND = new Decimal?(Convert.ToDecimal(num12));
                        Sample sample4 = sample3;
                        Decimal k3_2 = qm.K3;
                        nullable1 = sample3.ND;
                        Decimal? nullable12;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable12 = nullable2;
                        }
                        else
                            nullable12 = new Decimal?(k3_2 * nullable1.GetValueOrDefault());
                        nullable1 = nullable12;
                        nullable2 = sample3.ND;
                        Decimal? nullable13;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable13 = nullable3;
                        }
                        else
                            nullable13 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable13;
                        nullable2 = sample3.ND;
                        Decimal? nullable14;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable14 = nullable3;
                        }
                        else
                            nullable14 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable14;
                        Decimal k2_2 = qm.K2;
                        nullable2 = sample3.ND;
                        Decimal? nullable15;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable15 = nullable3;
                        }
                        else
                            nullable15 = new Decimal?(k2_2 * nullable2.GetValueOrDefault());
                        nullable2 = nullable15;
                        nullable3 = sample3.ND;
                        nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable16;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable16 = nullable3;
                        }
                        else
                            nullable16 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable16;
                        Decimal k1_2 = qm.K1;
                        nullable2 = sample3.ND;
                        Decimal? nullable17;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable17 = nullable3;
                        }
                        else
                            nullable17 = new Decimal?(k1_2 * nullable2.GetValueOrDefault());
                        nullable2 = nullable17;
                        Decimal? nullable18;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable18 = nullable3;
                        }
                        else
                            nullable18 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable18;
                        Decimal k0_2 = qm.K0;
                        Decimal? nullable19;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable19 = nullable2;
                        }
                        else
                            nullable19 = new Decimal?(nullable1.GetValueOrDefault() + k0_2);
                        sample4.XGD = nullable19;
                        source.Add(sample3);
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                        float num15 = (float)nullable1.Value;
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                        float num16 = (float)nullable1.Value;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                        num13 = (float)nullable1.Value;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                        num14 = (float)nullable1.Value;
                        if ((double)num15 < (double)num13)
                            num13 = num15;
                        if ((double)num16 > (double)num14)
                            num14 = num16;
                    }
                    else
                    {
                        Sample sample1 = new Sample();
                        sample1.XGD = new Decimal?(Convert.ToDecimal(num11));
                        Sample sample2 = sample1;
                        Decimal k13_1 = qm.K13;
                        nullable1 = sample1.XGD;
                        Decimal? nullable4;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable4 = nullable2;
                        }
                        else
                            nullable4 = new Decimal?(k13_1 * nullable1.GetValueOrDefault());
                        nullable1 = nullable4;
                        nullable2 = sample1.XGD;
                        Decimal? nullable5;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable5 = nullable3;
                        }
                        else
                            nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable5;
                        nullable2 = sample1.XGD;
                        Decimal? nullable6;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable6 = nullable3;
                        }
                        else
                            nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable6;
                        Decimal k12_1 = qm.K12;
                        nullable2 = sample1.XGD;
                        Decimal? nullable7;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable7 = nullable3;
                        }
                        else
                            nullable7 = new Decimal?(k12_1 * nullable2.GetValueOrDefault());
                        nullable2 = nullable7;
                        nullable3 = sample1.XGD;
                        nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable8;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable8 = nullable3;
                        }
                        else
                            nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable8;
                        Decimal k11_1 = qm.K11;
                        nullable2 = sample1.XGD;
                        Decimal? nullable9;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable9 = nullable3;
                        }
                        else
                            nullable9 = new Decimal?(k11_1 * nullable2.GetValueOrDefault());
                        nullable2 = nullable9;
                        Decimal? nullable10;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable10 = nullable3;
                        }
                        else
                            nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable10;
                        Decimal k10_1 = qm.K10;
                        Decimal? nullable11;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable11 = nullable2;
                        }
                        else
                            nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k10_1);
                        sample2.ND = nullable11;
                        source.Add(sample1);
                        Sample sample3 = new Sample();
                        sample3.XGD = new Decimal?(Convert.ToDecimal(num12));
                        Sample sample4 = sample3;
                        Decimal k13_2 = qm.K13;
                        nullable1 = sample3.XGD;
                        Decimal? nullable12;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable12 = nullable2;
                        }
                        else
                            nullable12 = new Decimal?(k13_2 * nullable1.GetValueOrDefault());
                        nullable1 = nullable12;
                        nullable2 = sample3.XGD;
                        Decimal? nullable13;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable13 = nullable3;
                        }
                        else
                            nullable13 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable13;
                        nullable2 = sample3.XGD;
                        Decimal? nullable14;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable14 = nullable3;
                        }
                        else
                            nullable14 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                        nullable1 = nullable14;
                        Decimal k12_2 = qm.K12;
                        nullable2 = sample3.XGD;
                        Decimal? nullable15;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable15 = nullable3;
                        }
                        else
                            nullable15 = new Decimal?(k12_2 * nullable2.GetValueOrDefault());
                        nullable2 = nullable15;
                        nullable3 = sample3.XGD;
                        nullable2 = nullable2.HasValue & nullable3.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new Decimal?();
                        Decimal? nullable16;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable16 = nullable3;
                        }
                        else
                            nullable16 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable16;
                        Decimal k11_2 = qm.K11;
                        nullable2 = sample3.XGD;
                        Decimal? nullable17;
                        if (!nullable2.HasValue)
                        {
                            nullable3 = new Decimal?();
                            nullable17 = nullable3;
                        }
                        else
                            nullable17 = new Decimal?(k11_2 * nullable2.GetValueOrDefault());
                        nullable2 = nullable17;
                        Decimal? nullable18;
                        if (!(nullable1.HasValue & nullable2.HasValue))
                        {
                            nullable3 = new Decimal?();
                            nullable18 = nullable3;
                        }
                        else
                            nullable18 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                        nullable1 = nullable18;
                        Decimal k10_2 = qm.K10;
                        Decimal? nullable19;
                        if (!nullable1.HasValue)
                        {
                            nullable2 = new Decimal?();
                            nullable19 = nullable2;
                        }
                        else
                            nullable19 = new Decimal?(nullable1.GetValueOrDefault() + k10_2);
                        sample4.ND = nullable19;
                        source.Add(sample3);
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                        float num15 = (float)nullable1.Value;
                        nullable1 = qm.SamList.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                        float num16 = (float)nullable1.Value;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                        num13 = (float)nullable1.Value;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                        num14 = (float)nullable1.Value;
                        if ((double)num15 < (double)num13)
                            num13 = num15;
                        if ((double)num16 > (double)num14)
                            num14 = num16;
                    }
                }
                else if (str1 == "ND")
                {
                    num11 = 0.0f;
                    num12 = 20f;
                    num13 = 0.0f;
                    num14 = 100f;
                }
                else
                {
                    num11 = 0.0f;
                    num12 = 100f;
                    num13 = 0.0f;
                    num14 = 20f;
                }
            }
            else
            {
                num11 = 0.0f;
                num12 = 100f;
                num13 = 0.0f;
                num14 = 20f;
                if (qm.AFCS != "" || qm.CFCS != "")
                {
                    if (str1 == "ND")
                    {
                        for (int index = 0; index < 100; ++index)
                        {
                            Sample sample1 = new Sample();
                            sample1.ND = new Decimal?(Convert.ToDecimal(index));
                            Sample sample2 = sample1;
                            Decimal k3 = qm.K3;
                            nullable1 = sample1.ND;
                            Decimal? nullable2;
                            Decimal? nullable3;
                            if (!nullable1.HasValue)
                            {
                                nullable2 = new Decimal?();
                                nullable3 = nullable2;
                            }
                            else
                                nullable3 = new Decimal?(k3 * nullable1.GetValueOrDefault());
                            nullable1 = nullable3;
                            nullable2 = sample1.ND;
                            Decimal? nullable4;
                            Decimal? nullable5;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable5 = nullable4;
                            }
                            else
                                nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                            nullable1 = nullable5;
                            nullable2 = sample1.ND;
                            Decimal? nullable6;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable6 = nullable4;
                            }
                            else
                                nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                            nullable1 = nullable6;
                            Decimal k2 = qm.K2;
                            nullable2 = sample1.ND;
                            Decimal? nullable7;
                            if (!nullable2.HasValue)
                            {
                                nullable4 = new Decimal?();
                                nullable7 = nullable4;
                            }
                            else
                                nullable7 = new Decimal?(k2 * nullable2.GetValueOrDefault());
                            nullable2 = nullable7;
                            nullable4 = sample1.ND;
                            nullable2 = nullable2.HasValue & nullable4.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new Decimal?();
                            Decimal? nullable8;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable8 = nullable4;
                            }
                            else
                                nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                            nullable1 = nullable8;
                            Decimal k1 = qm.K1;
                            nullable2 = sample1.ND;
                            Decimal? nullable9;
                            if (!nullable2.HasValue)
                            {
                                nullable4 = new Decimal?();
                                nullable9 = nullable4;
                            }
                            else
                                nullable9 = new Decimal?(k1 * nullable2.GetValueOrDefault());
                            nullable2 = nullable9;
                            Decimal? nullable10;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable10 = nullable4;
                            }
                            else
                                nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                            nullable1 = nullable10;
                            Decimal k0 = qm.K0;
                            Decimal? nullable11;
                            if (!nullable1.HasValue)
                            {
                                nullable2 = new Decimal?();
                                nullable11 = nullable2;
                            }
                            else
                                nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k0);
                            sample2.XGD = nullable11;
                            source.Add(sample1);
                        }
                        num3 = 0.0f;
                        num2 = 100f;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Min();
                        float num4 = (float)nullable1.Value;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Max();
                        float num5 = (float)nullable1.Value;
                        if ((double)num4 < (double)num13)
                            num13 = num4;
                        if ((double)num5 > (double)num14)
                            num14 = num5;
                    }
                    else
                    {
                        for (int index = 0; index < 100; ++index)
                        {
                            Sample sample1 = new Sample();
                            sample1.XGD = new Decimal?(Convert.ToDecimal(index));
                            Sample sample2 = sample1;
                            Decimal k13 = qm.K13;
                            nullable1 = sample1.XGD;
                            Decimal? nullable2;
                            Decimal? nullable3;
                            if (!nullable1.HasValue)
                            {
                                nullable2 = new Decimal?();
                                nullable3 = nullable2;
                            }
                            else
                                nullable3 = new Decimal?(k13 * nullable1.GetValueOrDefault());
                            nullable1 = nullable3;
                            nullable2 = sample1.XGD;
                            Decimal? nullable4;
                            Decimal? nullable5;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable5 = nullable4;
                            }
                            else
                                nullable5 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                            nullable1 = nullable5;
                            nullable2 = sample1.XGD;
                            Decimal? nullable6;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable6 = nullable4;
                            }
                            else
                                nullable6 = new Decimal?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault());
                            nullable1 = nullable6;
                            Decimal k12 = qm.K12;
                            nullable2 = sample1.XGD;
                            Decimal? nullable7;
                            if (!nullable2.HasValue)
                            {
                                nullable4 = new Decimal?();
                                nullable7 = nullable4;
                            }
                            else
                                nullable7 = new Decimal?(k12 * nullable2.GetValueOrDefault());
                            nullable2 = nullable7;
                            nullable4 = sample1.XGD;
                            nullable2 = nullable2.HasValue & nullable4.HasValue ? new Decimal?(nullable2.GetValueOrDefault() * nullable4.GetValueOrDefault()) : new Decimal?();
                            Decimal? nullable8;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable8 = nullable4;
                            }
                            else
                                nullable8 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                            nullable1 = nullable8;
                            Decimal k11 = qm.K11;
                            nullable2 = sample1.XGD;
                            Decimal? nullable9;
                            if (!nullable2.HasValue)
                            {
                                nullable4 = new Decimal?();
                                nullable9 = nullable4;
                            }
                            else
                                nullable9 = new Decimal?(k11 * nullable2.GetValueOrDefault());
                            nullable2 = nullable9;
                            Decimal? nullable10;
                            if (!(nullable1.HasValue & nullable2.HasValue))
                            {
                                nullable4 = new Decimal?();
                                nullable10 = nullable4;
                            }
                            else
                                nullable10 = new Decimal?(nullable1.GetValueOrDefault() + nullable2.GetValueOrDefault());
                            nullable1 = nullable10;
                            Decimal k10 = qm.K10;
                            Decimal? nullable11;
                            if (!nullable1.HasValue)
                            {
                                nullable2 = new Decimal?();
                                nullable11 = nullable2;
                            }
                            else
                                nullable11 = new Decimal?(nullable1.GetValueOrDefault() + k10);
                            sample2.ND = nullable11;
                            source.Add(sample1);
                        }
                        num3 = 0.0f;
                        num2 = 100f;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Min();
                        float num4 = (float)nullable1.Value;
                        nullable1 = source.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Max();
                        float num5 = (float)nullable1.Value;
                        if ((double)num4 < (double)num13)
                            num13 = num4;
                        if ((double)num5 > (double)num14)
                            num14 = num5;
                    }
                }
            }
            float x1 = num7;
            float y1 = num9 + 5f;
            graphics.DrawString(num11.ToString(format1), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x1, y1));
            SizeF sizeF3 = graphics.MeasureString(num12.ToString(format1), new Font("Segoe UI", (float)num1));
            float x2 = num8 - sizeF3.Width;
            graphics.DrawString(num12.ToString(format1), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x2, y1));
            SizeF sizeF4 = graphics.MeasureString(num13.ToString(format2), new Font("Segoe UI", (float)num1));
            float x3 = num7 - sizeF4.Width;
            float y2 = num9 - sizeF4.Height / 2f;
            graphics.DrawString(num13.ToString(format2), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x3, y2));
            SizeF sizeF5 = graphics.MeasureString(num14.ToString(format2), new Font("Segoe UI", (float)num1));
            float x4 = num7 - sizeF5.Width;
            float y3 = num10 - sizeF5.Height / 2f;
            graphics.DrawString(num14.ToString(format2), new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x4, y3));
            for (int index = 1; index < 4; ++index)
            {
                Pen pen = new Pen(Color.Black, 1f);
                pen.DashStyle = DashStyle.Dot;
                graphics.DrawLine(pen, num7 + (float)(((double)num8 - (double)num7) * (double)index / 4.0), num9, num7 + (float)(((double)num8 - (double)num7) * (double)index / 4.0), num10);
                graphics.DrawLine(pen, num7, num10 + (float)(((double)num9 - (double)num10) * (double)index / 4.0), num8, num10 + (float)(((double)num9 - (double)num10) * (double)index / 4.0));
            }
            float x5 = num7 + (float)(((double)num8 - (double)num7 - (double)sizeF1.Width) / 2.0);
            float y4 = num9 + 5f;
            float y5 = num10 + (float)(((double)num9 - (double)num10 - (double)sizeF2.Width) / 2.0);
            if (str1 == "ND")
            {
                float x6 = num7 - graphics.MeasureString("Abs", new Font("Segoe UI", (float)num1)).Width;
                graphics.DrawString(qm.QPar.Unit, new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
                graphics.DrawString("Abs", new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
            }
            else
            {
                float x6 = num7 - graphics.MeasureString(qm.QPar.Unit, new Font("Segoe UI", (float)num1)).Width;
                graphics.DrawString(qm.QPar.Unit, new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
                graphics.DrawString("Abs", new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
            }
            if ((double)num12 - (double)num11 == 0.0 || (double)num14 - (double)num13 == 0.0)
                return;
            double num17 = ((double)num8 - (double)num7) / ((double)num12 - (double)num11);
            double num18 = ((double)num9 - (double)num10) / ((double)num14 - (double)num13);
            if (source.Count > 0)
            {
                if (str1 == "ND")
                {
                    List<Sample> list = source.OrderBy<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).ToList<Sample>();
                    double num4 = (double)num7 + (Convert.ToDouble((object)list[list.Count<Sample>() - 1].ND) - (double)num11) * num17;
                    double num5 = (double)num9 - (Convert.ToDouble((object)list[list.Count<Sample>() - 1].XGD) - (double)num13) * num18;
                    for (int index = list.Count<Sample>() - 1; index >= 0; --index)
                    {
                        double num6 = (double)num7 + (Convert.ToDouble((object)list[index].ND) - (double)num11) * num17;
                        double num15 = Convert.ToDouble((object)list[index].XGD) >= (double)num13 ? (Convert.ToDouble((object)list[index].XGD) <= (double)num14 ? (double)num9 - (Convert.ToDouble((object)list[index].XGD) - (double)num13) * num18 : (double)num10) : (double)num9;
                        graphics.DrawLine(new Pen(Color.Red, 1f), (float)num4, (float)num5, (float)num6, (float)num15);
                        num4 = num6;
                        num5 = num15;
                    }
                    if (qm.SamList != null && qm.SamList.Count > 0)
                    {
                        for (int index = 0; index < qm.SamList.Count; ++index)
                        {
                            double num6 = (double)num7 + (Convert.ToDouble((object)qm.SamList[index].ND) - (double)num11) * num17;
                            double num15 = Convert.ToDouble((object)qm.SamList[index].XGD) >= (double)num13 ? (Convert.ToDouble((object)qm.SamList[index].XGD) <= (double)num14 ? (double)num9 - (Convert.ToDouble((object)qm.SamList[index].XGD) - (double)num13) * num18 : (double)num10) : (double)num9;
                            graphics.DrawEllipse(new Pen(Color.Blue, 2f), Convert.ToInt32(num6) - 1, Convert.ToInt32(num15) - 1, 3, 3);
                        }
                    }
                }
                else
                {
                    List<Sample> list = source.OrderBy<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).ToList<Sample>();
                    double num4 = (double)num7 + (Convert.ToDouble((object)list[list.Count<Sample>() - 1].XGD) - (double)num11) * num17;
                    double num5 = (double)num9 - (Convert.ToDouble((object)list[list.Count<Sample>() - 1].ND) - (double)num13) * num18;
                    for (int index = list.Count<Sample>() - 2; index >= 0; --index)
                    {
                        double num6 = (double)num7 + (Convert.ToDouble((object)list[index].XGD) - (double)num11) * num17;
                        double num15 = Convert.ToDouble((object)list[index].ND) >= (double)num13 ? (Convert.ToDouble((object)list[index].ND) <= (double)num14 ? (double)num9 - (Convert.ToDouble((object)list[index].ND) - (double)num13) * num18 : (double)num10) : (double)num9;
                        graphics.DrawLine(new Pen(Color.Red, 1f), (float)num4, (float)num5, (float)num6, (float)num15);
                        num4 = num6;
                        num5 = num15;
                    }
                    if (qm.SamList != null && qm.SamList.Count > 0)
                    {
                        for (int index = 0; index < qm.SamList.Count; ++index)
                        {
                            double num6 = (double)num7 + (Convert.ToDouble((object)qm.SamList[index].XGD) - (double)num11) * num17;
                            double num15 = Convert.ToDouble((object)qm.SamList[index].ND) >= (double)num13 ? (Convert.ToDouble((object)qm.SamList[index].ND) <= (double)num14 ? (double)num9 - (Convert.ToDouble((object)qm.SamList[index].ND) - (double)num13) * num18 : (double)num10) : (double)num9;
                            graphics.DrawEllipse(new Pen(Color.Blue, 2f), Convert.ToInt32(num6) - 1, Convert.ToInt32(num15) - 1, 3, 3);
                        }
                    }
                }
            }
            this.picCurve.Image = (Image)bitmap;
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows.Count != 0)
            {
                if (this.dataGridView1.SelectedCells.Count > 0 && this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Tag == null)
                    return;
                if (e.ColumnIndex == 2)
                {
                    try
                    {
                        this.QM.SamList[e.RowIndex].XGD = new Decimal?(Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells[2].Value));
                        this.CaculateK(this.QM);
                        this.QM.K0 = this.K0;
                        this.QM.K1 = this.K1;
                        this.QM.K2 = this.K2;
                        this.QM.K10 = this.K10;
                        this.QM.K11 = this.K11;
                        this.QM.K12 = this.K12;
                        this.QM.K13 = this.K13;
                        this.QM.R = new Decimal?(this.R);
                        CommonFun.GenerateFCS(this.QM);
                        this.DrawCurve(this.QM);
                    }
                    catch
                    {
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    try
                    {
                        this.QM.SamList[e.RowIndex].ND = new Decimal?(Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells[3].Value));
                        this.CaculateK(this.QM);
                        this.QM.K0 = this.K0;
                        this.QM.K1 = this.K1;
                        this.QM.K2 = this.K2;
                        this.QM.K10 = this.K10;
                        this.QM.K11 = this.K11;
                        this.QM.K12 = this.K12;
                        this.QM.K13 = this.K13;
                        CommonFun.GenerateFCS(this.QM);
                        this.DrawCurve(this.QM);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    if (e.ColumnIndex != 4)
                        return;
                    try
                    {
                        this.QM.SamList[e.RowIndex].C_bz = this.dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void CaculateK(QuaMethod qm)
        {
            List<Sample> list = qm.SamList.Where<Sample>((Func<Sample, bool>)(s => !s.IsExclude)).ToList<Sample>();
            if (list.Count < 2)
                return;
            double[] arrX = new double[list.Count];
            double[] arrY = new double[list.Count];
            Decimal num1;
            if (qm.QPar.Fitting == CommonFun.GetLanText("linear"))
            {
                Decimal num2 = list.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Sum().Value / (Decimal)list.Count<Sample>();
                Decimal num3 = list.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Sum().Value / (Decimal)list.Count<Sample>();
                Decimal num4 = 0M;
                Decimal num5 = 0M;
                for (int index = 0; index < list.Count; ++index)
                {
                    num4 += (list[index].ND.Value - num2) * (list[index].XGD.Value - num3);
                    num5 += (list[index].ND.Value - num2) * (list[index].ND.Value - num2);
                    arrX[index] = Convert.ToDouble((object)list[index].ND);
                    arrY[index] = Convert.ToDouble((object)list[index].XGD);
                }
                if (num5 == 0M)
                    return;
                this.K1 = Convert.ToDecimal((num4 / num5).ToString("f4"));
                if (!qm.QPar.ZeroB)
                {
                    num1 = num3 - this.K1 * num2;
                    this.K0 = Convert.ToDecimal(num1.ToString("f4"));
                }
                Decimal num6 = list.Select<Sample, Decimal>((Func<Sample, Decimal>)(s => s.XGD.Value)).Sum() / (Decimal)list.Count<Sample>();
                Decimal num7 = list.Select<Sample, Decimal>((Func<Sample, Decimal>)(s => s.ND.Value)).Sum() / (Decimal)list.Count<Sample>();
                Decimal num8 = 0M;
                Decimal num9 = 0M;
                for (int index = 0; index < list.Count; ++index)
                {
                    num8 += (list[index].XGD.Value - num6) * (list[index].ND.Value - num7);
                    num9 += (list[index].XGD.Value - num6) * (list[index].XGD.Value - num6);
                    arrX[index] = Convert.ToDouble((object)list[index].XGD);
                    arrY[index] = Convert.ToDouble((object)list[index].ND);
                }
                if (num9 == 0M)
                    return;
                num1 = num8 / num9;
                this.K11 = Convert.ToDecimal(num1.ToString("f4"));
                if (!qm.QPar.ZeroB)
                {
                    num1 = num7 - this.K11 * num6;
                    this.K10 = Convert.ToDecimal(num1.ToString("f4"));
                }
            }
            else if (qm.QPar.Fitting == CommonFun.GetLanText("squar"))
            {
                for (int index = 0; index < list.Count; ++index)
                {
                    arrX[index] = Convert.ToDouble((object)list[index].ND);
                    arrY[index] = Convert.ToDouble((object)list[index].XGD);
                }
                double[] numArray1 = this.MultiLine(arrX, arrY, list.Count, 2);
                this.K0 = Convert.ToDecimal(numArray1[0].ToString("f4"));
                this.K1 = Convert.ToDecimal(numArray1[1].ToString("f4"));
                this.K2 = Convert.ToDecimal(numArray1[2].ToString("f4"));
                for (int index = 0; index < list.Count; ++index)
                {
                    arrX[index] = Convert.ToDouble((object)list[index].XGD);
                    arrY[index] = Convert.ToDouble((object)list[index].ND);
                }
                double[] numArray2 = this.MultiLine(arrX, arrY, list.Count, 2);
                this.K10 = Convert.ToDecimal(numArray2[0].ToString("f4"));
                this.K11 = Convert.ToDecimal(numArray2[1].ToString("f4"));
                this.K12 = Convert.ToDecimal(numArray2[2].ToString("f4"));
            }
            else if (qm.QPar.Fitting == CommonFun.GetLanText("qube"))
            {
                for (int index = 0; index < list.Count; ++index)
                {
                    arrX[index] = Convert.ToDouble((object)list[index].ND);
                    arrY[index] = Convert.ToDouble((object)list[index].XGD);
                }
                double[] numArray1 = this.MultiLine(arrX, arrY, list.Count, 2);
                this.K0 = Convert.ToDecimal(numArray1[0].ToString("f4"));
                this.K1 = Convert.ToDecimal(numArray1[1].ToString("f4"));
                this.K2 = Convert.ToDecimal(numArray1[2].ToString("f4"));
                for (int index = 0; index < list.Count; ++index)
                {
                    arrX[index] = Convert.ToDouble((object)list[index].XGD);
                    arrY[index] = Convert.ToDouble((object)list[index].ND);
                }
                double[] numArray2 = this.MultiLine(arrX, arrY, list.Count, 3);
                this.K10 = Convert.ToDecimal(numArray2[0].ToString("f4"));
                this.K11 = Convert.ToDecimal(numArray2[1].ToString("f4"));
                this.K12 = Convert.ToDecimal(numArray2[2].ToString("f4"));
            }
            Decimal num10 = list.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.ND)).Sum().Value / (Decimal)list.Count<Sample>();
            Decimal num11 = list.Select<Sample, Decimal?>((Func<Sample, Decimal?>)(s => s.XGD)).Sum().Value / (Decimal)list.Count<Sample>();
            Decimal num12 = 0M;
            Decimal num13 = 0M;
            Decimal num14 = 0M;
            for (int index = 0; index < list.Count; ++index)
            {
                num12 += (list[index].ND.Value - num10) * (list[index].XGD.Value - num11);
                num13 += (list[index].ND.Value - num10) * (list[index].ND.Value - num10);
                num14 += (list[index].XGD.Value - num11) * (list[index].XGD.Value - num11);
            }
            if (num13 == 0M || num14 == 0M)
            {
                QuaMethod quaMethod = qm;
                this.R = num1 = 1M;
                Decimal? nullable = new Decimal?(num1);
                quaMethod.R = nullable;
            }
            else
            {
                QuaMethod quaMethod = qm;
                this.R = num1 = Convert.ToDecimal(Convert.ToDouble(num12) / (Math.Sqrt(Convert.ToDouble(num13)) * Math.Sqrt(Convert.ToDouble(num14))));
                Decimal? nullable = new Decimal?(num1);
                quaMethod.R = nullable;
            }
        }
        public double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)
        {
            int n = dimension + 1;
            double[,] Guass = new double[n, n + 1];
            for (int n1 = 0; n1 < n; ++n1)
            {
                int index;
                for (index = 0; index < n; ++index)
                    Guass[n1, index] = this.SumArr(arrX, index + n1, length);
                Guass[n1, index] = this.SumArr(arrX, n1, arrY, 1, length);
            }
            return this.ComputGauss(Guass, n);
        }

        private double SumArr(double[] arr, int n, int length)
        {
            double num = 0.0;
            for (int index = 0; index < length; ++index)
            {
                if (arr[index] != 0.0 || n != 0)
                    num += Math.Pow(arr[index], (double)n);
                else
                    ++num;
            }
            return num;
        }

        private double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double num = 0.0;
            for (int index = 0; index < length; ++index)
            {
                if ((arr1[index] != 0.0 || n1 != 0) && (arr2[index] != 0.0 || n2 != 0))
                    num += Math.Pow(arr1[index], (double)n1) * Math.Pow(arr2[index], (double)n2);
                else
                    ++num;
            }
            return num;
        }

        private double[] ComputGauss(double[,] Guass, int n)
        {
            double[] numArray = new double[n];
            for (int index = 0; index < n; ++index)
                numArray[index] = 0.0;
            for (int index1 = 0; index1 < n; ++index1)
            {
                double num1 = 0.0;
                int index2 = index1;
                for (int index3 = index1; index3 < n; ++index3)
                {
                    if (Math.Abs(Guass[index3, index1]) > num1)
                    {
                        num1 = Guass[index3, index1];
                        index2 = index3;
                    }
                }
                if (index2 != index1)
                {
                    for (int index3 = index1; index3 < n + 1; ++index3)
                    {
                        double num2 = Guass[index1, index3];
                        Guass[index1, index3] = Guass[index2, index3];
                        Guass[index2, index3] = num2;
                    }
                }
                if (0.0 == num1)
                    return numArray;
                for (int index3 = index1 + 1; index3 < n; ++index3)
                {
                    double num2 = Guass[index3, index1];
                    for (int index4 = index1; index4 < n + 1; ++index4)
                        Guass[index3, index4] = Guass[index3, index4] - Guass[index1, index4] * num2 / Guass[index1, index1];
                }
            }
            for (int index1 = n - 1; index1 >= 0; --index1)
            {
                double num = 0.0;
                for (int index2 = index1 + 1; index2 < n; ++index2)
                    num += Guass[index1, index2] * numArray[index2];
                numArray[index1] = (Guass[index1, n] - num) / Guass[index1, index1];
            }
            return numArray;
        }

        private void ConvertFcs()
        {
            if (this.QM.QPar.Fitting == CommonFun.GetLanText("linear"))
            {
                if (this.K1 != 0M)
                {
                    this.QM.K10 = this.K10 = Convert.ToDecimal((-this.K0 / this.K1).ToString("f4"));
                    QuaMethod qm = this.QM;
                    Decimal num1 = 1M / this.K1;
                    this.K11 = num1 = Convert.ToDecimal(num1.ToString("f4"));
                    Decimal num2 = num1;
                    qm.K11 = num2;
                }
                else
                {
                    this.QM.K10 = this.K10 = 0M;
                    this.QM.K11 = this.K11 = 0M;
                }
            }
            else if (this.QM.QPar.Fitting == CommonFun.GetLanText("squar") || !(this.QM.QPar.Fitting == CommonFun.GetLanText("qube")));
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowi = e.RowIndex;
            if (rowi != -1)
            {
                if (this.dataGridView1.Rows[rowi].Tag == null)
                    return;
                if (e.ColumnIndex == 3 && !this.dataGridView1.Columns[3].ReadOnly)
                {
                    using (InputDataFrm frm1 = new InputDataFrm())
                    {
                        frm1.txtValue.KeyDown += ((senders, es) =>
                        {
                            if (es.Key != Key.Return)
                                return;
                            try
                            {
                                Convert.ToDecimal(frm1.txtValue.Text);
                                this.dataGridView1.Rows[rowi].Cells[3].Value = (object)frm1.txtValue.Text;
                                if (this.QM.Page == 1)
                                    CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("conce"), false);
                                else
                                    CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("conce"), false);
                                frm1.Close();
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                            }
                        });
                        frm1.btnOK.Click += ((param0, param1) =>
                        {
                            try
                            {
                                Convert.ToDecimal(frm1.txtValue.Text);
                                this.dataGridView1.Rows[rowi].Cells[3].Value = (object)frm1.txtValue.Text;
                                if (this.QM.Page == 1)
                                    CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("conce"), false);
                                else
                                    CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("conce"), false);
                                frm1.Close();
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                            }
                        });
                        frm1.btnOK.PreviewMouseDown += ((param0, param1) =>
                        {
                            try
                            {
                                Convert.ToDecimal(frm1.txtValue.Text);
                                this.dataGridView1.Rows[rowi].Cells[3].Value = (object)frm1.txtValue.Text;
                                if (this.QM.Page == 1)
                                    CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("conce"), false);
                                else
                                    CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("conce"), false);
                                frm1.Close();
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                            }
                        });
                        frm1.ShowDialog();
                    }
                }
                if (e.ColumnIndex == 2 && !this.dataGridView1.Columns[2].ReadOnly)
                {
                    using (InputDataFrm frm1 = new InputDataFrm())
                    {
                        frm1.txtValue.KeyDown += ((senders, es) =>
                        {
                            if (es.Key != Key.Return)
                                return;
                            try
                            {
                                Convert.ToDecimal(frm1.txtValue.Text);
                                this.dataGridView1.Rows[rowi].Cells[2].Value = (object)frm1.txtValue.Text;
                                if (this.QM.Page == 1)
                                    CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("Abs"), false);
                                else
                                    CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("Abs"), false);
                                frm1.Close();
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                            }
                        });
                        frm1.btnOK.Click += ((param0, param1) =>
                        {
                            try
                            {
                                Convert.ToDecimal(frm1.txtValue.Text);
                                this.dataGridView1.Rows[rowi].Cells[2].Value = (object)frm1.txtValue.Text;
                                if (this.QM.Page == 1)
                                    CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("Abs"), false);
                                else
                                    CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("Abs"), false);
                                frm1.Close();
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                            }
                        });
                        frm1.btnOK.PreviewMouseDown += ((param0, param1) =>
                        {
                            try
                            {
                                Convert.ToDecimal(frm1.txtValue.Text);
                                this.dataGridView1.Rows[rowi].Cells[2].Value = (object)frm1.txtValue.Text;
                                if (this.QM.Page == 1)
                                    CommonFun.InsertLog(CommonFun.GetLanText("Quantitation"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("Abs"), false);
                                else
                                    CommonFun.InsertLog(CommonFun.GetLanText("dna"), CommonFun.GetLanText("inputs") + " " + CommonFun.GetLanText("Abs"), false);
                                frm1.Close();
                            }
                            catch
                            {
                                CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                            }
                        });
                        frm1.ShowDialog();
                    }
                }
                if(e.ColumnIndex == 4)
                {
                    using (KeyBoard frm2 = new KeyBoard("", ""))
                    {
                        // frm2.lbltitle.Text = "reason";
                        frm2.Loaded += (System.Windows.RoutedEventHandler)((param2_1, param2_2) => {
                            frm2.Activate();
                        });
                        frm2.btnOK.PreviewMouseDown += ((param0, param1) =>
                        {
                            string reason = frm2.txtValue.Text;
                            this.dataGridView1.Rows[rowi].Cells[4].Value = reason;
                            // CommonFun.Set("DDTime", "0");
                            //   CommonFun.InsertLog("System", "Reset deuterium lamp time" + "," + "Reason" + ":" + reason, true);
                            frm2.Close();

                        });
                       frm2.ShowDialog();
                    }
                }
            
                if (e.ColumnIndex == 0)
                {
                    Decimal? nullable = this.QM.SamList[e.RowIndex].XGD;
                    int num;
                    if (nullable.HasValue)
                    {
                        nullable = this.QM.SamList[e.RowIndex].ND;
                        num = !nullable.HasValue ? 1 : 0;
                    }
                    else
                        num = 1;
                    if (num == 0)
                    {
                        if (this.dataGridView1.SelectedCells[0].Tag.ToString() == "off")
                        {
                            this.dataGridView1.Rows[rowi].Cells["ColPC"].Value = (object)null;
                            this.dataGridView1.Rows[rowi].Cells["ColPC"].Value = (object)Resources.UI_DB_Check_Checked;
                            this.dataGridView1.Rows[rowi].Cells["ColPC"].Tag = (object)"on";
                            this.QM.SamList[e.RowIndex].IsExclude = true;
                        }
                        else
                        {
                            this.dataGridView1.Rows[rowi].Cells["ColPC"].Value = (object)null;
                            this.dataGridView1.Rows[rowi].Cells["ColPC"].Value = (object)Resources.UI_DB_Check_Unchecked;
                            this.dataGridView1.Rows[rowi].Cells["ColPC"].Tag = (object)"off";
                            this.QM.SamList[e.RowIndex].IsExclude = false;
                        }
                        this.CaculateK(this.QM);
                        this.QM.K0 = this.K0;
                        this.QM.K1 = this.K1;
                        this.QM.K2 = this.K2;
                        this.QM.K10 = this.K10;
                        this.QM.K11 = this.K11;
                        this.QM.K12 = this.K12;
                        this.QM.K13 = this.K13;
                        CommonFun.GenerateFCS(this.QM);
                        this.DrawCurve(this.QM);
                    }
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnLast_Click(object sender, EventArgs e)
        {

        }
    }
}
