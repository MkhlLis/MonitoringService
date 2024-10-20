## **Мониторинг.**

Экземпляры сервса могут быть запущены несколько раз, каждый экземпляр может работать со своей группой товаров 
доступность для бронирования которых будет отслеживать.

Реализация хранилища InMemory (InMemoryStore.cs)
```
private readonly List<Product> _products = new();
private readonly List<BookingRequest> _bookingRequests = new();
```


Интерфейсы связаны с реализациями посредством DI (Startup.cs).
```
services.AddHttpClient<IProduceEventService, ProduceEventService>();
services.AddSingleton<ProduceEventService, ProduceEventService>();
services.AddScoped<IMonitoringHandler, MonitoringHandler>();
services.AddSingleton<IStore, InMemoryProductStore>();
services.AddHostedService<AvailableCheckerService>();
```

Проверка доступности и бронирование заказа осуществляется HostedService-ом AvailableCheckerService.cs работающим в фоне.
В случае доступности товара и если на него есть заявка на бронирование товар бронируется, его сатус переключается в 
недоступно (забронировано), после чего сервису Orchestrator отправляется ивент о состоявшемся бронировании, 
Orchestrator в свою очередб перестраивает очереди и отпраляет обновленный список очередей и заказов сервису Monitoring.
```js
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

           await PostAsync(request, cancellationToken);
           _logger.LogInformation($"Product with title {request.Title} has been booked for a customer" +
                                               $" with id {request.Customers.First().CustomerId}.");
       }
   }
}
```

**1. Контроллер OrchestratorController**
API Реализует
1. GET /administration/get-products Запрос продуктов и их статусов.
2. PUT /administration/switch-state/{id} Переключение статуса продукта (заглушка, чтобы можно было переключить 
   статус продукта на досупен к бронироваению и hastedService мог его забронипровать).
3. POST /administration/set-queue Задание очередей на проверку доступности позиций заказов (этот метод вызывается 
   сервисом Orchestrator для передачи информации о заказах и очередях клиентов).
4. GET /administration/get-queues-list Проверка очередей на проверку доступности позиций заказов (проверям состояние 
   хранилища с очередями клиентов и заказами, которые нам передал сервис Orchestrator).


![Recording.gif](Recording.gif)

TODO: Реализация шаблонная, не претендует на готовую. CircuitBreaker, да и много еще чего. 
