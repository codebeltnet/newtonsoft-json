using System;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Codebelt.Extensions.AspNetCore.Newtonsoft.Json
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddMinimalNewtonsoftJsonOptions_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            IServiceCollection services = null;

            var sut = Assert.Throws<ArgumentNullException>(() => services.AddMinimalNewtonsoftJsonOptions());

            Assert.Equal("services", sut.ParamName);
        }

        [Fact]
        public void AddMinimalNewtonsoftJsonOptions_ShouldReturnSameServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddFaultDescriptorOptions();

            var result = services.AddMinimalNewtonsoftJsonOptions();

            Assert.Same(services, result);
        }

        [Fact]
        public void AddMinimalNewtonsoftJsonOptions_ShouldRegisterNewtonsoftJsonExceptionResponseFormatter()
        {
            var services = new ServiceCollection();
            services.AddFaultDescriptorOptions();
            services.AddMinimalNewtonsoftJsonOptions();

            var sp = services.BuildServiceProvider();
            var formatter = sp.GetService<HttpExceptionDescriptorResponseFormatter<NewtonsoftJsonFormatterOptions>>();

            Assert.NotNull(formatter);
        }

        [Fact]
        public void AddMinimalNewtonsoftJsonOptions_ShouldConfigureOptions_WhenSetupIsProvided()
        {
            var services = new ServiceCollection();
            services.AddFaultDescriptorOptions();
            services.AddMinimalNewtonsoftJsonOptions(o => o.SensitivityDetails = FaultSensitivityDetails.All);

            var sp = services.BuildServiceProvider();
            var options = sp.GetRequiredService<IOptions<NewtonsoftJsonFormatterOptions>>();

            Assert.Equal(FaultSensitivityDetails.All, options.Value.SensitivityDetails);
        }
    }
}
