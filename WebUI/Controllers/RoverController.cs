using System.Collections.Generic;
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

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Rover>>> GetAll()
        {
            return Ok(await _roverRepository.GetAllRovers());
        }

        [HttpPut("move")]
        public async Task<ActionResult<Rover>> Move([FromBody] MoveParams roverParams)
        {
            return Ok(await _roverRepository.MoveAsync(roverParams));
        }
        
    }
}