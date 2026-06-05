using System;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    public class MvcCoreBuilderExtensionsTest : Test
    {
        public MvcCoreBuilderExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddNewtonsoftJsonFormatters_ShouldThrowArgumentNullException_WhenBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MvcCoreBuilderExtensions.AddNewtonsoftJsonFormatters(null));
        }

        [Fact]
        public void AddNewtonsoftJsonFormattersOptions_ShouldThrowArgumentNullException_WhenBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MvcCoreBuilderExtensions.AddNewtonsoftJsonFormattersOptions(null));
        }

        [Fact]
        public void AddNewtonsoftJsonFormatters_ShouldRegisterFormatters_ViaMvcCoreBuilder()
        {
            using var host = WebHostTestFactory.Create(services =>
            {
                services.AddMvcCore()
                    .AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJsonFormatters();
            }, app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => routes.MapControllers());
            }, hostFixture: null);

            Assert.NotNull(host);
        }

        [Fact]
        public void AddNewtonsoftJsonFormatters_ShouldRegisterFormatters_WithSetup_ViaMvcCoreBuilder()
        {
            using var host = WebHostTestFactory.Create(services =>
            {
                services.AddMvcCore()
                    .AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJsonFormatters(o => o.SensitivityDetails = FaultSensitivityDetails.None);
            }, app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => routes.MapControllers());
            }, hostFixture: null);

            Assert.NotNull(host);
        }

        [Fact]
        public void AddNewtonsoftJsonFormattersOptions_ShouldRegisterOptions_ViaMvcCoreBuilder()
        {
            using var host = WebHostTestFactory.Create(services =>
            {
                services.AddMvcCore()
                    .AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJsonFormattersOptions(o => o.SensitivityDetails = FaultSensitivityDetails.None);
            }, app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => routes.MapControllers());
            }, hostFixture: null);

            Assert.NotNull(host);
        }
    }
}
