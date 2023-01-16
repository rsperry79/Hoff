using System;

namespace Hoff.Hardware.Common.Helpers
{
    public static class NumberExtentionMethods
    {
        public static float Truncate(this float val, uint places = 3)
        {
            float temp = (float)(Math.Floor(val * Math.Pow(10, places)) / Math.Pow(10, places));

            return temp;
        }
    }
}
