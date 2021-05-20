using Application.Constants;
using Domain.Entities;

namespace Infrastructure.Services
{
    public static class RoverService
    {
        class Coordinates
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
        }

        private static Coordinates CalculateCoordinates(Rover rover, int direction, int planetSize)
        {
            Coordinates coordinates = new Coordinates();
            int invert =
                rover.Direction == RoverCardinalPoints.West ||
                rover.Direction == RoverCardinalPoints.North
                    ? -1
                    : 1;
            if (rover.Direction == RoverCardinalPoints.East || rover.Direction == RoverCardinalPoints.West)
            {
                coordinates.PosY = rover.PosY;
                coordinates.PosX = rover.PosX + (direction * invert);

                if (coordinates.PosX < 0) coordinates.PosX = planetSize - 1;
                else if (coordinates.PosX > planetSize - 1) coordinates.PosX = 0;
            }
            else
            {
                coordinates.PosX = rover.PosX;
                coordinates.PosY = rover.PosY + (direction * invert);

                if (coordinates.PosY < 0) coordinates.PosY = planetSize - 1;
                else if (coordinates.PosY > planetSize - 1) coordinates.PosY = 0;
            }

            return coordinates;
        }

        public static Rover GetNewRoverPosition(Planet planet, Rover rover, char direction)
        {
            Rover newRover = new Rover
            {
                Id = rover.Id,
                PosX = rover.PosX,
                PosY = rover.PosY,
                Direction = rover.Direction,
                PlanetId = rover.PlanetId
            };
            int planetSize = planet.Size;
            Coordinates roverCoordinates = new Coordinates() {PosX = newRover.PosX, PosY = newRover.PosY};

            switch (direction)
            {
                case RoverDirections.Forward:
                {
                    roverCoordinates = CalculateCoordinates(newRover, 1, planetSize);
                    break;
                }
                case RoverDirections.Backward:
                {
                    roverCoordinates = CalculateCoordinates(newRover, -1, planetSize);
                    break;
                }
                default: return newRover;
            }

            newRover.PosX = roverCoordinates.PosX;
            newRover.PosY = roverCoordinates.PosY;

            return newRover;
        }

        public static char GetNewRoverDirection(Rover rover, char turn)
        {
            switch (turn)
            {
                case RoverDirections.Left:
                {
                    if (rover.Direction == RoverCardinalPoints.North) return RoverCardinalPoints.West;
                    if (rover.Direction == RoverCardinalPoints.East) return RoverCardinalPoints.North;
                    if (rover.Direction == RoverCardinalPoints.South) return RoverCardinalPoints.East;
                    if (rover.Direction == RoverCardinalPoints.West) return RoverCardinalPoints.South;
                }
                    break;
                case RoverDirections.Right:
                {
                    if (rover.Direction == RoverCardinalPoints.North) return RoverCardinalPoints.East;
                    if (rover.Direction == RoverCardinalPoints.East) return RoverCardinalPoints.South;
                    if (rover.Direction == RoverCardinalPoints.South) return RoverCardinalPoints.West;
                    if (rover.Direction == RoverCardinalPoints.West) return RoverCardinalPoints.North;
                }
                    break;
            }

            return rover.Direction;
        }
    }
}