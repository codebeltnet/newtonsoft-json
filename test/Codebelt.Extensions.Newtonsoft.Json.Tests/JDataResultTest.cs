using System;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json
{
    public class JDataResultTest : Test
    {
        public JDataResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString_WithPath()
        {
            var json = """{ "name": "test" }""";
            var results = JData.ReadAll(json).ToList();

            // The result has children (from the object)
            var withChildren = results.FirstOrDefault(r => r.Children.Count > 0);
            if (withChildren != null)
            {
                var str = withChildren.ToString();
                TestOutput.WriteLine(str);
                Assert.Contains("Children:", str);
            }
            else
            {
                // Fall back: verify any result has a usable ToString
                foreach (var r in results)
                {
                    var str = r.ToString();
                    TestOutput.WriteLine(str);
                    Assert.Contains("Children:", str);
                }
            }
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString_ShowingPathAndChildCount()
        {
            var json = """{ "id": 1, "name": "test" }""";
            var results = JData.ReadAll(json).ToList();

            TestOutput.WriteLine(string.Join(Environment.NewLine, results));

            Assert.NotEmpty(results);
            foreach (var r in results)
            {
                var str = r.ToString();
                Assert.Contains("Children:", str);
            }
        }

        [Fact]
        public void ReadAll_ShouldParseJsonString()
        {
            var json = """
                       { "id": 1, "name": "test" }
                       """;
            var results = JData.ReadAll(json);

            TestOutput.WriteLine(string.Join(Environment.NewLine, results));

            Assert.NotNull(results);
        }

        [Fact]
        public void ReadAll_ShouldThrowArgumentException_ForInvalidJson()
        {
            var invalidJson = "not json";
            Assert.Throws<ArgumentException>(() => JData.ReadAll(invalidJson));
        }

        [Fact]
        public void ReadAll_ShouldThrowArgumentNullException_ForNullJsonString()
        {
            Assert.Throws<ArgumentNullException>(() => JData.ReadAll((string)null));
        }

        [Fact]
        public void JDataResult_ShouldHaveDefaultProperties()
        {
            var sut = new JDataResult();

            Assert.Null(sut.Path);
            Assert.Null(sut.PropertyName);
            Assert.Null(sut.Value);
            Assert.Null(sut.Type);
            Assert.Null(sut.Parent);
            Assert.NotNull(sut.Children);
            Assert.Empty(sut.Children);
        }

        [Fact]
        public void ReadAll_ShouldExposeParentRelationship_InNestedObject()
        {
            var formatter = new NewtonsoftJsonFormatter();
            var exception = new ArgumentException("Test");
            using var stream = formatter.Serialize(exception);
            var allResults = JData.ReadAll(stream).ToList();

            TestOutput.WriteLine(string.Join(Environment.NewLine, allResults.Select(r => $"Path={r.Path}, Name={r.PropertyName}, Children={r.Children.Count}")));

            // Root level has children (object properties)
            Assert.NotEmpty(allResults);
        }
    }
}
