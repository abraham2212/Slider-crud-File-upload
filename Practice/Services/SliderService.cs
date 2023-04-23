using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;
using Practice.Services.Interfaces;

namespace Practice.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        public SliderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Slider>> GetAll() => await _context.Sliders.ToListAsync();
        public async Task<SliderInfo> GetInfo() => await _context.SliderInfos.FirstOrDefaultAsync();
    }
}
