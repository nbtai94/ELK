using DemoElasticKibana.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace DemoElasticKibana.Configs.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMQService(IOptions<RabbitMQOptions> options)
        {
            var settings = options.Value;
            var _factory = new ConnectionFactory()
            {
                HostName = settings.Host,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
                VirtualHost = settings.VirtualHost,
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("RABBITMQ_EXCHANGE", ExchangeType.Direct, durable: true, autoDelete: false, null);
            _channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "hello", exchange: "RABBITMQ_EXCHANGE", routingKey: "hello");
        }
        public void SendMessage<T>(T data, string type) where T : class
        {
            var message = new RabbitMQMessage<T>(data, type);
            var body = Encoding.UTF8.GetBytes(message.ToJson());
            _channel.BasicPublish(exchange: "RABBITMQ_EXCHANGE", routingKey: "hello", body: body);
            Console.WriteLine($" [x] Sent {message}");
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        public void StartConsumer()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var strMessage = Encoding.UTF8.GetString(body);

                var message = JsonConvert.DeserializeObject<RabbitMQMessage<object>>(strMessage);
                Console.WriteLine($" [x] {message}");
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume("hello", consumerTag: "MY_CONSUMMER", autoAck: false, consumer: consumer);
        }
    }
}
