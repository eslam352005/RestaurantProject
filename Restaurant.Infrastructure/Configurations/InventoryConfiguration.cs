using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Infrastructure.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ItemName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(i => i.QuantityAvailable)
                .HasColumnType("decimal(10,2)");

            builder.Property(i => i.Unit)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(i => i.MinimumThreshold)
                .HasColumnType("decimal(10,2)");

            builder.HasOne(i => i.Branch)
                .WithMany(b => b.InventoryItems)
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
