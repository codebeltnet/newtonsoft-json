---
uid: Codebelt.Extensions.Newtonsoft.Json.JsonSerializerSettingsExtensions
example: [*content]
---

## Examples

`JsonSerializerSettingsExtensions.ApplyToDefaultSettings` sets `JsonConvert.DefaultSettings` to a factory that returns the configured `JsonSerializerSettings`, making all subsequent `JsonConvert` calls use the applied configuration.

```csharp
// Program.cs
using System;
using Codebelt.Extensions.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

var options = new NewtonsoftJsonFormatterOptions();
options.Settings.Formatting = Formatting.None;
options.Settings.NullValueHandling = NullValueHandling.Ignore;
options.Settings.ApplyToDefaultSettings();

var json = JsonConvert.SerializeObject(new { Name = "Alice", Age = (int?)null });
Console.WriteLine(json.Contains("null") == false);
```
