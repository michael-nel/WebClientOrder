using System;
using System.Threading.Tasks;

namespace WebClientOrder.Domain.Services
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
