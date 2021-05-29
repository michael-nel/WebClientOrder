using WebClientOrder.Domain.ValueOfObjects;

namespace WebClientOrder.Domain.Entities
{
    public class Order : Entity
    {
        public Order(Client client, Address address, Product product) : base()
        {
            Client = client;
            Address = address;
            Product = product;
        }
        public Client Client { get; private set; }
        public Address Address { get; private set; }
        public Product Product { get; private set; }
    }
}
