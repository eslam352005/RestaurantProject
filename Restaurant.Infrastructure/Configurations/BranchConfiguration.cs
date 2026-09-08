using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Infrastructure.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Address)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(b => b.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.IsActive)
                .HasDefaultValue(true);

            builder.HasMany(b => b.Tables)
                .WithOne(t => t.Branch)
                .HasForeignKey(t => t.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Staff)
                .WithOne(s => s.Branch)
                .HasForeignKey(s => s.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.InventoryItems)
                .WithOne(i => i.Branch)
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Orders)
                .WithOne(o => o.Branch)
                .HasForeignKey(o => o.BranchId)
                .OnDelete(DeleteBehavior.Restrict);


            // BranchConfiguration.cs - داخل Configure()
            builder.HasData(
                new Branch { Id = 1, Name = "فرع المنصورة", Address = "شارع الجمهورية", Phone = "01000000001", IsActive = true },
                new Branch { Id = 2, Name = "فرع القاهرة", Address = "مدينة نصر", Phone = "01000000002", IsActive = true }
            );
        }
    }
}
