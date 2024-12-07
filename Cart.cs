

using System.Text.Json.Serialization;

namespace E_commerceStore01
{
    public class Cart
    {
        [JsonPropertyName("CustomerId")]
        public int Id { get; set; }


        public List<Product> Products { get; set; }

        public Customer Customer { get; set; }

        [JsonPropertyName("TotalAmount")]
        public decimal TotalAmount { get; set; }

        public Cart(int id, Customer customer, List<Product> products)
        {
            Id = id;
            Products = products;
            Customer = customer;
            TotalAmount = 0;
        }
    }
}
