using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModels;
using TaskManagementSystem.Repositories;
using AutoMapper;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace TaskManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;

        public CategoryController(
            CategoryRepository categoryRepository,
            IMapper mapper,
            INotyfService notyf)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryModels = _mapper.Map<IEnumerable<CategoryModel>>(categories);
            return View(categoryModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(model);
                await _categoryRepository.AddAsync(category);
                _notyf.Success("Kategori başarıyla eklendi");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _notyf.Error("Kategori bulunamadı");
                return RedirectToAction(nameof(Index));
            }
            var model = _mapper.Map<CategoryModel>(category);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetByIdAsync(model.Id);
                if (category == null)
                {
                    _notyf.Error("Kategori bulunamadı");
                    return RedirectToAction(nameof(Index));
                }

                category.Name = model.Name;
                category.Description = model.Description;
                await _categoryRepository.UpdateAsync(category);
                
                _notyf.Success("Kategori başarıyla güncellendi");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            _notyf.Success("Kategori başarıyla silindi");
            return RedirectToAction(nameof(Index));
        }
    }
}
