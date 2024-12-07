

using System.Text.Json.Serialization;

namespace E_commerceStore01
{
    public class Customer : IIdentifiablecs
    {
        [JsonPropertyName("CustomerId")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }


        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("Address")]
        public string Address { get; set; }

        [JsonPropertyName("OrderHistory")]
        public List<Order> OrderHistory { get; set; }
      // public List<Product> Products { get; set; }
      //  public object product { get; }

        public Customer(int id, string name, string email, string password, string address, List<Order> orderHistory)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Address = address;
            OrderHistory = new List<Order>();
        }
    }
}
