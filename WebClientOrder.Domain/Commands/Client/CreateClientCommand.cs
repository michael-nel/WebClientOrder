using Flunt.Notifications;
using Flunt.Validations;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Client
{
    public class CreateClientCommand : Notifiable, ICommand
    {
        public CreateClientCommand() { }
        public CreateClientCommand(string name, string document, string email)
        {
            Name = name;
            Document = document;
            Email = email;
        }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(Name, 4, "Name", "Should be have minimum 4 characters!")
                    .HasMinLen(Document, 11, "Name", "Should be have minimum 11 characters!")
                    .IsEmail(Email, "Email", "Its not a valid email!")
            );
        }
    }
}
