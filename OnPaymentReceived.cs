using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.IO;
using System.Threading.Tasks;

namespace AzfPluralsight
{
    public static class OnPaymentReceived
    {
        [FunctionName("OnPaymentReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] 
            HttpRequest req, 
            [Queue("orders")] IAsyncCollector<Order> orderQueue,
            [Table("orders")] IAsyncCollector<Order> orderTable,
            ILogger log)
        {
            log.LogInformation("Payment Received.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Order data = JsonConvert.DeserializeObject<Order>(requestBody);
            data.PartitionKey = "orders";

            await orderTable.AddAsync(data);
            await orderQueue.AddAsync(data);


            log.LogInformation($"Order {data.OrderId} received from e-mail {data.Email} for product {data.ProductId}");

            string responseMessage = "Thank you for your purchase";


            return new OkObjectResult(responseMessage);
        }
    }


}
