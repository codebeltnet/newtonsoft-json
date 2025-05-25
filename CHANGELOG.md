# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

> [!NOTE]  
> Changelog entries prior to version 8.4.0 was migrated from previous versions of Cuemon.Extensions.Newtonsoft.Json, Cuemon.Extensions.AspNetCore.Newtonsoft.Json and Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.

## [9.0.3] - 2025-05-25

This is a service update that focuses on package dependencies.

## [9.0.2] - 2025-04-16

This is a service update that focuses on package dependencies.

## [9.0.1] - 2025-01-30

This is a service update that primarily focuses on package dependencies and minor improvements.

## [9.0.0] - 2024-11-13

This major release is first and foremost focused on ironing out any wrinkles that have been introduced with .NET 9 preview releases so the final release is production ready together with the official launch from Microsoft.

### Added

- FailureConverter class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace to convert FailureConverter to JSON

### Changed

- JsonConverterCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Converters namespace was extended to include one new extension method: AddProblemDetailsConverter
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace was extended to include one new extension method: AddFailureConverter
- ValidatorExtensions class in the Codebelt.Extensions.Newtonsoft.Json namespace to be compliant with https://rules.sonarsource.com/csharp/type/Bug/RSPEC-3343/ (breaking change)
- DynamicJsonConverter class in the Codebelt.Extensions.Newtonsoft.Json namespace was renamed to JsonConverterFactory (breaking change)

### Removed

- HttpExceptionDescriptorResponseHandlerExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace (breaking change)

## [8.4.0] - 2024-09-22

### Dependencies

- Codebelt.Extensions.Newtonsoft.Json updated to latest and greatest with respect to TFMs
- Codebelt.Extensions.AspNetCore.Newtonsoft.Json updated to latest and greatest with respect to TFMs
- Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json updated to latest and greatest with respect to TFMs

## [8.3.0] - 2024-04-09

### Fixed

- ExceptionConverter class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace to use Environment.NewLine instead of Alphanumeric.NewLine (vital for non-Windows operating systems)


## [8.1.0] - 2024-02-11

### Added

- JsonConverterCollectionExtensions class in the Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters namespace that consist of extension methods for the JsonConverter class: AddHttpExceptionDescriptorConverter and AddStringValuesConverter
- ServiceCollectionExtensions class in the Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters namespace that consist of extension methods for the IServiceCollection interface: AddNewtonsoftJsonFormatterOptions and AddNewtonsoftJsonExceptionResponseFormatter

### Fixed

- HttpExceptionDescriptorResponseHandlerExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace so that AddNewtonsoftJsonResponseHandler now enumerates all supported media types in regards to content negotiation

### Changed

- NewtonsoftJsonFormatterOptions class in the Codebelt.Extensions.Newtonsoft.Json.Formatters namespace to derive from IExceptionDescriptorOptions
- HttpExceptionDescriptorResponseHandlerExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json marking the method, AddNewtonsoftJsonResponseHandler, obsolete (should use AddNewtonsoftJsonExceptionResponseFormatter instead)
- MvcBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to be more lean having only two extension methods remaining; AddNewtonsoftJsonFormatters and AddNewtonsoftJsonFormattersOptions
- MvcCoreBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to be more lean having only two extension methods remaining; AddNewtonsoftJsonFormatters and AddNewtonsoftJsonFormattersOptions


## [8.0.1] - 2024-01-11

### Fixed

- NewtonsoftJsonFormatterOptions class in the Codebelt.Extensions.Newtonsoft.Json.Formatters namespace to be consistent with general date time handling; applied DateFormatString = "O"


## [8.0.0] - 2023-11-14

### Changed

- DateParseHandling from `DateTimeOffset` to `DateTime` (as majority of Codebelt is the latter) on the JsonFormatterOptions class in the Codebelt.Extensions.Newtonsoft.Json.Formatters namespace
- ContractResolver to use custom rules as Newtonsoft relies heavily on the now deprecated ISerializable and SerializableAttribute
- Best effort to have consistency between System.Text.Json and Newtonsoft.Json serialization/deserialization
- JsonFormatter class in the Codebelt.Extensions.Newtonsoft.Json.Formatters namespace was renamed to NewtonsoftJsonFormatter
- JsonFormatterOptions class in the Codebelt.Extensions.Newtonsoft.Json.Formatters namespace was renamed to NewtonsoftJsonFormatterOptions

### Fixed

- AddNewtonsoftJsonResponseHandler extension method to properly propagate options to NewtonsoftJsonFormatter serialization method in the HttpExceptionDescriptorResponseHandlerExtensions in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace


## [7.0.0] 2022-11-09

### Added

