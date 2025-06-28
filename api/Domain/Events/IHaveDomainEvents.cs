using System.Collections.Generic;

namespace Domain.Events;

public interface IHaveDomainEvents
{
    IReadOnlyList<DomainEvent> CollectDomainEvents();
    void ClearDomainEvents();
}
