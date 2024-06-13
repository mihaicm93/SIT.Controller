using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


public class WeatherConfig
{
    [JsonPropertyName("acceleration")]
    public int Acceleration { get; set; }

    [JsonPropertyName("forceWinterEvent")]
    public bool ForceWinterEvent { get; set; }

    public Weather Weather { get; set; }
}

public class Weather
{
    public WeatherDetails Clouds { get; set; }
    public WeatherDetails WindSpeed { get; set; }
    public WeatherDetails WindDirection { get; set; }
    public Range WindGustiness { get; set; }
    public WeatherDetails Rain { get; set; }
    public Range RainIntensity { get; set; }
    public WeatherDetails Fog { get; set; }
    public Range Temp { get; set; }
    public Range Pressure { get; set; }
}

public class WeatherDetails
{
    public float[] Values { get; set; }
    public int[] Weights { get; set; }
}

public class Range
{
    public float Min { get; set; }
    public float Max { get; set; }
}


public class WeatherConfigService
{
    private readonly string weatherConfigPath;

    public string WeatherConfigPath => weatherConfigPath;

    public WeatherConfigService(IConfiguration configuration)
    {
        var serverPath = configuration["ServerPath"];
        weatherConfigPath = Path.Combine(serverPath, "Aki_Data", "Server", "configs", "weather.json");
    }

    public void UpdateField<T>(string fieldPath, T value)
    {
        if (!File.Exists(weatherConfigPath))
        {
            throw new FileNotFoundException("Configuration file not found.");
        }

        var jsonString = File.ReadAllText(weatherConfigPath);
        var jsonNode = JsonNode.Parse(jsonString);

        if (jsonNode == null)
        {
            throw new InvalidOperationException("Failed to parse the configuration file.");
        }

        var segments = fieldPath.Split('.');
        var current = jsonNode;

        for (int i = 0; i < segments.Length - 1; i++)
        {
            current = current[segments[i]] ??= new JsonObject();
        }

        current[segments[^1]] = JsonValue.Create(value);

        File.WriteAllText(weatherConfigPath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

    public WeatherConfig LoadWeatherConfig()
    {
        if (!File.Exists(weatherConfigPath))
        {
            throw new FileNotFoundException("Configuration file not found.");
        }

        var jsonString = File.ReadAllText(weatherConfigPath);
        var weatherConfig = JsonSerializer.Deserialize<WeatherConfig>(jsonString);

        if (weatherConfig == null)
        {
            throw new InvalidOperationException("Failed to deserialize the configuration file.");
        }

        return weatherConfig;
    }
}
