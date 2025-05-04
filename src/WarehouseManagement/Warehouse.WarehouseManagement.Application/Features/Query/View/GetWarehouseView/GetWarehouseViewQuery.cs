using Warehouse.Core.Abstractions;

namespace Warehouse.WarehouseManagement.Application.Features.Query.View.GetWarehouseView;

public record GetWarehouseViewQuery(Guid WarehouseId) : IQuery;