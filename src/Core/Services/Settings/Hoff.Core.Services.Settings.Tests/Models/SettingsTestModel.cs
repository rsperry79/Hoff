using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Models;

namespace Hoff.Core.Services.Settings.Tests.Models
{
    public class SettingsTestModel : SettingsBase, IChangeNotifcation
    {
        #region Properties

        public string MachineName { get; set; }

        protected override void Initialize()
        {

        }

        #endregion Properties
    }
}
