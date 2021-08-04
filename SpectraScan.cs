using System;
using System.Collections.Generic;
using System.Drawing;

namespace UVStudio
{

    [Serializable]
    public class SpectraScan : MeasureParams
    {
        public string InstrumentsType { get; set; }

        public string Serials { get; set; }

        public bool status { get; set; }

        public string C_name { get; set; }

        public DateTime? D_savetime { get; set; }

        public Color color { get; set; }

        public DateTime D_Time { get; set; }

        public MeasureParams MethodPar { get; set; }

        public string C_Operator { get; set; }

        public OperateType? opertype { get; set; }

        public List<SquareData> Squaredata { get; set; }

        public string ThreeDTime { get; set; }

        public bool FromZero { get; set; }

        public bool ShowZero { get; set; }

        public Decimal Factor { get; set; }

        public Decimal NCValue { get; set; }

        public Decimal Ratio { get; set; }

        public List<MeaureData> Data { get; set; }

        public int IsShow { get; set; }

        public int ESStatus { get; set; }

        public string OperatorES { get; set; }

        public DateTime OperatorESTime { get; set; }

        public byte[] OperatorESImage { get; set; }

        public string ReviewerES { get; set; }

        public DateTime ReviewerESTime { get; set; }

        public byte[] ReviewerESImage { get; set; }

        public string AdminES { get; set; }

        public DateTime AdminESTime { get; set; }

        public byte[] AdminESImage { get; set; }

        public string C_reasonOP { get; set; }

        public string C_reasonRE { get; set; }

        public string C_reasonAD { get; set; }

        public string FileDir { get; set; }
    }
}
