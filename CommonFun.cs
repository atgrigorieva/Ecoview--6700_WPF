using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using System.Xml.Linq;

namespace UVStudio
{
    public class CommonFun
    {
        public const int BoundRate = 9600;
        public const int subcnt = 9;
        public const string bzbc0 = "241.1,278.2,287.1,333.5,361.3,416.2,451.9,485.3,536.5,640.5";
        public const string bzbc1 = "241.2,278.2,287.3,333.6,361.3,416.3,451.4,485.3,536.6,640.5";
        public const string bzbc2 = "241.2,278.1,287.7,333.6,361.1,416.7,451.3,485.4,536.9,640.8";
        public const string bzbc4 = "241.1,278.0,287.7,333.6,361.1,417.1,451.3,485.3,537.7,641.5";
        public const string bzbc5 = "240.9,277.6,287.7,333.4,361.0,417.3,451.3,485.4,538.0,641.7";
        public const string RaceDongleID = "5131AFFD";
        public const string RaceDonglePWD = "DEA172BD99A88EDB";
        public const string GLPDongleID = "73F376F6";
        public const string GLPDonglePWD = "1D18D2074B2F1020";
        public const bool DongleGLP = true;
        public const string ParamsSetName = "ParmasSet.xml";

        public static void Startkeyboard()
        {
            string str1 = "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe";
            if (File.Exists(str1))
            {
                Process.Start(str1);
            }
            else
            {
                string str2 = "C:\\Windows\\System32\\osk.exe";
                if (File.Exists(str2))
                {
                    Process.Start(str2);
                }
                else
                {
                    switch (Environment.OSVersion.Version.Major.ToString() + "." + (object)Environment.OSVersion.Version.Minor)
                    {
                        case "6.2":
                            if (Environment.Is64BitOperatingSystem)
                            {
                                Process.Start(Environment.CurrentDirectory + "\\win832\\TabTip.exe");
                                break;
                            }
                            Process.Start(Environment.CurrentDirectory + "\\win864\\TabTip.exe");
                            break;
                        case "10.0":
                            if (Environment.Is64BitOperatingSystem)
                            {
                                Process.Start(Environment.CurrentDirectory + "\\win1032\\TabTip.exe");
                                break;
                            }
                            Process.Start(Environment.CurrentDirectory + "\\win1064\\TabTip.exe");
                            break;
                    }
                }
            }
        }


