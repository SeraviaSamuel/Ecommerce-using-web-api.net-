using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IWishListRepository
    {
        public List<WishList> GetAll();
        public WishList GetById(int id);
        public WishList FindByProductId(int id);
        public List<WishList> GetByCustomerId(string id);
        public List<WishList> GetByCustomerName(string userName);
        public void Insert(WishList obj);
        public void Update(WishList obj);
        public void Delete(int id);
        public void Save();
    }
}