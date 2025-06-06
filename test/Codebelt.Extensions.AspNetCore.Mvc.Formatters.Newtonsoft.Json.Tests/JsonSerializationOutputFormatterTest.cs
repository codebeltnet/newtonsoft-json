﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    public class JsonSerializationOutputFormatterTest : Test
    {
        public JsonSerializationOutputFormatterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_VerifyThatUtf8AndUtf16_WasAdded_ToSupportedEncodings()
        {
            var sut = new JsonSerializationOutputFormatter(new NewtonsoftJsonFormatterOptions());

            Assert.Equal(2, sut.SupportedEncodings.Count);
            Assert.Collection(sut.SupportedEncodings,
                e => Assert.Equal(Encoding.UTF8, e),
                e => Assert.Equal(Encoding.Unicode, e));
        }

        [Fact]
        public void Ctor_VerifyThatApplicationJsonAndTextJson_WasAdded_ToSupportedMediaTypes()
        {
            var sut = new JsonSerializationOutputFormatter(new NewtonsoftJsonFormatterOptions());

            Assert.Equal(3, sut.SupportedMediaTypes.Count);
            Assert.Collection(sut.SupportedMediaTypes,
                s => Assert.Contains("application/json", s),
                s => Assert.Contains("text/json", s),
                s => Assert.Contains("application/problem+json", s));
        }

        [Fact]
        public async Task WriteResponseBodyAsync_ShouldReturnOk()
        {
            using (var filter = WebHostTestFactory.Create(services =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); })
                    .AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJsonFormatters();
            }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }, hostFixture: null))
            {
                var client = filter.Host.GetTestClient();

                var result = await client.GetAsync("/fake");
                var model = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(model);

                Assert.Contains("\"date\":", model);
                Assert.Contains("\"temperatureC\":", model);
                Assert.Contains("\"temperatureF\":", model);
                Assert.Contains("\"summary\":", model);

                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                Assert.Equal(HttpMethod.Get, result.RequestMessage.Method);
            }
        }
    }
}