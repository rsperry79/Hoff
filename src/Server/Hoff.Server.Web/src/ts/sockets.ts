import { WsMessage } from "./WsMessage";
import { Settings } from "./settings_code"
const connection = new WebSocket("ws://" + location.hostname + ":80");
(window as any).userSettings  = new Settings(connection);
const sleep = (ms) => new Promise(r => setTimeout(r, ms));

connection.onmessage = function (evt) {

    const message: WsMessage = JSON.parse(evt.data);
    if (message.MessageType === "Hoff.Core.Services.WirelessConfig.Models.WifiSetting") {

        (window as any).userSettings.UpdateSettingsDisplay(message.Message);
    }
    else if (message.MessageType === "UiMessage") {
        const div = <HTMLDivElement>document.getElementById('ui_message');
        div.innerHTML = message.Message;
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

