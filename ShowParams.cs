using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class ShowParams
    {
        public bool AutoXY { get; set; }

        public float xMax { get; set; }

        public float xMin { get; set; }

        public float yMax { get; set; }

        public float yMin { get; set; }

        public int MulShow { get; set; }

        public bool AutoPrint { get; set; }

        public bool AutoSave { get; set; }
    }
}
