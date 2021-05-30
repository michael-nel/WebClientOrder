using System.Collections.Generic;
using WebClientOrder.Domain.ValueOfObjects;

namespace WebClientOrder.Domain.Entities
{
    public class Order : Entity
    {
        public Order(Client client, Address address, IEnumerable<ProductQuantity> products) : base()
        {
            Client = client;
            Address = address;
            Products = products;
        }
        public Client Client { get; private set; }
        public Address Address { get; private set; }
        public IEnumerable<ProductQuantity> Products { get; private set; }
    }
}
