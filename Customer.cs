using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2done
{
    public class Customer
    {
        public string Name { get; private set; }
        public string Password { get; private set; }
        public List<CartProduct> Cart { get; private set; }


        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            Cart = new List<CartProduct>();
        }

        public void AddToCart(Product product, int quantity)
        {
            bool productExist = false;

            foreach (CartProduct c in Cart)
            {
                if (c.Product.Id == product.Id)
                {
                    productExist = true;
                    c.AddQuantity(quantity);
                    break;
                }

            }
            if (productExist == false)
            {
                CartProduct newProduct = new CartProduct(product, quantity);
                Cart.Add(newProduct);
            }
        }
        public void PrintCart()
        {
            decimal totalPrice = 0;

            Console.WriteLine("Your cart: ");
            foreach (CartProduct c in Cart)
            {
                Console.WriteLine($"{c.Quantity} x {c.Product.Name}: {c.Product.Price}kr/each = {c.GetTotalPrice()}kr");

                totalPrice += c.GetTotalPrice();
            }
            Console.WriteLine($"Total: {totalPrice}kr");

        }

        public override string ToString()
        {
            string cartAndInfo = $"Username: {Name}\nPassword: {Password}\nCart:\n";
            decimal totalPrice = 0;

            if (Cart.Count == 0)
            {
                cartAndInfo += "\n(Cart is empty)\n";
            }
            else
            {
                foreach (CartProduct c in Cart)
                {
                    decimal productsTotal = c.GetTotalPrice();
                    cartAndInfo += $" - {c.Product.Name} x {c.Quantity} {c.Product.Price}kr/each = {productsTotal} kr.\n";
                    totalPrice = productsTotal;
                }
            }

            return cartAndInfo;
        }
    }
}
