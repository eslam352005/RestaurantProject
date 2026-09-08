using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Infrastructure.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.Description)
                .HasMaxLength(500);

            builder.Property(m => m.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(m => m.ImageUrl)
                .HasMaxLength(300);

            builder.Property(m => m.IsAvailable)
                .HasDefaultValue(true);

            builder.HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Branch)
                .WithMany()
                .HasForeignKey(m => m.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // MenuItemConfiguration.cs
            builder.HasData(
                new MenuItem { Id = 1, Name = "بطاطس محمرة", Price = 45, CategoryId = 1, BranchId = 1, IsAvailable = true, Description = "بطاطس مقرمشة" },
                new MenuItem { Id = 2, Name = "برجر لحمة", Price = 120, CategoryId = 2, BranchId = 1, IsAvailable = true, Description = "برجر 200 جرام" },
                new MenuItem { Id = 3, Name = "عصير مانجة", Price = 35, CategoryId = 3, BranchId = 1, IsAvailable = true, Description = "طازة" }
            );
        }
    }
}
