using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions.Brazil;
using Mongo2Go;
using MongoDB.Driver;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Infra.Context;
using WebClientOrder.Infra.Repositories;
using WebClientOrder.Domain.ValueOfObjects;
using Xunit;

namespace WebClientOrder.Test.Integration.Repository
{
    public class OrderRepositoryTest
    {
        private IOrderRepository _orderRepository;

        private MongoContext _context;
        private readonly Faker _faker;
        private MongoClient _client;
        private MongoDbRunner _runner;
        private IMongoDatabase _database;

        public OrderRepositoryTest()
        {
            _faker = new Faker("pt_BR");
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _database = _client.GetDatabase("IntegrationTest4");
            _context = new MongoContext();
            _context.ConfigureMongo(_client, _database);
            _orderRepository = new OrderRepository(_context);
        }

        public void DisposeAndReCreate()
        {
            _context.Dispose();
            _context = new MongoContext();
            _context.ConfigureMongo(_client, _database);
            _orderRepository = new OrderRepository(_context);
        }

        [Fact]
        public async Task ShouldBe_Create_A_Order()
        {
            var name = _faker.Person.FirstName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);
            
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();
            var product = new Product(description, value);

            var address = new Address
            {
                Street = _faker.Address.StreetName(),
                State = _faker.Address.State(),
                City = _faker.Address.City(),
                District = _faker.Address.CitySuffix(),
                ZipCode = _faker.Address.ZipCode()
            };

            var productQuantity = new List<ProductQuantity>()
            {
                new ProductQuantity
                {
                    Product = product, 
                    Quantity = 2
                }
            };

            var order = new Order(client, address, productQuantity);

            _orderRepository.Add(order);

            await _context.SaveChanges();

            var orderFind = await _orderRepository.GetById(order.Id);

            Assert.Equal(orderFind.Client.Name, client.Name);
            Assert.Equal(orderFind.Client.Document, client.Document);
            _runner.Dispose();
        }

        [Fact]
        public async Task ShouldBe_Create_A_Order_And_Delete()
        {
            var name = _faker.Person.FirstName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);

            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();
            var product = new Product(description, value);

            var address = new Address
            {
                Street = _faker.Address.StreetName(),
                State = _faker.Address.State(),
                City = _faker.Address.City(),
                District = _faker.Address.CitySuffix(),
                ZipCode = _faker.Address.ZipCode()
            };

            var productQuantity = new List<ProductQuantity>()
            {
                new ProductQuantity
                {
                    Product = product,
                    Quantity = 2
                }
            };

            var order = new Order(client, address, productQuantity);

            _orderRepository.Add(order);

            await _context.SaveChanges();
            
            DisposeAndReCreate();
            
            var orderFind = await _orderRepository.GetById(order.Id);
            
            _orderRepository.Remove(orderFind.Id);

            await _context.SaveChanges();

            var orderDeleted = await _orderRepository.GetById(order.Id);

            Assert.Null(orderDeleted);

            _runner.Dispose();
        }


    }
}
