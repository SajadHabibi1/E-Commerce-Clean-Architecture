namespace ECommerce.Application.Commands
{
    public sealed record MarkOrderAsShippedCommand(Guid OrderId);
}