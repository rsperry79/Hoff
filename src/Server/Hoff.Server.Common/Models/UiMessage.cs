using Hoff.Server.Common.Enums;

namespace Hoff.Server.Common.Models
{
    public class UiMessage
    {
        public bool IsVisible { get; set; } = true;
        public bool ShowAlert { get; set; } = false;

        public AlertLevel AlertLevel { get; set; } = AlertLevel.None;
        public string Message { get; set; }
        public UiMessage() { }

        public UiMessage(string message, bool showAlert = true, AlertLevel alertLevel = AlertLevel.None, bool isVisible = true) : this(showAlert, alertLevel, isVisible) => this.Message = message;

        public UiMessage(bool showAlert = true, AlertLevel alertLevel = AlertLevel.None, bool isVisible = true)
        {

            this.ShowAlert = showAlert;
            this.AlertLevel = alertLevel;
            this.IsVisible = isVisible;
        }
    }
}
