using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IShipmentRepository
    {
        public List<Shipment> GetAll();
        public Shipment GetById(int id);
        public void Insert(Shipment obj);
        public void Update(Shipment obj);
        public void Delete(int id);
        public void Save();
    }
}