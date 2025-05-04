namespace Warehouse.Core;

public class Rectangle
{
    public double Length { get; set; }
    public double Width { get; set; }

    public double X { get; set; }
    public double Y { get; set; }
    
    public Rectangle(double length, double width)
    {
        Length = length;
        Width = width;
    }

    public double Area => Width * Length;
    public bool CanRotate => Width != Length;
    public void Rotate() => (Width, Length) = (Length, Width);
}