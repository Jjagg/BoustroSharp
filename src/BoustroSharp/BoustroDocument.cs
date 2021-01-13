using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoustroSharp
{
    public class BoustroParagraphConverter : JsonConverter<BoustroParagraph>
    {
        private static readonly byte[] TypeKey = Encoding.UTF8.GetBytes("type");

        public override BoustroParagraph? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var typeReader = reader;
            if (typeReader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object.");

            while (typeReader.Read())
            {
                if (typeReader.TokenType == JsonTokenType.EndObject)
                {
                    throw new JsonException("Missing type property.");
                }

                if (typeReader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected property name.");
                }

                if (typeReader.ValueTextEquals(TypeKey))
                {
                    if (!typeReader.Read()) throw new JsonException();
                    var value = typeReader.GetString();

                    if (value is null)
                    {
                        throw new JsonException("Missing type value.");
                    }

                    if (value == "text")
                    {
                        return JsonSerializer.Deserialize<LineParagraph>(ref reader, options);
                    }
                    else
                    {
                        return JsonSerializer.Deserialize<ParagraphEmbed>(ref reader, options);
                    }
                }

                typeReader.Skip();
            }

            throw new JsonException("Invalid JSON.");
        }

        public override void Write(Utf8JsonWriter writer, BoustroParagraph value, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Subclasses should be directly serialized.");
        }
    }

    public class BoustroDocument
    {
        public List<BoustroParagraph> Paragraphs { get; set; }

        public BoustroDocument(IEnumerable<BoustroParagraph> paragraphs)
        {
            if (paragraphs is null)
            {
                throw new ArgumentNullException(nameof(paragraphs));
            }

            Paragraphs = new List<BoustroParagraph>(paragraphs);
        }

        public override bool Equals(object? obj)
        {
            return obj is BoustroDocument document && Paragraphs.SequenceEqual(document.Paragraphs);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Paragraphs);
        }
    }

    [JsonConverter(typeof(BoustroParagraphConverter))]
    public abstract class BoustroParagraph
    {
    }

    public class LineParagraph : BoustroParagraph
    {
        [JsonInclude]
        public string Type => "text";
        public string Text { get; set; }
        public List<AttributeSpan>? Spans { get; set; }
        public List<LineModifier>? Modifiers { get; set; }

        public LineParagraph(string text, List<AttributeSpan> spans, List<LineModifier> modifiers)
        {
            Text = text;
            Spans = spans is not null && spans.Any() ? spans : null;
            Modifiers = modifiers is not null && modifiers.Any() ? modifiers : null;
        }

        public override bool Equals(object? obj)
        {
            return obj is LineParagraph paragraph &&
                   Type == paragraph.Type &&
                   Text == paragraph.Text &&
                   EqualityComparer<List<AttributeSpan>?>.Default.Equals(Spans, paragraph.Spans) &&
                   EqualityComparer<List<LineModifier>?>.Default.Equals(Modifiers, paragraph.Modifiers);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Text, Spans, Modifiers);
        }
    }

    public class ParagraphEmbed : BoustroParagraph
    {
        public string Type { get; set; }
        public JsonElement? Value { get; set; }

        public ParagraphEmbed(string type, JsonElement? value = null)
        {
            Type = type;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is ParagraphEmbed embed &&
                   Type == embed.Type &&
                   EqualityComparer<JsonElement?>.Default.Equals(Value, embed.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }
    }

    public class LineModifier
    {
        public string Type { get; set; }
        public JsonElement? Value { get; set; }

        public LineModifier(string type, JsonElement? value = null)
        {
            Type = type;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is LineModifier modifier &&
                   Type == modifier.Type &&
                   EqualityComparer<JsonElement?>.Default.Equals(Value, modifier.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }
    }

    public struct AttributeSpan
    {
        public string Type { get; set; }
        public JsonElement? Value { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public AttributeSpan(string type, JsonElement? value, int start, int end)
        {
            Type = type;
            Value = value;
            Start = start;
            End = end;
        }

        public override bool Equals(object? obj)
        {
            return obj is AttributeSpan span &&
                   Type == span.Type &&
                   EqualityComparer<JsonElement?>.Default.Equals(Value, span.Value) &&
                   Start == span.Start &&
                   End == span.End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value, Start, End);
        }
    }
}
