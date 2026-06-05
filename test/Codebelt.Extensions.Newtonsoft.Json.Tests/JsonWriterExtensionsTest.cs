using System.IO;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json
{
    public class JsonWriterExtensionsTest : Test
    {
        public JsonWriterExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WritePropertyName_ShouldWritePropertyName_WithNullSerializer()
        {
            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);

            jw.WriteStartObject();
            jw.WritePropertyName("MyProperty", (JsonSerializer)null);
            jw.WriteValue("value");
            jw.WriteEndObject();

            var json = sw.ToString();
            TestOutput.WriteLine(json);

            Assert.Contains("\"MyProperty\"", json);
            Assert.Contains("\"value\"", json);
        }

        [Fact]
        public void WritePropertyName_ShouldApplyCamelCase_WithCamelCaseContractResolver()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var serializer = JsonSerializer.Create(settings);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);

            jw.WriteStartObject();
            jw.WritePropertyName("MyProperty", serializer);
            jw.WriteValue("value");
            jw.WriteEndObject();

            var json = sw.ToString();
            TestOutput.WriteLine(json);

            Assert.Contains("\"myProperty\"", json);
        }

        [Fact]
        public void WritePropertyName_ShouldPreservePascalCase_WithDefaultContractResolver()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            };
            var serializer = JsonSerializer.Create(settings);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);

            jw.WriteStartObject();
            jw.WritePropertyName("MyProperty", serializer);
            jw.WriteValue("value");
            jw.WriteEndObject();

            var json = sw.ToString();
            TestOutput.WriteLine(json);

            Assert.Contains("\"MyProperty\"", json);
        }

        [Fact]
        public void WriteObject_ShouldSerializeObject_ToJsonWriter()
        {
            var settings = new JsonSerializerSettings();
            var serializer = JsonSerializer.Create(settings);

            using var sw = new StringWriter();
            using var jw = new JsonTextWriter(sw);

            jw.WriteStartObject();
            jw.WritePropertyName("nested");
            jw.WriteObject(new { x = 1, y = 2 }, serializer);
            jw.WriteEndObject();

            var json = sw.ToString();
            TestOutput.WriteLine(json);

            Assert.Contains("\"nested\"", json);
            Assert.Contains("\"x\"", json);
            Assert.Contains("1", json);
        }
    }
}
