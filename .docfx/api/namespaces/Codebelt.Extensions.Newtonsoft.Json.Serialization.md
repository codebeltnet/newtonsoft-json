---
uid: Codebelt.Extensions.Newtonsoft.Json.Serialization
summary: *content
---
The `Codebelt.Extensions.Newtonsoft.Json.Serialization` namespace resolves the configured `NamingStrategy` from any `IContractResolver`, saving you from type-checking the resolver at runtime. Call `ResolveNamingStrategyOrDefault` on an `IContractResolver` to extract its `NamingStrategy` (for example, `CamelCaseNamingStrategy` from `CamelCasePropertyNamesContractResolver`) or fall back to a default when none is set. This is the sole entry point in this namespace.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Newtonsoft.Json.Serialization namespace](https://www.newtonsoft.com/json/help/html/N_Newtonsoft_Json_Serialization.htm) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IContractResolver|⬇️|`ResolveNamingStrategyOrDefault`|
