using yakov.ExchangeRates.Server.Application;
using yakov.ExchangeRates.Server.Domain.Interfaces;
using yakov.ExchangeRates.Server.Infrastructure;
using yakov.ExchangeRates.Server.Domain.Converters;
using yakov.ExchangeRates.Server.Infrastructure.FileServices;
using yakov.ExchangeRates.Server.Infrastructure.RemoteAPIServices;
using yakov.ExchangeRates.Server.Infrastructure.ValidationServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<ITimePeriodValidator, TimePeriodValidatorService>();
builder.Services.AddSingleton<IRatesFileService, RateFileService>();
builder.Services.AddSingleton<IStorageService, StorageService>();
builder.Services.AddSingleton<RatesContext>();
builder.Services.AddSingleton<IRatesRepository, RatesRepository>();
builder.Services.AddSingleton<IAPIServiceBuilder, RatesAPIServiceBuilder>();
builder.Services.AddSingleton<ISavedRatesLoaderService, SavedRatesLoaderService>();
builder.Services.AddSingleton<ICacheService, CacheService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
