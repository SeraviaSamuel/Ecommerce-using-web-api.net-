using AutoMapper;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShipmentDTO, Shipment>();
            CreateMap<Shipment, ShipmentDTO>();
            CreateMap<PaymentDTO, Payment>();
            CreateMap<Payment, PaymentDTO>();
            CreateMap<WishlistDTO, WishList>();
            CreateMap<WishList, WishlistDTO>();
            CreateMap<CartDTO, Cart>();
            CreateMap<Cart, CartDTO>();
        }
    }
}
