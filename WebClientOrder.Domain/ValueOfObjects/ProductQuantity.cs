using WebClientOrder.Domain.Entities;

namespace WebClientOrder.Domain.ValueOfObjects
{
    public class ProductQuantity
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
