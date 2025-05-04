

using System.Text.RegularExpressions;
using Warehouse.SharedKernel.Models;

namespace Warehouse.SharedKernel;

public static class StringExtensions
{
    public static bool IsEmail(this string email)
    {
        return Regex.IsMatch(email, Constants.Shared.PatternEmail);
    }
}