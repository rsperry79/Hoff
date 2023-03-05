namespace Hoff.Hardware.Common.Interfaces.Base
{
    public interface ISensorBase
    {
        #region Delegates

        delegate void RefreshSenorData();

        #endregion Delegates

        #region Public Methods

        void BeginTrackChanges(int ms);

        bool CanTrackChanges();

        void EndTrackChanges();

        #endregion Public Methods
    }
}