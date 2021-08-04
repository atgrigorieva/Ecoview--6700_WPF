using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class QuaMethod
    {
        public int Page { get; set; }

        public string InstrumentsType { get; set; }

        public string Serials { get; set; }

        public string C_name { get; set; }

        public DateTime? D_time { get; set; }

        public string C_operator { get; set; }

        public QuaParmas QPar { get; set; }

        public List<Sample> SamList { get; set; }

        public string C_mName { get; set; }

        public string C_methodcreator { get; set; }

        public DateTime? D_MTime { get; set; }

        public Decimal K0 { get; set; }

        public Decimal K1 { get; set; }

        public Decimal K2 { get; set; }

        public Decimal K3 { get; set; }

        public string AFCS { get; set; }

        public Decimal K10 { get; set; }

        public Decimal K11 { get; set; }

        public Decimal K12 { get; set; }

        public Decimal K13 { get; set; }

        public string CFCS { get; set; }

        public Decimal? R { get; set; }

        public string C_bz { get; set; }

        public string MethodCreatorES { get; set; }

        public DateTime? MethodESTime { get; set; }

        public byte[] MesthodESImage { get; set; }

        public string MESReason { get; set; }

        public List<Sample> MeasreList { get; set; }

        public List<DNAMeasureData> DNAMeaList { get; set; }

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
