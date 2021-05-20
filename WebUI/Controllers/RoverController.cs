using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Parameters.Rover;
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
        public async Task<ActionResult<Rover>> Move([FromBody] MoveParams roverParams)
        {
            return Ok(await _roverRepository.MoveAsync(roverParams.Id, roverParams.Direction));
        }

        [HttpPut("change-direction")]
        public async Task<ActionResult<Rover>> ChangeDirection([FromBody] ChangeDirectionParams roverParams)
        {
            return Ok(await _roverRepository.ChangeDirectionAsync(roverParams.Id, roverParams.Direction));
        }
    }
}