

using Spectre.Console;
using System.Text.Json;

namespace E_commerceStore01
{
    public class E_CommerceSystem
    {
        public void DisplayProduct(MyDB myDB)
        {

            AnsiConsole.MarkupLine("\n[bold green]=== Available Products ===[/]");

            var table = new Table()
                .Border(TableBorder.Rounded)
                 .AddColumn("[yellow]ID[/]")
                .AddColumn("[yellow]Name[/]")
                .AddColumn("[yellow]Description[/]")
                .AddColumn("[yellow]Price[/]")
                .AddColumn("[yellow]Stock[/]");

            foreach (var product in myDB.AllProductDatafromEHandelsButikDataJSON)
            {
                table.AddRow(
                    product.Id.ToString(),
                    product.Name,
                    product.Description,
                    product.Price.ToString("C"),
                    product.Stock.ToString()
                );

            }

            AnsiConsole.Write(table);


            Console.WriteLine("\n--------------------------------------------");
        }


        public void ViewAllCustomers(MyDB myDB)
        {
            try
            {
                if (myDB.AllCustomerDatafromEHandelsButikDataJSON == null || !myDB.AllCustomerDatafromEHandelsButikDataJSON.Any())
                {
                    AnsiConsole.MarkupLine("[bold yellow]No customers found in the database.[/]");
                    return;
                }

                // Create a table for displaying customer information
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[yellow]Customer ID[/]")
                    .AddColumn("[yellow]Name[/]")
                    .AddColumn("[yellow]Email[/]")
                    .AddColumn("[yellow]Address[/]")
                    .AddColumn("[yellow]Order Count[/]");

                // Populate the table with customer data
                foreach (var customer in myDB.AllCustomerDatafromEHandelsButikDataJSON)
                {
                    table.AddRow(
                        customer.Id.ToString(),
                        customer.Name,
                        customer.Email,
                        customer.Address,
                        customer.OrderHistory?.Count.ToString() ?? "0"
                    );
                }

                // Display the table
                AnsiConsole.Write(table);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An error occurred while viewing customers: {ex.Message}[/]");
            }
        }


        public void AddToCart(MyDB myDB, int customerId)
        {
            try
            {
                // Display header with Spectre.Console
                AnsiConsole.Write(new FigletText("Add to Cart").Centered().Color(Color.Green));
                AnsiConsole.MarkupLine("[bold cyan]Available Products:[/]");

                // Display available products
                DisplayProduct(myDB);

                // Ask user for Product ID
                Console.WriteLine();
                int productId = AnsiConsole.Ask<int>("[bold yellow]Enter the Product ID to add to the cart:[/]");

                // Find the selected product
                var selectedProduct = myDB.AllProductDatafromEHandelsButikDataJSON.FirstOrDefault(p => p.Id == productId);
                if (selectedProduct == null)
                {
                    AnsiConsole.MarkupLine($"[bold red]Error:[/] Product with ID {productId} not found.");
                    return;
                }

                // Check stock availability
                if (selectedProduct.Stock <= 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Sorry, this product is out of stock![/]");
                    return;
                }

                // Ask for quantity
                int quantity = AnsiConsole.Ask<int>("[bold yellow]Enter quantity to add to the cart:[/]");
                if (quantity <= 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid quantity![/]");
                    return;
                }

                if (quantity > selectedProduct.Stock)
                {
                    AnsiConsole.MarkupLine("[bold red]Not enough stock available![/]");
                    return;
                }

                // Find or create the customer's cart
                var Cart = myDB.AllCartDatafromEHandelsButikDataJSON.FirstOrDefault(c => c.Id == customerId);

                if (Cart == null)
                {
                    var customer = myDB.AllCustomerDatafromEHandelsButikDataJSON.FirstOrDefault(c => c.Id == customerId);
                    if (customer == null)
                    {
                        throw new Exception($"Customer with ID {customerId} not found.");
                    }

                    Cart = new Cart(customerId, customer, new List<Product>());
                    myDB.AllCartDatafromEHandelsButikDataJSON.Add(Cart);

                    AnsiConsole.MarkupLine("[bold green]Your cart has been created and is ready to use![/]");
                }

                // Add product to cart
                var cartProduct = Cart.Products.FirstOrDefault(p => p.Id == productId);

                if (cartProduct == null)
                {
                    cartProduct = new Product
                    (
                        selectedProduct.Id,
                        selectedProduct.Name,
                        selectedProduct.Description,
                        selectedProduct.Price,
                        quantity // Treat stock as quantity in the cart
                    );
                    Cart.Products.Add(cartProduct);
                }
                else
                {
                    // Update the quantity in the cart if the product already exists
                    cartProduct.Stock += quantity;
                }

                // Update stock in the store
                selectedProduct.Stock -= quantity;

                // Success message
                AnsiConsole.MarkupLine($"[bold green]{quantity} unit(s) of [yellow]{selectedProduct.Name}[/] added to your cart successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An error occurred:[/] {ex.Message}");
            }

            // Save updated data back to JSON
            SaveDataToJson(myDB);
        }


