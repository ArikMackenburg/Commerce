using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Models.Identity;

namespace Web.Components
{
    public class CartComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CartComponent(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            var cart = await db.CartItems.CountAsync(c => c.UserId == userId && c.Quantity > 0);

            return View( new CartViewModel
            {
                CartItems = cart
            });
        }
        

        
    }
    public class CartViewModel
    {
        public int CartItems { get; set; }
    }
}
