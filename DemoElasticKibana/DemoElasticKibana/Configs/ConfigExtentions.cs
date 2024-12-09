namespace DemoElasticKibana.Configs
{
    public static class ConfigExtentions
    {
        public static IServiceCollection AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ElasticSearchOptions>(configuration.GetSection("ElasticSearch"));
            services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));
            services.Configure<SerilogOptions>(configuration.GetSection("Serilog"));
            return services;
        }
    }
}
