---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializationMvcOptionsSetup
example:
- *content
---

Use `JsonSerializationMvcOptionsSetup` when you want `MvcOptions` to prefer the Newtonsoft.Json formatters first while preserving any later formatters MVC already knows about.

```csharp
// Program.cs
using System;
using System.Linq;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

var services = new ServiceCollection();
services.Configure<NewtonsoftJsonFormatterOptions>(options => options.Settings.Formatting = Formatting.None);
services.AddSingleton<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>();

var provider = services.BuildServiceProvider();
var mvcOptions = new MvcOptions();

foreach (var configurator in provider.GetServices<IConfigureOptions<MvcOptions>>())
{
    configurator.Configure(mvcOptions);
}

Console.WriteLine(mvcOptions.OutputFormatters[0] is JsonSerializationOutputFormatter);
Console.WriteLine(mvcOptions.InputFormatters[0] is JsonSerializationInputFormatter);
Console.WriteLine(mvcOptions.OutputFormatters.OfType<JsonSerializationOutputFormatter>().Count());
```
