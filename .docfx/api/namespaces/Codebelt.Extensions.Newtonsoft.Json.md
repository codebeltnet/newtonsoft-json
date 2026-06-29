---
uid: Codebelt.Extensions.Newtonsoft.Json
summary: *content
---
The `Codebelt.Extensions.Newtonsoft.Json` namespace helps you focus on your domain objects instead of writing boilerplate JSON serialization code. When you need dynamic converter creation, custom contract resolution per property, or rich JSON parsing that surfaces path and value details, this namespace provides the building blocks. Start with `JData.ReadAll` to parse JSON into navigable `JDataResult` sequences; use `JsonConverterFactory.Create` when you need a custom `JsonConverter` from a delegate; reach for `DynamicContractResolver.Create` when per-property contract customization is required.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Newtonsoft.Json namespace](https://www.newtonsoft.com/json/help/html/N_Newtonsoft_Json.htm) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IEnumerable&lt;JDataResult&gt;|⬇️|`Flatten`, `ExtractArrayValues`, `ExtractObjectValues`|
|JsonSerializerSettings|⬇️|`ApplyToDefaultSettings`|
|JsonWriter|⬇️|`WriteObject`, `WritePropertyName`|
|Validator|⬇️|`InvalidJsonDocument`|
