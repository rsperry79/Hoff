

using System;

namespace Hoff.Core.Hardware.Common.Helpers
{
    public static class Calibration
    {
        /// <summary>
        /// Apply calibration to the measured value..using 2-point calibration method
        /// Apply calibration formula to readings...see https://learn.adafruit.com/calibrating-sensors/two-point-calibration
        /// </summary>
        /// <param name="rawLow">Raw value low</param>
        /// <param name="rawRange">Raw value range</param>
        /// <param name="refLow">Reference low</param>
        /// <param name="refRange">Reference range</param>
        /// <param name="measuredVal">Value to calibrate</param>
        /// <returns>Calibrated value</returns>
        public static float Apply2PointCalibration(float rawLow, float rawRange, float refLow, float refRange, float measuredVal)
        {
            return refRange == 0
                ? throw new ArgumentException()
                : rawRange == 0 ? throw new ArgumentException() : ((measuredVal - rawLow) * refRange / rawRange) + refLow;
        }
    }
}
