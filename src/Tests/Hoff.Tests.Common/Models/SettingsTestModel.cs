
using Hoff.Tests.Common.Interfaces;

namespace Hoff.Tests.Common.Models
{
    public class SettingsTestModel : ISettingsTestModel
    {
        #region Properties
        public SettingsTestModel()
        {
            this.MachineName = string.Empty;
        }
        public string MachineName { get; set; }

        #endregion Properties
    }
}
