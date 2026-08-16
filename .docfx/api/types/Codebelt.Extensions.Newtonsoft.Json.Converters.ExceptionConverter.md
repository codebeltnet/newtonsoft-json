---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.ExceptionConverter
example:
- *content
---

Exception details in API responses often need to include stack traces for debugging or be scrubbed for security, while exception serialization must capture inner exceptions and custom data for complete diagnostics. Without specialized handling, exceptions serialize to verbose, implementation-specific output unsuitable for external clients or comprehensive logging. The `ExceptionConverter` class solves this by providing configurable serialization of exception graphs—including nested inner exceptions, stack traces, and data dictionaries—enabling applications to control which exception details appear in different contexts (internal diagnostics versus client responses). This example demonstrates how to use the `ExceptionConverter` to serialize and deserialize exceptions with configurable detail levels:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

class Program
{
    static void Main()
    {
        // Create settings with exception converter
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new ExceptionConverter(includeStackTrace: true, includeData: true));

        try
        {
            // Simulate an exception with nested inner exception
            throw new InvalidOperationException("Outer exception occurred", 
                new ArgumentException("Inner exception message"));
        }
        catch (Exception ex)
        {
            // Serialize the exception to JSON
            var json = JsonConvert.SerializeObject(ex, settings);
            Console.WriteLine("Serialized Exception:");
            Console.WriteLine(json);
        }
    }
}
```

The converter produces JSON output that captures the exception type, message, and optionally the stack trace and data dictionary.
