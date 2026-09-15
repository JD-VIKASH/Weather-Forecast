# ☀️ AeroCast - Weather & 5-Day Forecast App

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor WASM](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![C# 12/14](https://img.shields.io/badge/C%23-12%2F14-239120?logo=c-sharp)](https://docs.microsoft.com/dotnet/csharp/)
[![OpenWeatherMap API](https://img.shields.io/badge/API-OpenWeatherMap-orange)](https://openweathermap.org/api)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A modern, high-performance **Weather & 5-Day Forecast Web Application** built with **C# / .NET 8 Blazor WebAssembly**, `HttpClient`, **System.Text.Json Source Generators**, and the **OpenWeatherMap REST API**.

AeroCast features real-time weather monitoring, interactive SVG trend charts, a 5-day / 3-hour forecast breakdown, city search with auto-complete, unit toggling (°C/°F), weather-responsive dynamic ambient backgrounds, and full mock data fallback when an API key is not present.

---

## ✨ Features

- **🌐 REST API Consumption**: Consumes OpenWeatherMap REST endpoints (`/weather`, `/forecast`, `/geo/1.0/direct`) using typed `HttpClient`.
- **⚡ System.Text.Json Source Generation**: High-performance, zero-reflection JSON deserialization powered by `[JsonSourceGenerationOptions]` and custom `JsonSerializerContext`.
- **📈 Interactive SVG Trend Chart**: Custom SVG curve displaying 5-day / 3-hour temperature trends, rain probability bars, hover tooltips, and data markers.
- **📅 5-Day Forecast & Hourly Breakdown**: Daily outlook cards paired with a horizontal 3-hour breakdown slider including wind direction compasses and rain badges.
- **🧭 Detailed Environmental Metrics**: Cards for Feels-Like temperature, Humidity, Dew Point estimation, Wind speed/gusts, Atmospheric pressure (hPa), Visibility, and Sun schedule (Sunrise/Sunset).
- **🔑 OpenWeatherMap API Key Manager**: Modal dialog to enter and test live OpenWeatherMap API keys, or switch to **Interactive Demo Mode** with one click.
- **🎨 Glassmorphism & Animated Themes**: Vibrant dark mode UI with weather-adaptive ambient background effects (sunny glow, rain drops, snow flakes, drifting clouds).
- **💾 Local Storage Persistence**: Saves unit preferences (°C / °F), recent city search history, and API key settings in browser `localStorage`.

---

## 🛠️ Tech Stack & Concepts Reinforced

- **Language**: C# 12 / C# 14
- **Framework**: Blazor WebAssembly (.NET 8.0)
- **Networking**: `System.Net.Http.HttpClient` via Dependency Injection
- **JSON Serialization**: `System.Text.Json` Source Generators (`WeatherJsonContext`)
- **API Provider**: [OpenWeatherMap API](https://openweathermap.org/api) (Free Tier)
- **Styling**: Vanilla CSS3 Design System with Glassmorphism, CSS Grid/Flexbox, and SVG animations
- **State & Storage**: `IJSRuntime` LocalStorage interop

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later installed.

### Installation & Execution

1. **Clone the repository**:
   ```bash
   git clone https://github.com/vikash-JD/Weather-Forecast.git
   cd Weather-Forecast
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run locally**:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to:
   ```
   http://localhost:5000  (or the port printed in the terminal)
   ```

---

## 🔑 OpenWeatherMap API Key Setup

1. Register for a free API key at [openweathermap.org/api](https://openweathermap.org/api).
2. Launch the AeroCast application.
3. Click the **⚙️ Gear / API Settings** icon in the top navbar.
4. Enter your API Key and click **Save & Refresh**.
5. *Note: If no API key is entered, AeroCast automatically runs in **Interactive Demo Mode** with realistic weather datasets for global cities.*

---

## 📂 Project Structure

```
Weather_Forecast/
├── Components/
│   ├── ApiKeyModal.razor        # API Key settings dialog & Demo Mode switch
│   ├── HourlyForecastList.razor # 3-hour interval breakdown slider
│   ├── WeatherChart.razor       # SVG Temperature & Rain trend line chart
│   └── WeatherMetricsGrid.razor # Environmental metrics cards
├── Models/
│   └── WeatherData.cs           # OpenWeatherMap JSON response & view models
├── Pages/
│   └── Home.razor               # Main Weather Dashboard view
├── Services/
│   ├── IWeatherApiService.cs    # Service interface
│   ├── WeatherApiService.cs     # REST HttpClient & Mock provider
│   ├── WeatherJsonContext.cs    # System.Text.Json Source Generator Context
│   └── LocalStorageService.cs   # Browser localStorage JSInterop wrapper
├── Layout/
│   ├── MainLayout.razor         # Root layout wrapper
│   └── NavMenu.razor            # Navigation menu
├── wwwroot/
│   ├── css/app.css              # Custom CSS design system & weather themes
│   └── index.html               # Main HTML entry point
├── Program.cs                   # Blazor WebAssembly entry point & DI configuration
└── Weather_Forecast.csproj      # C# Project configuration (.NET 8.0)
```

---

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
