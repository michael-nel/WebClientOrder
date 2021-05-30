using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus.Extensions.Brazil;
using Mongo2Go;
using MongoDB.Driver;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Client;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Infra.Context;
using WebClientOrder.Domain.Handler;
using WebClientOrder.Infra.Repositories;
using Xunit;

namespace WebClientOrder.Test.Integration.Handle
{
    public class ClientHandleTest
    {
        private IClientRepository _clientRepository;
        private MongoContext _context;
        private readonly Faker _faker;
        private MongoClient _client;
        private MongoDbRunner _runner;
        private IMongoDatabase _database;
        private ClientHandler _clientHandler;

        public ClientHandleTest()
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
        public async Task Shouldbe_Add_New_Client()
        {
            _clientHandler = new ClientHandler(_clientRepository);

            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var createCommand = new CreateClientCommand
            {
                Name = name,
                Document = document,
                Email = email
            };

            var response = (GenericCommandResult)await _clientHandler.Handle(createCommand);

            var responseClient = (Client)response.Data;

            Assert.True(response.Success);
            Assert.Equal(name, responseClient.Name);
            Assert.Equal(document, responseClient.Document);
            Assert.Equal(email, responseClient.Email);
            Assert.NotEqual(Guid.Empty, responseClient.Id);
            _runner.Dispose();
        }


        [Fact]
        public async Task Shouldbe_Document_Exists_To_Add_Client()
        {
            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _clientHandler = new ClientHandler(_clientRepository);

            var newName = _faker.Person.FullName;
            var newDocument = document;
            var newEmail = _faker.Person.Email;

            var createCommand = new CreateClientCommand
            {
                Name = newName,
                Document = newDocument,
                Email = newEmail
            };

            var response = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => _clientHandler.Handle(createCommand));

            Assert.Equal("Client already registered!", response.ValidationResult.ToString());
            _runner.Dispose();
        }

        [Fact]
        public async Task Shouldbe_Update_A_Product()
        {
            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name,document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _clientHandler = new ClientHandler(_clientRepository);

            var nameToUpdate = _faker.Person.FirstName;
            var documentToUpdate = _faker.Person.Cpf();
            var emailToUpdate = _faker.Person.Email;

            var updateCommand = new UpdateClientCommand
            {
                Id = client.Id,
                Name = nameToUpdate,
                Document = documentToUpdate,
                Email =  emailToUpdate
            };

            var response = (GenericCommandResult)await _clientHandler.Handle(updateCommand);

            var responseClient = (Client)response.Data;

            Assert.True(response.Success);
            Assert.Equal(nameToUpdate, responseClient.Name);
            Assert.Equal(documentToUpdate, responseClient.Document);
            Assert.Equal(emailToUpdate, responseClient.Email);
            _runner.Dispose();
        }

        [Fact]
        public async Task Shouldbe_Not_Found_Product_To_Update()
        {
            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _clientHandler = new ClientHandler(_clientRepository);

            var nameToUpdate = _faker.Person.FirstName;
            var documentToUpdate = _faker.Person.Cpf();
            var emailToUpdate = _faker.Person.Email;

            var updateCommand = new UpdateClientCommand
            {
                Id = Guid.NewGuid(),
                Name = nameToUpdate,
                Document = documentToUpdate,
                Email = emailToUpdate
            };

            var response = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => _clientHandler.Handle(updateCommand));

            Assert.Equal("Client not found it!", response.ValidationResult.ToString());
            _runner.Dispose();

        }

        [Fact]
        public async Task Shouldbe_Delete_Client()
        {

            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _clientHandler = new ClientHandler(_clientRepository);

            var deleteCommand = new DeleteClientCommand
            {
                Id = client.Id
            };

            var response = (GenericCommandResult)await _clientHandler.Handle(deleteCommand);

            Assert.True(response.Success);

            _runner.Dispose();

        }

        [Fact]
        public async Task Shouldbe_Not_Found_Client_To_Delete()
        {

            var name = _faker.Person.FullName;
            var document = _faker.Person.Cpf();
            var email = _faker.Person.Email;

            var client = new Client(name, document, email);

            _clientRepository.Add(client);

            await _context.SaveChanges();

            DisposeAndReCreate();

            _clientHandler = new ClientHandler(_clientRepository);

            var deleteCommand = new DeleteClientCommand
            {
                Id = Guid.NewGuid()
            };

            var response = await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => _clientHandler.Handle(deleteCommand));

            Assert.Equal("Client not found it!", response.ValidationResult.ToString());

            _runner.Dispose();

        }

    }
}
