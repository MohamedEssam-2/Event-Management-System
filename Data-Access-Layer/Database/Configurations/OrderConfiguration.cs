using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enum;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_Access_Layer.Database.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Amount)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(o => o.Status)
                   .IsRequired()
                   .HasDefaultValue(OrderStatus.Pending);

            builder.Property(o => o.OrderDate)
                   .HasDefaultValueSql("GETDATE()");

            // User -> Orders
            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Event -> Orders
            builder.HasOne(o => o.Event)
                   .WithMany(e => e.Orders)
                   .HasForeignKey(o => o.EventId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.EventId);

        }
    }
}
