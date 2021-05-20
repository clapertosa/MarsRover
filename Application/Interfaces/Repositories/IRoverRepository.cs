using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Parameters.Rover;

namespace Application.Interfaces.Repositories
{
    public interface IRoverRepository
    {
        /// <summary>
        /// Returns all rovers
        /// </summary>
        Task<IEnumerable<Rover>> GetAllRovers();

        /// <summary>
        /// Get rover's info
        /// </summary>
        /// <param name="id">Rover's Id (int)</param>
        /// <returns>Rover's position and direction</returns>
        Task<Rover> GetRoverInfoAsync(int id);

        /// <summary>
        /// Moves Rover Forward (f) or Backward (b)
        /// </summary>
        /// <param name="moveParams">Rover's id and an array of chars 'f', 'b', 'l' and 'r'</param>
        /// <returns>Rover's position and direction</returns>
        Task<Rover> MoveAsync(MoveParams moveParams);
    }
}