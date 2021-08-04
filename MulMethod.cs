using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class MulMethod
    {
        public string InstrumentsType { get; set; }

        public string Serials { get; set; }

        public string C_name { get; set; }

        public DateTime? D_time { get; set; }

        public string C_methodcreator { get; set; }

        public DateTime? D_MTime { get; set; }

        public string C_mode { get; set; }

        public string WLCnt { get; set; }

        public string WL { get; set; }

        public string MeasureMethodName { get; set; }

        public QuaMeaMethod MeasureMethod { get; set; }

        public string R { get; set; }

        public int MCnt { get; set; }

        public string Length { get; set; }

        public bool EConvert { get; set; }

        public string Limits { get; set; }

        public string MethodCreatorES { get; set; }

        public DateTime? MethodESTime { get; set; }

        public byte[] MesthodESImage { get; set; }

        public string MESReason { get; set; }

        public List<MulData> MeasreList { get; set; }

        public string C_Operator { get; set; }

        public string C_head { get; set; }

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
