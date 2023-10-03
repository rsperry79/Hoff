"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
const sleep = (ms) => new Promise(r => setTimeout(r, ms));
const connection = new WebSocket("ws://" + location.hostname + ":80");
class Settings {
    constructor() {
        this.wifiSettings = new WifiSettings();
    }
    UpdateSsids() {
        const options = document.getElementById('inputSSID').options;
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
    UpdateSettingsDisplay() {
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
class APSAvailable {
    constructor() {
        this.Bsid = "";
        this.Ssid = "";
        this.SignalBars = 0;
        this.NetworkRssiInDecibelMilliwatts = 0;
        this.NetworkKind = 0;
    }
}
class WifiSettings {
    constructor() {
        this.IsConfigured = false;
        this.SSID = "";
        this.Password = "";
        this.AdHocEnabled = false;
        this.APsAvailable = [];
    }
}
class WsMessage {
    constructor() {
        this.Message = "";
        this.MessageType = "";
    }
}
const settings = new Settings();
connection.onmessage = function (evt) {
    const message = JSON.parse(evt.data);
    if (message.MessageType === "Hoff.Server.Common.Models.WifiSettings") {
        const payload = JSON.parse(message.Message);
        settings.wifiSettings = payload;
        settings.UpdateSettingsDisplay();
    }
};
connection.onerror = function (err) {
    console.log(err);
};
connection.onopen = function () {
    return __awaiter(this, void 0, void 0, function* () {
        console.log("On open");
        yield sleep(2000);
        this.send("GetWifiSettings");
    });
};
$(() => {
});
