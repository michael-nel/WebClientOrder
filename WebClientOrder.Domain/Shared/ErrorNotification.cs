using WebClientOrder.Domain.Commands;

namespace WebClientOrder.Domain.Shared
{
    public static class ErrorNotification
    {
        public static  GenericCommandResult Error(object notification) => new GenericCommandResult(false, "Ops, something wrong!", notification);
    }
}
