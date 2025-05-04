using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Infrastructure.Configurations.Read;

public class WarehouseDtoConfiguration : IEntityTypeConfiguration<WarehouseDto>
{
    public void Configure(EntityTypeBuilder<WarehouseDto> builder)
    {
        builder.ToTable("warehouses");

        builder.HasKey(a => a.Id);
        
        builder.HasMany(v => v.Sections).
            WithOne().
            HasForeignKey(v => v.WarehouseId);
        
        builder.Property(v => v.Name);
        
        builder.Property(v => v.Email);
        
        builder.ComplexProperty(v => v.Size, sb =>
        {
            sb.Property(v => v.Length);
            
            sb.Property(v => v.Width);
            
            sb.Property(v => v.Height);
        });
    }
}