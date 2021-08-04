using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class MeasureParams
    {
        public string C_methodname { get; set; }

        public string C_SampleName { get; set; }

        public string C_Mode { get; set; }

        public string C_BeginWL { get; set; }

        public string C_EndWL { get; set; }

        public string C_StepLen { get; set; }

        public string C_ScanSpeed { get; set; }

        public string C_ScanSpeedDM { get; set; }

        public string C_ScanCNT { get; set; }

        public string C_Interval { get; set; }

        public string C_Intervals { get; set; }

        public string C_SLength { get; set; }

        public string C_SamPool { get; set; }

        public string C_Precision { get; set; }

        public ShowParams spar { get; set; }

        public string C_methodoperator { get; set; }

        public DateTime? D_Mtime { get; set; }

        public string ParamsCreatorES { get; set; }

        public DateTime? ParamsESTime { get; set; }

        public byte[] ParamsESImage { get; set; }

        public string PASReason { get; set; }
    }
}
