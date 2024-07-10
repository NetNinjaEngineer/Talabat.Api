using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers;

public class ProductsController(
    IMapper mapper,
    IGenericRepository<Product> productRepo,
    IGenericRepository<ProductType> typesRepo,
    IGenericRepository<ProductBrand> brandRepo) : APIBaseController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetAllProductsAsync(string? sort = null)
    {
        var specification = new ProductWithTypeAndBrandSpecifications(sort);
        var products = await productRepo.GetAllWithSpecificationAsync(specification);
        var productsToReturn = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
        return Ok(productsToReturn);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
    {
        var specification = new ProductWithTypeAndBrandSpecifications(id);
        var product = await productRepo.GetByIdWithSpecification(specification);
        if (product is null) return NotFound(new ApiResponse(404));
        var mappedProduct = mapper.Map<Product, ProductToReturnDto>(product);
        return Ok(mappedProduct);
    }

    [HttpGet("Types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
    {
        var types = await typesRepo.GetAllAsync();
        return Ok(types);
    }

    [HttpGet("Brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
        var brands = await brandRepo.GetAllAsync();
        return Ok(brands);
    }
}
