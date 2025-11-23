using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopping.Services.Interfaces;
using OnlineShopping.Core.Entities;

namespace OnlineShopping.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService service, ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAllAsync();
            return View(products);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ProductDashboard(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return View(await _service.GetAllAsync());

            return View(await _service.SearchAsync(title));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _service.GetByIdAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] =
                new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile Photo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] =
                    new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
                return View(product);
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImages");
            Directory.CreateDirectory(path);

            string filename = Photo.FileName;
            using var fs = new FileStream(Path.Combine(path, filename), FileMode.Create);
            await Photo.CopyToAsync(fs);

            product.ProductPhoto = "ProductImages/" + filename;

            await _service.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewData["CategoryId"] =
                new SelectList(await _categoryService.GetAllAsync(), "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile EditPhoto)
        {
            if (id != product.Id) return NotFound();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImages");

            string filename = EditPhoto.FileName;

            using (var fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                await EditPhoto.CopyToAsync(fs);
            }

            product.ProductPhoto = "ProductImages/" + filename;

            await _service.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
