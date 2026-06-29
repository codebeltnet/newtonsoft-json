---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.MvcCoreBuilderExtensions
example:
- *content
---

Call `MvcCoreBuilderExtensions.AddNewtonsoftJsonFormattersOptions` when an `IMvcCoreBuilder` already owns the MVC core services and you need the shared Newtonsoft.Json formatter options plus the exception-response formatter without inserting the MVC input and output formatters.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddMvcCore()
    .AddNewtonsoftJsonFormatters(options =>
    {
        options.Settings.DateFormatString = "yyyy-MM-dd";
    });

var provider = services.BuildServiceProvider();
var formatterOptions = provider.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;

Console.WriteLine(formatterOptions.Settings.DateFormatString);

var optionServices = new ServiceCollection();
optionServices.AddMvcCore()
    .AddNewtonsoftJsonFormattersOptions(options =>
    {
        options.SynchronizeWithJsonConvert = true;
    });

var optionProvider = optionServices.BuildServiceProvider();
var optionOnly = optionProvider.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;

Console.WriteLine(optionOnly.SynchronizeWithJsonConvert);
```
