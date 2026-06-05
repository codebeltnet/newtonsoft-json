using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json
{
    public class JsonConverterFactoryTest : Test
    {
        public JsonConverterFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Create_WithTypeAndWriter_ShouldProduceConverter_WithCanWriteTrue()
        {
            var converter = JsonConverterFactory.Create(
                typeof(DateTime),
                (writer, value, serializer) => writer.WriteValue(((DateTime)value).ToString("O")));

            Assert.True(converter.CanWrite);
            Assert.False(converter.CanRead);
            Assert.True(converter.CanConvert(typeof(DateTime)));
            Assert.False(converter.CanConvert(typeof(string)));
        }

        [Fact]
        public void Create_WithTypeAndReader_ShouldProduceConverter_WithCanReadTrue()
        {
            var converter = JsonConverterFactory.Create(
                typeof(DateTime),
                writer: null,
                reader: (reader, type, existing, serializer) => DateTime.Parse(reader.Value.ToString()));

            Assert.False(converter.CanWrite);
            Assert.True(converter.CanRead);
            Assert.True(converter.CanConvert(typeof(DateTime)));
        }

        [Fact]
        public void Create_WithPredicateAndWriter_ShouldProduceConverter()
        {
            var converter = JsonConverterFactory.Create(
                type => type == typeof(Guid),
                (writer, value, serializer) => writer.WriteValue(value.ToString()),
                reader: null);

            Assert.True(converter.CanWrite);
            Assert.False(converter.CanRead);
            Assert.True(converter.CanConvert(typeof(Guid)));
            Assert.False(converter.CanConvert(typeof(string)));
        }

        [Fact]
        public void Create_WithPredicateAndReader_ShouldProduceConverter()
        {
            var converter = JsonConverterFactory.Create(
                type => type == typeof(Guid),
                writer: null,
                reader: (reader, type, existing, serializer) => Guid.Parse(reader.Value.ToString()));

            Assert.False(converter.CanWrite);
            Assert.True(converter.CanRead);
            Assert.True(converter.CanConvert(typeof(Guid)));
        }

        [Fact]
        public void Create_WithNullWriter_ShouldThrowNotImplementedException_OnWriteJson()
        {
            var converter = JsonConverterFactory.Create<int>(writer: null);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            var serializer = JsonSerializer.Create();

            Assert.Throws<NotImplementedException>(() => converter.WriteJson(jw, 42, serializer));
        }

        [Fact]
        public void Create_WithNullReader_ShouldThrowNotImplementedException_OnReadJson()
        {
            var converter = JsonConverterFactory.Create<int>(reader: null);

            using var sr = new StringReader("42");
            using var jr = new JsonTextReader(sr);
            var serializer = JsonSerializer.Create();

            Assert.Throws<NotImplementedException>(() => converter.ReadJson(jr, typeof(int), null, serializer));
        }

        [Fact]
        public void Create_WithExistingConverter_ShouldWrapConverter()
        {
            var original = new global::Newtonsoft.Json.Converters.StringEnumConverter();
            var wrapped = JsonConverterFactory.Create(original);

            Assert.True(wrapped.CanWrite);
            Assert.True(wrapped.CanRead);
            Assert.True(wrapped.CanConvert(typeof(DayOfWeek)));
        }

        [Fact]
        public void Create_GenericWithWriter_ShouldSerializeType()
        {
            var converter = JsonConverterFactory.Create<TimeSpan>((writer, value, serializer) =>
            {
                writer.WriteValue(value.TotalSeconds);
            });

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(converter);

            var ts = TimeSpan.FromSeconds(90);
            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            var serializer = JsonSerializer.Create(settings);
            serializer.Serialize(jw, ts);
            var json = sw.ToString();

            TestOutput.WriteLine(json);

            Assert.Equal("90.0", json);
        }

        [Fact]
        public void Create_GenericWithPredicate_ShouldOnlyConvertMatchingType()
        {
            var converter = JsonConverterFactory.Create<string>(
                predicate: type => type == typeof(string),
                writer: (writer, value, serializer) => writer.WriteValue(value.ToUpperInvariant()));

            Assert.True(converter.CanConvert(typeof(string)));
            Assert.False(converter.CanConvert(typeof(int)));
        }
    }
}
