using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using StackExchange.Redis;
using WeatherAplication.Clients;
using WeatherAplication.Configs;
using WeatherAplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var redis = ConnectionMultiplexer.Connect("localhost:6379");
builder.Services.AddScoped(r => redis.GetDatabase());

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddHttpClient("weatherapi", client =>
{
    client.BaseAddress = new Uri("http://api.worldweatheronline.com/premium/v1/weather.ashx");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<WeatherClientConfig>(builder.Configuration.GetSection("WeatherClientConfig"));

builder.Services.AddScoped<IWeatherClient, WeatherClient>();

//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
//{
//    containerBuilder.RegisterMediatR(typeof(Program).Assembly);
//    containerBuilder.RegisterModule<ServicesModule>();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
