using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IObstacleRepository
    {
        Task<IEnumerable<Obstacle>> GetAllObstaclesAsync();
        Task<bool> PositionHasObstacle(int posX, int posY);
    }
}