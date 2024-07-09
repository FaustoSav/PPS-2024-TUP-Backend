using FornitureStore.Models.Dtos.Orders;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Implementations;
using FornitureStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FornitureStore.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ordenes.");

                var asd = ex.ToString();
                return StatusCode(500, asd);
            }
            
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{orderId}/details")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _orderService.GetOrderDetailsByOrderIdAsync(orderId);
            return Ok(orderDetails);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _orderService.AddOrderAsync(newOrder);
                return Ok("Orden creada" +  newOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la orden.");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateDto orderDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var result = await _orderService.UpdateOrderAsync(orderDto);
        //        if (!result)
        //        {
        //            return NotFound();
        //        }
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al actualizar la orden.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAndDetailsAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok($"Orden ID {id} eliminada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la orden.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
