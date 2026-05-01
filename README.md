# dotnet-trim-strings

A simple scaffold .NET project that demonstrates how to use **JSON converters as filters** to automatically trim strings on every incoming API request — no manual `.Trim()` calls needed throughout your codebase.

## How It Works

.NET's `System.Text.Json` allows you to register custom `JsonConverter<T>` implementations globally. This project registers a `TrimStringConverter` that intercepts every `string` value during JSON deserialization and:

- **Trims** leading and trailing whitespace from the value.
- Returns **`null`** if the value is empty or whitespace-only.

The converter is registered once in `Program.cs`:

```csharp
builder.Services
    .AddControllers()
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
    });
```

## The Converter

```csharp
public class TrimStringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
```

## Behaviour

| Input JSON value | Deserialized result |
|------------------|---------------------|
| `"  John  "`     | `"John"`            |
| `"John "`        | `"John"`            |
| `""`             | `null`              |
| `"   "`          | `null`              |
| `null`           | `null`              |

## Project Structure

```
API/
  Program.cs                  # Registers TrimStringConverter globally
  TrimStringConverter.cs      # Custom JsonConverter<string> implementation
  Controllers/
    WeatherForecastController.cs
Tests/
  TrimStringConverterTests.cs # Unit tests for the converter
```

## Running the Project

```bash
dotnet run --project API
```

The API will be available with Swagger UI at `https://localhost:{port}/swagger`.

## Running the Tests

```bash
dotnet test
```
