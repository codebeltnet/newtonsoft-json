---
uid: Codebelt.Extensions.Newtonsoft.Json.JData
example:
- *content
---

Processing large JSON documents by fully deserializing them into memory requires buffering entire object graphs, consuming memory proportional to document size and preventing streaming architectures. Applications need a way to extract specific values or iterate over nested structures without materializing unused portions of the document. The `JData` class provides factory methods for RFC 7159-compliant streaming JSON parsing that yield values, paths, and types without requiring full deserialization, enabling efficient processing of large files and enabling downstream filtering or transformation pipelines. This example demonstrates how to parse a JSON string and extract structured results:

```csharp
using Codebelt.Extensions.Newtonsoft.Json;
using System;
using System.Linq;

namespace MyApplication
{
    public class JDataStringProgram
    {
        public static void Main()
        {
            var json = @"{
                ""name"": ""Alice"",
                ""age"": 30,
                ""email"": ""alice@example.com""
            }";

            var results = JData.ReadAll(json).ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"Path: {result.Path}");
                Console.WriteLine($"Property: {result.PropertyName}");
                Console.WriteLine($"Value: {result.Value}");
                Console.WriteLine($"Type: {result.Type?.Name}");
                Console.WriteLine("---");
            }
        }
    }
}
```

### Reading from a Stream

Serialized JSON often lives in files, network streams, or message queue payloads where reading it as a complete string in memory isn't practical. The `JData.ReadAll(Stream)` overload accepts a stream and optional configuration (character encoding, whether to leave the stream open), making it easy to parse JSON directly from `FileStream`, `NetworkStream`, or formatter output without buffering to string first. This approach scales to arbitrarily large documents while maintaining the same streaming parsing semantics as string-based calls. This example demonstrates how to read JSON from a stream with configuration:

```csharp
using Codebelt.Extensions.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using System;
using System.IO;
using System.Linq;

namespace MyApplication
{
    public class JDataStreamProgram
    {
        public static void Main()
        {
            var data = new
            {
                users = new[]
                {
                    new { id = 1, name = "Alice" },
                    new { id = 2, name = "Bob" }
                }
            };

            var formatter = new NewtonsoftJsonFormatter();
            var jsonStream = formatter.Serialize(data, data.GetType());

            // Read all values from stream with UTF-8 encoding
            var results = JData.ReadAll(jsonStream, options =>
            {
                options.LeaveOpen = true;
            }).ToList();

            Console.WriteLine($"Total tokens parsed: {results.Count}");

            var userNames = results
                .Where(r => r.PropertyName == "name")
                .Select(r => r.Value)
                .ToList();

            foreach (var name in userNames)
            {
                Console.WriteLine($"User: {name}");
            }
        }
    }
}
```

### Reading from a JsonReader

Newtonsoft.Json's `JsonReader` API gives fine-grained control over tokenization and supports custom token handling, custom error policies, and integration with Newtonsoft.Json's extension ecosystem. Some applications already use `JsonTextReader` directly or have custom reader implementations that need to be plugged into the streaming extraction pipeline. The `JData.ReadAll(JsonReader)` overload accepts any `JsonReader` instance, enabling composition with existing Newtonsoft.Json code and supporting advanced scenarios like nested readers or custom token processing. This example demonstrates direct usage with a `JsonReader`:

```csharp
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace MyApplication
{
    public class JDataReaderProgram
    {
        public static void Main()
        {
            var json = @"[1, 2, 3, 4, 5]";

            using (var sr = new StringReader(json))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    var results = JData.ReadAll(reader).ToList();

                    Console.WriteLine($"Array contains {results.Count} elements");
                    foreach (var result in results)
                    {
                        Console.WriteLine($"Value: {result.Value}, Type: {result.Type?.Name}");
                    }
                }
            }
        }
    }
}
```

The `JData` factory method returns an enumerable of `JDataResult` objects that provide hierarchical access to JSON structure through the `Path`, `Children`, and `Parent` properties. This approach enables efficient streaming parsing of large JSON documents without requiring full deserialization.
