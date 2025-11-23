using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Core.Entities;
using OnlineShopping.Services.Interfaces;

namespace OnlineShopping.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/category/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _service.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _service.CreateAsync(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        // PUT: api/category/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest("Id mismatch");

            if (!await _service.ExistsAsync(id))
                return NotFound();

            await _service.UpdateAsync(category);
            return NoContent();
        }

        // DELETE: api/category/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.ExistsAsync(id))
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
