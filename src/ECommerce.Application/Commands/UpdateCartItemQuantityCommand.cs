namespace ECommerce.Application.Commands
{
    public sealed record UpdateCartItemQuantityCommand(
        Guid CustomerId,
        Guid CartItemId,
        int Quantity
    );
}