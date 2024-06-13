using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WexAssessmentApi.Data;
using WexAssessmentApi.Interfaces;
using WexAssessmentApi.Models;

namespace WexAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery]int page = 1, [FromQuery]int pageSize = 10)
        {
            IEnumerable<Product> products = await this._productRepository.GetAllAsync();

            int countOfProducts = products.Count();
            int pageCount = (int)Math.Ceiling((double)countOfProducts / pageSize);
            var query = products.Skip((page - 1) * pageSize).Take(pageSize);
            var result = new
            {
                ProductCount = countOfProducts,
                PageCount = pageCount,
                CurrentPage = page,
                PageSize = pageSize,
                Products = query.ToList()
            };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                Product product = await this._productRepository.GetByIdAsync(id);
                return Ok(product);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]Product product)
        {
            await this._productRepository.AddAsync(product);
            return CreatedAtAction("AddProduct", product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody]Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    product.Id = id;
                }
                await this._productRepository.UpdateAsync(product);
                return Ok(product);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                Product target = await this._productRepository.GetByIdAsync(id);
                await this._productRepository.DeleteAsync(target.Id);
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
