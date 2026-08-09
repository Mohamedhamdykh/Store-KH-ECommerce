using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DiplayName { get; set; }
        public Address Address { get; set; }
    }
}
