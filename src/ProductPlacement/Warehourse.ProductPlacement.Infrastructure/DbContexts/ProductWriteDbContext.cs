using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehouse.SharedKernel.Models;

namespace Warehourse.ProductPlacement.Infrastructure.DbContexts;

public class ProductWriteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PendingProduct> PendingProducts => Set<PendingProduct>();
    public DbSet<ProductStorage> Storages => Set<ProductStorage>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.Shared.Database));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(ModulesName.WarehouseManagement.ToString());
        
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProductWriteDbContext).Assembly,
            type => type.FullName?.Contains(Constants.Shared.ConfigurationsWrite) ?? false);
    }
    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}