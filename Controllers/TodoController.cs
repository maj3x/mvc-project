using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModels;
using TaskManagementSystem.Repositories;
using TaskManagementSystem.Hubs;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly AssignmentRepository _assignmentRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IHubContext<GeneralHub> _hubContext;

        public TodoController(
            AssignmentRepository assignmentRepository,
            UserManager<AppUser> userManager,
            IMapper mapper,
            INotyfService notyf,
            IHubContext<GeneralHub> hubContext)
        {
            _assignmentRepository = assignmentRepository;
            _userManager = userManager;
            _mapper = mapper;
            _notyf = notyf;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isTeacher = await _userManager.IsInRoleAsync(currentUser, "Teacher");

            IEnumerable<Assignment> assignments;
            if (isTeacher)
            {
                assignments = await _assignmentRepository.GetAssignmentsWithDetailsAsync();
            }
            else
            {
                assignments = await _assignmentRepository.GetUserAssignmentsAsync(currentUser.Id);
            }

            var assignmentModels = _mapper.Map<IEnumerable<AssignmentModel>>(assignments);
            return View(assignmentModels);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, AssignmentStatus status)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var assignment = await _assignmentRepository.GetAssignmentWithDetailsAsync(id);

            if (assignment == null)
            {
                return Json(new { success = false, message = "Ödev bulunamadı" });
            }

            if (assignment.AssignedToId != currentUser.Id && !await _userManager.IsInRoleAsync(currentUser, "Teacher"))
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok" });
            }

            var oldStatus = assignment.Status;
            assignment.Status = status;
            await _assignmentRepository.UpdateAsync(assignment);

            // Öğretmene bildirim gönder
            if (assignment.AssignedById != null)
            {
                await _hubContext.Clients.User(assignment.AssignedById).SendAsync(
                    "ReceiveAssignmentNotification",
                    "Ödev Durumu Güncellendi",
                    $"{currentUser.FirstName} {currentUser.LastName} ödevinin durumunu {oldStatus} -> {status} olarak güncelledi."
                );
            }

            return Json(new { 
                success = true, 
                message = "Ödev durumu başarıyla güncellendi",
                newStatus = status.ToString() 
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoList()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var assignments = await _assignmentRepository.GetUserAssignmentsAsync(currentUser.Id);
            var todoList = assignments.Select(a => new
            {
                id = a.Id,
                title = a.Title,
                status = a.Status.ToString(),
                dueDate = a.DueDate.ToString("dd/MM/yyyy HH:mm"),
                isLate = a.DueDate < DateTime.Now && a.Status != AssignmentStatus.Completed,
                categoryName = a.Category.Name
            });

            return Json(todoList);
        }

        [HttpGet]
        public async Task<IActionResult> GetUpcomingDeadlines()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var assignments = await _assignmentRepository.GetUserAssignmentsAsync(currentUser.Id);
            var upcomingDeadlines = assignments
                .Where(a => a.DueDate > DateTime.Now && a.Status != AssignmentStatus.Completed)
                .OrderBy(a => a.DueDate)
                .Take(5)
                .Select(a => new
                {
                    id = a.Id,
                    title = a.Title,
                    dueDate = a.DueDate.ToString("dd/MM/yyyy HH:mm"),
                    isUrgent = a.DueDate < DateTime.Now.AddDays(1)
                });

            return Json(upcomingDeadlines);
        }
    }
}
