using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using UVStudio.Properties;

namespace UVStudio
{
    public partial class DNANewMethodFrm : Form
    {
        public DNANewMethodFrm()
        {
            InitializeComponent();
        }
        private void DNANewMethodFrm_Load(object sender, EventArgs e)
        {
            // this.panelleft.Width = this.Width / 2 - 13;
            // this.panelright.Width = this.panelleft.Width;
            //this.panelright.Location = new Point(this.Width / 2 + 6 + 1, this.panelright.Location.Y);
            panelright.Visible = false;
            /*this.pibEConvert.Tag = (object)"off";
            this.pibZeroB.Tag = (object)"off";
            this.pibBackWL.Tag = (object)"off";*/
        }
        private void lblMeaMeth_Click(object sender, EventArgs e)
        {
            DNAMode dnaMode = new DNAMode();
            dnaMode.ShowDialog();
            if (dnaMode.dnaMode != null && dnaMode.dnaMode != "")
            {
                lblMMV.Tag = CommonFun.GetLanText(dnaMode.dnaMode);
                lblMMV.Text = CommonFun.GetLanText(dnaMode.dnaMode);
                this.SetDefault(lblMMV.Text);
            }
        }

        private void SetDefault(string method)
        {
            if (method == CommonFun.GetLanText("lowery"))
            {
                this.panelright.Visible = true;
                this.btnFinish.Visible = false;
                this.btnOK.Visible = true;
                this.pibBackWL.Enabled = false;
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibBackWL.Tag = (object)"off";
                this.lblwl2.Text = "";
                this.lblwl2.Enabled = false;
                this.label5.Enabled = false;
                this.lblwl3.Text = "";
                this.lblwl3.Enabled = false;
                this.label6.Enabled = false;
                this.lblBackWL.Enabled = false;
                this.lblwl1.Text = "500";
                this.lblRV.Text = "-------";
            }
            else if (method == CommonFun.GetLanText("uv"))
            {
                this.panelright.Visible = true;
                this.btnOK.Visible = true;
                this.btnFinish.Visible = false;
                this.pibBackWL.Enabled = false;
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibBackWL.Tag = (object)"off";
                this.lblwl2.Text = "";
                this.lblwl2.Enabled = false;
                this.label5.Enabled = false;
                this.lblwl3.Text = "";
                this.lblwl3.Enabled = false;
                this.label6.Enabled = false;
                this.lblBackWL.Enabled = false;
                this.lblwl1.Text = "280";
                this.lblRV.Text = "-------";
            }
            else if (method == CommonFun.GetLanText("bca"))
            {
                this.panelright.Visible = true;
                this.btnOK.Visible = true;
                this.btnFinish.Visible = false;
                this.pibBackWL.Enabled = false;
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibBackWL.Tag = (object)"off";
                this.lblwl2.Text = "";
                this.lblwl2.Enabled = false;
                this.label5.Enabled = false;
                this.lblwl3.Text = "";
                this.lblwl3.Enabled = false;
                this.label6.Enabled = false;
                this.lblBackWL.Enabled = false;
                this.lblwl1.Text = "562";
                this.lblRV.Text = "-------";
            }
            else if (method == CommonFun.GetLanText("cbb"))
            {
                this.panelright.Visible = true;
                this.btnOK.Visible = true;
                this.btnFinish.Visible = false;
                this.pibBackWL.Enabled = false;
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibBackWL.Tag = (object)"off";
                this.lblwl2.Text = "";
                this.lblwl2.Enabled = false;
                this.label5.Enabled = false;
                this.lblwl3.Text = "";
                this.lblwl3.Enabled = false;
                this.label6.Enabled = false;
                this.lblBackWL.Enabled = false;
                this.lblwl1.Text = "595";
                this.lblRV.Text = "-------";
            }
            else if (method == CommonFun.GetLanText("biuret"))
            {
                this.panelright.Visible = true;
                this.btnOK.Visible = true;
                this.btnFinish.Visible = false;
                this.pibBackWL.Enabled = false;
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibBackWL.Tag = (object)"off";
                this.lblwl2.Text = "";
                this.lblwl2.Enabled = false;
                this.label5.Enabled = false;
                this.lblwl3.Text = "";
                this.lblwl3.Enabled = false;
                this.label6.Enabled = false;
                this.lblBackWL.Enabled = false;
                this.lblwl1.Text = "540";
                this.lblRV.Text = "-------";
            }
            else if (method == CommonFun.GetLanText("dna1"))
            {
                this.lblwl1.Text = "260.0";
                this.lblwl2.Text = "280.0";
                this.lblRV.Text = "62.9,36.0,1552.0,757.3";
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                this.pibBackWL.Tag = (object)"on";
                this.lblwl3.Text = "320.0";
                this.lblwl2.Enabled = true;
                this.label5.Enabled = true;
                this.pibBackWL.Enabled = true;
                this.lblwl3.Enabled = true;
                this.label6.Enabled = true;
                this.lblBackWL.Enabled = true;
                this.panelright.Visible = false;
                this.btnFinish.Visible = true;
                this.btnOK.Visible = false;
            }
            else
            {
                if (!(method == CommonFun.GetLanText("dna2")))
                    return;
                this.lblwl1.Text = "260.0";
                this.lblwl2.Text = "230.0";
                this.lblRV.Text = "49.1,3.48,183.0,75.8";
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                this.pibBackWL.Tag = (object)"on";
                this.lblwl3.Text = "320.0";
                this.lblwl2.Enabled = true;
                this.label5.Enabled = true;
                this.pibBackWL.Enabled = true;
                this.lblwl3.Enabled = true;
                this.label6.Enabled = true;
                this.lblBackWL.Enabled = true;
                this.panelright.Visible = false;
                this.btnOK.Visible = false;
                this.btnFinish.Visible = true;
            }
        }

