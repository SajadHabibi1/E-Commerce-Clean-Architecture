using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Queries
{
    public sealed class GetOrderByIdHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Result<OrderDto>> HandleAsync(GetOrderByIdQuery query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            var order = await _orderRepository.GetByIdAsync(query.OrderId, ct);

            if (order is null)
            {
                return Result<OrderDto>.NotFound("Order not found");
            }

            return Result<OrderDto>.Success(order.ToDto());
        }
    }
}