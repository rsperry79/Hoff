using System;

namespace Hoff.Hardware.Common.Interfaces
{
    public interface ITempatureSensor : ISensorBase, IDisposable
    {
        // Event Handlers
        delegate void TempatureChangedEventHandler();
        event TempatureChangedEventHandler TempatureSensorChanged;

        /// <summary>
        /// The current tempature
        /// </summary>
        UnitsNet.Temperature Temperature { get; }
    }
}
