using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.Entities;
using Microsoft.Data.Sqlite;

namespace Infrastructure.SQL
{
    public class Seed
    {
        private static readonly Random _random = new Random();

        public static void InitializeDatabase(string connectionString)
        {
            ushort planetSize = (ushort) _random.Next(0, 5);
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            string initializeDbSqlCmd = File.ReadAllText("../Infrastructure/SQL/Queries/InitializeDB.sql");
            SqliteCommand cmd = new SqliteCommand(initializeDbSqlCmd, connection);
            cmd.ExecuteNonQuery();

            // Insert random obstacles
            List<Obstacle> obstacles = new List<Obstacle>();
            string insertObstaclesSqlCmd =
                File.ReadAllText("../Infrastructure/SQL/Queries/Obstacle/InsertObstacles.sql");
            for (short i = 0; i < planetSize; i++)
            {
                SqliteCommand insertObstacle = new SqliteCommand(insertObstaclesSqlCmd, connection);
                ushort randomPosX = (ushort) _random.Next(0, planetSize);
                ushort randomPosY = (ushort) _random.Next(0, planetSize);
                insertObstacle.Parameters.Add(new SqliteParameter
                    {ParameterName = "$posX", SqliteType = SqliteType.Integer, Value = randomPosX});
                insertObstacle.Parameters.Add(new SqliteParameter
                    {ParameterName = "$posY", SqliteType = SqliteType.Integer, Value = randomPosY});

                SqliteDataReader obstacleReader = insertObstacle.ExecuteReader();
                while (obstacleReader.Read())
                {
                    obstacles.Add(new Obstacle
                    {
                        Id = obstacleReader.GetInt32(0), PosX = (ushort) obstacleReader.GetInt16(1),
                        PosY = (ushort) obstacleReader.GetInt16(2)
                    });
                }
            }

            // Insert rover
            Rover rover = new Rover();
            ushort randomRoverPosX = (ushort) _random.Next(0, planetSize);
            ushort randomRoverPosY = (ushort) _random.Next(0, planetSize);

            while (obstacles.Select(x => x.PosY).Contains(randomRoverPosY))
            {
                randomRoverPosY = (ushort) _random.Next(0, planetSize);
            }

            string insertRoverSqlCmd = File.ReadAllText("../Infrastructure/SQL/Queries/Rover/InsertRover.sql");
            SqliteCommand insertRover = new SqliteCommand(insertRoverSqlCmd, connection);
            insertRover.Parameters.Add(new SqliteParameter
                {ParameterName = "$posX", SqliteType = SqliteType.Integer, Value = randomRoverPosX});
            insertRover.Parameters.Add(new SqliteParameter
                {ParameterName = "$posY", SqliteType = SqliteType.Integer, Value = randomRoverPosY});
            insertRover.Parameters.Add(new SqliteParameter
                {ParameterName = "$direction", SqliteType = SqliteType.Text, Value = 'E'});

            SqliteDataReader roverReader = insertRover.ExecuteReader();

            while (roverReader.Read())
            {
                rover = new Rover
                {
                    Id = roverReader.GetInt32(0),
                    PosX = (ushort) roverReader.GetInt16(1),
                    PosY = (ushort) roverReader.GetInt16(2),
                    Direction = roverReader.GetChar(3)
                };
            }

            // Insert Planet Mars
            string insertPlanetSqlCmd = File.ReadAllText("../Infrastructure/SQL/Queries/Planets/InsertPlanet.sql");
            SqliteCommand insertPlanet = new SqliteCommand(insertPlanetSqlCmd, connection);
            insertPlanet.Parameters.AddWithValue("$name", "Mars");
            insertPlanet.Parameters.AddWithValue("$rows", planetSize);
            insertPlanet.Parameters.AddWithValue("$columns", planetSize);
            insertPlanet.Parameters.AddWithValue("$roverId", rover.Id);
            insertPlanet.ExecuteNonQuery();
        }
    }
}