using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Application.Constants;
using Application.Errors;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Parameters.Rover;
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

        public async Task<IEnumerable<Rover>> GetAllRovers()
        {
            List<Rover> rovers = new List<Rover>();

            await using SqliteConnection connection =
                new SqliteConnection(_configuration.GetSection("ConnectionStrings:Database").Value);
            await connection.OpenAsync();
            SqliteCommand cmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/GetRovers.sql")
            };
            SqliteDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                rovers.Add(new Rover
                {
                    Id = reader.GetInt32(0),
                    PosX = reader.GetInt16(1),
                    PosY = reader.GetInt16(2),
                    Direction = reader.GetChar(3),
                    PlanetId = reader.GetInt32(4)
                });
            }

            return rovers;
        }

        public async Task<Rover> GetRoverInfoAsync(int id)
        {
            Rover rover = null;
            await using SqliteConnection connection =
                new SqliteConnection(_configuration.GetSection("ConnectionStrings:Database").Value);
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

        public async Task<Rover> MoveAsync(MoveParams moveParams)
        {
            // Get Rover
            Rover rover = await GetRoverInfoAsync(moveParams.Id);
            if (rover == null)
                throw new RestException(HttpStatusCode.NotFound,
                    new {message = $"Rover with id {moveParams.Id} not found."});

            await using SqliteConnection connection =
                new SqliteConnection(_configuration.GetSection("ConnectionStrings:Database").Value);
            await connection.OpenAsync();

            // Get Planet
            Planet planet = await _planetRepository.GetPlanetInfoAsync(rover.PlanetId);

            Rover newRover = new Rover
            {
                Id = rover.Id,
                PosX = rover.PosX,
                PosY = rover.PosY,
                Direction = rover.Direction,
                PlanetId = rover.PlanetId
            };

            SqliteCommand updateRoverPositionCmd = new SqliteCommand
            {
                Connection = connection,
                CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/UpdateRover.sql"),
            };

            foreach (char instruction in moveParams.Instructions)
            {
                int prevPosX = newRover.PosX;
                int prevPosY = newRover.PosY;
                newRover = RoverService.GetNewRoverPosition(planet, newRover, instruction);
                // Check if there is an obstacle
                if (instruction == RoverDirections.Forward || instruction == RoverDirections.Backward)
                {
                    bool positionHasObstacle =
                        await _obstacleRepository.PositionHasObstacle(newRover.PosX, newRover.PosY);

                    if (positionHasObstacle)
                    {
                        updateRoverPositionCmd.Parameters.AddRange(
                            new[]
                            {
                                new SqliteParameter
                                {
                                    ParameterName = "$id",
                                    SqliteType = SqliteType.Integer,
                                    Value = newRover.Id
                                },
                                new SqliteParameter
                                {
                                    ParameterName = "$posX",
                                    SqliteType = SqliteType.Integer,
                                    Value = prevPosX
                                },
                                new SqliteParameter
                                {
                                    ParameterName = "$posY",
                                    SqliteType = SqliteType.Integer,
                                    Value = prevPosY
                                },
                                new SqliteParameter
                                {
                                    ParameterName = "$direction",
                                    SqliteType = SqliteType.Text,
                                    Value = newRover.Direction
                                }
                            }
                        );
                        await updateRoverPositionCmd.ExecuteNonQueryAsync();
                        throw new RestException(HttpStatusCode.Forbidden,
                            new
                            {
                                obstacle = new Obstacle
                                {
                                    PosX = newRover.PosX,
                                    PosY = newRover.PosY
                                },
                                rover =
                                    new Rover
                                    {
                                        Id = newRover.Id,
                                        PosX = prevPosX,
                                        PosY = prevPosY,
                                        Direction = newRover.Direction,
                                        PlanetId = newRover.PlanetId
                                    }
                            });
                    }
                }
            }

            updateRoverPositionCmd.Parameters.AddRange(
                new[]
                {
                    new SqliteParameter
                    {
                        ParameterName = "$id",
                        SqliteType = SqliteType.Integer,
                        Value = newRover.Id
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
            );

            await updateRoverPositionCmd.ExecuteNonQueryAsync();
            return newRover;
        }
    }
}