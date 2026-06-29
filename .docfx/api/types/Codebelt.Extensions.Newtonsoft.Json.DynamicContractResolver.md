---
uid: Codebelt.Extensions.Newtonsoft.Json.DynamicContractResolver
example: [*content]
---

## Examples

`DynamicContractResolver` creates `IContractResolver` instances with per-property handler callbacks. Call `Create<T>` where `T` is `CamelCasePropertyNamesContractResolver` or `DefaultContractResolver`.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var resolver = DynamicContractResolver.Create<CamelCasePropertyNamesContractResolver>(
    (pi, jp) =>
    {
        if (pi.Name == "Id")
        {
            jp.PropertyName = "identifier";
        }
    });

var settings = new JsonSerializerSettings { ContractResolver = resolver };
var json = JsonConvert.SerializeObject(new { Id = 42, Name = "Alice" }, settings);
Console.WriteLine(json.Contains("identifier"));
```
