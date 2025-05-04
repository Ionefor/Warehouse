using Warehouse.Core.Abstractions;
using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Application.Features.Command.Product.Add;

public record AddProductCommand(
    string Name, string Description,
    string Category, SizeDto Size) : ICommand;