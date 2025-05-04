namespace Warehouse.SharedKernel.Models.Errors;

public static class ErrorConstants
{
    public static class Code
    {
        public const string InternalServer = "server.is.internal";
        public const string Failed = "failed.operation";
        public const string NotFound = "value.not.found";
        public const string ValueIsRequired = "value.is.required";
        public const string ValueIsInvalid = "value.is.invalid";
        public const string DeleteIsInvalid = "cannot.delete.operation";
        public const string ValueAlreadyExists = "value.already.exists";
    }
   
    public static class GeneralMessage
    {
        public const string InternalServer = "Server is internal";
        public const string Failed = "Failed to do operation";
        public const string NotFound = "Value not found";
        public const string ValueIsRequired = "Value cannot be null or empty";
        public const string ValueIsInvalid = "Value is invalid";
        public const string DeleteIsInvalid = "Cannot be deleted";
    }

    public static class ExtraMessage
    {
        public const string ValueAlreadyExists = "Value already exists";
        public const string FailedCreateWarehouse = "Fail create warehouse";
        public const string FailedTransferTransaction = "Can not do transfer transaction";
        public const string FailedDepositTransaction = "Can not do deposit transaction";
    }
}