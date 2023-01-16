

using Hoff.Hardware.Common.Interfaces;
using System;
using System.Threading;

namespace Hoff.Hardware.Common.Abstract
{
    /// <summary>
    /// This abstract class contains implementation, that is common to
    /// all sensors.Every new sensor driver implementation should
    /// inherit from this class
    /// </summary>
    public abstract class SensorBase : ISensorBase
    {
        #region Implementation
        /// <summary>
        /// Is this sensor tracking changes
        /// </summary>
        protected bool _isTrackingChanges = false;
        /// <summary>
        /// The thread that keeps a track of sensor value change
        /// </summary>
        protected Thread _changeTracker = null;

        /// <summary>
        /// Dispose support
        /// </summary>
        protected bool _disposed = false; // To detect redundant calls
        #endregion

        #region IDispose Implementation
        /// <summary>
        /// Dispose the sensor
        /// </summary>
        protected abstract void DisposeSensor();
        #endregion

        #region Change tracking
        /// <summary>
        /// Is this sensor capable of tracking changes
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool CanTrackChanges()
        {
            return false;
        }

        /// <summary>
        /// Start to track the changes
        /// </summary>
        /// <param name="ms">Interval in milliseconds to track the changes to sensor values</param>
        public virtual void BeginTrackChanges(int ms)
        {
            if (_isTrackingChanges)
            {
                throw new InvalidOperationException("Already tracking changes");
            }

            if (ms < 50)
            {
                throw new ArgumentOutOfRangeException("ms", "Minimum interval to track sensor changes is 50 milliseconds");
            }

            _changeTracker = new Thread(() =>
            {
                CheckForChanges(HasSensorValueChanged, ms);
            });

            _isTrackingChanges = true;
            _changeTracker.Start();
        }


        public void CheckForChanges(ISensorBase.HasSensorValueChanged checkForChange, int ms)
        {

            int divs = ms / 1000;

            while (_isTrackingChanges)
            {
                if (ms > 1000)
                {
                    while (_isTrackingChanges && divs > 0)
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
        /// The sensor driver implementation should decide what is the meaning
        /// of change of a sensor value. We just check if the value of sensor has changed or not.
        /// Some sensor driver implementors may not want this automatic check and may want to have a
        /// polling mechanism for the client applications. The client apps,can use this method in their own
        /// polling implementation and check for value changes
        /// </summary>
        /// <returns>If true, then sensor value has changed else false</returns>
        public abstract void HasSensorValueChanged();


        /// <summary>
        /// Stop tracking changes
        /// </summary>
        public virtual void EndTrackChanges()
        {
            _isTrackingChanges = false;
            Thread.Sleep(3000);//see BeginChangeTracker to know why 3000 is chosen...3x of lowest wait time
            if (_changeTracker.IsAlive)
            {
                //force kill
                try { _changeTracker.Abort(); } finally { _changeTracker = null; }
            }
        }
        #endregion


        #region Abstract Sensor Methods
        /// <summary>
        /// Reset sensor
        /// </summary>
        //public abstract void Reset();

        //public abstract bool IsLastReadSuccessful();
        #endregion

    }
}