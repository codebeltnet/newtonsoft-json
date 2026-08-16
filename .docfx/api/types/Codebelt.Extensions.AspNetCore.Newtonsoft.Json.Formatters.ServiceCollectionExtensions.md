---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensions
example:
- *content
---

ASP.NET Core applications use the dependency injection system to register formatters, middleware, and controllers, but JSON serialization configuration often lives in multiple places—local formatter instantiation, static `JsonConvert` defaults, exception handlers—making it difficult to maintain consistency and test different configurations. The `ServiceCollectionExtensions` class provides chainable registration methods that add `NewtonsoftJsonFormatterOptions` and exception response formatters to the service container, enabling centralized configuration that can be injected into all components and overridden per-test. This patterns enables configuration-as-code and supports deployment scenarios where production, staging, and development have different sensitivity and formatting rules. This example demonstrates how to register and configure `NewtonsoftJsonFormatterOptions` in the service collection:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Examples;

class AddNewtonsoftJsonFormatterOptionsExample
{
    static void Main()
    {
        var services = new ServiceCollection();

        // Register JSON formatter options with custom configuration
        services.AddNewtonsoftJsonFormatterOptions(options =>
        {
            options.Settings.Formatting = Formatting.Indented;
            options.Settings.NullValueHandling = NullValueHandling.Ignore;
            options.Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SynchronizeWithJsonConvert = true;
        });

        var serviceProvider = services.BuildServiceProvider();
        var formatterOptions = serviceProvider.GetRequiredService<NewtonsoftJsonFormatterOptions>();
        
        Console.WriteLine("Newtonsoft.Json formatter options registered");
        Console.WriteLine($"Formatting: {formatterOptions.Settings.Formatting}");
        Console.WriteLine($"NullValueHandling: {formatterOptions.Settings.NullValueHandling}");
    }
}
```

### Adding Exception Response Formatter

Unhandled exceptions in ASP.NET Core controllers and middleware are caught by the exception handling middleware which can log them and return responses, but by default it returns HTML error pages unsuitable for API clients expecting JSON. Without a registered exception response formatter that understands JSON serialization, exceptions don't get the same treatment as successful responses—they skip custom converters, sensitivity rules, and formatting preferences configured elsewhere. The `AddNewtonsoftJsonExceptionResponseFormatter` method registers a formatter that participates in the standard exception handling pipeline and serializes exceptions using the configured Newtonsoft.Json settings and sensitivity rules. This example demonstrates how to register the Newtonsoft.Json exception response formatter:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
using Cuemon.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Examples;

class AddNewtonsoftJsonExceptionResponseFormatterExample
{
    static void Main()
    {
        var services = new ServiceCollection();

        // Register the exception response formatter with custom sensitivity settings
        services.AddNewtonsoftJsonExceptionResponseFormatter(options =>
        {
            options.Settings.Formatting = Formatting.Indented;
            options.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
        });

        var serviceProvider = services.BuildServiceProvider();
        Console.WriteLine("Exception formatter registered");
    }
}
```

These extension methods streamline the setup of JSON serialization and exception handling in ASP.NET Core applications by providing fluent, chainable configuration methods that integrate with the built-in dependency injection system.

---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensions.AddNewtonsoftJsonFormatterOptions
example:
- *content
---

ASP.NET Core applications that adopt Newtonsoft.Json for JSON serialization need a centralized place to configure converters, naming strategies, null value handling, and serialization defaults for injection into formatters and controllers. The `AddNewtonsoftJsonFormatterOptions` method registers `NewtonsoftJsonFormatterOptions` in the dependency injection container, allowing all components that consume JSON formatting—input formatters, output formatters, exception handlers, and service-to-service clients—to use consistent serialization rules. This prevents scattered configuration duplication and enables configuration inheritance patterns where base options are extended for specific use cases. This example demonstrates registering formatter options with camelCase naming and indented formatting suitable for development and API documentation:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Examples;

class AddNewtonsoftJsonFormatterOptionsMethodExample
{
    static void Main()
    {
        var services = new ServiceCollection();

        // Register JSON formatter options with custom configuration
        services.AddNewtonsoftJsonFormatterOptions(options =>
        {
            options.Settings.Formatting = Formatting.Indented;
            options.Settings.NullValueHandling = NullValueHandling.Ignore;
            options.Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SynchronizeWithJsonConvert = true;
        });

        var serviceProvider = services.BuildServiceProvider();
        Console.WriteLine("Formatter options registered");
    }
}
```

---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensions.AddNewtonsoftJsonExceptionResponseFormatter
example:
- *content
---

APIs need to transform application exceptions into standardized JSON error responses that follow RFC 7807 Problem Details format or organizational conventions, hiding implementation details from external clients while preserving diagnostics for internal use. Without centralized exception formatting, different endpoints expose inconsistent error structures, clients cannot reliably parse failures, and operational staff lack context for troubleshooting. The `AddNewtonsoftJsonExceptionResponseFormatter` method registers a middleware-compatible exception formatter that automatically converts unhandled exceptions to JSON responses using Newtonsoft.Json serialization, respecting environment-aware sensitivity settings to expose appropriate detail levels. This example demonstrates registering the formatter configured to expose message and stack trace for internal APIs:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
using Cuemon.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Examples;

class AddNewtonsoftJsonExceptionResponseFormatterMethodExample
{
    static void Main()
    {
        var services = new ServiceCollection();

        // Register the exception response formatter with custom sensitivity settings
        services.AddNewtonsoftJsonExceptionResponseFormatter(options =>
        {
            options.Settings.Formatting = Formatting.Indented;
            options.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
        });

        var serviceProvider = services.BuildServiceProvider();
        Console.WriteLine("Exception formatter registered");
    }
}
```
