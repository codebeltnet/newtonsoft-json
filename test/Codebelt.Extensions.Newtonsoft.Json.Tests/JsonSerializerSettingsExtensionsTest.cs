using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Newtonsoft.Json;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json
{
    public class JsonSerializerSettingsExtensionsTest : Test
    {
        public JsonSerializerSettingsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ApplyToDefaultSettings_ShouldSetJsonConvertDefaultSettings()
        {
            var originalSettings = JsonConvert.DefaultSettings;
            try
            {
                var sut = new NewtonsoftJsonFormatterOptions().Settings;
                sut.Formatting = Formatting.None;
                sut.ApplyToDefaultSettings();

                Assert.NotNull(JsonConvert.DefaultSettings);
                var retrieved = JsonConvert.DefaultSettings();
                Assert.Same(sut, retrieved);
            }
            finally
            {
                JsonConvert.DefaultSettings = originalSettings;
            }
        }

        [Fact]
        public void ApplyToDefaultSettings_ShouldAllowNewtonsoft_ToUseAppliedSettings()
        {
            var originalSettings = JsonConvert.DefaultSettings;
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                };
                settings.ApplyToDefaultSettings();

                var result = JsonConvert.SerializeObject(new { value = 42 });
                TestOutput.WriteLine(result);

                Assert.Contains("\n", result);
            }
            finally
            {
                JsonConvert.DefaultSettings = originalSettings;
            }
        }
    }
}
