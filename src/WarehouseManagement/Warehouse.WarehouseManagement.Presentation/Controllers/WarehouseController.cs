using Microsoft.AspNetCore.Mvc;
using Warehouse.Core.Models;
using Warehouse.Framework.Controller;
using Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Create;
using Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Delete;
using Warehouse.WarehouseManagement.Application.Features.Query.Section.GetByWarehouseId;
using Warehouse.WarehouseManagement.Application.Features.Query.Warehouse.GetAll;
using Warehouse.WarehouseManagement.Presentation.Requests;

namespace Warehouse.WarehouseManagement.Presentation.Controllers;

public class WarehouseController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateWarehouseRequest request,
        [FromServices] CreateWarehouseHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleCommand(
            request,
            r => r.ToCommand(),
            handler.Handle,
            error => BadRequest(Envelope.Error(error)),
            cancellationToken);
    }
    
    [HttpDelete("{warehouseId}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid warehouseId,
        [FromServices] DeleteWarehouseHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleCommand(
            warehouseId,
            i => new DeleteWarehouseCommand(warehouseId),
            handler.Handle,
            error => BadRequest(Envelope.Error(error)),
            cancellationToken);
    }
     
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] GetWarehousesWithPaginationRequest request,
        [FromServices] GetWarehousesWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleQuery(
            request,
            r => r.ToQuery(), 
            handler.Handle, 
            cancellationToken
        );
    }
    
    [HttpGet("/{warehouseId:guid}/sections")]
    public async Task<ActionResult> GetSectionsByWarehouseId(
        [FromRoute] Guid warehouseId,
        [FromQuery] GetSectionsWithPaginationRequest request,
        [FromServices] GetByWarehouseIdHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleQuery(
            request,
            r => r.ToQuery(warehouseId), 
            handler.Handle, 
            cancellationToken
        );
    }
}