        private void lblwl1_Click(object sender, EventArgs e)
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
                        if (Convert.ToDecimal(frm1.txtValue.Text) < 190M || Convert.ToDecimal(frm1.txtValue.Text) > 1100M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            this.lblwl1.Text = Convert.ToDecimal(frm1.txtValue.Text).ToString("f1");
                            frm1.Close();
                        }
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
                        if (Convert.ToDecimal(frm1.txtValue.Text) < 190M || Convert.ToDecimal(frm1.txtValue.Text) > 1100M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            this.lblwl1.Text = Convert.ToDecimal(frm1.txtValue.Text).ToString("f1");
                            frm1.Close();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm1.ShowDialog();
            }
        }

        private void lblwl2_Click(object sender, EventArgs e)
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
                        if (Convert.ToDecimal(frm1.txtValue.Text) < 190M || Convert.ToDecimal(frm1.txtValue.Text) > 1100M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            this.lblwl2.Text = Convert.ToDecimal(frm1.txtValue.Text).ToString("f1");
                            frm1.Close();
                        }
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
                        if (Convert.ToDecimal(frm1.txtValue.Text) < 190M || Convert.ToDecimal(frm1.txtValue.Text) > 1100M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            this.lblwl2.Text = Convert.ToDecimal(frm1.txtValue.Text).ToString("f1");
                            frm1.Close();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
               frm1.ShowDialog();
            }
        }

        private void lblwl3_Click(object sender, EventArgs e)
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
                        if (Convert.ToDecimal(frm1.txtValue.Text) < 190M || Convert.ToDecimal(frm1.txtValue.Text) > 1100M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            this.lblwl3.Text = Convert.ToDecimal(frm1.txtValue.Text).ToString("f1");
                            frm1.Close();
                        }
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
                        if (Convert.ToDecimal(frm1.txtValue.Text) < 190M || Convert.ToDecimal(frm1.txtValue.Text) > 1100M)
                        {
                            CommonFun.showbox(CommonFun.GetLanText("wlrangeout"), "Error");
                        }
                        else
                        {
                            this.lblwl3.Text = Convert.ToDecimal(frm1.txtValue.Text).ToString("f1");
                            frm1.Close();
                        }
                    }
                    catch
                    {
                        CommonFun.showbox(CommonFun.GetLanText("errordata"), "Error");
                    }
                });
                frm1.ShowDialog();
            }
        }

        private void pibBackWL_Click(object sender, EventArgs e)
        {
            if (this.pibBackWL.Tag.ToString() == "on")
            {
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibBackWL.Tag = (object)"off";
                this.lblwl3.Enabled = false;
                this.label6.Enabled = false;
                this.lblBackWL.Enabled = false;
            }
            else
            {
                this.pibBackWL.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                this.pibBackWL.Tag = (object)"on";
                this.lblBackWL.Enabled = true;
                this.label6.Enabled = true;
                this.lblwl3.Enabled = true;
            }
        }

        private void lblR_Click(object sender, EventArgs e)
        {
            if ((!(this.lblMMV.Text == CommonFun.GetLanText("dna1")) && !(this.lblMMV.Text == CommonFun.GetLanText("dna2")) ? 0 : 4) > 0)
            {
                using (QuaInputFrm frm = new QuaInputFrm())
                {
                    frm.lblSecond.Text = CommonFun.GetLanText("r");
                    frm.label1.Content = "K0";
                    frm.label2.Content = "K1";
                    frm.lblv2.Visibility = Visibility.Visible;
                    frm.lblv3.Visibility = Visibility.Visible;
                    frm.lblK2.Visibility = Visibility.Visible;
                    frm.lblK3.Visibility = Visibility.Visible;
                   // frm.lblk22.Visibility = Visibility.Visible;
                   // frm.lblk33.Visibility = Visibility.Visible;
                    if (this.lblRV.Text.Length > 4)
                    {
                        try
                        {
                            string[] strArray = this.lblRV.Text.Split(',');
                            frm.lblMax.Content = strArray[0];
                            frm.lblMin.Content = strArray[1];
                            frm.lblv2.Content = strArray[2];
                            frm.lblv3.Content = strArray[3];
                        }
                        catch
                        {
                        }
                    }
                    frm.btnOK.Click += ((param0, param1) =>
                    {
                        if (!(frm.lblMax.Content.ToString() != "") || !(frm.lblMin.Content.ToString() != "") || !(frm.lblv2.Content.ToString() != "") || !(frm.lblv3.Content.ToString() != ""))
                            return;
                        string str;
                        try
                        {
                            Convert.ToDecimal(frm.lblMax.Content.ToString());
                            Convert.ToDecimal(frm.lblMin.Content.ToString());
                            Convert.ToDecimal(frm.lblv2.Content.ToString());
                            Convert.ToDecimal(frm.lblv3.Content.ToString());
                            str = frm.lblMax.Content.ToString()+ "," + frm.lblMin.Content.ToString() + "," + frm.lblv2.Content.ToString() + "," + frm.lblv3.Content.ToString();
                        }
                        catch
                        {
                            CommonFun.showbox(CommonFun.GetLanText("inputerror"), "Error");
                            return;
                        }
                        frm.Close();
                        this.lblRV.Text = str;
                    });
                    frm.btnCancel.Click += ((param0, param1) =>
                    {
                        frm.Close();
                    });
                    frm.ShowDialog();
                }
            }
            else
                this.lblRV.Text = "----";
        }

        private void lblCnt_Click(object sender, EventArgs e)
        {
            CountMeasure wLCountMeasure = new CountMeasure(lblCntV.Text);
            wLCountMeasure.ShowDialog();
            if (lblCntV.Text != wLCountMeasure.count_measure)
            {
                lblCntV.Text = wLCountMeasure.count_measure.ToString();
            }
        }
        private void lblLength_Click(object sender, EventArgs e)
        {
            OpticalPath wLOpticalPath = new OpticalPath(lblLengthV.Text);
            wLOpticalPath.ShowDialog();
            if (lblLengthV.Text != wLOpticalPath.optical_path)
            {
                lblLengthV.Text = wLOpticalPath.optical_path.ToString();
            }
        }

        private void lblThreshold_Click(object sender, EventArgs e)
        {
            using (QuaInputFrm frm = new QuaInputFrm())
            {
                frm.Loaded += ((param0, param1) =>
                {
                    frm.label1.Content = "Минимум";
                    frm.label2.Content = "Максимум";
                    frm.k2_.Visibility = Visibility.Hidden;
                    frm.k3_.Visibility = Visibility.Hidden;
                });

                frm.btnOK.Click += ((param0, param1) =>
                {
                    if (frm.lblMax.Content.ToString().Replace(" >", "") == "" || frm.lblMin.Content.ToString().Replace(" >", "") == "")
                        return;
                    if (Convert.ToDecimal(frm.lblMax.Content.ToString().Replace(" >", "")) >= Convert.ToDecimal(frm.lblMin.Content.ToString().Replace(" >", "")))
                    {
                        this.lblThresholdV.Text = CommonFun.GetLanText("maxv") + ":" + frm.lblMax.Content.ToString().Replace(" >", "") + "," + CommonFun.GetLanText("minv") + ":" + frm.lblMin.Content.ToString().Replace(" >", "");
                      //  this.lblLimitsV.Text = frm.lblMax.Content.ToString() + "," + frm.lblMin.Content.ToString();
                    }
                    else
                    {
                        this.lblThresholdV.Text = CommonFun.GetLanText("maxv") + ":" + frm.lblMin.Content.ToString().Replace(" >", "") + "," + CommonFun.GetLanText("minv") + ":" + frm.lblMax.Content.ToString().Replace(" >", "");
                     //   this.lblLimitsV.Text = frm.lblMin.Content.ToString() + "," + frm.lblMax.Content.ToString();
                    }
                    frm.Close();
                });
                frm.btnCancel.Click += ((param0, param1) =>
                {
                    frm.Close();
                });
                frm.ShowDialog();
            }
        }
        private void pibEConvert_Click(object sender, EventArgs e)
        {
            if (this.pibEConvert.Tag.ToString() == "on")
            {
                this.pibEConvert.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibEConvert.Tag = (object)"off";
            }
            else
            {
                this.pibEConvert.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                this.pibEConvert.Tag = (object)"on";
            }
        }
        private void lblequation_Click(object sender, EventArgs e)
        {
            StandartCurveEquation standartCurveEquation = new StandartCurveEquation(lblequationV.Text);
            standartCurveEquation.ShowDialog();
            if (lblequationV.Text != standartCurveEquation.curveEquation)
            {
                lblequationV.Text = standartCurveEquation.curveEquation.ToString();
            }
        }

        private void lblfitting_Click(object sender, EventArgs e)
        {
            FittingMethod fittingMethod_ = new FittingMethod(lblfittingV.Text);
            fittingMethod_.ShowDialog();
            if (lblfittingV.Text != fittingMethod_.fittingMethod)
            {
                lblfittingV.Text = fittingMethod_.fittingMethod.ToString();
                lblfittingV.Tag = fittingMethod_.fittingMethod_tag.ToString();
            }
        }

        private void pibZeroB_Click(object sender, EventArgs e)
        {
            if (this.pibZeroB.Tag.ToString() == "on")
            {
                this.pibZeroB.BackgroundImage = (Image)Resources.UI_DB_Switcher_Off;
                this.pibZeroB.Tag = (object)"off";
            }
            else
            {
                this.pibZeroB.BackgroundImage = (Image)Resources.UI_DB_Switcher_On;
                this.pibZeroB.Tag = (object)"on";
            }
        }
        private void lblCabMethod_Click(object sender, EventArgs e)
        {
            CalibrationMethod calibration_method_ = new CalibrationMethod(this.lblCabMethodV.Text.ToString());
            calibration_method_.ShowDialog();
            if (this.lblCabMethodV.Text.ToString() != calibration_method_.calibration_method)
            {
                this.lblCabMethodV.Text = calibration_method_.calibration_method.ToString();

                if (this.lblCabMethodV.Text == CommonFun.GetLanText("inputr"))
                {
                    this.lblequationV.Text = "C=f(Abs)";
                    this.lblequation.Enabled = false;
                    this.lblequationV.Enabled = false;
                    this.label16.Enabled = false;
                }
                else
                {
                    this.lblequation.Enabled = true;
                    this.lblequationV.Enabled = true;
                    this.label16.Enabled = true;
                }
                this.lblCabMethodV.Text = calibration_method_.calibration_method.ToString();
                this.lblCabMethodV.Tag = calibration_method_.calibration_method_tag.ToString();
                // OpenWl();
            }
        }
        private void lblSamCnt_Click(object sender, EventArgs e)
        {
            if (this.lblCabMethodV.Text == CommonFun.GetLanText("inputr"))
            {
                CommonFun.showbox(CommonFun.GetLanText("noneedsample"), "Error");
            }
            else
            {
                StandardSampleCount count_standard_sample_ = new StandardSampleCount(lblSamCntV.Text);
                count_standard_sample_.ShowDialog();
                if (lblSamCntV.Text != count_standard_sample_.count_standard_sample.ToString())
                {
                    lblSamCntV.Text = count_standard_sample_.count_standard_sample.ToString();
                }
            }
        }
        private void lblUnit_Click(object sender, EventArgs e)
        {
            MeasureUnit measureunit_ = new MeasureUnit(lblUnitV.Text);
            measureunit_.ShowDialog();
            if (lblUnitV.Text != measureunit_.measureunit)
            {
                lblUnitV.Text = measureunit_.measureunit.ToString();
            }
        }
        private void btnBack_Click(object sender, EventArgs e) => this.Close();

        private void btnNew_Click(object sender, EventArgs e)
        {
            panelleft.Enabled = true;
            panelright.Enabled = true;
        }
    }
}
