using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class RoverController : BaseController
    {
        private readonly IRoverRepository _roverRepository;

        public RoverController(IRoverRepository roverRepository)
        {
            _roverRepository = roverRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Rover>> GetRoverInfo(int id)
        {
            return Ok(await _roverRepository.GetRoverInfoAsync(id));
        }

        [HttpPut("move")]
        public async Task<ActionResult<Rover>> Move([FromForm] int id, [FromForm] char direction)
        {
            return Ok(await _roverRepository.MoveAsync(id, direction));
        }

        [HttpPut("change-direction")]
        public async Task<ActionResult<Rover>> ChangeDirection([FromForm] int id, [FromForm] char direction)
        {
            return Ok(await _roverRepository.ChangeDirectionAsync(id, direction));
        }
    }
}