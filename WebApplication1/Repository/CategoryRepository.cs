using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        Context context;
        public CategoryRepository(Context _context)
        {
            context = _context;
        }
        public List<Category> GetAll()
        {
            List<Category> categories = context.categories.Include(c => c.products).Where(c => !c.IsDeleted).ToList();
            return categories;
        }
        public Category GetById(int id)
        {
            Category? category = context.categories.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            return category;
        }
        public void Insert(Category obj)
        {
            context.Add(obj);
        }
        public void Update(Category obj)
        {
            context.Update(obj);
        }
        public void Delete(int id)
        {
            Category category = GetById(id);
            category.IsDeleted = true;
            context.Update(category);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
