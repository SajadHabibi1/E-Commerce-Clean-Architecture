namespace ECommerce.Application.Commands
{
    public sealed record AddItemToCartCommand(
        Guid CustomerId,
        Guid ProductId,
        int Quantity
    );
}