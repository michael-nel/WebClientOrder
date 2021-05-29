using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Product;
using WebClientOrder.Domain.Handler;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Domain.Services;

namespace WebClientOrder.Api.Controllers
{
    [Route("product")]
    public class ProductController : _BaseController
    {
        public ProductController(IUnitOfWork unitOrWork): base(unitOrWork)  {}

        [HttpGet]
        public async Task<IActionResult> GetAll(
           [FromServices] IProductRepository repository
       )
        {
            var products = await repository.GetAll();

            return await ResponseGetAsync(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
            [FromRoute] Guid id,
            [FromServices] IProductRepository repository)
        {
            var response =  await repository.GetById(id);

            return await ResponseGetAsync(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
           [FromBody] CreateProductCommand command,
           [FromServices] ProductHandler handler)
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
           [FromBody] UpdateProductCommand command,
           [FromServices] ProductHandler handler)
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
          Guid  id,
          [FromServices] ProductHandler handler)
        {
            try
            {
                var command = new DeleteProductCommand {  Id = id};

                var response = (GenericCommandResult)await handler.Handle(command);

                return await ResponseAsync(response); 
            }
            catch( ValidationException ex)
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
