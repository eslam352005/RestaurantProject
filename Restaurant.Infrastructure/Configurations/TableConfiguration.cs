using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Infrastructure.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable("Tables");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.TableNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Capacity)
                .IsRequired();

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(TableStatus.Available);

            builder.HasOne(t => t.Branch)
                .WithMany(b => b.Tables)
                .HasForeignKey(t => t.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Orders)
                .WithOne(o => o.Table)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            // TableConfiguration.cs
            builder.HasData(
                new Table { Id = 1, TableNumber = "T1", Capacity = 4, Status = TableStatus.Available, BranchId = 1 },
                new Table { Id = 2, TableNumber = "T2", Capacity = 2, Status = TableStatus.Available, BranchId = 1 },
                new Table { Id = 3, TableNumber = "T3", Capacity = 6, Status = TableStatus.Available, BranchId = 1 }
            );
        }
    }
}
