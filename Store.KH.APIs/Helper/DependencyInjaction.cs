using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Store.KH.APIs.Erorrs;
using Store.KH.Core;
using Store.KH.Core.Entities.Identity;
using Store.KH.Core.Mapping.Auth;
using Store.KH.Core.Mapping.Basket;
using Store.KH.Core.Mapping.Orders;
using Store.KH.Core.Mapping.Products;
using Store.KH.Core.Repositories.Contract;
using Store.KH.Core.Services.Contract;
using Store.KH.Repository;
using Store.KH.Repository.Data.Contexts;
using Store.KH.Repository.Identity.Contexts;
using Store.KH.Repository.Repositories;
using Store.KH.Service.Services.Baskets;
using Store.KH.Service.Services.Caches;
using Store.KH.Service.Services.Orders;
using Store.KH.Service.Services.Products;
using Store.KH.Service.Services.Pyments;
using Store.KH.Service.Services.Tokens;
using Store.KH.Service.Services.Users;
using System.Text;

namespace Store.KH.APIs.Helper
{
    public static class DependencyInjaction
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices(configuration);
            services.AddSwaggerServices();
            services.AddDbContextServices(configuration);
            services.AddUserDefinedServices();
            services.AddAutoMapperServices(configuration);
            services.ConfigureInvalidModelStateResponseServices();
            services.AddRedisServices(configuration);
            services.AddIdentityServices();
            services.AddAuthenticationServices(configuration);
            return services;
        }
        private static IServiceCollection AddBuiltInServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",config =>
                {
                    config.AllowAnyHeader();
                    config.AllowAnyMethod();
                    config.WithOrigins(configuration["FrontEndBaseUrl"]);
                });
            });

            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            return services;
        }
        private static IServiceCollection AddUserDefinedServices(this IServiceCollection services)
        {
            services.AddScoped<IProductServices, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IPaymentService, PaymentService >();

            return services;
        }
        private static IServiceCollection AddAutoMapperServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            services.AddAutoMapper(M => M.AddProfile(new AuthProfile()));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile(configuration)));
            return services;
        }
        private static IServiceCollection ConfigureInvalidModelStateResponseServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errores = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                             .SelectMany(p => p.Value.Errors)
                                             .Select(E => E.ErrorMessage)
                                             .ToArray();
                    var response = new ApiValidationErorrResponse()
                    {
                        Erorrs = errores
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }

        //private static IServiceCollection AddRedisServices(this IServiceCollection services , IConfiguration configuration)
        //{
        //    services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
        //    {
        //       var connection = configuration.GetConnectionString("Redis");
        //        return ConnectionMultiplexer.Connect(connection);
        //    });
        //    return services;
        //}
        private static IServiceCollection AddRedisServices(
      this IServiceCollection services,
      IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var options = new ConfigurationOptions
                {
                    User = configuration["Redis:User"],
                    Password = configuration["Redis:Password"],
                    AbortOnConnectFail = false
                };

                options.EndPoints.Add(
                    configuration["Redis:Host"]!,
                    int.Parse(configuration["Redis:Port"]!)
                );

                return ConnectionMultiplexer.Connect(options);
            });

            return services;
        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser , IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }
        private static IServiceCollection AddAuthenticationServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issure"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            return services;
        }
    }
}
