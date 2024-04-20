using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ProductRepository : IProductRepository
    {
        Context context;
        public ProductRepository(Context _context)
        {
            context = _context;
        }
        public List<Product> GetAll()
        {
            List<Product> products = context.products.Where(p => !p.IsDeleted).ToList();
            return products;
        }
        public Product GetById(int id)
        {
            Product? product = context.products.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            return product;
        }
        public List<Product> GetByCategoryId(int id)
        {
            List<Product> products = context.products.Where(p => p.CategoryId == id && !p.IsDeleted).ToList();
            return products;
        }
        public void Insert(Product obj)
        {
            context.Add(obj);
        }
        public void Update(Product obj)
        {
            context.Update(obj);
        }
        public void Delete(int id)
        {
            Product product = GetById(id);
            product.IsDeleted = true;
            context.Update(product);

        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
