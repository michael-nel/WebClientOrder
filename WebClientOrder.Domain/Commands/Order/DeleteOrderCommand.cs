using Flunt.Notifications;
using Flunt.Validations;
using System;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Order
{
    public class DeleteOrderCommand: Notifiable, ICommand
    {
        public DeleteOrderCommand() { }
        public DeleteOrderCommand(Guid id)
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
