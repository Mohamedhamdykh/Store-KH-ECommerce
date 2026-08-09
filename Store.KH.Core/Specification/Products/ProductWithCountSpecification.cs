using Store.KH.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Specification.Products
{
    public class ProductWithCountSpecification : BaseSpecification<Product , int>
    {
        public ProductWithCountSpecification(ProductSpecParams productSpec) 
            : base
            (
                    P =>
                     (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
                     &&
                     (!productSpec.BrandId.HasValue || productSpec.BrandId == P.ProductBrandId)
                     &&
                     (!productSpec.TypeId.HasValue || productSpec.TypeId == P.ProductTypeId)

            )
        {
           

        }
    }
}
