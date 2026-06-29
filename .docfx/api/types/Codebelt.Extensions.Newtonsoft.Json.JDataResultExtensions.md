---
uid: Codebelt.Extensions.Newtonsoft.Json.JDataResultExtensions
example:
- *content
---

`JDataResultExtensions` helps you flatten a `JDataResult` tree and then extract values by the JSON paths produced by that flattened sequence.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using System.Linq;

var json = @"{ ""book"": { ""title"": ""Moby Dick"", ""price"": 12.99 } }";
var results = JData.ReadAll(json).Flatten().ToList();
results.ExtractObjectValues("book.title, book.price", dict =>
{
    foreach (var kv in dict)
    {
        Console.WriteLine($"{kv.Key}: {kv.Value.Value}");
    }
});

var json2 = @"{ ""items"": [{ ""id"": 1 }, { ""id"": 2 }] }";
var results2 = JData.ReadAll(json2).ToList();
results2.ExtractArrayValues("items", dict =>
{
    foreach (var kv in dict)
    {
        Console.WriteLine($"{kv.Key}: {kv.Value.Count()} items");
    }
});
```
