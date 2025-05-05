using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.ValidatorService;

public abstract class AbstractValidator<TInstance>
{
    public List<ValidationError> Errors { get; private set; } = [];
    public bool IsValid => Errors.Count == 0;

    public abstract Task Validate(TInstance instance, CancellationToken cancellationToken);

    protected void AddError(ErrorCode errorCode)
    {
        Errors.Add(new ValidationError(errorCode));
    }

    protected static bool IsEmpty(string? value) => ValidatorHelper.IsEmpty(value);
    protected static bool IsEmpty(Guid? value) => ValidatorHelper.IsEmpty(value);
    protected static bool IsEmpty<T>(ICollection<T>? collection) => ValidatorHelper.IsEmpty(collection);
    protected static bool IsEmpty<T>(IEnumerable<T>? enumerable) => ValidatorHelper.IsEmpty(enumerable);
    protected static bool IsEmpty(DateTime? value) => ValidatorHelper.IsEmpty(value);
    protected static bool IsEmailAddress(string? value) => ValidatorHelper.IsEmpty(value);
}
