---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.ExceptionConverter
example: [*content]
---

## Examples

`ExceptionConverter` serializes and deserializes exceptions to and from JSON. It handles `Exception` and all derived types. Enable `includeStackTrace` and `includeData` to capture stack traces and `Exception.Data` entries.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var converter = new ExceptionConverter(includeStackTrace: true, includeData: false);
var settings = new JsonSerializerSettings();
settings.Converters.Add(converter);

var ex = new InvalidOperationException("Something went wrong");
var sb = new StringBuilder();
using (var sw = new StringWriter(sb))
using (var writer = new JsonTextWriter(sw))
{
    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    var serializer = JsonSerializer.Create(settings);
    serializer.Serialize(writer, ex);
}

Console.WriteLine(sb.ToString().Contains("InvalidOperationException"));
```
