using System.Collections.Specialized;
using Warehouse.Core;
using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;
using Warehouse.WarehouseManagement.Domain.Entities;

namespace Warehouse.WarehouseManagement.Infrastructure.Services;

public class PackerService : IPackerService
{
    private readonly List<Rectangle> _placed = new();
    private Rectangle? _warehouseRectangle;
    
    public bool TryPack(
        SizeDto warehouseSize,
        List<SectionDto> sections)
    {
        var validateResult = SectionHeightIsValid(warehouseSize, sections);

        if (!validateResult)
        {
            return false;
        }
        
        var rectanglesResult = BuildRectangles(
            warehouseSize, sections);
        
        _warehouseRectangle = rectanglesResult.Item1;
        
        var orderedRectangles = rectanglesResult.Item2.
            OrderByDescending(r => r.Area).ToList();

        foreach (var rectangle in orderedRectangles)
        {
            if (!TryPlaceRectangle(rectangle) && rectangle.CanRotate)
            {
                rectangle.Rotate();
                if (!TryPlaceRectangle(rectangle))
                    return false;
            }
        }
        return true;
    }

    private bool TryPlaceRectangle(Rectangle sectionRectangle)
    {
        var points = new List<(double x, double y)> { (0, 0) };
        
        foreach (var placed in _placed)
        {
            points.Add((placed.X, placed.Y + placed.Width));
            points.Add((placed.X + placed.Length, placed.Y));
            points.Add((placed.X + placed.Length, placed.Y + placed.Width));
        }

        var sortedPoints = points.
            Distinct().
            OrderBy(p => p.y).
            ThenBy(p => p.x);
        
        foreach (var point in sortedPoints)
        {
            if (point.x + sectionRectangle.Length <= _warehouseRectangle!.Length && 
                point.y + sectionRectangle.Width <= _warehouseRectangle.Width)
            {
                if (!Overlaps(sectionRectangle, point.x, point.y))
                {
                    _placed.Add(
                        new Rectangle(sectionRectangle.Length, sectionRectangle.Width)
                        {
                            X = point.x,
                            Y = point.y
                        });
                    return true;
                }
            }
        }
        return false;
    }

    private bool Overlaps(Rectangle rectangle, double x, double y)
    {
        return _placed.Any(
            existing =>
            x < existing.X + existing.Length &&
            x + rectangle.Length > existing.X &&
            y < existing.Y + existing.Width &&
            y + rectangle.Width > existing.Y);
    }

    private bool SectionHeightIsValid(
        SizeDto warehouseSize,
        List<SectionDto> sections)
    {
        foreach (var section in sections)
        {
            if (section.Size.Height != warehouseSize.Height)
            {
                return false;
            }
        }
        
       return true;
    }
    
    private (Rectangle, List<Rectangle>) BuildRectangles(
        SizeDto warehouseSize,
        List<SectionDto> sections)
    {
        var warehouseRectangle = new Rectangle(
            warehouseSize.Length, warehouseSize.Width);

        List<Rectangle> sectionRectangles = new List<Rectangle>();

        foreach (var section in sections)
        {
            var rectangle = new Rectangle(
                section.Size.Length, section.Size.Width);
            
            sectionRectangles.Add(rectangle);
        }
        
        return (warehouseRectangle, sectionRectangles);
    }
}