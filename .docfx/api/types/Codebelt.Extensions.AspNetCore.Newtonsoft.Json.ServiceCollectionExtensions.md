---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.ServiceCollectionExtensions
example:
- *content
---

`ServiceCollectionExtensions.AddMinimalNewtonsoftJsonOptions` is the shortest path to wire Newtonsoft.Json into ASP.NET Core dependency injection. It registers `NewtonsoftJsonFormatterOptions` and the `IHttpExceptionDescriptorResponseFormatter` in one call.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddMinimalNewtonsoftJsonOptions(o =>
{
    o.Settings.Formatting = Newtonsoft.Json.Formatting.Indented;
    o.Settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});

var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;
Console.WriteLine(options.Settings.Formatting == Newtonsoft.Json.Formatting.Indented);
```
