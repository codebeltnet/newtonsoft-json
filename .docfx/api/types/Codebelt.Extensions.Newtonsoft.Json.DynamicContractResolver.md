---
uid: Codebelt.Extensions.Newtonsoft.Json.DynamicContractResolver
example:
- *content
---

Contract resolvers in JSON.NET determine how object properties are discovered, named, and serialized during JSON conversion, but creating custom resolvers typically requires subclassing `DefaultContractResolver` or `CamelCasePropertyNamesContractResolver` and overriding protected methods. This creates tight coupling to specific resolver implementations and makes it difficult to combine multiple customization strategies (property filtering, renaming, attribute handling) without complex class hierarchies. The `DynamicContractResolver` factory provides a simpler pattern: create a resolver instance using a factory method while passing handler delegates that customize `JsonProperty` metadata on a property-by-property basis. This enables composable, reusable property customization without subclassing. This example demonstrates creating a dynamic contract resolver with custom property handlers:

```csharp
using System;
using System.Reflection;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Examples;

public class DynamicResolverExample
{
    static void Main()
    {
        // Create a handler that marks all properties as required
        void RequiredPropertyHandler(PropertyInfo info, JsonProperty property)
        {
            if (info != null && property != null)
            {
                property.Required = Required.Always;
            }
        }

        // Create a handler that skips properties with a certain name
        void SkipPropertyHandler(PropertyInfo info, JsonProperty property)
        {
            if (info?.Name == "Secret")
            {
                property.Ignored = true;
            }
        }

        // Create a dynamic contract resolver with custom handlers
        var resolver = DynamicContractResolver.Create<DefaultContractResolver>(
            RequiredPropertyHandler,
            SkipPropertyHandler
        );

        var settings = new JsonSerializerSettings { ContractResolver = resolver };

        var data = new { Name = "Alice", Secret = "hidden", Age = 30 };
        var json = JsonConvert.SerializeObject(data, settings);
        
        Console.WriteLine("Serialized with dynamic contract resolver:");
        Console.WriteLine(json);
    }
}
```

The `DynamicContractResolver` factory method accepts the resolver type to create (e.g., `DefaultContractResolver`, `CamelCasePropertyNamesContractResolver`) and one or more handler delegates that customize each property's JSON representation. Handlers are invoked for every property discovered during contract resolution, enabling per-property customization without creating custom resolver subclasses.
