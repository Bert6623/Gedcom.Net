using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftWx.Dna {
    public struct MatchWeight {
        private int majorWeight;
        private int minorWeight;
        public MatchWeight(int majorWeight, int minorWeight) {
            this.majorWeight = majorWeight;
            this.minorWeight = minorWeight;
        }
        public int Major { get { return this.majorWeight; } }
        public int Minor { get { return this.minorWeight; } }
    }
}
