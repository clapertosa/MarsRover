namespace Domain.Entities
{
    public class Planet : Base<int>
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }
}