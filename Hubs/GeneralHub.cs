using Microsoft.AspNetCore.SignalR;

namespace TaskManagementSystem.Hubs
{
    public class GeneralHub : Hub
    {
        public async Task SendAssignmentNotification(string userId, string title, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveAssignmentNotification", title, message);
        }

        public async Task SendStatusUpdate(string userId, int assignmentId, string status)
        {
            await Clients.User(userId).SendAsync("ReceiveStatusUpdate", assignmentId, status);
        }
    }
}
