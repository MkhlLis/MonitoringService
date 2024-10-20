namespace MonitoringService.Contracts.Models;

/// <summary>
/// Событие отчета об успешном пронировании.
/// </summary>
public class BookingResponseEvent
{
        public int CustomerId { get; set; }
        public string Title { get; set; }
}