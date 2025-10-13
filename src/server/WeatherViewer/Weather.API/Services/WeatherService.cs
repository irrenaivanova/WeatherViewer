using Microsoft.Extensions.Options;
using System.Net;
using Weather.API.Models;

namespace Weather.API.Services;

public class WeatherService
{
	private readonly HttpClient client;
	private readonly WeatherSettings settings;

	public WeatherService(
		HttpClient client, 
		IOptions<WeatherSettings> options)
	{
		this.client = client;
		this.settings = options.Value;
	}

	public async Task<(WeatherDto? Data, HttpStatusCode StatusCode)> GetWeatherAsync(double lat, double lon)
	{
		try
		{
			//var urlOpen3 = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&exclude=minutely,hourly,daily,alerts&units=metric&appid={this.settings.ApiKey}";
			var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={this.settings.ApiKey}";

			using var response = await client.GetAsync(url);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				return (null, HttpStatusCode.Unauthorized);
			}

			if (!response.IsSuccessStatusCode)
			{
				return (null, response.StatusCode);
			}

			var content = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();

			if (content == null)
			{
				return (null, HttpStatusCode.BadGateway);
			}

			var dto = new WeatherDto
			{
				Temperature = content.Main.Temp,
				Description = content.Weather[0].Description,
				Icon = content.Weather[0].Icon,
				Humidity = content.Main.Humidity,
				WindSpeed = content.Wind.Speed,
				TimeZone = content.TimeZone.ToString()
			};

			return (dto, HttpStatusCode.OK);
		}
		catch
		{
			return (null, HttpStatusCode.BadGateway);
		}
	}
}
