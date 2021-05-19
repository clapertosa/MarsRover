using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class PlanetController : BaseController
    {
        private readonly IPlanetRepository _planetRepository;

        public PlanetController(IPlanetRepository planetRepository)
        {
            _planetRepository = planetRepository;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Planet>>> GetAll()
        {
            return Ok(await _planetRepository.GetAllPlanetsAsync());
        }

        [HttpGet]
        public async Task<ActionResult<Planet>> GetPlanet(int id)
        {
            return Ok(await _planetRepository.GetPlanetInfoAsync(id));
        }
    }
}