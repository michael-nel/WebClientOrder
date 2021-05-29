using Flunt.Notifications;
using Flunt.Validations;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Product
{
    public class CreateProductCommand : Notifiable, ICommand
    {
        public CreateProductCommand() { }
        public CreateProductCommand(string description, decimal value)
        {
            Description = description;
            Value = value;
        }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(Description, 3, "Description", "Should be have minimum 3 characters!")
                    .IsGreaterThan(Value, 0, "Value", "Should be greater then zero")
            );
        }
    }
}
