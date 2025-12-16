using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2done
{
    public class ShopManager
    {
        private readonly MongoDbService _db;
        private List<Customer> customers = new List<Customer>();
        private ProductManager productManager;

        public ShopManager(MongoDbService db)
        {
            _db = db;
            productManager = new ProductManager(_db);
            customers.Add(new Customer("Knatte", "123"));
            customers.Add(new Customer("Fnatte", "321"));
            customers.Add(new Customer("Tjatte", "213"));
            customers.Add(new Customer("kevin", "111")); //ADMIN ;)
            _db = db;
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
                           
                            if (customerLogIn.Name.ToLower() == "kevin")
                            {
                                RunAdminMenu();
                            }
                            else
                            {
                                RunCustomerMenu(customerLogIn);
                            }
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

                        Product selected = productManager.GetProductByIndex(idChoice);
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
                            var order = new Order
                            {
                                Items = customer.Cart.Select(item => new OrderItem
                                {
                                    ProductId = item.Product.Id,
                                    Quantity = item.Quantity
                                }).ToList()
                            };

                            _db.Orders.InsertOne(order);

                            foreach (var item in order.Items)
                            {
                                var product = _db.Products
                                    .Find(p => p.Id == item.ProductId)
                                    .FirstOrDefault();

                                if (product != null)
                                {
                                    product.Stock -= item.Quantity;

                                    _db.Products.ReplaceOne(p => p.Id == product.Id, product);
                                }
                            }

                            Console.WriteLine("Order saved to database!");
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
        public void RunAdminMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== ADMIN MENU ===");
                Console.WriteLine("[1] Create product");
                Console.WriteLine("[2] Read products");
                Console.WriteLine("[3] Update product");
                Console.WriteLine("[4] Delete product");
                Console.WriteLine("[5] Logout");
                Console.Write("\nMake a choice: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateProduct();
                        break;
                    case 2:
                        ReadProducts();
                        break;
                    case 3:
                        UpdateProduct();
                        break;
                    case 4:
                        DeleteProduct();
                        break;

                    case 5:
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid input");
                        Console.ReadKey();
                        break;
                }
            }
        }
        public void CreateProduct()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE PRODUCT ===");

            Console.Write("Product name: ");
            string name = Console.ReadLine();

            Console.Write("Price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Stock: ");
            int stock = int.Parse(Console.ReadLine());

            var product = new Product
            {
                Name = name,
                Price = price,
                Stock = stock
            };

            _db.Products.InsertOne(product);

            Console.WriteLine("\nProduct created and saved to MongoDB!");
            Console.WriteLine("Press any key to return to admin menu...");
            Console.ReadKey();
        }

        public void ReadProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ALL PRODUCTS ===\n");

            var products = _db.Products.Find(p => true).ToList();
            
            {
                for (int i = 0; i < products.Count; i++)
                {
                    var p = products[i];
                    Console.WriteLine($"{i + 1}. {p.Name} | {p.Price} kr | Stock: {p.Stock}");
                }
            }

            Console.WriteLine("\nPress any key to return to admin menu...");
            Console.ReadKey();
        }

        public void UpdateProduct()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE PRODUCT ===\n");

            var products = _db.Products.Find(p => true).ToList();

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {products[i].Name} | {products[i].Price} kr | {products[i].Stock} in stock");
            }

            Console.Write("\nSelect product number: ");
            int choice = int.Parse(Console.ReadLine());

            var selectedProduct = products[choice - 1];

            Console.Write("New price: ");
            selectedProduct.Price = decimal.Parse(Console.ReadLine());

            Console.Write("New stock count: ");
            selectedProduct.Stock = int.Parse(Console.ReadLine());

            _db.Products.ReplaceOne(p => p.Id == selectedProduct.Id, selectedProduct);

            Console.WriteLine("\nProduct updated!");
            Console.WriteLine("Press any key to return to admin menu...");
            Console.ReadKey();
        }

        public void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE PRODUCT ===\n");

            var products = _db.Products.Find(p => true).ToList();

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {products[i].Name} | {products[i].Price} kr");
            }

            Console.Write("\nSelect product number to delete: ");

            int choice = int.Parse(Console.ReadLine());
            var selectedProduct = products[choice - 1];

            _db.Products.DeleteOne(p => p.Id == selectedProduct.Id);

            Console.WriteLine("\nProduct deleted!");
            Console.WriteLine("Press any key to return to admin menu...");
            Console.ReadKey();
        }


    }
}
