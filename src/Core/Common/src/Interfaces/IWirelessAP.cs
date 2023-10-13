namespace Hoff.Core.Common.Interfaces
{
    public interface IWirelessAP
    {
        bool CheckForConnection(bool isEnabled);
        void Disable();
        void GetAvailableAPs();
        bool Setup();
    }
}