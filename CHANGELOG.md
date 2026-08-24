# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

> [!NOTE]  
> Changelog entries prior to version 8.4.0 was migrated from previous versions of Cuemon.Extensions.Newtonsoft.Json, Cuemon.Extensions.AspNetCore.Newtonsoft.Json and Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.

## [10.1.8] - 2026-08-24

This is a service update that focuses on package dependencies.

## [10.1.7] - 2026-08-16

This is a patch release focused on dependency service updates, test infrastructure consolidation, and enhanced API documentation.

### Changed

- Package dependencies upgraded:
  - Codebelt.Extensions.Xunit (11.1.2 → 11.2.1),
  - Codebelt.Extensions.Xunit.App (11.1.2 → 11.2.1),
  - Cuemon.AspNetCore.Mvc (10.5.5 → 10.7.0),
  - Cuemon.Core (10.5.5 → 10.7.0),
  - Cuemon.Extensions.AspNetCore (10.5.5 → 10.7.0),
  - Cuemon.Extensions.AspNetCore.Authentication (10.5.5 → 10.7.0),
  - Cuemon.Extensions.AspNetCore.Mvc (10.5.5 → 10.7.0),
  - Cuemon.Extensions.Core (10.5.5 → 10.7.0),
  - Cuemon.Extensions.IO (10.5.5 → 10.7.0),
  - Cuemon.IO (10.5.5 → 10.7.0),
  - Microsoft.NET.Test.Sdk (18.8.1 → 18.9.0),
  - Microsoft.AspNetCore.Mvc.NewtonsoftJson (9.0.17 → 9.0.19 for net9, 10.0.9 → 10.0.11 for net10),
- Test environment configuration simplified by consolidating separate Docker test runners for net9 and net10 into unified codebeltnet/ubuntu-testrunner:8-9-10-11 image, ensuring consistent test execution across all supported TFMs,
- DocFX type documentation enhanced with comprehensive usage examples, demonstrating core capabilities including streaming JSON parsing, formatter integration, converter usage, and configuration patterns for both ASP.NET Core and standalone applications.

## [10.1.6] - 2026-07-23

This is a patch release focused on dependency service updates, tighter source-project build analysis, and maintainability cleanups around ExceptionConverter and modern C# patterns.

### Changed

- Package dependencies upgraded:
  - Codebelt.Extensions.Xunit (11.1.1 → 11.1.2),
  - Codebelt.Extensions.Xunit.App (11.1.1 → 11.1.2),
  - Cuemon.AspNetCore.Mvc (10.5.4 → 10.5.5),
  - Cuemon.Core (10.5.4 → 10.5.5),
  - Cuemon.Extensions.AspNetCore (10.5.4 → 10.5.5),
  - Cuemon.Extensions.AspNetCore.Authentication (10.5.4 → 10.5.5),
  - Cuemon.Extensions.AspNetCore.Mvc (10.5.4 → 10.5.5),
  - Cuemon.Extensions.Core (10.5.4 → 10.5.5),
  - Cuemon.Extensions.IO (10.5.4 → 10.5.5),
  - Cuemon.IO (10.5.4 → 10.5.5),
  - Microsoft.NET.Test.Sdk (18.7.0 → 18.8.1),
- Directory.Build.props now scopes multi-targeting, packaging, and signing to source projects, enables recommended analyzers with style enforcement, and sets MinVerTagPrefix,
- ExceptionConverter JSON parse path extracted into focused helpers, removing the legacy GlobalSuppressions file that only covered the old monolithic parser,
- Source and tests modernized to prefer using declarations and ThrowIfNull on supported TFMs, with a direct char compare for trailing-asterisk path matching in JDataResultExtensions,
- XML cref documentation simplified by dropping redundant `T:` prefixes on generic type references,
- DocFX nginx base image pin loosened from `1.31.2-alpine` to `1.31-alpine`.

## [10.1.5] - 2026-06-30

This is a patch release focused on complete DocFX documentation publishing, agent guidance improvements, package version updates, and CI/CD robustness.

### Added

- Type-specific DocFX documentation pages under `.docfx/api/types/` for all public types, providing usage examples and API guidance,
- Comprehensive DocFX documentation maintenance standards in AGENTS.md covering namespace pages, type pages, Extension Members tables, example requirements, TFM selection, and verification workflows.

### Changed

- Namespace documentation pages enhanced with improved descriptions and Extension Members tables clarifying public extension methods and their targets,
- DocFX configuration updated to include type and namespace overwrite files in separate subdirectories as per Keep a Changelog conventions,
- Package dependencies upgraded:
  - Codebelt.Extensions.Xunit (11.1.0 → 11.1.1),
  - Codebelt.Extensions.Xunit.App (11.1.0 → 11.1.1),
  - Cuemon.AspNetCore.Mvc (10.5.3 → 10.5.4),
  - Cuemon.Core (10.5.3 → 10.5.4),
  - Cuemon.Extensions.AspNetCore (10.5.3 → 10.5.4),
  - Cuemon.Extensions.AspNetCore.Authentication (10.5.3 → 10.5.4),
  - Cuemon.Extensions.AspNetCore.Mvc (10.5.3 → 10.5.4),
  - Cuemon.Extensions.Core (10.5.3 → 10.5.4),
  - Cuemon.Extensions.IO (10.5.3 → 10.5.4),
  - Cuemon.IO (10.5.3 → 10.5.4),
  - Microsoft.NET.Test.Sdk (18.6.0 → 18.7.0),
  - Microsoft.AspNetCore.Mvc.NewtonsoftJson (9.0.16 → 9.0.17 for net9, 10.0.8 → 10.0.9 for net10),
