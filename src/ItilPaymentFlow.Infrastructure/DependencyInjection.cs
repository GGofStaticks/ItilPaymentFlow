using System.Text;
using ItilPaymentFlow.Application.Abstractions;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Abstractions.Security;
using ItilPaymentFlow.Infrastructure.Persistence;
using ItilPaymentFlow.Infrastructure.Persistence.Repositories;
using ItilPaymentFlow.Infrastructure.Repositories;
using ItilPaymentFlow.Infrastructure.Services;
using ItilPaymentFlow.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Minio;

namespace ItilPaymentFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ??
                                "Host=localhost;Port=5433;Database=payments;Username=postgres;Password=Gjtrnfert23";

        services.AddDbContext<PaymentDbContext>(options =>
        {
            options.UseNpgsql(connectionString, b =>
                b.MigrationsAssembly("ItilPaymentFlow.Infrastructure"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        // минио настройки
        services.Configure<MinioSettings>(configuration.GetSection("Minio"));

        // регистрация минио клиента
        services.AddSingleton<IMinioClient>(sp =>
        {
            var settings = configuration.GetSection("Minio").Get<MinioSettings>()!;
            return new MinioClient()
                .WithEndpoint(settings.Endpoint)
                .WithCredentials(settings.AccessKey, settings.SecretKey)
                .WithSSL(settings.WithSSL)
                .Build();
        });

        // регистрация сервиса
        services.AddSingleton<ItilPaymentFlow.Application.Abstractions.Storage.IMinioService, MinioService>();

        //JWT сервис
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}