using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;
using Practice.Services.Interfaces;

namespace Practice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public CategoryController(ICategoryService categoryService, AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _categoryService.GetAll());

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var dbCategory = await _context.Categories
                                  .FirstOrDefaultAsync(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower());

                if (dbCategory is not null)
                {
                    return RedirectToAction(nameof(Index));
                }
                //int a = 2;
                //int b = 0;
                //int res = a / b;
                //throw new Exception("jdjj");
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }

           
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id is null) return BadRequest();
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbCategory is null) return NotFound();
            
            return View(dbCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (id is null) return BadRequest();
                var dbCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (dbCategory is null) return NotFound();

                if(dbCategory.Name.Trim().ToLower() == category.Name.Trim().ToLower())
                {
                    return RedirectToAction(nameof(Index));
                }

                //dbCategory.Name = category.Name;
                 _context.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbCategory is null) return NotFound();

            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id is null) return BadRequest();
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbCategory is null) return NotFound();

            dbCategory.SoftDelete = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbCategory is null) return NotFound();

            return View(dbCategory);
        }
    }
}