- DateTimeConverter class in the Codebelt.Extensions.Text.Json.Converters namespace that provides a DateTime converter that can be configured like the Newtonsoft.JSON equivalent
- JsonFormatter class in the Codebelt.Extensions.Newtonsoft.Json.Formatters namespace was extended with two static methods; SerializeObject and DeserializeObject
- HttpExceptionDescriptorResponseHandlerExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace that consist of extension methods for the HttpExceptionDescriptorResponseHandler class: AddNewtonsoftJsonResponseHandler
- ExceptionConverter class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace that converts an Exception to or from JSON

### Changed

- MvcBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonSerializationFormatters method to AddNewtonsoftJsonFormatters
- MvcBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonFormatterOptions method to AddNewtonsoftJsonFormattersOptions
- MvcCoreBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonSerializationFormatters method to AddNewtonsoftJsonFormatters
- MvcCoreBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonFormatterOptions method to AddNewtonsoftJsonFormattersOptions
- JsonFormatterOptions class in the Codebelt.Extensions.Newtonsoft.Json.Formatters namespace to use DateTimeZoneHandling.RoundtripKind instead of DateTimeZoneHandling.Utc when dealing with DateTimeZoneHandling

### Removed

- AddTimeSpanConverter extension method from JsonConverterCollectionExtensions class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace
- ExceptionDescriptorExtensions class from the Codebelt.Extensions.Newtonsoft.Json.Diagnostics namespace

### Fixed

- StringFlagsEnumConverter class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace so that it includes check on FlagsAttribute definition in inherited CanConvert method


## [6.0.0] - 2021-04-18

### Added

- ExceptionDescriptorExtensions class in the Codebelt.Extensions.Newtonsoft.Json.Diagnostics namespace that consist of extension methods for the ExceptionDescriptor class: ToInsightsJsonString
- JData class in the Codebelt.Extensions.Newtonsoft.Json namespace that provides a factory based way to parse and extract values from various sources of JSON data. Compliant with RFC 7159 as it uses JsonTextReader behind the scene
- JDataResultExtensions class in the Codebelt.Extensions.Newtonsoft.Json namespace that consist of extension methods for the JDataResult class: ExtractArrayValues, ExtractObjectValues
- ValidatorExtensions class in the Codebelt.Extensions.Newtonsoft.Json namespace that consist of extension methods for the Validator class: InvalidJsonDocument
- ContractResolverExtensions class in the Codebelt.Extensions.Newtonsoft.Json.Serialization namespace that consist of extension methods for the IContractResolver interface: ResolveNamingStrategyOrDefault

### Changed

- JsonReaderResult class in the Codebelt.Extensions.Newtonsoft.Json namespace was renamed to JDataResult (including some refactoring)
- StringFlagsEnumConverter class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace to comply with Newtonsoft.Json.Serialization.NamingStrategy implementations
- JsonFormatterOptions class in the Codebelt.Extensions.Newtonsoft.Json namespace with several new options and a uniform way of adding default converters
- JsonConverterCollectionExtensions class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace to fully support whatever desired naming strategy wanted while simplifying the code greatly
- StringFlagsEnumConverter class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace to fully support whatever desired naming strategy wanted while simplifying the code greatly
- DynamicJsonConverter class in the Codebelt.Extensions.Newtonsoft.Json namespace to fully support whatever desired naming strategy wanted while being significantly more versatile in usage
- JsonWriterExtensions class in the Codebelt.Extensions.Newtonsoft.Json namespace to fully support whatever desired naming strategy wanted while simplifying the code greatly

### Fixed

- All relevant classes in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to be compliant with https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30#allowsynchronousio-disabled
- JsonSerializationInputFormatter class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to have 0 duplicated blocks of lines of code
- JsonSerializationOutputFormatter class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to have 0 duplicated blocks of lines of code
- JsonConverterCollectionExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Converters namespace to have 0 duplicated blocks of lines of code
- Justified https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2200 on ValidatorExtensions class in the Codebelt.Extensions.Newtonsoft.Json namespace
- JsonReaderExtensions class in the Codebelt.Extensions.Newtonsoft.Json namespace to have 0 duplicated blocks of lines of code
- JsonConverterCollectionExtensions class in the Codebelt.Extensions.Newtonsoft.Json.Converters namespace to have 0 duplicated blocks of lines of code

### Removed

- Any types found in the Codebelt.AspNetCore.Mvc.Formatters.Json namespace was merged into the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace
- DefaultJsonSerializerSettings class from the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace
- Any types found in the Codebelt.Serialization.Json namespace was merged into the Codebelt.Extensions.Newtonsoft.Json namespace
- JsonReaderResultExtensions class from the Codebelt.Extensions.Newtonsoft.Json namespace
- JsonReaderParser class from the Codebelt.Extensions.Newtonsoft.Json namespace
