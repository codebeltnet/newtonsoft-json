---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializationOutputFormatter
example:
- *content
---

ASP.NET Core MVC converts controller action return values to HTTP response bodies using output formatters, but without registering a Newtonsoft.Json-aware formatter, responses use the default System.Text.Json serializer which doesn't know about custom converters, exception descriptors, or exception sensitivity settings configured elsewhere. Objects with complex serialization requirements—exceptions, domain models with custom converters, flag enums—won't serialize correctly unless an output formatter that understands Newtonsoft.Json is available. The `JsonSerializationOutputFormatter` class integrates Newtonsoft.Json serialization into the MVC response pipeline, applying custom converters and settings configured in `NewtonsoftJsonFormatterOptions` to all response objects. This example demonstrates direct instantiation and usage of the JSON output formatter to serialize objects:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

namespace Examples;

public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int Temperature { get; set; }
    public string Summary { get; set; }
    public string Location { get; set; }
}

class JsonSerializationOutputFormatterExample
{
    static void Main()
    {
        // Create formatter with custom options
        var options = new NewtonsoftJsonFormatterOptions
        {
            Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            }
        };

        var outputFormatter = new JsonSerializationOutputFormatter(options);

        // Create object to serialize
        var forecast = new WeatherForecast
        {
            Date = DateTime.Now,
            Temperature = 25,
            Summary = "Warm",
            Location = "Seattle"
        };

        // The formatter is now registered and ready to handle JSON serialization
        // in an ASP.NET Core MVC application response pipeline
        var json = JsonConvert.SerializeObject(forecast, options.Settings);
        Console.WriteLine("Serialized WeatherForecast:");
        Console.WriteLine(json);
    }
}
```

The `JsonSerializationOutputFormatter` respects the formatter options and applies custom converters such as exception descriptor converters when serializing response objects. It supports the same media types as the input formatter and automatically handles content negotiation.
