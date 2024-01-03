using API.DTOs;
using API.Errors;
using AutoMapper;
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
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts ([FromQuery] ProductSpecificationParams productParams)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(productParams);

            var products = await _productRepository.ListAsync(specification);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products));
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

        ///////////////////// TO REMOVE


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(id);

            var product =  await _productRepository.GetEntityWithSpecification(specification);

            if (product == null) 
                return NotFound(new ApiResponse(NotFound().StatusCode));

            return _mapper.Map<Product, ProductDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepository.ListAllAsync());
        }        
        
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepository.ListAllAsync());
        }
    }
}
