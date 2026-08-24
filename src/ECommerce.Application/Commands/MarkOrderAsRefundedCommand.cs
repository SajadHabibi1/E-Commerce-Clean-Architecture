namespace ECommerce.Application.Commands
{
    public sealed record MarkOrderAsRefundedCommand(Guid OrderId);
}