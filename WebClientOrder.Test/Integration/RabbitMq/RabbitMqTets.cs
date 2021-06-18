using RabbitMQ.Client;
using RabbitMQ.Fakes.DotNetStandard;
using System.Text;
using FluentAssertions;
using Xunit;

namespace WebClientOrder.Test.Integration.RabbitMq
{
    public class RabbitMqTets
    {

        [Fact]
        public void ShouldSendAMessage()
        {
            var rabbitServer = new RabbitServer();

            InitializeQueue("my_queue", rabbitServer);
            SendMessage(rabbitServer, rabbitServer.DefaultExchange.Name, "my_queue", "hello_world");

            using var connection = new FakeConnectionFactory(rabbitServer).CreateConnection();
            using var channel = connection.CreateModel();

            // Act
            var message = channel.BasicGet("my_queue", autoAck: false);

            // Assert
            message.Should().NotBeNull();
            var messageBody = Encoding.ASCII.GetString(message.Body.ToArray());

            messageBody.Should().Be("hello_world");

            rabbitServer.Queues["my_queue"].Messages.Count.Should().Be(1);
            channel.BasicAck(message.DeliveryTag, multiple: false);
            rabbitServer.Queues["my_queue"].Messages.Count.Should().Be(0);
        }

        private void SendMessage(RabbitServer rabbitServer, string exchange, string routingKey, string message, IBasicProperties basicProperties = null)
        {
            using var connection = new FakeConnectionFactory(rabbitServer).CreateConnection();
            using var channel = connection.CreateModel();

            var messageBody = Encoding.ASCII.GetBytes(message);

            channel.BasicPublish(exchange: exchange, routingKey: routingKey, mandatory: false, basicProperties: basicProperties, body: messageBody);
        }

        private void InitializeQueue(string queueName, RabbitServer server)
        {
            using var connection = new FakeConnectionFactory(server).CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
    }
}
