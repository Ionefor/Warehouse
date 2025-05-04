using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Domain.Entities;
using Warehouse.WarehouseManagement.Domain.ValueObjects;

namespace Warehouse.WarehouseManagement.Domain.Aggregate;

public class Warehouse : Entity<WarehouseId>
{
    private readonly List<Section> _sections = [];
    
    private Warehouse(WarehouseId id) : base(id) {}
    
    public Warehouse(
        WarehouseId id,
        Name name,
        Size size,
        List<Section> sections,
        Email email
    ) : base(id)
    {
        _sections = sections;
        Name = name;
        Size = size;
        NotificationEmail = email;
    }
    
    public Name Name { get; init; }
    
    public Size Size { get; init; }

    public Email NotificationEmail { get; init; }
    public IReadOnlyList<Section> Sections => _sections;

    public Section? FindSectionBySize(Size productSize)
    {
        var suitableSections = _sections
            .Where(section => 
                section.Size.Length >= productSize.Length &&
                section.Size.Width >= productSize.Width &&
                section.Size.Height >= productSize.Height)
            .OrderByDescending(section => section.Size);

        return suitableSections.FirstOrDefault();
    }

    public void UpdateSectionRows(Section section, SectionRows rows)
    {
        section.UpdateSectionRows(rows);
    }
}