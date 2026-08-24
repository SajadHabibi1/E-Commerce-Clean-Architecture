namespace ECommerce.Application.Commands
{
    public sealed record CheckoutOrderCommand(
        Guid CustomerId,
        string ShippingStreet,
        string ShippingCity,
        string ShippingPostalCode,
        string ShippingCountry,
        string BillingStreet,
        string BillingCity,
        string BillingPostalCode,
        string BillingCountry
    );
}