using Microsoft.EntityFrameworkCore;
using Store.KH.Core.Entities;
using Store.KH.Core.Entities.Order;
using Store.KH.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.KH.Repository.Data
{
    public class StoreDbContextSeed
    {
        public async static Task SeedAsync(StoreDbContext _context)
        {
            //Brands
            if (_context.brands.Count() == 0)
            {
                //1.Data Read From Json File
                var brandsData = File.ReadAllText(@"..\Store.KH.Repository\Data\DataSeed\brands.json");
                //2.Convert Json String To List<T>
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                //Seed Data To DB
                if (brands is not null && brands.Count() > 0)
                {
                    await _context.brands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();

                   
                }
            }
            //Tybes
            if (_context.types.Count() == 0)
            {
                //1.Data Read From Json File
                var typesData = File.ReadAllText(@"..\Store.KH.Repository\Data\DataSeed\types.json");
                //2.Convert Json String To List<T>
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                //Seed Data To DB
                if (types is not null && types.Count() > 0)
                {
                    await _context.types.AddRangeAsync(types);
                    await _context.SaveChangesAsync();

                }
            }
            //Products
            if (_context.products.Count() == 0)
            {
                //1.Data Read From Json File
                var productsData = File.ReadAllText(@"..\Store.KH.Repository\Data\DataSeed\products.json");
                //2.Convert Json String To List<T>
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                //Seed Data To DB
                if (products is not null && products.Count() > 0)
                {
                    await _context.products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();

                }
            }

            if (_context.DeliveryMethods.Count() == 0)
            {
                //1.Data Read From Json File
                var deliveryData = File.ReadAllText(@"..\Store.KH.Repository\Data\DataSeed\delivery.json");
                //2.Convert Json String To List<T>
                var delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                     
                //Seed Data To DB
                if (delivery is not null && delivery.Count() > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(delivery);
                    await _context.SaveChangesAsync();

                }
            }

        }
    }
}
