using TaxCalculator.API.Extensions;
using TaxCalculator.Application.Extensions;
using TaxCalculator.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_ = builder.Services
    .RegisterPersistence(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.RegisterTaxEndpoints(builder.Configuration);

app.Run();