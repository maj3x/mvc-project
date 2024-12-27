using Microsoft.AspNetCore.SignalR;

namespace TaskManagementSystem.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendAssignmentNotification(string userId, string title, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveAssignmentNotification", title, message);
        }

        public async Task SendStatusUpdate(string userId, string title, string status)
        {
            await Clients.User(userId).SendAsync("ReceiveStatusUpdate", title, status);
        }

        public async Task SendGlobalNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveGlobalNotification", message);
        }
    }
}
