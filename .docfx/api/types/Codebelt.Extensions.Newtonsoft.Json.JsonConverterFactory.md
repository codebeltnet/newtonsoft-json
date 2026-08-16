---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonConverterFactory
example:
- *content
---

Creating custom JSON converters typically requires subclassing `JsonConverter<T>` and overriding `WriteJson` and `ReadJson` methods, which introduces boilerplate code and tight coupling to the converter base class. For simple conversions that don't require complex state management, full subclassing is overkill. The `JsonConverterFactory` provides a lightweight factory pattern that creates converters from simple delegate functions—one for writing objects to JSON and one for reading from JSON. This enables ad-hoc converter creation without subclassing, supporting quick customization for domain-specific types, legacy formats, and compatibility scenarios. This example demonstrates creating a custom converter for a coordinate type:

```csharp
using System;
using System.Globalization;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Examples;

public class Coordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Coordinate() { }
    public Coordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class ConverterFactoryExample
{
    static void Main()
    {
        // Create a converter that serializes Coordinate as "lat,lng"
        var converter = JsonConverterFactory.Create<Coordinate>(
            writer: (jw, coord, serializer) =>
            {
                jw.WriteValue($"{coord.Latitude:F4},{coord.Longitude:F4}");
            },
            reader: (jr, type, currentValue, serializer) =>
            {
                if (jr.TokenType == JsonToken.String && jr.Value is string coordStr)
                {
                    var parts = coordStr.Split(',');
                    if (parts.Length == 2 &&
                        double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var lng))
                    {
                        return new Coordinate(lat, lng);
                    }
                }
                return currentValue;
            }
        );

        var settings = new JsonSerializerSettings();
        settings.Converters.Add(converter);

        var location = new Coordinate(51.5074, -0.1278); // London
        var json = JsonConvert.SerializeObject(location, settings);
        
        Console.WriteLine($"Serialized coordinate: {json}");
        
        var deserialized = JsonConvert.DeserializeObject<Coordinate>(json, settings);
        Console.WriteLine($"Deserialized: Latitude={deserialized.Latitude}, Longitude={deserialized.Longitude}");
    }
}
```

The `JsonConverterFactory` accepts write and read delegates that implement custom serialization logic for a specific type. The factory creates a `JsonConverter` instance that can be registered in `JsonSerializerSettings.Converters` and will intercept serialization and deserialization of that type using the provided delegates.
