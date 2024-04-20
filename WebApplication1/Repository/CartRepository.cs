using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CartRepository : ICartRepository
    {
        Context context;
        public CartRepository(Context _context)
        {
            context = _context;
        }
        public List<Cart> GetAll()
        {
            List<Cart> carts = context.carts
                .Include(w => w.customer)
                .Include(w => w.product)
                .Where(w => !w.IsDeleted).ToList();
            return carts;
        }
        public Cart GetByProductId(int productId)
        {
            Cart cart = context.carts.FirstOrDefault(c => c.Product_Id == productId);
            return cart;
        }
        public Cart GetById(int id)
        {
            Cart? cart = context.carts
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
            return cart;
        }
        public List<Cart> GetByCustomerName(string userName)
        {
            List<Cart> carts = GetAll();
            List<Cart> wishListforSpecificCustomer = new List<Cart>();
            foreach (Cart cart in carts)
            {
                if (cart.customer.UserName == userName)
                {
                    wishListforSpecificCustomer.Add(cart);
                }
            }
            return wishListforSpecificCustomer;
        }
        public Cart FindByProductId(int id)
        {
            Cart cart = context.carts.FirstOrDefault(w => w.Product_Id == id);
            return cart;
        }
        public List<Cart> GetByCustomerId(string id)
        {
            List<Cart> carts = context.carts
                .Include(w => w.product)
                .Where(w => w.Customer_Id == id).ToList();
            return carts;
        }
        public void Insert(Cart obj)
        {
            context.Add(obj);
        }
        public void Update(Cart obj)
        {
            context.Update(obj);
        }
        public void Delete(int id)
        {
            Cart cart = GetById(id);
            cart.IsDeleted = true;
            context.Update(cart);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
