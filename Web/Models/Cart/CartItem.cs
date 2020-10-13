using System.ComponentModel.DataAnnotations.Schema;
using Web.Models.Identity;
using Web.Models.Products;

namespace Web.Models.Cart
{
    public class CartItem
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}
