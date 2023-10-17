
using System.Text;

namespace Hoff.Core.Hardware.Common.Models
{
    public class SettingsStorage
    {
        #region Properties

        public string StorageLocation { get; set; }
        public string StorageDriver { get; set; }
        public string ConfigType { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"StorageLocation: {this.StorageLocation}");
            sb.AppendLine($"StorageDriver: {this.StorageDriver}");
            sb.AppendLine($"ConfigType: {this.ConfigType}");
            return sb.ToString();
        }

        #endregion Methods
    }
}