- CI/CD deployment condition refined to explicitly validate success of all required jobs, preventing skipped optional jobs from suppressing deployment,
- nginx base image updated to 1.31.2-alpine.

## [10.1.4] - 2026-06-05

This is a patch release focused on expanding test coverage infrastructure, enhancing CI/CD capabilities, and establishing official guidance for AI agent contributions.

### Added

- AGENTS.md as official repository guidance for AI agents working in this codebase, including project overview, coding standards, test conventions, build & CI practices, and git operation safeguards,
- .bot/ directory to gitignore to exclude local AI agent ideation material (with allowance for .bot/README.md),
- Comprehensive unit test suite with new test files to improve code coverage: DynamicContractResolverTest, ExceptionConverterTest, StringFlagsEnumConverterTest, TransientFaultExceptionConverterTest, JDataResultTest, JDataResultExtensionsTest, JsonConverterFactoryTest, JsonSerializerSettingsExtensionsTest, and JsonWriterExtensionsTest.

### Changed

- Copilot instructions expanded with explicit guidelines prohibiting ExcludeFromCodeCoverage attributes across all code paths, emphasizing code refactoring over metrics exclusion,
- CI pipeline enhanced with optional macOS testing matrix (X64 and ARM64 variants) controlled via workflow_dispatch input,
- CI pipeline refactored with new test_qualitygate job to orchestrate test result validation and ensure all required test suites complete successfully before downstream quality and deployment jobs,
- Test coverage expanded with additional test methods across MvcBuilderExtensionsTests, NewtonsoftJsonFormatterTest, ContractResolverExtensionsTest, and ValidatorExtensionsTest,
- Microsoft.NET.Test.SDK upgraded from 18.5.1 to 18.6.0.

### Fixed

- TransientFaultExceptionConverter class to handle null evidence by providing default initialization,
- JDataResultExtensions class to prevent null reference exceptions by validating PropertyName before path comparison,
- Codecov repository reference corrected from 'codebeltnet/newtonsoft' to 'codebeltnet/newtonsoft-json' in CI pipeline.

## [10.1.3] - 2026-05-22

This is a service update that focuses on package dependencies.

## [10.1.2] - 2026-04-17

This is a service update that focuses on package dependencies.

## [10.1.1] - 2026-03-23

This is a patch release focused on dependency updates, build system improvements, and test infrastructure enhancements.

### Changed

- Dependencies upgraded to latest compatible versions: Microsoft.AspNetCore.Mvc.NewtonsoftJson (9.0.14 for net9, 10.0.5 for net10), coverlet.collector (8.0.1), and coverlet.msbuild (8.0.1),
- Build process refactored to use System.IO.File.ReadAllText for improved PackageReleaseNotes handling,
- Service update workflow improved with fixed line-ending handling in PackageReleaseNotes generation,
- Test environment configuration expanded to explicitly support .NET 9 and .NET 10 Docker test runners,
- Bump-nuget script extended with support for Carter package mapping.

## [10.1.0] - 2026-02-28

This is a minor release that improves minimal API formatter integration, while also tightening option registration behavior across ASP.NET Core formatter setup.

### Added

- `ServiceCollectionExtensions` class in the Codebelt.Extensions.AspNetCore.Newtonsoft.Json namespace was extended with a new method: `AddMinimalNewtonsoftJsonOptions`.

### Changed

- `ServiceCollectionExtensions` class in the Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters namespace to use TryConfigure in `AddNewtonsoftJsonFormatterOptions`.

### Fixed

- Prevented repeated `IConfigureOptions<TOptions>` registrations when formatter/options extension methods are called multiple times.

## [10.0.3] - 2026-02-20

This is a service update that focuses on package dependencies.

## [10.0.2] - 2026-02-15

This is a service update that focuses on package dependencies.

## [10.0.1] - 2026-01-22

This is a service update that focuses on package dependencies.

## [10.0.0] - 2025-11-12

This is a major release that focuses on adapting latest `.NET 10` release (LTS) in exchange for current `.NET 8` (LTS).

> To ensure access to current features, improvements, and security updates, and to keep the codebase clean and easy to maintain, we target only the latest long-term (LTS), short-term (STS) and (where applicable) cross-platform .NET versions.

## [9.0.8] - 2025-10-20

This is a service update that focuses on package dependencies.

## [9.0.7] - 2025-09-15

This is a service update that focuses on package dependencies.

## [9.0.6] - 2025-08-19

This is a service update that focuses on package dependencies.

## [9.0.5] - 2025-07-11

This is a service update that focuses on package dependencies.

## [9.0.4] - 2025-06-15

This is a service update that focuses on package dependencies.

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

[10.1.7]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.1.6...v10.1.7
[10.1.6]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.1.5...v10.1.6
[10.1.5]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.1.4...v10.1.5
[10.1.4]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.1.3...v10.1.4
[10.1.3]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.1.2...v10.1.3
[10.1.2]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.1.1...v10.1.2
[10.1.1]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.1.0...v10.1.1
[10.1.0]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.0.3...v10.1.0
[10.0.3]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.0.2...v10.0.3
[10.0.2]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.0.1...v10.0.2
[10.0.1]: https://github.com/codebeltnet/newtonsoft-json/compare/v10.0.0...v10.0.1
[10.0.0]: https://github.com/codebeltnet/newtonsoft-json/compare/v9.0.8...v10.0.0
