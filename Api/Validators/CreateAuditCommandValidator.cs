using Api.Queries;
using FluentValidation;

namespace Api.Validators;

public sealed class CreateAuditCommandValidator : AbstractValidator<CreateAuditCommand>
{
    public CreateAuditCommandValidator()
    {
        RuleFor(x => x.Audit.Author).NotEmpty();
        RuleFor(x => x.Audit.Area).NotEmpty();
        RuleFor(x => x.Audit.Answers).NotEmpty();
    }
}
