using System.Text.Json.Serialization;
using Weather_Forecast.Models;

namespace Weather_Forecast.Services;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(CurrentWeatherResponse))]
[JsonSerializable(typeof(ForecastResponse))]
[JsonSerializable(typeof(ForecastItem))]
[JsonSerializable(typeof(List<ForecastItem>))]
[JsonSerializable(typeof(List<DirectGeoLocation>))]
[JsonSerializable(typeof(DirectGeoLocation))]
[JsonSerializable(typeof(Coord))]
[JsonSerializable(typeof(MainInfo))]
[JsonSerializable(typeof(WeatherDescription))]
[JsonSerializable(typeof(WindInfo))]
[JsonSerializable(typeof(CloudsInfo))]
[JsonSerializable(typeof(PrecipitationInfo))]
[JsonSerializable(typeof(SysInfo))]
[JsonSerializable(typeof(CityInfo))]
public partial class WeatherJsonContext : JsonSerializerContext
{
}
