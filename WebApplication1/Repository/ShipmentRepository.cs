using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ShipmentRepository : IShipmentRepository
    {
        Context context;
        public ShipmentRepository(Context context)
        {
            this.context = context;
        }
        public List<Shipment> GetAll()
        {
            List<Shipment> shipments = context.shipments.Include(s => s.customer).Where(s => !s.IsDeleted).ToList();
            return shipments;
        }
        public Shipment GetById(int id)
        {
            Shipment? shipment = context.shipments.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
            return shipment;
        }
        public void Insert(Shipment obj)
        {
            context.Add(obj);
        }
        public void Update(Shipment obj)
        {
            context.Update(obj);
        }
        public void Delete(int id)
        {
            Shipment shipment = GetById(id);
            shipment.IsDeleted = true;
            context.Update(shipment);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
