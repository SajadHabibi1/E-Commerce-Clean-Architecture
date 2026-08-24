namespace ECommerce.Application.Commands
{
    public sealed record MarkOrderAsDeliveredCommand(Guid OrderId);
}