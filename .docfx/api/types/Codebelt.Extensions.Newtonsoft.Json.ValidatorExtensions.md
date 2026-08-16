---
uid: Codebelt.Extensions.Newtonsoft.Json.ValidatorExtensions
example:
- *content
---
The following example validates JSON document format before parsing.

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Cuemon;

namespace Examples;

class ValidatorExtensionsExample
{
    static void Main()
    {
        var validJson = """{ "id": "123", "name": "Test" }""";
        var invalidJson = """{ "id" "123" }""";

        try
        {
            Validator.ThrowIf.InvalidJsonDocument(validJson);
            Console.WriteLine("Valid JSON passed validation");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation failed: {ex.Message}");
        }

        try
        {
            Validator.ThrowIf.InvalidJsonDocument(invalidJson);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid JSON caught: {ex.Message}");
        }
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.ValidatorExtensions.InvalidJsonDocument
example:
- *content
---

JSON validation is a critical first step in any data pipeline that consumes untrusted JSON, preventing downstream parsing errors, deserialization exceptions, and cryptic error messages that frustrate developers and obscure the true source of invalid data. The `InvalidJsonDocument` validator method checks that a JSON string complies with RFC 8259 specification, throwing `ArgumentException` with a descriptive message if the document is malformed, enabling early failure and clear diagnostics. Applications can integrate this validator into request pipelines, configuration loaders, and data transformation steps to guarantee valid JSON before proceeding with parsing or deserialization. This example demonstrates validating a JSON string before further processing:

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Cuemon;

var json = """{ "status": "ok", "code": 200 }""";
Validator.ThrowIf.InvalidJsonDocument(json, nameof(json));
Console.WriteLine("JSON is valid");
```
