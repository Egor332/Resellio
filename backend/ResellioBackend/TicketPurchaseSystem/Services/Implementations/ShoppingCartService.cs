using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Mapper;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.DTOs;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICartRedisRepository _cartRepository;
        private readonly ITicketsRepository _ticketsRepository;

        public ShoppingCartService(ICartRedisRepository cartRedisRepository, ITicketsRepository ticketsRepository)
        {
            _cartRepository = cartRedisRepository;
            _ticketsRepository = ticketsRepository;
        }

        public async Task<CartInfoDto> GetShoppingCartInfoAsync(int userId)
        {
            var timeToExpiration = await _cartRepository.GetExpirationTimeAsync(userId);
            var ticketIdsEnumeration = await _cartRepository.GetAllTicketsAsync(userId);
            var ticketInfoList = new List<TicketInfoDto>();
            foreach (var ticketId in ticketIdsEnumeration)
            {
                var ticket = await _ticketsRepository.GetTicketWithAllDependenciesByIdAsync(ticketId);
                ticketInfoList.Add(EventManagementSystemMapper.TicketToTicketInfoDto(ticket));
            }
            if (ticketInfoList.Count > 0)
            {
                return CreateCartInfoDto(true, ticketInfoList, timeToExpiration);
            }
            else
            {
                return CreateCartInfoDto(false, null, null);
            }
        }

        private CartInfoDto CreateCartInfoDto(bool isExist, List<TicketInfoDto>? ticketsInfo, TimeSpan? timeToExpiration)
        {
            return new CartInfoDto()
            {
                IsCartExist = isExist,
                ticketsInCart = ticketsInfo,
                CartExpirationTime = DateTime.UtcNow + timeToExpiration,
            };
        }
    }
}
