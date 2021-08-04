namespace UVStudio
{
    partial class SettingPort
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingPort));
            this.NamePort = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.connection = new System.Windows.Forms.Button();
            this.selectPort = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // NamePort
            // 
            this.NamePort.AutoSize = true;
            this.NamePort.Location = new System.Drawing.Point(26, 36);
            this.NamePort.Name = "NamePort";
            this.NamePort.Size = new System.Drawing.Size(61, 13);
            this.NamePort.TabIndex = 7;
            this.NamePort.Text = "Имя порта";
            // 
            // connection
            // 
            this.connection.Location = new System.Drawing.Point(65, 80);
            this.connection.Name = "connection";
            this.connection.Size = new System.Drawing.Size(118, 23);
            this.connection.TabIndex = 9;
            this.connection.Text = "Подключить";
            this.connection.UseVisualStyleBackColor = true;
            this.connection.Click += new System.EventHandler(this.conection_Click);
            // 
            // selectPort
            // 
            this.selectPort.FormattingEnabled = true;
            this.selectPort.Location = new System.Drawing.Point(104, 33);
            this.selectPort.Name = "selectPort";
            this.selectPort.Size = new System.Drawing.Size(121, 21);
            this.selectPort.TabIndex = 8;
            // 
            // SettingPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 127);
            this.Controls.Add(this.NamePort);
            this.Controls.Add(this.connection);
            this.Controls.Add(this.selectPort);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingPort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выбор порта";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingPort_FormClosed);
            this.Load += new System.EventHandler(this.SettingPort_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NamePort;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Button connection;
        private System.Windows.Forms.ComboBox selectPort;
    }
}