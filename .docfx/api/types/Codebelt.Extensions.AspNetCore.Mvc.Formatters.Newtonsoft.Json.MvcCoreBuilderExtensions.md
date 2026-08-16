---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.MvcCoreBuilderExtensions
example:
- *content
---
`AddMvcCore()` provides minimal MVC services for advanced scenarios where you hand-select components, but doesn't automatically include input/output formatters—you must register them explicitly. Switching from System.Text.Json to Newtonsoft.Json with environment-aware exception sensitivity requires manually registering formatters and configuration. The `MvcCoreBuilderExtensions` class provides chainable extension methods on `IMvcCoreBuilder` that register Newtonsoft.Json formatters with consistent configuration, eliminating boilerplate and reducing integration errors. This example demonstrates registering and configuring Newtonsoft.Json formatters for advanced scenarios:

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

        var mvcCoreBuilder = builder.Services
            .AddMvcCore()
            .AddNewtonsoftJsonFormatters(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.All;
            });

        mvcCoreBuilder.AddNewtonsoftJsonFormattersOptions(options =>
        {
            options.SensitivityDetails = FaultSensitivityDetails.All;
        });

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}
```
