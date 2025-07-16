using AutoMapper;
using DommiArts.API.Models;
using DommiArts.API.DTOs.Order;
using DommiArts.API.DTOs.OrderItem;

namespace DommiArts.API.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderItem, OrderItemDTO>();


            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderItemCreateDTO, OrderItem>();
        }
    }

}
