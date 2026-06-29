---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters
summary: *content
---
The `Codebelt.Extensions.Newtonsoft.Json.Converters` namespace provides Newtonsoft.Json converters for enums, exceptions, failures, and data pairs, plus extension methods that register them into a `JsonConverter` collection. Use `AddStringEnumConverter` to serialize enums as their string names, `AddExceptionConverter` to write exceptions with configurable stack-trace and data inclusion, and the remaining converters for specialized `TransientFaultException`, `DataPair`, and `Failure` serialization.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Newtonsoft.Json.Converters namespace](https://www.newtonsoft.com/json/help/html/N_Newtonsoft_Json_Converters.htm) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection<JsonConverter>|⬇️|`AddStringEnumConverter`, `AddStringFlagsEnumConverter`, `AddExceptionDescriptorConverterOf<T>`, `AddExceptionConverter`, `AddDataPairConverter`, `AddTransientFaultExceptionConverter`, `AddFailureConverter`|
