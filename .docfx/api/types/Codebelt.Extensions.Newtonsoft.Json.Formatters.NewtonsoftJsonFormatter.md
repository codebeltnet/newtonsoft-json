---
uid: Codebelt.Extensions.Newtonsoft.Json.Formatters.NewtonsoftJsonFormatter
example:
- *content
---

Applications need to convert domain objects to JSON for API responses, file persistence, and message queues, but each conversion scenario may require different settings: indentation for human debugging, null value handling for API contracts, custom converters for domain types. Writing serialization code inline is repetitive and error-prone; sharing a single `JsonSerializerSettings` instance across components is inflexible and difficult to test. The `NewtonsoftJsonFormatter` class provides a reusable, configurable JSON formatter that encapsulates Newtonsoft.Json with sensible defaults and supports both basic and advanced customization scenarios. This example demonstrates basic serialization and deserialization:

```csharp
using System;
using System.IO;
using System.Text;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

namespace Examples;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

public class BasicFormatterProgram
{
    public static void Main()
    {
        // Create formatter with default options
        var formatter = new NewtonsoftJsonFormatter();

        var person = new Person
        {
            Name = "Alice Smith",
            Age = 30,
            BirthDate = new DateTime(1994, 5, 15)
        };

        // Serialize to JSON stream
        using (var jsonStream = formatter.Serialize(person, typeof(Person)))
        {
            var jsonString = Encoding.UTF8.GetString(((MemoryStream)jsonStream).ToArray());
            Console.WriteLine("Serialized JSON:");
            Console.WriteLine(jsonString);
            Console.WriteLine();

            // Deserialize back to object
            jsonStream.Position = 0;
            var deserializedPerson = formatter.Deserialize(jsonStream, typeof(Person)) as Person;

            Console.WriteLine("Deserialized Object:");
            Console.WriteLine($"Name: {deserializedPerson?.Name}");
            Console.WriteLine($"Age: {deserializedPerson?.Age}");
        }
    }
}
```

### Custom Configuration

Many applications need fine-grained control over formatting, null value handling, naming conventions, and custom converters for domain types (enums, flags, exceptions, etc.). Configuring each formatter instance from scratch is tedious and duplicates configuration logic across the codebase. The `NewtonsoftJsonFormatter` accepts a configuration delegate that customizes `JsonSerializerSettings` before formatters are created, enabling centralized configuration that can be unit-tested and easily reused. This example demonstrates configuring indented output, null handling, camelCase property names, and custom flag enum serialization:

```csharp
using System;
using System.IO;
using System.Text;
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Examples;

[Flags]
public enum UserRole { Admin = 1, User = 2, Guest = 4 }

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public UserRole Roles { get; set; }
}

public class CustomConfigProgram
{
    public static void Main()
    {
        // Create formatter with custom configuration
        var formatter = new NewtonsoftJsonFormatter(options =>
        {
            options.Settings.Formatting = Formatting.Indented;
            options.Settings.NullValueHandling = NullValueHandling.Ignore;
            options.Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            // Clear default converters and add custom ones
            options.Settings.Converters.Clear();
            options.Settings.Converters.AddStringFlagsEnumConverter();
            options.Settings.Converters.AddStringEnumConverter();
        });

        var user = new User
        {
            Id = 123,
            Username = "alice",
            Roles = UserRole.Admin | UserRole.User
        };

        using (var jsonStream = formatter.Serialize(user, typeof(User)))
        {
            var jsonString = Encoding.UTF8.GetString(((MemoryStream)jsonStream).ToArray());
            Console.WriteLine(jsonString);
        }
    }
}
```

### Synchronizing with JsonConvert

The `JsonConvert.DefaultSettings` property allows setting global serialization behavior for ad-hoc JSON operations throughout an application, but it often conflicts with application-specific formatter settings and makes testing difficult. The `SynchronizeWithJsonConvert` option on `NewtonsoftJsonFormatterOptions` bridges this gap by registering the formatter's settings as the global default, ensuring consistent behavior between formatter instances and static `JsonConvert` calls without duplicating configuration. This example demonstrates synchronizing formatter settings with the global `JsonConvert.DefaultSettings`:

```csharp
using System;
using System.IO;
using System.Text;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

namespace Examples;

public class SyncProgram
{
    public static void Main()
    {
        var formatter = new NewtonsoftJsonFormatter(options =>
        {
            options.SynchronizeWithJsonConvert = true;
            options.Settings.Formatting = Formatting.Indented;
            options.Settings.NullValueHandling = NullValueHandling.Ignore;
        });

        // When SynchronizeWithJsonConvert is true, JsonConvert.SerializeObject will use the configured settings
        var data = new { name = "test", value = (string)null };
        
        using (var stream = formatter.Serialize(data, data.GetType()))
        {
            var jsonString = Encoding.UTF8.GetString(((MemoryStream)stream).ToArray());
            Console.WriteLine(jsonString);
        }
    }
}
```

The `NewtonsoftJsonFormatter` automatically refreshes converter dependencies based on sensitivity settings and can optionally synchronize with global `JsonConvert` settings. It inherits from `StreamFormatter<NewtonsoftJsonFormatterOptions>` and provides both parameterless and configuration-based constructors for flexible initialization.
