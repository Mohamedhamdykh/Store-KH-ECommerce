using Store.KH.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Specification.Orders
{
    public class OrderSpecificationWithPaymentIntentId : BaseSpecification<Order,int>
    {
        public OrderSpecificationWithPaymentIntentId(string paymentIntentId) : base(O => O.PaymentIntentId == paymentIntentId)
        {
            Include.Add(O => O.DeliveryMethod);
            Include.Add(O => O.Items);
        }
    }
}
