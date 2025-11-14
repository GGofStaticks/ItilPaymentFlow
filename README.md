DDD-шаблон для .NET 8 (Itil Payment Flow)

Скелет чистой архитектуры на базе DDD для ASP.NET Core:

- Слои:
  - Domain: сущности, value-объекты, доменные события.
  - Application: кейсы (CQRS через MediatR), DTO, валидация.
  - Infrastructure: EF Core, репозитории, UnitOfWork, время.
  - Api: минимальные эндпоинты, DI и конфигурация.
  - tests: unit-тесты домена.

Структура

backend/
- ItilPaymentFlow.sln
- Directory.Build.props
- src/
  - ItilPaymentFlow.Domain/
  - ItilPaymentFlow.Application/
  - ItilPaymentFlow.Infrastructure/
  - ItilPaymentFlow.Api/
- tests/
  - ItilPaymentFlow.UnitTests/

Быстрый старт

1) Сборка и тесты:
   - `dotnet build backend/ItilPaymentFlow.sln -c Release`
   - `dotnet test backend/ItilPaymentFlow.sln -c Release`

2) Запуск API:
   - Требуется PostgreSQL. По умолчанию: `Host=localhost;Port=5432;Database=payments;Username=postgres;Password=postgres`
   - Настройка строки подключения: `backend/src/ItilPaymentFlow.Api/appsettings.json:9`
     или через переменную окружения `ConnectionStrings__Default`.
   - `dotnet run --project backend/src/ItilPaymentFlow.Api`
   - Миграции применяются автоматически на старте (`Database.Migrate()`).

3) Swagger:
   - UI: `/swagger` после запуска приложения.

4) Эндпоинты (Controllers):
   - POST `/api/payments` — создать платёж
     Пример тела: `{ "amount": 100.00, "currency": "USD", "reference": "INV-001" }`
   - GET `/api/payments/{id}` — получить платёж по Id

Технологии

- .NET 8, ASP.NET Core
- MediatR — CQRS/медиатор
- FluentValidation — валидация
- EF Core + SQLite — персистентность

DDD-принципы

- Domain не зависит от инфраструктуры. Содержит AggregateRoot, ValueObject, доменные события.
- Application — единственная точка использования домена: команды/запросы, порты (`IPaymentRepository`, `IUnitOfWork`).
- Infrastructure реализует порты, маппинг EF Core и UnitOfWork, публикует доменные события через MediatR.
- Api только конфигурирует DI и проксирует команды/запросы.
  Используются контроллеры (`backend/src/ItilPaymentFlow.Api/Controllers/PaymentsController.cs`). Конфигурация в `Startup` (`backend/src/ItilPaymentFlow.Api/Startup.cs`).

Миграции

- Провайдер: PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL`).
- Добавьте EF Tools: `dotnet tool install --global dotnet-ef`
- Создание миграции: `dotnet ef migrations add Initial --project backend/src/ItilPaymentFlow.Infrastructure --startup-project backend/src/ItilPaymentFlow.Api --context PaymentDbContext`
- Применение: выполняется на старте (`Database.Migrate()` в `Startup.Configure`).

Дальнейшее развитие

- Расширить агрегаты (Payment) доменными инвариантами и процессами.
- Добавить контракты интеграции и outbox-паттерн при необходимости.
- Вынести контракты API в отдельный Contracts-пакет при версии API.
- Добавить Observability (Serilog, OpenTelemetry), кэширование и политику ретраев.
