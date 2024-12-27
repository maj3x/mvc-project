using Microsoft.AspNetCore.SignalR;
using TaskManagementSystem.Hubs;

namespace TaskManagementSystem.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotification(string userId, string title, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", title, message);
        }
    }
}
