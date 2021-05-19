using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class PlanetRepository : IPlanetRepository
    {
        private readonly IConfiguration _configuration;

        public PlanetRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Planet>> GetAllPlanetsAsync()
        {
            List<Planet> planets = new List<Planet>();

            await using SqliteConnection connection = new SqliteConnection(_configuration["ConnectionString"]);
            await connection.OpenAsync();

            SqliteCommand cmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Planet/GetPlanets.sql"),
            };

            SqliteDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                planets.Add(new Planet
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Rows = reader.GetInt16(2),
                    Columns = reader.GetInt16(3)
                });
            }

            return planets;
        }

        public async Task<Planet> GetPlanetInfoAsync(int id)
        {
            Planet planet = new Planet();

            await using SqliteConnection connection = new SqliteConnection(_configuration["ConnectionString"]);
            await connection.OpenAsync();

            SqliteCommand cmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Planet/GetPlanetInfo.sql"),
                Parameters =
                {
                    new SqliteParameter
                    {
                        ParameterName = "$id",
                        SqliteType = SqliteType.Integer,
                        Value = id
                    }
                }
            };


            SqliteDataReader reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows)
                throw new RestException(HttpStatusCode.NotFound, new {message = $"Planet with id {id} not found"});
            await reader.ReadAsync();

            planet = new Planet
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Rows =  reader.GetInt16(2),
                Columns =  reader.GetInt16(3),
            };

            return planet;
        }
    }
}