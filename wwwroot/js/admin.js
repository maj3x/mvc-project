$(document).ready(function () {
    initializeDatatables();
    setupRealTimeUpdates();
    initializeCharts();
});

function initializeDatatables() {
    $('.datatable').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json'
        }
    });
}

function setupRealTimeUpdates() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/generalHub")
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveNotification", (title, message) => {
        toastr.info(message, title);
    });

    connection.start();
}

function initializeCharts() {
    if (document.getElementById('assignmentChart')) {
        new Chart(document.getElementById('assignmentChart'), {
            type: 'pie',
            data: {
                labels: ['Bekleyen', 'Devam Eden', 'Tamamlanan', 'Geciken'],
                datasets: [{
                    data: assignmentStats,
                    backgroundColor: ['#ffc107', '#0d6efd', '#198754', '#dc3545']
                }]
            }
        });
    }
}
