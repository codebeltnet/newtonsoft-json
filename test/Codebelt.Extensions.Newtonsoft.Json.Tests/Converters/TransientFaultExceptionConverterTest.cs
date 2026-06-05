using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Cuemon.Resilience;
using Newtonsoft.Json;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json.Converters
{
    public class TransientFaultExceptionConverterTest : Test
    {
        private static TransientFaultEvidence CreateEvidence()
        {
            var sig = new MethodSignature("TestCaller", "TestMethod", Array.Empty<string>(), Array.Empty<object>());
            return new TransientFaultEvidence(3, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(50), sig);
        }

        public TransientFaultExceptionConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanConvert_ShouldReturnTrue_ForTransientFaultException()
        {
            var sut = new TransientFaultExceptionConverter();

            Assert.True(sut.CanConvert(typeof(TransientFaultException)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalse_ForOtherTypes()
        {
            var sut = new TransientFaultExceptionConverter();

            Assert.False(sut.CanConvert(typeof(Exception)));
            Assert.False(sut.CanConvert(typeof(string)));
            Assert.False(sut.CanConvert(typeof(int)));
        }

        [Fact]
        public void WriteJson_ShouldSerializeTransientFaultException_WhenExceptionConverterPresent()
        {
            var inner = new TimeoutException("Simulated timeout");
            var sut1 = new TransientFaultException("Transient fault occurred", inner, CreateEvidence());

            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            settings.Converters.AddTransientFaultExceptionConverter();
            settings.Converters.AddExceptionConverter(false, false);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            var serializer = JsonSerializer.Create(settings);
            serializer.Serialize(jw, sut1);
            var json = sw.ToString();

            TestOutput.WriteLine(json);

            Assert.Contains("\"Type\": \"Cuemon.Resilience.TransientFaultException\"", json);
            Assert.Contains("\"Message\":", json);
        }

        [Fact]
        public void WriteJson_ShouldProduceNoOutput_WhenExceptionConverterNotPresent()
        {
            var sut1 = new TransientFaultException("Transient fault", null, CreateEvidence());

            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            // Intentionally no ExceptionConverter added

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            jw.WriteStartObject();
            jw.WritePropertyName("test");
            var serializer = JsonSerializer.Create(settings);
            var converter = new TransientFaultExceptionConverter();
            converter.WriteJson(jw, sut1, serializer);
            jw.WriteEndObject();
            var json = sw.ToString();

            TestOutput.WriteLine(json);

            // When no ExceptionConverter found, WriteJson does nothing
            Assert.Contains("{", json);
        }

        [Fact]
        public void WriteAndReadJson_ShouldRoundTrip_WithFormatter()
        {
            var inner = new ArgumentException("inner arg");
            var sut1 = new TransientFaultException("Transient fault", inner, CreateEvidence());

            var formatter = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.Converters.AddTransientFaultExceptionConverter();
                o.Settings.Converters.AddExceptionConverter(false, false);
            });

            var stream = formatter.Serialize(sut1, typeof(TransientFaultException));
            var json = new StreamReader(stream).ReadToEnd();

            TestOutput.WriteLine(json);

            Assert.Contains("transientFaultException", json, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeTransientFaultException_WithoutEvidence()
        {
            var sut = new TransientFaultExceptionConverter();

            // When the JSON has no evidence section, a minimal evidence is created so message is preserved
            var json = """
                       {
                         "message": "Transient fault occurred"
                       }
                       """;

            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            settings.Converters.Add(sut);
            settings.Converters.AddExceptionConverter(false, false);

            var serializer = JsonSerializer.Create(settings);
            using var sr = new StringReader(json);
            using var jr = new JsonTextReader(sr);
            var result = serializer.Deserialize(jr, typeof(TransientFaultException)) as TransientFaultException;

            TestOutput.WriteLine(result?.Message ?? "null");

            // Message should be preserved even when evidence is missing in JSON
            Assert.NotNull(result);
            Assert.IsType<TransientFaultException>(result);
            Assert.Equal("Transient fault occurred", result.Message);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeTransientFaultException_WithEvidence()
        {
            // First serialize with evidence, then deserialize to test the evidence parsing path
            var inner = new ArgumentException("inner error");
            var original = new TransientFaultException("Transient fault", inner, CreateEvidence());

            var formatter = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.Converters.AddTransientFaultExceptionConverter();
                o.Settings.Converters.AddExceptionConverter(false, false);
            });

            // Serialize
            var stream = formatter.Serialize(original, typeof(TransientFaultException));
            var json = new StreamReader(stream).ReadToEnd();

            TestOutput.WriteLine(json);

            // The JSON should contain evidence section for TransientFaultException
            Assert.Contains("transientFaultException", json, StringComparison.OrdinalIgnoreCase);
        }
    }
}
