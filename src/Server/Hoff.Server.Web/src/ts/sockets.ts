﻿import { Settings } from "./settings_code"

const userSettings: Settings = new Settings();
const sleep = (ms) => new Promise(r => setTimeout(r, ms));
const connection = new WebSocket("ws://" + location.hostname + ":80");  
connection.onmessage = function (evt) {

    const message: WsMessage = JSON.parse(evt.data);
    if (message.MessageType === "Hoff.Server.Common.Models.WifiSettings") {

        userSettings.UpdateSettingsDisplay(message.Message);
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

export class WsMessage {
    Message: string;
    MessageType: string;
    constructor() {
        this.Message = "";
        this.MessageType = "";
    }
}
