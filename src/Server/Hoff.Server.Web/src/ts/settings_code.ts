﻿
export class Settings {
    public wifiSettings: WifiSettings;
    constructor() {
        this.wifiSettings = new WifiSettings();
    }

    private UpdateSsids(): void {

        const options = (<HTMLSelectElement>document.getElementById('inputSSID')).options;

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


    public UpdateSettingsDisplay(message: string) {

        this.wifiSettings = JSON.parse(message);

        const pass = (<HTMLSelectElement>document.getElementById('inputPassword'));
        pass.value = this.wifiSettings.Password;
        this.UpdateSsids();

    }

    private AddSSID(ssid: string, active: string): void {
        const opt = document.createElement("option");

        opt.text = ssid;
        opt.value = ssid;

        if (ssid === active) {
            opt.selected = true;
        }

        (<HTMLSelectElement>document.getElementById('inputSSID')).options.add(opt);

    }
}


 class WifiSettings {
    public IsConfigured: boolean;
    public APsAvailable: APSAvailable[];
    public SSID: string;
    public Password: string;
    public AdHocEnabled: boolean;

    constructor() {
        this.IsConfigured = false;
        this.SSID = "";
        this.Password = "";
        this.AdHocEnabled = false;
        this.APsAvailable = [];
    }
}

class APSAvailable {
    public Bsid: string = "";
    public Ssid: string = "";
    public SignalBars: number = 0;
    public NetworkRssiInDecibelMilliwatts: number = 0;
    public NetworkKind: number = 0;
}