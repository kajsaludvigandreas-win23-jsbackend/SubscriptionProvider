using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SubscriptionProvider.Functions
{
    public class Subscribe
    {
        private readonly ILogger<Subscribe> _logger;
       

        public Subscribe(ILogger<Subscribe> logger)
        {
            _logger = logger;
        }

        [Function("Subscribe")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("");
            return new OkObjectResult("");
        }
    }
}
