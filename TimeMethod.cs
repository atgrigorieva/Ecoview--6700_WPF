// Decompiled with JetBrains decompiler
// Type: MWaveProfessional.TimeMethod
// Assembly: UV Studio, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null
// MVID: B671728E-936D-44C2-9ED5-B691939AD932
// Assembly location: H:\UV Studio\UV Studio\UV Studio.exe

using System;

namespace UVStudio
{
    [Serializable]
    public class TimeMethod
    {
        public string C_Methodname { get; set; }

        public DateTime? D_time { get; set; }

        public string C_bz { get; set; }

        public string C_mode { get; set; }

        public string C_modeDM { get; set; }

        public string WL { get; set; }

        public string BackWL { get; set; }

        public string Time { get; set; }

        public string Interval { get; set; }

        public string Length { get; set; }

        public bool EConvert { get; set; }

        public string DelayTime { get; set; }

        public string Rate { get; set; }

        public string DiffInterval { get; set; }

        public string Criterion { get; set; }

        public ShowParams spar { get; set; }

        public string C_methodoperator { get; set; }

        public string ParamsCreatorES { get; set; }

        public DateTime? ParamsESTime { get; set; }

        public byte[] ParamsESImage { get; set; }

        public string PASReason { get; set; }
    }
}
