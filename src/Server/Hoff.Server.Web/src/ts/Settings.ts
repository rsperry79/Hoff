console.log("Test");
alert("Alert");

const ws = new WebSocket("ws://" + location.hostname + ":80");
ws.binaryType = "arraybuffer";
ws.onopen = function () {
    //ws.send(""Hello"");
};

ws.onmessage = function (evt) {
    console.log(evt.data);
};