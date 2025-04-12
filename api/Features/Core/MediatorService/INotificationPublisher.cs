using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.MediatorService;

/// <summary>
/// Defines a strategy for publishing a notification to multiple handlers (e.g. sequential or parallel).
/// </summary>
public interface INotificationPublisher
{
    Task Publish<TNotification>(IEnumerable<INotificationHandler<TNotification>> handlers,
        TNotification notification, CancellationToken cancellationToken);
}
