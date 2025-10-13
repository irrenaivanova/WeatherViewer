using Microsoft.AspNetCore.Mvc;
using System.Net;
using Weather.API.Models;
using Weather.API.Services;

namespace Weather.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
	private const double SofiaLat = 42.6977;
	private const double SofiaLon = 23.3219;

	private readonly WeatherService weather;

	public HealthController(WeatherService weather)
	{
		this.weather = weather;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		(WeatherDto? data, HttpStatusCode statusCode) = await this.weather.GetWeatherAsync(SofiaLat, SofiaLon);

		if (statusCode == HttpStatusCode.Unauthorized)
		{
			return StatusCode(401, new
			{
				status = "Degraded",
				message = "Missing or invalid API key"
			});
		}

		if (data == null)
		{
			return StatusCode(502, new
			{
				status = "Degraded",
				message = "Cannot reach OpenWeather API"
			});
		}

		return Ok(new
		{
			status = "Healthy",
			message = "Weather API reachable for Sofia"
		});
	}
}
