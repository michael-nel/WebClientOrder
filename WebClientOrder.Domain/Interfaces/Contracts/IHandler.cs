using System.Threading.Tasks;

namespace WebClientOrder.Domain.Interfaces.Contracts
{
    public interface IHandler<T> where T : ICommand
    {
        Task<ICommandResult> Handle(T command);
    }
}
