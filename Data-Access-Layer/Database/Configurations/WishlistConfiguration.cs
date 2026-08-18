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
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(w => w.Id);

            builder.HasOne(w => w.User)
                   .WithMany(u => u.Wishlists)
                   .HasForeignKey(w => w.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Event)
                   .WithMany(e => e.Wishlists)
                   .HasForeignKey(w => w.EventId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(w => new { w.UserId, w.EventId })
                   .IsUnique();

            builder.Property(w => w.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(w => w.IsDeleted)
                   .HasDefaultValue(false);

            builder.Property(w => w.DeletedDate)
                   .IsRequired(false);
        }
    }
}
