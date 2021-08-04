using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Pen = System.Drawing.Pen;
using DashStyle = System.Drawing.Drawing2D.DashStyle;


namespace UVStudio
{
    /// <summary>
    /// Логика взаимодействия для QuantitationNewMethodNextCoefficien.xaml
    /// </summary>
    public partial class QuantitationNewMethodNextCoefficien : Window, IDisposable
    {
        private string absacc = CommonFun.GetAcc("absAccuracy");
        private string tacc = CommonFun.GetAcc("tAccuracy");
        private string conacc = CommonFun.GetAcc("ceAccuracy");
        public int dgvcnt = 0;
        private string lanvalue;
        private Decimal K0 = 0M;
        private Decimal K1 = 0M;
        private Decimal K2 = 0M;
        private Decimal K3 = 0M;
        private Decimal K10 = 0M;
        private Decimal K11 = 0M;
        private Decimal K12 = 0M;
        private Decimal K13 = 0M;
        private Decimal R = 0M;



        private void btnBack_PreviewMouseDown(object sender, EventArgs e) => this.Close();

        public QuaMethod QM { get; set; }

        public string[] wl_mass;
        public string step_interval;
        string count_measure;
        string optical_path;
        string curveEquation;
        string fittingMethod;
        string count_standard_sample;
        string measureunit;
        bool zero_intercept;
        string measure_method;
        string calibration_method;
        
        public void Dispose()
        {
            
        }


        public QuantitationNewMethodNextCoefficien(string[] wl_mass, string step_interval, string count_measure, string optical_path, string curveEquation, string fittingMethod, string count_standard_sample, string measureunit, bool zero_intercept, string measure_method, string calibration_method)
        {
            InitializeComponent();
            this.wl_mass = wl_mass;
            this.step_interval = step_interval;
            this.count_measure = count_measure;
            this.optical_path = optical_path;
            this.curveEquation = curveEquation;
            this.fittingMethod = fittingMethod;
            this.count_standard_sample = count_standard_sample;
            this.measureunit = measureunit;
            this.zero_intercept = zero_intercept;
            this.measure_method = measure_method;
            this.calibration_method = calibration_method;

            this.lblfcs.Font = new System.Drawing.Font("Segoe UI", 16F);

            /*QM.QPar = new QuaParmas();
            QM.QPar.MeasureMethodName = measure_method;
            QM.QPar.MeasureMethodName = measure_method;*/

            switch (fittingMethod) {
                case "Линейная":
                    if (curveEquation == "C=f(Abs)")
                        lblEquationCurve.Content = "C = K1*A + K0";
                    else
                        lblEquationCurve.Content = "A=K1*C+K0";
                    K0Grid.IsEnabled = true;
                    K1Grid.IsEnabled = true;
                    K2Grid.IsEnabled = false;
                    K3Grid.IsEnabled = false;
                        
                    break;
                case "Квадратичная":
                    if (curveEquation == "C=f(Abs)")
                        lblEquationCurve.Content = "C = K2*A^2 + K1*A + K0";
                    else
                        lblEquationCurve.Content = "A=K2*C^2+K1*C+K0";
                    K0Grid.IsEnabled = true;
                    K1Grid.IsEnabled = true;
                    K2Grid.IsEnabled = true;
                    K3Grid.IsEnabled = false;
                    break;
                case "Кубическая":
                    if (curveEquation == "C=f(Abs)")
                        lblEquationCurve.Content = "C = K3*A^3 + K2*A^2 + K1*A + K0";
                    else
                        lblEquationCurve.Content = "A=K3*C^3+K2*C^2+K1*C+K0";
                    K0Grid.IsEnabled = true;
                    K1Grid.IsEnabled = true;
                    K2Grid.IsEnabled = true;
                    K3Grid.IsEnabled = true;
                    break;
            }
        }

        private void Last_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            //QuantitationNewMethod quantationMethod = new QuantitationNewMethod(wl_mass, step_interval, count_measure, optical_path, curveEquation, fittingMethod, count_standard_sample, measureunit, zero_intercept, measure_method, calibration_method);
            //quantationMethod.ShowDialog();
            this.Close();
        }

        private void Last_PreviewMouseDown_1(object sender, RoutedEventArgs e)
        {

        }

        private void Blanking_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Measure_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void CloseSettings_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void Finish_PreviewMouseDown(object sender, RoutedEventArgs e)
        {

        }

        private void ConvertFcs()
        {
            if (fittingMethod == "Линейная")
            {
                if(K1 != 0M)
                {
                    K10 = Convert.ToDecimal((-this.K0 / this.K1).ToString("f4"));
                    Decimal num1 = 1M / this.K1;
                    this.K11 = num1 = Convert.ToDecimal(num1.ToString("f4"));
                   // Decimal num2 = num1;
                   // qm.K11 = num2;
                }
                else
                {
                    K10 = 0M;
                    K11 = 0M;
                }
            }
        }

