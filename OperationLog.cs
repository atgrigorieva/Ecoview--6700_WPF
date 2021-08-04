using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class OperationLog
    {
        public int LogID { get; set; }

        public int? LoginID { get; set; }

        public string C_name { get; set; }

        public string C_groups { get; set; }

        public DateTime? D_operatetime { get; set; }

        public string C_module { get; set; }

        public string C_Operation { get; set; }
    }
}
