---
uid: Codebelt.Extensions.Newtonsoft.Json.JDataResult
example: [*content]
---

## Examples

`JDataResult` represents a node in the JSON tree produced by `JData.ReadAll`. Each node carries its path, property name, value, CLR type, children, and parent reference.

```csharp
// Program.cs
using System;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json;

var json = @"{ ""name"": ""Alice"", ""age"": 30 }";
JDataResult[] results = JData.ReadAll(json).ToArray();

foreach (JDataResult r in results)
{
    Console.WriteLine($"{r.Path}: {r.PropertyName} = {r.Value} ({r.Type.Name})");
}
```
