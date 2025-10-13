using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Weather.API.Models;
using Weather.API.Services;

namespace Weather.API.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
	private readonly WeatherService weather;

	public WeatherController(WeatherService weather)
	{
		this.weather = weather;
	}

	[HttpGet]
	public async Task<IActionResult> Get(double lat, double lon)
	{
		if (lat < -90 || lat > 90 || lon < -180 || lon > 180)
		{
			return StatusCode(401, new { message = "Invalid coordinates" });
		}

		(WeatherDto? data, HttpStatusCode statusCode) = await this.weather.GetWeatherAsync(lat, lon);

		if (statusCode == HttpStatusCode.Unauthorized)
		{
			return StatusCode(401, new { message = "Missing or invalid API key" });
		}

		if (data == null)
		{
			return StatusCode(502, new { message = "Weather API error" });
		}

		return Ok(data);
	}
}
