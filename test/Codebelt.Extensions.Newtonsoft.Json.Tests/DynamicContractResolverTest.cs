using System;
using System.Reflection;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json
{
    public class DynamicContractResolverTest : Test
    {
        public DynamicContractResolverTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Create_ShouldReturnCamelCaseResolver_ForCamelCasePropertyNamesContractResolver()
        {
            var sut = DynamicContractResolver.Create<CamelCasePropertyNamesContractResolver>();

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<CamelCasePropertyNamesContractResolver>(sut);
        }

        [Fact]
        public void Create_ShouldReturnDefaultContractResolver_ForDefaultContractResolver()
        {
            var sut = DynamicContractResolver.Create<DefaultContractResolver>();

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<DefaultContractResolver>(sut);
        }

        [Fact]
        public void Create_WithHandlers_ShouldApplyHandlersForCamelCaseResolver()
        {
            var handlerInvoked = false;
            var sut = DynamicContractResolver.Create<CamelCasePropertyNamesContractResolver>(
                (pi, jp) => { handlerInvoked = true; });

            Assert.NotNull(sut);

            // Trigger CreateProperty by serializing an object
            var formatter = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.ContractResolver = sut;
            });
            formatter.Serialize(new { Name = "test", Value = 42 }).ToEncodedString();

            Assert.True(handlerInvoked);
        }

        [Fact]
        public void Create_WithHandlers_ShouldApplyHandlersForDefaultContractResolver()
        {
            var handlerInvoked = false;
            var sut = DynamicContractResolver.Create<DefaultContractResolver>(
                (pi, jp) => { handlerInvoked = true; });

            Assert.NotNull(sut);

            // Trigger CreateProperty by serializing an object
            var formatter = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.ContractResolver = sut;
            });
            formatter.Serialize(new { Name = "test", Value = 42 }).ToEncodedString();

            Assert.True(handlerInvoked);
        }

        [Fact]
        public void Create_ShouldApplyCamelCaseNaming_WhenCamelCaseResolverUsed()
        {
            var sut = DynamicContractResolver.Create<CamelCasePropertyNamesContractResolver>();
            var formatter = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.ContractResolver = sut;
            });

            var json = formatter.Serialize(new SampleDto { FirstName = "John", LastName = "Doe" }).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.Contains("\"firstName\":", json);
            Assert.Contains("\"lastName\":", json);
        }

        [Fact]
        public void Create_ShouldApplyPascalCaseNaming_WhenDefaultContractResolverUsed()
        {
            var sut = DynamicContractResolver.Create<DefaultContractResolver>();
            var formatter = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.ContractResolver = sut;
            });

            var json = formatter.Serialize(new SampleDto { FirstName = "John", LastName = "Doe" }).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.Contains("\"FirstName\":", json);
            Assert.Contains("\"LastName\":", json);
        }

        private class SampleDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
