using Flunt.Notifications;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Client;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Interfaces.Contracts;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Domain.Shared;

namespace WebClientOrder.Domain.Handler
{
    public class ClientHandler: Notifiable, IHandler<CreateClientCommand>, IHandler<UpdateClientCommand>, IHandler<DeleteClientCommand>
    {
        private readonly IClientRepository _clientRepository;
        public ClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<ICommandResult> Handle(CreateClientCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);

            await CheckDocumentExist(command.Document);

            var client = new Client(command.Name, command.Document, command.Email);

            _clientRepository.Add(client);

            return new GenericCommandResult(true, "SuccessFully", client);
        }
        public async Task<ICommandResult> Handle(UpdateClientCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);
            
            var client = await GetById(command.Id);
            
            client.Update(command.Name, command.Document, command.Email);

            _clientRepository.Update(client);

            return new GenericCommandResult(true, "SuccessFully", client);
        }
        public async Task<ICommandResult> Handle(DeleteClientCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);

            var client = await GetById(command.Id);

            _clientRepository.Remove(client.Id);
            
            return new GenericCommandResult(true, "SuccessFully", client);
        }
        private async Task<Client> GetById(Guid id)
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
                throw new ValidationException("Client not found it!");

            return client;
        }
        private async Task CheckDocumentExist(string document)
        {
            var exists = await _clientRepository.GetDocument(document);

            if (exists != null)
                throw new ValidationException("Client already registered!");
        }
       
    }
}
