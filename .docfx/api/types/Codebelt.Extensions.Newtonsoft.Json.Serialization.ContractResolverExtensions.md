---
uid: Codebelt.Extensions.Newtonsoft.Json.Serialization.ContractResolverExtensions
example: [*content]
---

## Examples

`ContractResolverExtensions.ResolveNamingStrategyOrDefault` extracts the `NamingStrategy` from any `IContractResolver`, falling back to `CamelCaseNamingStrategy` for null or unresolvable instances.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Serialization;

var resolver = new CamelCasePropertyNamesContractResolver();
var strategy = resolver.ResolveNamingStrategyOrDefault();
Console.WriteLine(strategy is CamelCaseNamingStrategy);

var resolver2 = new DefaultContractResolver();
var strategy2 = resolver2.ResolveNamingStrategyOrDefault();
Console.WriteLine(strategy2 is DefaultNamingStrategy);
```
