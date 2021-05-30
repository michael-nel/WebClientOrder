using System.Linq;
using Bogus;
using Mongo2Go;
using MongoDB.Driver;
using System.Threading.Tasks;
using Bogus.Extensions.Brazil;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Infra.Context;
using WebClientOrder.Infra.Repositories;
using Xunit;

namespace WebClientOrder.Test.Integration.Repository
{
    public class ClientRepositoryTest
    {
        private IClientRepository _clientRepository;
        private MongoContext _context;
        private readonly Faker _faker;
        private MongoClient _client;
        private IMongoDatabase _database;
        private MongoDbRunner _runner;

        public ClientRepositoryTest()
        {
            _faker = new Faker("pt_BR");
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _database = _client.GetDatabase("IntegrationTest");
            _context = new MongoContext();
            _context.ConfigureMongo(_client, _database);
            _clientRepository = new ClientRepository(_context);
        }

        public void DisposeAndReCreate()
        {
            _context.Dispose();
            _context = new MongoContext();
            _context.ConfigureMongo(_client, _database);
            _clientRepository = new ClientRepository(_context);
        }

        [Fact]
        public async Task Shoulbe_Add_3_Client_And_Get_All()
        {
            _clientRepository.Add(new Client(_faker.Person.FullName, _faker.Person.Cpf(), _faker.Person.Email));
            _clientRepository.Add(new Client(_faker.Person.FullName, _faker.Person.Cpf(), _faker.Person.Email));
            _clientRepository.Add(new Client(_faker.Person.FullName, _faker.Person.Cpf(), _faker.Person.Email));

            await _context.SaveChanges();

            var allClients = await _clientRepository.GetAll();

            Assert.Equal(3, allClients.Count());
            _runner.Dispose();
        }

        [Fact]
        public async Task Shoulbe_Add_1_Client_And_Delete()
        {
            var name = _faker.Person.FirstName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;
            
            var client = new Client(name, document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            DisposeAndReCreate();

            var findClient = await _clientRepository.GetById(client.Id);

            _clientRepository.Remove(client.Id);

            await _context.SaveChanges();

            var findDeleted = await _clientRepository.GetById(findClient.Id);

            Assert.Null(findDeleted);
            _runner.Dispose();
        }

        [Fact]
        public async Task Shoulbe_Add_1_Client_And_Update()
        {
            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            DisposeAndReCreate();

            var findClientToUpdate = await _clientRepository.GetById(client.Id);

            var nameUpdate = _faker.Person.FullName;
            var emailUpdate = _faker.Commerce.ProductDescription();
            var documentUpdate = findClientToUpdate.Document;

            findClientToUpdate.Update(nameUpdate, documentUpdate, emailUpdate);

            _clientRepository.Update(findClientToUpdate);

            await _context.SaveChanges();

            var findClientUpdated = await _clientRepository.GetById(findClientToUpdate.Id);

            Assert.Equal(nameUpdate, findClientUpdated.Name);
            Assert.Equal(emailUpdate, findClientUpdated.Email);
            _runner.Dispose();
        }

        [Fact]
        public async Task Shoulbe_Add_1_Products_And_Find_By_Product_Name()
        {
            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            var findByClientDocument = await _clientRepository.GetDocument(document);

            Assert.Equal(document, findByClientDocument.Document);
            _runner.Dispose();
        }
    }
}
