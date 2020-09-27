using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IService<TDto>
    {
        Task<TDto> GetAsync(int id);
        Task<TDto> CreateAsync(TDto dto);
        Task UpdateAsync(int id, TDto dto);
        Task DeleteAsync(int id);
    }
}
