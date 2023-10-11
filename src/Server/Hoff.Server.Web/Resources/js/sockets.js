import { Settings } from "./settings_code";
const connection = new WebSocket("ws://" + location.hostname + ":80");
window.userSettings = new Settings(connection);
const sleep = (ms) => new Promise(r => setTimeout(r, ms));
connection.onmessage = function (evt) {
    const message = JSON.parse(evt.data);
    if (message.MessageType === "Hoff.Core.Services.WirelessConfig.Models.WifiSettings") {
        window.userSettings.UpdateSettingsDisplay(message.Message);
    }
    else if (message.MessageType === "UiMessage") {
        const div = document.getElementById('ui_message');
        div.innerHTML = message.Message;
    }
};
connection.onerror = function (err) {
    console.log(err);
};
connection.onopen = async function () {
    console.log("On open");
    await sleep(2000);
    this.send("GetWifiSettings");
};
