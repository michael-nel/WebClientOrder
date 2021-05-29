using System.Threading.Tasks;
using WebClientOrder.Domain.Entities;

namespace WebClientOrder.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductDescription(string description);
    }
}
