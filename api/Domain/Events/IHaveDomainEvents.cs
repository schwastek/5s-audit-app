using System.Collections.Generic;

namespace Domain.Events;

public interface IHaveDomainEvents
{
    IReadOnlyList<DomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
