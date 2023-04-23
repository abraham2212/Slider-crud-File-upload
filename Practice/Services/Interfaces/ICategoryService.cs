using Practice.Models;

namespace Practice.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
