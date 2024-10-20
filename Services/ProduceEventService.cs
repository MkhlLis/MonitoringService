using System.Text;
using System.Text.Json;
using MonitoringService.Contracts.Interfaces;
using MonitoringService.Contracts.Models;

namespace MonitoringService.Services;

public class ProduceEventService : IProduceEventService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProduceEventService> _logger;

    public ProduceEventService(HttpClient httpClient, ILogger<ProduceEventService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task ProduceAsync(BookingResponseEvent content, CancellationToken cancellationToken)
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            "application/json");
        using var response = await _httpClient.PostAsync("http://localhost:5012/orchestrator/order-event", jsonContent, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError((int)response.StatusCode, "Failed to produce event");
        }
    }
}

