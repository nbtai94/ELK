using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace DemoElasticKibana.Configs.Serilog
{
    public static class SerilogConfig
    {
        public static void Config(WebApplicationBuilder builder)
        {
            var serilogOptions = builder.Configuration.GetSection("Serilog").Get<SerilogOptions>();
            var writeTo = serilogOptions.WriteTo.First(x => x.Name == "Elasticsearch").Args;
            // Cấu hình Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()  // Ghi log ra console
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(writeTo.nodeUris))
                {
                    AutoRegisterTemplate = writeTo.autoRegisterTemplate,
                    IndexFormat = writeTo.indexFormat,
                    NumberOfReplicas = writeTo.numberOfReplicas,  // Cấu hình replica của index
                    NumberOfShards = writeTo.numberOfShards     // Số shards của index
                })
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().GetName().Name)
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
        }
    }
}
