using Practice.Models;

namespace Practice.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetById(int id);
    }
}
