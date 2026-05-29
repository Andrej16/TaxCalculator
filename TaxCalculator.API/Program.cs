using Asp.Versioning;
using Microsoft.Extensions.Options;
using TaxCalculator.API.Extensions;
using TaxCalculator.Application.Extensions;
using TaxCalculator.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_ = builder.Services
    .RegisterPersistence(builder.Configuration)
    .AddApplicationServices()
    .AddMemoryCache();

builder.Services.AddApiVersioning(options => options.ApiVersionReader = new UrlSegmentApiVersionReader());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();

app.RegisterTaxEndpoints();
app.RegisterUsersEndpoints();

app.Run();