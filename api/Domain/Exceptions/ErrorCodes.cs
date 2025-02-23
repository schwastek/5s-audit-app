namespace Domain.Exceptions;

public static class ErrorCodes
{
    public static class Audit
    {
        public const string AuditIdIsRequired = "AuditIdIsRequired";
        public const string DoesNotExist = "AuditDoesNotExist";
        public const string AuthorIsRequired = "AuthorIsRequired";
        public const string AreaIsRequired = "AuditAreaIsRequired";
        public const string StartDateIsRequired = "AuditStartDateIsRequired";
        public const string EndDateIsRequired = "AuditEndDateIsRequired";
        public const string AnswersIsRequired = "AuditAnswersIsRequired";
        public const string InvalidOrderByField = "InvalidOrderByField";
    }

    public static class AuditAction
    {
        public const string ActionsIsRequired = "AuditActionsIsRequired";
        public const string ActionIdIsRequired = "AuditActionIdIsRequired";
        public const string DescriptionIsRequired = "AuditActionDescriptionIsRequired";
        public const string DoesNotExist = "AuditActionDoesNotExist";
        public const string DescriptionIsTooLong = "AuditActionDescriptionIsTooLong";
    }

    public static class Identity
    {
        public const string DisplayNameIsRequired = "IdentityDisplayNameIsRequired";
        public const string EmailIsRequired = "IdentityEmailIsRequired";
        public const string EmailFormatIsNotValid = "IdentityEmailFormatIsNotValid";
        public const string EmailIsAlreadyTaken = "IdentityEmailIsAlreadyTaken";
        public const string UsernameIsRequired = "IdentityUsernameIsRequired";
        public const string UsernameIsAlreadyTaken = "IdentityUsernameIsAlreadyTaken";
        public const string PasswordIsRequired = "IdentityPasswordIsRequired";
        public const string PasswordIsTooWeak = "IdentityPasswordIsTooWeak";
    }
}
