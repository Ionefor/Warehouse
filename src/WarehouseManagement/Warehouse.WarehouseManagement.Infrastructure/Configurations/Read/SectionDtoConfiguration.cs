using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Infrastructure.Configurations.Read;

public class SectionDtoConfiguration : IEntityTypeConfiguration<FullSectionDto>
{
    public void Configure(EntityTypeBuilder<FullSectionDto> builder)
    {
        builder.ToTable("sections");

        builder.HasKey(w => w.Id);
        
        builder.Property(v => v.Type);
        
        builder.Property(v => v.WarehouseId);
        
        builder.ComplexProperty(v => v.Size, sb =>
        {
            sb.Property(v => v.Length);
            
            sb.Property(v => v.Width);
            
            sb.Property(v => v.Height);
        });
        
        builder.Property(i => i.SectionRows)
            .HasConversion(
                requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<SectionRowDto[]>(json, JsonSerializerOptions.Default)!);
    }
}