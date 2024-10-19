using MonitoringService.Contracts.Models;

namespace MonitoringService.Contracts.Interfaces;

public interface IStore
{
    Task SwitchState(int id, bool isAvailable, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken);
    
    Task SetCustomersInProductQueue(IList<BookingRequest> request, CancellationToken cancellationToken);
    
    Task<IEnumerable<BookingRequest>> GetAllRequests(CancellationToken cancellationToken);
}