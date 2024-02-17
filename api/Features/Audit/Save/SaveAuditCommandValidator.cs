using FluentValidation;

namespace Features.Audit.Save;

public sealed class SaveAuditCommandValidator : AbstractValidator<SaveAuditCommand>
{
    public SaveAuditCommandValidator()
    {
        RuleFor(x => x.Author).NotEmpty();
        RuleFor(x => x.Area).NotEmpty();
        RuleFor(x => x.Answers).NotEmpty();
    }
}
