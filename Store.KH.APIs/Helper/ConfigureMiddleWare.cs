using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.KH.APIs.MiddleWares;
using Store.KH.Core.Entities.Identity;
using Store.KH.Repository.Data;
using Store.KH.Repository.Data.Contexts;
using Store.KH.Repository.Identity;
using Store.KH.Repository.Identity.Contexts;

namespace Store.KH.APIs.Helper
{
    public static class ConfigureMiddleWare
    {
        public static async Task<WebApplication> ConfigureMiddlewaresAsync(this WebApplication app)
        {
            #region UpdateDatabase
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreDbContext>();
            var contextIdentity = services.GetRequiredService<StoreIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);

                await contextIdentity.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There Are Problems During Apply Migrations !!");
            }
            #endregion

            app.UseMiddleware<ExceptionMiddleWare>(); //Configure User-Defined [ExceptionMiddleWare] MiddleWare

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

           return app;
        }
    }
}
