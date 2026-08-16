---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonSerializerSettingsExtensions
example:
- *content
---
The following example applies custom serializer settings to the default JSON serialization options.

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;

namespace Examples;

class JsonSerializerSettingsExtensionsExample
{
    static void Main()
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        settings.ApplyToDefaultSettings();
        Console.WriteLine("Custom settings applied to defaults");

        var obj = new { name = "Test", value = (string)null };
        var json = JsonConvert.SerializeObject(obj);
        Console.WriteLine(json);
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonSerializerSettingsExtensions.ApplyToDefaultSettings
example:
- *content
---

Applications that use `JsonConvert.SerializeObject` and `JsonConvert.DeserializeObject` static methods need a way to globally configure the default serialization behavior without directly modifying the static `JsonConvert.DefaultSettings` property. The `ApplyToDefaultSettings` extension method provides a clean API to register custom converters, null value handling, formatting, and other settings into the global default settings, ensuring all subsequent JSON.NET static method calls inherit the application's serialization preferences. This is essential for applications that mix static convenience methods with formatter instances and need consistent behavior across both paths. This example demonstrates applying custom date handling and null value ignoring to the global defaults:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;

namespace Examples;

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-dd",
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        settings.ApplyToDefaultSettings();
        Console.WriteLine("Custom settings are now default for JSON operations");
    }
}
```
