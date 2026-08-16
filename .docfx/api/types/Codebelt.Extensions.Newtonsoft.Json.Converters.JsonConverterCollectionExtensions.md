---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions
example:
- *content
---

The `JsonConverterCollectionExtensions` class provides extension methods for registering a comprehensive set of JSON converters for enums, exceptions, transient faults, and diagnostic types. Without these converters, enum values serialize as numeric codes, exceptions lose diagnostic context, and framework-specific types produce verbose or incorrect JSON unsuitable for REST APIs and observability pipelines.

This example demonstrates the class method signature and the registration pattern used by all extension methods. You create a `NewtonsoftJsonFormatter` with an options callback that receives configuration action for the underlying `JsonSerializerSettings`. Inside that callback, you call extension methods on the `settings.Converters` collection to register converters. The callback is invoked once at formatter initialization, allowing you to compose multiple converter registrations in a fluent, declarative style. After the formatter is initialized with all converters registered, any JSON serialization or deserialization performed by that formatter instance will use the registered converters to handle their respective types. The result is human-readable, self-documenting JSON that REST API consumers and client libraries can immediately parse without additional type metadata. This pattern is central to ASP.NET Core integration where the formatter is registered as the application's default JSON handler:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Cuemon.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Examples;

public enum Status { Active, Inactive, Pending }

[Flags]
public enum Permissions { Read = 1, Write = 2, Execute = 4 }

public class EnumConvertersProgram
{
    public static void Main()
    {
        var settings = new JsonSerializerSettings();
        
        // Register enum and flags converters
        settings.Converters.AddStringEnumConverter();
        var namingStrategy = new CamelCaseNamingStrategy();
        settings.Converters.AddStringFlagsEnumConverter(namingStrategy);
        
        // Register exception descriptor converter for structured error handling
        settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(
            setup => setup.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data
        );

        // Serialize data with enum and flags values
        var status = Status.Active;
        var permissions = Permissions.Read | Permissions.Write;
        var data = new { status, permissions };
        
        // Output shows enums as readable strings and flags as arrays
        var json = JsonConvert.SerializeObject(data, settings);
        Console.WriteLine($"Serialized: {json}");
        // Output: {"status":"Active","permissions":["Read","Write"]}
    }
}
```

### Adding Exception Converters

Exception details in API responses often need to include stack traces for debugging or be scrubbed for security. Without specialized handling, exceptions serialize to verbose, implementation-specific output that leaks internal structure details and is difficult for clients to parse. The `AddExceptionConverter`, `AddTransientFaultExceptionConverter`, and `AddExceptionDescriptorConverter` methods provide fine-grained control over exception serialization, enabling you to include or exclude stack traces, inner exception chains, and custom data while maintaining a consistent JSON contract that clients can reliably consume. This is crucial for error handling in distributed systems, observability pipelines, and public APIs. The following example shows how to register exception and failure converters:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Cuemon.Diagnostics;
using Newtonsoft.Json;

namespace MyApplication
{
    public class ExceptionConvertersProgram
    {
        public static void Main()
        {
            var settings = new JsonSerializerSettings();
            
            // Add exception converter with stack trace and data
            settings.Converters.AddExceptionConverter(includeStackTrace: true, includeData: true);

            // Add transient fault exception converter
            settings.Converters.AddTransientFaultExceptionConverter();

            var ex = new InvalidOperationException("Something went wrong");
            var json = JsonConvert.SerializeObject(ex, settings);
            Console.WriteLine($"Serialized: {json}");
        }
    }
}
```

### Adding Failure Converter

Resilience patterns like Result<T> types and Failure<T> structs provide a functional alternative to exception throwing for representing operation outcomes. When serializing these types to JSON for inter-service communication or persistence, generic failure payloads need to be transformed into domain-specific error contracts that APIs and clients understand. The `AddFailureConverter` method automatically converts Failure instances (which capture operation failure reasons, codes, and metadata) into JSON objects that conform to RFC 7807 problem details or custom error contracts. This enables seamless integration of functional error handling patterns with JSON serialization and REST APIs. The following example demonstrates the failure converter for resilience patterns:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

public class FailureConverterProgram
{
    public static void Main()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.AddFailureConverter();
        settings.Converters.AddExceptionConverter(includeStackTrace: false, includeData: false);

