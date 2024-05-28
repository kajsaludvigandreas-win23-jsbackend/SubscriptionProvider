using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SubscriptionProvider.Data.Contexts;
using SubscriptionProvider.Data.Entities;
using System;
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

            string requestBody;
            using (var reader = new StreamReader(req.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<SubscribeEntity>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.Email))
            {
                _logger.LogWarning("Invalid subscription data.");
                return new BadRequestResult();
            }

            try
            {
                var existingSubscription = await _context.Subscribers
                    .FirstOrDefaultAsync(s => s.Email == data.Email);

                if (existingSubscription != null)
                {
                    _logger.LogInformation("Email already subscribed. Toggling subscription status.");

                    existingSubscription.IsSubscribed = !existingSubscription.IsSubscribed;
                    _context.Subscribers.Update(existingSubscription);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Subscription status toggled successfully.");
                    return new OkObjectResult(existingSubscription);
                }

                var subscribeEntity = new SubscribeEntity
                {
                    Email = data.Email,
                    IsSubscribed = data.IsSubscribed
                };

                _context.Subscribers.Add(subscribeEntity);
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
