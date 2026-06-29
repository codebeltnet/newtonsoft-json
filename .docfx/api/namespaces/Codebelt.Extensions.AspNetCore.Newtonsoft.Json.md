---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json
summary: *content
---
The `Codebelt.Extensions.AspNetCore.Newtonsoft.Json` namespace wires Newtonsoft.Json into ASP.NET Core's dependency injection container so exception responses serialize through `IHttpExceptionDescriptorResponseFormatter` without manual wiring. Use it in any ASP.NET Core project where structured JSON error responses must flow through Newtonsoft.Json serialization. Call `AddMinimalNewtonsoftJsonOptions` on `IServiceCollection` to register `NewtonsoftJsonFormatterOptions` and the response formatter in a single call — this is the primary entry point in this namespace.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Codebelt.Extensions.Newtonsoft.Json namespace](/api/extensions/jsonnet/Cuemon.Extensions.Newtonsoft.Json.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddMinimalNewtonsoftJsonOptions`|
