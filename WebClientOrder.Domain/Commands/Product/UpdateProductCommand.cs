using Flunt.Notifications;
using Flunt.Validations;
using System;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Product
{
    public class UpdateProductCommand : Notifiable, ICommand
    {
        public UpdateProductCommand() { }
        public UpdateProductCommand(Guid id, string description, decimal value)
        {
            Id = id;
            Description = description;
            Value = value;
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(Description, 3, "Description", "Should be have minimum 3 characters!")
                    .IsGreaterThan(Value, 0, "Value", "Should be greater then zero")
                    .IsFalse(Id == Guid.Empty, "Id", "Should be not Guid empty")
            );
        }
    }
}
