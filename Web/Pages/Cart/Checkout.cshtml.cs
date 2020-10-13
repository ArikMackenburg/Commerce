using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web.Models.Cart;
using Web.Models.Checkout;
using Web.Models.Identity;
using Web.Models.Orders;

namespace Web.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly Web.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public CheckoutModel(Web.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;

        }
        public decimal CartTotal { get; set; }
        public IList<CartItem> CartItems { get; set; }
        public async Task OnGetAsync()
        {
            CartItems = await _context.CartItems
                .Where(c => c.UserId == userManager.GetUserId(User))
                .Where(c => c.Quantity > 0)
                .Include(c => c.Product).ToListAsync();

            decimal total = 0;
            foreach (var item in CartItems)
            {
                decimal itemPrice = item.Product.Price * Convert.ToDecimal(item.Quantity);
                total = total + itemPrice;
            }
            CartTotal = total;
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            CartItems = await _context.CartItems
                .Where(c => c.UserId == userManager.GetUserId(User))
                .Where(c => c.Quantity > 0)
                .Include(c => c.Product).ToListAsync();

            decimal total = 0;
            foreach (var item in CartItems)
            {
                decimal itemPrice = item.Product.Price * Convert.ToDecimal(item.Quantity);
                total = total + itemPrice;
            }
            CartTotal = total;
            
            

            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    UserId = userManager.GetUserId(User),
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Address = Input.Address,
                    City = Input.City,
                    State = Input.State,
                    PostalCode = Input.PostalCode,
                    Country = Input.Country,
                    Phone = Input.Phone,
                    Email = Input.Email,
                    Total = CartTotal,
                    Items = CartItems,
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var userId = userManager.GetUserId(User);

                foreach (var cartItem in CartItems)
                {


                    var existingCartItem = await _context.CartItems
                        .FirstOrDefaultAsync(c =>
                            c.ProductId == cartItem.ProductId &&
                            c.UserId == userId
                        );

                    existingCartItem.Quantity = 0;
                    

                    await _context.SaveChangesAsync();
                }

                
                return LocalRedirect("~/Index");
            }
            return Page();
        }
      
        [BindProperty]
        public CheckoutInput Input { get; set; }

        public class CheckoutInput
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public State State { get; set; }
            [Required]
            public string PostalCode { get; set; }
            [Required]
            public string Country { get; set; }
            [Required]
            [Phone]
            public string Phone { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }

        }
    }
}
