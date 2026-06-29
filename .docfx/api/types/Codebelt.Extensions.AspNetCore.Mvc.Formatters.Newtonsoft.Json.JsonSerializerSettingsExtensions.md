---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializerSettingsExtensions
example:
- *content
---

Use `JsonSerializerSettingsExtensions.Use<T>` when you already have a `JsonSerializerSettings` instance and want to copy a reusable formatter profile into it before MVC applies the settings to input and output formatters.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Cuemon.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var target = new JsonSerializerSettings();

target.Use<MvcJsonSerializerSettings>(settings =>
{
    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

Console.WriteLine(target.Formatting);
Console.WriteLine(target.NullValueHandling);
Console.WriteLine(target.ReferenceLoopHandling);
Console.WriteLine(target.ContractResolver is CamelCasePropertyNamesContractResolver);

sealed class MvcJsonSerializerSettings : JsonSerializerSettings, IParameterObject
{
    public MvcJsonSerializerSettings()
    {
        Formatting = Formatting.Indented;
        NullValueHandling = NullValueHandling.Ignore;
        ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
}
```

---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializerSettingsExtensions.Use``1(Newtonsoft.Json.JsonSerializerSettings,System.Action{``0})
example:
- *content
---

Call `Use<T>` on the target `JsonSerializerSettings` instance when a formatter profile already captures the JSON conventions you want MVC to reuse.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Cuemon.Configuration;
using Newtonsoft.Json;

var target = new JsonSerializerSettings();
target.Use<MvcJsonSerializerSettings>();

Console.WriteLine(target.Formatting);
Console.WriteLine(target.NullValueHandling);

sealed class MvcJsonSerializerSettings : JsonSerializerSettings, IParameterObject
{
    public MvcJsonSerializerSettings()
    {
        Formatting = Formatting.Indented;
        NullValueHandling = NullValueHandling.Ignore;
    }
}
```
