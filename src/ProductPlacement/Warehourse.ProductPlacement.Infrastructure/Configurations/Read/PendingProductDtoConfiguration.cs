using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehourse.ProductPlacement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Infrastructure.Configurations.Read;

public class PendingProductDtoConfiguration : IEntityTypeConfiguration<PendingProductDto>
{
    public void Configure(EntityTypeBuilder<PendingProductDto> builder)
    {
        builder.ToTable("pending_products");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name);

        builder.Property(c => c.Description);
        
        builder.Property(c => c.Category);
        
        builder.ComplexProperty(c => c.Size, sb =>
        {
            sb.Property(s => s.Length);
            
            sb.Property(s => s.Width);
            
            sb.Property(s => s.Height);
        });
    }
}