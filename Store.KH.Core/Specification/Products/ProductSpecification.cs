using Store.KH.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Specification.Products
{
    public class ProductSpecification : BaseSpecification<Product , int>
    {
        public ProductSpecification(int id) : base(P => P.Id ==id)
        {
            ApplyIncludes();
        }
        public ProductSpecification(ProductSpecParams productSpec) : base(
            P =>
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
            &&
            (!productSpec.BrandId.HasValue || productSpec.BrandId == P.ProductBrandId)
            &&
            (!productSpec.TypeId.HasValue || productSpec.TypeId == P.ProductTypeId)
            
            )
        {
            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price); 
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

            ApplyIncludes();
            ApplyPagination(productSpec.PageSize *(productSpec.PageIndex - 1),productSpec.PageSize);

        }

        private void ApplyIncludes()
        {
            Include.Add(P => P.ProductBrand);
            Include.Add(P => P.ProductType);
        }
    }
}
