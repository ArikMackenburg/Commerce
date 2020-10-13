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
using Web.Models.Products;

namespace Web.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly Web.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public DetailsModel(Web.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);

                var existingCartItem = await _context.CartItems
                    .FirstOrDefaultAsync(c =>
                        c.ProductId == Input.ProductId &&
                        c.UserId == userId
                    );

                if (existingCartItem == null)
                {
                    var item = new CartItem()
                    {
                        UserId = userId,
                        ProductId = Input.ProductId,
                        Quantity = Input.Quantity,
                    };
                    _context.CartItems.Add(item);
                }
                else
                {
                    existingCartItem.Quantity += Input.Quantity;
                }

                await _context.SaveChangesAsync();
                return LocalRedirect("/Products/Index");
            }

            return Page();
        }

        [BindProperty]
        public ProductInput Input { get; set; } = new ProductInput { Quantity = 1 };

        public class ProductInput
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
