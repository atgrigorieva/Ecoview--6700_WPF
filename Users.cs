using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class Users
    {
        public string C_name { get; set; }

        public string C_fullname { get; set; }

        public string C_pwd { get; set; }

        public string C_groups { get; set; }

        public DateTime? D_createtime { get; set; }

        public DateTime? D_lastlogintime { get; set; }

        public int? I_logincnt { get; set; }

        public int? I_glp { get; set; }

        public string C_desc { get; set; }
    }
}


