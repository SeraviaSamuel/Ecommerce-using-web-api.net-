using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class WishListRepository : IWishListRepository
    {
        Context context;
        public WishListRepository(Context context)
        {
            this.context = context;
        }
        public List<WishList> GetAll()
        {
            List<WishList> wishLists = context.wishLists
                .Include(w => w.customer)
                .Include(w => w.product)
                .Where(w => !w.IsDeleted).ToList();
            return wishLists;
        }
        public WishList GetById(int id)
        {
            WishList? wishList = context.wishLists
                .FirstOrDefault(w => w.Id == id && !w.IsDeleted);
            return wishList;
        }
        public List<WishList> GetByCustomerName(string userName)
        {
            List<WishList> wishLists = GetAll();
            List<WishList> wishListforSpecificCustomer = new List<WishList>();
            foreach (WishList wishList in wishLists)
            {
                if (wishList.customer.UserName == userName)
                {
                    wishListforSpecificCustomer.Add(wishList);
                }
            }
            return wishListforSpecificCustomer;
        }
        public WishList FindByProductId(int id)
        {
            WishList wish = context.wishLists.FirstOrDefault(w => w.Product_Id == id);
            return wish;
        }
        public List<WishList> GetByCustomerId(string id)
        {
            List<WishList> wishLists = context.wishLists
                .Include(w => w.product)
                .Where(w => w.Customer_Id == id).ToList();
            return wishLists;
        }
        public void Insert(WishList obj)
        {
            context.Add(obj);
        }
        public void Update(WishList obj)
        {
            context.Update(obj);
        }
        public void Delete(int id)
        {
            WishList wishList = GetById(id);
            wishList.IsDeleted = true;
            context.Update(wishList);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
