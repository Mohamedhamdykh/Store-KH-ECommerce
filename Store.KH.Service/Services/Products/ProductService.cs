using AutoMapper;
using Store.KH.Core;
using Store.KH.Core.Dtos.Products;
using Store.KH.Core.Entities;
using Store.KH.Core.Helper;
using Store.KH.Core.Services.Contract;
using Store.KH.Core.Specification.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Service.Services.Products
{
    public class ProductService : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<ProductDto>> GetAllProductAsync(ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);
            var product = await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);
            var mapperProduct = _mapper.Map<IEnumerable<ProductDto>>(product);
            var countSpec = new ProductWithCountSpecification(productSpec);
            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);
            return new PaginationResponse<ProductDto>(productSpec.PageSize, productSpec.PageIndex, count, mapperProduct);
        } 
        
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecification(id);

            return _mapper.Map<ProductDto>(await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec));
           
        }
        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
         => _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType , int>().GetAllAsync());
        

        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
        {
           var brands = await _unitOfWork.Repository<ProductBrand , int>().GetAllAsync();
            var mappedBrands = _mapper.Map<IEnumerable<TypeBrandDto>>(brands);
            return mappedBrands;
        }


    }
}
