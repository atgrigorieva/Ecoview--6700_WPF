﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    [Serializable]
    public class PrintParams
    {
        public bool ShowComImage { get; set; }

        public string ComImage { get; set; }

        public string Title { get; set; }

        public bool ShowAddtional { get; set; }

        public string Addtional { get; set; }

        public bool ShowDes { get; set; }

        public string Describtion { get; set; }

        public bool ShowInsAndUser { get; set; }

        public bool ShowMeasure { get; set; }

        public bool ShowCurve { get; set; }

        public bool ShowAllData { get; set; }

        public bool ShowPeak { get; set; }

        public bool ShowValley { get; set; }
    }
}
