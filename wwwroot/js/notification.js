const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect()
    .build();

notificationConnection.on("ReceiveAssignmentNotification", (title, message) => {
    toastr.info(message, title);
    updateNotificationBadge();
});

notificationConnection.on("ReceiveStatusUpdate", (title, status) => {
    toastr.info(`${title} durumu ${status} olarak güncellendi`);
    updateNotificationBadge();
});

notificationConnection.on("ReceiveGlobalNotification", (message) => {
    toastr.info(message);
});

function updateNotificationBadge() {
    $.get('/Notification/GetUnreadCount', function(count) {
        const badge = $('#notificationBadge');
        badge.text(count);
        badge.toggle(count > 0);
    });
}

notificationConnection.start();
