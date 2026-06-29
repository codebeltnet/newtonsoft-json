---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonWriterExtensions
example: [*content]
---

## Examples

`JsonWriterExtensions` provides `WritePropertyName` and `WriteObject` for `JsonWriter`. `WritePropertyName` resolves the naming strategy from the serializer's contract resolver; `WriteObject` delegates to `serializer.Serialize(writer, value)`.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var sb = new StringBuilder();
using var sw = new StringWriter(sb);
using var writer = new JsonTextWriter(sw);
var serializer = JsonSerializer.Create(new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver()
});

writer.WriteStartObject();
writer.WritePropertyName("MyProperty", serializer);
writer.WriteObject(new { x = 1, y = 2 }, serializer);
writer.WriteEndObject();

Console.WriteLine(sb.ToString().Contains("myProperty"));
Console.WriteLine(sb.ToString().Contains("\"x\":1"));
```
