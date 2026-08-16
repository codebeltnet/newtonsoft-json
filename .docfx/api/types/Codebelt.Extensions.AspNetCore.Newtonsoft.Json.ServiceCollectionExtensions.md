---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.ServiceCollectionExtensions
example:
- *content
---
ASP.NET Core applications need centralized exception response formatting that respects custom JSON serialization configuration. The `AddMinimalNewtonsoftJsonOptions` method registers an exception response formatter that applies Newtonsoft.Json serialization with configured sensitivity settings, ensuring consistent error responses across both controller-based and minimal API endpoints. This example demonstrates registering exception response formatter options:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Examples;

class Program
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddMinimalNewtonsoftJsonOptions(options =>
        {
            options.SensitivityDetails = FaultSensitivityDetails.All;
        });

        var app = builder.Build();
        Console.WriteLine("Newtonsoft.Json exception response formatter registered");
        app.Run();
    }
}
```
