using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.KH.APIs.Erorrs;
using Store.KH.Core;
using Store.KH.Core.Entities;
using Store.KH.Repository.Data.Contexts;

namespace Store.KH.APIs.Controllers
{
    
    public class BuggyController : BaseApiController
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public BuggyController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }
        [HttpGet("notFound")]
        public async Task<IActionResult> GetNotFoundRequestErorr()
        {
            var brand = await _unitOfWork.Repository<ProductBrand,int>().GetAsync(100);
            if(brand is null) return NotFound(new ApiErrorResponse(404 ));
            return Ok(brand);
        }
        [HttpGet("serverError")]
        public async Task<IActionResult> GetServerErorr()
        {
            var brand = await _unitOfWork.Repository<ProductBrand, int>().GetAsync(100);
            var brandToString = brand.ToString(); // Will Throw Exception (Null Reference Exception)
            return Ok(brand);
        }
        [HttpGet("badRequest")]
        public async Task<IActionResult> GetBadRequestErorr()
        {
            return BadRequest(new ApiErrorResponse(400));
        }
        [HttpGet("badRequest/{id}")]
        public async Task<IActionResult> GetBadRequestErorr(int id) //Validayion Erorr
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResponse(400));
            }
            return Ok();
        }
        [HttpGet("unauthorized")]
        public async Task<IActionResult> GetUnauthorizedErorr(int id) //Validayion Erorr
        {
            return Unauthorized(new ApiErrorResponse(401));
        }
    }
}
