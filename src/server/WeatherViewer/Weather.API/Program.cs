using Weather.API.Models;
using Weather.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WeatherSettings>(
	builder.Configuration.GetSection("OpenWeather"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<WeatherService>();

builder.Services.AddCors(options =>
{
	var frontendUrl = builder.Configuration["Frontend:AllowedOrigin"]!;
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins(frontendUrl)
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
