using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_Access_Layer.Database.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> options)
        {
            options.HasKey(options => options.Id);
           
            options.Property(options => options.Name).IsRequired().HasMaxLength(200);

            options.Property(x => x.CreatedAt)
       .HasDefaultValueSql("GETDATE()");

            options.Property(x => x.UpdatedAt)
                   .IsRequired(false);

            options.Property(x => x.DeletedDate)
                   .IsRequired(false);

            options.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);


        }
    }
}
