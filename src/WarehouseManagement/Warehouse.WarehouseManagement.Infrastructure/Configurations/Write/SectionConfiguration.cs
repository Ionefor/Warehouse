using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Core.Dtos;
using Warehouse.Core.Extensions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Contracts.Dtos;
using Warehouse.WarehouseManagement.Domain.Entities;
using Warehouse.WarehouseManagement.Domain.ValueObjects;

namespace Warehouse.WarehouseManagement.Infrastructure.Configurations.Write;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("sections");

        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Id).HasConversion(
            id => id.Id,
            value => SectionId.Create(value));
        
        builder.ComplexProperty(l => l.Size,
            lb =>
            {
                lb.Property(c => c.Length).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("length");
                
                lb.Property(c => c.Width).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("width");
                
                lb.Property(c => c.Height).IsRequired().
                    HasMaxLength(Constants.Shared.MaxLowTextLength)
                    .HasColumnName("height");
            });
        
        builder.OwnsOne(s => s.Rows, sb =>
        {
            sb.Property(ph => ph.Values)!
                .ValueObjectsCollectionJsonConversion(
                    sectionRow => new SectionRowDto(
                        new SizeDto(
                            sectionRow.Size.Length,
                            sectionRow.Size.Width,
                            sectionRow.Size.Height),
                        sectionRow.Number,
                        sectionRow.Shelfs.Values!.Select(s => new ShelfDto(
                            s.IsAvailable,
                            new SizeDto(s.Size.Length, s.Size.Width, s.Size.Height),
                            s.Row,
                            s.Column
                        ))
                    ),
                    dto => SectionRow.Create(
                        Size.Create(
                            dto.Size.Length,
                            dto.Size.Width,
                            dto.Size.Height).Value,
                        new Shelfs(dto.Shelfs.Select(s => Shelf.Create(
                            s.IsAvailable,
                            Size.Create(s.Size.Length, s.Size.Width, s.Size.Height).Value,
                            s.Row,
                            s.Column
                        ).Value)),
                        dto.Number
                    ).Value
                )
                .HasColumnName("section_rows");
        });
        
        builder.Property(u => u.Type)
            .HasConversion<string>(); 
    }
}