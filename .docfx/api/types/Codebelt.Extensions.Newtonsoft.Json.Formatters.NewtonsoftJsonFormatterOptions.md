---
uid: Codebelt.Extensions.Newtonsoft.Json.Formatters.NewtonsoftJsonFormatterOptions
example: [*content]
---

## Examples

`NewtonsoftJsonFormatterOptions` configures the `NewtonsoftJsonFormatter` with `JsonSerializerSettings`, supported media types, sensitivity details for exception serialization, and whether to synchronize with `JsonConvert.DefaultSettings`.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

var options = new NewtonsoftJsonFormatterOptions
{
    SynchronizeWithJsonConvert = true,
    Settings =
    {
        Formatting = Formatting.Indented,
        DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
    }
};

Console.WriteLine(options.Settings.Formatting == Formatting.Indented);
Console.WriteLine(options.SupportedMediaTypes.Count >= 3);
```
