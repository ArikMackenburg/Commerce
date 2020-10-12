using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Products;

namespace Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly Web.Data.ApplicationDbContext _context;

        public IndexModel(Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync(string name)
        {
            Product = await _context.Products.OrderBy(p=>p.Manufacturer).ToListAsync();
            if(name != null)
            {
                var searchResult = Product.Where(p => p.Name.ToLower() == name.ToLower());
                if(searchResult.Count() == 0)
                {
                    searchResult = Product.Where(p => p.Manufacturer.ToLower() == name.ToLower());
                }
                Product = searchResult.ToList();
            }
        }

        
    }
}
