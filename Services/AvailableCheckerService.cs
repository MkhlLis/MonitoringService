using MonitoringService.Contracts.Interfaces;

namespace MonitoringService.Services;

public class AvailableCheckerService : IHostedService
{
    private readonly IStore _store;

    public AvailableCheckerService(IStore store)
    {
        _store = store;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        AvailableChecker(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task AvailableChecker(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            var products = (await _store.GetAll(cancellationToken)).Where(x => x.IsAvailable).ToList();
            var bookingRequests = await _store.GetAllRequests(cancellationToken);
            if (products.Any() && bookingRequests.Any())
            {
                foreach (var request in bookingRequests)
                {
                    if (products.Select(x => x.Title).Contains(request.Title))
                    {
                        await _store.SwitchState(products.Where(x => x.Title == request.Title).Select(x => x.Id).First(), false, cancellationToken);
                        Console.WriteLine($"Product with title {request.Title} has been booked for a customer with id {request.Customers.First().CustomerId}.");
                    }
                }
            }
        }
    }
}