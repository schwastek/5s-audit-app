using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorService.PublishStrategies;

/// <summary>
/// Publishes a notification to all registered handlers in parallel using <see cref="Task.WhenAll"/>.
/// Returns when all handlers have finished execution.
/// If one or more handlers throw exceptions, they are aggregated into an <see cref="AggregateException"/>.
/// </summary>
/// <remarks>
/// It improves performance and ensures that handlers are isolated from each other,
/// but it comes with these caveats:
/// - If any handlers throw, you get only one <see cref="AggregateException"/>, wrapping all others.
/// - If multiple handlers mutate shared state (e.g. DB), you must ensure thread-safety.
/// </remarks>
public class ParallelWhenAllPublisher : INotificationPublisher
{
    public Task Publish<TNotification>(IEnumerable<INotificationHandler<TNotification>> handlers,
        TNotification notification, CancellationToken cancellationToken)
    {
        var tasks = handlers.Select(h => h.Handle(notification, cancellationToken)).ToArray();

        return Task.WhenAll(tasks);
    }
}
