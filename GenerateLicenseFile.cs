using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AzfPluralsight
{
    public static class GenerateLicenseFile
    {
        [FunctionName("GenerateLicenseFile")]
        public static async Task Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")]Order order,
            IBinder binder,
            ILogger log)
        {

            var outputBlob = await binder.BindAsync<TextWriter>(
                new BlobAttribute($"licenses/{order.OrderId}.lic")
                {
                    Connection = "AzureWebJobsStorage"
                });
            outputBlob.WriteLine($"OrderId: {order.OrderId}" );
            outputBlob.WriteLine($"Email: {order.Email}");
            outputBlob.WriteLine($"ProductId: {order.ProductId}");
            outputBlob.WriteLine($"PurchaseDate: {DateTime.Now}");

            var md5 = MD5.Create();

            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(order.Email + "secret"));

            outputBlob.WriteLine($"SecretCode: {BitConverter.ToString(hash).Replace("-","")}");
        } 
    }
}
