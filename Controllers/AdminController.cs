using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Security.Claims;


namespace TaskManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly INotyfService _notyf;

        public AdminController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            INotyfService notyf)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userModels = new List<UserModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userModels.Add(new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList()
                });
            }

            return View(userModels);
        }

        public async Task<IActionResult> UserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notyf.Error("Kullanıcı bulunamadı");
                return RedirectToAction(nameof(Users));
            }

            var model = new UserRoleModel
            {
                UserId = userId,
                UserName = user.UserName
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            model.Roles = allRoles.Select(role => new RoleSelection
            {
                RoleName = role.Name,
                IsSelected = userRoles.Contains(role.Name)
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserRoles(UserRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                _notyf.Error("Kullanıcı bulunamadı");
                return RedirectToAction(nameof(Users));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.Roles.Where(x => x.IsSelected).Select(x => x.RoleName);
            var rolesToAdd = selectedRoles.Except(userRoles);
            var rolesToRemove = userRoles.Except(selectedRoles);

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            _notyf.Success("Kullanıcı rolleri güncellendi");
            return RedirectToAction(nameof(Users));
        }

        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                _notyf.Error("Rol adı boş olamaz");
                return RedirectToAction(nameof(Roles));
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                _notyf.Error("Bu rol zaten mevcut");
                return RedirectToAction(nameof(Roles));
            }

            var role = new AppRole
            {
                Name = roleName,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                _notyf.Success("Rol başarıyla oluşturuldu");
            }
            else
            {
                _notyf.Error("Rol oluşturulurken hata oluştu");
            }

            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                _notyf.Error("Rol bulunamadı");
                return RedirectToAction(nameof(Roles));
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _notyf.Success("Rol başarıyla silindi");
            }
            else
            {
                _notyf.Error("Rol silinirken hata oluştu");
            }

            return RedirectToAction(nameof(Roles));
        }
    }
}
