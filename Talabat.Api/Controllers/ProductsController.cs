using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductsController(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var specification = new ProductWithTypeAndBrandSpecifications();
            var products = await _productRepository.GetAllWithSpecificationAsync(specification);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var specification = new ProductWithTypeAndBrandSpecifications();
            var product = await _productRepository.GetByIdWithSpecification(specification);
            return Ok(product);
        }
    }
}
