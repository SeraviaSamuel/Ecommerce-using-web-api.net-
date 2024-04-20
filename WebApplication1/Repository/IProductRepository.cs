using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public Product GetById(int id);
        public List<Product> GetByCategoryId(int id);
        public void Insert(Product obj);
        public void Update(Product obj);
        public void Delete(int id);
        public void Save();
    }
}