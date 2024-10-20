using MonitoringService.Contracts.Models;

namespace MonitoringService.Contracts.Interfaces;

/// <summary>
/// Сервис отправки ивентов.
/// </summary>
public interface IProduceEventService
{
    Task ProduceAsync(BookingResponseEvent content, CancellationToken cancellationToken);
}