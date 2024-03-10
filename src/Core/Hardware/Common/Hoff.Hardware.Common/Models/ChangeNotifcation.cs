//using System;

//using Hoff.Core.Hardware.Common.Interfaces;

//namespace Hoff.Core.Hardware.Common.Models
//{
//    public abstract class ChangeNotification : IChangeNotification
//    {
//        #region Delegates

//        public delegate void SettingsChangedEventHandler(object sender, bool dataChanged);

//        #endregion Delegates

//        #region Events

//        // Event Handlers
//        public event EventHandler<bool> DataChanged;

//        #endregion Events

//        #region Private Methods
//        protected void SendEvent()
//        {
//            EventHandler<bool> tempEvent = DataChanged;
//            tempEvent(this, true);
//        }
//        #endregion Private Methods
//    }
//}
