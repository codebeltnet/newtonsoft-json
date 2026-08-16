---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions
example:
- *content
---

ASP.NET Core applications frequently need to serialize framework types like `HttpExceptionDescriptor`, `ProblemDetails`, and `StringValues` in error responses, diagnostic logs, and middleware context. Out-of-the-box JSON.NET doesn't know how to handle these types efficiently, producing verbose or incorrect output that doesn't match REST API conventions. The `JsonConverterCollectionExtensions` class provides specialized converter registration methods for these ASP.NET Core-specific types, enabling clean, RFC-compliant serialization without custom marshaling. These extension methods enable seamless JSON serialization of ASP.NET Core framework types and improve error response handling with standardized problem details format. This example demonstrates adding converters for HTTP exception descriptors and observing the registered behavior:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Newtonsoft.Json;

namespace Examples;

class HttpExceptionDescriptorConverterExample
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Add HTTP exception descriptor converter with custom sensitivity settings
        settings.Converters.AddHttpExceptionDescriptorConverter(setup =>
        {
            setup.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
        });

        Console.WriteLine("HTTP exception converter registered");
    }
}
```

### Adding a Problem Details Converter

The following example demonstrates adding a converter for `ProblemDetails` responses:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Examples;

class ProblemDetailsConverterExample
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Add problem details converter for RFC 7807 responses
        settings.Converters.AddProblemDetailsConverter();

        var problemDetails = new ProblemDetails
        {
            Type = "https://example.com/errors/validation-failed",
            Title = "One or more validation errors occurred.",
            Status = 422,
            Detail = "The request body contains invalid data."
        };

        var json = JsonConvert.SerializeObject(problemDetails, settings);
        Console.WriteLine(json);
    }
}
```

### Adding a StringValues Converter

The following example demonstrates adding a converter for `StringValues` (HTTP header values):

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Examples;

class StringValuesConverterExample
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Add StringValues converter for serializing HTTP header collections
        settings.Converters.AddStringValuesConverter();

        var headerValues = new StringValues(new[] { "application/json", "text/plain" });

        var json = JsonConvert.SerializeObject(headerValues, settings);
        Console.WriteLine(json);
    }
}
```

These extension methods enable seamless JSON serialization of ASP.NET Core framework types and improve error response handling with standardized problem details format.

---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddHttpExceptionDescriptorConverter
example:
- *content
---

ASP.NET Core applications serving HTTP clients need to transform exceptions into standardized error responses that follow RFC 7807 Problem Details format or custom HTTP exception schemas. Raw exception serialization exposes internal details inappropriate for external clients, while under-reporting hides debugging information needed for internal diagnostics. The `AddHttpExceptionDescriptorConverter` method registers a converter for `HttpExceptionDescriptor` types that captures HTTP-specific error context—status codes, headers, content negotiation results—and respects environment-aware sensitivity settings to control which details appear in external versus internal error responses. This example demonstrates configuring the converter with message and stack trace details suitable for internal API clients:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Cuemon.Diagnostics;
using Newtonsoft.Json;

namespace Examples;

class AddHttpExceptionDescriptorConverterExample
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Add HTTP exception descriptor converter with custom sensitivity settings
        settings.Converters.AddHttpExceptionDescriptorConverter(setup =>
        {
            setup.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
        });

        Console.WriteLine("HTTP exception descriptor converter registered");
    }
}
```

---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddProblemDetailsConverter
example:
- *content
---

Modern REST APIs follow RFC 7807 Problem Details format for standardized error responses that clients and API gateways can parse, route, and handle consistently. The default JSON.NET serialization of `ProblemDetails` produces correct JSON but misses opportunities to integrate with custom error mapping, status code conventions, and content negotiation preferences. The `AddProblemDetailsConverter` method registers a converter for `ProblemDetails` that aligns serialization with RFC 7807 standards while enabling customization hooks for application-specific error details, type URIs, and validation error aggregation. This is essential for building APIs that provide rich, machine-readable error information to API clients and service meshes. This example demonstrates registering the converter for RFC-compliant error responses:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Examples;

class AddProblemDetailsConverterExample
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Add problem details converter for RFC 7807 responses
        settings.Converters.AddProblemDetailsConverter();

        var problemDetails = new ProblemDetails
        {
            Type = "https://example.com/errors/validation-failed",
            Title = "One or more validation errors occurred.",
            Status = 422,
            Detail = "The request body contains invalid data."
        };

        var json = JsonConvert.SerializeObject(problemDetails, settings);
        Console.WriteLine(json);
    }
}
```

---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddStringValuesConverter
example:
- *content
---

ASP.NET Core headers and query parameters are represented as `StringValues` collections for efficient multi-value handling, but standard JSON serialization produces verbose output unsuitable for diagnostics, logging, and API responses that need to include header information. Many applications need to serialize header sets, parameter collections, or accept-header preferences as JSON for request/response bodies, middleware context, or observability payloads. The `AddStringValuesConverter` method registers a converter for `StringValues` that produces compact, readable JSON arrays of header values, enabling applications to include HTTP header metadata in JSON structures without custom conversion logic. This example demonstrates registering the converter for serializing header collections:

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Examples;

class AddStringValuesConverterExample
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Add StringValues converter for serializing HTTP header collections
        settings.Converters.AddStringValuesConverter();

        var headerValues = new StringValues(new[] { "application/json", "text/plain" });

        var json = JsonConvert.SerializeObject(headerValues, settings);
        Console.WriteLine(json);
    }
}
```
