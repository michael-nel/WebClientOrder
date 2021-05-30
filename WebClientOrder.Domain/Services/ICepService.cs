using System.Threading.Tasks;
using WebClientOrder.Domain.ValueOfObjects;

namespace WebClientOrder.Domain.Services
{
    public interface ICepService
    {
        Task<Address> Execute(string zipCode);
    }
}
