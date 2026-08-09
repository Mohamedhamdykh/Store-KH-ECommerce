using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.KH.APIs.Attributes;
using Store.KH.APIs.Erorrs;
using Store.KH.Core.Dtos.Products;
using Store.KH.Core.Helper;
using Store.KH.Core.Services.Contract;
using Store.KH.Core.Specification.Products;

namespace Store.KH.APIs.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet] // Get UrlBase/Api/Products
        [Cached(100)]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec)//EndPoint
        {
            var result = await _productServices.GetAllProductAsync(productSpec);
            return Ok(result);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("brands")]// Get UrlBase/Api/Products/brands
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllBrands()
        {
           var result = await _productServices.GetAllBrandsAsync();
            return Ok(result);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")]// Get UrlBase/Api/Products/types
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var result = await _productServices.GetAllTypesAsync();
            return Ok(result);
        }

        [ProducesResponseType(typeof(TypeBrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));
           var result = await  _productServices.GetProductByIdAsync(id.Value);
            if (result is null) return NotFound(new ApiErrorResponse(404, $"The Product With Id : {id}  Not Found At DB :("));
            return Ok(result);
        }
    }
}
