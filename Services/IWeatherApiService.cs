using Weather_Forecast.Models;

namespace Weather_Forecast.Services;

public interface IWeatherApiService
{
    Task<CurrentWeatherResponse?> GetCurrentWeatherAsync(string city, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default);
    Task<CurrentWeatherResponse?> GetCurrentWeatherByCoordsAsync(double lat, double lon, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default);
    Task<ForecastResponse?> Get5DayForecastAsync(string city, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default);
    Task<ForecastResponse?> Get5DayForecastByCoordsAsync(double lat, double lon, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default);
    Task<List<DirectGeoLocation>> SearchLocationsAsync(string query, string? apiKey = null, CancellationToken cancellationToken = default);
    Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default);
    List<DailyForecastSummary> ProcessDailySummaries(ForecastResponse forecast, TemperatureUnit unit);
}
