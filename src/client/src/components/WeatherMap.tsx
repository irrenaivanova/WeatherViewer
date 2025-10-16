import { useEffect, useState } from "react";
import { MapContainer, TileLayer, Marker, useMapEvents } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import "./WeatherMap.css";

interface WeatherData {
  temperature: number;
  description: string;
  humidity: number;
  windSpeed: number;
  timeZone: string;
  icon: string;
}

const sofiaCoords = { lat: 42.6977, lon: 23.3219 };

export default function WeatherMap() {
  const [position, setPosition] = useState<{ lat: number; lon: number }>(
    sofiaCoords
  );
  const [weather, setWeather] = useState<WeatherData | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchWeather = async (lat: number, lon: number) => {
    try {
      setLoading(true);
      setError(null);
      const res = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/weather?lat=${lat}&lon=${lon}`
      );
      if (!res.ok) throw new Error(`Error: ${res.status}`);
      const data = await res.json();
      setWeather(data);
    } catch {
      setError("Failed to fetch weather data");
      setWeather(null);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (pos) => {
          const { latitude, longitude } = pos.coords;
          setPosition({ lat: latitude, lon: longitude });
          fetchWeather(latitude, longitude);
        },
        () => fetchWeather(sofiaCoords.lat, sofiaCoords.lon)
      );
    } else {
      fetchWeather(sofiaCoords.lat, sofiaCoords.lon);
    }
  }, []);

  function LocationMarker() {
    useMapEvents({
      click(e) {
        const { lat, lng } = e.latlng;
        setPosition({ lat, lon: lng });
        fetchWeather(lat, lng);
      },
    });

    return (
      <Marker
        position={[position.lat, position.lon]}
        draggable={true}
        eventHandlers={{
          dragend: (e) => {
            const marker = e.target;
            const { lat, lng } = marker.getLatLng();
            setPosition({ lat, lon: lng });
            fetchWeather(lat, lng);
          },
        }}
      />
    );
  }

  return (
    <div className="map-container">
      <MapContainer
        center={[position.lat, position.lon]}
        zoom={10}
        style={{ flex: 1 }}
      >
        <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
        <LocationMarker />
      </MapContainer>

      <div className="weather-panel">
        {loading && <p>Loading weather...</p>}
        {error && <p style={{ color: "red" }}>{error}</p>}
        {weather && (
          <div>
            <h4>
              Lat: {position.lat.toFixed(2)}, Lon: {position.lon.toFixed(2)}
            </h4>
            <p>{weather.temperature} °C</p>
            <p>Description: {weather.description}</p>
            <p>Humidity: {weather.humidity}%</p>
            <p>Wind: {weather.windSpeed} m/s</p>
            <img
              src={`https://openweathermap.org/img/wn/${weather.icon}@2x.png`}
              alt="weather icon"
            />
          </div>
        )}
      </div>
    </div>
  );
}
