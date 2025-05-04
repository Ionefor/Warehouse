using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Warehouse.SharedKernel.Models;
using Warehouse.WarehouseManagement.Domain.Entities;

namespace Warehouse.WarehouseManagement.Infrastructure.DbContexts;

public class WarehouseWriteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Domain.Aggregate.Warehouse> Warehouses => Set<Domain.Aggregate.Warehouse>();
    
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
            typeof(WarehouseWriteDbContext).Assembly,
            type => type.FullName?.Contains(Constants.Shared.ConfigurationsWrite) ?? false);
    }
    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}