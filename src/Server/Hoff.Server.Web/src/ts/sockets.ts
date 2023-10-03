const sleep = (ms) => new Promise(r => setTimeout(r, ms));

const connection = new WebSocket("ws://" + location.hostname + ":80");


class Settings {
    public wifiSettings: WifiSettings;
    constructor() {
        this.wifiSettings = new WifiSettings();
    }

    private UpdateSsids(): void {

        const options = (<HTMLSelectElement>document.getElementById('inputSSID')).options;
       
        for (let i = 0; i < options.length; i++) {
            options.remove(0);
        }

        let list = settings.wifiSettings.APsAvailable;

        list = list.sort(s => s.NetworkRssiInDecibelMilliwatts);
        list = list.reverse();

        for (let i = 0; i < list.length; i++) {
            this.AddSSID(list[i].Ssid, settings.wifiSettings.SSID);
        }

    }


    public UpdateSettingsDisplay() {
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

class APSAvailable {
    public Bsid: string = "";
    public Ssid: string = "";
    public SignalBars: number = 0;
    public NetworkRssiInDecibelMilliwatts: number = 0;
    public NetworkKind: number = 0;
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
class WsMessage {
    Message: string;
    MessageType: string;
    constructor() {
        this.Message = "";
        this.MessageType = "";
    }
}

const settings: Settings = new Settings();

connection.onmessage = function (evt) {

    const message: WsMessage = JSON.parse(evt.data);
    if (message.MessageType === "Hoff.Server.Common.Models.WifiSettings") {
        const payload: WifiSettings = JSON.parse(message.Message);
        settings.wifiSettings = payload;
        settings.UpdateSettingsDisplay();
    }
};

connection.onerror = function (err: Event) {
    console.log(err);
};

connection.onopen = async function () {
    console.log("On open");
    await sleep(2000);
    this.send("GetWifiSettings");
};
