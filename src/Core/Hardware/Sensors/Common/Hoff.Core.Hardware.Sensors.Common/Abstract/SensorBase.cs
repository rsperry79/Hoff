using System;
using System.Threading;

using Hoff.Hardware.Common.Senors.Interfaces.Base;

using Microsoft.Extensions.Logging;

using static Hoff.Hardware.Common.Senors.Interfaces.Base.ISensorBase;

namespace Hoff.Core.Hardware.Senors.Common.Abstract
{
    /// <summary>
    /// This abstract class contains implementation, that is common to
    /// all sensors.Every new sensor driver implementation should
    /// inherit from this class
    /// </summary>
    public abstract class SensorBase : ISensorBase
    {
        #region Fields

        /// <summary>
        /// The thread that keeps a track of sensor value change
        /// </summary>
        protected Thread _changeTracker = null;

        /// <summary>
        /// Dispose support
        /// </summary>
        protected bool _disposed = false;

        /// <summary>
        /// Is this sensor tracking changes
        /// </summary>
        protected bool _isTrackingChanges = false;

        protected ILogger _logger;

        #endregion Fields

        // To detect redundant calls

        #region Public Methods

        /// <summary>
        /// Start to track the changes
        /// </summary>
        /// <param name="ms">Interval in milliseconds to track the changes to sensor values</param>
        public virtual void BeginTrackChanges(int ms)
        {
            try
            {
                if (this._isTrackingChanges)
                {
                    throw new InvalidOperationException("Already tracking changes");
                }

                if (ms < 50)
                {
                    throw new ArgumentOutOfRangeException("ms", "Minimum interval to track sensor changes is 50 milliseconds");
                }

                this._changeTracker = new Thread(() =>
                {
                    //  this.CheckForChanges(this.HasSensorValueChanged, ms);
                }); ;

                this._isTrackingChanges = true;
                this._changeTracker.Start();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Is this sensor capable of tracking changes
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool CanTrackChanges()
        {
            return false;
        }

        public void CheckForChanges(RefreshSenorData checkForChange, int ms)
        {
            int divs = ms / 1000;

            while (this._isTrackingChanges)
            {
                if (ms > 1000)
                {
                    while (this._isTrackingChanges && divs > 0)
                    {
                        Thread.Sleep(1000);
                        divs--;
                    }
                }
                else
                {
                    Thread.Sleep(ms);
                }

                //now check for checkForChange
                checkForChange();
            }
        }

        /// <summary>
        /// Stop tracking changes
        /// </summary>
        public virtual void EndTrackChanges()
        {
            this._isTrackingChanges = false;
            Thread.Sleep(3000);//see BeginChangeTracker to know why 3000 is chosen...3x of lowest wait time
            if (this._changeTracker.IsAlive)
            {
                //force kill
                try { this._changeTracker.Abort(); } finally { this._changeTracker = null; }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Dispose the sensor
        /// </summary>
        protected abstract void DisposeSensor();

        /// <summary>
        /// The sensor driver implementation should decide what is the meaning
        /// of change of a sensor value. We just check if the value of sensor has changed or not.
        /// Some sensor driver implementors may not want this automatic check and may want to have a
        /// polling mechanism for the client applications. The client apps,can use this method in their own
        /// polling implementation and check for value changes
        /// </summary>
        protected abstract void RefreshSenorData();

        #endregion Protected Methods

        /// <summary>
        /// Reset sensor
        /// </summary>
        //public abstract void Reset();

        //public abstract bool IsLastReadSuccessful();
    }
}
