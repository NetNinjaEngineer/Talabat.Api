using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Errors;
using Talabat.Api.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers;


public class ProductsController(
    IMapper mapper,
    IUnitOfWork unitOfWork) : APIBaseController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProductsAsync([FromQuery] ProductSpecParams Params)
    {
        var specification = new ProductWithTypeAndBrandSpecifications(Params);
        var products = await unitOfWork.Repository<Product>().GetAllWithSpecificationAsync(specification);
        var productsToReturn = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
        var productCountSpec = new ProductWithFilterationCountSpec(Params);
        var Count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(productCountSpec);
        return Ok(new Pagination<ProductToReturnDto>(Params.PageNumber, Params.PageSize, productsToReturn, Count));
    }


    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
    {
        var specification = new ProductWithTypeAndBrandSpecifications(id);
        var product = await unitOfWork.Repository<Product>().GetEntityWithSpecification(specification);
        if (product is null) return NotFound(new ApiResponse(404));
        var mappedProduct = mapper.Map<Product, ProductToReturnDto>(product);
        return Ok(mappedProduct);
    }


    [HttpGet("Types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
    {
        var types = await unitOfWork.Repository<ProductType>().GetAllAsync();
        return Ok(types);
    }


    [HttpGet("Brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
        var brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();
        return Ok(brands);
    }
}
