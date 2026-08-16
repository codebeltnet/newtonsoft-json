---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.MvcBuilderExtensions
example:
- *content
---
ASP.NET Core `AddControllers()` configures default MVC services with System.Text.Json by default, which doesn't support custom Newtonsoft.Json converters or environment-aware exception sensitivity. Switching to Newtonsoft.Json requires registering input/output formatters with consistent configuration. The `MvcBuilderExtensions` class provides chainable extension methods on `IMvcBuilder` that register Newtonsoft.Json formatters and apply `NewtonsoftJsonFormatterOptions`, ensuring all MVC endpoints use consistent JSON serialization behavior. This example demonstrates registering and configuring Newtonsoft.Json formatters:

```csharp
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Examples;

class Program
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        var mvcBuilder = builder.Services
            .AddControllers()
            .AddNewtonsoftJsonFormatters(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.All;
            });

        mvcBuilder.AddNewtonsoftJsonFormattersOptions(options =>
        {
            options.SensitivityDetails = FaultSensitivityDetails.All;
        });

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}
```
