using Spectre.Console;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Channels;
using System.Transactions;

namespace E_commerceStore01
{
    public class DisplyUserE_CommerceSystemInteraction
    {

         private Customer? _loggedInCustomer; // Store the logged-in customer

        public void RunSystem()
        {
           

            E_CommerceSystem e_CommerceSystem = new E_CommerceSystem();

            string dataJsonFilePath = "EShoppingStore.json";
            string allDataAsJson = File.ReadAllText(dataJsonFilePath);
            MyDB myDB = JsonSerializer.Deserialize<MyDB>(allDataAsJson)!;

            AnsiConsole.Write(new FigletText("E-Shopping Store").Color(Color.Green));


            e_CommerceSystem.ViewAllCustomers(myDB);


            // Authenticate User
            _loggedInCustomer = Login(myDB);

            if (_loggedInCustomer == null)
            {
                Console.WriteLine("Login required to access the E-Commerce Store.");
                return;
            }

            bool running = true;
            while (running)
            {

                AnsiConsole.MarkupLine("\n[bold green]===[/] [yellow]E-Shopping Store Menu[/] [bold green]===\n[/]");
                AnsiConsole.MarkupLine("[bold cyan]1.[/] [white]View Products[/]");
                AnsiConsole.MarkupLine("[bold cyan]2.[/] [white]Add to Cart[/]");
                AnsiConsole.MarkupLine("[bold cyan]3.[/] [white]View Cart[/]");
                AnsiConsole.MarkupLine("[bold cyan]4.[/] [white]Checkout[/]");
                AnsiConsole.MarkupLine("[bold cyan]5.[/] [white]View Order History[/]");
                AnsiConsole.MarkupLine("[bold cyan]6.[/] [white]Exit[/]");

                var choice = AnsiConsole.Prompt(
                    new TextPrompt<string>("[bold blue]Enter your choice:[/] ")
                        .PromptStyle("green")
                        .Validate(input =>
                            input is "1" or "2" or "3" or "4" or "5" or "6" ? ValidationResult.Success() : ValidationResult.Error("[red]Invalid choice![/]")));


                switch (choice)
                {

                    case "1":
                    e_CommerceSystem.DisplayProduct(myDB);
                        break;
                    case "2":

                        e_CommerceSystem.AddToCart(myDB, _loggedInCustomer.Id);
                        break;
                    case "3":
                        e_CommerceSystem.ViewCart(myDB, _loggedInCustomer.Id);

                        break;
                    case "4":
                        e_CommerceSystem.Checkout(myDB, _loggedInCustomer.Id);

                        break;
                    case "5":
                        e_CommerceSystem.ViewOrderHistory(myDB, _loggedInCustomer.Id);

                        break;
                    case "6":

                        running = false;
                        Console.WriteLine("Thank you for using E-Commerce Store.");
                        break;


                }


            }




        }

        private Customer? Login(MyDB myDB)
        {
            Console.Write("Enter your ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                return null;
            }

            Console.Write("Enter your password: ");
            string password = Console.ReadLine() ?? string.Empty;

            // Find the customer in the database
            var customer = myDB.AllCustomerDatafromEHandelsButikDataJSON
                .FirstOrDefault(c => c.Id == id && c.Password == password);

            if (customer == null)
            {
                Console.WriteLine("Invalid ID or password. Please try again.");
                return null;
            }

            Console.WriteLine($"Welcome back, {customer.Name}!");

            return customer;
        }


    }
}
