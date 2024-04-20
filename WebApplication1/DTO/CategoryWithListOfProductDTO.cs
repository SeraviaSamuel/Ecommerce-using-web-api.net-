namespace WebApplication1.DTO
{
    public class CategoryWithListOfProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> ProductNames { get; set; }

    }
}
