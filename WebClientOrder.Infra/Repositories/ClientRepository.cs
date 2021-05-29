using MongoDB.Driver;
using System.Threading.Tasks;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Infra.Context;

namespace WebClientOrder.Infra.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        private readonly IMongoContext _context;
        protected IMongoCollection<Client> _collection;
        public ClientRepository(IMongoContext context) : base(context)
        {
            _context = context;
            _collection = _context.GetCollection<Client>(typeof(Client).Name);
        }

        public async Task<Client> GetDocument(string document)
        {
            var result = await _collection.FindAsync(Builders<Client>.Filter.Eq("Document", document));

            return result.FirstOrDefault();
        }
    }
}
