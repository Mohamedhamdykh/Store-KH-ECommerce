
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.KH.APIs.Erorrs;
using Store.KH.APIs.Helper;
using Store.KH.APIs.MiddleWares;
using Store.KH.Core;
using Store.KH.Core.Mapping.Products;
using Store.KH.Core.Services.Contract;
using Store.KH.Repository;
using Store.KH.Repository.Data;
using Store.KH.Repository.Data.Contexts;
using Store.KH.Service.Services.Products;

namespace Store.KH.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewaresAsync();

            app.Run();
        }
    }
}
