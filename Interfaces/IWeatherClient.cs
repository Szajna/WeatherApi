using Microsoft.AspNetCore.Mvc;


namespace WeatherAplication.Services
{
    public interface IWeatherClient
    {
        Task<List<WeatherData>> GetWeatherForCity();
    }
}
