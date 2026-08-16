---
uid: Codebelt.Extensions.Newtonsoft.Json.Serialization.ContractResolverExtensions
example:
- *content
---
The following example resolves or provides a default naming strategy from a contract resolver.

```csharp
using System;
using Codebelt.Extensions.Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Serialization;

namespace Examples;

class ContractResolverExtensionsExample
{
    static void Main()
    {
        var resolver = new DefaultContractResolver();
        var namingStrategy = resolver.ResolveNamingStrategyOrDefault();

        Console.WriteLine($"Naming strategy type: {namingStrategy.GetType().Name}");
        var converted = namingStrategy.GetPropertyName("UserName", false);
        Console.WriteLine($"UserName converted to: {converted}");
    }
}
```

---
uid: Codebelt.Extensions.Newtonsoft.Json.Serialization.ContractResolverExtensions.ResolveNamingStrategyOrDefault
example:
- *content
---

Contract resolvers in JSON.NET support optional `NamingStrategy` instances that transform property names during serialization (camelCase, snake_case, PascalCase, etc.), but retrieving the naming strategy requires inspecting resolver properties that may be null or missing depending on the resolver type. Applications that need to apply the same naming transformation used by a contract resolver to property names in different contexts—logs, error messages, API documentation generation—need a reliable way to extract the naming strategy without type-specific knowledge. The `ResolveNamingStrategyOrDefault` extension method returns the resolver's naming strategy if present, or a default pass-through strategy if none is configured, ensuring callers always receive a valid strategy for property name transformation. This example demonstrates resolving the naming strategy and applying it to a property name:

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Serialization;

var resolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
var strategy = resolver.ResolveNamingStrategyOrDefault();
var result = strategy.GetPropertyName("FirstName", false);

Console.WriteLine($"FirstName becomes: {result}");
```
