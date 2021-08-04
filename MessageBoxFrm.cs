// Decompiled with JetBrains decompiler
// Type: UVStudio.MessageBoxFrm
// Assembly: UV Studio, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null
// MVID: B671728E-936D-44C2-9ED5-B691939AD932
// Assembly location: H:\UV Studio\UV Studio\UV Studio.exe

using UVStudio.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace UVStudio
{
  public class MessageBoxFrm : Form
  {
    private string lanvalue = "";
    private string msg = "";
    private string titletype = "";
    private IContainer components = (IContainer) null;
    private Label label1;
    public Label lbltitle;
    public Button btnOK;

    public MessageBoxFrm(string message, string type)
    {
      this.lanvalue = CommonFun.GetAppConfig("Language");
      //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(this.lanvalue);
      this.InitializeComponent();
      this.ControlBox = false;
      this.msg = message;
      this.titletype = type;
    }

    private void MessageBoxFrm_Load(object sender, EventArgs e)
    {
      this.BackColor = Color.LightGray;
      this.TransparencyKey = Color.LightGray;
      this.label1.Text = this.msg;
      this.lbltitle.Text = this.titletype;
      switch (this.titletype)
      {
      }
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.label1 = new System.Windows.Forms.Label();
            this.lbltitle = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.label1.Location = new System.Drawing.Point(46, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(541, 229);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbltitle
            // 
            this.lbltitle.BackColor = System.Drawing.Color.Transparent;
            this.lbltitle.Font = new System.Drawing.Font("Microsoft YaHei", 14F);
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbltitle.Location = new System.Drawing.Point(215, 6);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(200, 30);
            this.lbltitle.TabIndex = 17;
            this.lbltitle.Text = "Заголовок";
            this.lbltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbltitle.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.btnOK.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(12, 285);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(618, 53);
            this.btnOK.TabIndex = 21;
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // MessageBoxFrm
            // 
            this.BackColor = System.Drawing.Color.LightGray;
            this.BackgroundImage = global::UVStudio.Properties.Resources.Dialog;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(639, 339);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbltitle);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.LightGray;
            this.Load += new System.EventHandler(this.MessageBoxFrm_Load);
            this.ResumeLayout(false);

    }
  }
}
