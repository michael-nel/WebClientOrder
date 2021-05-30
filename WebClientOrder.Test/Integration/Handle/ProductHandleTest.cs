using Bogus;
using Mongo2Go;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Product;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Handler;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Infra.Context;
using WebClientOrder.Infra.Repositories;
using Xunit;

namespace WebClientOrder.Test.Integration.Handle
{
    public class ProductHandleTest
    {
        private IProductRepository _productRepository;
        private MongoContext _context;
        private readonly Faker _faker;
        private MongoClient _client;
        private MongoDbRunner _runner;
        private IMongoDatabase _database;
        private ProductHandler _productHandler;

        public ProductHandleTest()
        {
            _faker = new Faker("pt_BR");
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
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
        public async Task Shouldbe_Add_New_Product()
        {
            _productHandler = new ProductHandler(_productRepository);

            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();

            var createCommand = new CreateProductCommand
            {
                Description = description,
                Value = value
            };

            var response = (GenericCommandResult) await _productHandler.Handle(createCommand);

            var responseProduct = (Product) response.Data;

            Assert.True(response.Success);
            Assert.Equal(description, responseProduct.Description);
            Assert.Equal(value, responseProduct.Value);
            Assert.NotEqual(Guid.Empty, responseProduct.Id);
            _runner.Dispose();
        }

        [Fact]
        public async Task Shouldbe_Update_A_Product()
        {
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();

            var product = new Product(description, value);
            
            _productRepository.Add(product);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _productHandler = new ProductHandler(_productRepository);

            var descriptionToUpdate = _faker.Commerce.ProductDescription();
            var valueToUpdate = _faker.Random.Decimal();

            var updateCommand = new UpdateProductCommand
            {
                Id = product.Id,
                Description = descriptionToUpdate,
                Value = valueToUpdate
            };

            var response = (GenericCommandResult) await _productHandler.Handle(updateCommand);

            var responseProduct = (Product)response.Data;

            Assert.True(response.Success);
            Assert.Equal(descriptionToUpdate, responseProduct.Description);
            Assert.Equal(valueToUpdate, responseProduct.Value);
            _runner.Dispose();
        }

        [Fact]
        public async Task Shouldbe_Not_Found_Product_To_Update()
        {
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();

            var product = new Product(description, value);

            _productRepository.Add(product);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _productHandler = new ProductHandler(_productRepository);

            var descriptionToUpdate = _faker.Commerce.ProductDescription();
            var valueToUpdate = _faker.Random.Decimal();

            var updateCommand = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Description = descriptionToUpdate,
                Value = valueToUpdate
            };

            var response = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => _productHandler.Handle(updateCommand));
            
            Assert.Equal("Product not found it!", response.ValidationResult.ToString());
           _runner.Dispose();

        }

        [Fact]
        public async Task Shouldbe_Delete_Product()
        {
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();

            var product = new Product(description, value);

            _productRepository.Add(product);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _productHandler = new ProductHandler(_productRepository);

            var deleteCommand = new DeleteProductCommand
            {
                Id = product.Id
            };

            var response = (GenericCommandResult)await _productHandler.Handle(deleteCommand);
            
            Assert.True(response.Success);

            _runner.Dispose();

        }

        [Fact]
        public async Task Shouldbe_Not_Found_Product_To_Delete()
        {
            var description = _faker.Commerce.ProductDescription();
            var value = _faker.Random.Decimal();

            var product = new Product(description, value);

            _productRepository.Add(product);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _productHandler = new ProductHandler(_productRepository);

            var deleteCommand = new DeleteProductCommand
            {
                Id = Guid.NewGuid(),
            };

            var response = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => _productHandler.Handle(deleteCommand));

            Assert.Equal("Product not found it!", response.ValidationResult.ToString());

            _runner.Dispose();
        }
    }
}
