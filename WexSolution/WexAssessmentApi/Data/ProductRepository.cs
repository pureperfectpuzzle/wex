using WexAssessmentApi.Interfaces;
using WexAssessmentApi.Models;

namespace WexAssessmentApi.Data
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private int _nextId = 1;
        private readonly object _lock = new object();

        /// <summary>
        /// Gets products of a category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            IEnumerable<Product> products = await this.GetAllAsync();
            return products.Where(p => p.Category == category);
        }

        /// <summary>
        /// Updates product exists in repository whose Id equals to entiy's Id.
        /// InvalidOperationException is thrown if no product is found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task UpdateAsync(Product entity)
        {
            Product product = await this.GetByIdAsync(entity.Id);
            product.Name = entity.Name;
            product.Price = entity.Price;
            product.Category = entity.Category;
            product.StockQuantity = entity.StockQuantity;
        }

        /// <summary>
        /// Creates a new Id for the new product before adding it into repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override int PreAddObject(Product entity)
        {
            int newId;
            lock (_lock)
            {
                newId = _nextId++;
                entity.Id = newId;
            }

            return newId;
        }
    }
}
