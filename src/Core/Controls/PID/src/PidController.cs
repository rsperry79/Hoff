﻿//https://github.com/ms-iot/pid-controller/tree/master/PidController

using System;

namespace Hoff.Core.Controls.PID

{
    /// <summary>
    /// A (P)roportional, (I)ntegral, (D)erivative Controller
    /// </summary>
    /// <remarks>
    /// The controller should be able to control any process with a
    /// measurable value, a known ideal value and an input to the
    /// process that will affect the measured value.
    /// </remarks>
    /// <see cref="https://en.wikipedia.org/wiki/PID_controller"/>
    public sealed class PidController
    {
        #region Fields

        private double processVariable = 0;

        #endregion Fields

        #region Public Constructors

        public PidController(double GainProportional, double GainIntegral, double GainDerivative, double OutputMax, double OutputMin)
        {
            this.GainDerivative = GainDerivative;
            this.GainIntegral = GainIntegral;
            this.GainProportional = GainProportional;
            this.OutputMax = OutputMax;
            this.OutputMin = OutputMin;
        }

        #endregion Public Constructors

        #region Properties

        /// <summary>
        /// The derivative term is proportional to the rate of
        /// change of the error
        /// </summary>
        public double GainDerivative { get; set; } = 0;

        /// <summary>
        /// The integral term is proportional to both the magnitude
        /// of the error and the duration of the error
        /// </summary>
        public double GainIntegral { get; set; } = 0;

        /// <summary>
        /// The proportional term produces an output value that
        /// is proportional to the current error value
        /// </summary>
        /// <remarks>
        /// Tuning theory and industrial practice indicate that the
        /// proportional term should contribute the bulk of the output change.
        /// </remarks>
        public double GainProportional { get; set; } = 0;

        /// <summary>
        /// Adjustment made by considering the accumulated error over time
        /// </summary>
        /// <remarks>
        /// An alternative formulation of the integral action, is the
        /// proportional-summation-difference used in discrete-time systems
        /// </remarks>
        public double IntegralTerm { get; private set; } = 0;

        /// <summary>
        /// The max output value the control device can accept.
        /// </summary>
        public double OutputMax { get; private set; } = 0;

        /// <summary>
        /// The minimum output value the control device can accept.
        /// </summary>
        public double OutputMin { get; private set; } = 0;

        /// <summary>
        /// The current value
        /// </summary>
        public double ProcessVariable
        {
            get => this.processVariable;
            set
            {
                this.ProcessVariableLast = this.processVariable;
                this.processVariable = value;
            }
        }

        /// <summary>
        /// The last reported value (used to calculate the rate of change)
        /// </summary>
        public double ProcessVariableLast { get; private set; } = 0;

        /// <summary>
        /// The desired value
        /// </summary>
        public double SetPoint { get; set; } = 0;

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// The controller output
        /// </summary>
        /// <param name="timeSinceLastUpdate">timespan of the elapsed time
        /// since the previous time that ControlVariable was called</param>
        /// <returns>Value of the variable that needs to be controlled</returns>
        public double ControlVariable(TimeSpan timeSinceLastUpdate)
        {
            double error = this.SetPoint - this.ProcessVariable;

            // integral term calculation
            this.IntegralTerm += this.GainIntegral * error * timeSinceLastUpdate.TotalSeconds;
            this.IntegralTerm = this.Clamp(this.IntegralTerm);

            // derivative term calculation
            double dInput = this.processVariable - this.ProcessVariableLast;
            double derivativeTerm = this.GainDerivative * (dInput / timeSinceLastUpdate.TotalSeconds);

            // proportional term calculation
            double proportionalTerm = this.GainProportional * error;

            double output = proportionalTerm + this.IntegralTerm - derivativeTerm;

            return this.Clamp(output);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Limit a variable to the set OutputMax and OutputMin properties
        /// </summary>
        /// <returns>
        /// A value that is between the OutputMax and OutputMin properties
        /// </returns>
        /// <remarks>
        /// Inspiration from http://stackoverflow.com/questions/3176602/how-to-force-a-number-to-be-in-a-range-in-c
        /// </remarks>
        private double Clamp(double variableToClamp)
        {
            return variableToClamp <= this.OutputMin ? this.OutputMin : variableToClamp >= this.OutputMax ? this.OutputMax : variableToClamp;
        }

        #endregion Private Methods
    }
}
