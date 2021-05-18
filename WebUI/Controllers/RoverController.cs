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
        public async Task<ActionResult<Rover>> GetRoverInfo(string id)
        {
            return Ok(await _roverRepository.GetRoverInfoAsync());
        }
    }
}