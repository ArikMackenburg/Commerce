using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Web.Models.Orders;

namespace Web.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public List<Order> Orders { get; set; }
    }
}
