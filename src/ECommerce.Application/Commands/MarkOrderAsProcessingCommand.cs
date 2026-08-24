namespace ECommerce.Application.Commands
{
    public sealed record MarkOrderAsProcessingCommand(Guid OrderId);
}