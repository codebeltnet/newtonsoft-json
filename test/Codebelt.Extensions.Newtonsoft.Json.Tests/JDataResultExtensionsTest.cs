using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Xunit;

namespace Codebelt.Extensions.Newtonsoft.Json
{
    public class JDataResultExtensionsTest : Test
    {
        public JDataResultExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Flatten_ShouldThrowArgumentNullException_WhenSourceIsNull()
        {
            IEnumerable<JDataResult> source = null;
            Assert.Throws<ArgumentNullException>(() => source.Flatten().ToList());
        }

        [Fact]
        public void Flatten_ShouldReturnFlatList_FromNestedHierarchy()
        {
            var formatter = new NewtonsoftJsonFormatter();
            var json = """
                       {
                         "outer": {
                           "inner": "value"
                         }
                       }
                       """;
            using var stream = json.ToStream();
            var results = JData.ReadAll(stream).Flatten().ToList();

            TestOutput.WriteLine(string.Join(Environment.NewLine, results));

            Assert.NotEmpty(results);
        }

        [Fact]
        public void ExtractObjectValues_ShouldThrowArgumentNullException_WhenSourceIsNull()
        {
            IEnumerable<JDataResult> source = null;
            Assert.Throws<ArgumentNullException>(() =>
                source.ExtractObjectValues("path", dict => { }));
        }

        [Fact]
        public void ExtractObjectValues_ShouldThrowArgumentException_WhenPropertyNamesIsNullOrWhiteSpace()
        {
            var source = new List<JDataResult>();
            Assert.Throws<ArgumentNullException>(() =>
                source.ExtractObjectValues(null, dict => { }));
            Assert.Throws<ArgumentException>(() =>
                source.ExtractObjectValues("  ", dict => { }));
        }

        [Fact]
        public void ExtractObjectValues_ShouldThrowArgumentNullException_WhenExtractorIsNull()
        {
            var source = new List<JDataResult>();
            Assert.Throws<ArgumentNullException>(() =>
                source.ExtractObjectValues("path", null));
        }

        [Fact]
        public void ExtractObjectValues_ShouldExtractValues_ByPropertyPath()
        {
            var json = """
                       [
                         { "name": "Alice", "age": 30 },
                         { "name": "Bob", "age": 25 }
                       ]
                       """;
            using var stream = json.ToStream();
            var results = JData.ReadAll(stream).ToList();
            var flatResults = results.Flatten().ToList();

            TestOutput.WriteLine(string.Join(Environment.NewLine, flatResults.Select(r => $"Path={r.Path}, Name={r.PropertyName}, Value={r.Value}")));

            // Paths inside array elements have a dot prefix, e.g. ".name"
            var extracted = new List<string>();
            flatResults.ExtractObjectValues(".name", dict =>
            {
                if (dict.TryGetValue("name", out var jr))
                {
                    extracted.Add(jr.Value?.ToString());
                }
            });

            Assert.Equal(2, extracted.Count);
            Assert.Contains("Alice", extracted);
            Assert.Contains("Bob", extracted);
        }

        [Fact]
        public void ExtractObjectValues_ShouldExtractMultipleValues_ByCommaSeparatedPaths()
        {
            var json = """
                       [
                         { "name": "Alice", "age": 30 },
                         { "name": "Bob", "age": 25 }
                       ]
                       """;
            using var stream = json.ToStream();
            var results = JData.ReadAll(stream).Flatten().ToList();

            TestOutput.WriteLine(string.Join(Environment.NewLine, results.Select(r => $"Path={r.Path}, Name={r.PropertyName}, Value={r.Value}")));

            // Paths inside array elements have a dot prefix, e.g. ".name", ".age"
            var pairs = new List<(string name, object age)>();
            results.ExtractObjectValues(".name, .age", dict =>
            {
                var name = dict.TryGetValue("name", out var n) ? n.Value?.ToString() : null;
                var age = dict.TryGetValue("age", out var a) ? a.Value : null;
                pairs.Add((name, age));
            });

            Assert.Equal(2, pairs.Count);
            Assert.Contains(pairs, p => p.name == "Alice");
            Assert.Contains(pairs, p => p.name == "Bob");
        }

        [Fact]
        public void ExtractArrayValues_ShouldThrowArgumentNullException_WhenSourceIsNull()
        {
            IEnumerable<JDataResult> source = null;
            Assert.Throws<ArgumentNullException>(() =>
                source.ExtractArrayValues("path", dict => { }));
        }

        [Fact]
        public void ExtractArrayValues_ShouldThrowArgumentException_WhenPropertyNamesIsNullOrWhiteSpace()
        {
            var source = new List<JDataResult>();
            Assert.Throws<ArgumentNullException>(() =>
                source.ExtractArrayValues(null, dict => { }));
            Assert.Throws<ArgumentException>(() =>
                source.ExtractArrayValues("  ", dict => { }));
        }

        [Fact]
        public void ExtractArrayValues_ShouldThrowArgumentNullException_WhenExtractorIsNull()
        {
            var source = new List<JDataResult>();
            Assert.Throws<ArgumentNullException>(() =>
                source.ExtractArrayValues("path", null));
        }

        [Fact]
        public void ExtractArrayValues_ShouldExtractArrayChildren_ByPropertyPath()
        {
            var json = """
                       {
                         "items": [ "a", "b", "c" ],
                         "other": [ "x", "y" ]
                       }
                       """;
            using var stream = json.ToStream();
            var results = JData.ReadAll(stream).ToList();

            TestOutput.WriteLine(string.Join(Environment.NewLine, results.Select(r => $"Path={r.Path}, Name={r.PropertyName}, Children={r.Children.Count}")));

            var extractedLists = new List<IEnumerable<JDataResult>>();
            results.ExtractArrayValues("items", dict =>
            {
                if (dict.TryGetValue("items", out var jr))
                {
                    extractedLists.Add(jr);
                }
            });

            Assert.Single(extractedLists);
            Assert.Equal(3, extractedLists[0].Count());
        }

        [Fact]
        public void ExtractArrayValues_ShouldExtractArrayWithAsterisk()
        {
            // Using a nested structure where arrays are properties of a parent object
            // After flattening, paths like "group.items1" and "group.items2" are produced
            // "group.*" pattern will match all properties under "group"
            var json = """
                       {
                         "group": {
                           "items1": [ "a", "b" ],
                           "items2": [ "c", "d" ]
                         }
                       }
                       """;
            using var stream = json.ToStream();
            var allResults = JData.ReadAll(stream).ToList();
            var flatResults = allResults.Flatten().ToList();

            TestOutput.WriteLine(string.Join(Environment.NewLine, flatResults.Select(r => $"Path={r.Path}, Name={r.PropertyName}, Children={r.Children.Count}")));

            var extractedGroups = new List<IEnumerable<JDataResult>>();
            flatResults.ExtractArrayValues("group.*", dict =>
            {
                foreach (var kv in dict)
                {
                    extractedGroups.Add(kv.Value);
                }
            });

            Assert.Equal(2, extractedGroups.Count);
        }
    }
}
