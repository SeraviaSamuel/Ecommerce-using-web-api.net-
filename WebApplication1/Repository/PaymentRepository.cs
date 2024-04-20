using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        Context context;
        public PaymentRepository(Context _context)
        {
            context = _context;
        }
        public List<Payment> GetAll()
        {
            List<Payment> payments = context.payments.Include(s => s.customer).Where(c => !c.IsDeleted).ToList();
            return payments;
        }
        public Payment GetById(int id)
        {
            Payment? payment = context.payments.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            return payment;
        }
        public void Insert(Payment obj)
        {
            context.Add(obj);
        }
        public void Update(Payment obj)
        {
            context.Update(obj);
        }
        public void Delete(int id)
        {
            Payment payment = GetById(id);
            payment.IsDeleted = true;
            context.Update(payment);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
