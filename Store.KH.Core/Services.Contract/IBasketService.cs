using Store.KH.Core.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Services.Contract
{
    public interface IBasketService
    {
         Task<CustomerBasketDto?> GetBasketAsync(string basketId);
         Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto basketDto);
         Task<bool?> DeleteBasketAsync(string basketId);
    }
}
