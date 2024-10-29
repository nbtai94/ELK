using DemoElasticKibana.Models;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Reflection;
using DemoElasticKibana.Configs;
using Microsoft.Extensions.Options;
using DemoElasticKibana.Configs.Serilog;
using DemoElasticKibana.Configs.ElasticSearch;

namespace DemoElasticKibana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Add configs
            builder.Services.Configure<ElasticSearchOptions>(builder.Configuration.GetSection("ElasticSearch"));
            builder.Services.Configure<SerilogOptions>(builder.Configuration.GetSection("Serilog"));

            // Add elastic search
            builder.Services.AddElasticSearch();
            builder.Services.AddSingleton(typeof(IElasticSeachService<>), typeof(ElasticSearchService<>));

            SerilogConfig.Config(builder);

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Sử dụng Serilog cho ASP.NET Core
            app.UseMiddleware<SerilogMiddleWare>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
