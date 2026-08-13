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
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> options)
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.Token)
                .IsRequired();

            options.HasIndex(x => x.Token)
                .IsUnique();

            options.Property(x => x.CreatedAt)
                 .HasDefaultValueSql("GETDATE()");

            options.Property(x => x.ExpiresAt)
                .IsRequired();

            options.Property(x => x.IsRevoked)
                .HasDefaultValue(false);

            options.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
