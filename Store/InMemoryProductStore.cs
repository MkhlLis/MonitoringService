using MonitoringService.Contracts.Interfaces;
using MonitoringService.Contracts.Models;

namespace MonitoringService.Store;

internal class InMemoryProductStore : IStore
{
    private readonly List<Product> _products = new();
    private readonly List<BookingRequest> _bookingRequests = new();

    public InMemoryProductStore()
    {
        _products.Add(new Product { Id = 1, Title = "Война и мир. Том 1", Description = "Роман", IsAvailable = false});
        _products.Add(new Product { Id = 2, Title = "Война и мир. Том 2", Description = "Роман", IsAvailable = false});
    }

    public async Task SwitchState(int id, bool isAvailable, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        _products.Find(p => p.Id == id)!.IsAvailable = isAvailable;
    }

    public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return _products;
    }

    public async Task SetCustomersInProductQueue(IList<BookingRequest> request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        foreach (var req in request)
        {
            _bookingRequests.Add(req);
        }
    }

    public async Task<IEnumerable<BookingRequest>> GetAllRequests(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return _bookingRequests;
    }
}