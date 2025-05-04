using Warehouse.Core.Abstractions;

namespace Warehouse.WarehouseManagement.Application.Features.Query.View.GetSectionView;

public record GetSectionViewQuery(Guid WarehouseId, Guid SectionId) : IQuery;