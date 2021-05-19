namespace Domain.Entities
{
    public class Base<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}