using System.Threading.Tasks;
using Host.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<OrderDto> Get(int id)
        {
            return await _mediator.Send(new GetOrderQuery {Id = id});
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] OrderDto dto)
        {
            await _mediator.Send(new UpdateOrderCommand {Id = id, Dto = dto});

            return NoContent();
        }
    }
}
