"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/generalHub")
    .build();

connection.on("ReceiveNotification", function (message, details) {
    toastr.info(details, message);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
