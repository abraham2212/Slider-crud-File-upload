using Practice.Models;

namespace Practice.Services.Interfaces
{
    public interface IExpertService
    {
        Task<IEnumerable<ExpertExpertPosition>> GetALL();
        Task<ExpertHeader> GetHeader();
    }
}
