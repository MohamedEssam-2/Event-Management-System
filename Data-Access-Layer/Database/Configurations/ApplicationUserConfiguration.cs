using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Data_Access_Layer.Database.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> options)
        {
            options.Property(u => u.FullName).IsRequired().HasMaxLength(400);
            options.Property(u => u.Age).IsRequired(false);
            options.Property(u => u.Phone_Number).IsRequired(false).HasMaxLength(20);



        }
    }
}
