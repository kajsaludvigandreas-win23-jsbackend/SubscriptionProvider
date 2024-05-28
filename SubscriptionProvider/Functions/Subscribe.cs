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

            string requestBody;
            using (var reader = new StreamReader(req.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<SubscribeEntity>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.Email) || !data.IsSubscribed)
            {
                _logger.LogWarning("Invalid subscription data.");
                return new BadRequestResult();
            }

            if (await _context.Subscribers.FindAsync(data.Email) != null)
            {
                _logger.LogWarning("Email already subscribed.");
                return new ConflictResult();
            }

            var subscribeEntity = new SubscribeEntity
            {
                Email = data.Email,
                IsSubscribed = data.IsSubscribed
            };



            try
            {
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
