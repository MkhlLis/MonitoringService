namespace MonitoringService.Contracts.Models;

/// <summary>
/// Товар.
/// </summary>
public class Product
{
    /// <summary>
    /// Идентификатор товара.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Названите товара.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описани товара.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Статитус товара.
    /// </summary>
    public bool IsAvailable { get; set; }
}