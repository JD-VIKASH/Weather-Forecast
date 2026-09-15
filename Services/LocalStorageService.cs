using Microsoft.JSInterop;

namespace Weather_Forecast.Services;

public interface ILocalStorageService
{
    ValueTask<T?> GetItemAsync<T>(string key);
    ValueTask SetItemAsync<T>(string key, T data);
    ValueTask RemoveItemAsync(string key);
}

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<T?> GetItemAsync<T>(string key)
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            if (string.IsNullOrEmpty(json))
                return default;

            if (typeof(T) == typeof(string))
                return (T)(object)json;

            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return default;
        }
    }

    public async ValueTask SetItemAsync<T>(string key, T data)
    {
        try
        {
            if (data == null)
            {
                await RemoveItemAsync(key);
                return;
            }

            string json = typeof(T) == typeof(string) ? data.ToString()! : System.Text.Json.JsonSerializer.Serialize(data);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }
        catch
        {
            // Ignore storage errors in browser sandbox mode
        }
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch
        {
            // Ignore
        }
    }
}
