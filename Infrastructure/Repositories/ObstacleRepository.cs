using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class ObstacleRepository : IObstacleRepository
    {
        private readonly IConfiguration _configuration;

        public ObstacleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Obstacle>> GetAllObstaclesAsync()
        {
            List<Obstacle> obstacles = new List<Obstacle>();
            await using SqliteConnection connection = new SqliteConnection(_configuration["ConnectionString"]);
            await connection.OpenAsync();

            string sqlCmdString = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Obstacle/GetObstacles.sql");
            SqliteCommand cmd = new SqliteCommand(sqlCmdString, connection);
            SqliteDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                 int posX =reader.GetInt16(1);
                 int posY =reader.GetInt16(2);

                obstacles.Add(new Obstacle {Id = id, PosX = posX, PosY = posY});
            }

            return obstacles;
        }
    }
}