using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SubscriptionProvider.Data;
using SubscriptionProvider.Data.Contexts;
using SubscriptionProvider.Data.Entities;
using System.IO;
using System.Threading.Tasks;

namespace SubscriptionProvider.Functions
{
    public class Subscribe
    {
        private readonly ILogger<Subscribe> _logger;
        private readonly DataContext _context;

        public Subscribe(ILogger<Subscribe> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("Subscribe")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processing subscription request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody)!;
            string email = data?.email;
            bool isSubscribed = data?.isSubscribed;

            if (string.IsNullOrEmpty(email) || !isSubscribed)
            {
                _logger.LogWarning("Invalid subscription data.");
                return new BadRequestResult();
            }

            var subscribeEntity = new SubscribeEntity
            {
                Email = email,
                IsSubscribed = isSubscribed
            };

            try
            {
                _context.Subscribers.Add(subscribeEntity); // Assuming you have a DbSet<SubscribeEntity> named Subscriptions
                await _context.SaveChangesAsync();
                _logger.LogInformation("Subscription saved successfully.");
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving subscription.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

