---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.MvcBuilderExtensions
example:
- *content
---

Call `MvcBuilderExtensions.AddNewtonsoftJsonFormatters` when an `IMvcBuilder` should register the shared Newtonsoft.Json options and move the Newtonsoft.Json input and output formatters to the front of MVC's formatter lists.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

var services = new ServiceCollection();
services.AddControllers()
    .AddNewtonsoftJsonFormatters(options =>
    {
        options.Settings.Formatting = Formatting.None;
    });

var provider = services.BuildServiceProvider();
var formatterOptions = provider.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;
var mvcOptions = new MvcOptions();

foreach (var configurator in provider.GetServices<IConfigureOptions<MvcOptions>>())
{
    configurator.Configure(mvcOptions);
}

Console.WriteLine(formatterOptions.Settings.Formatting);
Console.WriteLine(mvcOptions.OutputFormatters[0] is JsonSerializationOutputFormatter);
Console.WriteLine(mvcOptions.InputFormatters[0] is JsonSerializationInputFormatter);

var optionServices = new ServiceCollection();
optionServices.AddControllers()
    .AddNewtonsoftJsonFormattersOptions(options =>
    {
        options.SynchronizeWithJsonConvert = true;
    });

var optionProvider = optionServices.BuildServiceProvider();
var optionOnly = optionProvider.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;
Console.WriteLine(optionOnly.SynchronizeWithJsonConvert);
```
