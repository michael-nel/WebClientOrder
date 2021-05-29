namespace WebClientOrder.Domain.Entities
{
    public class Product : Entity
    {
        public Product(string description, decimal value) : base()
        {
            Description = description;
            Value = value;
        }
        public string Description { get; private set; }
        public decimal Value { get; private set; }

        public void Update(string description, decimal value)
        {
            Description = description;
            Value = value;
        }
    }
}
