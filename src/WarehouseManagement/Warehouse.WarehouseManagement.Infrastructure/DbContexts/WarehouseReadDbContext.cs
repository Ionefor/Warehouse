using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Warehouse.SharedKernel.Models;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Infrastructure.DbContexts;

public class WarehouseReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public IQueryable<WarehouseDto> Warehouses => Set<WarehouseDto>();
    
    public IQueryable<FullSectionDto> Sections => Set<FullSectionDto>();
    
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
            typeof(WarehouseReadDbContext).Assembly,
            type => type.FullName?.Contains(Constants.Shared.ConfigurationsRead) ?? false);
        
        modelBuilder.HasDefaultSchema(ModulesName.WarehouseManagement.ToString());
    }
    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}