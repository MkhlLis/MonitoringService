namespace MonitoringService.Contracts.Models;

public class BookingRequest
{
    public string Title { get; set; }

    public IList<Customer> Customers { get; set; }
}