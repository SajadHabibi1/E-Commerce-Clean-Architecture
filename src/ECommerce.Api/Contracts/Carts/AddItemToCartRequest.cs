namespace ECommerce.Api.Contracts.Carts
{
    public sealed record AddItemToCartRequest(
        Guid ProductId,
        int Quantity
    );
}