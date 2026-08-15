namespace ECommerce.Application.Commands
{
    public sealed record RemoveItemFromCartCommand(Guid CustomerId, Guid CartItemId);
}