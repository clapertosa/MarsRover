using System;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class RoverRepository : IRoverRepository
    {
        private readonly IConfiguration _configuration;

        public RoverRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Rover> GetRoverInfoAsync()
        {
            Rover rover = new Rover();
            using (SqliteConnection connection = new SqliteConnection(_configuration["ConnectionString"]))
            {
                await connection.OpenAsync();
                SqliteCommand cmd = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = await File.ReadAllTextAsync("../Infrastructure/SQL/Queries/Rover/GetRoverInfo.sql")
                };

                SqliteDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    rover = new Rover
                    {
                        Id = reader.GetInt32(0),
                        PosX = (ushort) reader.GetInt16(1),
                        PosY = (ushort) reader.GetInt16(2),
                        Direction = reader.GetChar(3)
                    };
                }

                return rover;
            }
        }

        public async Task<Rover> MoveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<char> ChangeDirectionAsync()
        {
            throw new NotImplementedException();
        }
    }
}