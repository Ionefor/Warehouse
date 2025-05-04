using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Warehouse.Core.Abstractions;
using Warehouse.Core.Extensions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;
using Warehouse.WarehouseManagement.Domain.Entities;
using Warehouse.WarehouseManagement.Domain.ValueObjects;

namespace Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Create;

public class CreateWarehouseHandler : ICommandHandler<Guid, CreateWarehouseCommand>
{
    private readonly IValidator<CreateWarehouseCommand> _validator;
    private readonly IPackerService _packerService;
    private readonly ILogger<CreateWarehouseHandler> _logger;
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateWarehouseHandler(
        IValidator<CreateWarehouseCommand> validator,
        IPackerService packerService,
        ILogger<CreateWarehouseHandler> logger,
        IWarehouseRepository repository,
        [FromKeyedServices(ModulesName.WarehouseManagement)]IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _packerService = packerService;
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateWarehouseCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.
            ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var packerResult = _packerService.TryPack(
            command.WarehouseSize,
            command.Sections);

        if (!packerResult)
        {
            return Errors.General.Failed(
                new ErrorParameters.Failed(ErrorConstants.ExtraMessage.
                    FailedCreateWarehouse)).ToErrorList();
        }
        
        var sections = new List<Section>();
        
        foreach (var section in command.Sections)
        {
            if (section.Type is SectionType.Large)
            {
                sections.Add(CreateLargeSection(section));
            }
            else if (section.Type is SectionType.Medium)
            {
                sections.Add(CreateMediumSection(section));
            }
            else
            {
                sections.Add(CreateSmallSection(section));
            }
        }
        
        var warehouseId = WarehouseId.NewGuid();
        
        var name = Name.Create(command.Name).Value;
        
        var size = Size.Create(
            command.WarehouseSize.Length,
            command.WarehouseSize.Width,
            command.WarehouseSize.Height).Value;

        var email = Email.Create(command.Email).Value;
        
        var warehouse = new Domain.Aggregate.
            Warehouse(warehouseId, name, size, sections, email);

        await _repository.Add(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation(
            "Warehouse for Id: {Id} created with sections",
            warehouse.Id);
        
        return warehouse.Id.Id;
    }

    private Section CreateLargeSection(SectionDto section)
    {
        var largeSectionId = SectionId.NewGuid();
        
        var sectionSize = Size.Create(
            section.Size.Length,
            section.Size.Width,
            section.Size.Height).Value;
        
        var rowCount = (int)
            (section.Size.Width / (Constants.ShelfSize.LargeWidth + Constants.ShelfSize.Aisle));
        
        var sectionRowSize = Size.Create(
            section.Size.Length,
            Constants.ShelfSize.LargeWidth,
            section.Size.Height).Value;

        var shelfSize = Size.Create(
            Constants.ShelfSize.LargeLength,
            Constants.ShelfSize.LargeWidth,
            Constants.ShelfSize.LargeHeight).Value;
        
        var shelfRowCount = (int)(sectionRowSize.Height / Constants.ShelfSize.LargeHeight);
        var shelfColumnCount = (int)(sectionRowSize.Length / Constants.ShelfSize.LargeLength);
        
        var sectionRows = new List<SectionRow>();
        
        for (var i = 0; i < rowCount; i++)
        {
            var shelfs = new List<Shelf>();
            
            for (var j = 0; j < shelfRowCount; j++)
            {
                for (var k = 0; k < shelfColumnCount; k++)
                {
                    shelfs.Add(Shelf.Create(true, shelfSize, j, k).Value);
                }
            }

            var sectionRow = SectionRow.
                Create(sectionRowSize, new Shelfs(shelfs), i).Value;
            
            sectionRows.Add(sectionRow);
        }

        return new Section(
            largeSectionId, sectionSize, new SectionRows(sectionRows));
    }
    
    private Section CreateMediumSection(SectionDto section)
    {
        var mediumSectionId = SectionId.NewGuid();
        
        var sectionSize = Size.Create(
            section.Size.Length,
            section.Size.Width,
            section.Size.Height).Value;
        
        var rowCount = (int)(section.Size.Width /
                             (Constants.ShelfSize.MediumWidth + Constants.ShelfSize.Aisle));
        
        var sectionRowSize = Size.Create(
            section.Size.Length,
            Constants.ShelfSize.MediumWidth,
            section.Size.Height).Value;

        var shelfSize = Size.Create(
            Constants.ShelfSize.MediumLength,
            Constants.ShelfSize.MediumWidth,
            Constants.ShelfSize.MediumHeight).Value;
        
        var shelfRowCount = (int)(sectionRowSize.Height / Constants.ShelfSize.MediumHeight);
        var shelfColumnCount = (int)(sectionRowSize.Length / Constants.ShelfSize.MediumLength);
        
        var sectionRows = new List<SectionRow>();
        
        for (var i = 0; i < rowCount; i++)
        {
            var shelfs = new List<Shelf>();
            
            for (var j = 0; j < shelfRowCount; j++)
            {
                for (var k = 0; k < shelfColumnCount; k++)
                {
                    shelfs.Add(Shelf.Create(true, shelfSize, j, k).Value);
                }
            }

            var sectionRow = SectionRow.
                Create(sectionRowSize, new Shelfs(shelfs), i).Value;
            
            sectionRows.Add(sectionRow);
        }

        return new Section(
            mediumSectionId, sectionSize, new SectionRows(sectionRows));
    }
    
    private Section CreateSmallSection(SectionDto section)
    {
        var smallSectionId = SectionId.NewGuid();
        
        var sectionSize = Size.Create(
            section.Size.Length,
            section.Size.Width,
            section.Size.Height).Value;
        
        var rowCount = (int)(section.Size.Width /
                             (Constants.ShelfSize.SmallWidth + Constants.ShelfSize.Aisle));
        
        var sectionRowSize = Size.Create(
            section.Size.Length,
            Constants.ShelfSize.SmallWidth,
            section.Size.Height).Value;

        var shelfSize = Size.Create(
            Constants.ShelfSize.SmallLength,
            Constants.ShelfSize.SmallWidth,
            Constants.ShelfSize.SmallHeight).Value;
        
        var shelfRowCount = (int)(sectionRowSize.Height / Constants.ShelfSize.SmallHeight);
        var shelfColumnCount = (int)(sectionRowSize.Length / Constants.ShelfSize.SmallLength);
        
        var sectionRows = new List<SectionRow>();
        
        for (var i = 0; i < rowCount; i++)
        {
            var shelfs = new List<Shelf>();
            
            for (var j = 0; j < shelfRowCount; j++)
            {
                for (var k = 0; k < shelfColumnCount; k++)
                {
                    shelfs.Add(Shelf.Create(true, shelfSize, j, k).Value);
                }
            }

            var sectionRow = SectionRow.
                Create(sectionRowSize, new Shelfs(shelfs), i).Value;
            
            sectionRows.Add(sectionRow);
        }
        
        return new Section(
            smallSectionId, sectionSize, new SectionRows(sectionRows));
    }
}