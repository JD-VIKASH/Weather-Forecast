using System.Net.Http.Json;
using System.Text.Json;
using Weather_Forecast.Models;

namespace Weather_Forecast.Services;

public class WeatherApiService : IWeatherApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.openweathermap.org";

    public WeatherApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CurrentWeatherResponse?> GetCurrentWeatherAsync(string city, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return GenerateMockCurrentWeather(city, unit);
        }

        try
        {
            string unitParam = unit == TemperatureUnit.Celsius ? "metric" : "imperial";
            string url = $"{BaseUrl}/data/2.5/weather?q={Uri.EscapeDataString(city)}&units={unitParam}&appid={apiKey}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new InvalidOperationException("Invalid API key provided. Please check your OpenWeatherMap API key.");
                }
                return GenerateMockCurrentWeather(city, unit);
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync(
                stream,
                WeatherJsonContext.Default.CurrentWeatherResponse,
                cancellationToken);

            return result;
        }
        catch (InvalidOperationException) { throw; }
        catch (Exception)
        {
            return GenerateMockCurrentWeather(city, unit);
        }
    }

    public async Task<CurrentWeatherResponse?> GetCurrentWeatherByCoordsAsync(double lat, double lon, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return GenerateMockCurrentWeather("Current Location", unit, lat, lon);
        }

        try
        {
            string unitParam = unit == TemperatureUnit.Celsius ? "metric" : "imperial";
            string url = $"{BaseUrl}/data/2.5/weather?lat={lat}&lon={lon}&units={unitParam}&appid={apiKey}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return GenerateMockCurrentWeather("Current Location", unit, lat, lon);
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync(
                stream,
                WeatherJsonContext.Default.CurrentWeatherResponse,
                cancellationToken);
        }
        catch (Exception)
        {
            return GenerateMockCurrentWeather("Current Location", unit, lat, lon);
        }
    }

    public async Task<ForecastResponse?> Get5DayForecastAsync(string city, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return GenerateMockForecast(city, unit);
        }

        try
        {
            string unitParam = unit == TemperatureUnit.Celsius ? "metric" : "imperial";
            string url = $"{BaseUrl}/data/2.5/forecast?q={Uri.EscapeDataString(city)}&units={unitParam}&appid={apiKey}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return GenerateMockForecast(city, unit);
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync(
                stream,
                WeatherJsonContext.Default.ForecastResponse,
                cancellationToken);
        }
        catch (Exception)
        {
            return GenerateMockForecast(city, unit);
        }
    }

    public async Task<ForecastResponse?> Get5DayForecastByCoordsAsync(double lat, double lon, TemperatureUnit unit, string? apiKey = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return GenerateMockForecast("Current Location", unit);
        }

        try
        {
            string unitParam = unit == TemperatureUnit.Celsius ? "metric" : "imperial";
            string url = $"{BaseUrl}/data/2.5/forecast?lat={lat}&lon={lon}&units={unitParam}&appid={apiKey}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return GenerateMockForecast("Current Location", unit);
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync(
                stream,
                WeatherJsonContext.Default.ForecastResponse,
                cancellationToken);
        }
        catch (Exception)
        {
            return GenerateMockForecast("Current Location", unit);
        }
    }

    public async Task<List<DirectGeoLocation>> SearchLocationsAsync(string query, string? apiKey = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<DirectGeoLocation>();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return FilterMockLocations(query);
        }

        try
        {
            string url = $"{BaseUrl}/geo/1.0/direct?q={Uri.EscapeDataString(query)}&limit=5&appid={apiKey}";
            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return FilterMockLocations(query);
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var results = await JsonSerializer.DeserializeAsync(
                stream,
                WeatherJsonContext.Default.ListDirectGeoLocation,
                cancellationToken);

            return results ?? FilterMockLocations(query);
        }
        catch
        {
            return FilterMockLocations(query);
        }
    }

    public async Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return false;

        try
        {
            string url = $"{BaseUrl}/data/2.5/weather?q=London&appid={apiKey}";
            using var response = await _httpClient.GetAsync(url, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public List<DailyForecastSummary> ProcessDailySummaries(ForecastResponse forecast, TemperatureUnit unit)
    {
        if (forecast?.List == null || forecast.List.Count == 0)
            return new List<DailyForecastSummary>();

        var groups = forecast.List
            .GroupBy(item => DateTimeOffset.FromUnixTimeSeconds(item.Dt).LocalDateTime.Date)
            .OrderBy(g => g.Key)
            .Take(5);

        var summaries = new List<DailyForecastSummary>();

        foreach (var group in groups)
        {
            var dayItems = group.ToList();
            var mainWeather = dayItems
                .GroupBy(i => i.Weather?.FirstOrDefault()?.Main ?? "Clear")
                .OrderByDescending(g => g.Count())
                .First().Key;

            var weatherDesc = dayItems.FirstOrDefault(i => i.Weather?.FirstOrDefault()?.Main == mainWeather)?.Weather?.FirstOrDefault();

            summaries.Add(new DailyForecastSummary
            {
                Date = group.Key,
                DayOfWeek = group.Key.ToString("ddd, MMM d"),
                MinTemp = dayItems.Min(i => i.Main?.TempMin ?? 0),
                MaxTemp = dayItems.Max(i => i.Main?.TempMax ?? 0),
                AvgHumidity = (int)dayItems.Average(i => i.Main?.Humidity ?? 0),
                MaxWindSpeed = dayItems.Max(i => i.Wind?.Speed ?? 0),
                MaxPrecipitationProb = dayItems.Max(i => i.Pop),
                WeatherMain = mainWeather,
                Description = weatherDesc?.Description ?? mainWeather,
                Icon = weatherDesc?.Icon ?? "01d",
                HourlyItems = dayItems
            });
        }

        return summaries;
    }

    #region Mock Data Generation
    private static readonly List<DirectGeoLocation> StaticLocations = new()
    {
        new DirectGeoLocation { Name = "London", Country = "GB", State = "England", Lat = 51.5074, Lon = -0.1278 },
        new DirectGeoLocation { Name = "Tokyo", Country = "JP", State = "Tokyo", Lat = 35.6762, Lon = 139.6503 },
        new DirectGeoLocation { Name = "New York", Country = "US", State = "New York", Lat = 40.7128, Lon = -74.0060 },
        new DirectGeoLocation { Name = "Paris", Country = "FR", State = "Île-de-France", Lat = 48.8566, Lon = 2.3522 },
        new DirectGeoLocation { Name = "Sydney", Country = "AU", State = "New South Wales", Lat = -33.8688, Lon = 151.2093 },
        new DirectGeoLocation { Name = "Reykjavik", Country = "IS", State = "Capital Region", Lat = 64.1466, Lon = -21.9426 },
        new DirectGeoLocation { Name = "Dubai", Country = "AE", State = "Dubai", Lat = 25.2048, Lon = 55.2708 },
        new DirectGeoLocation { Name = "Cairo", Country = "EG", State = "Cairo", Lat = 30.0444, Lon = 31.2357 },
        new DirectGeoLocation { Name = "San Francisco", Country = "US", State = "California", Lat = 37.7749, Lon = -122.4194 },
        new DirectGeoLocation { Name = "Mumbai", Country = "IN", State = "Maharashtra", Lat = 19.0760, Lon = 72.8777 },
        new DirectGeoLocation { Name = "Rio de Janeiro", Country = "BR", State = "Rio de Janeiro", Lat = -22.9068, Lon = -43.1729 }
    };

    private List<DirectGeoLocation> FilterMockLocations(string query)
    {
        var q = query.Trim().ToLowerInvariant();
        return StaticLocations
            .Where(l => l.Name.ToLowerInvariant().Contains(q) || l.Country.ToLowerInvariant().Contains(q))
            .ToList();
    }

    private CurrentWeatherResponse GenerateMockCurrentWeather(string city, TemperatureUnit unit, double lat = 51.5074, double lon = -0.1278)
    {
        int hash = Math.Abs(city.ToLowerInvariant().GetHashCode());
        var rand = new Random(hash);

        double baseTempC = (hash % 30) - 5; // -5°C to 25°C base
        if (city.Equals("Tokyo", StringComparison.OrdinalIgnoreCase)) baseTempC = 22;
        else if (city.Equals("Dubai", StringComparison.OrdinalIgnoreCase)) baseTempC = 36;
        else if (city.Equals("Reykjavik", StringComparison.OrdinalIgnoreCase)) baseTempC = 4;
        else if (city.Equals("Sydney", StringComparison.OrdinalIgnoreCase)) baseTempC = 19;
        else if (city.Equals("London", StringComparison.OrdinalIgnoreCase)) baseTempC = 17;

        double temp = unit == TemperatureUnit.Celsius ? baseTempC : (baseTempC * 9 / 5) + 32;
        double feels = temp + (rand.NextDouble() * 3 - 1.5);

        string[] conditions = { "Clear", "Clouds", "Rain", "Thunderstorm", "Snow", "Drizzle" };
        string mainCond = conditions[hash % conditions.Length];
        string desc = mainCond switch
        {
            "Clear" => "clear sky",
            "Clouds" => "broken clouds",
            "Rain" => "moderate rain",
            "Thunderstorm" => "thunderstorm with rain",
            "Snow" => "light snow",
            _ => "light intensity drizzle"
        };
        string icon = mainCond switch
        {
            "Clear" => "01d",
            "Clouds" => "03d",
            "Rain" => "10d",
            "Thunderstorm" => "11d",
            "Snow" => "13d",
            _ => "09d"
        };

        long nowSec = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return new CurrentWeatherResponse
        {
            Name = city,
            Coord = new Coord { Lat = lat, Lon = lon },
            Weather = new List<WeatherDescription>
            {
                new WeatherDescription { Id = 800, Main = mainCond, Description = desc, Icon = icon }
            },
            Main = new MainInfo
            {
                Temp = Math.Round(temp, 1),
                FeelsLike = Math.Round(feels, 1),
                TempMin = Math.Round(temp - 2, 1),
                TempMax = Math.Round(temp + 3, 1),
                Pressure = 1012 + (hash % 20),
                Humidity = 45 + (hash % 45),
                SeaLevel = 1012,
                GrndLevel = 1008
            },
            Wind = new WindInfo
            {
                Speed = Math.Round(2.5 + (hash % 10) * 0.8, 1),
                Deg = (hash * 37) % 360,
                Gust = Math.Round(4.0 + (hash % 10) * 1.1, 1)
            },
            Clouds = new CloudsInfo { All = 20 + (hash % 75) },
            Visibility = 10000,
            Sys = new SysInfo
            {
                Country = GetCountryForCity(city),
                Sunrise = nowSec - 21600,
                Sunset = nowSec + 21600
            },
            Dt = nowSec,
            Timezone = 3600 * ((hash % 12) - 5)
        };
    }

    private ForecastResponse GenerateMockForecast(string city, TemperatureUnit unit)
    {
        var currentWeather = GenerateMockCurrentWeather(city, unit);
        double baseTemp = currentWeather.Main?.Temp ?? 20;

        int hash = Math.Abs(city.ToLowerInvariant().GetHashCode());
        var rand = new Random(hash);

        var list = new List<ForecastItem>();
        DateTime startTime = DateTime.UtcNow.Date;

        for (int day = 0; day < 5; day++)
        {
            double dayDelta = Math.Sin(day * 1.2) * 4;
            for (int hour = 0; hour < 24; hour += 3)
            {
                DateTime dt = startTime.AddDays(day).AddHours(hour);
                double diurnalVar = -Math.Cos((hour / 24.0) * 2 * Math.PI) * 4;
                double t = baseTemp + dayDelta + diurnalVar + (rand.NextDouble() * 1.5 - 0.75);

                string[] conds = { "Clear", "Clouds", "Clouds", "Rain", "Clear" };
                string cond = conds[(hash + day + (hour / 6)) % conds.Length];
                string icon = cond switch
                {
                    "Clear" => (hour >= 6 && hour <= 18) ? "01d" : "01n",
                    "Clouds" => (hour >= 6 && hour <= 18) ? "03d" : "03n",
                    "Rain" => "10d",
                    _ => "02d"
                };

                double pop = cond == "Rain" ? 0.75 : (cond == "Clouds" ? 0.3 : 0.05);

                list.Add(new ForecastItem
                {
                    Dt = new DateTimeOffset(dt).ToUnixTimeSeconds(),
                    DtTxt = dt.ToString("yyyy-MM-dd HH:mm:ss"),
                    Main = new MainInfo
                    {
                        Temp = Math.Round(t, 1),
                        FeelsLike = Math.Round(t - 0.5, 1),
                        TempMin = Math.Round(t - 1.5, 1),
                        TempMax = Math.Round(t + 1.5, 1),
                        Pressure = 1010 + (hash % 15),
                        Humidity = Math.Clamp(50 + (int)(diurnalVar * -3) + (hash % 20), 25, 95)
                    },
                    Weather = new List<WeatherDescription>
                    {
                        new WeatherDescription { Main = cond, Description = cond.ToLowerInvariant(), Icon = icon }
                    },
                    Wind = new WindInfo
                    {
                        Speed = Math.Round(3.0 + (hash % 5) + Math.Sin(hour) * 2, 1),
                        Deg = (180 + hour * 15) % 360
                    },
                    Clouds = new CloudsInfo { All = cond == "Clear" ? 10 : 70 },
                    Visibility = 10000,
                    Pop = Math.Round(pop, 2),
                    Sys = new ForecastSysInfo { Pod = (hour >= 6 && hour <= 18) ? "d" : "n" }
                });
            }
        }

        return new ForecastResponse
        {
            Cod = "200",
            Cnt = list.Count,
            List = list,
            City = new CityInfo
            {
                Name = city,
                Country = GetCountryForCity(city),
                Population = 1000000,
                Timezone = 3600
            }
        };
    }

    private static string GetCountryForCity(string city)
    {
        var match = StaticLocations.FirstOrDefault(l => l.Name.Equals(city, StringComparison.OrdinalIgnoreCase));
        return match?.Country ?? "GLOBAL";
    }
    #endregion
}
