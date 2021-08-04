using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class MulData
    {
        public string C_name { get; set; }

        public DateTime? D_Stime { get; set; }

        public string C_bz { get; set; }

        public float SJJG { get; set; }

        public Decimal[] Avalue { get; set; }

        public DateTime? D_MTime { get; set; }
    }
}
