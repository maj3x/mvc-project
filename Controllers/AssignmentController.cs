using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModels;
using TaskManagementSystem.Repositories;
using AutoMapper;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        private readonly AssignmentRepository _assignmentRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;

        public AssignmentController(
            AssignmentRepository assignmentRepository,
            CategoryRepository categoryRepository,
            UserManager<AppUser> userManager,
            IMapper mapper,
            INotyfService notyf)
        {
            _assignmentRepository = assignmentRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _mapper = mapper;
            _notyf = notyf;
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

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Students = await _userManager.GetUsersInRoleAsync("Student");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Add(AssignmentModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var assignment = _mapper.Map<Assignment>(model);
                assignment.AssignedById = currentUser.Id;
                assignment.Status = AssignmentStatus.Pending;

                await _assignmentRepository.AddAsync(assignment);
                _notyf.Success("Ödev başarıyla oluşturuldu");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Students = await _userManager.GetUsersInRoleAsync("Student");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var assignment = await _assignmentRepository.GetAssignmentWithDetailsAsync(id);
            if (assignment == null)
            {
                _notyf.Error("Ödev bulunamadı");
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var isTeacher = await _userManager.IsInRoleAsync(currentUser, "Teacher");

            if (!isTeacher && assignment.AssignedToId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var model = _mapper.Map<AssignmentModel>(assignment);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Update(int id)
        {
            var assignment = await _assignmentRepository.GetAssignmentWithDetailsAsync(id);
            if (assignment == null)
            {
                _notyf.Error("Ödev bulunamadı");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Students = await _userManager.GetUsersInRoleAsync("Student");
            var model = _mapper.Map<AssignmentModel>(assignment);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Update(AssignmentModel model)
        {
            if (ModelState.IsValid)
            {
                var assignment = await _assignmentRepository.GetByIdAsync(model.Id);
                if (assignment == null)
                {
                    _notyf.Error("Ödev bulunamadı");
                    return RedirectToAction(nameof(Index));
                }

                assignment.Title = model.Title;
                assignment.Description = model.Description;
                assignment.DueDate = model.DueDate;
                assignment.CategoryId = model.CategoryId;
                assignment.AssignedToId = model.AssignedToId;

                await _assignmentRepository.UpdateAsync(assignment);
                _notyf.Success("Ödev başarıyla güncellendi");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Students = await _userManager.GetUsersInRoleAsync("Student");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, AssignmentStatus status)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null)
            {
                return Json(new { success = false, message = "Ödev bulunamadı" });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var isTeacher = await _userManager.IsInRoleAsync(currentUser, "Teacher");

            if (!isTeacher && assignment.AssignedToId != currentUser.Id)
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok" });
            }

            assignment.Status = status;
            await _assignmentRepository.UpdateAsync(assignment);

            return Json(new { success = true, message = "Ödev durumu güncellendi" });
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            await _assignmentRepository.DeleteAsync(id);
            _notyf.Success("Ödev başarıyla silindi");
            return RedirectToAction(nameof(Index));
        }
    }
}
