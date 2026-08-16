---
uid: Codebelt.Extensions.Newtonsoft.Json.JDataResultExtensions
example:
- *content
---
The following example shows how to parse JSON and flatten its hierarchical structure. When working with complex nested JSON documents—API responses, configuration files, or database exports—developers need to search for specific properties, validate values at any depth, or transform data without writing recursive navigation code. The `JDataResultExtensions` class provides extension methods to flatten nested structures, extract specific properties or arrays, and process results using standard LINQ queries. This approach transforms JSON navigation from imperative recursion into declarative LINQ queries that are easier to read, maintain, and test. This example demonstrates parsing multi-level JSON with users and tags, then using extension methods to flatten, extract objects by property names, and group array structures:

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json;

namespace Examples;

class JDataResultExtensionsExample
{
    static void Main()
    {
        var json = """
            {
              "users": [
                { "id": 1, "name": "Alice", "age": 30 },
                { "id": 2, "name": "Bob", "age": 25 }
              ],
              "tags": ["admin", "user"]
            }
            """;

        var results = JData.ReadAll(json);
        var flat = results.Flatten().ToList();

        // Demonstrate flattening
        foreach (var item in flat)
        {
            if (!string.IsNullOrEmpty(item.PropertyName))
            {
                Console.WriteLine($"{item.Path}: {item.Value}");
            }
        }
        
        // Demonstrate ExtractObjectValues to extract specific properties from objects
        flat.ExtractObjectValues("id,name,age", extracted =>
        {
            var id = extracted["id"].Value;
            var name = extracted["name"].Value;
            var age = extracted["age"].Value;
            Console.WriteLine($"Extracted: id={id}, name={name}, age={age}");
        });
        
        // Demonstrate ExtractArrayValues to extract and process arrays separately
        flat.ExtractArrayValues("users,tags", extracted =>
        {
            var userCount = extracted["users"].Count();
            var tagCount = extracted["tags"].Count();
            Console.WriteLine($"Grouped arrays: found {userCount} users and {tagCount} tags");
        });
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.JDataResultExtensions.Flatten
example:
- *content
---

Nested JSON structures require traversal and filtering to extract specific properties or understand hierarchical organization, but working with raw `JDataResult` collections that preserve parent-child relationships requires manual recursion or LINQ filtering. The `Flatten` extension method transforms hierarchical `JDataResult` sequences into a single flat enumerable while preserving path information, enabling straightforward LINQ-to-Objects queries for finding properties by name, filtering by value type, or extracting deep values without custom traversal logic. This is essential for applications that need to search, transform, or validate arbitrarily nested JSON documents without detailed knowledge of structure. This example demonstrates parsing multi-level JSON with objects and arrays, then flattening the result and using LINQ to find properties and filter values:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json;

namespace Examples;

class FlattenExample
{
    static void Main()
    {
        var json = """
            {
              "level1": {
                "level2": {
                  "value": "deep"
                }
              }
            }
            """;

        var results = JData.ReadAll(json);
        var flatList = results.Flatten().ToList();

        Console.WriteLine($"Flattened {flatList.Count} items from nested structure");
        foreach (var item in flatList.Where(r => !string.IsNullOrEmpty(r.Value?.ToString())))
        {
            Console.WriteLine($"  {item.Path}: {item.Value}");
        }
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.JDataResultExtensions.ExtractObjectValues
example:
- *content
---

Working with JSON arrays of objects often requires extracting specific properties from each object to build dictionaries or maps for processing. Manually iterating arrays, finding matching properties, and organizing them into lookup structures is error-prone and verbose, especially when dealing with variable numbers of objects and deeply nested property structures. The `ExtractObjectValues` extension method simplifies this pattern by accepting a comma-delimited list of property paths and invoking a callback for each object with a dictionary of extracted properties, enabling clean batch processing of JSON arrays without manual iteration or filtering. This is particularly useful for ETL pipelines, data transformation scripts, and APIs that need to extract columns from semi-structured JSON where the source structure may vary. This example demonstrates parsing a JSON array of person objects, flattening to access all properties, then using the extension method to extract name and age fields and transform each person into application objects:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json;

namespace Examples;

class ExtractObjectValuesExample
{
    static void Main()
    {
        var json = """
            [
              { "name": "Alice", "age": 30 },
              { "name": "Bob", "age": 25 }
            ]
            """;

        var results = JData.ReadAll(json);
        var flat = results.Flatten().ToList();

        // Call ExtractObjectValues to group and process properties
        flat.ExtractObjectValues("name,age", extracted =>
        {
            var name = extracted["name"].Value;
            var age = extracted["age"].Value;
            Console.WriteLine($"Extracted person: {name}, age {age}");
        });
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.JDataResultExtensions.ExtractArrayValues
example:
- *content
---

JSON documents often contain multiple arrays at different locations (users, tags, items, etc.) that need to be separately extracted and analyzed, but identifying which results correspond to which arrays and grouping them requires complex filtering logic that obscures the intent. The `ExtractArrayValues` extension method accepts a comma-delimited list of array paths and invokes a callback with grouped results for each array, making it straightforward to process multiple arrays in a single pass without custom filtering and grouping code. This pattern is essential for data validation, aggregation, and transformation scenarios where you need to count array lengths, validate membership, or apply transformations to array elements independently. This example demonstrates parsing mixed JSON with both user and tag arrays, flattening the result, then using the extension method to extract and process users and tags separately, enabling independent aggregation and reporting:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json;

namespace Examples;

class ExtractArrayValuesExample
{
    static void Main()
    {
        var json = """
            {
              "users": [
                { "id": 1, "name": "Alice" },
                { "id": 2, "name": "Bob" }
              ],
              "tags": ["admin", "user"]
            }
            """;

        var results = JData.ReadAll(json);
        var flat = results.Flatten().ToList();

        // Call ExtractArrayValues to group and process array elements
        flat.ExtractArrayValues("users,tags", extracted =>
        {
            var userCount = extracted["users"].Count();
            var tagCount = extracted["tags"].Count();
            Console.WriteLine($"Grouped arrays: found {userCount} users and {tagCount} tags");
        });
    }
}
```
