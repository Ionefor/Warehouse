using Microsoft.AspNetCore.Mvc;
using Warehouse.Core.Models;

namespace Warehouse.Framework.Controller;

[ApiController]
[Route("[controller]")]
public partial class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        
        return base.Ok(envelope);
    }
}