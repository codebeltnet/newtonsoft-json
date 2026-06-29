---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializationInputFormatter
example:
- *content
---

Create `JsonSerializationInputFormatter` directly when you want to confirm which JSON media types and encodings MVC will accept before the formatter is inserted into `MvcOptions`.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Net.Http.Headers;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;

var options = new NewtonsoftJsonFormatterOptions();
options.SupportedMediaTypes = options.SupportedMediaTypes
    .Append(MediaTypeHeaderValue.Parse("application/vnd.weather+json"))
    .ToArray();

var formatter = new JsonSerializationInputFormatter(options);

Console.WriteLine(formatter.SupportedMediaTypes.Any(mediaType => mediaType == "application/json"));
Console.WriteLine(formatter.SupportedMediaTypes.Any(mediaType => mediaType == "application/vnd.weather+json"));
Console.WriteLine(formatter.SupportedEncodings.Count);
```