        public void ViewCart(MyDB myDB, int customerId)
        {
            try
            {
                // find the cart based on customer knowledge
                var cart = myDB.AllCartDatafromEHandelsButikDataJSON.FirstOrDefault(c => c.Id == customerId);

                if (cart == null || !cart.Products.Any())
                {
                    // if the basket is empty or there is no basket for this customer
                    AnsiConsole.MarkupLine("[bold yellow]Your cart is empty. Add products to your cart before proceeding.[/]");
                    return;
                }

                // show cart contents 
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[yellow]Product ID[/]")
                    .AddColumn("[yellow]Product Name[/]")
                    .AddColumn("[yellow]Description[/]")
                    .AddColumn("[yellow]Quantity[/]")
                    .AddColumn("[yellow]Price[/]")
                    .AddColumn("[yellow]Total[/]");

                // calculate the total for the basket
                decimal totalAmount = 0;

                foreach (var product in cart.Products)
                {
                    decimal itemTotal = product.Price * product.Stock; // Stock represents quantity in the cart
                    table.AddRow(
                        product.Id.ToString(),
                        product.Name,
                        product.Description,
                        product.Stock.ToString(),
                        product.Price.ToString("C"),
                        itemTotal.ToString("C")
                    );
                    totalAmount += itemTotal;
                }

                // Display the table and total amount
                AnsiConsole.Write(table);
                AnsiConsole.MarkupLine($"[bold cyan]Total Amount: {totalAmount:C}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An error occurred while viewing your cart: {ex.Message}[/]");
            }
        }



        public void Checkout(MyDB myDB, int customerId)
        {
            try
            {
                // Display the Checkout header using Figgle
                Console.Clear();
                AnsiConsole.Write(new FigletText("Checkout").Centered().Color(Color.Green));

                // Find the customer's cart
                var cart = myDB.AllCartDatafromEHandelsButikDataJSON
                    .FirstOrDefault(c => c.Id == customerId);

                if (cart == null || cart.Products.Count == 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Your cart is empty! Add some products before checking out.[/]");
                    return;
                }

                // Create a table for displaying cart contents
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[yellow]Product Name[/]")
                    .AddColumn("[yellow]Quantity[/]")
                    .AddColumn("[yellow]Price (Each)[/]")
                    .AddColumn("[yellow]Subtotal[/]");

                decimal totalAmount = 0;

                // Populate the table and calculate the total
                foreach (var product in cart.Products)
                {
                    decimal productTotal = product.Price * product.Stock; // Stock treated as Quantity
                    totalAmount += productTotal;

                    table.AddRow(
                        product.Name,
                        product.Stock.ToString(),
                        $"${product.Price:F2}",
                        $"${productTotal:F2}"
                    );
                }

                // Display cart contents
                AnsiConsole.MarkupLine("[bold green]Your Cart:[/]");
                AnsiConsole.Write(table);

                // Display the total amount
                AnsiConsole.MarkupLine($"\n[bold yellow]Total Amount: [/]${totalAmount:F2}");

                // Confirm checkout
                var confirmation = AnsiConsole.Confirm("\n[bold cyan]Do you want to proceed with the checkout?[/]");

                if (!confirmation)
                {
                    AnsiConsole.MarkupLine("[bold yellow]Checkout canceled.[/]");
                    return;
                }

                // Process the order
                foreach (var cartProduct in cart.Products)
                {
                    var storeProduct = myDB.AllProductDatafromEHandelsButikDataJSON
                        .FirstOrDefault(p => p.Id == cartProduct.Id);

                    if (storeProduct == null)
                    {
                        AnsiConsole.MarkupLine($"[bold red]Error: Product {cartProduct.Name} not found in store database![/]");
                        continue;
                    }

                    if (cartProduct.Stock > storeProduct.Stock)
                    {
                        AnsiConsole.MarkupLine($"[bold red]Error: Insufficient stock for {cartProduct.Name}. Only {storeProduct.Stock} available.[/]");
                        continue;
                    }

                    // Deduct stock from store inventory
                    storeProduct.Stock -= cartProduct.Stock;
                }

                // Create a new order
                var order = new Order
                (
                    orderId: myDB.AllOrderDatafromEHandelsButikDataJSON.Count + 1,
                    customer: cart.Customer,
                    products: new List<Product>(cart.Products),
                    totalAmount: totalAmount
                );

                // Add the order to the database
                myDB.AllOrderDatafromEHandelsButikDataJSON.Add(order);

                // Clear the cart
                cart.Products.Clear();

                // Success message
                AnsiConsole.Write(new Panel("[bold green]Checkout successful! Your order has been placed.[/]")
                    .BorderColor(Color.Green)
                    .Expand());
                AnsiConsole.MarkupLine($"[bold cyan]Order ID: [/] {order.OrderId}");
                AnsiConsole.MarkupLine($"[bold cyan]Total: [/] ${totalAmount:F2}");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An error occurred during checkout: {ex.Message}[/]");
            }

            // Save updated data back to JSON
            SaveDataToJson(myDB);
        }

