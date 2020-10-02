using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverdoseTheGame
{
    public class PointRange
    {
        public double Min { get; set; }

        public double Max { get; set; }

        public PointRange(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public PointRange(PointRange range)
        {
            Min = range.Min;
            Max = range.Max;
        }
    }
}
