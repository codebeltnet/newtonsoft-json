---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensions
example:
- *content
---

Both `AddNewtonsoftJsonExceptionResponseFormatter` and `AddNewtonsoftJsonFormatterOptions` extend `IServiceCollection` with the shared Newtonsoft.Json formatter options. The first also registers the exception-response formatter; the second leaves the registration at options-only scope.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddNewtonsoftJsonExceptionResponseFormatter(o =>
{
    o.Settings.Formatting = Newtonsoft.Json.Formatting.Indented;
});

var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;
Console.WriteLine(options.Settings.Formatting == Newtonsoft.Json.Formatting.Indented);

var services2 = new ServiceCollection();
services2.AddNewtonsoftJsonFormatterOptions(o =>
{
    o.SynchronizeWithJsonConvert = true;
});

var provider2 = services2.BuildServiceProvider();
var options2 = provider2.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;
Console.WriteLine(options2.SynchronizeWithJsonConvert);
```
