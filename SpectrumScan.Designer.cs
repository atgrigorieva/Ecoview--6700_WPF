namespace UVStudio
{
    partial class SpectrumScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tt = new System.Windows.Forms.Timer(this.components);
            this.lblWL = new System.Windows.Forms.Label();
            this.lblmode = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSample = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pibInOut = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.picTop = new System.Windows.Forms.PictureBox();
            this.picCurve = new System.Windows.Forms.PictureBox();
            this.dgvPoint = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnBlank = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCSV = new System.Windows.Forms.Button();
            this.btnToExcel = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.picOut = new System.Windows.Forms.PictureBox();
            this.printScanSpectrum = new System.Drawing.Printing.PrintDocument();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOut)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(978, 156);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 342);
            this.dataGridView1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(508, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(213, 25);
            this.label4.TabIndex = 21;
            this.label4.Text = "Сканирование спектра";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.ForeColor = System.Drawing.Color.LimeGreen;
            this.progressBar1.Location = new System.Drawing.Point(5, 520);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1213, 23);
            this.progressBar1.TabIndex = 20;
            // 
            // tt
            // 
            this.tt.Interval = 1000;
            // 
            // lblWL
            // 
            this.lblWL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWL.AutoSize = true;
            this.lblWL.BackColor = System.Drawing.Color.Transparent;
            this.lblWL.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblWL.Location = new System.Drawing.Point(842, 117);
            this.lblWL.Name = "lblWL";
            this.lblWL.Size = new System.Drawing.Size(0, 25);
            this.lblWL.TabIndex = 14;
            // 
            // lblmode
            // 
            this.lblmode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblmode.AutoSize = true;
            this.lblmode.BackColor = System.Drawing.Color.Transparent;
            this.lblmode.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblmode.Location = new System.Drawing.Point(488, 117);
            this.lblmode.Name = "lblmode";
            this.lblmode.Size = new System.Drawing.Size(0, 25);
            this.lblmode.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(842, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 25);
            this.label3.TabIndex = 16;
            this.label3.Text = "Текущая длина волны";
            // 
            // lblSample
            // 
            this.lblSample.AutoSize = true;
            this.lblSample.BackColor = System.Drawing.Color.Transparent;
            this.lblSample.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblSample.Location = new System.Drawing.Point(11, 117);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(0, 25);
            this.lblSample.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(488, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 25);
            this.label2.TabIndex = 17;
            this.label2.Text = "Режим";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(11, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 25);
            this.label1.TabIndex = 18;
            this.label1.Text = "Текущий образец";
            // 
            // pibInOut
            // 
            this.pibInOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pibInOut.BackColor = System.Drawing.Color.Transparent;
            this.pibInOut.BackgroundImage = global::UVStudio.Properties.Resources.list;
            this.pibInOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pibInOut.FlatAppearance.BorderColor = System.Drawing.Color.SeaShell;
            this.pibInOut.FlatAppearance.BorderSize = 0;
            this.pibInOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pibInOut.Location = new System.Drawing.Point(1143, 88);
            this.pibInOut.Name = "pibInOut";
            this.pibInOut.Size = new System.Drawing.Size(75, 54);
            this.pibInOut.TabIndex = 12;
            this.pibInOut.TabStop = false;
            this.pibInOut.UseVisualStyleBackColor = false;
            this.pibInOut.Click += new System.EventHandler(this.PibInOut_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BackgroundImage = global::UVStudio.Properties.Resources.Icon_Home;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.SeaShell;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Location = new System.Drawing.Point(1143, 7);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 54);
            this.btnBack.TabIndex = 13;
            this.btnBack.TabStop = false;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // picTop
            // 
            this.picTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picTop.Location = new System.Drawing.Point(12, 3);
            this.picTop.Name = "picTop";
            this.picTop.Size = new System.Drawing.Size(954, 341);
            this.picTop.TabIndex = 10;
            this.picTop.TabStop = false;
            // 
            // picCurve
            // 
            this.picCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picCurve.Location = new System.Drawing.Point(12, 157);
            this.picCurve.Name = "picCurve";
            this.picCurve.Size = new System.Drawing.Size(954, 341);
            this.picCurve.TabIndex = 11;
            this.picCurve.TabStop = false;
            // 
            // dgvPoint
            // 
            this.dgvPoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPoint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPoint.Location = new System.Drawing.Point(978, 156);
            this.dgvPoint.Name = "dgvPoint";
            this.dgvPoint.Size = new System.Drawing.Size(240, 342);
            this.dgvPoint.TabIndex = 8;
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(978, 156);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(240, 342);
            this.dataGridView2.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImage = global::UVStudio.Properties.Resources.Title_Bar_01;
            this.panel1.Controls.Add(this.btnScan);
            this.panel1.Controls.Add(this.btnBlank);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnCSV);
            this.panel1.Controls.Add(this.btnToExcel);
            this.panel1.Controls.Add(this.btnSet);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(5, 558);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1231, 65);
            this.panel1.TabIndex = 22;
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnScan.Location = new System.Drawing.Point(1103, 9);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(123, 47);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "Измерение";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnBlank
            // 
            this.btnBlank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBlank.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnBlank.Location = new System.Drawing.Point(967, 9);
            this.btnBlank.Name = "btnBlank";
            this.btnBlank.Size = new System.Drawing.Size(130, 47);
            this.btnBlank.TabIndex = 4;
            this.btnBlank.Text = "Обнуление";
            this.btnBlank.UseVisualStyleBackColor = true;
            this.btnBlank.Click += new System.EventHandler(this.BtnBlank_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnPrint.Location = new System.Drawing.Point(255, 9);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(119, 47);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Печать";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCSV
            // 
            this.btnCSV.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCSV.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnCSV.Location = new System.Drawing.Point(380, 9);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.Size = new System.Drawing.Size(119, 47);
            this.btnCSV.TabIndex = 5;
            this.btnCSV.Text = "В CSV";
            this.btnCSV.UseVisualStyleBackColor = true;
            this.btnCSV.Click += new System.EventHandler(this.btnCSV_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnToExcel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnToExcel.Location = new System.Drawing.Point(508, 9);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(119, 47);
            this.btnToExcel.TabIndex = 5;
            this.btnToExcel.Text = "В Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Visible = false;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSet.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnSet.Location = new System.Drawing.Point(731, 9);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(119, 47);
            this.btnSet.TabIndex = 5;
            this.btnSet.Text = "Настройки";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnOpen.Location = new System.Drawing.Point(130, 9);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(119, 47);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.Text = "Открыть";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnSave.Location = new System.Drawing.Point(5, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 47);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // picOut
            // 
            this.picOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picOut.Location = new System.Drawing.Point(12, 157);
            this.picOut.Name = "picOut";
            this.picOut.Size = new System.Drawing.Size(954, 341);
            this.picOut.TabIndex = 23;
            this.picOut.TabStop = false;
            // 
            // printScanSpectrum
            // 
            this.printScanSpectrum.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printScanSpectrum_PrintPage);
            // 
            // SpectrumScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::UVStudio.Properties.Resources.Title_Bar_01;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1231, 620);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblWL);
            this.Controls.Add(this.lblmode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pibInOut);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.dgvPoint);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.picTop);
            this.Controls.Add(this.picCurve);
            this.Controls.Add(this.picOut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SpectrumScan";
            this.Text = "SpectrumScan";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer tt;
        private System.Windows.Forms.Label lblWL;
        private System.Windows.Forms.Label lblmode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button pibInOut;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.PictureBox picTop;
        private System.Windows.Forms.PictureBox picCurve;
        private System.Windows.Forms.DataGridView dgvPoint;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnBlank;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnToExcel;
        public System.Windows.Forms.PictureBox picOut;
        private System.Windows.Forms.Button btnPrint;
        private System.Drawing.Printing.PrintDocument printScanSpectrum;
        private System.Windows.Forms.Button btnCSV;
    }
}