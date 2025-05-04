using Warehourse.ProductPlacement.Application.Features.Command.Product.Add;
using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Presentation.Requests;

public record AddProductRequest(string Name, string Description, string Category, SizeDto Size)
{
    public AddProductCommand ToCommand() =>
        new(Name, Description, Category, Size);
}