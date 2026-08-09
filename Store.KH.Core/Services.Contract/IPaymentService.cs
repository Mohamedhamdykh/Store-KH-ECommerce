using Store.KH.Core.Dtos.Basket;
using Store.KH.Core.Entities;
using Store.KH.Core.Entities.Order;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);

        Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId,bool flag);
    }
}
