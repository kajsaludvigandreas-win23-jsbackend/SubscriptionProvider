using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SubscriptionProvider.Data.Entities;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
                   
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody)!;
            string email = data?.email;
            bool isSubscribed = data?.isSubscribed;
            if(email == null || isSubscribed == false)
            {
                return new BadRequestResult();
            }
            var subscribeEntity = new SubscribeEntity
            {
                Email = email,
                IsSubscribed = isSubscribed
            };
           
            return new OkResult();

        }
    }
}
