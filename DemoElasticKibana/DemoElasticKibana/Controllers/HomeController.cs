using DemoElasticKibana.Configs.ElasticSearch;
using DemoElasticKibana.Configs.RabbitMQ;
using DemoElasticKibana.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DemoElasticKibana.Controllers
{
    public class HomeController : Controller
    {
        public class Hehe
        {
            public Hehe()
            {
                hihi = "hello world";
            }
            public string hihi { get; set; }
        }
        private readonly ILogger<HomeController> _logger;
        private readonly IElasticSeachService<Hehe> _elasticSeachService;
        private readonly IRabbitMQService _rabbitMQService;

        public HomeController(ILogger<HomeController> logger, IElasticSeachService<Hehe> elasticSeachService, IRabbitMQService rabbitMQService)
        {
            _logger = logger;
            _elasticSeachService = elasticSeachService;
            _rabbitMQService = rabbitMQService;
        }

        public IActionResult Index()
        {
            //var document = new Hehe();
            //_elasticSeachService.IndexDocumentAsync("demo_hehe", document);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SendMessage(string message)
        {
            _rabbitMQService.SendMessage(new { Message = message }, "Text");
            return Ok();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
