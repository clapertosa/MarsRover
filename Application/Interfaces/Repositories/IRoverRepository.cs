using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoverRepository
    {
        /// <summary>
        /// Get rover's info
        /// </summary>
        /// <param name="id">Rover's Id (int)</param>
        /// <returns>Rover's position and direction</returns>
        Task<Rover> GetRoverInfoAsync(int id);

        /// <summary>
        /// Moves Rover Forward (f) or Backward (b)
        /// </summary>
        /// <param name="id">Rover's id</param>
        /// <param name="direction">Char of type 'f' or 'b'</param>
        /// <returns>Rover's position and direction</returns>
        Task<Rover> MoveAsync(int id, char direction);

        /// <summary>
        /// Turns Rover Left (l) or Right(r)
        /// </summary>
        /// /// <param name="id">Rover's id</param>
        /// <param name="direction">Char of type 'l' or 'r'</param>
        /// <returns>Rover's position and direction</returns>
        Task<Rover> ChangeDirectionAsync(int id, char direction);
    }
}