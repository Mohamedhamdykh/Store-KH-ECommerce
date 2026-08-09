using Microsoft.AspNetCore.Identity;
using Store.KH.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Repository.Identity
{
    public class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
           if(_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "mokhalifa4568@gmail.com",
                    DiplayName = "Mohamed Hamdy",
                    UserName = "Khalifa",
                    PhoneNumber = "01062517324",
                    Address = new Address()
                    {
                        FName = "Mohamed",
                        LName = "Hamdy",
                        City = "Cairo",
                        Country = "Egypt",
                        Street = "Elmansora"
                    }
                };

                await _userManager.CreateAsync(user, "P@ssW0rd");
            }
        }
    }
}
