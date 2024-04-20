using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface ICartRepository
    {
        public List<Cart> GetAll();
        public Cart GetById(int id);
        public Cart GetByProductId(int productId);
        public List<Cart> GetByCustomerName(string userName);
        public Cart FindByProductId(int id);
        public List<Cart> GetByCustomerId(string id);
        public void Insert(Cart obj);
        public void Update(Cart obj);
        public void Delete(int id);
        public void Save();
    }
}