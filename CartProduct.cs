using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2done
{
    public class CartProduct
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public CartProduct(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public decimal GetTotalPrice()
        {
            return Product.Price * Quantity;
        }

        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }

    }
}
