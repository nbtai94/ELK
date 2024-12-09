namespace DemoElasticKibana.Configs.RabbitMQ
{
    public interface IRabbitMQService
    {
        void SendMessage<T>(T data, string type) where T : class;
        void StartConsumer();
    }

}
