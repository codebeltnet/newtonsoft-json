---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.TransientFaultExceptionConverter
example:
- *content
---

Use `TransientFaultExceptionConverter` together with the exception converter when transient-fault details must round-trip through JSON without losing the captured retry evidence.

```csharp
// Program.cs
using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Reflection;
using Cuemon.Resilience;
using Newtonsoft.Json;

var evidence = new TransientFaultEvidence(
    3,
    TimeSpan.FromMilliseconds(100),
    TimeSpan.FromMilliseconds(300),
    TimeSpan.FromMilliseconds(50),
    new MethodSignature("PaymentsClient", "RetryAsync", Array.Empty<string>(), Array.Empty<object>()));

var original = new TransientFaultException(
    "Service unavailable",
    new TimeoutException("Gateway timed out"),
    evidence);

var converter = new TransientFaultExceptionConverter();
var settings = new JsonSerializerSettings();
settings.Converters.Add(converter);
settings.Converters.AddExceptionConverter(false, false);

var formatter = new NewtonsoftJsonFormatter(options => options.Settings = settings);

var stream = formatter.Serialize(original, typeof(TransientFaultException));
var json = new StreamReader(stream).ReadToEnd();
stream.Position = 0;

var restored = (TransientFaultException)formatter.Deserialize(stream, typeof(TransientFaultException));

Console.WriteLine(json.Contains("TransientFaultException", StringComparison.Ordinal));
Console.WriteLine(restored.Message);
Console.WriteLine(restored.Evidence.Attempts);
```
