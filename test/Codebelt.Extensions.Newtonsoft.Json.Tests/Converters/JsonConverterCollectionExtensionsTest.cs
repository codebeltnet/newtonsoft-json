﻿using System;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Cuemon.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Codebelt.Extensions.Newtonsoft.Json.Converters
{
    public class JsonConverterCollectionExtensionsTest : Test
    {
        public JsonConverterCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddStringEnumConverter_ShouldAddStringEnumConverterToConverterCollection_WithPascalCase()
        {
            var sut1 = DayOfWeek.Saturday;
            var sut2 = new NewtonsoftJsonFormatterOptions();
            sut2.Settings.ContractResolver = new DefaultContractResolver();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringEnumConverter(new DefaultNamingStrategy());

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var result = StreamFactory.Create(writer =>
                {
                    var js = JsonSerializer.Create(sut2.Settings);
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.CloseOutput = false;
                        js.Serialize(jsonWriter, sut1);
                    }
                });

                var json = result.ToEncodedString();

                Assert.True(jc.CanWrite);
                Assert.True(jc.CanRead);
                Assert.True(jc.CanConvert(typeof(DayOfWeek)));
                Assert.Equal("\"Saturday\"", json);

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddStringFlagsEnumConverter_ShouldAddStringFlagsEnumConverterToConverterCollection_WithPascalCase()
        {
            var sut1 = GuidFormats.N | GuidFormats.X;
            var sut2 = new NewtonsoftJsonFormatterOptions();
            sut2.Settings.ContractResolver = new DefaultContractResolver();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringFlagsEnumConverter(new DefaultNamingStrategy());

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var result = StreamFactory.Create(writer =>
                {
                    var js = JsonSerializer.Create(sut2.Settings);
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.CloseOutput = false;
                        js.Serialize(jsonWriter, sut1);
                    }
                });

                var json = result.ToEncodedString();

                Assert.True(jc.CanWrite);
                Assert.True(jc.CanRead);
                Assert.True(jc.CanConvert(typeof(GuidFormats)));
                Assert.Contains("[", json);
                Assert.Contains("\"N\",", json);
                Assert.Contains("\"X\"", json);
                Assert.Contains("]", json);

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddStringEnumConverter_ShouldAddStringEnumConverterToConverterCollection()
        {
            var sut1 = DayOfWeek.Saturday;
            var sut2 = new NewtonsoftJsonFormatterOptions();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringEnumConverter();

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var result = StreamFactory.Create(writer =>
                {
                    var js = JsonSerializer.Create(sut2.Settings);
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.CloseOutput = false;
                        js.Serialize(jsonWriter, sut1);
                    }
                });

                var json = result.ToEncodedString();

                Assert.True(jc.CanWrite);
                Assert.True(jc.CanRead);
                Assert.True(jc.CanConvert(typeof(DayOfWeek)));
                Assert.Equal("\"saturday\"", json);

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddStringFlagsEnumConverter_ShouldAddStringFlagsEnumConverterToConverterCollection()
        {
            var sut1 = GuidFormats.N | GuidFormats.X;
            var sut2 = new NewtonsoftJsonFormatterOptions();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringFlagsEnumConverter();

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var result = StreamFactory.Create(writer =>
                {
                    var js = JsonSerializer.Create(sut2.Settings);
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.CloseOutput = false;
                        js.Serialize(jsonWriter, sut1);
                    }
                });

                var json = result.ToEncodedString();

                Assert.True(jc.CanWrite);
                Assert.True(jc.CanRead);
                Assert.True(jc.CanConvert(typeof(GuidFormats)));
                Assert.Contains("[", json);
                Assert.Contains("\"n\",", json);
                Assert.Contains("\"x\"", json);
                Assert.Contains("]", json);

                TestOutput.WriteLine(json);
            });
        }

        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.None)]
        public void AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection_AndMakeUseOfIncludeOptions(FaultSensitivityDetails sensitivityDetails)
        {
            InsufficientMemoryException ime = null;
            try
            {
                throw new InsufficientMemoryException();
            }
            catch (InsufficientMemoryException e)
            {
                ime = e;
            }

            var sut1 = new ExceptionDescriptor(ime, "NoMemory", "System has exhausted memory.", new Uri("https://docs.microsoft.com/en-us/dotnet/api/system.insufficientmemoryexception"));
            sut1.AddEvidence("CorrelationId", Guid.Empty, correlationId => correlationId.ToString("N"));

            var sut2 = new NewtonsoftJsonFormatterOptions()
            {
                SensitivityDetails = sensitivityDetails
            };

            sut2.Settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(o =>
            {
                o.SensitivityDetails = sensitivityDetails;
            });

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(ExceptionDescriptor))), jc =>
            {
                var result = StreamFactory.Create(writer =>
                {
                    var js = JsonSerializer.Create(sut2.Settings);
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.CloseOutput = false;
                        js.Serialize(jsonWriter, sut1);
                    }
                });

                var json = result.ToEncodedString();

                Assert.True(jc.CanWrite);
                Assert.False(jc.CanRead);
                Assert.True(jc.CanConvert(typeof(ExceptionDescriptor)));
                Assert.Contains("\"error\":", json);
                Assert.Contains("\"code\": \"NoMemory\"", json);
                Assert.Contains("\"message\": \"System has exhausted memory.\"", json);
                Assert.Contains("\"helpLink\": \"https://docs.microsoft.com/en-us/dotnet/api/system.insufficientmemoryexception\"", json);

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Failure), () =>
                {
                    Assert.Contains("\"failure\":", json);
                    Assert.Contains("\"type\": \"System.InsufficientMemoryException\"", json);
                    Assert.Contains("\"source\": \"Codebelt.Extensions.Newtonsoft.Json.Tests\"", json);
                    Assert.Contains("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"failure\":", json);
                    Assert.DoesNotContain("\"type\": \"System.InsufficientMemoryException\"", json);
                    Assert.DoesNotContain("\"source\": \"Codebelt.Extensions.Newtonsoft.Json.Tests\"", json);
                    Assert.DoesNotContain("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
                });

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), () =>
                {
                    Assert.Contains("\"stack\":", json);
                    Assert.Contains("\"at Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensionsTest.AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"stack\":", json);
                    Assert.DoesNotContain("\"at Codebelt.Extensions.Newtonsoft.Json.Converters.JsonConverterCollectionExtensionsTest.AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection", json);
                });

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence), () =>
                {
                    Assert.Contains("\"evidence\":", json);
                    Assert.Contains("\"correlationId\": \"00000000000000000000000000000000\"", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"evidence\":", json);
                    Assert.DoesNotContain("\"correlationId\": \"00000000000000000000000000000000\"", json);
                });

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddDataPairConverter_ShouldAddDataPairConverterToConverterCollection()
        {
            var sut1 = new DataPair<int>("AnswerToEverything", 42);
            var sut2 = new NewtonsoftJsonFormatterOptions();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddDataPairConverter();

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(DataPair))), jc =>
            {
                var result = StreamFactory.Create(writer =>
                {
                    var js = JsonSerializer.Create(sut2.Settings);
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.CloseOutput = false;
                        js.Serialize(jsonWriter, sut1);
                    }
                });

                var json = result.ToEncodedString();

                Assert.True(jc.CanWrite);
                Assert.False(jc.CanRead);
                Assert.True(jc.CanConvert(typeof(DataPair)));
                Assert.Contains("\"name\": \"AnswerToEverything\"", json);
                Assert.Contains("\"value\": 42", json);
                Assert.Contains("\"type\": \"Int32\"", json);

                TestOutput.WriteLine(json);
            });
        }
    }
}