using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Client;
using WebClientOrder.Domain.Handler;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Domain.Services;

namespace WebClientOrder.Api.Controllers
{
    [Route("client")]
    public class ClientController : _BaseController
    {
        public ClientController(IUnitOfWork unitOfWork): base(unitOfWork) {}

        [HttpGet]
        public async Task<IActionResult> GetAll(
          [FromServices] IClientRepository repository
      )
        {
            var clients = await repository.GetAll();

            return await ResponseGetAsync(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
            [FromRoute] Guid id,
            [FromServices] IClientRepository repository)
        {
            var client  = await repository.GetById(id);

            return await ResponseGetAsync(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
           [FromBody] CreateClientCommand command,
           [FromServices] ClientHandler handler)
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

        [HttpPut]
        public async Task<IActionResult> Update(
           [FromBody] UpdateClientCommand command,
           [FromServices] ClientHandler handler)
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
          [FromServices] ClientHandler handler)
        {
            try
            {
                var command = new DeleteClientCommand { Id = id };

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
