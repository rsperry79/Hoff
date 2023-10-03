export class Settings {
    wifiSettings;
    constructor() {
        this.wifiSettings = new WifiSettings();
    }
    UpdateSsids() {
        const options = document.getElementById('inputSSID').options;
        for (let i = 0; i < options.length; i++) {
            options.remove(0);
        }
        let list = this.wifiSettings.APsAvailable;
        list = list.sort(s => s.NetworkRssiInDecibelMilliwatts);
        list = list.reverse();
        for (let i = 0; i < list.length; i++) {
            this.AddSSID(list[i].Ssid, this.wifiSettings.SSID);
        }
    }
    UpdateSettingsDisplay(message) {
        this.wifiSettings = JSON.parse(message);
        const pass = document.getElementById('inputPassword');
        pass.value = this.wifiSettings.Password;
        this.UpdateSsids();
    }
    AddSSID(ssid, active) {
        const opt = document.createElement("option");
        opt.text = ssid;
        opt.value = ssid;
        if (ssid === active) {
            opt.selected = true;
        }
        document.getElementById('inputSSID').options.add(opt);
    }
}
class WifiSettings {
    IsConfigured;
    APsAvailable;
    SSID;
    Password;
    AdHocEnabled;
    constructor() {
        this.IsConfigured = false;
        this.SSID = "";
        this.Password = "";
        this.AdHocEnabled = false;
        this.APsAvailable = [];
    }
}
class APSAvailable {
    Bsid = "";
    Ssid = "";
    SignalBars = 0;
    NetworkRssiInDecibelMilliwatts = 0;
    NetworkKind = 0;
}
