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
    public class Unsubscribe
    {
        private readonly ILogger<Unsubscribe> _logger;
        private readonly DataContext _context;

        public Unsubscribe(ILogger<Unsubscribe> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("Unsubscribe")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processing unsubscribe request.");

            string requestBody;
            using (var reader = new StreamReader(req.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<SubscribeEntity>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.Email))
            {
                _logger.LogWarning("Invalid unsubscribe data.");
                return new BadRequestResult();
            }

            try
            {
                var entity = await _context.Subscribers
                    .FirstOrDefaultAsync(s => s.Email == data.Email);

                if (entity == null)
                {
                    _logger.LogWarning("Email not found.");
                    return new NotFoundResult();
                }

                entity.IsSubscribed = false;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Unsubscribed successfully.");
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}