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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> options)
        {
            options.HasKey(p => p.Id);

            options.Property(p => p.Amount)
                   .HasColumnType("decimal(10,2)")
            .IsRequired();

            options.Property(p => p.Status)
            .IsRequired();

            options.Property(p => p.StripeSessionId)
                   .HasMaxLength(200);

            options.Property(p => p.StripePaymentIntentId)
                   .HasMaxLength(200);

            options.HasOne(p => p.Order)
                   .WithOne()
                   .HasForeignKey<Payment>(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            options.HasIndex(p => p.OrderId)
            .IsUnique();

        }
    }
}
