namespace ECommerce.Application.Commands
{
    public sealed record CancelOrderCommand(Guid OrderId);
}