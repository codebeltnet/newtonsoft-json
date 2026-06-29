---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions
example: [*content]
---

## Examples

`JsonConverterCollectionExtensions` registers common Newtonsoft.Json converters into a `JsonConverter` collection via fluent extension methods.

```csharp
// Program.cs
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

var converters = new List<JsonConverter>();
converters
    .AddStringEnumConverter(new CamelCaseNamingStrategy())
    .AddStringFlagsEnumConverter()
    .AddExceptionConverter(false, false)
    .AddTransientFaultExceptionConverter()
    .AddFailureConverter()
    .AddDataPairConverter();

var settings = new JsonSerializerSettings();
foreach (var c in converters) { settings.Converters.Add(c); }

settings.Converters.AddExceptionDescriptorConverterOf<Cuemon.Diagnostics.ExceptionDescriptor>();

var json = JsonConvert.SerializeObject(StringComparison.Ordinal, settings);
Console.WriteLine(json == "\"ordinal\"");
```
