using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2done
{
    public class ProductManager
    {
        public List<Product> Products { get; private set; }

        public ProductManager()
        {
            Products = new List<Product>()
            {
                new Product(1,"Basic T-Shirt",149.95m),
                new Product(2,"Basic Jeans",249.95m),
                new Product(3,"Basic Sweatshirt",199.95m),
                new Product(4,"Basic Hat",119.95m),
                new Product(5,"Premium T-Shirt",1450m),
                new Product(6,"Premium Jeans",2199m),
                new Product(7,"Premium Sweatshirt",1300m),
                new Product(8,"Premium Golden Hat",10000m),
                new Product(9,"Bag",5m)
            };
        }
        public void PrintProductList()
        {
            foreach (Product p in Products)
            {
                Console.WriteLine($"ID:{p.Id} {p.Name} = {p.Price} kr/each");
            }
        }

        public Product GetProductId(int id)
        {
            foreach (Product p in Products)
            {
                if (p.Id == id)
                {
                    return p;
                }
            }

            return null;
        }
    }
}
