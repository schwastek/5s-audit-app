using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.MediatorService;

/// <summary>
/// Defines a handler for a notification.
/// </summary>
/// <typeparam name="TNotification">The type of notification being handled</typeparam>
public interface INotificationHandler<in TNotification>
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}
