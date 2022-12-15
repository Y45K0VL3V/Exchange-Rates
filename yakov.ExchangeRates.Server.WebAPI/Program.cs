using yakov.ExchangeRates.Server.Application;
using yakov.ExchangeRates.Server.Domain.Interfaces;
using yakov.ExchangeRates.Server.Infrastructure;
using yakov.ExchangeRates.Server.Infrastructure.FileServices;
using yakov.ExchangeRates.Server.Infrastructure.RemoteAPIServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRatesFileService, RateFileService>();
builder.Services.AddSingleton<IStorageService, StorageService>();
builder.Services.AddSingleton<RatesContext>();
builder.Services.AddSingleton<IRatesRepository, RatesRepository>();
builder.Services.AddSingleton<IAPIServiceBuilder, RatesAPIServiceBuilder>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
