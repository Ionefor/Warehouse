using Microsoft.AspNetCore.Mvc;
using Warehourse.ProductPlacement.Application.Features.Command.Product.Add;
using Warehourse.ProductPlacement.Application.Features.Command.Product.Transfer;
using Warehourse.ProductPlacement.Application.Features.Query.PendingProduct.GetAll;
using Warehourse.ProductPlacement.Application.Features.Query.Product.GetByCategory;
using Warehourse.ProductPlacement.Application.Features.Query.ProductStorage.GetAll;
using Warehourse.ProductPlacement.Presentation.Requests;
using Warehouse.Core.Models;
using Warehouse.Framework.Controller;

namespace Warehourse.ProductPlacement.Presentation;

public class ProductPlacementController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> AddProduct(
        [FromBody] AddProductRequest request,
        [FromServices] AddProductHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleCommand(
            request,
            r => r.ToCommand(),
            handler.Handle,
            error => BadRequest(Envelope.Error(error)),
            cancellationToken);
    }
    
    [HttpPost("product_transfer")]
    public async Task<ActionResult<Guid>> Transfer(
        [FromBody] ProductTransferRequest request,
        [FromServices] ProductTransferHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleCommand(
            request,
            r => r.ToCommand(),
            handler.Handle,
            error => BadRequest(Envelope.Error(error)),
            cancellationToken);
    }
    
    [HttpGet("pending_products")]
    public async Task<ActionResult> GetAllPending(
        [FromQuery] GetPendingProductsWithPaginationRequest request,
        [FromServices] GetPendingProductsWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleQuery(
            request,
            r => r.ToQuery(), 
            handler.Handle, 
            cancellationToken
        );
    }
    
    [HttpGet("products")]
    public async Task<ActionResult> GetProductByCategory(
        [FromQuery] GetProductsWithPaginationRequest request,
        [FromServices] GetProductsWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleQuery(
            request,
            r => r.ToQuery(), 
            handler.Handle, 
            cancellationToken
        );
    }
    
    [HttpGet("product_storages")]
    public async Task<ActionResult> GetProductByCategory(
        [FromQuery] GetProductStoragesWithPaginationRequest request,
        [FromServices] GetProductStoragesWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        return await HandleQuery(
            request,
            r => r.ToQuery(), 
            handler.Handle, 
            cancellationToken
        );
    }
}