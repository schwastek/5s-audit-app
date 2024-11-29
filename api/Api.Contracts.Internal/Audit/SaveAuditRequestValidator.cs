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
            .WithErrorCode(ErrorCodes.Audit.AuditIdIsRequired);

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.AuthorIsRequired);

        RuleFor(x => x.Area)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.AreaIsRequired);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.StartDateIsRequired);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.EndDateIsRequired);

        RuleFor(x => x.Answers)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.AnswersIsRequired);
    }
}
