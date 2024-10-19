using MonitoringService.Contracts.Interfaces;

namespace MonitoringService.Services;

public class AvailableCheckerService : IHostedService
{
    private readonly IStore _store;
    private readonly ILogger<AvailableCheckerService> _logger;

    public AvailableCheckerService(IStore store, ILogger<AvailableCheckerService> logger)
    {
        _store = store;
        _logger = logger;
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
                        await _store.SwitchState(
                            products.Where(x => x.Title == request.Title)
                                .Select(x => x.Id)
                                .First(),
                            false,
                            cancellationToken);
                        _logger.LogInformation($"Product with title {request.Title} has been booked for a customer" +
                                               $" with id {request.Customers.First().CustomerId}.");
                    }
                }
            }
        }
    }
}