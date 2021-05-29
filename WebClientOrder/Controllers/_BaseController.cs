using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Services;

namespace WebClientOrder.Api.Controllers
{
    public class _BaseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public _BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ResponseAsync(GenericCommandResult response)
        {
            if (!response.Success)
                return BadRequest(response);
            else
            {
                try
                {
                    await _unitOfWork.Commit();
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Internal Error: {ex.Message}");
                }
            }
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ResponseGetAsync(object response)
        {
            if (response != null)
                return await ResponseAsync(new GenericCommandResult(true, "Got it!!!", response));
            else
                return await ResponseAsync(new GenericCommandResult(false, "Sorry!!! Dont found it anyone!!!", response));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ResponseErrorInternalAsync(string error)
        {
            var result = new GenericCommandResult(false, error, null);
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
