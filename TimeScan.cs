// Decompiled with JetBrains decompiler
// Type: MWaveProfessional.TimeScan
// Assembly: UV Studio, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null
// MVID: B671728E-936D-44C2-9ED5-B691939AD932
// Assembly location: H:\UV Studio\UV Studio\UV Studio.exe

using System;
using System.Collections.Generic;
using System.Drawing;

namespace UVStudio
{
    [Serializable]
    public class TimeScan : TimeMethod
    {
        public string InstrumentsType { get; set; }

        public string Serials { get; set; }

        public bool status { get; set; }

        public string C_name { get; set; }

        public DateTime? D_savetime { get; set; }

        public Color color { get; set; }

        public DateTime D_Time { get; set; }

        public string C_Operator { get; set; }

        public TimeMethod MethodPar { get; set; }

        public List<MeaureData> Data { get; set; }

        public int IsShow { get; set; }

        public int ESStatus { get; set; }

        public string OperatorES { get; set; }

        public DateTime OperatorESTime { get; set; }

        public string C_reasonOP { get; set; }

        public byte[] OperatorESImage { get; set; }

        public string ReviewerES { get; set; }

        public DateTime ReviewerESTime { get; set; }

        public string C_reasonRE { get; set; }

        public byte[] ReviewerESImage { get; set; }

        public string AdminES { get; set; }

        public string C_reasonAD { get; set; }

        public DateTime AdminESTime { get; set; }

        public byte[] AdminESImage { get; set; }

        public string FileDir { get; set; }
    }
}
