using FluentValidation.Results;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehouse.Core.Extensions;

public static class ValidationExtension
{
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Errors.General.ValueIsInvalid(
                new ErrorParameters.ValueIsInvalid(nameof(validationError.PropertyName)));

        return errors.ToList();
    }
}