using DemoElasticKibana.Configs;
using Microsoft.Extensions.Options;
using Nest;

namespace DemoElasticKibana.Configs.ElasticSearch
{
    public static class ElasticSearchClient
    {
        public static void AddElasticSearch(this IServiceCollection services)
        {
            // Sử dụng DI để truy cập IOptions<ElasticSearchOptions>
            services.AddSingleton<IElasticClient>(sp =>
            {
                // Lấy IOptions<ElasticSearchOptions> thông qua ServiceProvider
                var options = sp.GetRequiredService<IOptions<ElasticSearchOptions>>().Value;

                // Khởi tạo ElasticClient với cấu hình từ options
                var settings = new ConnectionSettings(new Uri(options.Url));
                return new ElasticClient(settings);
            });
        }
    }
}
