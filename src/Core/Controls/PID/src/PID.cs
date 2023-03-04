//https://github.com/tommallama/CSharp-PID

using System;

namespace Hoff.Core.Controls.PID
{
    public class PID
    {
        private double Ts;                  // Sample period in seconds
        private double K;                   // Roll-up parameter
        private double b0, b1, b2;          // Roll-up parameters
        private double a0, a1, a2;          // Roll-up parameters
        private double y0 = 0;              // Current output
        private double y1 = 0;              // Output one iteration old
        private double y2 = 0;              // Output two iterations old
        private double e0 = 0;              // Current error
        private double e1 = 0;              // Error one iteration old
        private double e2 = 0;              // Error two iterations old

        /// <summary>
        /// PID Constructor
        /// </summary>
        /// <param name="Kp">Proportional Gain</param>
        /// <param name="Ki">Integral Gain</param>
        /// <param name="Kd">Derivative Gain</param>
        /// <param name="N">Derivative Filter Coefficient</param>
        /// <param name="OutputUpperLimit">Controller Upper Output Limit</param>
        /// <param name="OutputLowerLimit">Controller Lower Output Limit</param>
        public PID(double Kp, double Ki, double Kd, double N, double OutputUpperLimit, double OutputLowerLimit)
        {
            this.Kp = Kp;
            this.Ki = Ki;
            this.Kd = Kd;
            this.N = N;
            this.OutputUpperLimit = OutputUpperLimit;
            this.OutputLowerLimit = OutputLowerLimit;
        }

        /// <summary>
        /// PID iterator, call this function every sample period to get the current controller output.
        /// set-point and processValue should use the same units.
        /// </summary>
        /// <param name="setPoint">Current Desired Set-point</param>
        /// <param name="processValue">Current Process Value</param>
        /// <param name="ts">TimeSpan <see cref="TimeSpan"/> Since Last Iteration, Use Default Sample Period for First Call</param>
        /// <returns>Current Controller Output</returns>
        public double PID_iterate(double setPoint, double processValue, TimeSpan ts)
        {
            // Ensure the Timespan is not too small or zero.
            this.Ts = (ts.TotalSeconds >= this.TsMin) ? ts.TotalSeconds : this.TsMin;

            // Calculate roll-up parameters
            this.K = 2 / this.Ts;
            this.b0 = (Math.Pow(this.K, 2) * this.Kp) + (this.K * this.Ki) + (this.Ki * this.N) + (this.K * this.Kp * this.N) + (Math.Pow(this.K, 2) * this.Kd * this.N);
            this.b1 = (2 * this.Ki * this.N) - (2 * Math.Pow(this.K, 2) * this.Kp) - (2 * Math.Pow(this.K, 2) * this.Kd * this.N);
            this.b2 = (Math.Pow(this.K, 2) * this.Kp) - (this.K * this.Ki) + (this.Ki * this.N) - (this.K * this.Kp * this.N) + (Math.Pow(this.K, 2) * this.Kd * this.N);
            this.a0 = Math.Pow(this.K, 2) + (this.N * this.K);
            this.a1 = -2 * Math.Pow(this.K, 2);
            this.a2 = Math.Pow(this.K, 2) - (this.K * this.N);

            // Age errors and output history
            this.e2 = this.e1;                        // Age errors one iteration
            this.e1 = this.e0;                        // Age errors one iteration
            this.e0 = setPoint - processValue;   // Compute new error
            this.y2 = this.y1;                        // Age outputs one iteration
            this.y1 = this.y0;                        // Age outputs one iteration
            this.y0 = (-this.a1 / this.a0 * this.y1) - (this.a2 / this.a0 * this.y2) + (this.b0 / this.a0 * this.e0) + (this.b1 / this.a0 * this.e1) + (this.b2 / this.a0 * this.e2); // Calculate current output

            // Clamp output if needed
            if (this.y0 > this.OutputUpperLimit)
            {
                this.y0 = this.OutputUpperLimit;
            }
            else if (this.y0 < this.OutputLowerLimit)
            {
                this.y0 = this.OutputLowerLimit;
            }

            return this.y0;
        }

        /// <summary>
        /// Reset controller history effectively resetting the controller.
        /// </summary>
        public void ResetController()
        {
            this.e2 = 0;
            this.e1 = 0;
            this.e0 = 0;
            this.y2 = 0;
            this.y1 = 0;
            this.y0 = 0;
        }

        /// <summary>
        /// Proportional Gain, consider resetting controller if this parameter is drastically changed.
        /// </summary>
        public double Kp { get; set; }

        /// <summary>
        /// Integral Gain, consider resetting controller if this parameter is drastically changed.
        /// </summary>
        public double Ki { get; set; }

        /// <summary>
        /// Derivative Gain, consider resetting controller if this parameter is drastically changed.
        /// </summary>
        public double Kd { get; set; }

        /// <summary>
        /// Derivative filter coefficient.
        /// A smaller N for more filtering.
        /// A larger N for less filtering.
        /// Consider resetting controller if this parameter is drastically changed.
        /// </summary>
        public double N { get; set; }

        /// <summary>
        /// Minimum allowed sample period to avoid dividing by zero!
        /// The Ts value can be mistakenly set to too low of a value or zero on the first iteration.
        /// TsMin by default is set to 1 millisecond.
        /// </summary>
        public double TsMin { get; set; } = 0.001;

        /// <summary>
        /// Upper output limit of the controller.
        /// This should obviously be a numerically greater value than the lower output limit.
        /// </summary>
        public double OutputUpperLimit { get; set; }

        /// <summary>
        /// Lower output limit of the controller
        /// This should obviously be a numerically lesser value than the upper output limit.
        /// </summary>
        public double OutputLowerLimit { get; set; }
    }
}

