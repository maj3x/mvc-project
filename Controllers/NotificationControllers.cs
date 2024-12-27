using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManagementSystem.Hubs;
using Microsoft.AspNetCore.Identity;


namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;

        public NotificationController(
            IHubContext<NotificationHub> hubContext,
            UserManager<AppUser> userManager)
        {
            _hubContext = hubContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var user = await _userManager.GetUserAsync(User);
            // Burada veritabanından okunmamış bildirim sayısını alabilirsiniz
            return Json(5); // Örnek değer
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            // Bildirimi okundu olarak işaretle
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            // Tüm bildirimleri okundu olarak işaretle
            return Json(new { success = true });
        }
    }
}
