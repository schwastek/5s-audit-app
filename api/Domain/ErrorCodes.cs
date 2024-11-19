namespace Domain;

public static class ErrorCodes
{
    public const string AuditIdIsRequired = "AuditIdIsRequired";
    public const string AuditDoesNotExist = "AuditDoesNotExist";
    public const string AuthorIsRequired = "AuthorIsRequired";
    public const string AreaIsRequired = "AreaIsRequired";
    public const string StartDateIsRequired = "StartDateIsRequired";
    public const string EndDateIsRequired = "EndDateIsRequired";
    public const string AnswersIsRequired = "AnswersIsRequired";
    public const string ActionsIsRequired = "ActionsIsRequired";
    public const string ActionIdIsRequired = "ActionIdIsRequired";
    public const string DescriptionIsRequired = "DescriptionIsRequired";
    public const string AuditActionDescriptionIsTooLong = "AuditActionDescriptionIsTooLong";
    public const string AuditActionDoesNotExist = "AuditActionDoesNotExist";
    public const string DisplayNameIsRequired = "DisplayNameIsRequired";
    public const string EmailIsRequired = "EmailIsRequired";
    public const string EmailFormatIsNotValid = "EmailFormatIsNotValid";
    public const string EmailIsAlreadyTaken = "EmailIsAlreadyTaken";
    public const string UsernameIsRequired = "UsernameIsRequired";
    public const string UsernameIsAlreadyTaken = "UsernameIsAlreadyTaken";
    public const string PasswordIsRequired = "PasswordIsRequired";
    public const string PasswordIsTooWeak = "PasswordIsTooWeak";
}
