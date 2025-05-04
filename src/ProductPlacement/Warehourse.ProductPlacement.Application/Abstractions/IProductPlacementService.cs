using Warehourse.ProductPlacement.Domain.Aggregate;

namespace Warehourse.ProductPlacement.Application.Abstractions;

public interface IProductPlacementService
{
    Task PendingProductPlacement(CancellationToken cancellationToken);
}