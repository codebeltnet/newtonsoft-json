---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
summary: *content
---
The `Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json` namespace inserts Newtonsoft.Json-backed input and output formatters at the front of the ASP.NET Core MVC pipeline, letting MVC negotiate requests and responses through Newtonsoft.Json while leaving the built-in formatters available behind them. Use it in ASP.NET Core MVC or Web API applications that must keep Newtonsoft.Json converters, contract resolvers, or fault formatting without abandoning MVC's formatter infrastructure. Start with `AddNewtonsoftJsonFormatters` on `IMvcCoreBuilder` to register both the formatters and `NewtonsoftJsonFormatterOptions`; use `AddNewtonsoftJsonFormattersOptions` when MVC formatter insertion is already handled elsewhere and you still need the shared formatter options plus the exception-response formatter.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Codebelt.Extensions.Newtonsoft.Json namespace](/api/extensions/jsonnet/Cuemon.Extensions.Newtonsoft.Json.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IMvcBuilder|⬇️|`AddNewtonsoftJsonFormatters`, `AddNewtonsoftJsonFormattersOptions`|
|IMvcCoreBuilder|⬇️|`AddNewtonsoftJsonFormatters`, `AddNewtonsoftJsonFormattersOptions`|
|JsonSerializerSettings|⬇️|`Use<T>`|
