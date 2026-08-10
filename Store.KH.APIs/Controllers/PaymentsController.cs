using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.KH.APIs.Erorrs;
using Store.KH.Core.Services.Contract;
using Stripe;

namespace Store.KH.APIs.Controllers
{
   
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        const string endPointSecret = "whsec_SgrYOMb5V0bmlGwk797kZFzwOG8CJHtG";

        public PaymentsController(IPaymentService paymentService)
        {
           _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            if(basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var basket = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);
            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
        }


        [HttpPost("webHook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {

                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endPointSecret);
                Console.WriteLine($"Event Type: {stripeEvent.Type}");
                Console.WriteLine($"Object Type: {stripeEvent.Data.Object?.GetType().FullName}");
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                if(stripeEvent.Type  == "payment_intent.payment_failed")
                {
                    //UpdateDatabase
                   await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, false);
                }
                else if(stripeEvent.Type == "payment_intent.succeeded")
                {
                    await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, true);

                }
                else
                {
                    Console.WriteLine("Unhandeld event type : {0}",stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest(e.Message);
            }
        }
    }
}
