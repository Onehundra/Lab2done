using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2done
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Decimal Price { get; private set; }

        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
