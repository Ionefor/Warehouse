using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Abstractions;

public interface IReadDbContext
{
    IQueryable<WarehouseDto> Warehouses { get; }
    
    IQueryable<FullSectionDto> Sections { get; }
}