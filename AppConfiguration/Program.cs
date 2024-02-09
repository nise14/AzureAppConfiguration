using System.ComponentModel;
using AppConfiguration;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddAzureAppConfiguration(options =>
{
    var credential = new DefaultAzureCredential();
    var appUri = builder.Configuration.GetConnectionString("appConfiguration");
    options.Connect(new Uri(appUri), credential);
    //Endpoint=https://demo2023.azconfig.io;Id=naQw;Secret=g5SuI6O6QRQepWpcbR5SsH9QOq2dwhZGW6SrRwTt4T4="
    // options.Connect(builder.Configuration.GetConnectionString("appConfiguration"));
    options.Select("weather:*", labelFilter: LabelFilter.Null);
    options.ConfigureRefresh(refresh =>
    {

        refresh.Register("refreshAll", refreshAll: true)
        .SetCacheExpiration(TimeSpan.FromSeconds(5));
    });
});

builder.Services.AddAzureAppConfiguration();
builder.Services.Configure<WeatherConfiguration>(builder.Configuration.GetSection("weather"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseAzureAppConfiguration();

app.Run();
