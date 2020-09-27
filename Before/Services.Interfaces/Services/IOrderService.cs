
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService : IService<OrderDto>
    {
        Task<string> GetOrderStatusAsync(int id);
    }
}