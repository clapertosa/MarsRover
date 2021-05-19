namespace Domain.Entities
{
    public class Planet : Base<int>
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}