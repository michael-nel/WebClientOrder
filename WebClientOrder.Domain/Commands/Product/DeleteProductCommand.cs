using Flunt.Notifications;
using Flunt.Validations;
using System;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Product
{
    public class DeleteProductCommand : Notifiable, ICommand
    {
        public DeleteProductCommand() { }
        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .IsFalse(Id == Guid.Empty, "Id", "Id is empty!")
            );
        }
    }
}
