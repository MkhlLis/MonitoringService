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
            var products = await _store.GetAll(cancellationToken);
            Console.WriteLine($"Available checker service is checking. Book" +
                              $"\n {products.FirstOrDefault(x => x.Id == 1).Id}" +
                              $"\n {products.FirstOrDefault(x => x.Id == 1).Title}" +
                              $"\n {products.FirstOrDefault(x => x.Id == 1).Description}" +
                              $"\n is {(products.FirstOrDefault(x => x.Id == 1).IsAvailable 
                                  ? "available" 
                                  : "not available")}");
        }
    }
}