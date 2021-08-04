using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class Sample
    {
        public Decimal[] Avalue { get; set; }

        public Decimal? XGD { get; set; }

        public Decimal? ND { get; set; }

        public DateTime? D_sj { get; set; }

        public string C_bz { get; set; }

        public bool IsExclude { get; set; }
    }
}
