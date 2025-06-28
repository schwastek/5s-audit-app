using System.Collections.Generic;

namespace Domain.Events.ChangeTracking;

public abstract class MemberChanged
{
    public string MemberName { get; }

    protected MemberChanged(string memberName)
    {
        MemberName = memberName;
    }

    public override string ToString()
    {
        return $"Member '{MemberName}' changed.";
    }
}

public abstract class MemberChanged<T> : MemberChanged
{
    public IEqualityComparer<T> Comparer { get; }

    protected MemberChanged(string memberName, IEqualityComparer<T>? comparer) : base(memberName)
    {
        Comparer = comparer ?? EqualityComparer<T>.Default;
    }

    public virtual bool NoChanges()
    {
        // Default: member changed.
        return false;
    }
}
