using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Application.Abstractions;

public interface IEmailNotificationService
{
    Task SendNotification(Product product, WarehouseDto warehouse);
}