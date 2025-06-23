using System.Collections.Generic;

namespace Domain.Events;

public abstract class MemberChangedEvent
{
    public string MemberName { get; }

    protected MemberChangedEvent(string memberName)
    {
        MemberName = memberName;
    }

    public virtual string GetDescription()
    {
        return $"Member '{MemberName}' changed.";
    }

    public override string ToString() => GetDescription();
}

public abstract class MemberChangedEvent<T> : MemberChangedEvent
{
    public IEqualityComparer<T> Comparer { get; }

    protected MemberChangedEvent(string memberName, IEqualityComparer<T>? comparer) : base(memberName)
    {
        Comparer = comparer ?? EqualityComparer<T>.Default;
    }
}
