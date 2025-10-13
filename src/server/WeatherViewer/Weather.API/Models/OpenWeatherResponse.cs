using System.Text.Json.Serialization;

namespace Weather.API.Models;

public class OpenWeatherResponse
{
	[JsonPropertyName("timezone")]
	public int TimeZone { get; set; }

	[JsonPropertyName("main")]
	public MainWeather Main { get; set; } = new();

	[JsonPropertyName("wind")]
	public Wind Wind { get; set; } = new();

	[JsonPropertyName("weather")]
	public List<WeatherInfo> Weather { get; set; } = new();
}

public class MainWeather
{
	[JsonPropertyName("temp")]
	public double Temp { get; set; }

	[JsonPropertyName("humidity")]
	public int Humidity { get; set; }
}

public class Wind
{
	[JsonPropertyName("speed")]
	public double Speed { get; set; }
}

public class WeatherInfo
{
	[JsonPropertyName("description")]
	public string Description { get; set; } = string.Empty;

	[JsonPropertyName("icon")]
	public string Icon { get; set; } = string.Empty;
}
