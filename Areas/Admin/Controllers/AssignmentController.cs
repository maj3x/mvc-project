using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Repositories;

namespace OdevDagitimPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AssignmentController : Controller
    {
        private readonly IRepository<Assignment> _repository;

        public AssignmentController(IRepository<Assignment> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var assignments = _repository.GetAll();
            return View(assignments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                assignment.CreatedDate = DateTime.Now;
                assignment.Status = "Active";
                _repository.Insert(assignment);
                _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(assignment);
        }

        public IActionResult Edit(int id)
        {
            var assignment = _repository.GetById(id);
            if (assignment == null)
                return NotFound();
            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(assignment);
                _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(assignment);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                _repository.Save();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public IActionResult Details(int id)
        {
            var assignment = _repository.GetById(id);
            if (assignment == null)
                return NotFound();
            return View(assignment);
        }
    }
}