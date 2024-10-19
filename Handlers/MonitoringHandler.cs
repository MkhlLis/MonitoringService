using MonitoringService.Contracts.Interfaces;
using MonitoringService.Contracts.Models;

namespace MonitoringService.Handlers;

/// <summary>
/// Интерфейс хэндлера сервиса мониторинга.
/// </summary>
internal class MonitoringHandler : IMonitoringHandler
{
    private readonly IStore _store;
    
    public MonitoringHandler(IStore store)
    {
        _store = store;
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
    {
        return await _store.GetAll(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SwitchState(int id, bool isAvailable, CancellationToken cancellationToken)
    {
        await _store.SwitchState(id, isAvailable, cancellationToken);
    }

    public async Task SetCustomersInProductQueue(IList<BookingRequest> request, CancellationToken cancellationToken)
    {
        await _store.SetCustomersInProductQueue(request, cancellationToken);
    }

    public async Task<IEnumerable<BookingRequest>> GetAllRequests(CancellationToken cancellationToken)
    {
        return await _store.GetAllRequests(cancellationToken);
    }
}