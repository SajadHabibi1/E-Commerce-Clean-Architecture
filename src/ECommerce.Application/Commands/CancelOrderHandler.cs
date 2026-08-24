using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Commands
{
    public sealed class CancelOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Result<Guid>> HandleAsync(CancelOrderCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            var order = await _orderRepository.GetByIdAsync(cmd.OrderId, ct);

            if (order is null)
            {
                return Result<Guid>.NotFound("Order not found");
            }

            try
            {
                order.Cancel();
                await _orderRepository.UpdateAsync(order, ct);

                return Result<Guid>.Success(order.Id);
            }

            catch(DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}