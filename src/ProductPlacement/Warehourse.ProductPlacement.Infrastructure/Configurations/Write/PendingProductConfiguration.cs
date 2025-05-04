using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehourse.ProductPlacement.Infrastructure.Configurations.Write;

public class PendingProductConfiguration : IEntityTypeConfiguration<PendingProduct>
{
    public void Configure(EntityTypeBuilder<PendingProduct> builder)
    {
        builder.ToTable("pending_products");

        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Id).HasConversion(
            id => id.Id,
            value => ProductId.Create(value));
        
        builder.ComplexProperty(l => l.Name,
            lb =>
            {
                lb.Property(c => c.Value).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("name");
            });
        
        builder.ComplexProperty(l => l.Description,
            lb =>
            {
                lb.Property(c => c.Value).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("description");
            });
        
        builder.ComplexProperty(l => l.Category,
            lb =>
            {
                lb.Property(c => c.Value).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("category");
            });
        
        builder.ComplexProperty(l => l.Size,
            lb =>
            {
                lb.Property(c => c.Length).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("size_length");
                
                lb.Property(c => c.Width).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("size_width");
                
                lb.Property(c => c.Height).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("size_height");
            });
    }
}