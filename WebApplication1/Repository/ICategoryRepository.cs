using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetById(int id);
        public void Insert(Category obj);
        public void Update(Category obj);
        public void Delete(int id);
        public void Save();
    }
}