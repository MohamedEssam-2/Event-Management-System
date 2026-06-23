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
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> options)
        {
            options.HasKey(options => options.Id);

            options.Property(options => options.Name).IsRequired().HasMaxLength(200);

            options.Property(e => e.Location).IsRequired().HasMaxLength(300);

            options.Property(p=>p.Price).HasColumnType("decimal(10,2)").HasDefaultValue(0m);


            options.HasOne(c=>c.Category)
                   .WithMany(e => e.Events)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);


            options.HasOne(o=>o.Organizer)
                .WithMany(e => e.Events)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);


            options.Property(x => x.CreatedAt)
       .HasDefaultValueSql("GETDATE()");

            options.Property(x => x.UpdatedAt)
                   .IsRequired(false);

            options.Property(x => x.DeletedDate)
                   .IsRequired(false);

            options.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            options.HasIndex(e => e.CategoryId);
            options.HasIndex(e => e.OrganizerId);

        }
    }
}
