using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Xunit;

namespace BoustroSharp.Test
{
    public class BoustroSharpTest
    {
        private readonly JsonSerializerOptions options = new()
        {
            AllowTrailingCommas = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) },
            IgnoreNullValues = true,
            IgnoreReadOnlyProperties = false,
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        };

        [Fact]
        public void SimpleRoundtrip()
        {
            var paragraph = new LineParagraph("Hello, World!", new List<AttributeSpan>(), new List<LineModifier>());
            var json = JsonSerializer.Serialize(paragraph, options);
            Assert.Equal("{\"type\":\"text\",\"text\":\"Hello, World!\"}", json);

            var reconstructed = JsonSerializer.Deserialize<BoustroParagraph>(json, options);
            Assert.IsType<LineParagraph>(reconstructed);
            var reconstructedLine = reconstructed as LineParagraph;
            Assert.Equal("Hello, World!", reconstructedLine.Text);
            Assert.Null(reconstructedLine.Modifiers);
            Assert.Null(reconstructedLine.Spans);
        }

        [Fact]
        public void SpansRoundtrip()
        {
            var testSpan = new AttributeSpan("test", null, 3, 7);
            var paragraph = new LineParagraph(
                "Hello, World!",
                new List<AttributeSpan> { testSpan },
                new List<LineModifier>()
            );
            var json = JsonSerializer.Serialize(paragraph, options);
            Assert.Equal("{\"type\":\"text\",\"text\":\"Hello, World!\",\"spans\":[{\"type\":\"test\",\"start\":3,\"end\":7}]}", json);

            var reconstructed = JsonSerializer.Deserialize<BoustroParagraph>(json, options);
            Assert.IsType<LineParagraph>(reconstructed);
            var reconstructedLine = reconstructed as LineParagraph;
            Assert.Equal("Hello, World!", reconstructedLine.Text);
            Assert.Null(reconstructedLine.Modifiers);
            Assert.Equal(new List<AttributeSpan> { testSpan }, reconstructedLine.Spans);
        }

        [Fact]
        public void LinemodRoundtrip()
        {
            var testMod = new LineModifier("test", null);
            var paragraph = new LineParagraph(
                "Hello, World!",
                new List<AttributeSpan>(),
                new List<LineModifier> { testMod }
            );
            var json = JsonSerializer.Serialize(paragraph, options);
            Assert.Equal("{\"type\":\"text\",\"text\":\"Hello, World!\",\"modifiers\":[{\"type\":\"test\"}]}", json);

            var reconstructed = JsonSerializer.Deserialize<BoustroParagraph>(json, options);
            Assert.IsType<LineParagraph>(reconstructed);
            var reconstructedLine = reconstructed as LineParagraph;
            Assert.Equal("Hello, World!", reconstructedLine.Text);
            Assert.Equal(new List<LineModifier> { testMod }, reconstructedLine.Modifiers);
            Assert.Null(reconstructedLine.Spans);
        }

        [Fact]
        public void EmbedRoundtrip()
        {
            var testEmbed = new ParagraphEmbed("test", null);
            var paragraph = testEmbed;
            var json = JsonSerializer.Serialize(paragraph, options);
            Assert.Equal("{\"type\":\"test\"}", json);

            var reconstructed = JsonSerializer.Deserialize<BoustroParagraph>(json, options);
            Assert.IsType<ParagraphEmbed>(reconstructed);
            var reconstructedLine = reconstructed as ParagraphEmbed;
            Assert.Null(reconstructedLine.Value);
        }
    }
}
