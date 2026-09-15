using System.Text.Json.Serialization;

namespace Weather_Forecast.Models;

public class CurrentWeatherResponse
{
    [JsonPropertyName("coord")]
    public Coord? Coord { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDescription>? Weather { get; set; }

    [JsonPropertyName("base")]
    public string? Base { get; set; }

    [JsonPropertyName("main")]
    public MainInfo? Main { get; set; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    [JsonPropertyName("wind")]
    public WindInfo? Wind { get; set; }

    [JsonPropertyName("clouds")]
    public CloudsInfo? Clouds { get; set; }

    [JsonPropertyName("rain")]
    public PrecipitationInfo? Rain { get; set; }

    [JsonPropertyName("snow")]
    public PrecipitationInfo? Snow { get; set; }

    [JsonPropertyName("dt")]
    public long Dt { get; set; }

    [JsonPropertyName("sys")]
    public SysInfo? Sys { get; set; }

    [JsonPropertyName("timezone")]
    public int Timezone { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("cod")]
    public int Cod { get; set; }
}

public class ForecastResponse
{
    [JsonPropertyName("cod")]
    public string? Cod { get; set; }

    [JsonPropertyName("message")]
    public double Message { get; set; }

    [JsonPropertyName("cnt")]
    public int Cnt { get; set; }

    [JsonPropertyName("list")]
    public List<ForecastItem> List { get; set; } = new();

    [JsonPropertyName("city")]
    public CityInfo? City { get; set; }
}

public class ForecastItem
{
    [JsonPropertyName("dt")]
    public long Dt { get; set; }

    [JsonPropertyName("main")]
    public MainInfo? Main { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDescription>? Weather { get; set; }

    [JsonPropertyName("clouds")]
    public CloudsInfo? Clouds { get; set; }

    [JsonPropertyName("wind")]
    public WindInfo? Wind { get; set; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    [JsonPropertyName("pop")]
    public double Pop { get; set; } // Probability of precipitation 0.0 - 1.0

    [JsonPropertyName("rain")]
    public PrecipitationInfo? Rain { get; set; }

    [JsonPropertyName("snow")]
    public PrecipitationInfo? Snow { get; set; }

    [JsonPropertyName("sys")]
    public ForecastSysInfo? Sys { get; set; }

    [JsonPropertyName("dt_txt")]
    public string? DtTxt { get; set; }
}

public class MainInfo
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; set; }

    [JsonPropertyName("sea_level")]
    public int SeaLevel { get; set; }

    [JsonPropertyName("grnd_level")]
    public int GrndLevel { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("temp_kf")]
    public double TempKf { get; set; }
}

public class WeatherDescription
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("main")]
    public string Main { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;
}

public class WindInfo
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    [JsonPropertyName("deg")]
    public int Deg { get; set; }

    [JsonPropertyName("gust")]
    public double Gust { get; set; }
}

public class CloudsInfo
{
    [JsonPropertyName("all")]
    public int All { get; set; }
}

public class PrecipitationInfo
{
    [JsonPropertyName("1h")]
    public double OneHour { get; set; }

    [JsonPropertyName("3h")]
    public double ThreeHour { get; set; }
}

public class SysInfo
{
    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("sunrise")]
    public long Sunrise { get; set; }

    [JsonPropertyName("sunset")]
    public long Sunset { get; set; }
}

public class ForecastSysInfo
{
    [JsonPropertyName("pod")]
    public string? Pod { get; set; }
}

public class CityInfo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("coord")]
    public Coord? Coord { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("population")]
    public long Population { get; set; }

    [JsonPropertyName("timezone")]
    public int Timezone { get; set; }

    [JsonPropertyName("sunrise")]
    public long Sunrise { get; set; }

    [JsonPropertyName("sunset")]
    public long Sunset { get; set; }
}

public class Coord
{
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }
}

public class DirectGeoLocation
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string? State { get; set; }
}

// Aggregated View Model for Daily Summary
public class DailyForecastSummary
{
    public DateTime Date { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public double MinTemp { get; set; }
    public double MaxTemp { get; set; }
    public int AvgHumidity { get; set; }
    public double MaxWindSpeed { get; set; }
    public double MaxPrecipitationProb { get; set; }
    public string WeatherMain { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public List<ForecastItem> HourlyItems { get; set; } = new();
}

public enum TemperatureUnit
{
    Celsius,
    Fahrenheit
}
