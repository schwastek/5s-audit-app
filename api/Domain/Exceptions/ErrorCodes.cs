namespace Domain.Exceptions;

public static class ErrorCodes
{
    public static class Audit
    {
        public static readonly ErrorCode AuditIdIsRequired = new("AuditIdIsRequired");
        public static readonly ErrorCode AuditDoesNotExist = new("AuditDoesNotExist");
        public static readonly ErrorCode AuditAuthorIsRequired = new("AuditAuthorIsRequired");
        public static readonly ErrorCode AuditAreaIsRequired = new("AuditAreaIsRequired");
        public static readonly ErrorCode AuditStartDateIsRequired = new("AuditStartDateIsRequired");
        public static readonly ErrorCode AuditEndDateIsRequired = new("AuditEndDateIsRequired");
        public static readonly ErrorCode AuditAnswersIsRequired = new("AuditAnswersIsRequired");
    }

    public static class AuditAction
    {
        public static readonly ErrorCode AuditActionIdIsRequired = new("AuditActionIdIsRequired");
        public static readonly ErrorCode AuditActionDescriptionIsRequired = new("AuditActionDescriptionIsRequired");
        public static readonly ErrorCode AuditActionDoesNotExist = new("AuditActionDoesNotExist");
        public static readonly ErrorCode AuditActionDescriptionIsTooLong = new("AuditActionDescriptionIsTooLong");
    }

    public static class Identity
    {
        public static readonly ErrorCode IdentityDisplayNameIsRequired = new("IdentityDisplayNameIsRequired");
        public static readonly ErrorCode IdentityEmailIsRequired = new("IdentityEmailIsRequired");
        public static readonly ErrorCode IdentityEmailFormatIsNotValid = new("IdentityEmailFormatIsNotValid");
        public static readonly ErrorCode IdentityEmailIsAlreadyTaken = new("IdentityEmailIsAlreadyTaken");
        public static readonly ErrorCode IdentityUsernameIsRequired = new("IdentityUsernameIsRequired");
        public static readonly ErrorCode IdentityUsernameIsAlreadyTaken = new("IdentityUsernameIsAlreadyTaken");
        public static readonly ErrorCode IdentityPasswordIsRequired = new("IdentityPasswordIsRequired");
        public static readonly ErrorCode IdentityPasswordIsTooWeak = new("IdentityPasswordIsTooWeak");
    }
}
