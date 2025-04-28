using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.RequestsParameters;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypesController : ControllerBase
    {
        private readonly ITicketTypeService _ticketTypeService;

        public TicketTypesController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        [HttpGet("ticket-types-of-event")]
        public async Task<IActionResult> GetTicketTypesOfEvent([FromQuery] GetTicketTypesOfEventRequestParameters parameters)
        {
            var paginationResponse = new PaginationResult<TicketTypeInfoDto>();
            try
            {
                paginationResponse = await _ticketTypeService.GetTicketTypesOfEventAsync(parameters.Filter, parameters.Pagination.Page, parameters.Pagination.PageSize);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(paginationResponse);
        }
    }
}
