using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class QuaParmas
    {
        public string C_name { get; set; }

        public DateTime? D_time { get; set; }

        public string C_bz { get; set; }

        public string MeasureMethodName { get; set; }

        public string MeasureMethodNameDM { get; set; }

        public QuaMeaMethod MeasureMethod { get; set; }

        public string WL { get; set; }

        public string R { get; set; }

        public int MCnt { get; set; }

        public string Length { get; set; }

        public bool EConvert { get; set; }

        public string Limits { get; set; }

        public string Equation { get; set; }

        public string Fitting { get; set; }

        public string FittingDM { get; set; }

        public bool ZeroB { get; set; }

        public string CabMethod { get; set; }

        public string CabMethodDM { get; set; }

        public int SamCnt { get; set; }

        public string Unit { get; set; }

        public string BackWL { get; set; }

        public string C_operator { get; set; }

        public bool Cuvettemath { get; set; }

        public string ParamsCreatorES { get; set; }

        public DateTime? ParamsESTime { get; set; }

        public byte[] ParamsESImage { get; set; }

        public string PASReason { get; set; }
    }
}
