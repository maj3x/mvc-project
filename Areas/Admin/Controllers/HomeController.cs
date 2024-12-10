using Microsoft.AspNetCore.Mvc;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Repositories;

namespace OdevDagitimPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IRepository<Assignment> _assignmentRepository;

        public HomeController(IRepository<Assignment> assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public IActionResult Index()
        {
            var assignments = _assignmentRepository.GetAll();
            return View(assignments);
        }
    }
}