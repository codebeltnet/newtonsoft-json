---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializerSettingsExtensions
example:
- *content
---

API frameworks often define JSON serialization settings in configuration classes but need to propagate those settings to multiple target instances—formatters, exception handlers, response processors—without duplicating configuration logic. Direct property assignment is error-prone and doesn't scale when you have dozens of settings to copy. The `Use<T>` extension method solves this by copying all serialization properties from a configured source type to a target instance, enabling consistent behavior across your entire request/response pipeline. This example demonstrates how to use the `Use<T>` method to apply custom JSON serializer settings to an existing `JsonSerializerSettings` instance:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Cuemon.Configuration;
using Newtonsoft.Json;

namespace Examples;

class CustomSettings : JsonSerializerSettings, IParameterObject
{
    public CustomSettings()
    {
        Formatting = Formatting.Indented;
        NullValueHandling = NullValueHandling.Ignore;
    }
}

class JsonSerializerSettingsExtensionsExample
{
    static void Main()
    {
        var customSettings = new CustomSettings();
        var targetSettings = new JsonSerializerSettings();
        
        // Use the Use<T> method to copy settings from CustomSettings to target
        targetSettings.Use<CustomSettings>();
        
        Console.WriteLine($"Target settings formatting: {targetSettings.Formatting}");
        Console.WriteLine($"Settings have been synchronized from CustomSettings");
    }
}
```

---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializerSettingsExtensions.Use
example:
- *content
---

ASP.NET Core MVC applications often need to share JSON serialization configuration across multiple components—input formatters, output formatters, exception handlers, and custom serialization points—to maintain consistency. Without a centralized way to propagate settings, developers duplicate configuration code or resort to static global settings that are difficult to test and override. The `Use<T>` extension method enables configuration inheritance by copying all serialization properties from a configured source settings instance to a target instance, supporting optional custom setup delegates that refine settings before application. This pattern simplifies building consistent serialization behavior across your API without duplication. This example demonstrates applying custom settings from a configuration class:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Cuemon.Configuration;
using Newtonsoft.Json;

namespace Examples;

class CustomSettings : JsonSerializerSettings, IParameterObject
{
    public CustomSettings()
    {
        Formatting = Formatting.Indented;
        NullValueHandling = NullValueHandling.Ignore;
    }
}

class UseMethodExample
{
    static void Main()
    {
        var targetSettings = new JsonSerializerSettings();

        // Use the Use<T> method to copy settings from CustomSettings to target
        targetSettings.Use<CustomSettings>();

        Console.WriteLine($"Settings have been applied from CustomSettings");
    }
}
```
