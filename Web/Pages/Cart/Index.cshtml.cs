using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Cart;
using Web.Models.Identity;

namespace Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly Web.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public IndexModel(Web.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;

        }
        public decimal CartTotal { get; set; }
        public IList<CartItem> CartItems { get;set; }

        public async Task OnGetAsync()
        {
            CartItems = await _context.CartItems
                .Where(c => c.UserId == userManager.GetUserId(User))
                .Where(c => c.Quantity > 0)
                .Include(c => c.Product).ToListAsync();

            decimal total = 0;
            foreach(var item in CartItems)
            {
                decimal itemPrice = item.Product.Price * Convert.ToDecimal(item.Quantity);
                total = total + itemPrice;
            }
            CartTotal = total;
        }

        public async Task<IActionResult> RemoveCartItem(int id)
        {
            var existingCartItem = await _context.CartItems
                       .FirstOrDefaultAsync(c =>
                           c.ProductId == id &&
                           c.UserId == userManager.GetUserId(User)
                       );
            existingCartItem.Quantity = 0;
            await _context.SaveChangesAsync();

            return LocalRedirect("~/Index");
        }
    }
}
