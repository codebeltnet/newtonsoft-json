using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json.Converters
{
    [Flags]
    internal enum TestFlagsEnum
    {
        None = 0,
        Option1 = 1,
        Option2 = 2,
        Option3 = 4
    }

    internal enum TestNonFlagsEnum
    {
        Value1 = 1,
        Value2 = 2,
        Value3 = 3
    }

    public class StringFlagsEnumConverterTest : Test
    {
        public StringFlagsEnumConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanConvert_ShouldReturnTrue_ForFlagsEnum()
        {
            var sut = new StringFlagsEnumConverter();

            Assert.True(sut.CanConvert(typeof(TestFlagsEnum)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalse_ForNonFlagsEnum()
        {
            var sut = new StringFlagsEnumConverter();

            Assert.False(sut.CanConvert(typeof(TestNonFlagsEnum)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalse_ForNonEnumType()
        {
            var sut = new StringFlagsEnumConverter();

            Assert.False(sut.CanConvert(typeof(string)));
            Assert.False(sut.CanConvert(typeof(int)));
        }

        [Fact]
        public void WriteJson_ShouldWriteArray_ForFlagsEnum()
        {
            var value = TestFlagsEnum.Option1 | TestFlagsEnum.Option2;
            var sut = new StringFlagsEnumConverter();

            var json = Serialize(sut, value);

            TestOutput.WriteLine(json);

            Assert.Contains("[", json);
            Assert.Contains("option1", json);
            Assert.Contains("option2", json);
            Assert.Contains("]", json);
        }

        [Fact]
        public void WriteJson_ShouldWriteArray_ForFlagsEnum_WithNamingStrategy()
        {
            var value = TestFlagsEnum.Option1 | TestFlagsEnum.Option2;
            var sut = new StringFlagsEnumConverter(new DefaultNamingStrategy());

            var json = Serialize(sut, value);

            TestOutput.WriteLine(json);

            Assert.Contains("[", json);
            Assert.Contains("Option1", json);
            Assert.Contains("Option2", json);
            Assert.Contains("]", json);
        }

        [Fact]
        public void WriteJson_ShouldWriteSingleValue_ForNonFlagsEnum_WhenCalledDirectly()
        {
            var value = TestNonFlagsEnum.Value1;
            var sut = new StringFlagsEnumConverter();

            // The CanConvert returns false for non-flags enums, but WriteJson has this code path.
            // Call WriteJson directly to exercise the non-flags branch.
            var settings = new JsonSerializerSettings();
            var serializer = JsonSerializer.Create(settings);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            sut.WriteJson(jw, value, serializer);
            var json = sw.ToString();

            TestOutput.WriteLine(json);

            // Non-flags enum values are serialized as camelCase string names
            Assert.Equal("\"value1\"", json);
        }

        [Fact]
        public void WriteJson_ShouldWriteNull_WhenValueIsNull()
        {
            var sut = new StringFlagsEnumConverter();

            // Call WriteJson directly since CanConvert blocks normal serialization path for null
            var settings = new JsonSerializerSettings();
            var serializer = JsonSerializer.Create(settings);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            sut.WriteJson(jw, null, serializer);
            var json = sw.ToString();

            TestOutput.WriteLine(json);

            Assert.Equal("null", json);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeFlagsArray()
        {
            var sut = new StringFlagsEnumConverter();
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(sut);
            var serializer = JsonSerializer.Create(settings);

            var json = "[\"Option1\",\"Option2\"]";
            using var sr = new StringReader(json);
            using var jr = new JsonTextReader(sr);

            var result = serializer.Deserialize(jr, typeof(TestFlagsEnum));

            TestOutput.WriteLine($"Deserialized: {result}");

            var intResult = (int)result;
            Assert.Equal((int)(TestFlagsEnum.Option1 | TestFlagsEnum.Option2), intResult);
        }

        [Fact]
        public void WriteJson_ShouldWriteNumericValue_ForUndefinedEnumValue_WhenCalledDirectly()
        {
            var sut = new StringFlagsEnumConverter();
            var settings = new JsonSerializerSettings();
            var serializer = JsonSerializer.Create(settings);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);

            // An undefined enum value (e.g., 99) is represented as its number string
            var undefinedValue = (TestNonFlagsEnum)99;
            sut.WriteJson(jw, undefinedValue, serializer);
            var json = sw.ToString();

            TestOutput.WriteLine(json);

            // Undefined numeric enum values should be serialized as their numeric representation
            Assert.Equal("99", json);
        }

        private static string Serialize(StringFlagsEnumConverter converter, object value)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(converter);
            var serializer = JsonSerializer.Create(settings);
            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);
            serializer.Serialize(jw, value);
            return sw.ToString();
        }
    }
}