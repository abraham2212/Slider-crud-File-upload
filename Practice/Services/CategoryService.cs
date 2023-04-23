using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;
using Practice.Services.Interfaces;

namespace Practice.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAll() => await _context.Categories.ToListAsync();
    }
}
