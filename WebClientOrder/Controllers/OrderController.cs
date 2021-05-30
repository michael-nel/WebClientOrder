using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Order;
using WebClientOrder.Domain.Handler;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Domain.Services;

namespace WebClientOrder.Api.Controllers
{
    [Route("order")]
    public class OrderController : _BaseController
    {
        public OrderController(IUnitOfWork unitOfWork): base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
         [FromServices] IOrderRepository repository
     )
        {
            var orders = await repository.GetAll();

            return await ResponseGetAsync(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
            [FromRoute] Guid id,
            [FromServices] IOrderRepository repository)
        {
            var order = await repository.GetById(id);

            return await ResponseGetAsync(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
           [FromBody] CreateOrderCommand command,
           [FromServices] OrderHandler handler)
        {
            try
            {
                var response = (GenericCommandResult)await handler.Handle(command);

                return await ResponseAsync(response);
            }
            catch (ValidationException ex)
            {
                return await ResponseAsync(new GenericCommandResult(false, ex.Message, null));
            }
            catch (Exception ex)
            {
                return ResponseErrorInternalAsync(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(
          Guid id,
          [FromServices] OrderHandler handler)
        {
            try
            {
                var command = new DeleteOrderCommand { Id = id };

                var response = (GenericCommandResult)await handler.Handle(command);

                return await ResponseAsync(response);
            }
            catch (ValidationException ex)
            {
                return await ResponseAsync(new GenericCommandResult(false, ex.Message, null));
            }
            catch (Exception ex)
            {
                return ResponseErrorInternalAsync(ex.Message);
            }
        }
    }
}
