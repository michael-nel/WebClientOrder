using System.Threading.Tasks;
using WebClientOrder.Domain.Entities;

namespace WebClientOrder.Domain.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetDocument(string document);
    }
}
