using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Contracts.Dtos;
using Warehouse.SharedKernel.Models;

namespace Warehourse.ProductPlacement.Infrastructure.DbContexts;

public class ProductReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public IQueryable<ProductStorageDto> ProductStorages => Set<ProductStorageDto>();
    
    public IQueryable<ProductDto> Products => Set<ProductDto>();
    
    public IQueryable<PendingProductDto> PendingProducts => Set<PendingProductDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.Shared.Database));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProductReadDbContext).Assembly,
            type => type.FullName?.Contains(Constants.Shared.ConfigurationsRead) ?? false);
        
        modelBuilder.HasDefaultSchema(ModulesName.WarehouseManagement.ToString());
    }
    
    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}