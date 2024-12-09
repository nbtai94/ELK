using Nest;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace DemoElasticKibana.Configs.Serilog
{
    public class SerilogMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SerilogMiddleWare> _logger;
        public SerilogMiddleWare(RequestDelegate next, ILogger<SerilogMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestTime = DateTime.UtcNow;
            // Đọc request body
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0; // Reset stream về đầu để tiếp tục xử lý request

            // Ghi đè response body để có thể đọc nó sau khi response hoàn tất
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Gọi tiếp đến middleware tiếp theo trong pipeline
            await _next(context);

            // Ghi lại thời gian kết thúc request
            var responseTime = DateTime.UtcNow;

            // Đọc response body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Log thông tin request và response
            _logger.LogInformation("Request in {Duration}ms\nRequest: {RequestBody}\nResponse: {ResponseBody}",
                (responseTime - requestTime).TotalMilliseconds,
                requestBody,
                responseText);

            // Ghi response ra stream thực tế để trả về client
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
