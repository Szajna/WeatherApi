using MediatR;
using WeatherAplication.Queries;
using WeatherAplication.Services;

namespace WeatherAplication.Handlers
{
    public class GetWeatherHandler : IRequestHandler<GetWeatherQuery, List<WeatherData>>
    {
        private readonly IWeatherClient _weatherClient;

        public GetWeatherHandler(IWeatherClient weatherClient)
        {
            _weatherClient = weatherClient;
        }
        async Task<List<WeatherData>> IRequestHandler<GetWeatherQuery, List<WeatherData>>.Handle(GetWeatherQuery request, CancellationToken cancellationToken)
        {
            return await _weatherClient.GetWeatherForCity();
        }
    }
}
