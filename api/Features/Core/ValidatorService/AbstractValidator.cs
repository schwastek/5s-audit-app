using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.ValidatorService;

public abstract class AbstractValidator<TInstance>
{
    public List<string> Errors { get; private set; } = [];
    public bool IsValid => Errors.Count == 0;

    public abstract Task Validate(TInstance instance, CancellationToken cancellationToken);

    protected void AddError(string error)
    {
        Errors.Add(error);
    }

    protected bool IsEmpty(string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    protected bool IsEmpty(Guid? value)
    {
        return value is null || value == Guid.Empty;
    }

    protected bool IsEmpty<T>(ICollection<T>? collection)
    {
        if (collection is null) return true;

        return collection.Count == 0;
    }

    protected bool IsEmpty<T>(IEnumerable<T>? enumerable)
    {
        if (enumerable is null) return true;

        var enumerator = enumerable.GetEnumerator();

        using (enumerator as IDisposable)
        {
            return !enumerator.MoveNext();
        }
    }
}