        private void LblK0_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            InputDataFrm frm = new InputDataFrm();
            frm.ShowDialog();
            if (frm.wl != null)
            {
                Decimal num = Convert.ToDecimal(frm.wl, new CultureInfo("en-US"));
                this.lblK0.Content = frm.wl;

                if(curveEquation == "Abs=f(C)")
                {
                    K0 = num;
                    ConvertFcs();
                  //  this.QM.K0 = this.K0;
                }
                else
                {
                    K10 = num;
                 //   this.QM.K10 = this.K10;
                }
                this.DrawCurve();
            }
        }

        public void DrawCurve()
        {
            string str1;
            string format1;
            string format2;
            if (curveEquation == "Abs=f(C)")
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
            if (str1 == "ND")
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

            if (curveEquation == "Abs=f(C)")
            {
                //Label lblfcs = this.lblfcs;
                lblfcs.Text = "A = " + K1 + "*A + " + K0;
            }
            else
            {
                lblfcs.Text = "C = " + K11 + "*A + " + K10;

            }

            graphics.DrawLine(new Pen(Color.Black, 1f), num7, num9, num8, num9);
            graphics.DrawLine(new Pen(Color.Black, 1f), num7, num10, num8, num10);
            graphics.DrawLine(new Pen(Color.Black, 1f), num7, num9, num7, num10);
            graphics.DrawLine(new Pen(Color.Black, 1f), num8, num9, num8, num10);
            float num11;
            float num12;
            float num13;
            float num14;
            num11 = 0.0f;
            num12 = 100f;
            num13 = 0.0f;
            num14 = 20f;
            if (str1 == "ND")
            {
                for (int index = 0; index < 100; ++index)
                {
                    Sample sample1 = new Sample();
                    sample1.ND = new Decimal?(Convert.ToDecimal(index));
                    Sample sample2 = sample1;
                    Decimal k3 = K3;
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
                    Decimal k2 = K2;
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
                    Decimal k1 = K1;
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
                    Decimal k0 = K0;
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
                    Decimal k13 = K13;
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
                    Decimal k12 = K12;
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
                    Decimal k11 = K11;
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
                    Decimal k10 = K10;
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
                graphics.DrawString(measureunit, new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x5, y4));
                graphics.DrawString("Abs", new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
            }
            else
            {
                float x6 = num7 - graphics.MeasureString(measureunit, new Font("Segoe UI", (float)num1)).Width;
                graphics.DrawString(measureunit, new Font("Segoe UI", (float)num1), (Brush)new SolidBrush(Color.Black), new PointF(x6, y5));
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
                    double num4 = (double)num7 + (Convert.ToDouble((object)source[source.Count<Sample>() - 1].ND) - (double)num11) * num17;
                    double num5 = (double)num9 - (Convert.ToDouble((object)source[source.Count<Sample>() - 1].XGD) - (double)num13) * num18;
                    for (int index = source.Count<Sample>() - 1; index >= 0; --index)
                    {
                        double num6 = (double)num7 + (Convert.ToDouble((object)source[index].ND) - (double)num11) * num17;
                        double num15 = Convert.ToDouble((object)source[index].XGD) >= (double)num13 ? (Convert.ToDouble((object)source[index].XGD) <= (double)num14 ? (double)num9 - (Convert.ToDouble((object)source[index].XGD) - (double)num13) * num18 : (double)num10) : (double)num9;
                        graphics.DrawLine(new Pen(Color.Red, 1f), (float)num4, (float)num5, (float)num6, (float)num15);
                        num4 = num6;
                        num5 = num15;
                    }
                   
                }
                else
                {
                    double num4 = (double)num7 + (Convert.ToDouble((object)source[source.Count<Sample>() - 1].XGD) - (double)num11) * num17;
                    double num5 = (double)num9 - (Convert.ToDouble((object)source[source.Count<Sample>() - 1].ND) - (double)num13) * num18;
                    for (int index = source.Count<Sample>() - 2; index >= 0; --index)
                    {
                        double num6 = (double)num7 + (Convert.ToDouble((object)source[index].XGD) - (double)num11) * num17;
                        double num15 = Convert.ToDouble((object)source[index].ND) >= (double)num13 ? (Convert.ToDouble((object)source[index].ND) <= (double)num14 ? (double)num9 - (Convert.ToDouble((object)source[index].ND) - (double)num13) * num18 : (double)num10) : (double)num9;
                        graphics.DrawLine(new Pen(Color.Red, 1f), (float)num4, (float)num5, (float)num6, (float)num15);
                        num4 = num6;
                        num5 = num15;
                    }
                   
                }
            }

            this.picCurve.Image = (Image)bitmap;
        }

        private void LblK1_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            InputDataFrm frm = new InputDataFrm();
            frm.ShowDialog();
            if (frm.wl != null)
            {
                Decimal num = Convert.ToDecimal(frm.wl, new CultureInfo("en-US"));
                this.lblK1.Content = frm.wl;

                if (curveEquation == "Abs=f(C)")
                {
                    K1 = num;
                    ConvertFcs();
                    //  this.QM.K0 = this.K0;
                }
                else
                {
                    K11 = num;
                    //   this.QM.K10 = this.K10;
                }
                this.DrawCurve();
            }
        }

        private void LblK2_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            InputDataFrm frm = new InputDataFrm();
            frm.ShowDialog();
            if (frm.wl != null)
            {
                Decimal num = Convert.ToDecimal(frm.wl, new CultureInfo("en-US"));
                this.lblK2.Content = frm.wl;

                if (curveEquation == "Abs=f(C)")
                {
                    K2 = num;
                    ConvertFcs();
                    //  this.QM.K0 = this.K0;
                }
                else
                {
                    K12 = num;
                    //   this.QM.K10 = this.K10;
                }
                this.DrawCurve();
            }
        }

        private void LblK3_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            InputDataFrm frm = new InputDataFrm();
            frm.ShowDialog();
            if (frm.wl != null)
            {
                Decimal num = Convert.ToDecimal(frm.wl, new CultureInfo("en-US"));
                this.lblK3.Content = frm.wl;

                if (curveEquation == "Abs=f(C)")
                {
                    K3 = num;
                    ConvertFcs();
                    //  this.QM.K0 = this.K0;
                }
                else
                {
                    K13 = num;
                    //   this.QM.K10 = this.K10;
                }
                this.DrawCurve();
            }
        }

        private void CaculateK()
        {
            if(this.fittingMethod == "Линейная")
            {

            }

        }
    }
}
