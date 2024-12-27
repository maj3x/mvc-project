$(document).ready(function () {
    initializeCharts();
    setupRealTimeUpdates();
});

function initializeCharts() {
    // Admin dashboard için gerekli grafikleri başlat
    var assignmentStats = new Chart($("#assignmentStatsChart"), {
        type: 'bar',
        data: {
            labels: ['Bekleyen', 'Devam Eden', 'Tamamlanan', 'Geciken'],
            datasets: [{
                data: [10, 5, 15, 3],
                backgroundColor: ['#ffc107', '#0d6efd', '#198754', '#dc3545']
            }]
        }
    });
}

function setupRealTimeUpdates() {
    // SignalR bağlantısını kur
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/generalHub")
        .build();

    connection.start();
}
