using MonitoringService.Contracts.Models;

namespace MonitoringService.Contracts.Interfaces;

/// <summary>
/// Интерфейс хэндлера сервиса мониторинга.
/// </summary>
public interface IMonitoringHandler
{
    Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken);
    
    Task SwitchState(int id, bool isAvailable, CancellationToken cancellationToken);
    
    Task SetCustomersInProductQueue(IList<BookingRequest> request, CancellationToken cancellationToken);
    
    Task<IEnumerable<BookingRequest>> GetAllRequests(CancellationToken cancellationToken);
}