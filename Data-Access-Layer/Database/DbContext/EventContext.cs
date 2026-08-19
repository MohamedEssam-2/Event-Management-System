using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Data_Access_Layer.Database
{
    public class EventContext :IdentityDbContext<ApplicationUser>
    {
        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Event> Events { get; set; } = null!;

        public DbSet<Registration> Registrations { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Order> Orders { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(EventContext).Assembly);

            builder.Entity<Event>().HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Registration>().HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Order>().HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
