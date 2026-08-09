using Store.KH.Core;
using Store.KH.Core.Entities;
using Store.KH.Core.Entities.Order;
using Store.KH.Core.Services.Contract;
using Store.KH.Core.Specification.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork ,IBasketService basketService, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _paymentService = paymentService;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
           var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;
            var orderItems = new List<OrderItem>();

            if (basket.items.Count() > 0) 
            {
                foreach (var item in basket.items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    var productOrderedItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productOrderedItem,product.Price,item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            var delivaryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

            var supTotal = orderItems.Sum(I => I.Price * I.Quantity);


            //TODO
            //if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            //{
            //   var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
            //    var ExOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            //    _unitOfWork.Repository<Order, int>().DeleteAsync(ExOrder);
            //}
            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);

                var ExOrder = await _unitOfWork.Repository<Order, int>()
                    .GetWithSpecAsync(spec);

                if (ExOrder != null)
                {
                    _unitOfWork.Repository<Order, int>().DeleteAsync(ExOrder);
                }
            }

            var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);
            basket.PaymentIntentId = basketDto.PaymentIntentId;

            Console.WriteLine("ORDER PaymentIntent = " + basketDto.PaymentIntentId);

            var order = new Order(buyerEmail, shippingAddress, delivaryMethod , orderItems, supTotal,basketDto.PaymentIntentId);

            await _unitOfWork.Repository<Order, int>().AddAsync(order);

           var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }

        public async Task<Order>? GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification(buyerEmail, orderId);
           var order = await _unitOfWork.Repository<Order,int>().GetWithSpecAsync(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if (orders is null) return null;
            return orders;
        }
    }
}
