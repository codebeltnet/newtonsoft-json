---
uid: Codebelt.Extensions.Newtonsoft.Json.Formatters.NewtonsoftJsonFormatter
example: [*content]
---

## Examples

`NewtonsoftJsonFormatter` serializes and deserializes objects to and from JSON streams using Newtonsoft.Json. Use the static `SerializeObject` and `DeserializeObject<T>` convenience methods for simple round-trips.

```csharp
// Program.cs
using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

var formatter = new NewtonsoftJsonFormatter(o =>
{
    o.Settings.Formatting = Formatting.Indented;
    o.Settings.NullValueHandling = NullValueHandling.Ignore;
});

var source = new { Message = "Hello" };
using var stream = formatter.Serialize(source, source.GetType());
var result = formatter.Deserialize(stream, source.GetType());
Console.WriteLine(result?.GetType().GetProperty("Message")?.GetValue(result));

var json = NewtonsoftJsonFormatter.SerializeObject(new { Count = 42 });
var deserialized = NewtonsoftJsonFormatter.DeserializeObject<dynamic>(json);
Console.WriteLine((int)deserialized.Count);
```
