using MediatR;

namespace WeatherAplication.Queries
{
    public class GetWeatherQuery : IRequest<List<WeatherData>>
    {
    }
}