        public void ViewOrderHistory(MyDB myDB, int customerId)
        {
            try
            {
                // Prompt user for Customer ID
                AnsiConsole.Markup("[bold blue]Enter your Customer ID to view order history: [/]");

                if (!int.TryParse(Console.ReadLine(), out  customerId))
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid Customer ID![/]");
                    return;
                }

                // Find the customer in the database
                var customer = myDB.AllCustomerDatafromEHandelsButikDataJSON
                                   .FirstOrDefault(c => c.Id == customerId);

                if (customer == null)
                {
                    AnsiConsole.MarkupLine("[bold red]Customer not found![/]");
                    return;
                }

                // Retrieve the orders associated with this customer
                var customerOrders = myDB.AllOrderDatafromEHandelsButikDataJSON
                                         .Where(o => o.Customer != null && o.Customer.Id == customerId)
                                         .ToList();

                if (customerOrders.Count == 0)
                {
                    AnsiConsole.MarkupLine("[bold yellow]No order history found for this customer.[/]");
                    return;
                }

                // Display the order history
                AnsiConsole.MarkupLine("\n[bold green]=== Order History ===[/]");
                foreach (var order in customerOrders)
                {
                    AnsiConsole.MarkupLine($"[bold cyan]Order ID:[/] {order.OrderId}");
                    AnsiConsole.MarkupLine($"[bold cyan]Order Date:[/] {order.OrderDate.ToShortDateString()}");
                    AnsiConsole.MarkupLine($"[bold cyan]Total Amount:[/] {order.TotalAmount:C}");
                    AnsiConsole.MarkupLine($"[bold cyan]Ordered Items:[/]");

                    // Display the ordered products
                    foreach (var product in order.Products)
                    {
                        AnsiConsole.MarkupLine($"  - [green]{product.Name}[/], [yellow]Quantity: {product.Stock}[/], [cyan]Price: {product.Price:C}[/]");
                    }

                    AnsiConsole.MarkupLine("[gray]-------------------------------------[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An error occurred: {ex.Message}[/]");
            }
        }



        static void SaveDataToJson(MyDB myDB)
        {
            try
            {
                string dataJsonFilePath = "EShoppingStore.json";

                string updatedEShopDB = JsonSerializer.Serialize(myDB, new JsonSerializerOptions { WriteIndented = true });


                File.WriteAllText(dataJsonFilePath, updatedEShopDB);

                MirrorChangesToProjectRoot("EShoppingStore.json");


                AnsiConsole.MarkupLine("[bold green]Data saved successfully to JSON file.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An error occurred while saving data: {ex.Message}[/]");
            }
        }

        static void MirrorChangesToProjectRoot(string fileName)
        {
            // Get the path to the output directory
            string outputDir = AppDomain.CurrentDomain.BaseDirectory;

            // Get the path to the project root directory
            string projectRootDir = Path.Combine(outputDir, "../../../");

            // Define paths for the source (output directory) and destination (project root)
            string sourceFilePath = Path.Combine(outputDir, fileName);
            string destFilePath = Path.Combine(projectRootDir, fileName);

            // Copy the file if it exists
            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, destFilePath, true); // true to overwrite
                Console.WriteLine($"{fileName} has been mirrored to the project root.");
            }
            else
            {
                Console.WriteLine($"Source file {fileName} not found.");
            }
        }






    }
}