---
uid: Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters.JsonConverterCollectionExtensions
example: [*content]
---

## Examples

`JsonConverterCollectionExtensions` registers ASP.NET Core-specific converters `ProblemDetails`, `HttpExceptionDescriptor`, and `StringValues` into a `JsonConverter` collection.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

var options = new NewtonsoftJsonFormatterOptions();
options.Settings.Converters.Clear();
options.Settings.Converters
    .AddHttpExceptionDescriptorConverter()
    .AddProblemDetailsConverter()
    .AddStringValuesConverter();

Console.WriteLine(options.Settings.Converters.Count == 3);
```
