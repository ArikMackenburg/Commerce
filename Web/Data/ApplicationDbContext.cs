using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Cart;
using Web.Models.Checkout;
using Web.Models.Identity;
using Web.Models.Orders;
using Web.Models.Products;

namespace Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            SeedRole(builder, "Admin", "get", "create", "update", "delete");
            SeedRole(builder, "User");

            builder.Entity<CartItem>()
                .HasKey(cartItem => new
                {
                    cartItem.UserId,
                    cartItem.ProductId,
                });
           
        }

        private int nextRoleClaimId = 1;

        private void SeedRole(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            var role = new ApplicationRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString(),
            };
            modelBuilder.Entity<ApplicationRole>()
                .HasData(role);

            var roleClaims = permissions
                .Select(permission =>
                    new IdentityRoleClaim<string>
                    {
                        Id = nextRoleClaimId++,
                        RoleId = role.Id,
                        ClaimType = "permissions",
                        ClaimValue = permission,
                    })
                .ToArray();

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .HasData(roleClaims);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
