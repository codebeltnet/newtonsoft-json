---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializationInputFormatter
example:
- *content
---

ASP.NET Core MVC model binding automatically deserializes incoming JSON request bodies to controller action parameters, but only when the appropriate input formatter is registered that understands JSON.NET configuration, custom converters, and exception sensitivity rules. Without proper input formatter setup, JSON deserialization defaults to the framework's built-in serializer or misses custom converters registered with Newtonsoft.Json. The `JsonSerializationInputFormatter` class integrates Newtonsoft.Json with ASP.NET Core's formatter pipeline, supporting custom converters, null value handling, and media type negotiation as configured in `NewtonsoftJsonFormatterOptions`. This example demonstrates direct instantiation and usage of the JSON input formatter to deserialize JSON data:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

namespace Examples;

public class DataModel
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string[] Tags { get; set; }
}

class JsonSerializationInputFormatterExample
{
    static void Main()
    {
        // Create formatter with custom options
        var options = new NewtonsoftJsonFormatterOptions();
        var inputFormatter = new JsonSerializationInputFormatter(options);

        // The formatter is now registered and ready to handle JSON deserialization
        // in an ASP.NET Core MVC application. It integrates with the framework's
        // input formatter pipeline to deserialize incoming HTTP request bodies.

        var json = @"{""name"":""Test Data"",""createdAt"":""2024-01-01T00:00:00Z"",""tags"":[""important"",""review""]}";
        var deserialized = JsonConvert.DeserializeObject<DataModel>(json, options.Settings);
        
        Console.WriteLine($"Deserialized: {deserialized.Name}");
    }
}
```

The `JsonSerializationInputFormatter` automatically registers support for multiple JSON media types (`application/json`, `text/json`, and `application/problem+json`) and handles UTF-8 and UTF-16 encodings. The formatter integrates with the configured `NewtonsoftJsonFormatterOptions` to apply custom converters and serialization rules, including HTTP exception descriptor conversion when configured.
