using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    internal class CustomJsonSerializerSettings : JsonSerializerSettings, IParameterObject
    {
        public CustomJsonSerializerSettings()
        {
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }

    public class JsonSerializerSettingsExtensionsTest : Test
    {
        public JsonSerializerSettingsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Use_ShouldCopyAllSettings_FromSourceToTarget()
        {
            var target = new JsonSerializerSettings();

            target.Use<CustomJsonSerializerSettings>();

            Assert.Equal(Formatting.Indented, target.Formatting);
            Assert.Equal(NullValueHandling.Ignore, target.NullValueHandling);
            Assert.IsType<CamelCasePropertyNamesContractResolver>(target.ContractResolver);
        }

        [Fact]
        public void Use_ShouldCopySettings_WithCustomSetup()
        {
            var target = new JsonSerializerSettings();

            target.Use<CustomJsonSerializerSettings>(setup =>
            {
                setup.Formatting = Formatting.None;
                setup.NullValueHandling = NullValueHandling.Include;
            });

            Assert.Equal(Formatting.None, target.Formatting);
            Assert.Equal(NullValueHandling.Include, target.NullValueHandling);
        }

        [Fact]
        public void Use_ShouldCopyAllDefinedProperties_FromNewtonsoftJsonFormatterOptions()
        {
            var target = new JsonSerializerSettings();
            target.Use<CustomJsonSerializerSettings>();

            // Verify all major setting fields are copied over
            var source = new CustomJsonSerializerSettings();
            Assert.Equal(source.CheckAdditionalContent, target.CheckAdditionalContent);
            Assert.Equal(source.ConstructorHandling, target.ConstructorHandling);
            Assert.Equal(source.DateFormatHandling, target.DateFormatHandling);
            Assert.Equal(source.DateFormatString, target.DateFormatString);
            Assert.Equal(source.DateParseHandling, target.DateParseHandling);
            Assert.Equal(source.DateTimeZoneHandling, target.DateTimeZoneHandling);
            Assert.Equal(source.DefaultValueHandling, target.DefaultValueHandling);
            Assert.Equal(source.FloatFormatHandling, target.FloatFormatHandling);
            Assert.Equal(source.FloatParseHandling, target.FloatParseHandling);
            Assert.Equal(source.Formatting, target.Formatting);
            Assert.Equal(source.MaxDepth, target.MaxDepth);
            Assert.Equal(source.MetadataPropertyHandling, target.MetadataPropertyHandling);
            Assert.Equal(source.MissingMemberHandling, target.MissingMemberHandling);
            Assert.Equal(source.NullValueHandling, target.NullValueHandling);
            Assert.Equal(source.ObjectCreationHandling, target.ObjectCreationHandling);
            Assert.Equal(source.PreserveReferencesHandling, target.PreserveReferencesHandling);
            Assert.Equal(source.ReferenceLoopHandling, target.ReferenceLoopHandling);
            Assert.Equal(source.StringEscapeHandling, target.StringEscapeHandling);
            Assert.Equal(source.TypeNameAssemblyFormatHandling, target.TypeNameAssemblyFormatHandling);
            Assert.Equal(source.TypeNameHandling, target.TypeNameHandling);
        }
    }
}
