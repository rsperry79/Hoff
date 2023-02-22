using System;

using Hoff.Hardware.Common.Interfaces.Base;

namespace Hoff.Hardware.Common.Interfaces
{
    public interface ITempatureSensor : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate void TempatureChangedEventHandler();
        event TempatureChangedEventHandler TemperatureSensorChanged;

        /// <summary>
        /// The current tempature
        /// </summary>
        double Temperature { get; }
    }
}
