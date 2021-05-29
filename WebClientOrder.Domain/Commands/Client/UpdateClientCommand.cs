using Flunt.Notifications;
using Flunt.Validations;
using System;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Client
{
    public class UpdateClientCommand : Notifiable, ICommand
    {
        public UpdateClientCommand() { }
        public UpdateClientCommand(Guid id, string name, string document, string email)
        {
            Id = id;
            Name = name;
            Document = document;
            Email = email;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(Name, 5, "Name", "Should be have minimum 5 characters!")
                    .HasMinLen(Document, 11, "Name", "Should be have minimum 11 characters!")
                    .IsEmail(Email, "Email", "Its not a valid email!")
                    .IsFalse(Id == Guid.Empty, "Id", "Its not a valida Id")
            );
        }
    }
}
