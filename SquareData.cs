using System;
using System.Drawing;

namespace UVStudio
{
    [Serializable]
    public class SquareData
    {
        public string BeginWL { get; set; }

        public string EndWL { get; set; }

        public string Square { get; set; }

        public float k { get; set; }

        public float b { get; set; }

        public Color sqColor { get; set; }
    }
}
