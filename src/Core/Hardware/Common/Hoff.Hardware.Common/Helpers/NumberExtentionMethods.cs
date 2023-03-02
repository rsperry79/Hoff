using System;

namespace Hoff.Hardware.Common.Helpers
{
    public static class NumberExtentionMethods
    {
        public static double Truncate(this double val, uint places)
        {
            double temp = (Math.Floor(val * Math.Pow(10, places)) / Math.Pow(10, places));
            return temp;
        }
    }
}
