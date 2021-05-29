namespace WebClientOrder.Domain.Entities
{
    public class Client: Entity
    {
        public Client(string name, string document, string email) : base()
        {
            Name = name;
            Document = document;
            Email = email;
        }
        public string Name { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }

        public void Update(string name, string document, string email)
        {
            Name = name;
            Document = document;
            Email = email;
        }
    }
}
