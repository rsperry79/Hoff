namespace Hoff.Hardware.Common.Interfaces.Base
{
    public interface ISensorBase
    {
        delegate void HasSensorValueChanged();

        bool CanTrackChanges();

        void BeginTrackChanges(int ms);

        void EndTrackChanges();
    }
}
