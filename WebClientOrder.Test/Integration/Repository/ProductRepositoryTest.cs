using Bogus;
using Mongo2Go;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Infra.Context;
using WebClientOrder.Infra.Repositories;
using Xunit;

namespace WebClientOrder.Test.Integration.Repository
{
    public class ProductRepositoryTest
    {
        private IProductRepository _productRepository;
        private MongoContext _context;
        private readonly Faker _faker;
        private MongoClient _client;
        private IMongoDatabase _database;

        public ProductRepositoryTest()
        {
            _faker = new Faker("pt_BR");
            var runner = MongoDbRunner.Start();
            _client = new MongoClient(runner.ConnectionString);
            _database = _client.GetDatabase("IntegrationTest");
            _context = new MongoContext();
            _context.ConfigureMongo(_client, _database);
            _productRepository = new ProductRepository(_context);
        }

        public void DisposeAndReCreate()
        {
            _context.Dispose();
            _context = new MongoContext();
            _context.ConfigureMongo(_client, _database);
            _productRepository = new ProductRepository(_context);
        }

        [Fact]
        public async Task Shoulbe_Add_3_Products_And_Get_All()
        {

            _productRepository.Add(new Product(_faker.Commerce.ProductDescription(), _faker.Random.Decimal()));
            _productRepository.Add(new Product(_faker.Commerce.ProductDescription(), _faker.Random.Decimal()));
            _productRepository.Add(new Product(_faker.Commerce.ProductDescription(), _faker.Random.Decimal()));

            await _context.SaveChanges();

            var allProducts = await  _productRepository.GetAll();

            Assert.Equal(3, allProducts.Count());
        }

        [Fact]
        public async Task Shoulbe_Add_1_Products_And_Delete()
        {
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();
            var product = new Product(description, value);

            _productRepository.Add(product);

            await _context.SaveChanges();

            DisposeAndReCreate();

            var findProduct = await _productRepository.GetById(product.Id);

            _productRepository.Remove(findProduct.Id);

            await _context.SaveChanges();

            var findDeleted = await _productRepository.GetById(findProduct.Id);

            Assert.Equal(null, findDeleted);
        }

        [Fact]
        public async Task Shoulbe_Add_1_Products_And_Update()
        {
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();
            var product = new Product(description, value);

            _productRepository.Add(product);

            await _context.SaveChanges();

            DisposeAndReCreate();

            var findProductToUpdate = await _productRepository.GetById(product.Id);

            var descriptionUpdate = _faker.Commerce.ProductDescription();
            var valueUpdate = _faker.Random.Decimal();

            findProductToUpdate.Update(descriptionUpdate, valueUpdate);

            _productRepository.Update(findProductToUpdate);

            await _context.SaveChanges();

            var findProductUpdated = await _productRepository.GetById(findProductToUpdate.Id);

            Assert.Equal(descriptionUpdate, findProductUpdated.Description);
            Assert.Equal(valueUpdate, findProductUpdated.Value);
        }

        [Fact]
        public async Task Shoulbe_Add_1_Products_And_Find_By_Product_Name()
        {
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();
            var product = new Product(description, value);

            _productRepository.Add(product);

            await _context.SaveChanges();

            var findByProductName = await _productRepository.GetProductDescription(description);

            Assert.Equal(description, findByProductName.Description);
        }
    }
}
