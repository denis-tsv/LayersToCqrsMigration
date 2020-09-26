using System.Threading.Tasks;
using Host.Services;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [CheckOrderAsyncActionFilter]
        //[ServiceFilter(typeof(CheckOrderAsyncActionFilter))]
        public async Task<OrderDto> Get(int id)
        {
            return await _orderService.GetAsync(id);
        }

        [HttpPost("{id}")]
        [CheckOrderAsyncActionFilter]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] OrderDto dto)
        {
            await _orderService.UpdateAsync(id, dto);

            return NoContent();
        }
    }
}
