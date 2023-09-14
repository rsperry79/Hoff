using System;

namespace Hoff.Core.Hardware.Common.Helpers
{
    public static class NumberExtentionMethods
    {
        #region Public Methods

        public static double Truncate(this double val, uint places)
        {
            double temp = (Math.Floor(val * Math.Pow(10, places)) / Math.Pow(10, places));
            return temp;
        }

        #endregion Public Methods
    }
}