        public static string getName(OperateType? type, string SourceName)
        {
            string path = Environment.CurrentDirectory + "\\TestFile\\SpectrumScan";
            int num = 1;
            string str1;
            if (!type.HasValue)
            {
                str1 = "SpectrumScan";
            }
            else
            {
                ref OperateType? local = ref type;
                OperateType valueOrDefault = local.GetValueOrDefault();
                if (local.HasValue)
                {
                    switch (valueOrDefault)
                    {
                        case OperateType.Smooth:
                            str1 = SourceName + "-S";
                            goto label_22;
                        case OperateType.AVG:
                            str1 = SourceName + "-AVG";
                            goto label_22;
                        case OperateType.FirstDeri:
                            str1 = SourceName + "-1d";
                            goto label_22;
                        case OperateType.SecDeri:
                            str1 = SourceName + "-2d";
                            goto label_22;
                        case OperateType.ThirdDeri:
                            str1 = SourceName + "-3d";
                            goto label_22;
                        case OperateType.FourthDeri:
                            str1 = SourceName + "-4d";
                            goto label_22;
                        case OperateType.SAddR:
                            str1 = SourceName + "-AddR";
                            goto label_22;
                        case OperateType.SAddS:
                            str1 = SourceName + "-AddS";
                            goto label_22;
                        case OperateType.SSubR:
                            str1 = SourceName + "-SubR";
                            goto label_22;
                        case OperateType.SSubS:
                            str1 = SourceName + "-SubS";
                            goto label_22;
                        case OperateType.SMulR:
                            str1 = SourceName + "-MulR";
                            goto label_22;
                        case OperateType.SMulS:
                            str1 = SourceName + "-MulS";
                            goto label_22;
                        case OperateType.SDivR:
                            str1 = SourceName + "-DivR";
                            goto label_22;
                        case OperateType.SDivS:
                            str1 = SourceName + "-DivS";
                            goto label_22;
                        case OperateType.NC:
                            str1 = SourceName + "-NC";
                            goto label_22;
                        case OperateType.ThreeD:
                            str1 = SourceName + "-3D";
                            goto label_22;
                        case OperateType.Square:
                            str1 = SourceName + "-Squa";
                            goto label_22;
                    }
                }
                str1 = SourceName;
            }
        label_22:
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (FileSystemInfo file in new DirectoryInfo(path).GetFiles())
            {
                string name = file.Name;
                if (name.Contains(str1))
                {
                    string str2 = name.Substring(0, name.LastIndexOf('.'));
                    try
                    {
                        int int16 = (int)Convert.ToInt16(((IEnumerable<string>)str2.Split('-')).Last<string>());
                        if (int16 >= num)
                            num = int16 + 1;
                    }
                    catch
                    {
                    }
                }
            }
            return str1 + "-" + num.ToString();
        }
        public static void GenerateFCS(QuaMethod qm)
        {
           // string appConfig = CommonFun.GetAppConfig("Language");
            string str1 = "";
            Decimal num;
            if (qm.QPar.Fitting == "Линейная")
            {
                if (Convert.ToDecimal(qm.K1) != 0M)
                {
                    num = qm.K1;
                    str1 = "Abs=" + num.ToString("f4") + "*C";
                    if (Convert.ToDecimal(qm.K0) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K0) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K0) != 0M)
                    str1 = "Abs=" + qm.K0.ToString("f4");
            }
            else if (qm.QPar.Fitting == "Квадратичная")
            {
                if (Convert.ToDecimal(qm.K2) != 0M)
                {
                    num = qm.K2;
                    str1 = "Abs=" + num.ToString("f4") + "*C\x00B2";
                    if (Convert.ToDecimal(qm.K1) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K1;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3 + "*C";
                        if (Convert.ToDecimal(qm.K0) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K0) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K1) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K1;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3 + "*C";
                        if (Convert.ToDecimal(qm.K0) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K0) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K0) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K0) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K1) != 0M)
                {
                    str1 = "Abs=" + (object)qm.K1 + "*C";
                    if (Convert.ToDecimal(qm.K0) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K0) < 0M)
                        str1 += qm.K0.ToString("f4");
                }
                else if (Convert.ToDecimal(qm.K0) != 0M)
                    str1 = "Abs=" + qm.K0.ToString("f4");
            }
            else if (qm.QPar.Fitting == "Кубическая")
            {
                if (Convert.ToDecimal(qm.K3) != 0M)
                {
                    num = qm.K3;
                    str1 = "Abs=" + num.ToString("f4") + "*C\x00B3";
                    if (Convert.ToDecimal(qm.K2) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K2;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3 + "*C\x00B2";
                        if (Convert.ToDecimal(qm.K1) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K1;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5 + "*C";
                            if (Convert.ToDecimal(qm.K0) > 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K1) < 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K1) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K1;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5 + "*C";
                            if (Convert.ToDecimal(qm.K0) > 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K0) < 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K0) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K0) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K2) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K2;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3 + "*C\x00B2";
                        if (Convert.ToDecimal(qm.K1) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K1;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5 + "*C";
                            if (Convert.ToDecimal(qm.K0) > 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K0) < 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K1) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K1;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5 + "*C";
                            if (Convert.ToDecimal(qm.K0) > 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K0) < 0M)
                            {
                                string str6 = str1;
                                num = qm.K0;
                                string str7 = num.ToString("f4");
                                str1 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K0) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K0) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K1) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K1;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3 + "*C";
                        if (Convert.ToDecimal(qm.K0) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K0) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K1) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K1;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3 + "*C";
                        if (Convert.ToDecimal(qm.K0) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K0) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K0) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K0) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K2) != 0M)
                {
                    num = qm.K2;
                    str1 = "Abs=" + num.ToString("f4") + "*C\x00B2";
                    if (Convert.ToDecimal(qm.K1) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K1;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3 + "*C";
                        if (Convert.ToDecimal(qm.K1) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K1) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K1) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K1;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3 + "*C";
                        if (Convert.ToDecimal(qm.K0) > 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K0) < 0M)
                        {
                            string str4 = str1;
                            num = qm.K0;
                            string str5 = num.ToString("f4");
                            str1 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K0) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K0) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K1) != 0M)
                {
                    num = qm.K1;
                    str1 = "Abs=" + num.ToString("f4") + "*C";
                    if (Convert.ToDecimal(qm.K0) > 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K0) < 0M)
                    {
                        string str2 = str1;
                        num = qm.K0;
                        string str3 = num.ToString("f4");
                        str1 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K0) != 0M)
                    str1 = "Abs=" + qm.K0.ToString("f4");
            }
            qm.AFCS = str1;
            string str8 = "";
            if (qm.QPar.Fitting == "Линейная")
            {
                if (Convert.ToDecimal(qm.K11) != 0M)
                {
                    num = qm.K11;
                    str8 = "C=" + num.ToString("f4") + "*A";
                    if (Convert.ToDecimal(qm.K10) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K10) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K10) != 0M)
                {
                    num = qm.K10;
                    str8 = "C=" + num.ToString("f4");
                }
            }
            else if (qm.QPar.Fitting == "Квадратичная")
            {
                if (Convert.ToDecimal(qm.K12) != 0M)
                {
                    num = qm.K12;
                    str8 = "C=" + num.ToString("f4") + "*A\x00B2";
                    if (Convert.ToDecimal(qm.K11) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K11;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3 + "*A";
                        if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K11) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K11;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3 + "*A";
                        if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K10) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K10) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K11) != 0M)
                {
                    str8 = "C=" + (object)qm.K11 + "*A";
                    if (Convert.ToDecimal(qm.K10) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K10) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K10) != 0M)
                {
                    num = qm.K10;
                    str8 = "C=" + num.ToString("f4");
                }
            }
            else if (qm.QPar.Fitting == "Кубическая")
            {
                if (Convert.ToDecimal(qm.K13) != 0M)
                {
                    num = qm.K13;
                    str8 = "C=" + num.ToString("f4") + "*A\x00B3";
                    if (Convert.ToDecimal(qm.K12) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K12;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3 + "*A\x00B2";
                        if (Convert.ToDecimal(qm.K11) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K11;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5 + "*A";
                            if (Convert.ToDecimal(qm.K10) > 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K10) < 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K11) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K11;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5 + "*A";
                            if (Convert.ToDecimal(qm.K10) > 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K10) < 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K12) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K12;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3 + "*A\x00B2";
                        if (Convert.ToDecimal(qm.K11) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K11;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5 + "*A";
                            if (Convert.ToDecimal(qm.K10) > 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K10) < 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K11) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K11;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5 + "*A";
                            if (Convert.ToDecimal(qm.K10) > 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + "+" + str7;
                            }
                            else if (Convert.ToDecimal(qm.K10) < 0M)
                            {
                                string str6 = str8;
                                num = qm.K10;
                                string str7 = num.ToString("f4");
                                str8 = str6 + str7;
                            }
                        }
                        else if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K11) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K11;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3 + "*A";
                        if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K11) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K11;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3 + "*A";
                        if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K10) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K10) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K12) != 0M)
                {
                    num = qm.K12;
                    str8 = "C=" + num.ToString("f4") + "*A\x00B2";
                    if (Convert.ToDecimal(qm.K11) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K11;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3 + "*A";
                        if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K11) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K11;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3 + "*A";
                        if (Convert.ToDecimal(qm.K10) > 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + "+" + str5;
                        }
                        else if (Convert.ToDecimal(qm.K10) < 0M)
                        {
                            string str4 = str8;
                            num = qm.K10;
                            string str5 = num.ToString("f4");
                            str8 = str4 + str5;
                        }
                    }
                    else if (Convert.ToDecimal(qm.K10) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K10) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K11) != 0M)
                {
                    num = qm.K11;
                    str8 = "C=" + num.ToString("f4") + "*A";
                    if (Convert.ToDecimal(qm.K10) > 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + "+" + str3;
                    }
                    else if (Convert.ToDecimal(qm.K10) < 0M)
                    {
                        string str2 = str8;
                        num = qm.K10;
                        string str3 = num.ToString("f4");
                        str8 = str2 + str3;
                    }
                }
                else if (Convert.ToDecimal(qm.K10) != 0M)
                {
                    num = qm.K10;
                    str8 = "C=" + num.ToString("f4");
                }
            }
            qm.CFCS = str8;
        }
        public static int getNear(float[] array, float target)
        {
            if (array.Length == 0)
                return -1;
            if (array.Length == 1)
                return 0;
            int index1 = 0;
            int index2 = array.Length - 1;
            int index3 = (index1 + index2) / 2;
            while (index2 - index1 > 1)
            {
                if ((double)target == (double)array[index3])
                    return index3;
                if ((double)target < (double)array[index3])
                    index1 = index3;
                if ((double)target > (double)array[index3])
                    index2 = index3;
                index3 = (index1 + index2) / 2;
            }
            return (double)target - (double)array[index2] < (double)array[index1] - (double)target ? index2 : index1;
        }
        public static List<System.Drawing.Color> Colorlist() => new List<System.Drawing.Color>()
        {
          System.Drawing.Color.Red,
          System.Drawing.Color.Orange,
          System.Drawing.Color.Yellow,
          System.Drawing.Color.Green,
          System.Drawing.Color.Blue,
          System.Drawing.Color.Purple,
          System.Drawing.Color.Gray,
          System.Drawing.Color.Pink,
          System.Drawing.Color.Brown,
          System.Drawing.Color.Coral,
          System.Drawing.Color.Cyan,
          System.Drawing.Color.Fuchsia,
          System.Drawing.Color.GreenYellow,
          System.Drawing.Color.Olive,
          System.Drawing.Color.DarkGoldenrod,
          System.Drawing.Color.Khaki,
          System.Drawing.Color.Lime,
          System.Drawing.Color.Linen,
          System.Drawing.Color.Maroon,
          System.Drawing.Color.MediumAquamarine,
          System.Drawing.Color.MediumOrchid,
          System.Drawing.Color.MediumPurple,
          System.Drawing.Color.MediumSeaGreen,
          System.Drawing.Color.Moccasin,
          System.Drawing.Color.Silver,
          System.Drawing.Color.SkyBlue,
          System.Drawing.Color.SlateGray,
          System.Drawing.Color.Tan,
          System.Drawing.Color.Teal,
          System.Drawing.Color.Thistle,
          System.Drawing.Color.DarkBlue
        };

        public static string GetAppConfig(string strKey) => ConfigurationManager.AppSettings[strKey];

        public static void Set(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (configuration.AppSettings.Settings[key] != null)
                configuration.AppSettings.Settings[key].Value = value;
            else
                configuration.AppSettings.Settings.Add(key, value);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void WriteLine(string text)
        {
            string path1 = Environment.CurrentDirectory + "\\Log";
            string path2 = path1 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "error.log";
            if (!Directory.Exists(path1))
                Directory.CreateDirectory(path1);
            if (!File.Exists(path2))
                File.Create(path2).Close();
            text += "\r\n";
            using (StreamWriter streamWriter = new StreamWriter(path2, true, Encoding.UTF8))
                streamWriter.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }

        public static void WriteSendLine(string text)
        {
            try
            {
                string path1 = Environment.CurrentDirectory + "/Log";
                string path2 = path1 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "S_ecoview.log";
                if (!Directory.Exists(path1))
                    Directory.CreateDirectory(path1);
                if (!File.Exists(path2))
                    File.Create(path2).Close();
                text += "\r\n";
                using (StreamWriter streamWriter = new StreamWriter(path2, true, Encoding.UTF8))
                    streamWriter.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
            catch
            {
                return;
            }
        }

        public static string GetLanText(string value)
        {
            string str = "";
            switch (value)
            {
                case "linear":
                    str = "Линейная";
                    break;
                case "squar":
                    str = "Квадратичная";
                    break;
                case "qube":
                    str = "Кубическая";
                    break;
                case "opencom":
                    str = "Пожалуйста, откройте последовательный порт!";
                    break;
                case "offlineversion":
                    str = "Оффлайнверсия";
                    break;
                case "noresponse":
                    str = "Нет ответа с сообщением, выйдите и повторите попытку!";
                    break;
                case "com":
                    str = "com";
                    break;
                case "pass":
                    str = "ОК";
                    break;
                case "nonauthorize":
                    str = "Ошибка подключения, неавторизованное программное обеспечение";
                    break;
                case "fail":
                    str = "Ошибка репликации!";
                    break;
                case "notconfig":
                    str = "Не настроено";
                    break;
                case "checking":
                    str = "Проверка";
                    break;
                case "warmup":
                    str = "Осталось времени";
                    break;
                case "second":
                    str = "секунда";
                    break;
                case "minute":
                    str = "минута";
                    break;
                case "login":
                    str = "логин";
                    break;
                case "system":
                    str = "Система";
                    break;
                case "errorretry":
                    str = "Ошибка! Повторите!";
                    break;
                case "stopmeasure":
                    str = "Остановить";
                    break;
                case "stopblanking":
                    str = "Остановить";
                    break;
                case "waitforcmd":
                    str = "Последняя команда не завершена!";
                    break;
                case "logresetdd":
                    str = "Сброс времени работы дейтериевой лампы";
                    break;
                case "logresetwd":
                    str = "Сбросить время вольфрамовой лампы";
                    break;
                case "blanking":
                    str = "Калибровка";
                    break;
                case "wlrangeout":
                    str = "Длина волны за пределами диапазона";
                    break;
                case "errordata":
                    str = "Ошибочные данные";
                    break;
                case "inputerror":
                    str = "Пожалуйста, проверьте входные данные";
                    break;
                case "saved":
                    str = "Сохранено";
                    break;
                case "unseved":
                    str = "Не сохрарнено";
                    break;
                case "open":
                    str = "Открыто";
                    break;
                /*case "blanking":
                    str = "Обнуление";
                    break;*/
                case "reactionrate":
                    str = "Скорость реакции";
                    break;
                case "activity":
                    str = "Активность";
                    break;

                case "scanspeed":
                    str = "Скорость измерения";
                    break;
                case "speed":
                    str = "Скорость";
                    break;
                case "criterion":
                    str = "Критерий";
                    break;
                case "kinetics":
                    str = "Кинетика";
                    break;

                case "exporterror":
                    str = "Ошибка экспорта! Сообщение об ошибке: ";
                    break;

                case "exitconfirm":
                    str = "Вы уверены, что хотите выйти?: ";
                    break;

                case "datasaveexit":
                    str = "Файл был изменен, сначала сохраните.";
                    break;
                case "unsavedataexit":
                    str = "Результаты не сохраняются,Продолжать?";
                    break;
                case "kineticsratio":
                    str = "Кинетика скорости";
                    break;
                case "measurefinish":
                    str = "Измерение завершено";
                    break;
                case "measure":
                    str = "Измерить";
                    break;
                case "errorstopmeasure":
                    str = "Ошибка! Прекратите измерение! Сообщение об ошибке:";
                    break;
                
                case "blankfinish":
                    str = "Обнулене завершено";
                    break;
                
                case "norangedata":
                    str = "Нет данных в диапазоне координат, установите!";
                    break;
                case "time":
                    str = "Время";
                    break;
                case "pause":
                    str = "Пауза";
                    break;

                case "nomethod":
                    str = "Пожалуйста, сначала создайте метод";
                    break;

                case "delaytime":
                    str = "Время задержки";
                    break;
                case "Differentialtime":
                    str = "Дифференциальное время";
                    break;   
                case "linearpj":
                    str = "Линейный критерий";
                    break;  
                case "wl":
                    str = "Длина волны";
                    break;
                case "interval":
                    str = "Интервал (сек)";
                    break;
                case "miniinterval":
                    str = "Непрерывное измерение в течение 12 часов, Интервал должен быть не менее 1 секунды. Каждые дополнительные 12 часов Интервал увеличивается минимум на 1 секунду. Выберите подходящий Интервал измерения.";
                    break;                
                case "recommand":
                    str = "Чтобы убедиться, что он работает правильно, рекомендуется увеличивать на одну секунду каждые 10 нм.";
                    break;         
                case "maxv1":
                    str = "Оно должно быть числовым и меньше максимального!";
                    break;    
                case "minv1":
                    str = "Оно должно быть числовым и больше минимального!";
                    break;    
                case "resume":
                    str = "Продолжить";
                    break;
                case "savemethodwithes":
                    str = "Перед использованием методы должны быть сохранены и подписаны";
                    break;
                case "noesmethod":
                    str = "Метод не имеет электронной подписи и не может быть использован.";
                    break;
                case "parerror":
                    str = "Ошибка параметров!";
                    break;
                case "timescan":
                    str = "Временное сканирование";
                    break;
                case "logupdateM":
                    str = "Метод обновлен";
                    break;
                case "Abs":
                    str = "Abs";
                    break;
                case "standardcurve":
                    str = "Стандартный график";
                    break;
                case "inputr":
                    str = "Ввод коэффициентов уравнения";
                    break;                    
                case "active":
                    str = "Активный";
                    break;            
                case "closed":
                    str = "Closed";
                    break;
                case "cacuerror":
                    str = "Знаменатель равен нулю, и формулу невозможно вычислить";
                    break;
                case "smcnterror":
                    str = "Требуется подсчет образцов!";
                    break;
                case "dualinputr":
                    str = "2-компонентный анализ - Новые Настройки - Ввод коэффиентов";
                    break;
                case "measrues":
                    str = "Измерение стандартных образцов";
                    break;                    
                case "dualmeas":
                    str = "2-компонентный анализ - Новые Настройки - Измерение стандартных образцов";
                    break;                    
                case "inputs":
                    str = "Ввод стандартных образцов";
                    break;             
                case "dualinputs":
                    str = "2-компонентный анализ - Новые Настройки - Ввод образцов";
                    break;           
                case "dualFerror":
                    str = "Должны быть четыре значения оптической плотности, и ни одно из них не может быть нулевым.";
                    break;                           
                case "Incompletedata":
                    str = "Неполные данные!!";
                    break;                   
                case "Dualcomponet":
                    str = "2-компонентный анализ";
                    break;               
                case "measureing":
                    str = "Измерение";
                    break;    
                case "movecursor":
                    str = "Пожалуйста, переместите курсор в правую строку!";
                    break;
                case "inblanking":
                    str = "Калибровка";
                    break;
                case "fileunsave":
                    str = "Пожалуйста, сначала сохраните файл!";
                    break;
                case "nodata":
                    str = "Нет данных!";
                    break;
                case "quadatalist":
                    str = "Количественный анализ - Список данных";
                    break;
                case "deleteconfirm":
                    str = "Удалить выбранные результаты, Продолжить?";
                    break;
                case "logdelData":
                    str = "Удалить данные";
                    break;
                case "dna1":
                    str = "ДНК Метод 1";
                    break;
                case "dna2":
                    str = "ДНК Метод 2";
                    break;
                case "quamethod":
                    str = "Количественный анализ - метод";
                    break;
                case "dnamethod":
                    str = "ДНК/Протеин - метод";
                    break;
                case "meamethod":
                    str = "Метод измерения";
                    break;
                case "standardcurveequation":
                    str = "Стандартное уравнение кривой";
                    break;
                case "conce":
                    str = "Конц";
                    break;
                case "biogmethod":
                    str = "Биометрический метод";
                    break;
                case "maxv":
                    str = "Максимум";
                    break;
                case "minv":
                    str = "Минимум";
                    break;
                case "lowery":
                    str = "Метод Лоури";
                    break;
                case "uv":
                    str = "УФ метод";
                    break;
                case "bca":
                    str = "BCA метод";
                    break;
                case "cbb":
                    str = "CBB метод";
                    break;
                case "biuret":
                    str = "Биуретовый метод";
                    break;
                case "r":
                    str = "Коэффициент";
                    break;
                case "wlerror":
                    str = "Длина волны не соответствует количеству длин волн.";
                    break;
                case "rerror":
                    str = "Ошибка загрузки параметра метода";
                    break;
                case "quainputr":
                    str = "Количественное измерение - Новый метод - Введите коэффициенты уравнения";
                    break;
                case "proteininputr":
                    str = "Измерение белка - Новый метод - Введите коэффициенты уравнения";
                    break;
                case "errorformula":
                    str = "Ошибка формулы, проверьте";
                    break;
                case "number":
                    str = "Номер";
                    break;
                case "loadparerror":
                    str = "Ошибка загрузки параметров";
                    break;
                case "noneedsample":
                    str = "Образец не нужен!";
                    break;
                case "proteinmeas":
                    str = "Анализ ДНК / белков - Новый метод - Измерение стандартных образцов";
                    break;
                case "quameas":
                    str = "Количественное анализ - Новый метод - Измерение стандартных образцов";
                    break;
                case "quainputs":
                    str = "Количественный анализ - новый метод - Ввод стандартных образцов";
                    break;
                case "proteininputs":
                    str = "Анализ ДНК / белков - новый метод - Ввод стандартных образцов";
                    break;
                case "type":
                    str = "Тип прибора";
                    break;
                case "serino":
                    str = "Серийный номер";
                    break;
                case "specturmrange":
                    str = "Спектральный диапазон";
                    break;
                case "lightswitch":
                    str = "Выключатель";
                    break;
                case "operatemode":
                    str = "Рабочий режим";
                    break;
                case "operators":
                    str = "Пользователь";
                    break;
                case "measuretime":
                    str = "Время измерений";
                    break;
                case "measurechart":
                    str = "График измерений";
                    break;
                case "photometricmode":
                    str = "Режим измерения";
                    break;
                case "scanrange":
                    str = "Диапазон измерений";
                    break;
                case "scaninterval":
                    str = "Интервал сканирования";
                    break;
                case "lightlength":
                    str = "Оптический путь";
                    break;
                /*case "scanspeed":
                    str = "Скорость";
                    break;*/
                default:
                    str = value;
                    break;

            }
            return str;
        }
        public static string getXmlValue(string xmlElement, string xmlAttribute)
        {
            IEnumerable<XElement> xelements = XDocument.Load(Directory.GetCurrentDirectory() + "\\ParmasSet.xml").Descendants((XName)xmlElement).Select<XElement, XElement>((Func<XElement, XElement>)(c => c));
            string str = "";
            foreach (XContainer xcontainer in xelements)
                if (xcontainer.Element((XName)xmlAttribute) != null)
                    str = xcontainer.Element((XName)xmlAttribute).Value.ToString();
            return str;
        }

        public static void setXmlValue(string xmlElement, string xmlAttribute, string xmlValue)
        {
            string str = Directory.GetCurrentDirectory() + "\\ParmasSet.xml";
            if (!File.Exists(str))
                CommonFun.CreateParmasSet(str);
            XDocument xdocument = XDocument.Load(str);
            xdocument.Element((XName)"Params").Element((XName)xmlElement).Element((XName)xmlAttribute).SetValue((object)xmlValue);
            xdocument.Save(str);
        }

        public static void CreateParmasSet(string filename)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string)null);
            xmlDocument.AppendChild((XmlNode)xmlDeclaration);
            XmlNode element1 = (XmlNode)xmlDocument.CreateElement("Params");
            xmlDocument.AppendChild(element1);
            XmlNode node1 = xmlDocument.CreateNode(XmlNodeType.Element, "MeasureParams", (string)null);
            XmlElement element2 = xmlDocument.CreateElement("C_SampleName");
            node1.AppendChild((XmlNode)element2);
            XmlElement element3 = xmlDocument.CreateElement("C_Mode");
            node1.AppendChild((XmlNode)element3);
            XmlElement element4 = xmlDocument.CreateElement("C_ScanSpeed");
            node1.AppendChild((XmlNode)element4);
            XmlElement element5 = xmlDocument.CreateElement("C_BeginWL");
            node1.AppendChild((XmlNode)element5);
            XmlElement element6 = xmlDocument.CreateElement("C_EndWL");
            node1.AppendChild((XmlNode)element6);
            XmlElement element7 = xmlDocument.CreateElement("C_StepLen");
            node1.AppendChild((XmlNode)element7);
            XmlElement element8 = xmlDocument.CreateElement("C_ScanCNT");
            node1.AppendChild((XmlNode)element8);
            XmlElement element9 = xmlDocument.CreateElement("C_Interval");
            node1.AppendChild((XmlNode)element9);
            XmlElement element10 = xmlDocument.CreateElement("C_Intervals");
            node1.AppendChild((XmlNode)element10);
            XmlElement element11 = xmlDocument.CreateElement("C_SLength");
            node1.AppendChild((XmlNode)element11);
            XmlElement element12 = xmlDocument.CreateElement("C_Precision");
            node1.AppendChild((XmlNode)element12);
            XmlElement element13 = xmlDocument.CreateElement("C_SamPool");
            node1.AppendChild((XmlNode)element13);
            XmlElement element14 = xmlDocument.CreateElement("PeakTS");
            node1.AppendChild((XmlNode)element14);
            XmlElement element15 = xmlDocument.CreateElement("ValleyTS");
            node1.AppendChild((XmlNode)element15);
            element1.AppendChild(node1);
            XmlNode node2 = xmlDocument.CreateNode(XmlNodeType.Element, "ShowParams", (string)null);
            XmlElement element16 = xmlDocument.CreateElement("AutoXY");
            node2.AppendChild((XmlNode)element16);
            XmlElement element17 = xmlDocument.CreateElement("xMax");
            node2.AppendChild((XmlNode)element17);
            XmlElement element18 = xmlDocument.CreateElement("xMin");
            node2.AppendChild((XmlNode)element18);
            XmlElement element19 = xmlDocument.CreateElement("yMax");
            node2.AppendChild((XmlNode)element19);
            XmlElement element20 = xmlDocument.CreateElement("yMin");
            node2.AppendChild((XmlNode)element20);
            XmlElement element21 = xmlDocument.CreateElement("MulShow");
            node2.AppendChild((XmlNode)element21);
            XmlElement element22 = xmlDocument.CreateElement("AutoPrint");
            node2.AppendChild((XmlNode)element22);
            XmlElement element23 = xmlDocument.CreateElement("AutoSave");
            node2.AppendChild((XmlNode)element23);
            element1.AppendChild(node2);
            XmlNode node3 = xmlDocument.CreateNode(XmlNodeType.Element, "PrintParams", (string)null);
            XmlElement element24 = xmlDocument.CreateElement("ShowComImage");
            node3.AppendChild((XmlNode)element24);
            XmlElement element25 = xmlDocument.CreateElement("ComImage");
            node3.AppendChild((XmlNode)element25);
            XmlElement element26 = xmlDocument.CreateElement("Title");
            node3.AppendChild((XmlNode)element26);
            XmlElement element27 = xmlDocument.CreateElement("ShowAddtional");
            node3.AppendChild((XmlNode)element27);
            XmlElement element28 = xmlDocument.CreateElement("Addtional");
            node3.AppendChild((XmlNode)element28);
            XmlElement element29 = xmlDocument.CreateElement("ShowDes");
            node3.AppendChild((XmlNode)element29);
            XmlElement element30 = xmlDocument.CreateElement("Describtion");
            node3.AppendChild((XmlNode)element30);
            XmlElement element31 = xmlDocument.CreateElement("ShowInsAndUser");
            node3.AppendChild((XmlNode)element31);
            XmlElement element32 = xmlDocument.CreateElement("ShowMeasure");
            node3.AppendChild((XmlNode)element32);
            XmlElement element33 = xmlDocument.CreateElement("ShowCurve");
            node3.AppendChild((XmlNode)element33);
            XmlElement element34 = xmlDocument.CreateElement("ShowAllData");
            node3.AppendChild((XmlNode)element34);
            XmlElement element35 = xmlDocument.CreateElement("ShowPeak");
            node3.AppendChild((XmlNode)element35);
            XmlElement element36 = xmlDocument.CreateElement("ShowValley");
            node3.AppendChild((XmlNode)element36);
            element1.AppendChild(node3);
            XmlNode node4 = xmlDocument.CreateNode(XmlNodeType.Element, "QuaParams", (string)null);
            XmlElement element37 = xmlDocument.CreateElement("MeaMethod");
            node4.AppendChild((XmlNode)element37);
            XmlElement element38 = xmlDocument.CreateElement("WL");
            node4.AppendChild((XmlNode)element38);
            XmlElement element39 = xmlDocument.CreateElement("R");
            node4.AppendChild((XmlNode)element39);
            XmlElement element40 = xmlDocument.CreateElement("MCnt");
            node4.AppendChild((XmlNode)element40);
            XmlElement element41 = xmlDocument.CreateElement("Length");
            node4.AppendChild((XmlNode)element41);
            XmlElement element42 = xmlDocument.CreateElement("EConvert");
            node4.AppendChild((XmlNode)element42);
            XmlElement element43 = xmlDocument.CreateElement("Threshold");
            node4.AppendChild((XmlNode)element43);
            XmlElement element44 = xmlDocument.CreateElement("Equation");
            node4.AppendChild((XmlNode)element44);
            XmlElement element45 = xmlDocument.CreateElement("Fitting");
            node4.AppendChild((XmlNode)element45);
            XmlElement element46 = xmlDocument.CreateElement("CapMethod");
            node4.AppendChild((XmlNode)element46);
            XmlElement element47 = xmlDocument.CreateElement("ZeroB");
            node4.AppendChild((XmlNode)element47);
            XmlElement element48 = xmlDocument.CreateElement("SamCnt");
            node4.AppendChild((XmlNode)element48);
            XmlElement element49 = xmlDocument.CreateElement("Unit");
            node4.AppendChild((XmlNode)element49);
            XmlElement element50 = xmlDocument.CreateElement("Sample");
            node4.AppendChild((XmlNode)element50);
            XmlElement element51 = xmlDocument.CreateElement("K0");
            node4.AppendChild((XmlNode)element51);
            XmlElement element52 = xmlDocument.CreateElement("K1");
            node4.AppendChild((XmlNode)element52);
            XmlElement element53 = xmlDocument.CreateElement("K2");
            node4.AppendChild((XmlNode)element53);
            XmlElement element54 = xmlDocument.CreateElement("K3");
            node4.AppendChild((XmlNode)element54);
            XmlElement element55 = xmlDocument.CreateElement("AFCS");
            node4.AppendChild((XmlNode)element55);
            XmlElement element56 = xmlDocument.CreateElement("K10");
            node4.AppendChild((XmlNode)element56);
            XmlElement element57 = xmlDocument.CreateElement("K11");
            node4.AppendChild((XmlNode)element57);
            XmlElement element58 = xmlDocument.CreateElement("K12");
            node4.AppendChild((XmlNode)element58);
            XmlElement element59 = xmlDocument.CreateElement("K13");
            node4.AppendChild((XmlNode)element59);
            XmlElement element60 = xmlDocument.CreateElement("CFCS");
            node4.AppendChild((XmlNode)element60);
            element1.AppendChild(node4);
            XmlNode node5 = xmlDocument.CreateNode(XmlNodeType.Element, "QuaPrintParams", (string)null);
            XmlElement element61 = xmlDocument.CreateElement("ShowComImage");
            node5.AppendChild((XmlNode)element61);
            XmlElement element62 = xmlDocument.CreateElement("ComImage");
            node5.AppendChild((XmlNode)element62);
            XmlElement element63 = xmlDocument.CreateElement("Title");
            node5.AppendChild((XmlNode)element63);
            XmlElement element64 = xmlDocument.CreateElement("ShowAddtional");
            node5.AppendChild((XmlNode)element64);
            XmlElement element65 = xmlDocument.CreateElement("Addtional");
            node5.AppendChild((XmlNode)element65);
            XmlElement element66 = xmlDocument.CreateElement("ShowDes");
            node5.AppendChild((XmlNode)element66);
            XmlElement element67 = xmlDocument.CreateElement("Describtion");
            node5.AppendChild((XmlNode)element67);
            XmlElement element68 = xmlDocument.CreateElement("ShowInsAndUser");
            node5.AppendChild((XmlNode)element68);
            XmlElement element69 = xmlDocument.CreateElement("ShowMeasure");
            node5.AppendChild((XmlNode)element69);
            XmlElement element70 = xmlDocument.CreateElement("ShowCurve");
            node5.AppendChild((XmlNode)element70);
            XmlElement element71 = xmlDocument.CreateElement("ShowStandardCurve");
            node5.AppendChild((XmlNode)element71);
            XmlElement element72 = xmlDocument.CreateElement("ShowStandardData");
            node5.AppendChild((XmlNode)element72);
            element1.AppendChild(node5);
            XmlNode node6 = xmlDocument.CreateNode(XmlNodeType.Element, "MulMethod", (string)null);
            XmlElement element73 = xmlDocument.CreateElement("C_mode");
            node6.AppendChild((XmlNode)element73);
            XmlElement element74 = xmlDocument.CreateElement("WLCnt");
            node6.AppendChild((XmlNode)element74);
            XmlElement element75 = xmlDocument.CreateElement("WL");
            node6.AppendChild((XmlNode)element75);
            XmlElement element76 = xmlDocument.CreateElement("MeaMethod");
            node6.AppendChild((XmlNode)element76);
            XmlElement element77 = xmlDocument.CreateElement("R");
            node6.AppendChild((XmlNode)element77);
            XmlElement element78 = xmlDocument.CreateElement("MCnt");
            node6.AppendChild((XmlNode)element78);
            XmlElement element79 = xmlDocument.CreateElement("Length");
            node6.AppendChild((XmlNode)element79);
            XmlElement element80 = xmlDocument.CreateElement("EConvert");
            node6.AppendChild((XmlNode)element80);
            XmlElement element81 = xmlDocument.CreateElement("Threshold");
            node6.AppendChild((XmlNode)element81);
            element1.AppendChild(node6);
            XmlNode node7 = xmlDocument.CreateNode(XmlNodeType.Element, "MulPrintParams", (string)null);
            XmlElement element82 = xmlDocument.CreateElement("ShowComImage");
            node7.AppendChild((XmlNode)element82);
            XmlElement element83 = xmlDocument.CreateElement("ComImage");
            node7.AppendChild((XmlNode)element83);
            XmlElement element84 = xmlDocument.CreateElement("Title");
            node7.AppendChild((XmlNode)element84);
            XmlElement element85 = xmlDocument.CreateElement("ShowAddtional");
            node7.AppendChild((XmlNode)element85);
            XmlElement element86 = xmlDocument.CreateElement("Addtional");
            node7.AppendChild((XmlNode)element86);
            XmlElement element87 = xmlDocument.CreateElement("ShowDes");
            node7.AppendChild((XmlNode)element87);
            XmlElement element88 = xmlDocument.CreateElement("Describtion");
            node7.AppendChild((XmlNode)element88);
            XmlElement element89 = xmlDocument.CreateElement("ShowInsAndUser");
            node7.AppendChild((XmlNode)element89);
            element1.AppendChild(node7);
            XmlNode node8 = xmlDocument.CreateNode(XmlNodeType.Element, "PhoMethod", (string)null);
            XmlElement element90 = xmlDocument.CreateElement("C_mode");
            node8.AppendChild((XmlNode)element90);
            XmlElement element91 = xmlDocument.CreateElement("R");
            node8.AppendChild((XmlNode)element91);
            XmlElement element92 = xmlDocument.CreateElement("WL");
            node8.AppendChild((XmlNode)element92);
            element1.AppendChild(node8);
            XmlNode node9 = xmlDocument.CreateNode(XmlNodeType.Element, "TimeMethod", (string)null);
            XmlElement element93 = xmlDocument.CreateElement("C_mode");
            node9.AppendChild((XmlNode)element93);
            XmlElement element94 = xmlDocument.CreateElement("WL");
            node9.AppendChild((XmlNode)element94);
            XmlElement element95 = xmlDocument.CreateElement("Time");
            node9.AppendChild((XmlNode)element95);
            XmlElement element96 = xmlDocument.CreateElement("Interval");
            node9.AppendChild((XmlNode)element96);
            XmlElement element97 = xmlDocument.CreateElement("Length");
            node9.AppendChild((XmlNode)element97);
            XmlElement element98 = xmlDocument.CreateElement("EConvert");
            node9.AppendChild((XmlNode)element98);
            XmlElement element99 = xmlDocument.CreateElement("AutoXY");
            node9.AppendChild((XmlNode)element99);
            XmlElement element100 = xmlDocument.CreateElement("xMax");
            node9.AppendChild((XmlNode)element100);
            XmlElement element101 = xmlDocument.CreateElement("xMin");
            node9.AppendChild((XmlNode)element101);
            XmlElement element102 = xmlDocument.CreateElement("yMax");
            node9.AppendChild((XmlNode)element102);
            XmlElement element103 = xmlDocument.CreateElement("yMin");
            node9.AppendChild((XmlNode)element103);
            XmlElement element104 = xmlDocument.CreateElement("MulShow");
            node9.AppendChild((XmlNode)element104);
            XmlElement element105 = xmlDocument.CreateElement("AutoPrint");
            node9.AppendChild((XmlNode)element105);
            XmlElement element106 = xmlDocument.CreateElement("AutoSave");
            node9.AppendChild((XmlNode)element106);
            element1.AppendChild(node9);
            XmlNode node10 = xmlDocument.CreateNode(XmlNodeType.Element, "TimePrintParams", (string)null);
            XmlElement element107 = xmlDocument.CreateElement("Title");
            node10.AppendChild((XmlNode)element107);
            XmlElement element108 = xmlDocument.CreateElement("ShowAddtional");
            node10.AppendChild((XmlNode)element108);
            XmlElement element109 = xmlDocument.CreateElement("Addtional");
            node10.AppendChild((XmlNode)element109);
            XmlElement element110 = xmlDocument.CreateElement("ShowDes");
            node10.AppendChild((XmlNode)element110);
            XmlElement element111 = xmlDocument.CreateElement("Describtion");
            node10.AppendChild((XmlNode)element111);
            XmlElement element112 = xmlDocument.CreateElement("ShowInsAndUser");
            node10.AppendChild((XmlNode)element112);
            XmlElement element113 = xmlDocument.CreateElement("ShowMeasure");
            node10.AppendChild((XmlNode)element113);
            XmlElement element114 = xmlDocument.CreateElement("ShowCurve");
            node10.AppendChild((XmlNode)element114);
            element1.AppendChild(node10);
            XmlNode node11 = xmlDocument.CreateNode(XmlNodeType.Element, "Kinetics", (string)null);
            XmlElement element115 = xmlDocument.CreateElement("C_mode");
            node11.AppendChild((XmlNode)element115);
            XmlElement element116 = xmlDocument.CreateElement("WL");
            node11.AppendChild((XmlNode)element116);
            XmlElement element117 = xmlDocument.CreateElement("BackWL");
            node11.AppendChild((XmlNode)element117);
            XmlElement element118 = xmlDocument.CreateElement("Time");
            node11.AppendChild((XmlNode)element118);
            XmlElement element119 = xmlDocument.CreateElement("Interval");
            node11.AppendChild((XmlNode)element119);
            XmlElement element120 = xmlDocument.CreateElement("Length");
            node11.AppendChild((XmlNode)element120);
            XmlElement element121 = xmlDocument.CreateElement("EConvert");
            node11.AppendChild((XmlNode)element121);
            XmlElement element122 = xmlDocument.CreateElement("DelayTime");
            node11.AppendChild((XmlNode)element122);
            XmlElement element123 = xmlDocument.CreateElement("Rate");
            node11.AppendChild((XmlNode)element123);
            XmlElement element124 = xmlDocument.CreateElement("Criterion");
            node11.AppendChild((XmlNode)element124);
            XmlElement element125 = xmlDocument.CreateElement("DiffInterval");
            node11.AppendChild((XmlNode)element125);
            XmlElement element126 = xmlDocument.CreateElement("AutoXY");
            node11.AppendChild((XmlNode)element126);
            XmlElement element127 = xmlDocument.CreateElement("xMax");
            node11.AppendChild((XmlNode)element127);
            XmlElement element128 = xmlDocument.CreateElement("xMin");
            node11.AppendChild((XmlNode)element128);
            XmlElement element129 = xmlDocument.CreateElement("yMax");
            node11.AppendChild((XmlNode)element129);
            XmlElement element130 = xmlDocument.CreateElement("yMin");
            node11.AppendChild((XmlNode)element130);
            XmlElement element131 = xmlDocument.CreateElement("MulShow");
            node11.AppendChild((XmlNode)element131);
            XmlElement element132 = xmlDocument.CreateElement("AutoPrint");
            node11.AppendChild((XmlNode)element132);
            XmlElement element133 = xmlDocument.CreateElement("AutoSave");
            node11.AppendChild((XmlNode)element133);
            element1.AppendChild(node11);
            XmlNode node12 = xmlDocument.CreateNode(XmlNodeType.Element, "KinPrintParams", (string)null);
            XmlElement element134 = xmlDocument.CreateElement("Title");
            node12.AppendChild((XmlNode)element134);
            XmlElement element135 = xmlDocument.CreateElement("ShowAddtional");
            node12.AppendChild((XmlNode)element135);
            XmlElement element136 = xmlDocument.CreateElement("Addtional");
            node12.AppendChild((XmlNode)element136);
            XmlElement element137 = xmlDocument.CreateElement("ShowDes");
            node12.AppendChild((XmlNode)element137);
            XmlElement element138 = xmlDocument.CreateElement("Describtion");
            node12.AppendChild((XmlNode)element138);
            XmlElement element139 = xmlDocument.CreateElement("ShowInsAndUser");
            node12.AppendChild((XmlNode)element139);
            XmlElement element140 = xmlDocument.CreateElement("ShowMeasure");
            node12.AppendChild((XmlNode)element140);
            XmlElement element141 = xmlDocument.CreateElement("ShowCurve");
            node12.AppendChild((XmlNode)element141);
            element1.AppendChild(node12);
            XmlNode node13 = xmlDocument.CreateNode(XmlNodeType.Element, "DNAParams", (string)null);
            XmlElement element142 = xmlDocument.CreateElement("MeaMethod");
            node13.AppendChild((XmlNode)element142);
            XmlElement element143 = xmlDocument.CreateElement("WL");
            node13.AppendChild((XmlNode)element143);
            XmlElement element144 = xmlDocument.CreateElement("BackWL");
            node13.AppendChild((XmlNode)element144);
            XmlElement element145 = xmlDocument.CreateElement("R");
            node13.AppendChild((XmlNode)element145);
            XmlElement element146 = xmlDocument.CreateElement("MCnt");
            node13.AppendChild((XmlNode)element146);
            XmlElement element147 = xmlDocument.CreateElement("Length");
            node13.AppendChild((XmlNode)element147);
            XmlElement element148 = xmlDocument.CreateElement("EConvert");
            node13.AppendChild((XmlNode)element148);
            XmlElement element149 = xmlDocument.CreateElement("Threshold");
            node13.AppendChild((XmlNode)element149);
            XmlElement element150 = xmlDocument.CreateElement("Unit");
            node13.AppendChild((XmlNode)element150);
            XmlElement element151 = xmlDocument.CreateElement("Equation");
            node13.AppendChild((XmlNode)element151);
            XmlElement element152 = xmlDocument.CreateElement("Fitting");
            node13.AppendChild((XmlNode)element152);
            XmlElement element153 = xmlDocument.CreateElement("CapMethod");
            node13.AppendChild((XmlNode)element153);
            XmlElement element154 = xmlDocument.CreateElement("ZeroB");
            node13.AppendChild((XmlNode)element154);
            XmlElement element155 = xmlDocument.CreateElement("SamCnt");
            node13.AppendChild((XmlNode)element155);
            XmlElement element156 = xmlDocument.CreateElement("Sample");
            node13.AppendChild((XmlNode)element156);
            XmlElement element157 = xmlDocument.CreateElement("K0");
            node13.AppendChild((XmlNode)element157);
            XmlElement element158 = xmlDocument.CreateElement("K1");
            node13.AppendChild((XmlNode)element158);
            XmlElement element159 = xmlDocument.CreateElement("K2");
            node13.AppendChild((XmlNode)element159);
            XmlElement element160 = xmlDocument.CreateElement("K3");
            node13.AppendChild((XmlNode)element160);
            XmlElement element161 = xmlDocument.CreateElement("AFCS");
            node13.AppendChild((XmlNode)element161);
            XmlElement element162 = xmlDocument.CreateElement("K10");
            node13.AppendChild((XmlNode)element162);
            XmlElement element163 = xmlDocument.CreateElement("K11");
            node13.AppendChild((XmlNode)element163);
            XmlElement element164 = xmlDocument.CreateElement("K12");
            node13.AppendChild((XmlNode)element164);
            XmlElement element165 = xmlDocument.CreateElement("K13");
            node13.AppendChild((XmlNode)element165);
            XmlElement element166 = xmlDocument.CreateElement("CFCS");
            node13.AppendChild((XmlNode)element166);
            element1.AppendChild(node13);
            XmlNode node14 = xmlDocument.CreateNode(XmlNodeType.Element, "DNAPrintParams", (string)null);
            XmlElement element167 = xmlDocument.CreateElement("ShowComImage");
            node14.AppendChild((XmlNode)element167);
            XmlElement element168 = xmlDocument.CreateElement("ComImage");
            node14.AppendChild((XmlNode)element168);
            XmlElement element169 = xmlDocument.CreateElement("Title");
            node14.AppendChild((XmlNode)element169);
            XmlElement element170 = xmlDocument.CreateElement("ShowAddtional");
            node14.AppendChild((XmlNode)element170);
            XmlElement element171 = xmlDocument.CreateElement("Addtional");
            node14.AppendChild((XmlNode)element171);
            XmlElement element172 = xmlDocument.CreateElement("ShowDes");
            node14.AppendChild((XmlNode)element172);
            XmlElement element173 = xmlDocument.CreateElement("Describtion");
            node14.AppendChild((XmlNode)element173);
            XmlElement element174 = xmlDocument.CreateElement("ShowInsAndUser");
            node14.AppendChild((XmlNode)element174);
            XmlElement element175 = xmlDocument.CreateElement("ShowMeasure");
            node14.AppendChild((XmlNode)element175);
            XmlElement element176 = xmlDocument.CreateElement("ShowCurve");
            node14.AppendChild((XmlNode)element176);
            XmlElement element177 = xmlDocument.CreateElement("ShowStandardCurve");
            node14.AppendChild((XmlNode)element177);
            XmlElement element178 = xmlDocument.CreateElement("ShowStandardData");
            node14.AppendChild((XmlNode)element178);
            element1.AppendChild(node14);
            XmlNode node15 = xmlDocument.CreateNode(XmlNodeType.Element, "DualParams", (string)null);
            XmlElement element179 = xmlDocument.CreateElement("WL1");
            node15.AppendChild((XmlNode)element179);
            XmlElement element180 = xmlDocument.CreateElement("WL2");
            node15.AppendChild((XmlNode)element180);
            XmlElement element181 = xmlDocument.CreateElement("MCnt");
            node15.AppendChild((XmlNode)element181);
            XmlElement element182 = xmlDocument.CreateElement("Length");
            node15.AppendChild((XmlNode)element182);
            XmlElement element183 = xmlDocument.CreateElement("EConvert");
            node15.AppendChild((XmlNode)element183);
            XmlElement element184 = xmlDocument.CreateElement("Equation");
            node15.AppendChild((XmlNode)element184);
            XmlElement element185 = xmlDocument.CreateElement("Fitting");
            node15.AppendChild((XmlNode)element185);
            XmlElement element186 = xmlDocument.CreateElement("CapMethod");
            node15.AppendChild((XmlNode)element186);
            XmlElement element187 = xmlDocument.CreateElement("ZeroB");
            node15.AppendChild((XmlNode)element187);
            XmlElement element188 = xmlDocument.CreateElement("SamCnt");
            node15.AppendChild((XmlNode)element188);
            XmlElement element189 = xmlDocument.CreateElement("Unit");
            node15.AppendChild((XmlNode)element189);
            XmlElement element190 = xmlDocument.CreateElement("F1");
            node15.AppendChild((XmlNode)element190);
            XmlElement element191 = xmlDocument.CreateElement("F2");
            node15.AppendChild((XmlNode)element191);
            XmlElement element192 = xmlDocument.CreateElement("F3");
            node15.AppendChild((XmlNode)element192);
            XmlElement element193 = xmlDocument.CreateElement("F4");
            node15.AppendChild((XmlNode)element193);
            XmlElement element194 = xmlDocument.CreateElement("Sample");
            node15.AppendChild((XmlNode)element194);
            XmlElement element195 = xmlDocument.CreateElement("cbSample");
            node15.AppendChild((XmlNode)element195);
            XmlElement element196 = xmlDocument.CreateElement("K10");
            node15.AppendChild((XmlNode)element196);
            XmlElement element197 = xmlDocument.CreateElement("K11");
            node15.AppendChild((XmlNode)element197);
            XmlElement element198 = xmlDocument.CreateElement("K12");
            node15.AppendChild((XmlNode)element198);
            XmlElement element199 = xmlDocument.CreateElement("K13");
            node15.AppendChild((XmlNode)element199);
            XmlElement element200 = xmlDocument.CreateElement("AFCS1");
            node15.AppendChild((XmlNode)element200);
            XmlElement element201 = xmlDocument.CreateElement("R11");
            node15.AppendChild((XmlNode)element201);
            XmlElement element202 = xmlDocument.CreateElement("K100");
            node15.AppendChild((XmlNode)element202);
            XmlElement element203 = xmlDocument.CreateElement("K110");
            node15.AppendChild((XmlNode)element203);
            XmlElement element204 = xmlDocument.CreateElement("K120");
            node15.AppendChild((XmlNode)element204);
            XmlElement element205 = xmlDocument.CreateElement("K130");
            node15.AppendChild((XmlNode)element205);
            XmlElement element206 = xmlDocument.CreateElement("CFCS1");
            node15.AppendChild((XmlNode)element206);
            XmlElement element207 = xmlDocument.CreateElement("R12");
            node15.AppendChild((XmlNode)element207);
            XmlElement element208 = xmlDocument.CreateElement("K20");
            node15.AppendChild((XmlNode)element208);
            XmlElement element209 = xmlDocument.CreateElement("K21");
            node15.AppendChild((XmlNode)element209);
            XmlElement element210 = xmlDocument.CreateElement("K22");
            node15.AppendChild((XmlNode)element210);
            XmlElement element211 = xmlDocument.CreateElement("K23");
            node15.AppendChild((XmlNode)element211);
            XmlElement element212 = xmlDocument.CreateElement("AFCS2");
            node15.AppendChild((XmlNode)element212);
            XmlElement element213 = xmlDocument.CreateElement("R21");
            node15.AppendChild((XmlNode)element213);
            XmlElement element214 = xmlDocument.CreateElement("K200");
            node15.AppendChild((XmlNode)element214);
            XmlElement element215 = xmlDocument.CreateElement("K210");
            node15.AppendChild((XmlNode)element215);
            XmlElement element216 = xmlDocument.CreateElement("K220");
            node15.AppendChild((XmlNode)element216);
            XmlElement element217 = xmlDocument.CreateElement("K230");
            node15.AppendChild((XmlNode)element217);
            XmlElement element218 = xmlDocument.CreateElement("CFCS2");
            node15.AppendChild((XmlNode)element218);
            XmlElement element219 = xmlDocument.CreateElement("R22");
            node15.AppendChild((XmlNode)element219);
            element1.AppendChild(node15);
            XmlNode node16 = xmlDocument.CreateNode(XmlNodeType.Element, "DualPrintParams", (string)null);
            XmlElement element220 = xmlDocument.CreateElement("ShowComImage");
            node16.AppendChild((XmlNode)element220);
            XmlElement element221 = xmlDocument.CreateElement("ComImage");
            node16.AppendChild((XmlNode)element221);
            XmlElement element222 = xmlDocument.CreateElement("Title");
            node16.AppendChild((XmlNode)element222);
            XmlElement element223 = xmlDocument.CreateElement("ShowAddtional");
            node16.AppendChild((XmlNode)element223);
            XmlElement element224 = xmlDocument.CreateElement("Addtional");
            node16.AppendChild((XmlNode)element224);
            XmlElement element225 = xmlDocument.CreateElement("ShowDes");
            node16.AppendChild((XmlNode)element225);
            XmlElement element226 = xmlDocument.CreateElement("Describtion");
            node16.AppendChild((XmlNode)element226);
            XmlElement element227 = xmlDocument.CreateElement("ShowInsAndUser");
            node16.AppendChild((XmlNode)element227);
            XmlElement element228 = xmlDocument.CreateElement("ShowMeasure");
            node16.AppendChild((XmlNode)element228);
            XmlElement element229 = xmlDocument.CreateElement("ShowCurve");
            node16.AppendChild((XmlNode)element229);
            XmlElement element230 = xmlDocument.CreateElement("ShowStandardCurve");
            node16.AppendChild((XmlNode)element230);
            XmlElement element231 = xmlDocument.CreateElement("ShowStandardData");
            node16.AppendChild((XmlNode)element231);
            element1.AppendChild(node16);
            XmlNode node17 = xmlDocument.CreateNode(XmlNodeType.Element, "GMP", (string)null);
            XmlElement element232 = xmlDocument.CreateElement("wlenabled");
            node17.AppendChild((XmlNode)element232);
            XmlElement element233 = xmlDocument.CreateElement("gdenabled");
            node17.AppendChild((XmlNode)element233);
            XmlElement element234 = xmlDocument.CreateElement("zsgenabled");
            node17.AppendChild((XmlNode)element234);
            XmlElement element235 = xmlDocument.CreateElement("fblenabled");
            node17.AppendChild((XmlNode)element235);
            XmlElement element236 = xmlDocument.CreateElement("wlbw1enabled");
            node17.AppendChild((XmlNode)element236);
            XmlElement element237 = xmlDocument.CreateElement("wlname1");
            node17.AppendChild((XmlNode)element237);
            XmlElement element238 = xmlDocument.CreateElement("wlbw2enabled");
            node17.AppendChild((XmlNode)element238);
            XmlElement element239 = xmlDocument.CreateElement("wlname2");
            node17.AppendChild((XmlNode)element239);
            XmlElement element240 = xmlDocument.CreateElement("wlcnt1");
            node17.AppendChild((XmlNode)element240);
            XmlElement element241 = xmlDocument.CreateElement("wlcnt2");
            node17.AppendChild((XmlNode)element241);
            XmlElement element242 = xmlDocument.CreateElement("wl1");
            node17.AppendChild((XmlNode)element242);
            XmlElement element243 = xmlDocument.CreateElement("wl2");
            node17.AppendChild((XmlNode)element243);
            XmlElement element244 = xmlDocument.CreateElement("wlpc");
            node17.AppendChild((XmlNode)element244);
            XmlElement element245 = xmlDocument.CreateElement("gdmode");
            node17.AppendChild((XmlNode)element245);
            XmlElement element246 = xmlDocument.CreateElement("gdbw1enabled");
            node17.AppendChild((XmlNode)element246);
            XmlElement element247 = xmlDocument.CreateElement("gdwlname1");
            node17.AppendChild((XmlNode)element247);
            XmlElement element248 = xmlDocument.CreateElement("gdwlcnt1");
            node17.AppendChild((XmlNode)element248);
            XmlElement element249 = xmlDocument.CreateElement("gdwl1");
            node17.AppendChild((XmlNode)element249);
            XmlElement element250 = xmlDocument.CreateElement("gdwlvalue1");
            node17.AppendChild((XmlNode)element250);
            XmlElement element251 = xmlDocument.CreateElement("gdbw2enabled");
            node17.AppendChild((XmlNode)element251);
            XmlElement element252 = xmlDocument.CreateElement("gdwlname2");
            node17.AppendChild((XmlNode)element252);
            XmlElement element253 = xmlDocument.CreateElement("gdwlcnt2");
            node17.AppendChild((XmlNode)element253);
            XmlElement element254 = xmlDocument.CreateElement("gdwl2");
            node17.AppendChild((XmlNode)element254);
            XmlElement element255 = xmlDocument.CreateElement("gdwlvalue2");
            node17.AppendChild((XmlNode)element255);
            XmlElement element256 = xmlDocument.CreateElement("gdbw3enabled");
            node17.AppendChild((XmlNode)element256);
            XmlElement element257 = xmlDocument.CreateElement("gdwlname3");
            node17.AppendChild((XmlNode)element257);
            XmlElement element258 = xmlDocument.CreateElement("gdwlcnt3");
            node17.AppendChild((XmlNode)element258);
            XmlElement element259 = xmlDocument.CreateElement("gdwl3");
            node17.AppendChild((XmlNode)element259);
            XmlElement element260 = xmlDocument.CreateElement("gdwlvalue3");
            node17.AppendChild((XmlNode)element260);
            XmlElement element261 = xmlDocument.CreateElement("gdbw4enabled");
            node17.AppendChild((XmlNode)element261);
            XmlElement element262 = xmlDocument.CreateElement("gdwlname4");
            node17.AppendChild((XmlNode)element262);
            XmlElement element263 = xmlDocument.CreateElement("gdwlcnt4");
            node17.AppendChild((XmlNode)element263);
            XmlElement element264 = xmlDocument.CreateElement("gdwl4");
            node17.AppendChild((XmlNode)element264);
            XmlElement element265 = xmlDocument.CreateElement("gdwlvalue4");
            node17.AppendChild((XmlNode)element265);
            XmlElement element266 = xmlDocument.CreateElement("gdpc");
            node17.AppendChild((XmlNode)element266);
            XmlElement element267 = xmlDocument.CreateElement("zsgmode");
            node17.AppendChild((XmlNode)element267);
            XmlElement element268 = xmlDocument.CreateElement("zsgwl1");
            node17.AppendChild((XmlNode)element268);
            XmlElement element269 = xmlDocument.CreateElement("zsgwl2");
            node17.AppendChild((XmlNode)element269);
            XmlElement element270 = xmlDocument.CreateElement("zsgwl3");
            node17.AppendChild((XmlNode)element270);
            XmlElement element271 = xmlDocument.CreateElement("zsgwl4");
            node17.AppendChild((XmlNode)element271);
            XmlElement element272 = xmlDocument.CreateElement("zsgbw1enabled");
            node17.AppendChild((XmlNode)element272);
            XmlElement element273 = xmlDocument.CreateElement("zsgname1");
            node17.AppendChild((XmlNode)element273);
            XmlElement element274 = xmlDocument.CreateElement("zsgbw2enabled");
            node17.AppendChild((XmlNode)element274);
            XmlElement element275 = xmlDocument.CreateElement("zsgname2");
            node17.AppendChild((XmlNode)element275);
            XmlElement element276 = xmlDocument.CreateElement("zsgbw3enabled");
            node17.AppendChild((XmlNode)element276);
            XmlElement element277 = xmlDocument.CreateElement("zsgname3");
            node17.AppendChild((XmlNode)element277);
            XmlElement element278 = xmlDocument.CreateElement("zsgbw4enabled");
            node17.AppendChild((XmlNode)element278);
            XmlElement element279 = xmlDocument.CreateElement("zsgname4");
            node17.AppendChild((XmlNode)element279);
            XmlElement element280 = xmlDocument.CreateElement("zsgpc");
            node17.AppendChild((XmlNode)element280);
            XmlElement element281 = xmlDocument.CreateElement("fblname");
            node17.AppendChild((XmlNode)element281);
            XmlElement element282 = xmlDocument.CreateElement("fblbeginwl");
            node17.AppendChild((XmlNode)element282);
            XmlElement element283 = xmlDocument.CreateElement("fblendwl");
            node17.AppendChild((XmlNode)element283);
            XmlElement element284 = xmlDocument.CreateElement("fblpc");
            node17.AppendChild((XmlNode)element284);
            element1.AppendChild(node17);
            XmlNode node18 = xmlDocument.CreateNode(XmlNodeType.Element, "DKParams", (string)null);
            XmlElement element285 = xmlDocument.CreateElement("C_BeginWL");
            node18.AppendChild((XmlNode)element285);
            XmlElement element286 = xmlDocument.CreateElement("C_EndWL");
            node18.AppendChild((XmlNode)element286);
            XmlElement element287 = xmlDocument.CreateElement("C_StepLen");
            node18.AppendChild((XmlNode)element287);
            element1.AppendChild(node18);
            XmlNode node19 = xmlDocument.CreateNode(XmlNodeType.Element, "WLCal", (string)null);
            XmlElement element288 = xmlDocument.CreateElement("WLvalue");
            node19.AppendChild((XmlNode)element288);
            element1.AppendChild(node19);
            xmlDocument.Save(filename);
        }

        public static void WriteReceiveMsg(string text)
        {
            string path1 = Environment.CurrentDirectory + "\\Log";
            string path2 = path1 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "RS_ecoview.log";
            if (!Directory.Exists(path1))
                Directory.CreateDirectory(path1);
            if (!File.Exists(path2))
                File.Create(path2).Close();
            text += "\r\n";
            using (StreamWriter streamWriter = new StreamWriter(path2, true, Encoding.UTF8))
                streamWriter.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }

        public static string GetAcc(string acc)
        {
            string appConfig = CommonFun.GetAppConfig(acc);
            string str = "f0";
            switch (appConfig)
            {
                case "0":
                    str = "f0";
                    break;
                case "0.0":
                case "0.1":
                    str = "f1";
                    break;
                case "0.00":
                case "0.01":
                    str = "f2";
                    break;
                case "0.000":
                case "0.001":
                    str = "f3";
                    break;
                case "0.0000":
                case "0.0001":
                    str = "f4";
                    break;
                case "0.00000":
                case "0.00001":
                    str = "f5";
                    break;
                case "0.000000":
                case "0.000001":
                    str = "f6";
                    break;
                case "0.0000000":
                case "0.0000001":
                    str = "f7";
                    break;
            }
            return str;
        }

        public static void showbox(string msg, string type)
        {
            int num = (int)new MessageBoxFrm(msg, type).ShowDialog();
        }

        public static QuaMeaMethod GetByName(string name)
        {
            QuaMeaMethod quaMeaMethod = new QuaMeaMethod();
            string appConfig = CommonFun.GetAppConfig("Language");
            if (name == "singlewl")
            {
                quaMeaMethod.C_name = CommonFun.GetLanText("singlewl");
                quaMeaMethod.C_DM = "singlewl";
                quaMeaMethod.C_mode = "All";
                quaMeaMethod.C_gs = "A=A1";
                quaMeaMethod.RCnt = new int?(0);
                quaMeaMethod.Square = new int?(0);
                quaMeaMethod.WLCnt = new int?(1);
            }
            else if (name == "doublem")
            {
                quaMeaMethod.C_name = CommonFun.GetLanText("doublem");
                quaMeaMethod.C_DM = "doublem";
                quaMeaMethod.C_mode = "All";
                quaMeaMethod.C_gs = "A=A1-A2";
                quaMeaMethod.RCnt = new int?(2);
                quaMeaMethod.Square = new int?(0);
                quaMeaMethod.WLCnt = new int?(2);
            }
            else if (name == "doubler")
            {
                quaMeaMethod.C_name = CommonFun.GetLanText("doubler");
                quaMeaMethod.C_DM = "doubler";
                quaMeaMethod.C_mode = "All";
                quaMeaMethod.C_gs = "A=K0*A1/A2";
                quaMeaMethod.RCnt = new int?(1);
                quaMeaMethod.Square = new int?(0);
                quaMeaMethod.WLCnt = new int?(2);
            }
            else if (name == "threewl")
            {
                quaMeaMethod.C_name = CommonFun.GetLanText("threewl");
                quaMeaMethod.C_DM = "threewl";
                quaMeaMethod.C_gs = "A=A1-A2-(λ1-λ2)*(A2-A3)/(λ2-λ3)";
                quaMeaMethod.C_mode = "All";
                quaMeaMethod.RCnt = new int?(0);
                quaMeaMethod.Square = new int?(0);
                quaMeaMethod.WLCnt = new int?(3);
            }
            else if (name == "area")
            {
                quaMeaMethod.C_name = CommonFun.GetLanText("area");
                quaMeaMethod.C_DM = "area";
                quaMeaMethod.C_mode = "All";
                quaMeaMethod.C_gs = "A=∫f(A)dλ";
                quaMeaMethod.RCnt = new int?(0);
                quaMeaMethod.Square = new int?(1);
                quaMeaMethod.WLCnt = new int?(2);
            }
            return quaMeaMethod;
        }

        public static void InsertLog(string module, string content, bool nonglprecord)
        {
            if (!(CommonFun.GetAppConfig("GLPEnabled") == "true") && !nonglprecord)
                return;
           OperationLog operationLog = new OperationLog()
            {
                C_name = CommonFun.GetAppConfig("currentuser"),
                C_module = module,
                C_Operation = content,
                D_operatetime = new DateTime?(DateTime.Now)
            };
            operationLog.C_groups = CommonFun.GetUserGroup(operationLog.C_name);
            SQLiteHelper.ExecuteNonQuery(new SQLiteConnection("Data Source=programdb.db;Version=3;"), " insert into OperationLog (C_name,C_groups,D_operatetime,C_module,C_operatecontent) values (@C_name,@C_groups,@D_operatetime,@C_module,@C_operation) ", (object)operationLog.C_name, (object)operationLog.C_groups, (object)operationLog.D_operatetime, (object)operationLog.C_module, (object)operationLog.C_Operation);
        }

        public static bool Hasright(string C_name, string right)
        {
            DataSet dataSet = SQLiteHelper.ExecuteDataSet("Data Source=programdb.db;Version=3;", "select * from  GroupRights where C_rights=@C_rights and C_groups=(select C_groups from users where C_name=@C_name) ", new object[2]
            {
        (object) right,
        (object) C_name
            });
            return dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        }

        public static string GetUserGroup(string C_name)
        {
            string str = "";
            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {
                IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select C_groups from  Users where C_name=@C_name ", new object[1]
                {
          (object) C_name
                });
                while (dataReader.Read())
                    str = dataReader.IsDBNull(dataReader.GetOrdinal("C_groups")) ? (string)null : (string)dataReader["C_groups"];
                dataReader.Close();
            }
            return str;
        }

        public static string GetUserDir(string C_name)
        {
            string str = "";
            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {
                IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select C_desc from  Users where C_name=@C_name ", new object[1]
                {
          (object) C_name
                });
                while (dataReader.Read())
                    str = dataReader.IsDBNull(dataReader.GetOrdinal("C_desc")) ? (string)null : (string)dataReader["C_desc"];
                dataReader.Close();
            }
            return str;
        }

        public static int? GetLoginID(string C_name)
        {
            int? nullable = new int?();
            using (SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection("Data Source=programdb.db;Version=3;")))
            {
                IDataReader dataReader = SQLiteHelper.ExecuteReader(cmd, "select LoginID from LoginLog where C_name=@C_name and D_jssj is null order by LoginID desc LIMIT  0,1 ", new object[1]
                {
          (object) C_name
                });
                while (dataReader.Read())
                {
                    string str = dataReader.IsDBNull(dataReader.GetOrdinal("LoginID")) ? (string)null : dataReader["LoginID"].ToString();
                    try
                    {
                        nullable = new int?(Convert.ToInt32(str));
                    }
                    catch
                    {
                        nullable = new int?();
                    }
                }
                dataReader.Close();
            }
            return nullable;
        }


        public static List<QuaMeaMethod> CommonMethodList(int qua)
        {
            List<QuaMeaMethod> quaMeaMethodList = new List<QuaMeaMethod>();
            string appConfig = CommonFun.GetAppConfig("Language");
            quaMeaMethodList.Add(new QuaMeaMethod()
            {
                C_name = "Одноволновое",
                C_DM = "Одноволновое",
                C_mode = "All",
                C_gs = "A=A1",
                RCnt = new int?(0),
                Square = new int?(0),
                WLCnt = new int?(1)
            });
            quaMeaMethodList.Add(new QuaMeaMethod()
            {
                C_name = "Двухволновое",
                C_DM = "Двухволновое",
                C_mode = "All",
                C_gs = "A=A1-A2",
                RCnt = new int?(2),
                Square = new int?(0),
                WLCnt = new int?(2)
            });
            quaMeaMethodList.Add(new QuaMeaMethod()
            {
                C_name = "Трехволновое",
                C_DM = "Трехволновое",
                C_mode = "All",
                C_gs = "A=A1-A2-(λ1-λ2)*(A2-A3)/(λ2-λ3)",
                RCnt = new int?(0),
                Square = new int?(0),
                WLCnt = new int?(3)
            });
            if (qua > 0)
                quaMeaMethodList.Add(new QuaMeaMethod()
                {
                    C_name = "Границы",
                    C_DM = "Границы",
                    C_mode = "All",
                    C_gs = "A=∫f(A)dλ",
                    RCnt = new int?(0),
                    Square = new int?(1),
                    WLCnt = new int?(2)
                });
            return quaMeaMethodList;
        }

    }
}
