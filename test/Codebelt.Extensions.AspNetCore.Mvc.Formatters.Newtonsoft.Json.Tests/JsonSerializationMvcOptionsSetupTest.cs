using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    public class JsonSerializationMvcOptionsSetupTest : Test
    {
        public JsonSerializationMvcOptionsSetupTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldAddInputAndOutputFormatters_ToMvcOptions()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.Configure<NewtonsoftJsonFormatterOptions>(_ => { });
            services.AddSingleton<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>();

            var provider = services.BuildServiceProvider();
            var mvcOptions = new MvcOptions();
            foreach (var configurator in provider.GetServices<IConfigureOptions<MvcOptions>>())
            {
                configurator.Configure(mvcOptions);
            }

            var outputFormatters = mvcOptions.OutputFormatters.OfType<JsonSerializationOutputFormatter>().ToList();
            var inputFormatters = mvcOptions.InputFormatters.OfType<JsonSerializationInputFormatter>().ToList();

            Assert.Single(outputFormatters);
            Assert.Single(inputFormatters);
        }

        [Fact]
        public void Ctor_ShouldInsertFormattersAtPositionZero_InMvcOptions()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.Configure<NewtonsoftJsonFormatterOptions>(_ => { });
            services.AddSingleton<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>();

            var provider = services.BuildServiceProvider();
            var mvcOptions = new MvcOptions();
            foreach (var configurator in provider.GetServices<IConfigureOptions<MvcOptions>>())
            {
                configurator.Configure(mvcOptions);
            }

            Assert.IsType<JsonSerializationOutputFormatter>(mvcOptions.OutputFormatters[0]);
            Assert.IsType<JsonSerializationInputFormatter>(mvcOptions.InputFormatters[0]);
        }
    }
}
