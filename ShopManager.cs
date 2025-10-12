using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2done
{
    public class ShopManager
    {
        private List<Customer> customers = new List<Customer>();
        private ProductManager productManager = new ProductManager();

        public ShopManager()
        {
            customers.Add(new Customer("Knatte", "123"));
            customers.Add(new Customer("Fnatte", "321"));
            customers.Add(new Customer("Tjatte", "213"));
            customers.Add(new Customer("kevin", "111"));
        }

        public void RunStartMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Welcome to the store ===");
                Console.WriteLine("\n[1] Log in");
                Console.WriteLine("[2] Create new account");
                Console.WriteLine("[3] Exit");
                Console.Write("\nMake a choice: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Customer customerLogIn = LogIn();
                        if (customerLogIn != null)
                        {
                            Console.Clear();
                            Console.WriteLine($"Logging in...\n\n");
                            Console.WriteLine(customerLogIn);
                            Console.ReadKey();
                            RunCustomerMenu(customerLogIn);
                        }
                        break;
                    case 2:
                        CreateAccount();
                        break;
                    case 3:
                        Console.WriteLine("Exiting shop");
                        running = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input, please try again, press any key to continue!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }

            }
        }
        public Customer LogIn()
        {
            Console.WriteLine("Please enter your username");
            string username = Console.ReadLine().ToLower();

            foreach (Customer c in customers)
            {
                if (username.ToLower() == c.Name.ToLower())
                {
                    Console.WriteLine("Please enter your password");
                    string password = Console.ReadLine().ToLower();

                    if (password == c.Password.ToLower())
                    {
                        Console.WriteLine($"{c.Name} logged in.");
                        return c;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong password!\nPlease try log in again, press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        return null;
                    }
                }

            }

            Console.Clear();
            Console.WriteLine("Username doesnt exist. Do you want to create new account?(yes/no)");
            string input = Console.ReadLine().ToLower();
            if (input == "yes")
            {
                CreateAccount();
            }
            else if (input == "no")
            {
                return null;
            }
            else
            {
                Console.WriteLine("Invalid input.. Press any key to return to main menu");
                Console.ReadKey();
                Console.Clear();
            }
            return null;

        }
        public void RunCustomerMenu(Customer customer)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine($"=== {customer.Name} Menu ===");
                Console.WriteLine("[1] Shop");
                Console.WriteLine("[2] Print cart");
                Console.WriteLine("[3] Checkout cart");
                Console.WriteLine("[4] Sign out");
                Console.Write("\nMake a choice: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        productManager.PrintProductList();
                        Console.WriteLine("Pick a product by ID. Return = 0.");
                        int idChoice = int.Parse(Console.ReadLine());

                        if (idChoice == 0)
                        {
                            break;
                        }

                        Product selected = productManager.GetProductId(idChoice);
                        if (selected != null)
                        {
                            Console.WriteLine("How many?");
                            int quantity = int.Parse(Console.ReadLine());

                            customer.AddToCart(selected, quantity);
                            Console.WriteLine($"{quantity} x {selected.Name} added to your cart.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid product ID..");
                        }

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        customer.PrintCart();
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Do you wanna checkout?");

                        if (customer.Cart.Count == 0)
                        {
                            Console.WriteLine("Cart is empty, nothing to checkout.. Press any key to return");
                            Console.ReadKey();
                            break;
                        }

                        customer.PrintCart();

                        Console.WriteLine("Proceed checkout? (yes/no)");
                        string input = Console.ReadLine().ToLower();

                        if (input == "yes")
                        {
                            Console.WriteLine("Thank you for your purchase!");
                            customer.Cart.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Checkout cancelled");
                        }
                        Console.WriteLine("Press any key to return to menu..");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
                        Console.WriteLine("Signing out");
                        Console.WriteLine("Press any key to return to mainmenu");
                        Console.ReadKey();
                        Console.Clear();
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input..(press any key)");
                        Console.ReadKey();
                        return;

                }

            }
        }
        public void CreateAccount()
        {
            Console.Clear();
            Console.Write("Enter new username: ");
            string username = Console.ReadLine();

            foreach (Customer c in customers)
            {
                if (username == c.Name)
                {
                    Console.WriteLine($"{username} already exists, try new username. Press any key to return");
                    Console.ReadKey();
                    return;
                }
            }
            Console.Write("\nEnter new password: ");
            string password = Console.ReadLine();
            Console.Clear();
            customers.Add(new Customer(username, password));
            Console.WriteLine($"User: {username} was created!");
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
