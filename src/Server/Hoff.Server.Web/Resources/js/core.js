console.log("Test");
alert("Alert");
var ws = new WebSocket("ws://" + location.hostname + ":80");
ws.binaryType = "arraybuffer";
ws.onopen = function () {
    //ws.send(""Hello"");
};
ws.onmessage = function (evt) {
    console.log(evt.data);
};
$(function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });
});
//# sourceMappingURL=core.js.map