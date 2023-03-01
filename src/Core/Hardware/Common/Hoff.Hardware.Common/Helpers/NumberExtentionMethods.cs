using System;

namespace Hoff.Hardware.Common.Helpers
{
    public static class NumberExtentionMethods
    {
        public static double Truncate(this double val, uint places)
        {
            float temp = (float)(Math.Floor(val * Math.Pow(10, places)) / Math.Pow(10, places));

            return temp;
        }
    }
}
