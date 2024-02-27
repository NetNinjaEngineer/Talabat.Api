using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var specification = new ProductWithTypeAndBrandSpecifications();
            var products = await _productRepository.GetAllWithSpecificationAsync(specification);
            var productsToReturn = _mapper.Map<IEnumerable<ProductToReturnDto>>(products);
            return Ok(productsToReturn);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var specification = new ProductWithTypeAndBrandSpecifications(id);
            var product = await _productRepository.GetByIdWithSpecification(specification);
            return Ok(product);
        }
    }
}
