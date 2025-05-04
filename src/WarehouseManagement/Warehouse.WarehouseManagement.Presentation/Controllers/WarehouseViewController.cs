using Microsoft.AspNetCore.Mvc;
using Warehouse.Core.Models;
using Warehouse.Framework.Controller;
using Warehouse.WarehouseManagement.Application.Features.Query.View.GetSectionView;
using Warehouse.WarehouseManagement.Application.Features.Query.View.GetWarehouseView;

namespace Warehouse.WarehouseManagement.Presentation.Controllers;

public class WarehouseViewController : ApplicationController
{
    [HttpGet("{warehouseId:guid}")]
    public async Task<ActionResult> GetWarehouseView(
        [FromRoute] Guid warehouseId,
        [FromServices] GetWarehouseViewHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await HandleQuery(
            warehouseId,
            r => new GetWarehouseViewQuery(warehouseId), 
            handler.Handle,
            error => BadRequest(Envelope.Error(error)),
            cancellationToken);
    
        return result.Result;
    }
    
    [HttpGet("{sectionId:guid}")]
    public async Task<ActionResult> GetWarehouseView(
        [FromRoute] Guid warehouseId,
        [FromRoute] Guid sectionId,
        [FromServices] GetSectionViewHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await HandleQuery(
            warehouseId,
            sectionId,
            (w, s) => new GetSectionViewQuery(warehouseId, sectionId), 
            handler.Handle,
            error => BadRequest(Envelope.Error(error)),
            cancellationToken);
    
        return result.Result;
    }
}