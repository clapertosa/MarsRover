using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoverRepository
    {
        Task<Rover> GetRoverInfoAsync();
        Task<Rover> MoveAsync();
        Task<char> ChangeDirectionAsync();
    }
}