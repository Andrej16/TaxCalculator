using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaxCalculator.Domain.Repositories;
using TaxCalculator.Persistence.Context;
using TaxCalculator.Persistence.Repositories;

namespace TaxCalculator.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public const string ConnectionStringName = "TaxCalculatorDb";

    public static IServiceCollection RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is not configured.");
        }

        services.AddDbContext<TaxCalculationContext>(builder =>
            builder.UseSqlServer(connectionString, options => options
                .MigrationsAssembly(typeof(TaxCalculationContext).Assembly.FullName)));

        _ = services.AddScoped<IUnitOfWork, UnitOfWork>();
        _ = services.AddScoped<ITaxCalculationsRepository, TaxCalculationsRepository>();

        return services;
    }
}
