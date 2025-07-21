# 🌤️ Weather Forecast API

A minimal ASP.NET Core Web API that supports user registration, login with JWT authentication, and provides weather forecasts for a given city using a mocked service with in-memory caching.

---

## 📁 Project Structure

```
WeatherForecastAPI/
├── WeatherForecast.API/                    # Web API project
├── WeatherForecast.Application/            # Interfaces & DTOs
├── WeatherForecast.Domain/                 # Domain models
├── WeatherForecast.Infrastructure/         # Implementations (services, repos)
└── WeatherForecastAPI.Tests/              # Unit & integration tests
```

---

## 🚀 Getting Started

### 1. Clone the repository:
```bash
git clone https://github.com/yourname/weather-forecast-api.git
cd weather-forecast-api
```

### 2. Configure secrets:
Add your JWT secret settings to `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32Chars",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "DurationInMinutes": 60
  }
}
```

### 3. Run the app:
```bash
dotnet run --project WeatherForecast.API
```

---

## 🔐 Authentication Endpoints

| Method | Endpoint             | Description              |
| ------ | -------------------- | ------------------------ |
| POST   | `/api/auth/register` | Register a new user      |
| POST   | `/api/auth/login`    | Authenticate and get JWT |

**Sample Request:**
```json
{
  "username": "testuser",
  "password": "yourpassword"
}
```


## 🌦️ Weather Endpoint

| Method | Endpoint               | Description                 | Auth Required |
| ------ | ---------------------- | --------------------------- | ------------- |
| GET    | `/weather?city=London` | Get weather info for a city | ✅ Yes         |

Uses mocked weather service and caches results per city using IMemoryCache.

---

## 🧪 Testing

Run tests using:
```bash
dotnet test
```

**Covers:**
- ✅ User registration
- ✅ Login
- ✅ Weather forecast service

---

## 🛠 Technologies Used

- ASP.NET Core 8
- JWT Authentication
- In-memory User Repository
- In-memory Cache (IMemoryCache)
- Custom exception classes
- Manual password hashing using SHA256
- Moq + xUnit + FluentAssertions for testing

---

## 📬 Contact

For feedback or issues: abdelaliemhosny18@gmail.com
