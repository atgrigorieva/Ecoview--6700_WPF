namespace UVStudio
{
    partial class TimeScanFrm
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
            this.btncancel = new System.Windows.Forms.Button();
            this.lblWL = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.picCurve = new System.Windows.Forms.PictureBox();
            this.btnPause = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnBlank = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lbllocation = new System.Windows.Forms.Label();
            this.lblmode = new System.Windows.Forms.Label();
            this.lblSample = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.picTop = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pibInOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picCurve)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTop)).BeginInit();
            this.SuspendLayout();
            // 
            // btncancel
            // 
            this.btncancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btncancel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btncancel.Location = new System.Drawing.Point(679, 500);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(123, 47);
            this.btncancel.TabIndex = 39;
            this.btncancel.Text = "Завершить";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // lblWL
            // 
            this.lblWL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWL.AutoSize = true;
            this.lblWL.BackColor = System.Drawing.Color.Transparent;
            this.lblWL.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblWL.Location = new System.Drawing.Point(538, 112);
            this.lblWL.Name = "lblWL";
            this.lblWL.Size = new System.Drawing.Size(0, 25);
            this.lblWL.TabIndex = 47;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.tt_Tick);
            // 
            // picCurve
            // 
            this.picCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picCurve.Location = new System.Drawing.Point(6, 152);
            this.picCurve.Name = "picCurve";
            this.picCurve.Size = new System.Drawing.Size(366, 341);
            this.picCurve.TabIndex = 43;
            this.picCurve.TabStop = false;
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnPause.Location = new System.Drawing.Point(554, 500);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(119, 47);
            this.btnPause.TabIndex = 40;
            this.btnPause.Text = "Пауза";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImage = global::UVStudio.Properties.Resources.Title_Bar_01;
            this.panel1.Controls.Add(this.btnScan);
            this.panel1.Controls.Add(this.btnBlank);
            this.panel1.Controls.Add(this.btnSet);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(1, 553);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 65);
            this.panel1.TabIndex = 56;
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnScan.Location = new System.Drawing.Point(678, 9);
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
            this.btnBlank.Location = new System.Drawing.Point(542, 9);
            this.btnBlank.Name = "btnBlank";
            this.btnBlank.Size = new System.Drawing.Size(130, 47);
            this.btnBlank.TabIndex = 4;
            this.btnBlank.Text = "Обнуление";
            this.btnBlank.UseVisualStyleBackColor = true;
            this.btnBlank.Click += new System.EventHandler(this.btnBlank_Click);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSet.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.btnSet.Location = new System.Drawing.Point(351, 9);
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
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(378, 152);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(417, 342);
            this.dataGridView1.TabIndex = 41;
            // 
            // lbllocation
            // 
            this.lbllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbllocation.AutoSize = true;
            this.lbllocation.BackColor = System.Drawing.Color.Transparent;
            this.lbllocation.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lbllocation.Location = new System.Drawing.Point(372, 115);
            this.lbllocation.Name = "lbllocation";
            this.lbllocation.Size = new System.Drawing.Size(0, 25);
            this.lbllocation.TabIndex = 46;
            // 
            // lblmode
            // 
            this.lblmode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblmode.AutoSize = true;
            this.lblmode.BackColor = System.Drawing.Color.Transparent;
            this.lblmode.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblmode.Location = new System.Drawing.Point(231, 115);
            this.lblmode.Name = "lblmode";
            this.lblmode.Size = new System.Drawing.Size(0, 25);
            this.lblmode.TabIndex = 48;
            // 
            // lblSample
            // 
            this.lblSample.AutoSize = true;
            this.lblSample.BackColor = System.Drawing.Color.Transparent;
            this.lblSample.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblSample.Location = new System.Drawing.Point(7, 112);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(0, 25);
            this.lblSample.TabIndex = 53;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(231, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 25);
            this.label2.TabIndex = 51;
            this.label2.Text = "Режим";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 25);
            this.label1.TabIndex = 52;
            this.label1.Text = "Текущий образец";
            this.label1.Click += new System.EventHandler(this.label1_Click);
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
            this.btnBack.Location = new System.Drawing.Point(720, 7);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 54);
            this.btnBack.TabIndex = 45;
            this.btnBack.TabStop = false;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // picTop
            // 
            this.picTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picTop.Location = new System.Drawing.Point(6, 7);
            this.picTop.Name = "picTop";
            this.picTop.Size = new System.Drawing.Size(366, 341);
            this.picTop.TabIndex = 42;
            this.picTop.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(292, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(242, 25);
            this.label4.TabIndex = 55;
            this.label4.Text = "Временное сканирование";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.ForeColor = System.Drawing.Color.LimeGreen;
            this.progressBar1.Location = new System.Drawing.Point(6, 515);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(542, 23);
            this.progressBar1.TabIndex = 54;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(544, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 25);
            this.label5.TabIndex = 50;
            this.label5.Text = "WL(нм)";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(372, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 25);
            this.label3.TabIndex = 49;
            this.label3.Text = "Текущая точка";
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
            this.pibInOut.Location = new System.Drawing.Point(719, 86);
            this.pibInOut.Name = "pibInOut";
            this.pibInOut.Size = new System.Drawing.Size(75, 54);
            this.pibInOut.TabIndex = 44;
            this.pibInOut.TabStop = false;
            this.pibInOut.UseVisualStyleBackColor = false;
            this.pibInOut.Click += new System.EventHandler(this.pibInOut_Click);
            // 
            // TimeScanFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::UVStudio.Properties.Resources.Title_Bar_01;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(806, 620);
            this.Controls.Add(this.picCurve);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.lblWL);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lbllocation);
            this.Controls.Add(this.lblmode);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pibInOut);
            this.Controls.Add(this.picTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TimeScanFrm";
            this.Text = "TimeScanFrm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TimeScanFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCurve)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Label lblWL;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox picCurve;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnBlank;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lbllocation;
        private System.Windows.Forms.Label lblmode;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.PictureBox picTop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button pibInOut;
    }
}