using System.Threading.Tasks;

namespace ApplicationServices.Interfaces
{
    public interface IStatisticService
    {
        Task SaveAsync(string eventName);
    }
}
