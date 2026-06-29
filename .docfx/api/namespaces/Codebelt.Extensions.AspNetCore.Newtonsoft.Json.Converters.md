---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters
summary: *content
---
The `Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters` namespace registers ASP.NET Core-specific Json.NET converters — `ProblemDetails`, `HttpExceptionDescriptor`, and `StringValues` — into a converter collection. Use this namespace in ASP.NET Core applications where HTTP error responses must serialize through the standard `IHttpExceptionDescriptorResponseFormatter` pipeline. Start with `AddHttpExceptionDescriptorConverter` on `ICollection<JsonConverter>` to register an `HttpExceptionDescriptor` converter with optional options; `AddProblemDetailsConverter` and `AddStringValuesConverter` handle the remaining ASP.NET Core types.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Codebelt.Extensions.Newtonsoft.Json.Converters namespace](/api/extensions/jsonnet/Cuemon.Extensions.Newtonsoft.Json.Converters.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection&lt;JsonConverter&gt;|⬇️|`AddHttpExceptionDescriptorConverter`, `AddProblemDetailsConverter`, `AddStringValuesConverter`|
