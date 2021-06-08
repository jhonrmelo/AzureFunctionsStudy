public class Order
{
    public string PartitionKey { get; set; }
    public string RowKey { get { return OrderId.ToString(); } }
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public string Email { get; set; }
    public decimal Price { get; set; }
}