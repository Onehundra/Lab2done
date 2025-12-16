using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2done
{
    public class ProductManager
    {
        private readonly MongoDbService _db;
        public List<Product> Products { get; private set; }

        public ProductManager(MongoDbService db)
        {
            _db = db;
            loadProducts();
        }
        public void PrintProductList()
        {
            loadProducts();
            for (int i = 0; i < Products.Count; i++)
            {
                var p = Products[i];
                Console.WriteLine($"{i + 1}. {p.Name} = {p.Price} kr/each");
            }
        }

        public Product GetProductByIndex(int index)
        {
            if (index <1 || index > Products.Count)
            {
                return null;
            }
            return Products[index - 1];
        }
        public void loadProducts()
        {
            Products = _db.Products.Find(p => true).ToList();
        }
    }
}
