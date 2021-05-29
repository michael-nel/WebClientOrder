using Flunt.Notifications;
using Flunt.Validations;
using System;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Client
{
    public class DeleteClientCommand : Notifiable, ICommand
    {
        public DeleteClientCommand() { }
        public DeleteClientCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .IsFalse(Id == Guid.Empty, "Id", "Its not a valid id!")
            );
        }
    }
}
