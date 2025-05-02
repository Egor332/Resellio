using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.RequestsParameters;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [Authorize(Policy = AuthorizationPolicies.CustomerPolicy)]
        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetMyTickets([FromQuery]GetTicketsDefaultParameters parameters) 
        {
            var userIdString = User.FindFirst(BearerTokenClaimsNames.Id);
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString.Value);

            var paginationResponse = new PaginationResult<TicketInfoDto>();
            try
            {
                paginationResponse = await _ticketService.GetMyTicketsAsync(userId, parameters.Filter, parameters.Pagination.Page, parameters.Pagination.PageSize);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(paginationResponse);
        }

        [HttpGet("available-tickets-of-selected-type")]
        public async Task<IActionResult> GetAvailableTicketsOfSelectedType([FromQuery]GetTicketOfSelectedTypeParameters parameters)
        {
            var paginationResponse = new PaginationResult<TicketInfoDto>();
            try
            {
                paginationResponse = await _ticketService.GetTicketsForSaleOfThisType(parameters.TicketTypeId, parameters.Filter, parameters.Pagination.Page, parameters.Pagination.PageSize);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(paginationResponse);
        }

    }
}
