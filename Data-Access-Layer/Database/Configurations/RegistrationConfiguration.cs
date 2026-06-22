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
                  .OnDelete(DeleteBehavior.Cascade);

            options.HasOne(e => e.Event)
                   .WithMany(r => r.Registrations)
                   .HasForeignKey(r => r.EventId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
