---
uid: Codebelt.Extensions.Newtonsoft.Json.ValidatorExtensions
example: [*content]
---

## Examples

`ValidatorExtensions` extends `Validator` with `InvalidJsonDocument` to guard method arguments against invalid JSON strings and `JsonReader` instances.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Cuemon;

var validJson = @"{ ""id"": ""abc-123"" }";
Validator.ThrowIf.InvalidJsonDocument(validJson, paramName: "validJson");

try
{
    var invalidJson = @"{ broken";
    Validator.ThrowIf.InvalidJsonDocument(invalidJson, paramName: "invalidJson");
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message.StartsWith("Value must be a JSON representation"));
}
```
