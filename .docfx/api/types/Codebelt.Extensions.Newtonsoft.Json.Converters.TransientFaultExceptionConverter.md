---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.TransientFaultExceptionConverter
example:
- *content
---

Resilience patterns like retry logic capture valuable evidence—attempt counts, wait intervals, method signatures, latency measurements—when transient faults occur, but standard exception serialization loses this context entirely. Losing this evidence makes post-incident diagnosis difficult and hides important patterns about which operations are retryable and how long customers should wait. The `TransientFaultExceptionConverter` preserves the complete `TransientFaultEvidence` structure during JSON serialization, capturing attempts, recovery wait times, method descriptors, and inner exceptions in a structured format suitable for logging systems, APM platforms, and diagnostic dashboards. This example demonstrates how to use the `TransientFaultExceptionConverter` with retry exceptions:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

public class TransientFaultProgram
{
    public static void Main()
    {
        // Create settings with TransientFaultExceptionConverter
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new TransientFaultExceptionConverter());
        settings.Converters.Add(new ExceptionConverter(includeStackTrace: true, includeData: false));

        try
        {
            // Simulate a transient fault scenario with an inner exception
            var innerException = new TimeoutException("Database connection timeout");

            // Create an exception that wraps the transient fault context
            var fault = new Exception("Failed to fetch user data after 3 attempts", innerException);

            // Serialize the exception - the converter will handle the serialization
            var json = JsonConvert.SerializeObject(new { error = fault }, settings);
            Console.WriteLine("Serialized fault with resilience context:");
            Console.WriteLine(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

The converter preserves the complete evidence structure in JSON format, including:

```json
{
  "message": "Failed to fetch user data after 3 attempts",
  "evidence": {
    "attempts": 3,
    "recoveryWaitTime": "00:00:01",
    "totalRecoveryWaitTime": "00:00:03",
    "latency": "00:00:00.1500000",
    "descriptor": {
      "caller": "MyApplication.DataService",
      "methodName": "FetchUserData",
      "parameters": ["userId (Int32)"],
      "arguments": [12345]
    }
  },
  "inner": {
    "Type": "System.TimeoutException",
    "Message": "Database connection timeout"
  }
}
```

This converter is particularly useful for diagnostics and logging of resilience patterns, capturing the context and progression of transient fault handling.
