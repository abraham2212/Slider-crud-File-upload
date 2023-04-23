using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;
using Practice.Services.Interfaces;

namespace Practice.Services
{

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Product> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
