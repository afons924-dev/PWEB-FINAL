using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync(string userId); // For Client/Supplier
    Task<IEnumerable<Order>> GetAllOrdersAsync(); // For Admin
    Task<Order?> GetOrderAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> UpdateOrderAsync(Order order);
}
