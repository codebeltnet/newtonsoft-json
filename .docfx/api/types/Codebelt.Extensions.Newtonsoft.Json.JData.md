---
uid: Codebelt.Extensions.Newtonsoft.Json.JData
example: [*content]
---

## Examples

`JData` reads JSON from streams, strings, or `JsonReader` instances into `IEnumerable<JDataResult>` sequences, enabling structured navigation of the parsed document without deserializing to a statically typed model.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using System.Linq;

var json = @"{ ""store"": { ""book"": [{ ""title"": ""Moby Dick"" }, { ""title"": ""Hamlet"" }] } }";
var results = JData.ReadAll(json).ToList();
var titles = results.Flatten()
    .Where(r => r.PropertyName == "title")
    .Select(r => r.Value);

foreach (var title in titles)
{
    Console.WriteLine(title);
}
```
