using Store.KH.Core.Dtos.Products;
using Store.KH.Core.Entities;
using Store.KH.Core.Helper;
using Store.KH.Core.Specification.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Services.Contract
{
    public interface IProductServices
    {
        Task<PaginationResponse<ProductDto>> GetAllProductAsync(ProductSpecParams productSpec);
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
