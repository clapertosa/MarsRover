using System.IO;
using System.Net;
using System.Threading.Tasks;
using Application.Constants;
using Application.Errors;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class RoverRepository : IRoverRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IPlanetRepository _planetRepository;
        private readonly IObstacleRepository _obstacleRepository;

        public RoverRepository(IConfiguration configuration, IPlanetRepository planetRepository,
            IObstacleRepository obstacleRepository)
        {
            _configuration = configuration;
            _planetRepository = planetRepository;
            _obstacleRepository = obstacleRepository;
        }

        public async Task<Rover> GetRoverInfoAsync(int id)
        {
            Rover rover = new Rover();
            await using SqliteConnection connection = new SqliteConnection(_configuration["ConnectionString"]);
            await connection.OpenAsync();
            SqliteCommand cmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/GetRoverInfo.sql"),
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
                throw new RestException(HttpStatusCode.NotFound, new {message = $"Rover with id {id} not found."});

            while (await reader.ReadAsync())
            {
                rover = new Rover
                {
                    Id = reader.GetInt32(0),
                    PosX = reader.GetInt16(1),
                    PosY = reader.GetInt16(2),
                    Direction = reader.GetChar(3),
                    PlanetId = reader.GetInt32(4)
                };
            }

            return rover;
        }

        public async Task<Rover> MoveAsync(int id, char direction)
        {
            char formattedDirection = direction.ToString().ToLower()[0];
            if (formattedDirection != RoverDirections.Forward && formattedDirection != RoverDirections.Backward)
                throw new RestException(HttpStatusCode.Forbidden, new {message = "Can only accept 'f' or 'b'"});

            Rover rover = new Rover();

            await using SqliteConnection connection = new SqliteConnection(_configuration["ConnectionString"]);
            await connection.OpenAsync();

            // Get Rover
            SqliteCommand roverCmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/GetRoverInfo.sql"),
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

            SqliteDataReader roverReader = await roverCmd.ExecuteReaderAsync();
            if (!roverReader.HasRows)
                throw new RestException(HttpStatusCode.NotFound, new {message = $"Rover with id {id} not found."});
            await roverReader.ReadAsync();
            rover = new Rover
            {
                Id = roverReader.GetInt32(0),
                PosX = roverReader.GetInt16(1),
                PosY = roverReader.GetInt16(2),
                Direction = roverReader.GetChar(3),
                PlanetId = roverReader.GetInt32(4)
            };

            // Get Planet
            Planet planet = await _planetRepository.GetPlanetInfoAsync(rover.PlanetId);

            Rover newRover = RoverService.GetNewRoverPosition(planet, rover, formattedDirection);

            // Check if there is an obstacle
            bool positionHasObstacle = await _obstacleRepository.PositionHasObstacle(newRover.PosX, newRover.PosY);

            if (positionHasObstacle) return rover;

            SqliteCommand updateRoverPositionCmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/UpdateRover.sql"),
                Parameters =
                {
                    new SqliteParameter
                    {
                        ParameterName = "$id",
                        SqliteType = SqliteType.Integer,
                        Value = id
                    },
                    new SqliteParameter
                    {
                        ParameterName = "$posX",
                        SqliteType = SqliteType.Integer,
                        Value = newRover.PosX
                    },
                    new SqliteParameter
                    {
                        ParameterName = "$posY",
                        SqliteType = SqliteType.Integer,
                        Value = newRover.PosY
                    },
                    new SqliteParameter
                    {
                        ParameterName = "$direction",
                        SqliteType = SqliteType.Text,
                        Value = newRover.Direction
                    }
                }
            };
            await updateRoverPositionCmd.ExecuteNonQueryAsync();
            return newRover;
        }

        public async Task<Rover> ChangeDirectionAsync(int id, char direction)
        {
            Rover rover = new Rover();

            await using SqliteConnection connection = new SqliteConnection(_configuration["ConnectionString"]);
            await connection.OpenAsync();

            // Get Rover
            SqliteCommand roverCmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/GetRoverInfo.sql"),
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

            SqliteDataReader roverReader = await roverCmd.ExecuteReaderAsync();
            if (!roverReader.HasRows)
                throw new RestException(HttpStatusCode.NotFound, new {message = $"Rover with id {id} not found."});
            await roverReader.ReadAsync();
            rover = new Rover
            {
                Id = roverReader.GetInt32(0),
                PosX = roverReader.GetInt16(1),
                PosY = roverReader.GetInt16(2),
                Direction = roverReader.GetChar(3),
                PlanetId = roverReader.GetInt32(4)
            };

            rover.Direction = RoverService.GetNewRoverDirection(rover, direction);

            SqliteCommand updateRoverPositionCmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/UpdateRover.sql"),
                Parameters =
                {
                    new SqliteParameter
                    {
                        ParameterName = "$id",
                        SqliteType = SqliteType.Integer,
                        Value = id
                    },
                    new SqliteParameter
                    {
                        ParameterName = "$posX",
                        SqliteType = SqliteType.Integer,
                        Value = rover.PosX
                    },
                    new SqliteParameter
                    {
                        ParameterName = "$posY",
                        SqliteType = SqliteType.Integer,
                        Value = rover.PosY
                    },
                    new SqliteParameter
                    {
                        ParameterName = "$direction",
                        SqliteType = SqliteType.Text,
                        Value = rover.Direction
                    }
                }
            };
            await updateRoverPositionCmd.ExecuteNonQueryAsync();
            return rover;
        }
    }
}