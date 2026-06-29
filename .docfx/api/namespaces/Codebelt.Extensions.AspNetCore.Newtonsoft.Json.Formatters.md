---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters
summary: *content
---
The `Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters` namespace registers `NewtonsoftJsonFormatterOptions` and the `IHttpExceptionDescriptorResponseFormatter` into ASP.NET Core's `IServiceCollection`. Start with `AddNewtonsoftJsonExceptionResponseFormatter` on `IServiceCollection` to wire up exception-to-JSON formatting with the response formatter registered; use `AddNewtonsoftJsonFormatterOptions` when you only need the options registered without the response formatter.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Codebelt.Extensions.Newtonsoft.Json namespace](/api/extensions/jsonnet/Cuemon.Extensions.Newtonsoft.Json.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddNewtonsoftJsonFormatterOptions`, `AddNewtonsoftJsonExceptionResponseFormatter`|
