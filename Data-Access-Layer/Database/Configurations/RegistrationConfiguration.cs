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
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> options)
        {
           options.HasOne(u=>u.User)
                  .WithMany(r => r.Registrations)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            options.HasOne(e => e.Event)
                   .WithMany(r => r.Registrations)
                   .HasForeignKey(r => r.EventId)
                   .OnDelete(DeleteBehavior.Restrict);

           options.HasKey(r => r.Id);

            options.HasIndex(r => new { r.UserId, r.EventId })
                   .IsUnique();

            options.Property(r => r.RegistrationDate)
                   .HasDefaultValueSql("GETDATE()");



            options.Property(x => x.CreatedAt)
       .HasDefaultValueSql("GETDATE()");

            options.Property(x => x.UpdatedAt)
                   .IsRequired(false);

            options.Property(x => x.DeletedDate)
                   .IsRequired(false);

            options.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            options.HasIndex(r => r.UserId);
            options.HasIndex(r => r.EventId);
        }
    }
}
