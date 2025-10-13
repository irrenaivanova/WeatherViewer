namespace Weather.API.Models;

public class WeatherDto
{
	public double Temperature { get; set; }
	public string Description { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public int Humidity { get; set; }
	public double WindSpeed { get; set; }
	public string TimeZone { get; set; } = string.Empty;
}
