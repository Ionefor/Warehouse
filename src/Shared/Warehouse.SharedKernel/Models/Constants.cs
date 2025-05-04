namespace Warehouse.SharedKernel.Models;

public static class Constants
{
    public static class Shared
    {
        public const int MaxLowTextLength = 40;
        public const int MaxMediumTextLength = 500;
        public const int MaxLargeTextLength = 2000;
        
        public const string Database = "database";
        public const string Redis = "redis";
        public const string PatternEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string ConfigurationsWrite = "Configurations.Write";
        public const string ConfigurationsRead = "Configurations.Read";
    }
    
    public static class Size
    {
        public const int MaxLengthSize = 50;
        public const int MaxWidthSize = 1000;
        public const int MaxHeightSize = 1000;
        
        public const int MinSize = 0;
    }
    
    public static class ShelfSize
    {
        public const int LargeLength = 2;
        public const int LargeWidth = 2;
        public const int LargeHeight = 2;
        
        public const int MediumLength = 1;
        public const int MediumWidth = 1;
        public const int MediumHeight = 1;
        
        public const double SmallLength = 0.5;
        public const double SmallWidth = 0.5;
        public const double SmallHeight = 0.5;
        
        public const double Aisle = 1;
    }
}