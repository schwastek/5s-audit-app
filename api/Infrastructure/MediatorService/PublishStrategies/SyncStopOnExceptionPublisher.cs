using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorService.PublishStrategies;

/// <summary>
/// Publishes a notification to all registered handlers sequentially.
/// Returns when all handlers have been successfully executed. 
/// If any handler throws an exception, execution stops, subsequent handlers are not executed.
/// </summary>
public class SyncStopOnExceptionPublisher : INotificationPublisher
{
    public async Task Publish<TNotification>(IEnumerable<INotificationHandler<TNotification>> handlers,
        TNotification notification, CancellationToken cancellationToken)
    {
        foreach (var handler in handlers)
        {
            await handler.Handle(notification, cancellationToken);
        }
    }
}
