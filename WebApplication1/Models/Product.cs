using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("category")]
        public int? CategoryId { get; set; }
        public Category category { get; set; }
        public ICollection<WishList>? wishLists { get; set; }
        public ICollection<Cart>? carts { get; set; }
    }
}
