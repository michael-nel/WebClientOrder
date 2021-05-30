using MongoDB.Driver;
using System.Threading.Tasks;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Infra.Context;

namespace WebClientOrder.Infra.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly IMongoContext _context;
        protected IMongoCollection<Product> _collection;
        public ProductRepository(IMongoContext context) : base(context)
        {
            _context = context;
            _collection = _context.GetCollection<Product>(typeof(Product).Name);
        }

        public async Task<Product> GetProductDescription(string description)
        {
            var result  = await _collection.FindAsync(Builders<Product>.Filter.Eq("Description", description));
            
            return result.FirstOrDefault();
        }
    }
}
