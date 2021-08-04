using System;
using System.Collections.Generic;

namespace UVStudio
{
    [Serializable]
    public class DNAMeasureData
    {
        public Decimal DNA { get; set; }

        public Decimal Protein { get; set; }

        public Decimal Ratio { get; set; }

        public Decimal ND { get; set; }

        public Decimal XGD { get; set; }

        public List<Decimal> A { get; set; }

        public Decimal ABack { get; set; }

        public string C_bz { get; set; }

        public DateTime D_sj { get; set; }
    }
}
