using Api.Contracts.Audit.Requests;
using Core.ValidatorService;
using Domain;
using FluentValidation;

namespace Api.Contracts.Internal.Audit;

public class SaveAuditRequestValidator : CustomAbstractValidator<SaveAuditRequest>
{
    public SaveAuditRequestValidator()
    {
        RuleFor(x => x.AuditId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditIdIsRequired);

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuthorIsRequired);

        RuleFor(x => x.Area)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AreaIsRequired);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.StartDateIsRequired);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.EndDateIsRequired);

        RuleFor(x => x.Answers)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AnswersIsRequired);

        RuleFor(x => x.Actions)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.ActionsIsRequired);
    }
}
