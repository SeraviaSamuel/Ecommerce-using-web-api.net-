using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Shipment>? shipments { get; set; }
        public ICollection<Payment>? payments { get; set; }
        public ICollection<WishList>? wishLists { get; set; }
        public ICollection<Cart>? carts { get; set; }
    }
}
