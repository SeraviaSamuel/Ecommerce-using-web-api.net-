using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IPaymentRepository
    {
        public List<Payment> GetAll();
        public Payment GetById(int id);
        public void Insert(Payment obj);
        public void Update(Payment obj);
        public void Delete(int id);
        public void Save();
    }
}