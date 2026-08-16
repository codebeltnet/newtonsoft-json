---
uid: Codebelt.Extensions.Newtonsoft.Json.Formatters.NewtonsoftJsonFormatterOptions
example:
- *content
---

Configuring JSON serialization requires setting many independent properties—formatting style, null handling, property naming conventions, custom converters—and these settings must be consistently applied across input formatters, output formatters, and exception handlers. Configuring each instance separately is error-prone and hides consistency issues until runtime. The `NewtonsoftJsonFormatterOptions` class centralizes JSON configuration in a single, immutable options object that can be validated, tested, and shared across all formatter instances in your application. This example demonstrates configuring formatter options with common serialization settings:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyApplication
{
    public class OptionsConfigProgram
    {
        public static void Main()
        {
            var options = new NewtonsoftJsonFormatterOptions();

            // Configure JSON serialization settings
            options.Settings.Formatting = Formatting.Indented;
            options.Settings.NullValueHandling = NullValueHandling.Ignore;
            options.Settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            options.Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Configure sensitivity for error responses
            options.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;

            var formatter = new NewtonsoftJsonFormatter(options);
            Console.WriteLine("Formatter options configured");
        }
    }
}
```

### Customizing Media Types

ASP.NET Core uses content negotiation to select input/output formatters based on request Accept headers and response Content-Type, but the default media type list (application/json, text/json, application/problem+json) may not match custom or vendor-specific media types your API supports. Without adding custom media types to the formatter, valid requests with "application/vnd.api+json" or other conventions are rejected even though your formatter could handle them. The `SupportedMediaTypes` property lets you extend the default list to match your API's contract, enabling content negotiation to succeed for additional media types. This example demonstrates configuring supported media types:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace MyApplication
{
    public class MediaTypeProgram
    {
        public static void Main()
        {
            var options = new NewtonsoftJsonFormatterOptions();

            // Default media types are application/json, text/json, and application/problem+json
            // Customize if needed
            options.SupportedMediaTypes = new List<MediaTypeHeaderValue>
            {
                new MediaTypeHeaderValue("application/json"),
                new MediaTypeHeaderValue("application/vnd.api+json"),
                new MediaTypeHeaderValue("text/json")
            };

            var formatter = new NewtonsoftJsonFormatter(options);
            Console.WriteLine($"Configured {options.SupportedMediaTypes.Count} media types");

            foreach (var mediaType in options.SupportedMediaTypes)
            {
                Console.WriteLine($"  - {mediaType.MediaType}");
            }
        }
    }
}
```

### Exception Handling with Converters

Exception responses in REST APIs must balance two competing needs: developers need detailed stack traces and error context for diagnostics, while external clients should see minimal details that don't expose implementation internals. A single formatter configuration can't satisfy both; you need per-audience sensitivity controls. The `SensitivityDetails` flag and custom exception converter registration enable environment-aware configuration where development APIs include stack traces and data while production APIs omit them, all from the same code base. This example demonstrates adding exception converters with sensitivity configuration:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Diagnostics;

namespace MyApplication
{
    public class ExceptionHandlingProgram
    {
        public static void Main()
        {
            var options = new NewtonsoftJsonFormatterOptions();

            // Configure for detailed exception information
            options.SensitivityDetails = FaultSensitivityDetails.StackTrace 
                | FaultSensitivityDetails.Data;

            // Add custom converters
            options.Settings.Converters.AddExceptionConverter(
                includeStackTrace: true,
                includeData: true
            );

            options.Settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(
                setup => setup.SensitivityDetails = options.SensitivityDetails
            );

            var formatter = new NewtonsoftJsonFormatter(options);
            Console.WriteLine("Exception handling configured");
        }
    }
}
```

### Synchronizing with JsonConvert

Applications often use static `JsonConvert.SerializeObject` and `JsonConvert.DeserializeObject` calls throughout the codebase for quick JSON operations, but these calls don't automatically use the formatter's configuration, leading to inconsistent behavior between formatted responses and ad-hoc serialization. The `SynchronizeWithJsonConvert` flag addresses this by registering the formatter's settings as `JsonConvert.DefaultSettings`, ensuring all JSON operations in your application—whether via formatters or static calls—use the same configuration. This example demonstrates synchronizing with global JsonConvert settings:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

namespace MyApplication
{
    public class JsonConvertSyncProgram
    {
        public static void Main()
        {
            var options = new NewtonsoftJsonFormatterOptions();

            options.Settings.Formatting = Formatting.Indented;
            options.Settings.NullValueHandling = NullValueHandling.Ignore;

            // Enable synchronization with JsonConvert.DefaultSettings
            options.SynchronizeWithJsonConvert = true;

            var formatter = new NewtonsoftJsonFormatter(options);

            // Now JsonConvert.SerializeObject will use the configured settings
            var testObject = new { test = "value", nullable = (string)null };
            var json = JsonConvert.SerializeObject(testObject);
            Console.WriteLine(json);
        }
    }
}
```

The `NewtonsoftJsonFormatterOptions` class implements `IExceptionDescriptorOptions`, `IContentNegotiation`, and `IValidatableParameterObject`, providing comprehensive validation and configuration capabilities. The default configuration includes reasonable defaults for JSON formatting, null value handling, date parsing, and a set of commonly-used converters.
