using FornitureStore.DBContext;
using FornitureStore.Models.Dtos.Orders;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FornitureStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly FornitureStoreContext _fornitureStoreContext;

        public OrderService(FornitureStoreContext fornitureStoreContext, ILogger<OrderService> logger)
        {
            _fornitureStoreContext = fornitureStoreContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
         

            
                return await _fornitureStoreContext.Orders
              .Include(o => o.OrderDetails)
              .AsNoTracking()
              .ToListAsync();

           

        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _fornitureStoreContext.Orders
                .Include(o => o.OrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _fornitureStoreContext.Orders
                .Where(o => o.Id == orderId)
                .SelectMany(o => o.OrderDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int idUser)
        {
            return await _fornitureStoreContext.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.UserId == idUser)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> OrderExistsAsync(int idOrder)
        {
            return await _fornitureStoreContext.Orders
                .AnyAsync(o => o.Id == idOrder);
        }

        public async Task AddOrderAsync(OrderCreateDto orderDto)
        {
            try
            {
                var order = new Order
                {
                    UserId = orderDto.UserId,
                    CreatedAt = DateTime.Now,
                    OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
                    {
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        Price = od.Price
                    }).ToList(),
                    Total = orderDto.OrderDetails.Sum(od => od.Price * od.Quantity)
                };

              
                await _fornitureStoreContext.Orders.AddAsync(order);
                await _fornitureStoreContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar la orden" );
                throw;
            }
        }

        //public async Task<bool> UpdateOrderAsync(Order order, List<OrderDetail> orderDetails)
        //{
        //    try
        //    {
        //        var existingOrder = await _fornitureStoreContext.Orders
        //            .Include(o => o.OrderDetails)
        //            .FirstOrDefaultAsync(o => o.Id == order.Id);

        //        if (existingOrder == null)
        //        {
        //            _logger.LogWarning("La orden con ID {OrderId} no fue encontrada", order.Id);
        //            return false;
        //        }

               
        //        existingOrder.CreatedAt = order.CreatedAt;
        //        existingOrder.UserId = order.UserId;

              
        //        _fornitureStoreContext.OrdersDetails.UpdateRange(orderDetails);

        //        await _fornitureStoreContext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Ocurrió un error al actualizar la orden con ID {OrderId}", order.Id);
        //        return false;
        //    }
        //}

        public async Task<bool> DeleteOrderAndDetailsAsync(int orderId)
        {
            try
            {
                var order = await _fornitureStoreContext.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning($"La orden con ID {orderId} no fue encontrada" );
                    return false;
                }

                _fornitureStoreContext.OrdersDetails.RemoveRange(order.OrderDetails);
                _fornitureStoreContext.Orders.Remove(order);
                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocurrió un error al eliminar la orden con ID {orderId}" );
                return false;
            }
        }

    }
}
