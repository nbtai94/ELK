using Microsoft.Extensions.Options;

namespace DemoElasticKibana.Configs.RabbitMQ
{
    public static class RabbitMQExtensions
    {
        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQService>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RabbitMQOptions>>();
                return new RabbitMQService(options);
            });
        }

        public static WebApplication StartConsumer(this WebApplication app)
        {
            var rabbitMQService = app.Services.GetRequiredService<IRabbitMQService>();
            rabbitMQService.StartConsumer();
            return app;
        }
    }
}
