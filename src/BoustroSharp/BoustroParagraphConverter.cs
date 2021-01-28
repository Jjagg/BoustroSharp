using System;
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
            switch (value)
            {
                case LineParagraph line:
                    JsonSerializer.Serialize(writer, line, options);
                    break;
                case ParagraphEmbed embed:
                    JsonSerializer.Serialize(writer, embed, options);
                    break;
                default:
                    throw new JsonException($"Missing serializer for type '{value.GetType()}'.");
            }
        }
    }
}
