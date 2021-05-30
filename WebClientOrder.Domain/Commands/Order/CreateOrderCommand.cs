using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using WebClientOrder.Domain.Interfaces.Contracts;

namespace WebClientOrder.Domain.Commands.Order
{
    public class CreateOrderCommand : Notifiable, ICommand
    {
        public CreateOrderCommand() { }
        public CreateOrderCommand(Guid clientId, IEnumerable<ProductsListCommand> products, string zipCode)
        {
            ClientId = clientId;
            Products = products;
            ZipCode = zipCode;
        }
        public Guid ClientId { get; set; }
        public IEnumerable<ProductsListCommand> Products { get; set; }
        public string ZipCode { get; set; }
        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasLen(ZipCode, 8, "ZipCode", "Should be have 8 characters!")
                    .IsFalse(ClientId == Guid.Empty, "ClientId", "Its not a valid ClientId!")
                    .IsFalse(Products.Any( x => x.Id == Guid.Empty), "Products", "Ids empty in Products")
                    .IsFalse(Products.Any(x => x.Quantity <= 0), "Products","Quantity need greather then zero")
            );
        }
        public class ProductsListCommand
        {
            public Guid Id { get; set; }
            public int Quantity { get; set; }
        }
    }
}
