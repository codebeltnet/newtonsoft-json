using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json.Converters
{
    public class ExceptionConverterTest : Test
    {
        public ExceptionConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldSetDefaultValues()
        {
            var sut = new ExceptionConverter();

            Assert.False(sut.IncludeStackTrace);
            Assert.False(sut.IncludeData);
        }

        [Fact]
        public void Ctor_ShouldSetSpecifiedValues()
        {
            var sut = new ExceptionConverter(includeStackTrace: true, includeData: true);

            Assert.True(sut.IncludeStackTrace);
            Assert.True(sut.IncludeData);
        }

        [Fact]
        public void CanConvert_ShouldReturnTrue_ForExceptionTypes()
        {
            var sut = new ExceptionConverter();

            Assert.True(sut.CanConvert(typeof(Exception)));
            Assert.True(sut.CanConvert(typeof(ArgumentException)));
            Assert.True(sut.CanConvert(typeof(InvalidOperationException)));
            Assert.True(sut.CanConvert(typeof(OutOfMemoryException)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalse_ForNonExceptionTypes()
        {
            var sut = new ExceptionConverter();

            Assert.False(sut.CanConvert(typeof(string)));
            Assert.False(sut.CanConvert(typeof(int)));
            Assert.False(sut.CanConvert(typeof(object)));
        }

        [Fact]
        public void WriteJson_ShouldSerializeException_WithoutStackTraceAndData()
        {
            var exception = new ArgumentException("Test message", "paramName");
            var sut = new ExceptionConverter(includeStackTrace: false, includeData: false);
            var json = SerializeException(sut, exception);

            TestOutput.WriteLine(json);

            Assert.Contains("\"Type\": \"System.ArgumentException\"", json);
            Assert.Contains("\"Message\":", json);
            Assert.Contains("Test message", json);
            Assert.DoesNotContain("\"Stack\":", json);
            Assert.DoesNotContain("\"Data\":", json);
        }

        [Fact]
        public void WriteJson_ShouldSerializeException_WithStackTrace()
        {
            Exception exception = null;
            try
            {
                throw new ArgumentException("Test message", "paramName");
            }
            catch (Exception e)
            {
                exception = e;
            }

            var sut = new ExceptionConverter(includeStackTrace: true, includeData: false);
            var json = SerializeException(sut, exception);

            TestOutput.WriteLine(json);

            Assert.Contains("\"Type\": \"System.ArgumentException\"", json);
            Assert.Contains("\"Stack\":", json);
            Assert.DoesNotContain("\"Data\":", json);
        }

        [Fact]
        public void WriteJson_ShouldSerializeException_WithData()
        {
            var exception = new ArgumentException("Test message");
            exception.Data["key1"] = "value1";

            var sut = new ExceptionConverter(includeStackTrace: false, includeData: true);
            var json = SerializeException(sut, exception);

            TestOutput.WriteLine(json);

            Assert.Contains("\"Type\": \"System.ArgumentException\"", json);
            Assert.DoesNotContain("\"Stack\":", json);
            Assert.Contains("\"Data\":", json);
            Assert.Contains("\"key1\": \"value1\"", json);
        }

        [Fact]
        public void WriteJson_ShouldSerializeException_WithInnerException()
        {
            var inner = new InvalidOperationException("inner message");
            var exception = new ArgumentException("outer message", inner);

            var sut = new ExceptionConverter(includeStackTrace: false, includeData: false);
            var json = SerializeException(sut, exception);

            TestOutput.WriteLine(json);

            Assert.Contains("\"Type\": \"System.ArgumentException\"", json);
            Assert.Contains("\"Inner\":", json);
            Assert.Contains("\"Type\": \"System.InvalidOperationException\"", json);
            Assert.Contains("\"Message\": \"inner message\"", json);
        }

        [Fact]
        public void WriteJson_ShouldSerializeAggregateException_WithMultipleInnerExceptions()
        {
            var agg = new AggregateException(
                new InvalidOperationException("e1"),
                new ArgumentNullException("e2"));

            var sut = new ExceptionConverter(includeStackTrace: false, includeData: false);
            var json = SerializeException(sut, agg);

            TestOutput.WriteLine(json);

            Assert.Contains("\"Type\": \"System.AggregateException\"", json);
            Assert.Contains("\"Inner\":", json);
            Assert.Contains("\"System.InvalidOperationException\"", json);
            Assert.Contains("\"System.ArgumentNullException\"", json);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeException_RoundTrip()
        {
            var original = new ArgumentException("Round-trip message");
            var sut = new ExceptionConverter();

            var json = SerializeException(sut, original);
            TestOutput.WriteLine(json);

            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            settings.Converters.Add(sut);
            var serializer = JsonSerializer.Create(settings);

            using var sr = new StringReader(json);
            using var jr = new JsonTextReader(sr);
            var deserialized = serializer.Deserialize(jr, typeof(ArgumentException)) as Exception;

            Assert.NotNull(deserialized);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeException_WithInnerException()
        {
            var inner = new InvalidOperationException("inner");
            var original = new ArgumentException("outer", inner);
            var sut = new ExceptionConverter();

            var json = SerializeException(sut, original);
            TestOutput.WriteLine(json);

            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            settings.Converters.Add(sut);
            var serializer = JsonSerializer.Create(settings);

            using var sr = new StringReader(json);
            using var jr = new JsonTextReader(sr);
            var deserialized = serializer.Deserialize(jr, typeof(ArgumentException)) as Exception;

            Assert.NotNull(deserialized);
        }

        [Fact]
        public void WriteJson_ShouldSerializeException_ViaFormatter_WithCamelCase()
        {
            var exception = new ArgumentException("Test message");
            var formatter = new NewtonsoftJsonFormatter();
            var json = formatter.Serialize(exception).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.Contains("\"type\": \"System.ArgumentException\"", json);
            Assert.Contains("\"message\":", json);
        }

        private string SerializeException(ExceptionConverter converter, Exception exception)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
            };
            settings.Converters.Add(converter);
            var serializer = JsonSerializer.Create(settings);
            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            serializer.Serialize(jw, exception);
            return sw.ToString();
        }
    }
}
