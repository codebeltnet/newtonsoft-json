---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonWriterExtensions
example:
- *content
---
The following example demonstrates writing JSON objects and property names using extension methods.

```csharp
using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;

namespace Examples;

class JsonWriterExtensionsExample
{
    static void Main()
    {
        using var sw = new StringWriter();
        using var jw = new JsonTextWriter(sw);
        var serializer = JsonSerializer.Create();

        jw.WriteStartObject();
        jw.WritePropertyName("user");
        jw.WriteStartObject();
        jw.WritePropertyName("name");
        jw.WriteValue("Alice");
        jw.WriteEndObject();
        
        jw.WritePropertyName("metadata");
        var metadataObject = new { version = "1.0", timestamp = DateTime.UtcNow };
        jw.WriteObject(metadataObject, serializer);
        
        jw.WriteEndObject();

        Console.WriteLine(sw.ToString());
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonWriterExtensions.WriteObject
example:
- *content
---

Building JSON documents programmatically with `JsonWriter` often requires creating nested objects and writing properties with values that are themselves objects or complex types. The base `JsonWriter` API requires manual calls to `WriteStartObject`, `WritePropertyName`, and `WriteEndObject` for each nested level, leading to verbose, error-prone code with a high risk of mismatched braces. The `WriteObject` extension method provides a convenient shorthand that accepts an anonymous object, ExpandoObject, or other object, automatically serializes it, and writes it to the current JSON writer in a single statement. This dramatically simplifies nested object creation and reduces bracket-matching errors in large JSON construction workflows. This example demonstrates writing a nested object as a single property value:

```csharp
using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;

namespace Examples;

class WriteObjectExample
{
    static void Main()
    {
        using var sw = new StringWriter();
        using var jw = new JsonTextWriter(sw);

        // Create a serializer for the WriteObject call
        var serializer = JsonSerializer.Create();

        jw.WriteStartObject();
        jw.WritePropertyName("status");
        jw.WriteValue("active");
        jw.WritePropertyName("metadata");
        
        // Call WriteObject extension to write a nested object
        var metadataObject = new { version = "1.0", timestamp = DateTime.UtcNow };
        jw.WriteObject(metadataObject, serializer);
        
        jw.WriteEndObject();

        Console.WriteLine($"Written nested object: {sw.ToString()}");
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonWriterExtensions.WritePropertyName
example:
- *content
---

Writing JSON property names with `JsonWriter.WritePropertyName` requires manual escaping and validation to handle special characters, quotes, and non-ASCII characters according to JSON specification. Applications building dynamic JSON documents with property names derived from user input, database columns, or external sources need a reliable way to write safely-escaped property names without worrying about RFC 7159 compliance. The `WritePropertyName` extension method wraps the base writer method with validation and escaping, ensuring property names are always written in valid JSON format regardless of their source. This is particularly important for applications that generate JSON with programmatically-determined property names. This example demonstrates writing property names with special characters:

```csharp
using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;

namespace Examples;

class WritePropertyNameExample
{
    static void Main()
    {
        using var sw = new StringWriter();
        using var jw = new JsonTextWriter(sw);

        jw.WriteStartObject();
        jw.WritePropertyName("special:field");
        jw.WriteValue("value");
        jw.WriteEndObject();

        Console.WriteLine($"Property written: {sw.ToString()}");
    }
}
```
