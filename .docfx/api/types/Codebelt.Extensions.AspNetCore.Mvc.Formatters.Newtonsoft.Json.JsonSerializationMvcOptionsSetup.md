---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.JsonSerializationMvcOptionsSetup
example:
- *content
---

ASP.NET Core's dependency injection system calls `IConfigureOptions<MvcOptions>` implementations during MVC setup to register formatters and configure behavior, but manually creating and registering these setup classes is verbose and error-prone. The `JsonSerializationMvcOptionsSetup` class encapsulates the work of registering both input and output formatters with consistent Newtonsoft.Json configuration, and when registered via extension methods like `AddMvc()`, it seamlessly integrates formatters into the standard MVC pipeline without requiring developers to handle setup details. This example demonstrates direct instantiation and usage of this `IConfigureOptions<MvcOptions>` implementation:

```csharp
using System;
using System.Linq;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Examples;

class JsonSerializationMvcOptionsSetupExample
{
    static void Main()
    {
        // Create formatter options
        var formatterOptions = new NewtonsoftJsonFormatterOptions
        {
            Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            }
        };

        // Create MVC options
        var mvcOptions = new MvcOptions();

        // Create and apply the setup
        var setup = new JsonSerializationMvcOptionsSetup(
            Options.Create(formatterOptions)
        );

        // Configure MVC options with the formatters
        setup.Configure(mvcOptions);

        // Verify formatters were added
        Console.WriteLine($"Input formatters count: {mvcOptions.InputFormatters.Count}");
        Console.WriteLine($"Output formatters count: {mvcOptions.OutputFormatters.Count}");

        // Check if our formatters are present
        var hasInputFormatter = mvcOptions.InputFormatters.OfType<JsonSerializationInputFormatter>().Any();
        var hasOutputFormatter = mvcOptions.OutputFormatters.OfType<JsonSerializationOutputFormatter>().Any();

        Console.WriteLine($"Has JsonSerializationInputFormatter: {hasInputFormatter}");
        Console.WriteLine($"Has JsonSerializationOutputFormatter: {hasOutputFormatter}");
    }
}
```

The setup automatically inserts both `JsonSerializationInputFormatter` and `JsonSerializationOutputFormatter` at the beginning of the MVC formatters collection, ensuring Newtonsoft.Json is the primary JSON handler for your application.
