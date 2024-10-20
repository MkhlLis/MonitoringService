using Microsoft.AspNetCore.Mvc;
using MonitoringService.Contracts.Interfaces;
using MonitoringService.Contracts.Models;

namespace MonitoringService.Controllers;

/// <summary>
/// Конитроллер микросервиса мониторинга.
/// </summary>
[ApiController]
[Route("administration")]
public class MonitoringController : ControllerBase
{
    private readonly IMonitoringHandler _handler;
    
    public MonitoringController(IMonitoringHandler handler)
    {
        _handler = handler;
    }
    
    /// <summary>
    ///  Запрос продуктов и их статусов.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Информация о товарах.</returns>
    [HttpGet("get-products")]
    public async Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
    {
        return await _handler.GetProducts(cancellationToken);
    }

    /// <summary>
    ///  Переключение статуса продукта.
    /// </summary>
    /// <param name="id">Идентификатор продукта.</param>
    /// <param name="isAvailable">Статус продукта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPut("switch-state/{id}")]
    public async Task GetProductById([FromRoute] int id, [FromQuery] bool isAvailable, CancellationToken cancellationToken)
    {
        await _handler.SwitchState(id, isAvailable, cancellationToken);
        await Task.FromResult(Ok());
    }

    /// <summary>
    /// Звдание очередей на проверку доступности позиций заказов.
    /// </summary>
    /// <param name="request">Очереди на проверку доступности позиции заказа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost("set-queue")]
    public async Task SetCustomersInProductQueue([FromBody] IList<BookingRequest> request, CancellationToken cancellationToken)
    {
        await _handler.SetCustomersInProductQueue(request, cancellationToken);
        await Task.FromResult(Ok());
    }

    /// <summary>
    /// Проверка очередей на проверку доступности позиций заказов.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Очереди на проверку доступности позиции заказа.</returns>
    [HttpGet("get-queues-list")]
    public async Task<IEnumerable<BookingRequest>> GetAllRequests(CancellationToken cancellationToken)
    {
        return await _handler.GetAllRequests(cancellationToken);
    }
}