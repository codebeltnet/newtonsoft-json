---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.StringFlagsEnumConverter
example: [*content]
---

## Examples

`StringFlagsEnumConverter` serializes `[Flags]` enums as JSON arrays of named string values instead of a numeric bitmask. It extends `StringEnumConverter` and applies the configured naming strategy.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var settings = new JsonSerializerSettings();
settings.Converters.Add(new StringFlagsEnumConverter(new CamelCaseNamingStrategy()));

var access = FileAccess.Read | FileAccess.Write;
var json = JsonConvert.SerializeObject(access, settings);
Console.WriteLine(json == "[\"read\",\"write\"]");

[Flags]
public enum FileAccess
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}
```
