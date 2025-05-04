using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Domain.Entities;

namespace Warehouse.WarehouseManagement.Infrastructure.Configurations.Write;

public class WarehouseConfiguration : IEntityTypeConfiguration<Domain.Aggregate.Warehouse>
{
    public void Configure(EntityTypeBuilder<Domain.Aggregate.Warehouse> builder)
    {
        builder.ToTable("warehouses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasConversion(
            id => id.Id,
            value => WarehouseId.Create(value));
        
        builder.HasMany(w => w.Sections).
            WithOne().
            HasForeignKey("warehouse_id").
            OnDelete(DeleteBehavior.Cascade).
            IsRequired();
        
        builder.ComplexProperty(w => w.Name,
            wb =>
            {
                wb.Property(c => c.Value).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("name");
            });
        
        builder.ComplexProperty(w => w.NotificationEmail,
            wb =>
            {
                wb.Property(c => c.Value).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("notification_email");
            });
        
        builder.ComplexProperty(w => w.Size,
            wb =>
            {
                wb.Property(c => c.Length).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("length");
                
                wb.Property(c => c.Width).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("width");
                
                wb.Property(c => c.Height).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("height");
            });
    }
}