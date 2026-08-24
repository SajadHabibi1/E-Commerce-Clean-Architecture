namespace ECommerce.Application.Commands
{
    public sealed record MarkOrderAsPaidCommand(Guid OrderId);
}