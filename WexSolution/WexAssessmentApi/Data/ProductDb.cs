using System.Diagnostics;
using WexAssessmentApi.Models;

namespace WexAssessmentApi.Data
{
    internal class ProductDb
    {
        private readonly List<Product> _products = new List<Product>();
        private static ProductDb? _db;
        private static Object _lock = new Object();
        private int _nextId = 101;

        private ProductDb() 
        {
            _products.Add(new Product()
            {
                Id = 1,
                Name = "Apple",
                Price = 2.99M,
                Category = "Fruit",
                StockQuantity = 1000
            });
        }

        public static ProductDb Instance
        {
            get 
            {
                if (_db == null)
                {
                    lock (_lock)
                    {
                        if (_db == null)
                        {
                            _db = new ProductDb();
                        }
                    }
                }

                return _db; 
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return this._products;
        }

        public Product? GetProductById(int id)
        {
            return FindProduct(id);
        }

        public Product AddProduct(Product product)
        {
            Debug.Assert(product != null);

            Product newObj = new Product()
            {
                Id = this._nextId++,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                StockQuantity = product.StockQuantity,
            };
            this._products.Add(newObj);

            return newObj;
        }

        public void UpdateProduct(int id, Product product)
        {
            Debug.Assert(product != null);

            Product? target = FindProduct(id);
            if (target != null)
            {
                target!.Name = product.Name;
                target.Price = product.Price;
                target.Category = product.Category;
                target.StockQuantity = product.StockQuantity;
            }
        }

        public void RemoveProduct(int id)
        {
            Product? product = FindProduct(id);
            if (product != null)
            {
                this._products.Remove(product);
            }
        }

        private Product? FindProduct(int id)
        {
            return this._products.Where(p => p.Id == id)
                                 .FirstOrDefault();
        }
    }
}
