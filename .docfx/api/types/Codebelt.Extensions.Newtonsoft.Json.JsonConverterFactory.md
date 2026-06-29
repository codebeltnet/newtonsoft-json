---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonConverterFactory
example: [*content]
---

## Examples

`JsonConverterFactory` creates `JsonConverter` instances from delegates without defining a custom converter class. Call `Create<T>` to make a converter for a specific type with writer and optional reader lambdas. Use `Create(Func<Type, bool>, ...)` for a custom `CanConvert` predicate.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;

var settings = new JsonSerializerSettings();
settings.Converters.Add(JsonConverterFactory.Create<DateTime>(
    (writer, value, serializer) => writer.WriteValue(value.ToString("O"))));

var json = JsonConvert.SerializeObject(DateTime.UtcNow, settings);
Console.WriteLine(json.StartsWith("\"") && json.EndsWith("\""));

var settings2 = new JsonSerializerSettings();
settings2.Converters.Add(JsonConverterFactory.Create(
    t => t.IsEnum,
    (writer, value, serializer) => writer.WriteValue(value.ToString()?.ToLowerInvariant())));

var json2 = JsonConvert.SerializeObject(StringComparison.OrdinalIgnoreCase, settings2);
Console.WriteLine(json2 == "\"ordinalignorecase\"");
```
