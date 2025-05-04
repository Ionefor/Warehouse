using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehourse.ProductPlacement.Infrastructure.Configurations.Write;

public class ProductStorageConfiguration : IEntityTypeConfiguration<ProductStorage>
{
    public void Configure(EntityTypeBuilder<ProductStorage> builder)
    {
        builder.ToTable("product_storages");

        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Id).HasConversion(
            id => id.Id,
            value => ProductStorageId.Create(value));
        
        builder.Property(w => w.ProductId).HasConversion(
            id => id.Id,
            value => ProductId.Create(value));
        
        builder.Property(w => w.WarehouseId).HasConversion(
            id => id.Id,
            value => WarehouseId.Create(value));
        
        builder.Property(w => w.SectionId).HasConversion(
            id => id.Id,
            value => SectionId.Create(value));
        
        builder.ComplexProperty(l => l.ShelfPosition,
            lb =>
            {
                lb.Property(c => c.SectionRowNumber).IsRequired().
                    HasColumnName("shelf_position_section_row_number");
                
                lb.Property(c => c.ShelfRowNumber).IsRequired().
                    HasColumnName("shelf_position_shelf_row_number");
                
                lb.Property(c => c.ShelfColumnNumber).IsRequired().
                    HasColumnName("shelf_position_shelf_column_number");
            });
    }
}