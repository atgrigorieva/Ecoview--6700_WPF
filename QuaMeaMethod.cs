using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class QuaMeaMethod
    {
        public string C_mode { get; set; }

        public string C_name { get; set; }

        public string C_DM { get; set; }

        public string C_gs { get; set; }

        public string C_jsgs { get; set; }

        public List<int> wl { get; set; }

        public int? WLCnt { get; set; }

        public int? RCnt { get; set; }

        public int? Square { get; set; }

        public DateTime? D_sj { get; set; }

        public string User { get; set; }
    }
}