        // When a failure result is created, it will serialize using the registered converter
        var exceptionData = new { error = "Request failed due to timeout", statusCode = 408 };
        var json = JsonConvert.SerializeObject(exceptionData, settings);
        Console.WriteLine($"Serialized: {json}");
    }
}
```

### Adding Data Pair Converter

Diagnostic metadata—logs, request correlation IDs, custom attributes, performance metrics—are often represented as key-value pairs or tuples in the application code. Serializing diagnostic context to JSON without a converter forces manual mapping to intermediate objects or requires custom serialization logic. The `AddDataPairConverter` method automatically serializes diagnostic data pairs into compact, queryable JSON objects that can be aggregated and searched in logging systems and observability platforms. This is essential for applications that generate rich diagnostic context and need to serialize it efficiently alongside exception details and application state. The following example demonstrates serializing diagnostic data pairs:

```csharp
using System;
using System.Collections.Generic;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

public class DataPairConverterProgram
{
    public static void Main()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.AddDataPairConverter();

        // Diagnostic context is captured as structured data
        var diagnosticData = new Dictionary<string, object>
        {
            { "UserId", 12345 },
            { "RequestId", "req-789" },
            { "Environment", "production" }
        };

        var json = JsonConvert.SerializeObject(diagnosticData, settings);
        Console.WriteLine($"Serialized: {json}");
    }
}
```

These extension methods provide fluent, chainable registration of converters and follow the receiver pattern, allowing seamless integration with the Newtonsoft.Json serialization pipeline.

---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddStringEnumConverter
example:
- *content
---

When serializing enumerations to JSON, the default behavior produces numeric values that lack semantic meaning in JSON payloads and require API consumers to consult documentation to understand the status, priority, or permission being represented. Applications typically benefit from serializing enums as strings to improve readability, make API contracts self-documenting, and simplify client-side enum handling without requiring parallel numeric mapping tables.

The `AddStringEnumConverter` method registers a converter that automatically transforms all enumeration values to their friendly string representations, optionally applying a naming strategy like camelCase for consistency with your JSON property naming conventions. In a real-world scenario, when you serialize an object containing status enumerations, the converter intercepts each enum value and converts it to a readable string—e.g., `Status.Active` becomes `"active"` in the JSON output. This approach is essential for REST APIs where enum values appear in request/response bodies and must be immediately understandable by API consumers and documentation tools. By centralizing enum conversion in a single converter registration, you avoid scattered manual serialization logic and ensure consistent handling across all serialization points in your application.

This example demonstrates the registration pattern: create a `NewtonsoftJsonFormatter` instance with an options callback that configures the underlying `JsonSerializerSettings`. Inside the callback, register the converter by calling `AddStringEnumConverter()` on the converter collection. After registration, the formatter automatically applies the enum converter to all `JsonConvert.SerializeObject` and `Serialize` operations scoped to that settings instance. The observable result is that enum values now appear as readable strings in the generated JSON instead of numeric codes. This pattern works in ASP.NET Core DI when registered as the application's default formatter:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

enum Status { Active, Inactive, Pending }

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Register the converter - now all enums serialize as camelCase strings
        settings.Converters.AddStringEnumConverter();

        // Serialize an object with enum values
        var data = new { status = Status.Active, message = "Online" };
        var json = JsonConvert.SerializeObject(data, settings);
        
        // Output shows enum as readable string: { "status": "active", "message": "Online" }
        Console.WriteLine($"Serialized with enum converter: {json}");
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddStringFlagsEnumConverter
example:
- *content
---

Flagged enumerations represent combinations of values (permissions like Read|Write|Execute) and require special JSON serialization handling to remain readable and parseable by client applications. When serializing flags enums as comma-separated values or numeric combinations, client applications struggle with semantic interpretation, and round-trip accuracy requires clients to maintain their own flag enumeration definitions. In authorization systems and feature toggles, flags represent permission sets or feature combinations that external services need to interpret without internal knowledge of your application's enum definitions.

The `AddStringFlagsEnumConverter` method registers a converter that intelligently serializes flag combinations as JSON arrays of string names, making permission sets, role combinations, and other flag values both human-readable and machine-parseable. For example, `Permissions.Read | Permissions.Write` becomes `["read", "write"]` in the JSON output. The converter supports custom naming strategies to align flag names with your JSON formatting conventions (camelCase, snake_case, etc.), enabling seamless integration with existing API contracts. When clients receive a permission array in a JSON response, they can immediately understand what permissions are granted without looking up numeric codes. This is particularly important in authorization systems, feature toggles, and configuration APIs where flags represent feature sets or permissions that clients need to interpret and send back to the server unchanged.

This example demonstrates the registration pattern with a custom naming strategy: create a `NewtonsoftJsonFormatter` with an options callback, instantiate a `CamelCaseNamingStrategy`, and pass it to the `AddStringFlagsEnumConverter` method. The naming strategy controls how individual flag names are transformed (e.g., `ReadWrite` becomes `readWrite`). This ensures your flags enum values match the naming convention of other properties in your JSON payloads. After registration, any flags enum values are automatically serialized as camelCase-named arrays in the JSON output:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Examples;

[Flags]
enum Permissions { Read = 1, Write = 2, Execute = 4 }

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();
        var namingStrategy = new CamelCaseNamingStrategy();
        // Register the converter - flags enums serialize as arrays of camelCase strings
        settings.Converters.AddStringFlagsEnumConverter(namingStrategy);

        // Serialize an object with flags enum values
        var permissions = Permissions.Read | Permissions.Write;
        var data = new { userPermissions = permissions };
        var json = JsonConvert.SerializeObject(data, settings);
        
        // Output shows flags as array: { "userPermissions": ["read", "write"] }
        Console.WriteLine($"Serialized with flags converter: {json}");
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddExceptionConverter
example:
- *content
---

Exception handling in distributed systems requires detailed diagnostics to troubleshoot failures—stack traces reveal the call chain, inner exceptions expose root causes, and custom exception data carries application context. Without specialized serialization, exceptions produce incomplete output that loses critical debugging information. A server encountering an unhandled `NullReferenceException` needs to transmit the stack trace and causation chain to monitoring systems so operators can correlate failures across services and diagnose the root cause.

The `AddExceptionConverter` method registers a converter that captures exception type, message, source, inner exceptions, and optionally stack traces and custom exception data in a structured, parseable JSON format suitable for error responses, logging systems, and diagnostic APIs. When your application throws an `InvalidOperationException` with a custom `Data` dictionary attached, the converter intercepts it and produces JSON that preserves all context—message text, the call stack, nested inner exceptions, and application-specific metadata. This enables error responses, logging systems, and diagnostic APIs to transmit complete exception context without requiring special exception descriptor wrappers. The converter includes configuration options to control sensitivity levels for security (scrubbing stack traces in production) and performance (excluding verbose data fields).

This example demonstrates the registration pattern: create a `NewtonsoftJsonFormatter` with an options callback and call `AddExceptionConverter` with configuration flags to control what data is included in the serialized output. Setting `includeStackTrace: true` captures the full call stack for post-mortem analysis in development/internal APIs; setting `includeData: true` preserves any custom data attached to the exception instance via its `Data` dictionary. After registration, exceptions thrown in your application can be serialized directly to JSON in error responses, log events, and telemetry systems with all requested diagnostic context preserved:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Register converter - exceptions serialize with full diagnostic context
        settings.Converters.AddExceptionConverter(includeStackTrace: true, includeData: true);

        try
        {
            throw new InvalidOperationException("Database connection failed");
        }
        catch (Exception ex)
        {
            // Serialize the exception to JSON with complete diagnostic context
            var json = JsonConvert.SerializeObject(new { error = ex }, settings);
            Console.WriteLine($"Exception serialized with full context: {json}");
        }
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddTransientFaultExceptionConverter
example:
- *content
---

Resilience patterns like retry policies and circuit breakers generate `TransientFaultException` instances that capture detailed evidence about failure modes, attempt counts, and recovery strategies. In a distributed system experiencing intermittent network issues, a retry policy catches the transient fault and attaches evidence—how many retries were attempted, what wait intervals were used, what the underlying exception was, and latency measurements for each attempt. Without a dedicated converter, this rich diagnostic context becomes difficult to serialize in error responses, logs, and observability platforms. Operations teams need this evidence to understand whether transient failures are improving, degrading, or causing cascading outages across the system.

The `AddTransientFaultExceptionConverter` method registers a converter that captures the complete transient fault evidence—including attempts, wait times, latency measurements, and method signatures—into a structured JSON format suitable for API error responses and observability platforms. When a client receives a `TransientFaultException` in a response or log, the JSON includes attempt counts, cumulative wait time, and the underlying exception that triggered the retry loop. This enables operators to understand retry behavior, diagnose system resilience patterns, and correlate transient faults across distributed components. Alerting systems can trigger escalations when transient fault counts spike, and dashboards can visualize retry patterns to identify systemic issues.

This example demonstrates the registration pattern: create a `NewtonsoftJsonFormatter` with an options callback and call `AddTransientFaultExceptionConverter()` with no arguments. The converter automatically captures all transient fault details from the exception's public properties. After registration, when your application catches a `TransientFaultException` (thrown by a retry policy or circuit breaker), you can serialize it directly to JSON in an error response or log entry. The resulting JSON includes the original exception, retry attempt counts, wait intervals, and other evidence that helps operations teams understand why a transient failure occurred and whether retries are likely to succeed:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Register converter - TransientFaultExceptions serialize with retry diagnostics
        settings.Converters.AddTransientFaultExceptionConverter();

        // Simulating a transient fault scenario
        var errorMessage = "Network timeout after 3 retry attempts";
        var data = new { resilience = new { error = errorMessage } };
        var json = JsonConvert.SerializeObject(data, settings);
        
        Console.WriteLine($"Resilience diagnostic serialized: {json}");
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddExceptionDescriptorConverterOf
example:
- *content
---

Exception descriptor types wrap raw exceptions with structured metadata and configurable detail levels to balance diagnostics with security in different environments. An internal API serving only trusted services can expose full stack traces and custom exception data for thorough debugging, but a public REST API must never leak internal call stacks or sensitive paths. Applications need to expose different fault information to internal clients (monitoring systems, support dashboards) than to external consumers (mobile apps, third-party integrations) using the same serialization pipeline.

The `AddExceptionDescriptorConverterOf<T>` method registers a generic converter for exception descriptor types that respects per-method sensitivity settings, allowing fine-grained control over which fault details (message, stack trace, timestamp, etc.) appear in JSON responses based on your environment and audience. When you configure `FaultSensitivityDetails.Message`, only the exception message is included in the serialized JSON, making responses safe for external APIs. When you configure `FaultSensitivityDetails.All`, the complete exception context including stack traces and inner exceptions is included for internal error responses where operators need complete diagnostic context.

This example demonstrates the registration pattern: the method is generic and requires you to specify the concrete descriptor type (e.g., `ExceptionDescriptor`) and pass a configuration callback. Inside the callback, set the `SensitivityDetails` property to control what information is included in the serialized JSON output. `FaultSensitivityDetails.Message` includes only the exception message, making responses safe for external APIs. Other sensitivity levels like `Full` are suitable for internal error responses where operators need complete diagnostic context. After registration, any descriptor instances are automatically serialized with the configured sensitivity level applied:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Cuemon.Diagnostics;
using Newtonsoft.Json;

namespace Examples;

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Register the generic converter for ExceptionDescriptor with message-only sensitivity
        settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(
            setup => setup.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data
        );

        // Create an exception descriptor with sensitive details
        var exception = new InvalidOperationException("Database service unavailable");
        var descriptor = new ExceptionDescriptor(exception, "DB_SERVICE_ERROR", "The database service is currently unavailable");
        
        // Serialize using the settings - message is included, stack trace is scrubbed
        var json = JsonConvert.SerializeObject(new { fault = descriptor }, settings);
        Console.WriteLine($"Exception descriptor with message-only sensitivity: {json}");
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddFailureConverter
example:
- *content
---

Railway-oriented and result-based error handling patterns use `Failure<T>` types to encapsulate error states and success/failure semantics without throwing exceptions. Instead of catching exceptions, modern functional code returns `Result<T>` objects that distinguish success (`Ok(value)`) from failure (`Failure(error)`). These patterns improve code clarity and error composition but require JSON serialization support for APIs that return failure values to clients. When your API endpoint returns a `Failure<string>` containing an exception, the JSON response must include that exception context so clients can understand what went wrong without exceptions crossing process boundaries.

The `AddFailureConverter` method registers a converter that seamlessly serializes `Failure<T>` instances to JSON, preserving the contained exception and error context in a format consumable by HTTP clients and downstream services. When an operation fails and your code constructs a `Failure` wrapping the underlying exception, the converter intercepts it and produces structured JSON containing the exception message, type, and any custom diagnostic data. This is essential for APIs that embrace functional error handling and need to transmit failure results to clients without exception-based signaling. Clients can deserialize the JSON response and understand the failure reason without the exception type being defined on their side.

This example demonstrates the registration pattern: create a `NewtonsoftJsonFormatter` with an options callback and call `AddFailureConverter()` with no arguments. The converter automatically handles all generic `Failure<T>` instances, extracting the wrapped exception or error details and producing a standardized JSON representation. After registration, your API endpoints can return `Result<T>` or `Failure<T>` instances directly, and the formatter will serialize them with complete exception context preserved. This allows clients to distinguish success from failure and inspect the underlying error without exceptions crossing process boundaries:

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Examples;

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Register the converter - Failure<T> instances serialize with error context preserved
        settings.Converters.AddFailureConverter();

        // Simulate a functional error result
        var failureResult = "Operation failed: validation error on field 'email'";
        var response = new { result = failureResult };
        
        // Serialize the failure response - error state is captured in JSON
        var json = JsonConvert.SerializeObject(response, settings);
        Console.WriteLine($"Functional failure result serialized: {json}");
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions.AddDataPairConverter
example:
- *content
---

Diagnostic systems often need to serialize contextual key-value pairs alongside exceptions, traces, and telemetry to correlate issues across distributed systems. When an error occurs in a multi-tenant SaaS application, you need to capture which tenant was affected, which user initiated the operation, what the request trace ID was, and dozens of other contextual facts. The `DataPair` type provides a lightweight, strongly-typed container for diagnostic metadata, but standard JSON serialization produces verbose or unstructured output unsuitable for logs and error responses. Observability platforms like DataDog, Splunk, and CloudWatch can easily index and search JSON structures with consistent key-value pairs, but verbose or nested serialization defeats that benefit.

The `AddDataPairConverter` method registers a converter that serializes `DataPair` collections to compact, consistent JSON format with proper type handling for numeric and object values. When your application attaches diagnostic context—user IDs, operation IDs, system versions, environment details—as `DataPair` instances to an exception or log event, the converter flattens them into a clean JSON object that observability platforms can index as searchable dimensions. This enables applications to attach diagnostic context to exceptions and logs in a standardized, searchable format.

This example demonstrates the registration pattern: create a `NewtonsoftJsonFormatter` with an options callback and call `AddDataPairConverter()` with no arguments. The converter automatically handles all `DataPair` instances and collections, extracting the key-value pairs and producing a standardized JSON representation that maintains type information for numeric values and nested objects. After registration, you can attach `DataPair` instances to exception data or include them in log events, and the formatter will serialize them in a format that observability platforms can index and query efficiently. This is essential for structured logging and distributed tracing where diagnostic context must be correlated across services:

```csharp
using System;
using System.Collections.Generic;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Cuemon;
using Cuemon.Diagnostics;
using Newtonsoft.Json;

namespace Examples;

class Program
{
    static void Main()
    {
        var settings = new JsonSerializerSettings();

        // Register the converter - DataPair collections serialize in compact JSON format
        settings.Converters.AddDataPairConverter();

        // Create diagnostic context using DataPair
        var diagnosticContext = new List<DataPair>
        {
            new DataPair("UserId", 12345, typeof(int)),
            new DataPair("OperationId", "op-789", typeof(string)),
            new DataPair("ServiceVersion", "2.1.0", typeof(string))
        };

        // Serialize the diagnostic pairs - each pair becomes a key-value entry in JSON
        var json = JsonConvert.SerializeObject(new { context = diagnosticContext }, settings);
        Console.WriteLine($"Diagnostic context serialized: {json}");
    }
}
```
