namespace Domain.Entities
{
    public class Base<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public ushort PosX { get; set; }
        public ushort PosY { get; set; }
    }
}