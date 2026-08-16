---
uid: Codebelt.Extensions.Newtonsoft.Json.Converters.StringFlagsEnumConverter
example:
- *content
---

Flag enumerations represent combinations of independent boolean options (read, write, execute, delete), but JSON.NET serializes them as numeric values that aren't human-readable and don't reflect the semantic intent of multi-value flags. REST APIs and configuration systems need to represent flags as arrays or comma-separated strings that document which options are actually enabled. The `StringFlagsEnumConverter` handles this by detecting `[Flags]` attributes and serializing flag combinations as JSON arrays while keeping single flags as strings, producing readable, semantic output. You can customize the naming strategy (camelCase, PascalCase, etc.) by passing a `NamingStrategy` to the constructor. This example demonstrates how to use the `StringFlagsEnumConverter` with a flags enumeration:

```csharp
using Codebelt.Extensions.Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;

namespace MyApplication
{
    [Flags]
    public enum FilePermissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        Delete = 8
    }

    public class FileAccessConfig
    {
        public string FileName { get; set; }
        public FilePermissions Permissions { get; set; }
    }

    public class FlagsEnumProgram
    {
        public static void Main()
        {
            // Create settings with StringFlagsEnumConverter
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringFlagsEnumConverter());

            // Serialize multiple flags combined
            var config = new FileAccessConfig
            {
                FileName = "document.txt",
                Permissions = FilePermissions.Read | FilePermissions.Write | FilePermissions.Delete
            };

            var json = JsonConvert.SerializeObject(config, settings);
            Console.WriteLine("Serialized with Flags as Array:");
            Console.WriteLine(json);
            Console.WriteLine();

            // Serialize single flag
            var readOnly = new FileAccessConfig
            {
                FileName = "readonly.txt",
                Permissions = FilePermissions.Read
            };

            var jsonSingle = JsonConvert.SerializeObject(readOnly, settings);
            Console.WriteLine("Serialized with Single Flag:");
            Console.WriteLine(jsonSingle);
        }
    }
}
```

The converter produces the following JSON output:

```json
{
  "fileName": "document.txt",
  "permissions": ["read", "write", "delete"]
}
```

For a single flag:

```json
{
  "fileName": "readonly.txt",
  "permissions": "read"
}
```

The converter automatically detects `[Flags]` attributed enumerations and serializes combinations as JSON arrays, while non-flags enumerations and single flag values are serialized as string values.
