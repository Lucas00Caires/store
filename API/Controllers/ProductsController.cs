using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Specifications;
using Domain.Interfaces;
using Domain.Model.Entities;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;


        public ProductsController(IGenericRepository<Product> productRepository, 
                                  IGenericRepository<ProductBrand> productBrandRepository, 
                                  IGenericRepository<ProductType> productTypeRepository,
                                  IMapper mapper)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts(
            [FromQuery] ProductSpecificationParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _productRepository.CountAsync(countSpec);
            var products = await _productRepository.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return Ok(new Pagination<ProductDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }
        
        ////////////////////// TO REMOVE - Just for tests 


        [HttpGet("error")]
        public async Task<ActionResult<ProductDto>> Error()
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(100);
            var product = await _productRepository.GetEntityWithSpecification(specification);
            var name = product.ToString();

            return Ok();
        }

        [HttpGet("error500")]
        public async Task<ActionResult<ProductDto>> Error500()
        {
            return StatusCode(500, "Internal Server Error Custom Message");
        }

        [HttpGet("error400")]
        public async Task<ActionResult<ProductDto>> Error400()
        {
            return BadRequest("Unique Bad Request");
        }

        [HttpGet("error400/multi")]
        public async Task<ActionResult<List<string>>> Error400Multi()
        {
            var badRequests = new List<string> { "Bad Request 1", "Bad Request 2", "Bad Request 3" };
            return BadRequest(badRequests);
        }


        ///////////////////// TO REMOVE


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepository.GetEntityWithSpecification(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepository.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await _productTypeRepository.ListAllAsync());
        }
    }
}
