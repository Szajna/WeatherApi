using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using WeatherAplication.Configs;
using WeatherAplication.Services;

namespace WeatherAplication.Clients
{
    public class WeatherClient : IWeatherClient
    {
        private readonly IDatabase _database;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherClientConfig _clientConfig;
        private TimeSpan GetTimeSpanUntilNextDay(int hour) => new DateTime(DateTime.Now.Date.AddDays(1).Ticks).AddHours(hour) - DateTime.Now;

        public WeatherClient(IDatabase database, IHttpClientFactory httpClientFactory, IOptions<WeatherClientConfig> clientConfig)
        {
            _database = database;
            _httpClientFactory = httpClientFactory;
            _clientConfig = clientConfig.Value;
        }

        public async Task<List<WeatherData>> GetWeatherForCity()
        {
            var client = _httpClientFactory.CreateClient("weatherapi");
            var weatherDatas = new List<WeatherData>(_clientConfig.Cities.Count());

            foreach (var city in _clientConfig.Cities)
            {
                if (!_database.KeyExists(city))
                {
                    HttpResponseMessage responseMessage = await client
                                            .GetAsync($"?key={_clientConfig.WeatherApiKey}&q={city}&fx=yes&cc=no&mca=no&date=today&format=json");

                    responseMessage.EnsureSuccessStatusCode();
                    string responseBody = await responseMessage.Content.ReadAsStringAsync();

                    weatherDatas.Add(WeatherData.FromJson(responseBody));

                    _database.StringSet(city, responseBody);
                    //_database.KeyExpire(city, TimeSpan.FromHours(24));
                    _database.KeyExpire(city, GetTimeSpanUntilNextDay(6));
                }
                else
                {
                    weatherDatas.Add(WeatherData.FromJson(_database.StringGet(city)));
                }
            }

            return weatherDatas;
        }
    }
}
