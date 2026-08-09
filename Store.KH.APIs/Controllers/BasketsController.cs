using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.KH.APIs.Erorrs;
using Store.KH.Core.Dtos.Basket;
using Store.KH.Core.Entities;
using Store.KH.Core.Repositories.Contract;
using Store.KH.Core.Services.Contract;

namespace Store.KH.APIs.Controllers
{
    
    public class BasketsController : BaseApiController
    {
        
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            
            _basketService = basketService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketById(string? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400, "Invalid Id !"));
            var basket = await _basketService.GetBasketAsync(id);
            if (basket is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(basket);
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string? id)
        {
            if (id is null)
                return BadRequest(new ApiErrorResponse(400, "Invalid Id !"));

            var basket = await _basketService.GetBasketAsync(id);

            if (basket is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(basket);
        }



        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasket(CustomerBasketDto? model)
        {
            if (model is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var basket = await _basketService.UpdateBasketAsync(model);
            if (basket is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(basket);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string? id)  
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var flag = await _basketService.DeleteBasketAsync(id);
            
             if(flag is false) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return NoContent();
        }
        



    }
}
