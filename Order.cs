

using System.Text.Json.Serialization;

namespace E_commerceStore01
{
    public class Order
    {

        [JsonPropertyName("OrderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<Product> Products { get; set; }


        [JsonPropertyName("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("OrderDate")]
        public DateTime OrderDate { get; set; }

       

        public Order(int orderId, Customer customer, List<Product> products, decimal totalAmount)
        {
            OrderId = orderId;
            Customer = customer;
            Products = products;
            TotalAmount = totalAmount;
            OrderDate = DateTime.Now;
        }
        // Method to display the order details
        public void DisplayOrderDetails()
        {
            Console.WriteLine($"Order ID: {OrderId}");
            Console.WriteLine($"Customer: {Customer.Name} (ID: {Customer.Id})");
            Console.WriteLine($"Order Date: {OrderDate}");
            Console.WriteLine("Products:");

            foreach (var product in Products)
            {
                Console.WriteLine($"- {product.Name} x {product.Stock} @ ${product.Price} each");
            }

            Console.WriteLine($"Total Amount: ${TotalAmount:F2}");
        }
    }
}
