namespace Domain.Entities
{
    public class Rover : Base<int>
    {
        public char Direction { get; set; }
        public int PlanetId { get; set; }
    }
}