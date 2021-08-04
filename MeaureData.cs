using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class MeaureData
    {
        public string xValue { get; set; }

        public float yABS { get; set; }

        public float YT { get; set; }

        public int? PV { get; set; }

        public string Rate { get; set; }

        public string PJ { get; set; }
    }
}
