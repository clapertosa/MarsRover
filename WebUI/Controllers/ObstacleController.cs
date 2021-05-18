using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class ObstacleController : BaseController
    {
        private readonly IObstacleRepository _obstacleRepository;

        public ObstacleController(IObstacleRepository obstacleRepository)
        {
            _obstacleRepository = obstacleRepository;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Obstacle>>> GetAll()
        {
            return Ok(await _obstacleRepository.GetAllObstaclesAsync());
        }
    }
}