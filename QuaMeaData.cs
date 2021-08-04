using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class QuaMeaData
    {
        public string Value { get; set; }

        public int Slot { get; set; }

        public int WL { get; set; }
    }
}
