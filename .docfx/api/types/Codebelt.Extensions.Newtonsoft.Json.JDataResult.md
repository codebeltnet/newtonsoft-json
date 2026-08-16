---
uid: Codebelt.Extensions.Newtonsoft.Json.JDataResult
example:
- *content
---

The `JDataResult` class represents a single parsed result from a JSON reading operation, providing access to the token's path, value, type, and hierarchical structure. The following example demonstrates how to work with `JDataResult` objects:

```csharp
using Codebelt.Extensions.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApplication
{
    public class JDataResultProgram
    {
        public static void Main()
        {
            var json = @"{
                ""user"": {
                    ""id"": 1,
                    ""profile"": {
                        ""firstName"": ""John"",
                        ""lastName"": ""Doe"",
                        ""tags"": [""admin"", ""user"", ""developer""]
                    }
                }
            }";

            var results = JData.ReadAll(json).ToList();

            // Find results with specific properties
            var firstNameResult = results.FirstOrDefault(r => r.PropertyName == "firstName");
            if (firstNameResult != null)
            {
                Console.WriteLine($"Property: {firstNameResult.PropertyName}");
                Console.WriteLine($"Value: {firstNameResult.Value}");
                Console.WriteLine($"Path: {firstNameResult.Path}");
                Console.WriteLine($"Type: {firstNameResult.Type?.Name}");
            }

            // Find results with children (complex objects/arrays)
            var complexResults = results.Where(r => r.Children.Count > 0).ToList();
            Console.WriteLine($"Complex structures found: {complexResults.Count}");

            // Navigate hierarchy
            var profileResults = results.Where(r => r.PropertyName == "profile").ToList();
            foreach (var profileResult in profileResults)
            {
                Console.WriteLine($"Profile has {profileResult.Children.Count} children");
                foreach (var child in profileResult.Children)
                {
                    Console.WriteLine($"  - {child.PropertyName}: {child.Value}");
                }
            }

            // Find results with parent references
            var rootResults = results.Where(r => r.Parent == null).ToList();
            Console.WriteLine($"Root level results: {rootResults.Count}");

            // Traverse and print structure
            PrintHierarchy(results.Where(r => r.Parent == null).ToList(), 0);
        }

        private static void PrintHierarchy(List<JDataResult> results, int indent)
        {
            foreach (var result in results)
            {
                var indentation = new string(' ', indent * 2);
                var displayValue = result.Value != null ? result.Value.ToString() : "[object]";

                if (result.PropertyName != null)
                {
                    Console.WriteLine($"{indentation}{result.PropertyName}: {displayValue}");
                }
                else
                {
                    Console.WriteLine($"{indentation}{displayValue}");
                }

                if (result.Children.Count > 0)
                {
                    PrintHierarchy(result.Children.ToList(), indent + 1);
                }
            }
        }
    }
}
```

The `JDataResult` class provides the following properties:

- **Path**: The JSON path to the token (e.g., "user.profile.firstName")
- **PropertyName**: The name of the property if this result represents a property
- **Value**: The parsed value of the token
- **Type**: The CLR type of the value
- **Children**: A collection of child results for complex types (objects/arrays)
- **Parent**: A reference to the parent result in the hierarchy

The `ToString()` method provides a formatted representation showing the path and child count, making it useful for diagnostics and logging.
