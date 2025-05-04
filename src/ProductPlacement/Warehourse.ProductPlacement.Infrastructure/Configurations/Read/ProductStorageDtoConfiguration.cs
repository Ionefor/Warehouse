using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehourse.ProductPlacement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Infrastructure.Configurations.Read;

public class ProductStorageDtoConfiguration : IEntityTypeConfiguration<ProductStorageDto>
{
    public void Configure(EntityTypeBuilder<ProductStorageDto> builder)
    {
        builder.ToTable("product_storages");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.ProductId);

        builder.Property(c => c.WarehouseId);
        
        builder.Property(c => c.SectionId);

        builder.ComplexProperty(c => c.ShelfPosition, sb =>
        {
            sb.Property(s => s.SectionRowNumber);
            
            sb.Property(s => s.ShelfColumnNumber);
            
            sb.Property(s => s.ShelfRowNumber);
        });
    }
}
