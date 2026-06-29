---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializationOutputFormatter
example:
- *content
---

Create `JsonSerializationOutputFormatter` directly when you need to inspect the response media types MVC will negotiate after you customize the shared Newtonsoft.Json formatter options.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Net.Http.Headers;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;

var options = new NewtonsoftJsonFormatterOptions();
options.SupportedMediaTypes = options.SupportedMediaTypes
    .Append(MediaTypeHeaderValue.Parse("application/vnd.invoice+json"))
    .ToArray();

var formatter = new JsonSerializationOutputFormatter(options);

Console.WriteLine(formatter.SupportedMediaTypes.Any(mediaType => mediaType == "application/problem+json"));
Console.WriteLine(formatter.SupportedMediaTypes.Any(mediaType => mediaType == "application/vnd.invoice+json"));
Console.WriteLine(formatter.SupportedEncodings.Count);
```
