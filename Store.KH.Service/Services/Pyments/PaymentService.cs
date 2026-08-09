using Microsoft.Extensions.Configuration;
using Store.KH.Core;
using Store.KH.Core.Dtos.Basket;
using Store.KH.Core.Entities;
using Store.KH.Core.Entities.Order;
using Store.KH.Core.Services.Contract;
using Store.KH.Core.Specification.Orders;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.KH.Core.Entities.Product;

namespace Store.KH.Service.Services.Pyments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService,IUnitOfWork unitOfWork , IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }
            if (basket.items.Count() > 0)
            {
                foreach (var item in basket.items)
                {
                   var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            var supTotal = basket.items.Sum(I => I.Price * I.Quantity);

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(supTotal * 100 + shippingPrice * 100),
                    PaymentMethodTypes = new List<string>() { "card"},
                    Currency = "usd"
                };
               paymentIntent =  await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(supTotal * 100 + shippingPrice * 100),
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
                

            }

            

            basket = await _basketService.UpdateBasketAsync(basket);

            
            if (basket is null) return null;
            
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderSpecificationWithPaymentIntentId(paymentIntentId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFaild;
            }

          _unitOfWork.Repository<Order,int>().UpdateAsync(order);
           await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
