using FornitureStore.Models.Dtos.Orders;
using FornitureStore.Models.Entities;
using System.Threading.Tasks;

namespace FornitureStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(); 
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int idUser);
        Task<bool> OrderExistsAsync(int idOrder);
        Task AddOrderAsync(OrderCreateDto order);
        //Task<bool> UpdateOrderAsync(Order order, List<OrderDetail> orderDetails);
        Task<bool> DeleteOrderAndDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetCurrentUserOrdersAsync(int currentUserId);





    }
}
