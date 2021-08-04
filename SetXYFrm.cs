// Decompiled with JetBrains decompiler
// Type: MWaveProfessional.SetXYFrm
// Assembly: UV Studio, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null
// MVID: B671728E-936D-44C2-9ED5-B691939AD932
// Assembly location: H:\UV Studio\UV Studio\UV Studio.exe

/*
using UVStudio.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace UVStudio
{
  public class SetXYFrm : Form
  {
    private string lanvalue = "";
    private IContainer components = (IContainer) null;
    public Button Focusable="False"btnClose;
    public Button Focusable="False"btnOK;
    public Label lblXMin;
    public Label lblXMax;
    private Label label5;
    private Label label6;
    public Label label4;
    public Label label8;
    public Label lblYMin;
    public Label lblYMax;
    private Label label11;
    private Label label12;
    public Label label13;
    public Label label14;
    private Label label17;

    public SetXYFrm()
    {
      t//his.lanvalue = CommonFun.GetAppConfig("Language");
      //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(this.lanvalue);
      this.InitializeComponent();
      this.ControlBox = false;
    }

    private void SetXYFrm_Load(object sender, EventArgs e)
    {
    }

    private void lblXMax_PreviewMouseDown(object sender, EventArgs e)
    {
      using (InputDataFrm frm = new InputDataFrm())
      {
        frm.txtValue.KeyDown += (System.Windows.Input.KeyEventHandler) ((senders, es) =>
        {
          if (es.Key != System.Windows.Input.Key.Return)
            return;
          try
          {
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblXMax.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox("errordata", "Error");
          }
        });
        frm.btnOK.Click += (System.Windows.RoutedEventHandler) ((param0, param1) =>
        {
          try
          {
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblXMax.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox("errordata", "Error");
          }
        });
        int num = Convert.ToInt32(frm.ShowDialog());
      }
    }

    private void lblXMin_PreviewMouseDown(object sender, EventArgs e)
    {
      using (InputDataFrm frm = new InputDataFrm())
      {
        frm.txtValue.KeyDown += (System.Windows.RoutedEventHandler) ((senders, es) =>
        {
          if (es.Key != System.Windows.Input.Key.Return)
            return;
          try
          {
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblXMin.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox("errordata"), "Error");
          }
        });
        frm.btnOK.Click += (EventHandler) ((param0, param1) =>
        {
          try
          {
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblXMin.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox("errordata", "Error");
          }
        });
        int num = (int) frm.ShowDialog();
      }
    }

    private void lblYMax_PreviewMouseDown(object sender, EventArgs e)
    {
      using (InputDataFrm frm = new InputDataFrm())
      {
        frm.txtValue.KeyDown += (System.Windows.RoutedEventHandler) ((senders, es) =>
        {
          if (es.KeyCode != Keys.Return)
            return;
          try
          {
            if (frm.txtValue.Text.IndexOf('.') == 0)
              frm.txtValue.Text = "0" + frm.txtValue.Text;
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblYMax.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox(CommonFun.GetLanText(this.lanvalue, "errordata"), "Error");
          }
        });
        frm.btnOK.Click += (System.Windows.RoutedEventHandler) ((param0, param1) =>
        {
          try
          {
            if (frm.txtValue.Text.IndexOf('.') == 0)
              frm.txtValue.Text = "0" + frm.txtValue.Text;
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblYMax.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox("errordata", "Error");
          }
        });
        int num = (int) frm.ShowDialog();
      }
    }

    private void lblYMin_PreviewMouseDown(object sender, EventArgs e)
    {
      using (InputDataFrm frm = new InputDataFrm())
      {
        frm.txtValue.KeyDown += (System.Windows.RoutedEventHandler) ((senders, es) =>
        {
          if (es.KeyCode != Keys.Return)
            return;
          try
          {
            if (frm.txtValue.Text.IndexOf('.') == 0)
              frm.txtValue.Text = "0" + frm.txtValue.Text;
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblYMin.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox("errordata", "Error");
          }
        });
        frm.btnOK.Click += (System.Windows.RoutedEventHandler) ((param0, param1) =>
        {
          try
          {
            if (frm.txtValue.Text.IndexOf('.') == 0)
              frm.txtValue.Text = "0" + frm.txtValue.Text;
            Convert.ToDecimal(frm.txtValue.Text);
            this.lblYMin.Text = frm.txtValue.Text;
            frm.Close(); frm.Dispose();
          }
          catch
          {
            CommonFun.showbox("errordata", "Error");
          }
        });
        int num = (int) frm.ShowDialog();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager ComponentResourceManager = new ComponentResourceManager(typeof (SetXYFrm));
      this.btnClose = new Button();
      this.btnOK = new Button();
      this.lblXMin = new Label();
      this.lblXMax = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label4 = new Label();
      this.label8 = new Label();
      this.lblYMin = new Label();
      this.lblYMax = new Label();
      this.label11 = new Label();
      this.label12 = new Label();
      this.label13 = new Label();
      this.label14 = new Label();
      this.label17 = new Label();
      this.SuspendLayout();
      ComponentResourceManager.ApplyResources((object) this.btnClose, "btnClose");
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0, 0);
      this.btnClose.FlatAppearance.BorderSize = 0;
      this.btnClose.FlatAppearance.MouseDownBackColor = Color.Transparent;
      this.btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;
      this.btnClose.ForeColor = SystemColors.ControlDarkDark;
      this.btnClose.Image = (Image) Resources.close;
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = false;
      ComponentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.BackColor = Color.Transparent;
      this.btnOK.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0, 0);
      this.btnOK.FlatAppearance.BorderSize = 0;
      this.btnOK.FlatAppearance.MouseDownBackColor = Color.Transparent;
      this.btnOK.FlatAppearance.MouseOverBackColor = Color.Transparent;
      this.btnOK.ForeColor = SystemColors.ControlDarkDark;
      this.btnOK.Image = (Image) Resources.ok;
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = false;
      ComponentResourceManager.ApplyResources((object) this.lblXMin, "lblXMin");
      this.lblXMin.Name = "lblXMin";
      this.lblXMin.PreviewMouseDown += new EventHandler(this.lblXMin_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.lblXMax, "lblXMax");
      this.lblXMax.Name = "lblXMax";
      this.lblXMax.PreviewMouseDown += new EventHandler(this.lblXMax_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.BorderStyle = BorderStyle.Fixed3D;
      this.label5.Name = "label5";
      ComponentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.BorderStyle = BorderStyle.Fixed3D;
      this.label6.Name = "label6";
      ComponentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      this.label4.PreviewMouseDown += new EventHandler(this.lblXMin_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.Name = "label8";
      this.label8.PreviewMouseDown += new EventHandler(this.lblXMax_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.lblYMin, "lblYMin");
      this.lblYMin.Name = "lblYMin";
      this.lblYMin.PreviewMouseDown += new EventHandler(this.lblYMin_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.lblYMax, "lblYMax");
      this.lblYMax.Name = "lblYMax";
      this.lblYMax.PreviewMouseDown += new EventHandler(this.lblYMax_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.label11, "label11");
      this.label11.BorderStyle = BorderStyle.Fixed3D;
      this.label11.Name = "label11";
      ComponentResourceManager.ApplyResources((object) this.label12, "label12");
      this.label12.BorderStyle = BorderStyle.Fixed3D;
      this.label12.Name = "label12";
      ComponentResourceManager.ApplyResources((object) this.label13, "label13");
      this.label13.Name = "label13";
      this.label13.PreviewMouseDown += new EventHandler(this.lblYMin_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.label14, "label14");
      this.label14.Name = "label14";
      this.label14.PreviewMouseDown += new EventHandler(this.lblYMax_PreviewMouseDown);
      ComponentResourceManager.ApplyResources((object) this.label17, "label17");
      this.label17.BackColor = Color.Transparent;
      this.label17.ForeColor = SystemColors.Window;
      this.label17.Name = "label17";
      ComponentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Window;
      this.BackgroundImage = (Image) Resources.Item_select;
      this.Controls.Add((Control) this.label17);
      this.Controls.Add((Control) this.lblYMin);
      this.Controls.Add((Control) this.lblYMax);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.lblXMin);
      this.Controls.Add((Control) this.lblXMax);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SetXYFrm);
      this.ShowIcon = false;
      this.Load += new EventHandler(this.SetXYFrm_Load);
      this.ResumeLayout(false);
    }
  }
}*/
