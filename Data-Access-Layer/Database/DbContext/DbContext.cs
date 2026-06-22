using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Data_Access_Layer.Database.DbContext
{
    public class DbContext :IdentityDbContext<ApplicationUser>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Event> Events { get; set; } = null!;

        public DbSet<Registration> Registrations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
        }
    }
}
